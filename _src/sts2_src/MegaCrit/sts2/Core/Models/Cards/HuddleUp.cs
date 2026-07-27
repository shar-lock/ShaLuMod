// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.HuddleUp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class HuddleUp : CardModel
{
  public HuddleUp()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
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
    foreach (Creature creature in this.CombatState.GetTeammatesOf(this.Owner.Creature).Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer)))
      await CardPileCmd.DrawWithoutBlockingOnOtherPlayers(choiceContext, this.DynamicVars.Cards.BaseValue, creature.Player);
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}
