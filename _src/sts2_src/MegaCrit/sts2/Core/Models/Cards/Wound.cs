// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Wound
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Wound : CardModel
{
  public Wound()
    : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }
}
