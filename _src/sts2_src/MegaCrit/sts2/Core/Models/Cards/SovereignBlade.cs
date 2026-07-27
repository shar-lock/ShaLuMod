// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.SovereignBlade
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class SovereignBlade : CardModel
{
  private const int _baseDamage = 10;
  private const string _sovereignBladeSfx = "event:/sfx/characters/regent/regent_sovereign_blade";
  private Decimal _currentDamage = 10M;
  private Decimal _currentRepeats = 1M;
  private bool _createdThroughForge;

  public SovereignBlade()
    : base(2, CardType.Attack, CardRarity.Token, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NSovereignBladeVfx.AssetPaths;

  public override TargetType TargetType
  {
    get => !this.HasSeekingEdge ? TargetType.AnyEnemy : TargetType.AllEnemies;
  }

  public override bool GainsBlock => SovereignBlade.GetOwnerParryAmount((CardModel) this) > 0M;

  private Decimal CurrentDamage
  {
    get => this._currentDamage;
    set
    {
      this.AssertMutable();
      this._currentDamage = value;
    }
  }

  private Decimal CurrentRepeats
  {
    get => this._currentRepeats;
    set
    {
      this.AssertMutable();
      this._currentRepeats = value;
    }
  }

  public bool CreatedThroughForge
  {
    get => this._createdThroughForge;
    set
    {
      this.AssertMutable();
      this._createdThroughForge = value;
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Retain);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[5]
      {
        (DynamicVar) new DamageVar(10M, ValueProp.Move),
        (DynamicVar) new CalculationBaseVar(0M),
        (DynamicVar) new CalculationExtraVar(1M),
        (DynamicVar) new CalculatedBlockVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => SovereignBlade.GetOwnerParryAmount(card))),
        (DynamicVar) new RepeatVar(1)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    string animName = this.Owner.Character is Regent ? "sovereignBladeTrigger" : "Cast";
    float delay = this.Owner.Character is Regent ? 0.25f : this.Owner.Character.CastAnimDelay;
    AttackCommand attackCommand1 = DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).WithHitCount(this.DynamicVars.Repeat.IntValue).WithAttackerAnim(animName, delay).WithAttackerFx(sfx: "event:/sfx/characters/regent/regent_sovereign_blade");
    AttackCommand attackCommand2;
    if (this.HasSeekingEdge)
    {
      attackCommand2 = attackCommand1.TargetingAllOpponents(this.CombatState).BeforeDamage((Func<Task>) (() =>
      {
        IReadOnlyList<Creature> hittableEnemies = this.CombatState.HittableEnemies;
        if (hittableEnemies.Count <= 0)
          return Task.CompletedTask;
        NSovereignBladeVfx vfxNode = SovereignBlade.GetVfxNode(this.Owner, (CardModel) this);
        NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(hittableEnemies[0]);
        if (vfxNode != null && creatureNode != null)
          vfxNode.Attack(creatureNode.VfxSpawnPosition);
        return Task.CompletedTask;
      })).WithHitFx("vfx/vfx_giant_horizontal_slash", tmpSfx: "slash_attack.mp3");
    }
    else
    {
      ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      attackCommand2 = attackCommand1.Targeting(cardPlay.Target).BeforeDamage((Func<Task>) (() =>
      {
        NSovereignBladeVfx vfxNode = SovereignBlade.GetVfxNode(this.Owner, (CardModel) this);
        NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(cardPlay.Target);
        if (vfxNode != null && creatureNode != null)
          vfxNode.Attack(creatureNode.VfxSpawnPosition);
        return Task.CompletedTask;
      })).WithHitVfxNode(SovereignBlade.\u003C\u003EO.\u003C0\u003E__Create ?? (SovereignBlade.\u003C\u003EO.\u003C0\u003E__Create = new Func<Creature, Node2D>(NBigSlashVfx.Create))).WithHitVfxNode(SovereignBlade.\u003C\u003EO.\u003C1\u003E__Create ?? (SovereignBlade.\u003C\u003EO.\u003C1\u003E__Create = new Func<Creature, Node2D>(NBigSlashImpactVfx.Create)));
    }
    AttackCommand attackCommand3 = await attackCommand2.Execute(choiceContext);
    if (!(SovereignBlade.GetOwnerParryAmount((CardModel) this) > 0M))
      ;
    else
    {
      Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), this.DynamicVars.CalculatedBlock.Props, cardPlay);
    }
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.CreatedThroughForge = false;
  }

  protected override void AfterDowngraded()
  {
    base.AfterDowngraded();
    this.DynamicVars.Damage.BaseValue = this.CurrentDamage;
    this.DynamicVars.Repeat.BaseValue = this.CurrentRepeats;
  }

  public override void AfterTransformedFrom() => this.RemoveSovereignBladeNode();

  public override Task AfterCardChangedPiles(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    if (card != this)
      return Task.CompletedTask;
    if (!this.CreatedThroughForge && oldPileType == PileType.None || oldPileType == PileType.Exhaust)
      ForgeCmd.PlayCombatRoomForgeVfx(this.Owner, (CardModel) this);
    if (card.Pile.Type == PileType.Exhaust)
      this.RemoveSovereignBladeNode();
    return Task.CompletedTask;
  }

  public void AddDamage(Decimal amount)
  {
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + amount;
    this.CurrentDamage = this.DynamicVars.Damage.BaseValue;
  }

  public void SetRepeats(Decimal amount)
  {
    this.DynamicVars.Repeat.BaseValue = amount;
    this.CurrentRepeats = this.DynamicVars.Repeat.BaseValue;
  }

  public static NSovereignBladeVfx? GetVfxNode(Player player, CardModel card)
  {
    CardModel originalCard = card.DupeOf ?? card;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(player.Creature);
    return creatureNode == null ? (NSovereignBladeVfx) null : ((IEnumerable) ((Node) creatureNode).GetChildren(false)).OfType<NSovereignBladeVfx>().FirstOrDefault<NSovereignBladeVfx>((Func<NSovereignBladeVfx, bool>) (b => b.Card == originalCard));
  }

  private void RemoveSovereignBladeNode()
  {
    SovereignBlade.GetVfxNode(this.Owner, (CardModel) this)?.RemoveSovereignBlade();
  }

  private bool HasSeekingEdge
  {
    get => this.IsMutable && this.Owner != null && this.Owner.Creature.HasPower<SeekingEdgePower>();
  }

  private static Decimal GetOwnerParryAmount(CardModel card)
  {
    return !card.IsMutable || card.Owner == null || card.Pile == null || !card.Pile.IsCombatPile ? 0M : (Decimal) card.Owner.Creature.GetPowerAmount<ParryPower>();
  }
}
