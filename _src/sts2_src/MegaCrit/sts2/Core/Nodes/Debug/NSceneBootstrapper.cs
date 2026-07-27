// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.NSceneBootstrapper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[ScriptPath("res://src/Core/Nodes/Debug/NSceneBootstrapper.cs")]
public class NSceneBootstrapper : Node
{
  private bool _openConsole;
  private NGame _game;

  public override void _Ready()
  {
    if (this.GetParent() is NGame parent)
    {
      this._game = parent;
    }
    else
    {
      this._game = SceneHelper.Instantiate<NGame>("game");
      this._game.StartOnMainMenu = false;
      this.AddChildSafely((Node) this._game);
    }
    TaskHelper.RunSafely(this.StartNewRun());
  }

  private async Task StartNewRun()
  {
    await this._game.GameStartupComplete;
    Type type = BootstrapSettingsUtil.Get();
    IBootstrapSettings settings;
    RunState runState;
    if (type == (Type) null)
    {
      Log.Error("No type implementing IBootstrapSettings found in the project! To use the bootstrap scene, copy src/Core/Nodes/Debug/BootstrapSettings.cs.template and rename it to BootstrapSettings.cs");
      settings = (IBootstrapSettings) null;
      runState = (RunState) null;
    }
    else
    {
      settings = (IBootstrapSettings) Activator.CreateInstance(type);
      if (settings.Language != null)
        LocManager.Instance.SetLanguage(settings.Language);
      PreloadManager.Enabled = settings.DoPreloading;
      string seed = settings.Seed ?? SeedHelper.GetRandomSeed();
      List<ActModel> list = ActModel.GetDefaultList().ToList<ActModel>();
      list[0] = settings.Act;
      // ISSUE: object of a compiler-generated type is created
      runState = RunState.CreateForNewRun((IReadOnlyList<Player>) new \u003C\u003Ez__ReadOnlySingleElementList<Player>(Player.CreateForNewRun(settings.Character, SaveManager.Instance.GenerateUnlockStateFromProgress(), 1UL)), (IReadOnlyList<ActModel>) list.Select<ActModel, ActModel>((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList<ActModel>(), (IReadOnlyList<ModifierModel>) settings.Modifiers, GameMode.Standard, settings.Ascension, seed);
      RunManager.Instance.SetUpNewSingleplayer(runState, settings.SaveRunHistory);
      // ISSUE: object of a compiler-generated type is created
      await PreloadManager.LoadRunAssets((IEnumerable<CharacterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CharacterModel>(settings.Character));
      RunManager.Instance.Launch();
      this._game.RootSceneContainer.SetCurrentScene((Control) NRun.Create(runState));
      await RunManager.Instance.SetActInternal(0);
      RunManager.Instance.RunLocationTargetedBuffer.OnLocationChanged(runState.RunLocation);
      RunManager.Instance.MapSelectionSynchronizer.OnLocationChanged(runState.MapLocation);
      await settings.Setup(runState.Players[0]);
      switch (settings.RoomType)
      {
        case RoomType.Unassigned:
          await RunManager.Instance.EnterAct(0);
          break;
        case RoomType.Treasure:
        case RoomType.Shop:
        case RoomType.RestSite:
          AbstractRoom abstractRoom1 = await RunManager.Instance.EnterRoomDebug(settings.RoomType);
          RunManager.Instance.ActionExecutor.Unpause();
          break;
        case RoomType.Event:
          AbstractRoom abstractRoom2 = await RunManager.Instance.EnterRoomDebug(settings.RoomType, model: (AbstractModel) settings.Event);
          if (abstractRoom2 != null && abstractRoom2.IsVictoryRoom)
          {
            runState.CurrentActIndex = runState.Acts.Count - 1;
            break;
          }
          break;
        default:
          AbstractRoom abstractRoom3 = await RunManager.Instance.EnterRoomDebug(settings.RoomType, model: settings.RoomType.IsCombatRoom() ? (AbstractModel) settings.Encounter.ToMutable() : (AbstractModel) null);
          break;
      }
      if (!this._openConsole)
      {
        settings = (IBootstrapSettings) null;
        runState = (RunState) null;
      }
      else
      {
        NDevConsole.Instance.ShowConsole();
        NDevConsole.Instance.MakeFullScreen();
        NDevConsole.Instance.SetBackgroundColor(Colors.White);
        settings = (IBootstrapSettings) null;
        runState = (RunState) null;
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NSceneBootstrapper.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NSceneBootstrapper.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSceneBootstrapper.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSceneBootstrapper.PropertyName._openConsole))
    {
      this._openConsole = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSceneBootstrapper.PropertyName._game))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._game = VariantUtils.ConvertTo<NGame>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSceneBootstrapper.PropertyName._openConsole))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._openConsole);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSceneBootstrapper.PropertyName._game))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NGame>(ref this._game);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NSceneBootstrapper.PropertyName._openConsole, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSceneBootstrapper.PropertyName._game, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSceneBootstrapper.PropertyName._openConsole, Variant.From<bool>(ref this._openConsole));
    info.AddProperty(NSceneBootstrapper.PropertyName._game, Variant.From<NGame>(ref this._game));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSceneBootstrapper.PropertyName._openConsole, ref variant1))
      this._openConsole = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (!info.TryGetProperty(NSceneBootstrapper.PropertyName._game, ref variant2))
      return;
    this._game = ((Variant) ref variant2).As<NGame>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _openConsole = StringName.op_Implicit(nameof (_openConsole));
    public static readonly StringName _game = StringName.op_Implicit(nameof (_game));
  }

  public class SignalName : Node.SignalName
  {
  }
}
