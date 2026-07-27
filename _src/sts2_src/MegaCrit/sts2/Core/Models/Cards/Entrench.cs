// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Entrench
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Entrench : CardModel
{
  public Entrench()
    : base(2, CardType.Skill, CardRarity.Event, TargetType.Self)
  {
  }

  public override CardPoolModel VisualCardPool
  {
    get => (CardPoolModel) ModelDb.CardPool<IroncladCardPool>();
  }

  public override bool GainsBlock => true;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, (Decimal) this.Owner.Creature.Block, ValueProp.Unpowered | ValueProp.Move, cardPlay);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
