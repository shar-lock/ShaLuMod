// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.ImitationLearning
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class ImitationLearning : CardModel
{
  public ImitationLearning()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyAlly)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<ImitationLearningPower>(2M));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
    ImitationLearningPower power = this.Owner.Creature.Powers.OfType<ImitationLearningPower>().FirstOrDefault<ImitationLearningPower>((Func<ImitationLearningPower, bool>) (s => s.PlayerTarget == cardPlay.Target.Player));
    Decimal baseValue = this.DynamicVars["ImitationLearningPower"].BaseValue;
    if (power != null)
    {
      int num = await PowerCmd.ModifyAmount(choiceContext, (PowerModel) power, baseValue, this.Owner.Creature, (CardModel) this);
    }
    else
    {
      ImitationLearningPower imitationLearningPower = await PowerCmd.Apply<ImitationLearningPower>(choiceContext, this.Owner.Creature, baseValue, this.Owner.Creature, (CardModel) this);
      if (imitationLearningPower == null)
        ;
      else
        imitationLearningPower.PlayerTarget = cardPlay.Target.Player;
    }
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars["ImitationLearningPower"].UpgradeValueBy(1M);
  }
}
