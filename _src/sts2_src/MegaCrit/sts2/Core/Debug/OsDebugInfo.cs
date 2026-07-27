// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.OsDebugInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Logging;
using SharpGen.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vortice.DXGI;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public static class OsDebugInfo
{
  public static async Task LogSystemInfo()
  {
    if (ReleaseInfoManager.Instance.ReleaseInfo == null)
      ;
    else
    {
      string systemInfo = "Fetching system info failed!";
      string str = await Task.Run<string>((Func<string>) (() => systemInfo = OsDebugInfo.GetSystemInfoString()));
      Log.Info(systemInfo);
    }
  }

  public static string GetSystemInfoString()
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.AppendLine("=== Godot OS Debug Information ===");
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 1, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Timestamp: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<DateTime>(DateTime.Now);
    ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    stringBuilder3.AppendLine(ref local1);
    StringBuilder stringBuilder4 = stringBuilder1;
    StringBuilder stringBuilder5 = stringBuilder4;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(4, 1, stringBuilder4);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("OS: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetName());
    ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
    stringBuilder5.AppendLine(ref local2);
    StringBuilder stringBuilder6 = stringBuilder1;
    StringBuilder stringBuilder7 = stringBuilder6;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(12, 1, stringBuilder6);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("OS Version: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetVersion());
    ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
    stringBuilder7.AppendLine(ref local3);
    StringBuilder stringBuilder8 = stringBuilder1;
    StringBuilder stringBuilder9 = stringBuilder8;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(19, 1, stringBuilder8);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Distribution Name: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetDistributionName());
    ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
    stringBuilder9.AppendLine(ref local4);
    StringBuilder stringBuilder10 = stringBuilder1;
    StringBuilder stringBuilder11 = stringBuilder10;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(14, 1, stringBuilder10);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Device Model: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetModelName());
    ref StringBuilder.AppendInterpolatedStringHandler local5 = ref interpolatedStringHandler;
    stringBuilder11.AppendLine(ref local5);
    StringBuilder stringBuilder12 = stringBuilder1;
    StringBuilder stringBuilder13 = stringBuilder12;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(16 /*0x10*/, 1, stringBuilder12);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Is Debug Build: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(OS.IsDebugBuild());
    ref StringBuilder.AppendInterpolatedStringHandler local6 = ref interpolatedStringHandler;
    stringBuilder13.AppendLine(ref local6);
    StringBuilder stringBuilder14 = stringBuilder1;
    StringBuilder stringBuilder15 = stringBuilder14;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(14, 1, stringBuilder14);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Is Sandboxed: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(OS.IsSandboxed());
    ref StringBuilder.AppendInterpolatedStringHandler local7 = ref interpolatedStringHandler;
    stringBuilder15.AppendLine(ref local7);
    StringBuilder stringBuilder16 = stringBuilder1;
    StringBuilder stringBuilder17 = stringBuilder16;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(17, 1, stringBuilder16);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Executable Path: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetExecutablePath());
    ref StringBuilder.AppendInterpolatedStringHandler local8 = ref interpolatedStringHandler;
    stringBuilder17.AppendLine(ref local8);
    StringBuilder stringBuilder18 = stringBuilder1;
    StringBuilder stringBuilder19 = stringBuilder18;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(16 /*0x10*/, 1, stringBuilder18);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Data Directory: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetDataDir());
    ref StringBuilder.AppendInterpolatedStringHandler local9 = ref interpolatedStringHandler;
    stringBuilder19.AppendLine(ref local9);
    StringBuilder stringBuilder20 = stringBuilder1;
    StringBuilder stringBuilder21 = stringBuilder20;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(21, 1, stringBuilder20);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("User Data Directory: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetUserDataDir());
    ref StringBuilder.AppendInterpolatedStringHandler local10 = ref interpolatedStringHandler;
    stringBuilder21.AppendLine(ref local10);
    StringBuilder stringBuilder22 = stringBuilder1;
    StringBuilder stringBuilder23 = stringBuilder22;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(19, 1, stringBuilder22);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Command Line Args: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join(" ", OS.GetCmdlineArgs()));
    ref StringBuilder.AppendInterpolatedStringHandler local11 = ref interpolatedStringHandler;
    stringBuilder23.AppendLine(ref local11);
    StringBuilder stringBuilder24 = stringBuilder1;
    StringBuilder stringBuilder25 = stringBuilder24;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(24, 1, stringBuilder24);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("User Command Line Args: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join(" ", OS.GetCmdlineUserArgs()));
    ref StringBuilder.AppendInterpolatedStringHandler local12 = ref interpolatedStringHandler;
    stringBuilder25.AppendLine(ref local12);
    StringBuilder stringBuilder26 = stringBuilder1;
    StringBuilder stringBuilder27 = stringBuilder26;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 1, stringBuilder26);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("OS Locale: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetLocale());
    ref StringBuilder.AppendInterpolatedStringHandler local13 = ref interpolatedStringHandler;
    stringBuilder27.AppendLine(ref local13);
    StringBuilder stringBuilder28 = stringBuilder1;
    StringBuilder stringBuilder29 = stringBuilder28;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(13, 1, stringBuilder28);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("OS Language: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetLocaleLanguage());
    ref StringBuilder.AppendInterpolatedStringHandler local14 = ref interpolatedStringHandler;
    stringBuilder29.AppendLine(ref local14);
    StringBuilder stringBuilder30 = stringBuilder1;
    StringBuilder stringBuilder31 = stringBuilder30;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(13, 1, stringBuilder30);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Game Locale: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(TranslationServer.GetLocale());
    ref StringBuilder.AppendInterpolatedStringHandler local15 = ref interpolatedStringHandler;
    stringBuilder31.AppendLine(ref local15);
    StringBuilder stringBuilder32 = stringBuilder1;
    StringBuilder stringBuilder33 = stringBuilder32;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(22, 1, stringBuilder32);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Is UserFS Persistent: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(OS.IsUserfsPersistent());
    ref StringBuilder.AppendInterpolatedStringHandler local16 = ref interpolatedStringHandler;
    stringBuilder33.AppendLine(ref local16);
    StringBuilder stringBuilder34 = stringBuilder1;
    StringBuilder stringBuilder35 = stringBuilder34;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(19, 1, stringBuilder34);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Is Stdout Verbose: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(OS.IsStdOutVerbose());
    ref StringBuilder.AppendInterpolatedStringHandler local17 = ref interpolatedStringHandler;
    stringBuilder35.AppendLine(ref local17);
    StringBuilder stringBuilder36 = stringBuilder1;
    StringBuilder stringBuilder37 = stringBuilder36;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(29, 1, stringBuilder36);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Is Low Processor Usage Mode: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(OS.IsInLowProcessorUsageMode());
    ref StringBuilder.AppendInterpolatedStringHandler local18 = ref interpolatedStringHandler;
    stringBuilder37.AppendLine(ref local18);
    StringBuilder stringBuilder38 = stringBuilder1;
    StringBuilder stringBuilder39 = stringBuilder38;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(14, 1, stringBuilder38);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Architecture: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(Engine.GetArchitectureName());
    ref StringBuilder.AppendInterpolatedStringHandler local19 = ref interpolatedStringHandler;
    stringBuilder39.AppendLine(ref local19);
    StringBuilder stringBuilder40 = stringBuilder1;
    StringBuilder stringBuilder41 = stringBuilder40;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(16 /*0x10*/, 1, stringBuilder40);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Engine Version: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<Variant>(Engine.GetVersionInfo()[Variant.op_Implicit("string")]);
    ref StringBuilder.AppendInterpolatedStringHandler local20 = ref interpolatedStringHandler;
    stringBuilder41.AppendLine(ref local20);
    StringBuilder stringBuilder42 = stringBuilder1;
    StringBuilder stringBuilder43 = stringBuilder42;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 1, stringBuilder42);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Is Editor: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(Engine.IsEditorHint());
    ref StringBuilder.AppendInterpolatedStringHandler local21 = ref interpolatedStringHandler;
    stringBuilder43.AppendLine(ref local21);
    ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
    if (releaseInfo != null)
    {
      StringBuilder stringBuilder44 = stringBuilder1;
      StringBuilder stringBuilder45 = stringBuilder44;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(17, 1, stringBuilder44);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Release Version: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(releaseInfo.Version);
      ref StringBuilder.AppendInterpolatedStringHandler local22 = ref interpolatedStringHandler;
      stringBuilder45.AppendLine(ref local22);
      StringBuilder stringBuilder46 = stringBuilder1;
      StringBuilder stringBuilder47 = stringBuilder46;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(16 /*0x10*/, 1, stringBuilder46);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Release Commit: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(releaseInfo.Commit);
      ref StringBuilder.AppendInterpolatedStringHandler local23 = ref interpolatedStringHandler;
      stringBuilder47.AppendLine(ref local23);
      StringBuilder stringBuilder48 = stringBuilder1;
      StringBuilder stringBuilder49 = stringBuilder48;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(14, 1, stringBuilder48);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Release Date: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<DateTime>(releaseInfo.Date);
      ref StringBuilder.AppendInterpolatedStringHandler local24 = ref interpolatedStringHandler;
      stringBuilder49.AppendLine(ref local24);
    }
    StringBuilder stringBuilder50 = stringBuilder1;
    StringBuilder stringBuilder51 = stringBuilder50;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(17, 1, stringBuilder50);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Processor Count: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(OS.GetProcessorCount());
    ref StringBuilder.AppendInterpolatedStringHandler local25 = ref interpolatedStringHandler;
    stringBuilder51.AppendLine(ref local25);
    StringBuilder stringBuilder52 = stringBuilder1;
    StringBuilder stringBuilder53 = stringBuilder52;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(16 /*0x10*/, 1, stringBuilder52);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Processor Name: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetProcessorName());
    ref StringBuilder.AppendInterpolatedStringHandler local26 = ref interpolatedStringHandler;
    stringBuilder53.AppendLine(ref local26);
    StringBuilder stringBuilder54 = stringBuilder1;
    StringBuilder stringBuilder55 = stringBuilder54;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(20, 1, stringBuilder54);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Main assembly hash: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(AssemblyHasher.GetMainAssemblyHash());
    ref StringBuilder.AppendInterpolatedStringHandler local27 = ref interpolatedStringHandler;
    stringBuilder55.AppendLine(ref local27);
    RenderingDevice renderingDevice = RenderingServer.GetRenderingDevice();
    StringBuilder stringBuilder56 = stringBuilder1;
    StringBuilder stringBuilder57 = stringBuilder56;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(23, 1, stringBuilder56);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Rendering device name: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(renderingDevice?.GetDeviceName() ?? "N/A (headless)");
    ref StringBuilder.AppendInterpolatedStringHandler local28 = ref interpolatedStringHandler;
    stringBuilder57.AppendLine(ref local28);
    IDXGIFactory1 idxgiFactory1;
    if (Result.op_Equality(Vortice.DXGI.DXGI.CreateDXGIFactory1<IDXGIFactory1>(ref idxgiFactory1), Result.Ok))
    {
      IDXGIAdapter1 idxgiAdapter1;
      for (uint index = 0; Result.op_Equality(idxgiFactory1.EnumAdapters1(index, ref idxgiAdapter1), Result.Ok); ++index)
      {
        long num;
        ((IDXGIAdapter) idxgiAdapter1).CheckInterfaceSupport(typeof (IDXGIDevice), ref num);
        StringBuilder stringBuilder58 = stringBuilder1;
        StringBuilder stringBuilder59 = stringBuilder58;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(21, 2, stringBuilder58);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Graphics adapter ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<uint>(index);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(((IDXGIAdapter) idxgiAdapter1).Description.Description);
        ref StringBuilder.AppendInterpolatedStringHandler local29 = ref interpolatedStringHandler;
        stringBuilder59.AppendLine(ref local29);
        StringBuilder stringBuilder60 = stringBuilder1;
        \u003C\u003Ey__InlineArray4<object> buffer = new \u003C\u003Ey__InlineArray4<object>();
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray4<object>, object>(ref buffer, 0) = (object) (num >> 48 /*0x30*/);
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray4<object>, object>(ref buffer, 1) = (object) (num >> 32 /*0x20*/ & (long) ushort.MaxValue);
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray4<object>, object>(ref buffer, 2) = (object) (num >> 16 /*0x10*/ & (long) ushort.MaxValue);
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.InlineArrayElementRef<\u003C\u003Ey__InlineArray4<object>, object>(ref buffer, 3) = (object) (num >> 32 /*0x20*/ & (long) ushort.MaxValue);
        // ISSUE: reference to a compiler-generated method
        string str = string.Format("  version: {0}.{1}.{2}.{3}", \u003CPrivateImplementationDetails\u003E.InlineArrayAsReadOnlySpan<\u003C\u003Ey__InlineArray4<object>, object>(in buffer, 4));
        stringBuilder60.AppendLine(str);
      }
    }
    StringBuilder stringBuilder61 = stringBuilder1;
    StringBuilder stringBuilder62 = stringBuilder61;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(23, 1, stringBuilder61);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Screen info (primary ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(DisplayServer.GetPrimaryScreen());
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("):");
    ref StringBuilder.AppendInterpolatedStringHandler local30 = ref interpolatedStringHandler;
    stringBuilder62.AppendLine(ref local30);
    for (int index = 0; index < DisplayServer.GetScreenCount(); ++index)
    {
      StringBuilder stringBuilder63 = stringBuilder1;
      StringBuilder stringBuilder64 = stringBuilder63;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(59, 6, stringBuilder63);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Index ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(index);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": Size: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<Vector2I>(DisplayServer.ScreenGetSize(index));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Orientation: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<DisplayServer.ScreenOrientation>(DisplayServer.ScreenGetOrientation(index));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Scale: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<float>(DisplayServer.ScreenGetScale(index));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" DPI: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(DisplayServer.ScreenGetDpi(-1));
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Refresh Rate: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<float>(DisplayServer.ScreenGetRefreshRate(-1));
      ref StringBuilder.AppendInterpolatedStringHandler local31 = ref interpolatedStringHandler;
      stringBuilder64.AppendLine(ref local31);
    }
    ulong renderingInfo = RenderingServer.GetRenderingInfo((RenderingServer.RenderingInfo) 5L);
    StringBuilder stringBuilder65 = stringBuilder1;
    StringBuilder stringBuilder66 = stringBuilder65;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(19, 1, stringBuilder65);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Video Memory Used: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes(renderingInfo));
    ref StringBuilder.AppendInterpolatedStringHandler local32 = ref interpolatedStringHandler;
    stringBuilder66.AppendLine(ref local32);
    Dictionary memoryInfo = OS.GetMemoryInfo();
    if (memoryInfo.Count > 0)
    {
      stringBuilder1.AppendLine("Memory Info:");
      foreach (KeyValuePair<Variant, Variant> keyValuePair in memoryInfo)
      {
        StringBuilder stringBuilder67 = stringBuilder1;
        StringBuilder stringBuilder68 = stringBuilder67;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder67);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<Variant>(keyValuePair.Key);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes(Variant.op_Explicit(keyValuePair.Value)));
        ref StringBuilder.AppendInterpolatedStringHandler local33 = ref interpolatedStringHandler;
        stringBuilder68.AppendLine(ref local33);
      }
    }
    StringBuilder stringBuilder69 = stringBuilder1;
    StringBuilder stringBuilder70 = stringBuilder69;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(23, 1, stringBuilder69);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Static Memory Usage: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes(OS.GetStaticMemoryUsage()));
    ref StringBuilder.AppendInterpolatedStringHandler local34 = ref interpolatedStringHandler;
    stringBuilder70.AppendLine(ref local34);
    StringBuilder stringBuilder71 = stringBuilder1;
    StringBuilder stringBuilder72 = stringBuilder71;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(28, 1, stringBuilder71);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Static Memory Peak Usage: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes(OS.GetStaticMemoryPeakUsage()));
    ref StringBuilder.AppendInterpolatedStringHandler local35 = ref interpolatedStringHandler;
    stringBuilder72.AppendLine(ref local35);
    try
    {
      stringBuilder1.AppendLine("Disk Info:");
      DriveInfo[] drives = DriveInfo.GetDrives();
      for (int index = 0; index < drives.Length; ++index)
      {
        DriveInfo driveInfo = drives[index];
        bool flag;
        switch (driveInfo.DriveType)
        {
          case DriveType.Removable:
          case DriveType.Fixed:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        if (flag)
        {
          StringBuilder stringBuilder73 = stringBuilder1;
          StringBuilder stringBuilder74 = stringBuilder73;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 3, stringBuilder73);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Index ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(index);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": Name: ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(driveInfo.Name);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Type: ");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<DriveType>(driveInfo.DriveType);
          ref StringBuilder.AppendInterpolatedStringHandler local36 = ref interpolatedStringHandler;
          stringBuilder74.Append(ref local36);
          if (driveInfo.IsReady)
          {
            StringBuilder stringBuilder75 = stringBuilder1;
            StringBuilder stringBuilder76 = stringBuilder75;
            interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(47, 4, stringBuilder75);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" Format: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(driveInfo.DriveFormat);
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Available: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes((ulong) driveInfo.AvailableFreeSpace));
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Total Free: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes((ulong) driveInfo.TotalFreeSpace));
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Total Size: ");
            ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OsDebugInfo.FormatBytes((ulong) driveInfo.TotalSize));
            ref StringBuilder.AppendInterpolatedStringHandler local37 = ref interpolatedStringHandler;
            stringBuilder76.AppendLine(ref local37);
          }
          else
            stringBuilder1.AppendLine();
        }
      }
    }
    catch (Exception ex)
    {
      Log.Warn($"Couldn't get disk info: {ex}");
      stringBuilder1.AppendLine("Couldn't get disk info");
    }
    string[] grantedPermissions = OS.GetGrantedPermissions();
    if (grantedPermissions.Length != 0)
    {
      stringBuilder1.AppendLine("Granted Permissions:");
      foreach (string str in grantedPermissions)
      {
        StringBuilder stringBuilder77 = stringBuilder1;
        StringBuilder stringBuilder78 = stringBuilder77;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder77);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str);
        ref StringBuilder.AppendInterpolatedStringHandler local38 = ref interpolatedStringHandler;
        stringBuilder78.AppendLine(ref local38);
      }
    }
    string[] strArray = new string[5]
    {
      "PATH",
      "GODOT_ROOT_DIR",
      "HOME",
      "DYLD_LIBRARY_PATH",
      "LD_LIBRARY_PATH"
    };
    stringBuilder1.AppendLine("Important Environment Variables:");
    foreach (string str in strArray)
    {
      StringBuilder stringBuilder79 = stringBuilder1;
      StringBuilder stringBuilder80 = stringBuilder79;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(4, 2, stringBuilder79);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(": ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(LogSanitizer.Sanitize(OS.GetEnvironment(str)));
      ref StringBuilder.AppendInterpolatedStringHandler local39 = ref interpolatedStringHandler;
      stringBuilder80.AppendLine(ref local39);
    }
    if (OS.GetName() == "Linux")
    {
      stringBuilder1.AppendLine("Linux Environment:");
      bool flag = !string.IsNullOrEmpty(OS.GetEnvironment("STEAM_COMPAT_DATA_PATH"));
      StringBuilder stringBuilder81 = stringBuilder1;
      StringBuilder stringBuilder82 = stringBuilder81;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(24, 1, stringBuilder81);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  Running under Proton: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(flag);
      ref StringBuilder.AppendInterpolatedStringHandler local40 = ref interpolatedStringHandler;
      stringBuilder82.AppendLine(ref local40);
      StringBuilder stringBuilder83 = stringBuilder1;
      StringBuilder stringBuilder84 = stringBuilder83;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(18, 1, stringBuilder83);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  PROTON_VERSION: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetEnvironment("PROTON_VERSION"));
      ref StringBuilder.AppendInterpolatedStringHandler local41 = ref interpolatedStringHandler;
      stringBuilder84.AppendLine(ref local41);
      StringBuilder stringBuilder85 = stringBuilder1;
      StringBuilder stringBuilder86 = stringBuilder85;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(18, 1, stringBuilder85);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  WINEPREFIX set: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(!string.IsNullOrEmpty(OS.GetEnvironment("WINEPREFIX")));
      ref StringBuilder.AppendInterpolatedStringHandler local42 = ref interpolatedStringHandler;
      stringBuilder86.AppendLine(ref local42);
      StringBuilder stringBuilder87 = stringBuilder1;
      StringBuilder stringBuilder88 = stringBuilder87;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(20, 1, stringBuilder87);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  XDG_SESSION_TYPE: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetEnvironment("XDG_SESSION_TYPE"));
      ref StringBuilder.AppendInterpolatedStringHandler local43 = ref interpolatedStringHandler;
      stringBuilder88.AppendLine(ref local43);
      StringBuilder stringBuilder89 = stringBuilder1;
      StringBuilder stringBuilder90 = stringBuilder89;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(23, 1, stringBuilder89);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  XDG_CURRENT_DESKTOP: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetEnvironment("XDG_CURRENT_DESKTOP"));
      ref StringBuilder.AppendInterpolatedStringHandler local44 = ref interpolatedStringHandler;
      stringBuilder90.AppendLine(ref local44);
      StringBuilder stringBuilder91 = stringBuilder1;
      StringBuilder stringBuilder92 = stringBuilder91;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(19, 1, stringBuilder91);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("  WAYLAND_DISPLAY: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(OS.GetEnvironment("WAYLAND_DISPLAY"));
      ref StringBuilder.AppendInterpolatedStringHandler local45 = ref interpolatedStringHandler;
      stringBuilder92.AppendLine(ref local45);
    }
    return stringBuilder1.ToString();
  }

  private static string FormatBytes(ulong bytes)
  {
    string[] strArray = new string[6]
    {
      "B",
      "KB",
      "MB",
      "GB",
      "TB",
      "PB"
    };
    int index = 0;
    Decimal num = (Decimal) bytes;
    while (Math.Round(num / 1024M) >= 1M)
    {
      num /= 1024M;
      ++index;
    }
    return $"{num:n1}{strArray[index]}";
  }
}
