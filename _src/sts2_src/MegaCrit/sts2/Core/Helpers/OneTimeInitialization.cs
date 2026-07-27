// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.OneTimeInitialization
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using SmartFormat.Extensions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class OneTimeInitialization
{
  private static AtlasResourceLoader? _atlasResourceLoader;
  private static OneTimeInitialization.State _state;

  public static ReadSaveResult<SettingsSave> SettingsReadResult { get; private set; }

  public static async Task ExecuteVeryEarly()
  {
    if (OneTimeInitialization._state != OneTimeInitialization.State.None)
    {
      Log.Error($"Tried to call OneTimeInitialization.ExecuteVeryEarly in state {OneTimeInitialization._state}!");
    }
    else
    {
      OneTimeInitialization._state = OneTimeInitialization.State.VeryEarly;
      OneTimeInitialization.SettingsReadResult = !TestMode.IsOn ? SaveManager.Instance.InitSettingsData() : SaveManager.Instance.InitSettingsDataForTest();
      await ModManager.Initialize((IModManagerFileIo) new ModManagerFileIo(), SaveManager.Instance.SettingsSave.ModSettings, ReleaseInfoManager.Instance.SemVer);
      UserDataPathProvider.IsRunningModded = ModManager.IsRunningModded();
      SentryService.DisableSentryIfModded();
    }
  }

  public static void ExecuteEssential()
  {
    if (OneTimeInitialization._state != OneTimeInitialization.State.VeryEarly)
    {
      Log.Error($"Tried to call OneTimeInitialization.ExecuteEssential in state {OneTimeInitialization._state}!");
    }
    else
    {
      OneTimeInitialization._state = OneTimeInitialization.State.Essential;
      OneTimeInitialization._atlasResourceLoader = new AtlasResourceLoader();
      ResourceLoader.AddResourceFormatLoader((ResourceFormatLoader) OneTimeInitialization._atlasResourceLoader, true);
      AtlasManager.LoadEssentialAtlases();
      LocManager.Initialize();
      AssemblyInfo.Init();
      ModelDb.Init();
      ModelIdSerializationCache.Init();
      ModelDb.InitIds();
      MessageTypes.Initialize();
      ActionTypes.Initialize();
    }
  }

  public static void ExecuteDeferred()
  {
    if (OneTimeInitialization._state != OneTimeInitialization.State.Essential)
    {
      Log.Error($"Tried to call OneTimeInitialization.ExecuteDeferred in state {OneTimeInitialization._state}!");
    }
    else
    {
      OneTimeInitialization._state = OneTimeInitialization.State.Done;
      AtlasManager.LoadAllAtlases();
      if (OS.HasFeature("editor"))
        return;
      ModelDb.Preload();
      OneTimeInitialization.PrewarmJit();
      ConditionalFormatter conditionalFormatter = new ConditionalFormatter();
    }
  }

  private static void PrewarmJit()
  {
    Type type1 = typeof (PacketWriter);
    Type type2 = typeof (PacketReader);
    foreach (Type subtype in ReflectionHelper.GetSubtypes<IPacketSerializable>())
    {
      RuntimeHelpers.PrepareMethod(subtype.GetMethod("Serialize").MethodHandle);
      RuntimeHelpers.PrepareMethod(subtype.GetMethod("Deserialize").MethodHandle);
      RuntimeHelpers.PrepareMethod(type1.GetMethod("WriteList").MethodHandle, new RuntimeTypeHandle[1]
      {
        subtype.TypeHandle
      });
      RuntimeHelpers.PrepareMethod(type1.GetMethod("Write").MethodHandle, new RuntimeTypeHandle[1]
      {
        subtype.TypeHandle
      });
      RuntimeHelpers.PrepareMethod(type2.GetMethod("ReadList").MethodHandle, new RuntimeTypeHandle[1]
      {
        subtype.TypeHandle
      });
      RuntimeHelpers.PrepareMethod(type2.GetMethod("Read").MethodHandle, new RuntimeTypeHandle[1]
      {
        subtype.TypeHandle
      });
    }
    foreach (Type subtype in ReflectionHelper.GetSubtypes<INetMessage>())
    {
      RuntimeHelpers.PrepareMethod(subtype.GetMethod("get_ShouldBuffer").MethodHandle);
      RuntimeHelpers.PrepareMethod(subtype.GetMethod("get_LogLevel").MethodHandle);
      RuntimeHelpers.PrepareMethod(subtype.GetMethod("get_Mode").MethodHandle);
      RuntimeHelpers.PrepareMethod(subtype.GetMethod("get_ShouldBroadcast").MethodHandle);
    }
  }

  private enum State
  {
    None,
    VeryEarly,
    Essential,
    Done,
  }
}
