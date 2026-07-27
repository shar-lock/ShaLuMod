// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.CaptureSpirit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class CaptureSpirit : CardModel
{
  public CaptureSpirit()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(3M, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardPlay.Target, this.DynamicVars.Damage, (CardModel) this, cardPlay);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) Soul.Create(this.Owner, this.DynamicVars.Cards.IntValue, this.CombatState).ToList<Soul>(), PileType.Draw, this.Owner, CardPilePosition.Random));
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(1M);
    this.DynamicVars.Cards.UpgradeValueBy(1M);
  }
}
