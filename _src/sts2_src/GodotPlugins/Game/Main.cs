// Decompiled with JetBrains decompiler
// Type: GodotPlugins.Game.Main
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace GodotPlugins.Game;

internal static class Main
{
  [UnmanagedCallersOnly(EntryPoint = "godotsharp_game_main_init")]
  private static godot_bool InitializeFromGameProject(
    IntPtr godotDllHandle,
    IntPtr outManagedCallbacks,
    IntPtr unmanagedCallbacks,
    int unmanagedCallbacksSize)
  {
    try
    {
      // ISSUE: method pointer
      NativeLibrary.SetDllImportResolver(typeof (GodotObject).Assembly, new DllImportResolver((object) new GodotDllImportResolver(godotDllHandle), __methodptr(OnResolveDllImport)));
      NativeFuncs.Initialize(unmanagedCallbacks, unmanagedCallbacksSize);
      ManagedCallbacks.Create(outManagedCallbacks);
      ScriptManagerBridge.LookupScriptsInAssembly(typeof (Main).Assembly);
      return (godot_bool) 1;
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine((object) ex);
      return GodotBoolExtensions.ToGodotBool(false);
    }
  }
}
