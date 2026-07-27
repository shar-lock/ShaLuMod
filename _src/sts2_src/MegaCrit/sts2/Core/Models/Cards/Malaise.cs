// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Malaise
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Malaise : CardModel
{
  public Malaise()
    : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  public override TargetType TargetType => TargetType.AnyEnemy;

  protected override bool HasEnergyCostX => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<WeakPower>()
      });
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    int powerAmount = this.ResolveEnergyXValue();
    if (this.IsUpgraded)
      ++powerAmount;
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, (Decimal) -powerAmount, this.Owner.Creature, (CardModel) this);
    WeakPower weakPower = await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, (Decimal) powerAmount, this.Owner.Creature, (CardModel) this);
  }
}
