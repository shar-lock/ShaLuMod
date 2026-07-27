// Decompiled with JetBrains decompiler
// Type: RiderTestRunner.NetCoreRunner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using System.ComponentModel;

#nullable enable
namespace RiderTestRunner;

[ScriptPath("res://RiderTestRunner/NetCoreRunner.cs")]
public class NetCoreRunner : Node
{
  public static NetCoreRunner? Instance { get; private set; }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(
  #nullable disable
  GodotSerializationInfo info) => ((GodotObject) this).SaveGodotObjectData(info);

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node.MethodName
  {
  }

  public class PropertyName : Node.PropertyName
  {
  }

  public class SignalName : Node.SignalName
  {
  }
}
