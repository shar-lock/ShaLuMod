// Decompiled with JetBrains decompiler
// Type: NLightningOrbVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using System;
using System.ComponentModel;

#nullable disable
[ScriptPath("res://src/Core/Nodes/Orbs/NLightningOrbVfx.cs")]
public class NLightningOrbVfx : NOrbVfx
{
  public override void OnPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    base.OnPassiveActivated(passiveVal, evokeVal);
    this.ShakeOrb(1f, 0.4f);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NOrbVfx.MethodName
  {
  }

  public new class PropertyName : NOrbVfx.PropertyName
  {
  }

  public new class SignalName : NOrbVfx.SignalName
  {
  }
}
