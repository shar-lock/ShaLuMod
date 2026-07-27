// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.ModManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Managers;
using MegaCrit.Sts2.Core.TestSupport;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public static class ModManager
{
  private static bool _allowInitForTests;
  private static List<Mod> _mods = new List<Mod>();
  private static Callback<ItemInstalled_t>? _steamItemInstalledCallback;
  private static ModSettings? _settings;
  private static IModManagerFileIo? _fileIo;
  private static SemanticVersion? _gameVersion;
  private static readonly Dictionary<string, string> _circularDependencies = new Dictionary<string, string>();
  private static bool? _hasHarmonyPatches;

  public static ModManagerState State { get; private set; }

  public static IReadOnlyList<Mod> Mods => (IReadOnlyList<Mod>) ModManager._mods;

  public static event Action<Mod>? OnModDetected;

  public static event ModManager.MetricsUploadHook? OnMetricsUpload;

  public static bool PlayerAgreedToModLoading
  {
    get
    {
      ModSettings settings = ModManager._settings;
      return settings != null && settings.PlayerAgreedToModLoading;
    }
  }

  public static bool UnmoddedSavesWereCopied { get; private set; }

  public static Dictionary<string, Action> TestInitializers { get; } = new Dictionary<string, Action>();

  public static async Task Initialize(
    IModManagerFileIo fileIo,
    ModSettings? settings,
    SemanticVersion? gameVersion)
  {
    ModManager._settings = settings;
    ModManager._fileIo = fileIo;
    ModManager._gameVersion = gameVersion;
    if (ModManager._gameVersion == null)
      Log.Warn("Game doesn't have ReleaseInfo. We can't check version compatibility, so assuming all mods are supported.");
    if (CommandLineHelper.HasArg("nomods"))
    {
      Log.Info("'nomods' passed as executable argument, skipping mod initialization");
      ModManager.State = ModManagerState.Skipped;
    }
    else if (TestMode.IsOn && !ModManager._allowInitForTests)
    {
      ModManager.State = ModManagerState.Skipped;
    }
    else
    {
      ModManager._allowInitForTests = false;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      AppDomain.CurrentDomain.AssemblyResolve += ModManager.\u003C\u003EO.\u003C0\u003E__HandleAssemblyResolveFailure ?? (ModManager.\u003C\u003EO.\u003C0\u003E__HandleAssemblyResolveFailure = new ResolveEventHandler(ModManager.HandleAssemblyResolveFailure));
      string directoryName = Path.GetDirectoryName(OS.GetExecutablePath());
      string path1 = Path.Combine(directoryName, "mods");
      string path2 = Path.Combine(directoryName, "mods_STEAMTEST");
      if (fileIo.DirectoryExists(path1))
        ModManager.ReadModsInDirRecursive(path1, ModSource.ModsDirectory, (List<Mod>) null);
      if (fileIo.DirectoryExists(path2))
        ModManager.ReadModsInDirRecursive(path2, ModSource.SteamWorkshop, (List<Mod>) null);
      if (SteamInitializer.Initialized)
        ModManager.ReadSteamMods();
      if (OS.IsDebugBuild())
        await Task.Yield();
      if (ModManager._mods.Count == 0)
      {
        ModManager.State = ModManagerState.Initialized;
      }
      else
      {
        await ModManager.CheckSteamBranchSupport();
        ModManager.RemoveDisabledMods();
        ModManager.SortModList(ModManager._settings?.ModList ?? new List<SettingsSaveMod>());
        foreach (Mod mod in ModManager._mods)
          ModManager.TryLoadMod(mod);
        if (ModManager.IsRunningModded())
          Log.Info($" --- RUNNING MODDED! --- Loaded {ModManager._mods.Count<Mod>((Func<Mod, bool>) (m => m.state == ModLoadState.Loaded))} mods ({ModManager._mods.Count} total)");
        ModManager.State = ModManagerState.Initialized;
        bool flag1 = false;
        if (ModManager._settings != null)
        {
          List<SettingsSaveMod> settingsSaveModList = new List<SettingsSaveMod>();
          foreach (Mod mod1 in ModManager._mods)
          {
            Mod mod = mod1;
            SettingsSaveMod settingsSaveMod1 = new SettingsSaveMod(mod);
            SettingsSaveMod settingsSaveMod2 = ModManager._settings.ModList.FirstOrDefault<SettingsSaveMod>((Func<SettingsSaveMod, bool>) (m => m.Id == mod.manifest?.id));
            bool flag2 = settingsSaveMod2 == null || settingsSaveMod2.IsEnabled;
            settingsSaveMod1.IsEnabled = flag2;
            settingsSaveModList.Add(settingsSaveMod1);
          }
          flag1 = ModManager._settings.ModList.Count == 0;
          ModManager._settings.ModList = settingsSaveModList;
        }
        if (!flag1)
          return;
        Log.Info("Player is playing modded for the first time. Checking if we need to copy unmodded save files");
        ModManager.CopyUnmoddedSaveFilesIfNeeded();
      }
    }
  }

  public static void ResetForTests()
  {
    if (TestMode.IsOff)
      throw new NotImplementedException("Tried to reset ModManager outside of tests! This is not allowed, as we cannot unload DLLs or PCKs");
    ModManager._mods.Clear();
    ModManager.State = ModManagerState.None;
    ModManager._settings = (ModSettings) null;
    ModManager._fileIo = (IModManagerFileIo) null;
    ModManager._allowInitForTests = true;
    ModManager._circularDependencies.Clear();
    ModManager.TestInitializers.Clear();
  }

  private static void SortModList(List<SettingsSaveMod> manualOrdering)
  {
    List<Mod> source = new List<Mod>();
    List<Mod> collection = new List<Mod>();
    foreach (Mod mod in ModManager._mods)
    {
      if (mod.state != ModLoadState.None)
        collection.Add(mod);
      else
        source.Add(mod);
    }
    List<int> intList = new List<int>();
    Dictionary<Mod, List<Mod>> dictionary1 = new Dictionary<Mod, List<Mod>>();
    for (int index = 0; index < source.Count; ++index)
    {
      Mod mod = source[index];
      int num = 0;
      if (mod.manifest?.dependencies != null)
      {
        foreach (ModDependency dependency in mod.manifest.dependencies)
        {
          ModDependency declaredDependency = dependency;
          Mod key = source.FirstOrDefault<Mod>((Func<Mod, bool>) (m => m.manifest?.id == declaredDependency.id));
          if (key != null)
          {
            ++num;
            List<Mod> modList;
            if (!dictionary1.TryGetValue(key, out modList))
            {
              modList = new List<Mod>();
              dictionary1[key] = modList;
            }
            modList.Add(mod);
          }
        }
      }
      intList.Add(num);
    }
    PriorityQueue<Mod, int> priorityQueue = new PriorityQueue<Mod, int>();
    Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
    for (int index = 0; index < manualOrdering.Count; ++index)
      dictionary2[manualOrdering[index].Id] = index;
    for (int index = 0; index < source.Count; ++index)
    {
      if (intList[index] == 0)
      {
        int num1;
        int num2 = dictionary2.TryGetValue(source[index].manifest.id, out num1) ? num1 : 999999999;
        priorityQueue.Enqueue(source[index], num2);
      }
    }
    List<Mod> modList1 = new List<Mod>();
    while (priorityQueue.Count > 0)
    {
      Mod key = priorityQueue.Dequeue();
      modList1.Add(key);
      List<Mod> modList2;
      if (dictionary1.TryGetValue(key, out modList2))
      {
        foreach (Mod mod in modList2)
        {
          int index = source.IndexOf(mod);
          if (index < 0)
            throw new InvalidOperationException("Bug in mod sorting logic!");
          intList[index]--;
          if (intList[index] == 0)
          {
            int num3;
            int num4 = dictionary2.TryGetValue(mod.manifest.id, out num3) ? num3 : 999999999;
            priorityQueue.Enqueue(mod, num4);
          }
        }
      }
    }
    HashSet<Mod> modSet = new HashSet<Mod>();
    foreach (Mod mod in modList1)
      modSet.Add(mod);
    HashSet<Mod> sortedSet = modSet;
    string str = string.Join(", ", source.Where<Mod>((Func<Mod, bool>) (m => !sortedSet.Contains(m))).Select<Mod, string>((Func<Mod, string>) (m => m.manifest?.id)));
    foreach (Mod mod in source)
    {
      if (!sortedSet.Contains(mod) && mod.manifest?.id != null)
        ModManager._circularDependencies[mod.manifest.id] = str;
    }
    foreach (Mod mod in source)
    {
      if (!sortedSet.Contains(mod))
        modList1.Add(mod);
    }
    bool flag = manualOrdering.Count != modList1.Count;
    if (!flag)
    {
      for (int index = 0; index < manualOrdering.Count; ++index)
      {
        if (manualOrdering[index].Id != modList1[index].manifest?.id)
        {
          flag = true;
          break;
        }
      }
    }
    if (flag)
    {
      Log.Info("Mods have been re-sorted because we detected a change or dependency order was broken. New sorting order:");
      for (int index = 0; index < modList1.Count; ++index)
        Log.Info($"  {index}: {modList1[index].manifest?.name} ({modList1[index].manifest?.id})");
    }
    modList1.AddRange((IEnumerable<Mod>) collection);
    ModManager._mods = modList1;
  }

  private static void RemoveDisabledMods()
  {
    Dictionary<string, Mod> dictionary = new Dictionary<string, Mod>();
    foreach (Mod mod in ModManager._mods)
    {
      if (mod.manifest?.id != null)
      {
        ModSettings settings = ModManager._settings;
        if (settings != null && settings.IsModDisabled(mod.manifest.id, mod.modSource))
        {
          Log.Info($"Skipping loading mod {mod.manifest.id}, it is set to disabled in settings");
          mod.state = ModLoadState.Disabled;
        }
        else if (mod.modSource == ModSource.ModsDirectory)
          dictionary.TryAdd(mod.manifest.id, mod);
      }
    }
    foreach (Mod mod1 in ModManager._mods)
    {
      Mod mod2;
      if (mod1.manifest?.id != null && mod1.modSource == ModSource.SteamWorkshop && mod1.state == ModLoadState.None && dictionary.TryGetValue(mod1.manifest.id, out mod2))
      {
        SemanticVersion version1 = (SemanticVersion) null;
        SemanticVersion version2 = (SemanticVersion) null;
        if (mod2.manifest?.version != null)
          SemanticVersion.TryFromString(mod2.manifest.version, out version1);
        if (mod1.manifest.version != null)
          SemanticVersion.TryFromString(mod1.manifest.version, out version2);
        if (version2 == null || version1 == null)
        {
          Log.Warn($"Mod with ID {mod1.manifest.id} and unknown version is loaded both via Steam and local mods directory. Disabling the Steam workshop version.");
          mod1.state = ModLoadState.DisabledDuplicate;
          mod1.errors?.Clear();
        }
        else
        {
          int num = version2.CompareTo(version1);
          if (num == 0)
          {
            Log.Warn($"Mod with ID {mod1.manifest.id} and version {mod1.manifest.version} is loaded both via Steam and local mods directory. Disabling the Steam workshop version.");
            mod1.state = ModLoadState.DisabledDuplicate;
            mod1.errors?.Clear();
          }
          else if (num > 0)
          {
            Log.Warn($"Mod with ID {mod1.manifest.id} is loaded both via Steam and local mods directory. Steam version ({version2}) is greater than local version ({version1}), so we are disabling the local version.");
            mod2.state = ModLoadState.DisabledDuplicate;
            mod2.errors?.Clear();
          }
          else
          {
            Log.Warn($"Mod with ID {mod1.manifest.id} is loaded both via Steam and local mods directory. Local version ({version1}) is greater than steam version ({version2}), so we are disabling the Steam workshop version.");
            mod1.state = ModLoadState.DisabledDuplicate;
            mod1.errors?.Clear();
          }
        }
      }
    }
  }

  private static void ReadModsInDirRecursive(string path, ModSource source, List<Mod>? newMods)
  {
    foreach (string str in ModManager._fileIo?.GetFilesAt(path) ?? Array.Empty<string>())
    {
      if (str.EndsWith(".json"))
      {
        string filename = Path.Combine(path, str);
        Log.Info("Found mod manifest file " + filename);
        Mod mod = ModManager.ReadModManifest(filename, source);
        if (mod != null)
        {
          ModManager._mods.Add(mod);
          newMods?.Add(mod);
        }
      }
    }
    foreach (string str in ModManager._fileIo?.GetDirectoriesAt(path) ?? Array.Empty<string>())
    {
      string path1 = Path.Combine(path, str);
      if (ModManager._fileIo.DirectoryExists(path1))
        ModManager.ReadModsInDirRecursive(path1, source, newMods);
    }
  }

  private static Mod? ReadModManifest(string filename, ModSource source)
  {
    if (ModManager._fileIo == null)
      return (Mod) null;
    try
    {
      using (Stream stream = ModManager._fileIo.OpenStream(filename, (FileAccess.ModeFlags) 1L))
      {
        List<LocString> errors;
        ModManifest modManifest = ModManifest.ReadFromStream(stream, out errors);
        if (modManifest == (ModManifest) null)
          throw new InvalidOperationException("JSON deserialization returned null when trying to deserialize mod manifest!");
        if (modManifest.id == null)
        {
          if (modManifest.name == null && modManifest.author == null && modManifest.description == null && modManifest.version == null)
            Log.Info($"JSON file {filename} does not look like a mod manifest; skipping it.");
          else
            Log.Error($"JSON file {filename} looks like a mod manifest but is missing the 'id' field! This is not allowed.");
          return (Mod) null;
        }
        return new Mod()
        {
          path = StringExtensions.GetBaseDir(filename),
          modSource = source,
          manifest = modManifest,
          errors = errors
        };
      }
    }
    catch (Exception ex)
    {
      Log.Error($"Caught {ex.GetType()} trying to deserialize mod manifest json at path {filename}:\n{ex}");
      return (Mod) null;
    }
  }

  public static (bool IsSupported, PlatformBranch? MaxSupportedBranch) EvaluateBranchSupport(
    PlatformBranch currentBranch,
    IReadOnlyList<(string MinBranch, string MaxBranch)> supportedVersions)
  {
    if (supportedVersions.Count == 0)
      return (true, new PlatformBranch?());
    PlatformBranch? nullable1 = new PlatformBranch?();
    foreach ((string str1, string str2) in (IEnumerable<(string MinBranch, string MaxBranch)>) supportedVersions)
    {
      bool flag1 = string.IsNullOrEmpty(str1);
      bool flag2 = string.IsNullOrEmpty(str2);
      PlatformBranch? nullable2 = flag1 ? new PlatformBranch?() : PlatformBranchExtensions.FromName(str1);
      PlatformBranch? nullable3 = flag2 ? new PlatformBranch?() : PlatformBranchExtensions.FromName(str2);
      PlatformBranch? nullable4 = flag2 ? new PlatformBranch?(PlatformBranch.DevTest) : nullable3;
      PlatformBranch? nullable5;
      if (nullable4.HasValue)
      {
        if (nullable1.HasValue)
        {
          PlatformBranch? nullable6 = nullable4;
          nullable5 = nullable1;
          if (!(nullable6.GetValueOrDefault() > nullable5.GetValueOrDefault() & nullable6.HasValue & nullable5.HasValue))
            goto label_8;
        }
        nullable1 = nullable4;
      }
label_8:
      int num1;
      if (!flag1)
      {
        if (nullable2.HasValue)
        {
          int num2 = (int) currentBranch;
          nullable5 = nullable2;
          int valueOrDefault = (int) nullable5.GetValueOrDefault();
          num1 = num2 >= valueOrDefault & nullable5.HasValue ? 1 : 0;
        }
        else
          num1 = 0;
      }
      else
        num1 = 1;
      bool flag3 = num1 != 0;
      int num3;
      if (!flag2)
      {
        if (nullable3.HasValue)
        {
          int num4 = (int) currentBranch;
          nullable5 = nullable3;
          int valueOrDefault = (int) nullable5.GetValueOrDefault();
          num3 = num4 <= valueOrDefault & nullable5.HasValue ? 1 : 0;
        }
        else
          num3 = 0;
      }
      else
        num3 = 1;
      bool flag4 = num3 != 0;
      if (flag3 & flag4)
        return (true, nullable1);
    }
    return (false, nullable1);
  }

  private static void ReadSteamMods()
  {
    uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
    PublishedFileId_t[] publishedFileIdTArray = new PublishedFileId_t[(int) numSubscribedItems];
    uint subscribedItems = SteamUGC.GetSubscribedItems(publishedFileIdTArray, numSubscribedItems);
    for (int index = 0; (long) index < (long) subscribedItems; ++index)
      ModManager.TryReadModFromSteam(publishedFileIdTArray[index], (List<Mod>) null);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    ModManager._steamItemInstalledCallback = Callback<ItemInstalled_t>.Create(ModManager.\u003C\u003EO.\u003C1\u003E__OnSteamWorkshopItemInstalled ?? (ModManager.\u003C\u003EO.\u003C1\u003E__OnSteamWorkshopItemInstalled = new Callback<ItemInstalled_t>.DispatchDelegate((object) null, __methodptr(OnSteamWorkshopItemInstalled))));
  }

  private static void TryReadModFromSteam(PublishedFileId_t workshopItemId, List<Mod>? newMods)
  {
    ulong num1;
    string path;
    uint num2;
    if (!SteamUGC.GetItemInstallInfo(workshopItemId, ref num1, ref path, 256U /*0x0100*/, ref num2))
    {
      Log.Warn($"Could not get Steam Workshop item install info for item {workshopItemId.m_PublishedFileId}");
    }
    else
    {
      Log.Info($"Looking for mods to load from Steam Workshop mod {workshopItemId.m_PublishedFileId} in {path} (size {num1}, last modified {num2})");
      if (ModManager._fileIo != null && !ModManager._fileIo.DirectoryExists(path))
      {
        Log.Warn("Could not open Steam Workshop folder: " + path);
      }
      else
      {
        List<Mod> modList = new List<Mod>();
        ModManager.ReadModsInDirRecursive(path, ModSource.SteamWorkshop, modList);
        foreach (Mod mod in modList)
          mod.workshopId = new ulong?(workshopItemId.m_PublishedFileId);
        newMods?.AddRange((IEnumerable<Mod>) modList);
      }
    }
  }

  private static async Task CheckSteamBranchSupport()
  {
    PublishedFileId_t[] workshopMods = ModManager._mods.Where<Mod>((Func<Mod, bool>) (m => m.workshopId.HasValue)).Select<Mod, PublishedFileId_t>((Func<Mod, PublishedFileId_t>) (m => new PublishedFileId_t(m.workshopId.Value))).Distinct<PublishedFileId_t>().ToArray<PublishedFileId_t>();
    if (workshopMods.Length == 0)
    {
      workshopMods = (PublishedFileId_t[]) null;
    }
    else
    {
      UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUGCDetailsRequest(workshopMods, (uint) workshopMods.Length);
      try
      {
        using (SteamCallResult<SteamUGCQueryCompleted_t> callResult = new SteamCallResult<SteamUGCQueryCompleted_t>(SteamUGC.SendQueryUGCRequest(queryHandle), SteamInitializer.DisconnectToken))
        {
          SteamUGCQueryCompleted_t task = await callResult.Task;
          if (task.m_eResult != 1)
          {
            Log.Warn($"Steam UGC branch-support query failed with {task.m_eResult}; loading mods without the branch check.");
            workshopMods = (PublishedFileId_t[]) null;
            return;
          }
          for (uint index1 = 0; (long) index1 < (long) workshopMods.Length; ++index1)
          {
            PublishedFileId_t publishedFileIdT = workshopMods[(int) index1];
            uint supportedGameVersions = SteamUGC.GetNumSupportedGameVersions(task.m_handle, index1);
            List<(string, string)> supportedVersions = new List<(string, string)>();
            for (uint index2 = 0; index2 < supportedGameVersions; ++index2)
            {
              string str1;
              string str2;
              if (SteamUGC.GetSupportedGameVersionData(task.m_handle, index1, index2, ref str1, ref str2, 999U))
                supportedVersions.Add((str1, str2));
            }
            if (supportedGameVersions > 0U && supportedVersions.Count == 0)
              Log.Warn($"Steam reported {supportedGameVersions} supported game version range(s) for mod {publishedFileIdT.m_PublishedFileId} but none could be read; loading it without the branch check.");
            PlatformBranch platformBranch = PlatformUtil.GetPlatformBranch();
            (bool IsSupported, PlatformBranch? MaxSupportedBranch) = ModManager.EvaluateBranchSupport(platformBranch, (IReadOnlyList<(string, string)>) supportedVersions);
            if (!IsSupported)
            {
              foreach (Mod mod1 in ModManager._mods)
              {
                ulong? workshopId = mod1.workshopId;
                ulong publishedFileId = publishedFileIdT.m_PublishedFileId;
                if ((long) workshopId.GetValueOrDefault() == (long) publishedFileId & workshopId.HasValue)
                {
                  LocString locString = new LocString("main_menu_ui", "MOD_ERROR.STEAM_BRANCH_UNSUPPORTED");
                  locString.Add("id", mod1.manifest?.id ?? "<null>");
                  locString.Add("currentBranch", platformBranch.ToName());
                  locString.Add("supportedBranch", (MaxSupportedBranch.HasValue ? MaxSupportedBranch.GetValueOrDefault().ToName() : (string) null) ?? "<null>");
                  Mod mod2 = mod1;
                  if (mod2.errors == null)
                    mod2.errors = new List<LocString>();
                  mod1.errors.Add(locString);
                  Log.Error($"Tried to load mod with id {mod1.manifest?.id}, but the current Steam branch {platformBranch.ToName()} does not lie in the min/max steam branches the mod supports! Max supported: {(MaxSupportedBranch.HasValue ? MaxSupportedBranch.GetValueOrDefault().ToName() : (string) null)}");
                }
              }
            }
          }
        }
        workshopMods = (PublishedFileId_t[]) null;
      }
      catch (Exception ex)
      {
        Log.Warn($"Could not verify Steam branch support for mods. Loading them anyways. Exception: {ex}");
        workshopMods = (PublishedFileId_t[]) null;
      }
      finally
      {
        SteamUGC.ReleaseQueryUGCRequest(queryHandle);
      }
    }
  }

  private static void OnSteamWorkshopItemInstalled(ItemInstalled_t ev)
  {
    if (ev.m_unAppID.m_AppId != 2868840U)
      return;
    Log.Info($"Detected new Steam Workshop item installation, id: {ev.m_nPublishedFileId.m_PublishedFileId}");
    List<Mod> newMods = new List<Mod>();
    ModManager.TryReadModFromSteam(ev.m_nPublishedFileId, newMods);
    foreach (Mod mod in newMods)
    {
      mod.state = ModLoadState.AddedAtRuntime;
      ModManager.InvokeOnModDetected(mod);
    }
  }

  private static void TryLoadMod(Mod mod)
  {
    if (mod.state != ModLoadState.None)
    {
      ModManager.InvokeOnModDetected(mod);
    }
    else
    {
      Assembly assembly = (Assembly) null;
      List<LocString> locStringList = mod.errors ?? new List<LocString>();
      if (mod.manifest == (ModManifest) null)
        throw new InvalidOperationException("Tried to load mod before its manifest was loaded!");
      if (mod.manifest.version == null)
      {
        Log.Warn($"Mod {mod.manifest.id} does not declare a version");
      }
      else
      {
        SemanticVersion version;
        if (!SemanticVersion.TryFromString(mod.manifest.version, out version))
          Log.Warn($"Mod {mod.manifest.id} declares version {mod.manifest.version} which is not a valid Semantic Version");
        else
          mod.version = version;
      }
      string modId = mod.manifest.id;
      bool flag1 = ModManager._mods.Any<Mod>((Func<Mod, bool>) (m => m.manifest?.id == modId && m.state == ModLoadState.Loaded));
      bool flag2 = false;
      bool flag3 = true;
      if (ModManager._gameVersion != null)
      {
        if (mod.manifest.minGameVersion == null)
        {
          Log.Warn($"Mod {mod.manifest.id} does not declare min game version. Assuming that it is supported.");
        }
        else
        {
          SemanticVersion version;
          if (!SemanticVersion.TryFromString(mod.manifest.minGameVersion, out version))
            flag2 = true;
          else
            flag3 = ModManager._gameVersion.CompareTo(version) >= 0;
        }
      }
      if (ModManager.State != ModManagerState.None)
      {
        Log.Info($"Skipping loading mod {modId}, can't load mods at runtime");
        mod.state = ModLoadState.AddedAtRuntime;
      }
      else if (!ModManager.PlayerAgreedToModLoading)
      {
        Log.Info($"Skipping loading mod {modId}, user has not yet seen the mods warning");
        mod.state = ModLoadState.Disabled;
      }
      else if (flag1)
      {
        LocString locString = new LocString("main_menu_ui", "MOD_ERROR.DUPLICATE_ID");
        locString.Add("id", modId);
        locStringList.Add(locString);
        Log.Error($"Tried to load mod with id {modId}, but a mod is already loaded with that name!");
        mod.state = ModLoadState.Failed;
      }
      else
      {
        string variable1;
        if (ModManager._circularDependencies.TryGetValue(modId, out variable1))
        {
          LocString locString = new LocString("main_menu_ui", "MOD_ERROR.CIRCULAR_DEPENDENCY");
          locString.Add("id", modId);
          locString.Add("dependencyChain", variable1);
          locStringList.Add(locString);
          Log.Error($"Tried to load mod with id {modId}, but it is part of a circular dependency chain: {variable1}!");
          mod.state = ModLoadState.Failed;
        }
        else if (flag2)
        {
          LocString locString = new LocString("main_menu_ui", "MOD_ERROR.GAME_VERSION_INVALID");
          locString.Add("id", modId);
          locString.Add("minGameVersion", mod.manifest.minGameVersion ?? "<null>");
          locStringList.Add(locString);
          Log.Error($"Mod {mod.manifest.id} declares min game version {mod.manifest.minGameVersion} that can't be parsed! Assuming it is supported");
          mod.state = ModLoadState.Failed;
        }
        else if (!flag3)
        {
          LocString locString = new LocString("main_menu_ui", "MOD_ERROR.GAME_VERSION_UNSUPPORTED");
          locString.Add("id", modId);
          locString.Add("minGameVersion", mod.manifest.minGameVersion ?? "<null>");
          locString.Add("gameVersion", ModManager._gameVersion?.ToString() ?? "<null>");
          locStringList.Add(locString);
          Log.Error($"Tried to load mod with id {modId}, but its declared min game version {mod.manifest.minGameVersion} is higher than the current game version {ModManager._gameVersion}");
          mod.state = ModLoadState.Failed;
        }
        else
        {
          List<string> values = new List<string>();
          if (mod.manifest.dependencies != null)
          {
            foreach (ModDependency dependency in mod.manifest.dependencies)
            {
              ModDependency declaredDependency = dependency;
              Mod mod1 = ModManager._mods.FirstOrDefault<Mod>((Func<Mod, bool>) (m => m.manifest?.id == declaredDependency.id));
              if (mod1 == null || mod1.state != ModLoadState.Loaded)
                values.Add(declaredDependency.id);
              else if (declaredDependency.minVersion != null)
              {
                SemanticVersion version;
                if (!SemanticVersion.TryFromString(declaredDependency.minVersion, out version))
                {
                  LocString locString = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_MIN_VERSION_INVALID");
                  locString.Add("id", mod.manifest.id);
                  locString.Add("dependency", declaredDependency.id);
                  locString.Add("minVersion", declaredDependency.minVersion);
                  locStringList.Add(locString);
                  Log.Error($"Mod {modId} which depends on {declaredDependency.id} with min version {declaredDependency.minVersion} which cannot be parsed");
                  mod.state = ModLoadState.Failed;
                }
                else if (mod1.manifest?.version == null)
                {
                  LocString locString = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_VERSION_MISSING");
                  locString.Add("id", mod.manifest.id);
                  locString.Add("dependency", declaredDependency.id);
                  locString.Add("minVersion", declaredDependency.minVersion);
                  locStringList.Add(locString);
                  Log.Error($"Tried to load mod {modId} which depends on {declaredDependency.id} with min version {declaredDependency.minVersion}, but the mod declares no version!");
                  mod.state = ModLoadState.Failed;
                }
                else if (mod1.version == null)
                {
                  LocString locString = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_VERSION_INVALID");
                  locString.Add("id", mod.manifest.id);
                  locString.Add("dependency", declaredDependency.id);
                  locString.Add("minVersion", declaredDependency.minVersion);
                  locString.Add("version", mod1.manifest.version);
                  locStringList.Add(locString);
                  Log.Error($"Tried to load mod {modId} which depends on {declaredDependency.id} with min version {declaredDependency.minVersion}, but the mod declares version {mod1.manifest.version} which cannot be parsed!");
                  mod.state = ModLoadState.Failed;
                }
                else if (mod1.version.CompareTo(version) < 0)
                {
                  LocString locString = new LocString("main_menu_ui", "MOD_ERROR.DEPENDENCY_VERSION_UNSUPPORTED");
                  locString.Add("id", mod.manifest.id);
                  locString.Add("dependency", declaredDependency.id);
                  locString.Add("minVersion", declaredDependency.minVersion);
                  locString.Add("version", mod1.manifest.version);
                  locStringList.Add(locString);
                  Log.Error($"Tried to load mod {modId} which depends on {declaredDependency.id} with min version {declaredDependency.minVersion} but you have {mod1.version}!");
                  mod.state = ModLoadState.Failed;
                }
              }
            }
            if (values.Count > 0)
            {
              string variable2 = string.Join(",", (IEnumerable<string>) values);
              LocString locString = new LocString("main_menu_ui", "MOD_ERROR.MISSING_DEPENDENCY");
              locString.Add("id", mod.manifest.id);
              locString.Add("missingCount", (Decimal) values.Count);
              locString.Add("missingDependencies", variable2);
              locStringList.Add(locString);
              Log.Error($"Tried to load mod {modId}, but it depends on mods which have not been loaded: {variable2}!");
              mod.state = ModLoadState.Failed;
            }
          }
        }
      }
      if (mod.state != ModLoadState.None)
      {
        mod.errors = locStringList.Count == 0 ? (List<LocString>) null : locStringList;
        ModManager.InvokeOnModDetected(mod);
      }
      else
      {
        try
        {
          bool flag4 = false;
          string path1 = Path.Combine(mod.path, modId + ".dll");
          if (mod.manifest.hasDll && TestMode.IsOff)
          {
            if (ModManager._fileIo != null && ModManager._fileIo.FileExists(path1))
            {
              Log.Info("Loading assembly DLL " + path1);
              AssemblyLoadContext loadContext = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
              if (loadContext != null)
              {
                assembly = loadContext.LoadFromAssemblyPath(path1);
                flag4 = true;
              }
            }
            else
              Log.Error($"Mod manifest for mod {mod.manifest.id} declares that it should load an assembly, but no assembly at path {path1} was found!");
          }
          else if (TestMode.IsOn && ModManager.TestInitializers.ContainsKey(modId))
            flag4 = true;
          string path2 = Path.Combine(mod.path, modId + ".pck");
          if (mod.manifest.hasPck && TestMode.IsOff)
          {
            if (ModManager._fileIo != null && ModManager._fileIo.FileExists(path2))
            {
              Log.Info("Loading Godot PCK " + path2);
              if (!ProjectSettings.LoadResourcePack(path2, true, 0))
                throw new InvalidOperationException($"Godot errored while loading PCK file {modId}!");
              flag4 = true;
            }
            else
              Log.Error($"Mod manifest for mod {mod.manifest.id} declares that it should load a PCK, but no PCK at path {path2} was found!");
          }
          if (!flag4)
            Log.Warn($"Neither a DLL nor a PCK was loaded for mod {mod.manifest.id}, something seems wrong!");
          bool? nullable1 = new bool?();
          if (TestMode.IsOff && Assembly.op_Inequality(assembly, (Assembly) null))
          {
            nullable1 = new bool?(true);
            List<Type> list = ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => CustomAttributeExtensions.GetCustomAttribute<ModInitializerAttribute>((MemberInfo) t) != null)).ToList<Type>();
            if (list.Count > 0)
            {
              foreach (Type initializerType in list)
              {
                Log.Info($"Calling initializer method of type {initializerType} for {assembly}");
                bool flag5 = ModManager.CallModInitializer(initializerType);
                nullable1 = new bool?(nullable1.Value & flag5);
              }
            }
            else
            {
              try
              {
                Log.Info($"No ModInitializerAttribute detected. Calling Harmony.PatchAll for {assembly}");
                new Harmony($"{mod.manifest.author ?? "unknown"}.{modId}").PatchAll(assembly);
              }
              catch (Exception ex)
              {
                Log.Error($"Exception caught while trying to run PatchAll on assembly {assembly}:\n{ex}");
                nullable1 = new bool?(false);
              }
            }
          }
          else if (TestMode.IsOn && mod.manifest.hasDll)
          {
            Action action;
            nullable1 = new bool?(ModManager.TestInitializers.TryGetValue(mod.manifest.id, out action));
            if (nullable1.Value)
              action();
          }
          bool? nullable2 = nullable1;
          bool flag6 = false;
          if (nullable2.GetValueOrDefault() == flag6 & nullable2.HasValue)
          {
            LocString locString = new LocString("main_menu_ui", "MOD_ERROR.ASSEMBLY_LOAD");
            locString.Add("id", mod.manifest.id);
            locStringList.Add(locString);
          }
          Log.Info($"Finished mod initialization for '{mod.manifest.name}' ({modId}).");
          if (Assembly.op_Inequality(assembly, (Assembly) null))
            mod.assemblies.Add(assembly);
          mod.state = ModLoadState.Loaded;
          mod.errors = locStringList.Count == 0 ? (List<LocString>) null : locStringList;
          ModManager.InvokeOnModDetected(mod);
        }
        catch (Exception ex)
        {
          Log.Error($"Exception thrown while loading mod {modId}: {ex}");
          LocString locString = new LocString("main_menu_ui", "MOD_ERROR.EXCEPTION");
          locString.Add("exceptionType", ex.GetType().ToString());
          locString.Add("id", mod.manifest.id);
          locStringList.Add(locString);
          if (Assembly.op_Inequality(assembly, (Assembly) null))
            mod.assemblies.Add(assembly);
          mod.state = ModLoadState.Failed;
          mod.errors = locStringList.Count == 0 ? (List<LocString>) null : locStringList;
          ModManager.InvokeOnModDetected(mod);
        }
      }
    }
  }

  private static void InvokeOnModDetected(Mod mod)
  {
    foreach (Delegate @delegate in ModManager.OnModDetected?.GetInvocationList() ?? Array.Empty<Delegate>())
    {
      try
      {
        @delegate.DynamicInvoke((object) mod);
      }
      catch (Exception ex)
      {
        if (Assembly.op_Equality(@delegate.Target?.GetType().Assembly, Assembly.GetExecutingAssembly()))
          throw;
        Log.Error($"Exception emitted from {"OnModDetected"} delegate {@delegate}: {ex}");
      }
    }
  }

  private static bool CallModInitializer(Type initializerType)
  {
    ModInitializerAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<ModInitializerAttribute>((MemberInfo) initializerType);
    MethodInfo method = initializerType.GetMethod(customAttribute.initializerMethod, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
    {
      if (MethodInfo.op_Inequality(initializerType.GetMethod(customAttribute.initializerMethod, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (MethodInfo) null))
        Log.Error($"Tried to call mod initializer {initializerType.Name}.{customAttribute.initializerMethod} but it's not static! Declare it to be static");
      else
        Log.Error($"Found mod initializer class of type {initializerType}, but it does not contain the method {customAttribute.initializerMethod} declared in the ModInitializerAttribute!");
      return false;
    }
    try
    {
      ((MethodBase) method).Invoke((object) null, (object[]) null);
    }
    catch (Exception ex)
    {
      Log.Error($"Exception thrown when calling mod initializer of type {initializerType}: {ex}");
      return false;
    }
    return true;
  }

  public static IEnumerable<string> GetModdedLocTables(string language, string file)
  {
    foreach (Mod mod in ModManager._mods)
    {
      if (mod.state == ModLoadState.Loaded)
      {
        string moddedLocTable = $"res://{mod.manifest.id}/localization/{language}/{file}";
        if (ResourceLoader.Exists(moddedLocTable, ""))
          yield return moddedLocTable;
      }
    }
  }

  public static List<string>? GetGameplayRelevantModNameList()
  {
    return !ModManager.IsRunningModded() ? (List<string>) null : ModManager.GetLoadedMods().Where<Mod>((Func<Mod, bool>) (m =>
    {
      ModManifest manifest = m.manifest;
      return (object) manifest == null || manifest.affectsGameplay;
    })).Select<Mod, string>((Func<Mod, string>) (m => $"{m.manifest?.id}-{m.manifest?.version}")).ToList<string>();
  }

  public static List<string>? GetNonGameplayRelevantModNameList()
  {
    return !ModManager.IsRunningModded() ? (List<string>) null : ModManager.GetLoadedMods().Where<Mod>((Func<Mod, bool>) (m =>
    {
      ModManifest manifest = m.manifest;
      return ((object) manifest != null ? (manifest.affectsGameplay ? 1 : 0) : 1) == 0;
    })).Select<Mod, string>((Func<Mod, string>) (m => $"{m.manifest?.id}-{m.manifest?.version}")).ToList<string>();
  }

  private static Assembly HandleAssemblyResolveFailure(object? source, ResolveEventArgs ev)
  {
    if (ev.Name.StartsWith("sts2,"))
    {
      Log.Info($"Failed to resolve assembly '{ev.Name}' but it looks like the STS2 assembly. Resolving using {Assembly.GetExecutingAssembly()}");
      return Assembly.GetExecutingAssembly();
    }
    if (!ev.Name.StartsWith("0Harmony,"))
      return (Assembly) null;
    Log.Info($"Failed to resolve assembly '{ev.Name}' but it looks like the Harmony assembly. Resolving using {typeof (Harmony).Assembly}");
    return typeof (Harmony).Assembly;
  }

  public static void CallMetricsHooks(SerializableRun run, bool isVictory, ulong localPlayerId)
  {
    ModManager.MetricsUploadHook onMetricsUpload = ModManager.OnMetricsUpload;
    if (onMetricsUpload == null)
      return;
    onMetricsUpload(run, isVictory, localPlayerId);
  }

  public static void AssociateAssemblyWithMod(string modId, Assembly assembly)
  {
    Mod mod1 = ModManager._mods.FirstOrDefault<Mod>((Func<Mod, bool>) (m =>
    {
      bool flag;
      switch (m.state)
      {
        case ModLoadState.None:
        case ModLoadState.Loaded:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag && m.manifest?.id == modId;
    }));
    if (mod1 == null)
    {
      Mod mod2 = ModManager._mods.FirstOrDefault<Mod>((Func<Mod, bool>) (m => m.manifest?.id == modId));
      if (mod2 != null)
        Log.Warn($"Tried to associate assembly {assembly} with mod {modId} but its state is {mod2.state}");
      else
        Log.Warn($"Tried to associate assembly {assembly} with mod {modId} but we couldn't find any such mod");
    }
    else
    {
      Log.Info($"Associated assembly {assembly} with mod {modId}");
      mod1.assemblies.Add(assembly);
      if (AssemblyInfo.ModMap == null)
        return;
      Log.Error($"Assembly {assembly} was associated with mod {mod1.manifest?.id} after {"AssemblyInfo"} has already been initialized. The types will not be included in multiplayer maps.");
      AssemblyInfo.ModMap[assembly] = mod1;
    }
  }

  public static bool IsRunningModded()
  {
    return ModManager._mods.Any<Mod>((Func<Mod, bool>) (m =>
    {
      bool flag;
      switch (m.state)
      {
        case ModLoadState.Loaded:
        case ModLoadState.Failed:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }));
  }

  public static bool HasHarmonyPatches()
  {
    try
    {
      ModManager._hasHarmonyPatches.GetValueOrDefault();
      if (!ModManager._hasHarmonyPatches.HasValue)
        ModManager._hasHarmonyPatches = new bool?(Harmony.GetAllPatchedMethods().Any<MethodBase>());
    }
    catch
    {
      ModManager._hasHarmonyPatches = new bool?(true);
    }
    return ModManager._hasHarmonyPatches.Value;
  }

  public static IEnumerable<Mod> GetLoadedMods()
  {
    return ModManager._mods.Where<Mod>((Func<Mod, bool>) (m => m.state == ModLoadState.Loaded));
  }

  public static void Dispose() => ModManager._steamItemInstalledCallback?.Dispose();

  public static void CopyUnmoddedSaveFilesIfNeeded()
  {
    string accountScopedBasePath1 = UserDataPathProvider.GetAccountScopedBasePath((string) null);
    string accountScopedBasePath2 = UserDataPathProvider.GetAccountScopedBasePath("modded/");
    string profileSavePath1 = ProfileSaveManager.GetProfileSavePath(new bool?(false));
    string profileSavePath2 = ProfileSaveManager.GetProfileSavePath(new bool?(true));
    if (DirAccess.DirExistsAbsolute(accountScopedBasePath2))
    {
      if (!FileAccess.FileExists(StringExtensions.PathJoin(accountScopedBasePath1, profileSavePath2)))
      {
        Log.Info("Modded saves exist, but profile.save wasn't present. Copying profile.save from unmodded to modded");
        ModManager.Copy(accountScopedBasePath1, profileSavePath1, profileSavePath2);
      }
      Log.Info("Modded saves exist. Skipping first-time save copy");
    }
    else
    {
      DirAccess.MakeDirRecursiveAbsolute(accountScopedBasePath2);
      if (!FileAccess.FileExists(StringExtensions.PathJoin(accountScopedBasePath1, profileSavePath1)))
      {
        Log.Info("Modded saves don't exist, but neither do unmodded saves. Skipping first-time copy");
      }
      else
      {
        Log.Info("Copying all unmodded saves to the modded save location. Base path: " + accountScopedBasePath1);
        ModManager.Copy(accountScopedBasePath1, profileSavePath1, profileSavePath2);
        for (int profileId = 1; profileId <= 3; ++profileId)
        {
          string progressPathForProfile1 = ProgressSaveManager.GetProgressPathForProfile(profileId, new bool?(false));
          string progressPathForProfile2 = ProgressSaveManager.GetProgressPathForProfile(profileId, new bool?(true));
          ModManager.Copy(accountScopedBasePath1, progressPathForProfile1, progressPathForProfile2);
          string runSavePath1 = RunSaveManager.GetRunSavePath(profileId, "current_run.save", new bool?(false));
          string runSavePath2 = RunSaveManager.GetRunSavePath(profileId, "current_run.save", new bool?(true));
          ModManager.Copy(accountScopedBasePath1, runSavePath1, runSavePath2);
          string runSavePath3 = RunSaveManager.GetRunSavePath(profileId, "current_run_mp.save", new bool?(false));
          string runSavePath4 = RunSaveManager.GetRunSavePath(profileId, "current_run_mp.save", new bool?(true));
          ModManager.Copy(accountScopedBasePath1, runSavePath3, runSavePath4);
          string prefsPath1 = PrefsSaveManager.GetPrefsPath(profileId, new bool?(false));
          string prefsPath2 = PrefsSaveManager.GetPrefsPath(profileId, new bool?(true));
          ModManager.Copy(accountScopedBasePath1, prefsPath1, prefsPath2);
          string historyPath1 = RunHistorySaveManager.GetHistoryPath(profileId, new bool?(false));
          DirAccess dirAccess = DirAccess.Open(StringExtensions.PathJoin(accountScopedBasePath1, historyPath1));
          if (dirAccess != null)
          {
            string historyPath2 = RunHistorySaveManager.GetHistoryPath(profileId, new bool?(true));
            DirAccess.MakeDirRecursiveAbsolute(StringExtensions.PathJoin(accountScopedBasePath1, historyPath2));
            foreach (string file in dirAccess.GetFiles())
            {
              string sourceFile = StringExtensions.PathJoin(historyPath1, file);
              string targetFile = StringExtensions.PathJoin(historyPath2, file);
              ModManager.Copy(accountScopedBasePath1, sourceFile, targetFile);
            }
          }
        }
        ModManager.UnmoddedSavesWereCopied = true;
      }
    }
  }

  public static void Copy(string baseDir, string sourceFile, string targetFile)
  {
    string str1 = StringExtensions.PathJoin(baseDir, sourceFile);
    if (!FileAccess.FileExists(str1))
      return;
    string str2 = StringExtensions.PathJoin(baseDir, targetFile);
    Log.Info($"Copying {sourceFile} -> {targetFile}");
    DirAccess.MakeDirRecursiveAbsolute(StringExtensions.GetBaseDir(str2));
    Error error = DirAccess.CopyAbsolute(str1, str2, -1);
    if (error == null)
      return;
    Log.Error($"Error: {error}");
  }

  public delegate void MetricsUploadHook(SerializableRun run, bool isVictory, ulong localPlayerId);
}
