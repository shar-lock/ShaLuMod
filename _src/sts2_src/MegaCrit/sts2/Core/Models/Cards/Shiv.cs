// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Shiv
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Shiv : CardModel
{
  public Shiv()
    : base(0, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
  {
  }

  public override TargetType TargetType
  {
    get => !this.HasFanOfKnives ? TargetType.AnyEnemy : TargetType.AllEnemies;
  }

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.Shiv };
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(4M, ValueProp.Move));
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
    AttackCommand attackCommand1 = DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay);
    AttackCommand attackCommand2;
    if (this.HasFanOfKnives)
    {
      Creature lastEnemy = this.CombatState.HittableEnemies.LastOrDefault<Creature>();
      attackCommand2 = attackCommand1.TargetingAllOpponents(this.CombatState).WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NShivThrowVfx.Create(this.Owner.Creature, lastEnemy, Colors.Green)));
    }
    else
    {
      ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
      attackCommand2 = attackCommand1.Targeting(cardPlay.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NShivThrowVfx.Create(this.Owner.Creature, t, Colors.Green)));
    }
    if (this.Owner.Character is Silent)
      attackCommand2.WithAttackerAnim(nameof (Shiv), 0.2f);
    AttackCommand attackCommand3 = await attackCommand2.Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);

  public static async Task<CardModel?> CreateInHand(
    Player owner,
    ICombatState combatState,
    Player? creator = null)
  {
    return (await Shiv.CreateInHand(owner, 1, combatState, creator)).FirstOrDefault<CardModel>();
  }

  public static async Task<IEnumerable<CardModel>> CreateInHand(
    Player owner,
    int count,
    ICombatState combatState,
    Player? creator = null)
  {
    if (count == 0)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    if (CombatManager.Instance.IsOverOrEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    List<CardModel> shivs = new List<CardModel>();
    for (int index = 0; index < count; ++index)
      shivs.Add((CardModel) combatState.CreateCard<Shiv>(owner));
    IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) shivs, PileType.Hand, creator ?? owner);
    return (IEnumerable<CardModel>) shivs;
  }

  private bool HasFanOfKnives
  {
    get => this.IsMutable && this.Owner != null && this.Owner.Creature.HasPower<FanOfKnivesPower>();
  }
}
