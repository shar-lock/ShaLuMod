// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardKeywordOrder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public static class CardKeywordOrder
{
  public static readonly CardKeyword[] beforeDescription = new CardKeyword[5]
  {
    CardKeyword.Ethereal,
    CardKeyword.Sly,
    CardKeyword.Retain,
    CardKeyword.Innate,
    CardKeyword.Unplayable
  };
  public static readonly CardKeyword[] afterDescription = new CardKeyword[2]
  {
    CardKeyword.Exhaust,
    CardKeyword.Eternal
  };
}
