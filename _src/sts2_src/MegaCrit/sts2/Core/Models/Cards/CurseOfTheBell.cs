// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.CurseOfTheBell
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class CurseOfTheBell : CardModel
{
  public CurseOfTheBell()
    : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
  {
  }

  public override bool CanBeGeneratedByModifiers => false;

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlyArray<CardKeyword>(new CardKeyword[2]
      {
        CardKeyword.Eternal,
        CardKeyword.Unplayable
      });
    }
  }
}
