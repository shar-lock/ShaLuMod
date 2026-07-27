// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.CrystalSphereCell
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;

public class CrystalSphereCell
{
  private bool _isHidden;
  private bool _isHighlighted;
  private bool _isHovered;

  public CrystalSphereItem? Item { get; private set; }

  public int X { get; private set; }

  public int Y { get; private set; }

  public bool IsHidden
  {
    get => this._isHidden;
    set
    {
      this._isHidden = value;
      Action fogUpdated = this.FogUpdated;
      if (fogUpdated == null)
        return;
      fogUpdated();
    }
  }

  public event Action? FogUpdated;

  public bool IsHighlighted
  {
    get => this._isHighlighted;
    set
    {
      this._isHighlighted = value;
      Action highlightUpdated = this.HighlightUpdated;
      if (highlightUpdated == null)
        return;
      highlightUpdated();
    }
  }

  public bool IsHovered
  {
    get => this._isHovered;
    set
    {
      this._isHovered = value;
      Action highlightUpdated = this.HighlightUpdated;
      if (highlightUpdated == null)
        return;
      highlightUpdated();
    }
  }

  public event Action? HighlightUpdated;

  public CrystalSphereCell(int x, int y)
  {
    this.X = x;
    this.Y = y;
    this.IsHidden = true;
  }

  public void SetItem(CrystalSphereItem? item)
  {
    this.Item = this.Item == null ? item : throw new InvalidOperationException("An item already occupies this cell");
  }
}
