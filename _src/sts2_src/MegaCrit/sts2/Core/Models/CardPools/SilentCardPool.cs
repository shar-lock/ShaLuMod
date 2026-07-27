// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.SilentCardPool
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

public sealed class SilentCardPool : CardPoolModel
{
  public override string Title => "silent";

  public override string EnergyColorName => "silent";

  public override string CardFrameMaterialPath => "card_frame_green";

  public override Color DeckEntryCardColor => new Color("5EBD00");

  public override Color EnergyOutlineColor => new Color("1A6625");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[91]
    {
      (CardModel) ModelDb.Card<Abrasive>(),
      (CardModel) ModelDb.Card<Accelerant>(),
      (CardModel) ModelDb.Card<Accuracy>(),
      (CardModel) ModelDb.Card<Acrobatics>(),
      (CardModel) ModelDb.Card<Adrenaline>(),
      (CardModel) ModelDb.Card<Afterimage>(),
      (CardModel) ModelDb.Card<Anticipate>(),
      (CardModel) ModelDb.Card<Assassinate>(),
      (CardModel) ModelDb.Card<Backflip>(),
      (CardModel) ModelDb.Card<Backstab>(),
      (CardModel) ModelDb.Card<BladeOfInk>(),
      (CardModel) ModelDb.Card<BladeDance>(),
      (CardModel) ModelDb.Card<BladeSymphony>(),
      (CardModel) ModelDb.Card<Blur>(),
      (CardModel) ModelDb.Card<BouncingFlask>(),
      (CardModel) ModelDb.Card<BubbleBubble>(),
      (CardModel) ModelDb.Card<BulletTime>(),
      (CardModel) ModelDb.Card<Burst>(),
      (CardModel) ModelDb.Card<CalculatedGamble>(),
      (CardModel) ModelDb.Card<CloakAndDagger>(),
      (CardModel) ModelDb.Card<CorrosiveWave>(),
      (CardModel) ModelDb.Card<Concoct>(),
      (CardModel) ModelDb.Card<DaggerSpray>(),
      (CardModel) ModelDb.Card<DaggerThrow>(),
      (CardModel) ModelDb.Card<Dash>(),
      (CardModel) ModelDb.Card<DeadlyPoison>(),
      (CardModel) ModelDb.Card<DefendSilent>(),
      (CardModel) ModelDb.Card<Deflect>(),
      (CardModel) ModelDb.Card<DodgeAndRoll>(),
      (CardModel) ModelDb.Card<EchoingSlash>(),
      (CardModel) ModelDb.Card<Envenom>(),
      (CardModel) ModelDb.Card<EscapePlan>(),
      (CardModel) ModelDb.Card<Expertise>(),
      (CardModel) ModelDb.Card<Expose>(),
      (CardModel) ModelDb.Card<Fade>(),
      (CardModel) ModelDb.Card<FanOfKnives>(),
      (CardModel) ModelDb.Card<Finisher>(),
      (CardModel) ModelDb.Card<Flanking>(),
      (CardModel) ModelDb.Card<Flechettes>(),
      (CardModel) ModelDb.Card<FlickFlack>(),
      (CardModel) ModelDb.Card<Scare>(),
      (CardModel) ModelDb.Card<Footwork>(),
      (CardModel) ModelDb.Card<GrandFinale>(),
      (CardModel) ModelDb.Card<HandTrick>(),
      (CardModel) ModelDb.Card<Haze>(),
      (CardModel) ModelDb.Card<HiddenDaggers>(),
      (CardModel) ModelDb.Card<InfiniteBlades>(),
      (CardModel) ModelDb.Card<KnifeTrap>(),
      (CardModel) ModelDb.Card<LeadingStrike>(),
      (CardModel) ModelDb.Card<LegSweep>(),
      (CardModel) ModelDb.Card<Malaise>(),
      (CardModel) ModelDb.Card<MasterPlanner>(),
      (CardModel) ModelDb.Card<MementoMori>(),
      (CardModel) ModelDb.Card<Mirage>(),
      (CardModel) ModelDb.Card<Murder>(),
      (CardModel) ModelDb.Card<Neutralize>(),
      (CardModel) ModelDb.Card<Nightmare>(),
      (CardModel) ModelDb.Card<NoxiousFumes>(),
      (CardModel) ModelDb.Card<Outbreak>(),
      (CardModel) ModelDb.Card<PhantomBlades>(),
      (CardModel) ModelDb.Card<PiercingWail>(),
      (CardModel) ModelDb.Card<Pinpoint>(),
      (CardModel) ModelDb.Card<PoisonedStab>(),
      (CardModel) ModelDb.Card<Pounce>(),
      (CardModel) ModelDb.Card<PreciseCut>(),
      (CardModel) ModelDb.Card<Predator>(),
      (CardModel) ModelDb.Card<Prepared>(),
      (CardModel) ModelDb.Card<Reflex>(),
      (CardModel) ModelDb.Card<Ricochet>(),
      (CardModel) ModelDb.Card<SerpentForm>(),
      (CardModel) ModelDb.Card<ShadowStep>(),
      (CardModel) ModelDb.Card<Shadowmeld>(),
      (CardModel) ModelDb.Card<Skewer>(),
      (CardModel) ModelDb.Card<Slice>(),
      (CardModel) ModelDb.Card<Snakebite>(),
      (CardModel) ModelDb.Card<Sneaky>(),
      (CardModel) ModelDb.Card<Speedster>(),
      (CardModel) ModelDb.Card<StormOfSteel>(),
      (CardModel) ModelDb.Card<Strangle>(),
      (CardModel) ModelDb.Card<StrikeSilent>(),
      (CardModel) ModelDb.Card<SuckerPunch>(),
      (CardModel) ModelDb.Card<Suppress>(),
      (CardModel) ModelDb.Card<Survivor>(),
      (CardModel) ModelDb.Card<Tactician>(),
      (CardModel) ModelDb.Card<TheHunt>(),
      (CardModel) ModelDb.Card<ToolsOfTheTrade>(),
      (CardModel) ModelDb.Card<Tracking>(),
      (CardModel) ModelDb.Card<Untouchable>(),
      (CardModel) ModelDb.Card<UpMySleeve>(),
      (CardModel) ModelDb.Card<WellLaidPlans>(),
      (CardModel) ModelDb.Card<WraithForm>()
    };
  }

  protected override IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (!unlockState.IsEpochRevealed<Silent2Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Silent2Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Silent5Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Silent5Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Silent7Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Silent7Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    return (IEnumerable<CardModel>) list;
  }
}
