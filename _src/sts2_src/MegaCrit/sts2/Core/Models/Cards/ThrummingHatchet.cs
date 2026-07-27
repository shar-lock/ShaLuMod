// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.ThrummingHatchet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class ThrummingHatchet : CardModel
{
  public ThrummingHatchet()
    : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(11M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  public override async Task BeforeHandDraw(
    Player player,
    PlayerChoiceContext choiceContext,
    ICombatState combatState)
  {
    if (player != this.Owner || !CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedLastPlayerTurn(this.Owner) && e.CardPlay.Card == this)))
      return;
    CardPile pile = this.Pile;
    if ((pile != null ? (pile.Type != PileType.Hand ? 1 : 0) : 1) == 0)
      return;
    CardPileAddResult cardPileAddResult = await CardPileCmd.Add((CardModel) this, PileType.Hand);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
