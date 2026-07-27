// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Colossus
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Colossus : CardModel
{
  private const string _powerVarName = "Colossus";

  public Colossus()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VulnerablePower>());
    }
  }

  public override bool GainsBlock => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new BlockVar(4M, ValueProp.Move),
        new DynamicVar(nameof (Colossus), 1M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    ColossusPower colossusPower = await PowerCmd.Apply<ColossusPower>(choiceContext, this.Owner.Creature, this.DynamicVars[nameof (Colossus)].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
