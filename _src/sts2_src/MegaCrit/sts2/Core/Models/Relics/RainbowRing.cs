// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RainbowRing
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RainbowRing : RelicModel
{
  private int _attacksPlayedThisTurn;
  private int _skillsPlayedThisTurn;
  private int _powersPlayedThisTurn;
  private int _activationCountThisTurn;

  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<StrengthPower>(1M),
        (DynamicVar) new PowerVar<DexterityPower>(1M)
      });
    }
  }

  private int AttacksPlayedThisTurn
  {
    get => this._attacksPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._attacksPlayedThisTurn = value;
    }
  }

  private int SkillsPlayedThisTurn
  {
    get => this._skillsPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._skillsPlayedThisTurn = value;
    }
  }

  private int PowersPlayedThisTurn
  {
    get => this._powersPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._powersPlayedThisTurn = value;
    }
  }

  private int ActivationCountThisTurn
  {
    get => this._activationCountThisTurn;
    set
    {
      this.AssertMutable();
      this._activationCountThisTurn = value;
      this.Status = this._activationCountThisTurn > 0 ? RelicStatus.Active : RelicStatus.Normal;
    }
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.AttacksPlayedThisTurn = 0;
    this.SkillsPlayedThisTurn = 0;
    this.PowersPlayedThisTurn = 0;
    this.ActivationCountThisTurn = 0;
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || !CombatManager.Instance.IsInProgress || this.ActivationCountThisTurn >= 1)
      return;
    this.AttacksPlayedThisTurn += cardPlay.Card.Type == CardType.Attack ? 1 : 0;
    this.SkillsPlayedThisTurn += cardPlay.Card.Type == CardType.Skill ? 1 : 0;
    this.PowersPlayedThisTurn += cardPlay.Card.Type == CardType.Power ? 1 : 0;
    if (this.AttacksPlayedThisTurn <= 0 || this.SkillsPlayedThisTurn <= 0 || this.PowersPlayedThisTurn <= 0)
      return;
    this.Flash();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, this.DynamicVars.Strength.BaseValue, this.Owner.Creature, (CardModel) null);
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature, this.DynamicVars.Dexterity.BaseValue, this.Owner.Creature, (CardModel) null);
    this.ActivationCountThisTurn++;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.AttacksPlayedThisTurn = 0;
    this.SkillsPlayedThisTurn = 0;
    this.PowersPlayedThisTurn = 0;
    this.ActivationCountThisTurn = 0;
    return Task.CompletedTask;
  }
}
