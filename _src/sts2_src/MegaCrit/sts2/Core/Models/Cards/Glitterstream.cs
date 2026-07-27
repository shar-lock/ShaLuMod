// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Glitterstream
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Glitterstream : CardModel
{
  private const string _blockNextTurnKey = "BlockNextTurn";

  public Glitterstream()
    : base(2, CardType.Skill, CardRarity.Common, TargetType.Self)
  {
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new BlockVar(11M, ValueProp.Move),
        (DynamicVar) new BlockVar("BlockNextTurn", 5M, ValueProp.Move)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    BlockVar dynamicVar = (BlockVar) this.DynamicVars["BlockNextTurn"];
    Decimal blockNextTurnAmount = Hook.ModifyBlock(this.CombatState, this.Owner.Creature, dynamicVar.BaseValue, dynamicVar.Props, (CardModel) this, cardPlay, out IEnumerable<AbstractModel> _);
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    BlockNextTurnPower blockNextTurnPower = await PowerCmd.Apply<BlockNextTurnPower>(choiceContext, this.Owner.Creature, blockNextTurnAmount, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Block.UpgradeValueBy(2M);
    this.DynamicVars["BlockNextTurn"].UpgradeValueBy(2M);
  }
}
