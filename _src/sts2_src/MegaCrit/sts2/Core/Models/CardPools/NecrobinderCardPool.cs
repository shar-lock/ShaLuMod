// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.NecrobinderCardPool
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

public sealed class NecrobinderCardPool : CardPoolModel
{
  public override string Title => "necrobinder";

  public override string EnergyColorName => "necrobinder";

  public override string CardFrameMaterialPath => "card_frame_pink";

  public override Color DeckEntryCardColor => new Color("CD4EED");

  public override Color EnergyOutlineColor => new Color("803367");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[91]
    {
      (CardModel) ModelDb.Card<Afterlife>(),
      (CardModel) ModelDb.Card<BansheesCry>(),
      (CardModel) ModelDb.Card<BlightStrike>(),
      (CardModel) ModelDb.Card<Bodyguard>(),
      (CardModel) ModelDb.Card<BoneShards>(),
      (CardModel) ModelDb.Card<BorrowedTime>(),
      (CardModel) ModelDb.Card<Bury>(),
      (CardModel) ModelDb.Card<Cacophony>(),
      (CardModel) ModelDb.Card<Calcify>(),
      (CardModel) ModelDb.Card<CallOfTheVoid>(),
      (CardModel) ModelDb.Card<CaptureSpirit>(),
      (CardModel) ModelDb.Card<Cleanse>(),
      (CardModel) ModelDb.Card<Countdown>(),
      (CardModel) ModelDb.Card<DanseMacabre>(),
      (CardModel) ModelDb.Card<DeathMarch>(),
      (CardModel) ModelDb.Card<Deathbringer>(),
      (CardModel) ModelDb.Card<DeathsDoor>(),
      (CardModel) ModelDb.Card<Debilitate>(),
      (CardModel) ModelDb.Card<DefendNecrobinder>(),
      (CardModel) ModelDb.Card<Defile>(),
      (CardModel) ModelDb.Card<Defy>(),
      (CardModel) ModelDb.Card<Delay>(),
      (CardModel) ModelDb.Card<Demesne>(),
      (CardModel) ModelDb.Card<DevourLife>(),
      (CardModel) ModelDb.Card<Dirge>(),
      (CardModel) ModelDb.Card<DrainPower>(),
      (CardModel) ModelDb.Card<Dredge>(),
      (CardModel) ModelDb.Card<Eidolon>(),
      (CardModel) ModelDb.Card<EndOfDays>(),
      (CardModel) ModelDb.Card<EnfeeblingTouch>(),
      (CardModel) ModelDb.Card<Eradicate>(),
      (CardModel) ModelDb.Card<Fear>(),
      (CardModel) ModelDb.Card<Fetch>(),
      (CardModel) ModelDb.Card<Flatten>(),
      (CardModel) ModelDb.Card<ForbiddenGrimoire>(),
      (CardModel) ModelDb.Card<Friendship>(),
      (CardModel) ModelDb.Card<GlimpseBeyond>(),
      (CardModel) ModelDb.Card<GraveWarden>(),
      (CardModel) ModelDb.Card<Graveblast>(),
      (CardModel) ModelDb.Card<Hang>(),
      (CardModel) ModelDb.Card<Haunt>(),
      (CardModel) ModelDb.Card<HighFive>(),
      (CardModel) ModelDb.Card<Invoke>(),
      (CardModel) ModelDb.Card<LegionOfBone>(),
      (CardModel) ModelDb.Card<Lethality>(),
      (CardModel) ModelDb.Card<Melancholy>(),
      (CardModel) ModelDb.Card<Misery>(),
      (CardModel) ModelDb.Card<NecroMastery>(),
      (CardModel) ModelDb.Card<NegativePulse>(),
      (CardModel) ModelDb.Card<Neurosurge>(),
      (CardModel) ModelDb.Card<NoEscape>(),
      (CardModel) ModelDb.Card<Oblivion>(),
      (CardModel) ModelDb.Card<Pagestorm>(),
      (CardModel) ModelDb.Card<Parse>(),
      (CardModel) ModelDb.Card<Poke>(),
      (CardModel) ModelDb.Card<Protector>(),
      (CardModel) ModelDb.Card<PullAggro>(),
      (CardModel) ModelDb.Card<PullFromBelow>(),
      (CardModel) ModelDb.Card<Putrefy>(),
      (CardModel) ModelDb.Card<Rattle>(),
      (CardModel) ModelDb.Card<Reanimate>(),
      (CardModel) ModelDb.Card<Reap>(),
      (CardModel) ModelDb.Card<ReaperForm>(),
      (CardModel) ModelDb.Card<Reave>(),
      (CardModel) ModelDb.Card<RightHandHand>(),
      (CardModel) ModelDb.Card<Sacrifice>(),
      (CardModel) ModelDb.Card<Scourge>(),
      (CardModel) ModelDb.Card<SculptingStrike>(),
      (CardModel) ModelDb.Card<Seance>(),
      (CardModel) ModelDb.Card<SentryMode>(),
      (CardModel) ModelDb.Card<Severance>(),
      (CardModel) ModelDb.Card<SharedFate>(),
      (CardModel) ModelDb.Card<Shroud>(),
      (CardModel) ModelDb.Card<SicEm>(),
      (CardModel) ModelDb.Card<SleightOfFlesh>(),
      (CardModel) ModelDb.Card<Snap>(),
      (CardModel) ModelDb.Card<SoulStorm>(),
      (CardModel) ModelDb.Card<Soulbound>(),
      (CardModel) ModelDb.Card<Sow>(),
      (CardModel) ModelDb.Card<SpiritOfAsh>(),
      (CardModel) ModelDb.Card<Spur>(),
      (CardModel) ModelDb.Card<Squeeze>(),
      (CardModel) ModelDb.Card<StrikeNecrobinder>(),
      (CardModel) ModelDb.Card<TheScythe>(),
      (CardModel) ModelDb.Card<TimesUp>(),
      (CardModel) ModelDb.Card<Transfigure>(),
      (CardModel) ModelDb.Card<Undeath>(),
      (CardModel) ModelDb.Card<Underworld>(),
      (CardModel) ModelDb.Card<Unleash>(),
      (CardModel) ModelDb.Card<Veilpiercer>(),
      (CardModel) ModelDb.Card<Wisp>()
    };
  }

  protected override IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (!unlockState.IsEpochRevealed<Necrobinder2Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Necrobinder2Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Necrobinder5Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Necrobinder5Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Necrobinder7Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Necrobinder7Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    return (IEnumerable<CardModel>) list;
  }
}
