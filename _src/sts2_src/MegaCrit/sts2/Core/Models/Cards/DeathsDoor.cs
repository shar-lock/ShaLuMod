// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DeathsDoor
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DeathsDoor : CardModel
{
  public DeathsDoor()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  public override bool GainsBlock => true;

  protected override bool ShouldGlowGoldInternal => this.WasDoomAppliedThisTurn;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new BlockVar(6M, ValueProp.Move),
        (DynamicVar) new RepeatVar(2)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DoomPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    int blockGains = 1;
    if (this.WasDoomAppliedThisTurn)
      blockGains += this.DynamicVars.Repeat.IntValue;
    for (int i = 0; i < blockGains; ++i)
    {
      Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(1M);

  private bool WasDoomAppliedThisTurn
  {
    get
    {
      return CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>().Any<PowerReceivedEntry>((Func<PowerReceivedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.Power is DoomPower && e.Applier == this.Owner.Creature));
    }
  }
}
