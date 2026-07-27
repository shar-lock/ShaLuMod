// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.TheHunt
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class TheHunt : CardModel
{
  public TheHunt()
    : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(10M, ValueProp.Move));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Fatal));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!(this.CombatState.RunState.CurrentRoom is CombatRoom combatRoom))
    {
      combatRoom = (CombatRoom) null;
    }
    else
    {
      ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
      bool shouldTriggerFatal = cardPlay.Target.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()));
      AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
      if (!shouldTriggerFatal)
        combatRoom = (CombatRoom) null;
      else if (!attackCommand.Results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))
      {
        combatRoom = (CombatRoom) null;
      }
      else
      {
        combatRoom.AddExtraReward(this.Owner, (Reward) new CardReward(CardCreationOptions.ForRoom(this.Owner, combatRoom.RoomType), 3, this.Owner));
        TheHuntPower theHuntPower = await PowerCmd.Apply<TheHuntPower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
        combatRoom = (CombatRoom) null;
      }
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}
