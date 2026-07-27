// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SoulboundPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SoulboundPower : PowerModel
{
  private bool _isAddingSoul;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());
    }
  }

  private bool IsAddingSoul
  {
    get => this._isAddingSoul;
    set
    {
      this.AssertMutable();
      this._isAddingSoul = value;
    }
  }

  public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
  {
    if (creator == null || creator.Creature != this.Applier || !(card is Soul) || this.IsAddingSoul)
      return;
    this.IsAddingSoul = true;
    this.Flash();
    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) Soul.Create(this.Owner.Player, this.Amount, this.CombatState), PileType.Draw, this.Owner.Player, CardPilePosition.Random));
    this.IsAddingSoul = false;
  }
}
