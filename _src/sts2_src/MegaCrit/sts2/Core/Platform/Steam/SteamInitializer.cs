// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Steam.SteamInitializer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using Steamworks;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Steam;

public static class SteamInitializer
{
  public const ulong steamAppId = 2868840;
  private static CancellationTokenSource _disconnectCts = new CancellationTokenSource();

  public static bool Initialized { get; private set; }

  public static ESteamAPIInitResult? InitResult { get; private set; }

  public static string? InitErrorMessage { get; private set; }

  public static CancellationToken DisconnectToken => SteamInitializer._disconnectCts.Token;

  public static event Action? SteamNoLongerRunning;

  private static IntPtr SteamDebugResolver(
    string libraryName,
    Assembly assembly,
    DllImportSearchPath? searchPath)
  {
    if (libraryName != "steam_api" && libraryName != "steam_api64" || !OS.HasFeature("editor"))
      return IntPtr.Zero;
    string name = OS.GetName();
    string str1;
    if (name != null)
    {
      switch (name.Length)
      {
        case 3:
          if (name == "BSD")
            break;
          goto label_16;
        case 5:
          switch (name[0])
          {
            case 'L':
              if (name == "Linux")
                break;
              goto label_16;
            case 'm':
              if (name == "macOS")
              {
                str1 = "steam/SteamApi/libsteam_api.dylib";
                goto label_17;
              }
              goto label_16;
            default:
              goto label_16;
          }
          break;
        case 6:
          if (name == "NetBSD")
            break;
          goto label_16;
        case 7:
          switch (name[0])
          {
            case 'F':
              if (name == "FreeBSD")
                break;
              goto label_16;
            case 'O':
              if (name == "OpenBSD")
                break;
              goto label_16;
            case 'W':
              if (name == "Windows")
              {
                str1 = "steam/SteamApi/steam_api64.dll";
                goto label_17;
              }
              goto label_16;
            default:
              goto label_16;
          }
          break;
        default:
          goto label_16;
      }
      str1 = "steam/SteamApi/libsteam_api.so";
      goto label_17;
    }
label_16:
    str1 = (string) null;
label_17:
    string str2 = str1;
    return str2 != null ? NativeLibrary.Load(str2) : IntPtr.Zero;
  }

  public static bool Initialize(Node node)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: method pointer
    NativeLibrary.SetDllImportResolver(Assembly.GetAssembly(typeof (SteamAPI)), SteamInitializer.\u003C\u003EO.\u003C0\u003E__SteamDebugResolver ?? (SteamInitializer.\u003C\u003EO.\u003C0\u003E__SteamDebugResolver = new DllImportResolver((object) null, __methodptr(SteamDebugResolver))));
    Log.Info("Steamworks: attempting initialization...");
    for (int index = 0; index < 3; ++index)
    {
      if (index > 0)
      {
        Thread.Sleep(100);
        Log.Info($"Steam initialization retry attempt {index}");
      }
      SteamInitializer.Initialized = SteamInitializer.InitializeInternal();
      if (SteamInitializer.Initialized)
        break;
    }
    if (SteamInitializer.Initialized)
    {
      SteamNetworkingUtils.InitRelayNetworkAccess();
      TaskHelper.RunSafely(SteamInitializer.RunCallbacksAsync(node));
    }
    return SteamInitializer.Initialized;
  }

  private static bool InitializeInternal()
  {
    try
    {
      Log.Info($"Steam is running: {SteamAPI.IsSteamRunning()}");
      string str;
      SteamInitializer.InitResult = new ESteamAPIInitResult?(SteamAPI.InitEx(ref str));
      SteamInitializer.InitErrorMessage = str;
      ESteamAPIInitResult? initResult = SteamInitializer.InitResult;
      ESteamAPIInitResult esteamApiInitResult = (ESteamAPIInitResult) 0;
      if (initResult.GetValueOrDefault() == esteamApiInitResult & initResult.HasValue)
      {
        Log.Info("Steamworks initialization succeeded!");
        return true;
      }
      Log.Error($"Steamworks initialization failed! Result: {SteamInitializer.InitResult}, message: {SteamInitializer.InitErrorMessage}");
    }
    catch (Exception ex)
    {
      Log.Error($"Steamworks initialization threw an exception: {ex}");
      SteamInitializer.InitResult = new ESteamAPIInitResult?();
      SteamInitializer.InitErrorMessage = ex.Message;
    }
    return false;
  }

  private static async Task RunCallbacksAsync(Node node)
  {
    // ISSUE: unable to decompile the method.
  }

  public static void Uninitialize()
  {
    if (!SteamInitializer.Initialized)
      return;
    Log.Info("Steamworks: shutting down...");
    try
    {
      SteamAPI.Shutdown();
      Log.Info("Steamworks shutdown succeeded!");
    }
    catch (Exception ex)
    {
      Log.Error($"Steamworks shutdown threw an exception: {ex}");
    }
    finally
    {
      SteamInitializer.Initialized = false;
    }
  }
}
