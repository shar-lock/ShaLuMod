// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Bindings.MegaSpine.SpineAnimationAccess
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Bindings.MegaSpine;

public readonly struct SpineAnimationAccess(MegaSprite? sprite)
{
  private readonly MegaSprite? _sprite = sprite;

  public bool IsValid => this._sprite != null;

  public void SetAnimation(string name, bool loop = true, int track = 0)
  {
    this._sprite?.GetAnimationState().SetAnimation(name, loop, track);
  }

  public void AddAnimation(string name, float delay = 0.0f, bool loop = true, int track = 0)
  {
    this._sprite?.GetAnimationState().AddAnimation(name, delay, loop, track);
  }

  public MegaTrackEntry? GetCurrentTrack(int track = 0)
  {
    return this._sprite?.GetAnimationState().GetCurrent(track);
  }

  public string? GetCurrentAnimationName(int track = 0)
  {
    return this._sprite?.GetAnimationState().GetCurrentAnimationName(track);
  }

  public float? GetCurrentAnimationDuration(int track = 0)
  {
    return this._sprite?.GetAnimationState().GetCurrentAnimationDuration(track);
  }

  public void SetTimeScale(float scale) => this._sprite?.GetAnimationState().SetTimeScale(scale);

  public MegaAnimationState? GetAnimationState() => this._sprite?.GetAnimationState();
}
