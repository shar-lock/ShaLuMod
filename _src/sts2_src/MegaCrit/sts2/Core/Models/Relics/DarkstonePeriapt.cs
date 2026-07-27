// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.DarkstonePeriapt
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class DarkstonePeriapt : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Event;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new MaxHpVar(6M));
    }
  }

  public override async Task AfterCardChangedPiles(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    CardPile pile = card.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0 || card.Owner != this.Owner || card.Type != CardType.Curse)
      return;
    this.Flash();
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
  }
}
