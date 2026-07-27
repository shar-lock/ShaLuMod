// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.DefectCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class DefectCardPool : CardPoolModel
{
  public override string Title => "defect";

  public override string EnergyColorName => "defect";

  public override string CardFrameMaterialPath => "card_frame_blue";

  public override Color DeckEntryCardColor => new Color("3EB3ED");

  public override Color EnergyOutlineColor => new Color("1D5673");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[91]
    {
      (CardModel) ModelDb.Card<AdaptiveStrike>(),
      (CardModel) ModelDb.Card<AllForOne>(),
      (CardModel) ModelDb.Card<BallLightning>(),
      (CardModel) ModelDb.Card<Barrage>(),
      (CardModel) ModelDb.Card<BeamCell>(),
      (CardModel) ModelDb.Card<BiasedCognition>(),
      (CardModel) ModelDb.Card<BoostAway>(),
      (CardModel) ModelDb.Card<BootSequence>(),
      (CardModel) ModelDb.Card<MegaCrit.Sts2.Core.Models.Cards.Buffer>(),
      (CardModel) ModelDb.Card<BulkUp>(),
      (CardModel) ModelDb.Card<Capacitor>(),
      (CardModel) ModelDb.Card<Chaos>(),
      (CardModel) ModelDb.Card<ChargeBattery>(),
      (CardModel) ModelDb.Card<Chill>(),
      (CardModel) ModelDb.Card<Claw>(),
      (CardModel) ModelDb.Card<ColdSnap>(),
      (CardModel) ModelDb.Card<Compact>(),
      (CardModel) ModelDb.Card<CompileDriver>(),
      (CardModel) ModelDb.Card<ConsumingShadow>(),
      (CardModel) ModelDb.Card<Coolant>(),
      (CardModel) ModelDb.Card<Coolheaded>(),
      (CardModel) ModelDb.Card<CreativeAi>(),
      (CardModel) ModelDb.Card<Darkness>(),
      (CardModel) ModelDb.Card<DefendDefect>(),
      (CardModel) ModelDb.Card<Defragment>(),
      (CardModel) ModelDb.Card<DoubleEnergy>(),
      (CardModel) ModelDb.Card<Dualcast>(),
      (CardModel) ModelDb.Card<EchoForm>(),
      (CardModel) ModelDb.Card<EnergySurge>(),
      (CardModel) ModelDb.Card<Feral>(),
      (CardModel) ModelDb.Card<FightThrough>(),
      (CardModel) ModelDb.Card<FlakCannon>(),
      (CardModel) ModelDb.Card<FocusedStrike>(),
      (CardModel) ModelDb.Card<Ftl>(),
      (CardModel) ModelDb.Card<Fusion>(),
      (CardModel) ModelDb.Card<GeneticAlgorithm>(),
      (CardModel) ModelDb.Card<Glacier>(),
      (CardModel) ModelDb.Card<Glasswork>(),
      (CardModel) ModelDb.Card<GoForTheEyes>(),
      (CardModel) ModelDb.Card<GunkUp>(),
      (CardModel) ModelDb.Card<Hailstorm>(),
      (CardModel) ModelDb.Card<HelixDrill>(),
      (CardModel) ModelDb.Card<Hibernate>(),
      (CardModel) ModelDb.Card<Hologram>(),
      (CardModel) ModelDb.Card<Hotfix>(),
      (CardModel) ModelDb.Card<Hyperbeam>(),
      (CardModel) ModelDb.Card<IceLance>(),
      (CardModel) ModelDb.Card<Ignition>(),
      (CardModel) ModelDb.Card<ImitationLearning>(),
      (CardModel) ModelDb.Card<Iteration>(),
      (CardModel) ModelDb.Card<Leap>(),
      (CardModel) ModelDb.Card<LightningRod>(),
      (CardModel) ModelDb.Card<Loop>(),
      (CardModel) ModelDb.Card<MachineLearning>(),
      (CardModel) ModelDb.Card<MeteorStrike>(),
      (CardModel) ModelDb.Card<Modded>(),
      (CardModel) ModelDb.Card<MomentumStrike>(),
      (CardModel) ModelDb.Card<MultiCast>(),
      (CardModel) ModelDb.Card<Null>(),
      (CardModel) ModelDb.Card<OneForAll>(),
      (CardModel) ModelDb.Card<Overclock>(),
      (CardModel) ModelDb.Card<Quadcast>(),
      (CardModel) ModelDb.Card<Rainbow>(),
      (CardModel) ModelDb.Card<Reboot>(),
      (CardModel) ModelDb.Card<Refract>(),
      (CardModel) ModelDb.Card<RocketPunch>(),
      (CardModel) ModelDb.Card<Scavenge>(),
      (CardModel) ModelDb.Card<Scrape>(),
      (CardModel) ModelDb.Card<ShadowShield>(),
      (CardModel) ModelDb.Card<Shatter>(),
      (CardModel) ModelDb.Card<SignalBoost>(),
      (CardModel) ModelDb.Card<Skim>(),
      (CardModel) ModelDb.Card<Smokestack>(),
      (CardModel) ModelDb.Card<Spinner>(),
      (CardModel) ModelDb.Card<Storm>(),
      (CardModel) ModelDb.Card<StrikeDefect>(),
      (CardModel) ModelDb.Card<Subroutine>(),
      (CardModel) ModelDb.Card<Sunder>(),
      (CardModel) ModelDb.Card<Supercritical>(),
      (CardModel) ModelDb.Card<SweepingBeam>(),
      (CardModel) ModelDb.Card<Synchronize>(),
      (CardModel) ModelDb.Card<Synthesis>(),
      (CardModel) ModelDb.Card<Tempest>(),
      (CardModel) ModelDb.Card<TeslaCoil>(),
      (CardModel) ModelDb.Card<Thunder>(),
      (CardModel) ModelDb.Card<TrashToTreasure>(),
      (CardModel) ModelDb.Card<Turbo>(),
      (CardModel) ModelDb.Card<Uproar>(),
      (CardModel) ModelDb.Card<Voltaic>(),
      (CardModel) ModelDb.Card<WhiteNoise>(),
      (CardModel) ModelDb.Card<Zap>()
    };
  }

  protected override IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (!unlockState.IsEpochRevealed<Defect2Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Defect2Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Defect5Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Defect5Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Defect7Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Defect7Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    return (IEnumerable<CardModel>) list;
  }
}
