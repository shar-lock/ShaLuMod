// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.SentryService
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.AutoSlay;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public static class SentryService
{
  private const string _dsnSettingPath = "sentry/config/dsn";
  private static readonly StringName _sentrySdkSingleton = new StringName("SentrySDK");
  private static readonly StringName _sentryUserClass = new StringName("SentryUser");
  private static readonly StringName _sentryBreadcrumbClass = new StringName("SentryBreadcrumb");
  private static readonly StringName _levelProperty = new StringName("level");
  private static readonly StringName _categoryProperty = new StringName("category");
  private static readonly StringName _idProperty = new StringName("id");
  private static readonly StringName _createMethod = new StringName("create");
  private static readonly StringName _setUserMethod = new StringName("set_user");
  private static readonly StringName _addBreadcrumbMethod = new StringName("add_breadcrumb");
  private static readonly StringName _shutdownMethod = new StringName("shutdown");
  private static readonly StringName _setShouldSampleEventMethod = new StringName("set_should_sample_event");
  private static readonly StringName _setPlatformBranchMethod = new StringName("set_platform_branch");
  private static readonly StringName _setCsharpContextMethod = new StringName("set_csharp_context");
  private static IDisposable? _sentryInstance;
  private static float _sampleRate = 1f;
  private static bool _isGameInitialized;
  private static volatile bool _suppressAllEvents;
  private static readonly string _sessionId = Guid.NewGuid().ToString();
  private static Node? _sentryInit;
  private static GodotObject? _extensionSdk;

  public static bool IsEnabled { get; private set; }

  public static bool SampleForNonSteamBranches { get; private set; }

  public static bool IsForcedOn { get; private set; }

  public static string SessionId => SentryService._sessionId;

  public static void DisableSentryIfModded()
  {
    if (!ModManager.IsRunningModded())
      return;
    SentryService._suppressAllEvents = true;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ((GodotObject) (Engine.GetMainLoop() is SceneTree mainLoop ? ((Node) mainLoop.Root).GetNodeOrNull(NodePath.op_Implicit("SentryInit")) : (Node) null))?.Call(SentryService._setShouldSampleEventMethod, new Variant[1]
    {
      Variant.op_Implicit(Callable.From<bool>(SentryService.\u003C\u003EO.\u003C0\u003E__AlwaysRejectEvent ?? (SentryService.\u003C\u003EO.\u003C0\u003E__AlwaysRejectEvent = new Func<bool>(SentryService.AlwaysRejectEvent))))
    });
  }

  private static bool AlwaysRejectEvent() => false;

  public static void Initialize()
  {
    bool flag1 = OS.HasFeature("editor");
    bool flag2 = DisplayServer.GetName().Equals("headless", StringComparison.OrdinalIgnoreCase);
    bool isForcedOn = CommandLineHelper.HasArg("force-sentry");
    if (flag1 && !flag2 && !isForcedOn)
    {
      Log.Info("[Sentry.NET] Disabled in editor");
    }
    else
    {
      SentryService.SampleForNonSteamBranches = flag2 | isForcedOn;
      SentryService.IsForcedOn = isForcedOn;
      string dsn = SentryService.GetDsn();
      if (string.IsNullOrEmpty(dsn))
      {
        Log.Info("[Sentry.NET] Disabled: no DSN configured in project settings");
      }
      else
      {
        ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
        string environment = "unknown";
        string release = releaseInfo?.Version ?? "dev";
        SentryService._sentryInstance = SentrySdk.Init((Action<SentryOptions>) (options =>
        {
          options.Dsn = dsn;
          options.Environment = environment;
          options.Release = release;
          options.Debug = isForcedOn;
          options.AutoSessionTracking = true;
          options.IsGlobalModeEnabled = true;
          options.SendDefaultPii = false;
          options.SetBeforeSend((Func<SentryEvent, SentryHint, SentryEvent>) ((sentryEvent, hint) => SentryService.FilterEvent(sentryEvent)));
        }));
        SentryService.IsEnabled = SentrySdk.IsEnabled;
        if (!SentryService.IsEnabled)
        {
          Log.Warn("[Sentry.NET] SDK initialization failed");
        }
        else
        {
          SentrySdk.ConfigureScope((Action<Scope>) (scope =>
          {
            scope.SetTag("sdk", "dotnet");
            scope.SetTag("session_id", SentryService._sessionId);
            scope.SetExtra("assembly.main_hash", (object) AssemblyHasher.GetMainAssemblyHash());
            if (releaseInfo == null)
              return;
            scope.SetTag("branch", releaseInfo.Branch);
            scope.SetExtra("build.commit", (object) releaseInfo.Commit);
            scope.SetExtra("build.main_hash", (object) releaseInfo.MainAssemblyHash);
            scope.SetExtra("build.date", (object) releaseInfo.Date.ToString("o"));
          }));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Log.LogCallback += SentryService.\u003C\u003EO.\u003C1\u003E__OnLogCallback ?? (SentryService.\u003C\u003EO.\u003C1\u003E__OnLogCallback = new Action<LogLevel, string, int>(SentryService.OnLogCallback));
          Log.Info($"[Sentry.NET] Initialized: env={environment}, release={release}");
        }
      }
    }
  }

  public static void AfterGameInit(string? platformBranch, string uniqueId, Node treeRoot)
  {
    if (!SentryService.IsEnabled)
      return;
    SentryService._sentryInit = treeRoot.GetNode(NodePath.op_Implicit("SentryInit"));
    ((GodotObject) SentryService._sentryInit)?.Call(SentryService._setCsharpContextMethod, new Variant[2]
    {
      Variant.op_Implicit(SentryService._sessionId),
      Variant.op_Implicit(AssemblyHasher.GetMainAssemblyHash())
    });
    if (!SentryService.ShouldStayAliveAfterInit(true))
    {
      Log.Info("[Sentry.NET] Shutting down because event reporting is disabled.");
      SentryService.Shutdown();
    }
    else
    {
      SentrySdk.ConfigureScope((Action<Scope>) (scope => scope.User = new SentryUser()
      {
        Id = uniqueId
      }));
      SentryService.SetGdExtensionUser(uniqueId);
      Log.Debug("[Sentry.NET] User context set");
      SentryService.SetPlatformBranch(platformBranch);
    }
    SentryService._isGameInitialized = true;
  }

  private static void OnLogCallback(LogLevel level, string message, int skipFrames)
  {
    if (!SentryService.IsEnabled)
      return;
    if (level != LogLevel.Warn)
    {
      if (level != LogLevel.Error)
        return;
      SentrySdk.AddBreadcrumb(message, "log", (string) null, (IDictionary<string, string>) null, (BreadcrumbLevel) 2);
      SentryService.SetGdExtensionBreadcrumb(message, "log", (BreadcrumbLevel) 2);
    }
    else
    {
      SentrySdk.AddBreadcrumb(message, "log", (string) null, (IDictionary<string, string>) null, (BreadcrumbLevel) 1);
      SentryService.SetGdExtensionBreadcrumb(message, "log", (BreadcrumbLevel) 1);
    }
  }

  private static void SetGdExtensionUser(string uniqueId)
  {
    try
    {
      if (!Engine.HasSingleton(SentryService._sentrySdkSingleton))
        return;
      if (SentryService._extensionSdk == null)
        SentryService._extensionSdk = Engine.GetSingleton(SentryService._sentrySdkSingleton);
      Variant variant = ClassDB.Instantiate(SentryService._sentryUserClass);
      GodotObject godotObject = ((Variant) ref variant).AsGodotObject();
      godotObject.Set(SentryService._idProperty, Variant.op_Implicit(uniqueId));
      SentryService._extensionSdk.Call(SentryService._setUserMethod, new Variant[1]
      {
        Variant.op_Implicit(godotObject)
      });
    }
    catch (Exception ex)
    {
      Log.Warn("[Sentry] Failed to set GDExtension user: " + ex.Message);
    }
  }

  private static void SetGdExtensionBreadcrumb(
    string message,
    string category,
    BreadcrumbLevel level)
  {
    try
    {
      if (!Engine.HasSingleton(SentryService._sentrySdkSingleton))
        return;
      if (SentryService._extensionSdk == null)
        SentryService._extensionSdk = Engine.GetSingleton(SentryService._sentrySdkSingleton);
      Variant variant = ClassDB.ClassCallStatic(SentryService._sentryBreadcrumbClass, SentryService._createMethod, new Variant[1]
      {
        Variant.op_Implicit(message)
      });
      GodotObject godotObject = ((Variant) ref variant).AsGodotObject();
      godotObject.Set(SentryService._categoryProperty, Variant.op_Implicit(category));
      godotObject.Set(SentryService._levelProperty, Variant.op_Implicit(level + 1));
      SentryService._extensionSdk.Call(SentryService._addBreadcrumbMethod, new Variant[1]
      {
        Variant.op_Implicit(godotObject)
      });
    }
    catch (Exception ex)
    {
      Log.Warn("[Sentry] Failed to set GDExtension breadcrumb: " + ex.Message);
    }
  }

  private static void SetPlatformBranch(string? branch)
  {
    float num;
    switch (branch)
    {
      case "public":
        num = 0.1f;
        break;
      case "private-beta":
        num = 1f;
        break;
      case "public-beta":
        num = 0.2f;
        break;
      default:
        num = branch == null ? (SentryService.SampleForNonSteamBranches ? 1f : 0.0f) : 0.1f;
        break;
    }
    SentryService._sampleRate = num;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ((GodotObject) SentryService._sentryInit)?.Call(SentryService._setShouldSampleEventMethod, new Variant[1]
    {
      Variant.op_Implicit(Callable.From<bool>(SentryService.\u003C\u003EO.\u003C2\u003E__ShouldSampleEvent ?? (SentryService.\u003C\u003EO.\u003C2\u003E__ShouldSampleEvent = new Func<bool>(SentryService.ShouldSampleEvent))))
    });
    if (branch != null)
      ((GodotObject) SentryService._sentryInit)?.Call(SentryService._setPlatformBranchMethod, new Variant[1]
      {
        Variant.op_Implicit(branch)
      });
    if (SentryService.IsEnabled)
    {
      if ((double) SentryService._sampleRate == 0.0)
      {
        Log.Info("[Sentry.NET] Disabled: no platform branch (non-Steam build)");
        SentryService.Shutdown();
        return;
      }
      if (branch != null)
        SentrySdk.ConfigureScope((Action<Scope>) (scope =>
        {
          scope.SetTag("platform.branch", branch);
          scope.Environment = branch;
        }));
    }
    Log.Info($"[Sentry.NET] Platform branch: {branch}, sample rate: {SentryService._sampleRate:P0}");
  }

  public static void AddBreadcrumb(string message, string category = "app", BreadcrumbLevel level = 0)
  {
    if (!SentryService.IsEnabled)
      return;
    SentrySdk.AddBreadcrumb(message, category, (string) null, (IDictionary<string, string>) null, level);
  }

  public static void CaptureException(Exception ex)
  {
    if (!SentryService.IsEnabled)
      return;
    SentrySdk.CaptureException(ex, (Action<Scope>) (scope => SentryService.AttachGameState(scope)));
  }

  public static void CaptureException(Exception ex, Action<Scope> configureScope)
  {
    if (!SentryService.IsEnabled)
      return;
    SentrySdk.CaptureException(ex, (Action<Scope>) (scope =>
    {
      SentryService.AttachGameState(scope);
      configureScope(scope);
    }));
  }

  public static void CaptureMessage(
    string message,
    SentryLevel level = 1,
    Action<Scope>? configureScope = null)
  {
    if (!SentryService.IsEnabled)
      return;
    SentrySdk.CaptureEvent(new SentryEvent()
    {
      Message = SentryMessage.op_Implicit(message),
      Level = new SentryLevel?(level)
    }, (Action<Scope>) (scope =>
    {
      SentryService.AttachGameState(scope);
      Action<Scope> action = configureScope;
      if (action == null)
        return;
      action(scope);
    }));
  }

  public static void SetTag(string key, string value)
  {
    if (!SentryService.IsEnabled)
      return;
    SentrySdk.ConfigureScope((Action<Scope>) (scope => scope.SetTag(key, value)));
  }

  public static void SetExtra(string key, object value)
  {
    if (!SentryService.IsEnabled)
      return;
    SentrySdk.ConfigureScope((Action<Scope>) (scope => scope.SetExtra(key, value)));
  }

  public static void InitializeForTesting()
  {
    if (SentryService.IsEnabled)
      return;
    string dsn = SentryService.GetDsn();
    if (string.IsNullOrEmpty(dsn))
    {
      Log.Warn("[Sentry.NET] Cannot initialize for testing: no DSN configured");
    }
    else
    {
      SentryService._sentryInstance = SentrySdk.Init((Action<SentryOptions>) (options =>
      {
        options.Dsn = dsn;
        options.Environment = "development";
        options.Release = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "dev-console-test";
        options.Debug = false;
        options.AutoSessionTracking = false;
        options.IsGlobalModeEnabled = true;
        options.SendDefaultPii = false;
      }));
      SentryService.IsEnabled = SentrySdk.IsEnabled;
      if (!SentryService.IsEnabled)
      {
        Log.Warn("[Sentry.NET] SDK initialization failed for testing");
      }
      else
      {
        SentrySdk.ConfigureScope((Action<Scope>) (scope =>
        {
          scope.SetTag("sdk", "dotnet");
          scope.SetTag("session_id", SentryService._sessionId);
          scope.SetTag("source", "dev-console-test");
        }));
        Log.Info("[Sentry.NET] Initialized for testing via dev console");
      }
    }
  }

  public static void Shutdown()
  {
    if (!SentryService.IsEnabled)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    Log.LogCallback -= SentryService.\u003C\u003EO.\u003C1\u003E__OnLogCallback ?? (SentryService.\u003C\u003EO.\u003C1\u003E__OnLogCallback = new Action<LogLevel, string, int>(SentryService.OnLogCallback));
    Log.Info("[Sentry.NET] Shutting down");
    SentryService._sentryInstance?.Dispose();
    SentryService._sentryInstance = (IDisposable) null;
    ((GodotObject) SentryService._sentryInit)?.Call(SentryService._shutdownMethod, Array.Empty<Variant>());
    SentryService.IsEnabled = false;
  }

  private static string GetDsn()
  {
    Variant setting = ProjectSettings.GetSetting("sentry/config/dsn", Variant.op_Implicit(""));
    return ((Variant) ref setting).AsString();
  }

  private static void AttachGameState(Scope scope)
  {
    try
    {
      scope.SetExtra("loc.language", (object) LocManager.Instance.Language);
      string currentSceneName = SentryService.GetCurrentSceneName();
      if (currentSceneName != null)
        scope.SetTag("game.scene", currentSceneName);
      RunState state1 = RunManager.Instance.DebugOnlyGetState();
      if (RunManager.Instance.IsInProgress && state1 != null)
      {
        scope.SetTag("game.in_run", "true");
        scope.SetExtra("game.seed", (object) state1.Rng.StringSeed);
        scope.SetExtra("game.ascension", (object) state1.AscensionLevel);
        scope.SetExtra("game.act", (object) (state1.CurrentActIndex + 1));
        scope.SetExtra("game.act_name", (object) state1.Act.Id.ToString());
        scope.SetExtra("game.floor", (object) state1.TotalFloor);
        scope.SetExtra("game.mode", (object) state1.GameMode);
        AbstractRoom currentRoom = state1.CurrentRoom;
        scope.SetExtra("game.room_type", (object) currentRoom?.GetType().Name);
        if (currentRoom is EventRoom eventRoom)
          scope.SetExtra("game.event", (object) eventRoom.CanonicalEvent.Id.Entry);
        IReadOnlyList<Player> players = state1.Players;
        if (players.Count > 0)
        {
          scope.SetExtra("game.characters", (object) string.Join<ModelId>(", ", players.Select<Player, ModelId>((Func<Player, ModelId>) (p => p.Character.Id))));
          scope.SetExtra("game.player_count", (object) players.Count);
        }
      }
      else
        scope.SetTag("game.in_run", "false");
      CombatState state2 = CombatManager.Instance.DebugOnlyGetState();
      if (state2 == null)
        return;
      scope.SetExtra("combat.encounter", (object) state2.Encounter?.Id.Entry);
      scope.SetExtra("combat.round", (object) state2.RoundNumber);
      scope.SetExtra("combat.enemy_count", (object) state2.Enemies.Count);
      scope.SetExtra("combat.enemies", (object) string.Join(", ", state2.Enemies.Select<Creature, string>((Func<Creature, string>) (e => e.Monster?.Id.ToString() ?? "unknown"))));
      List<string> list = state2.Players.Select<Player, string>((Func<Player, string>) (p => $"{p.Creature.CurrentHp}/{p.Creature.MaxHp}")).ToList<string>();
      if (list.Count <= 0)
        return;
      scope.SetExtra("combat.player_hp", (object) string.Join(", ", (IEnumerable<string>) list));
    }
    catch
    {
    }
  }

  private static string? GetCurrentSceneName()
  {
    try
    {
      NGame instance = NGame.Instance;
      if (instance == null)
        return (string) null;
      if (instance.MainMenu != null)
        return "MainMenu";
      if (instance.CurrentRunNode != null)
      {
        NRun currentRunNode = instance.CurrentRunNode;
        if (currentRunNode.CombatRoom != null)
          return "CombatRoom";
        if (currentRunNode.MapRoom != null)
          return "MapRoom";
        if (currentRunNode.EventRoom != null)
          return "EventRoom";
        if (currentRunNode.RestSiteRoom != null)
          return "RestSiteRoom";
        if (currentRunNode.MerchantRoom != null)
          return "MerchantRoom";
        return currentRunNode.TreasureRoom != null ? "TreasureRoom" : "Run";
      }
      return instance.LogoAnimation != null ? "LogoAnimation" : (string) null;
    }
    catch
    {
      return (string) null;
    }
  }

  private static SentryEvent? FilterEvent(SentryEvent sentryEvent)
  {
    if (SentryService._suppressAllEvents)
      return (SentryEvent) null;
    if (sentryEvent.Exception is AutoSlayTimeoutException)
      return (SentryEvent) null;
    return !SentryService.ShouldSampleEvent() ? (SentryEvent) null : sentryEvent;
  }

  private static bool ShouldSampleEvent()
  {
    return Random.Shared.NextDouble() < (double) SentryService._sampleRate && (!SaveManager.Instance.IsProfileInitialized || SaveManager.Instance.PrefsSave.UploadData) && (SentryService._isGameInitialized || SentryService.ShouldStayAliveAfterInit(false));
  }

  private static bool ShouldStayAliveAfterInit(bool shouldLog)
  {
    if (SentryService.IsForcedOn)
    {
      if (shouldLog)
        Log.Info("[Sentry.NET] Staying alive because we're forced on");
      return true;
    }
    if (!SteamInitializer.Initialized)
    {
      if (shouldLog)
        Log.Info("[Sentry.NET] Steam not initialized");
      return false;
    }
    try
    {
      if (SaveManager.Instance.SettingsSave.FullConsole)
      {
        if (shouldLog)
          Log.Info("[Sentry.NET] Full console is on");
        return false;
      }
    }
    catch
    {
      if (shouldLog)
        Log.Info("[Sentry.NET] Exception while checking UploadData or FullConsole");
      return false;
    }
    if (ModManager.IsRunningModded())
    {
      if (shouldLog)
        Log.Info("[Sentry.NET] Is running modded");
      return false;
    }
    LocManager instance = LocManager.Instance;
    if (instance != null && instance.OverridesActive)
    {
      if (shouldLog)
        Log.Info("[Sentry.NET] Loc overrides are active");
      return false;
    }
    if (!ModManager.HasHarmonyPatches())
      return true;
    if (shouldLog)
      Log.Info("[Sentry.NET] Harmony patches active");
    return false;
  }
}
