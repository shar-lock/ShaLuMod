// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.TpSheetTexture
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

public class TpSheetTexture
{
  public string Image { get; set; } = "";

  public TpSheetSize Size { get; set; } = new TpSheetSize();

  public List<TpSheetSprite> Sprites { get; set; } = new List<TpSheetSprite>();
}
