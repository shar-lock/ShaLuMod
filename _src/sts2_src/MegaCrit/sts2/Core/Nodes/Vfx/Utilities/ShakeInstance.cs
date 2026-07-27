// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.ShakeInstance
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

public abstract class ShakeInstance
{
  protected float _strength;
  protected double _startDuration;
  protected double _duration;
  protected float _ease;
  protected float _angle;

  protected static float WiggleSpeed => 60f;

  public bool IsDone { get; protected set; }

  public abstract Vector2 Update(double delta);
}
