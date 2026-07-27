// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Flanking
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Flanking : CardModel
{
  public Flanking()
    : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    FlankingPower flankingPower = await PowerCmd.Apply<FlankingPower>(choiceContext, cardPlay.Target, 2M, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
