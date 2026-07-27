// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.ColorlessCardPool
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

public sealed class ColorlessCardPool : CardPoolModel
{
  public const string energyColorName = "colorless";

  public override string Title => "colorless";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => new Color("A3A3A3FF");

  public override bool IsColorless => true;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[65]
    {
      (CardModel) ModelDb.Card<Alchemize>(),
      (CardModel) ModelDb.Card<Anointed>(),
      (CardModel) ModelDb.Card<Automation>(),
      (CardModel) ModelDb.Card<BeaconOfHope>(),
      (CardModel) ModelDb.Card<BeatDown>(),
      (CardModel) ModelDb.Card<BelieveInYou>(),
      (CardModel) ModelDb.Card<Bolas>(),
      (CardModel) ModelDb.Card<Calamity>(),
      (CardModel) ModelDb.Card<Catastrophe>(),
      (CardModel) ModelDb.Card<Coordinate>(),
      (CardModel) ModelDb.Card<DarkShackles>(),
      (CardModel) ModelDb.Card<Discovery>(),
      (CardModel) ModelDb.Card<DramaticEntrance>(),
      (CardModel) ModelDb.Card<Entropy>(),
      (CardModel) ModelDb.Card<Equilibrium>(),
      (CardModel) ModelDb.Card<EternalArmor>(),
      (CardModel) ModelDb.Card<Fasten>(),
      (CardModel) ModelDb.Card<Finesse>(),
      (CardModel) ModelDb.Card<Fisticuffs>(),
      (CardModel) ModelDb.Card<FlashOfSteel>(),
      (CardModel) ModelDb.Card<GangUp>(),
      (CardModel) ModelDb.Card<GoldAxe>(),
      (CardModel) ModelDb.Card<HandOfGreed>(),
      (CardModel) ModelDb.Card<HiddenGem>(),
      (CardModel) ModelDb.Card<HuddleUp>(),
      (CardModel) ModelDb.Card<Impatience>(),
      (CardModel) ModelDb.Card<Intercept>(),
      (CardModel) ModelDb.Card<JackOfAllTrades>(),
      (CardModel) ModelDb.Card<Jackpot>(),
      (CardModel) ModelDb.Card<Knockdown>(),
      (CardModel) ModelDb.Card<Lift>(),
      (CardModel) ModelDb.Card<MasterOfStrategy>(),
      (CardModel) ModelDb.Card<Mayhem>(),
      (CardModel) ModelDb.Card<Mimic>(),
      (CardModel) ModelDb.Card<MindBlast>(),
      (CardModel) ModelDb.Card<Nostalgia>(),
      (CardModel) ModelDb.Card<Omnislice>(),
      (CardModel) ModelDb.Card<Panache>(),
      (CardModel) ModelDb.Card<PanicButton>(),
      (CardModel) ModelDb.Card<PrepTime>(),
      (CardModel) ModelDb.Card<Production>(),
      (CardModel) ModelDb.Card<Prolong>(),
      (CardModel) ModelDb.Card<Prowess>(),
      (CardModel) ModelDb.Card<Purity>(),
      (CardModel) ModelDb.Card<Rally>(),
      (CardModel) ModelDb.Card<Rend>(),
      (CardModel) ModelDb.Card<Restlessness>(),
      (CardModel) ModelDb.Card<RollingBoulder>(),
      (CardModel) ModelDb.Card<Salvo>(),
      (CardModel) ModelDb.Card<Scrawl>(),
      (CardModel) ModelDb.Card<SecretTechnique>(),
      (CardModel) ModelDb.Card<SecretWeapon>(),
      (CardModel) ModelDb.Card<SeekerStrike>(),
      (CardModel) ModelDb.Card<Shockwave>(),
      (CardModel) ModelDb.Card<Splash>(),
      (CardModel) ModelDb.Card<Stratagem>(),
      (CardModel) ModelDb.Card<TagTeam>(),
      (CardModel) ModelDb.Card<TheBall>(),
      (CardModel) ModelDb.Card<TheBomb>(),
      (CardModel) ModelDb.Card<TheGambit>(),
      (CardModel) ModelDb.Card<ThinkingAhead>(),
      (CardModel) ModelDb.Card<ThrummingHatchet>(),
      (CardModel) ModelDb.Card<UltimateDefend>(),
      (CardModel) ModelDb.Card<UltimateStrike>(),
      (CardModel) ModelDb.Card<Volley>()
    };
  }

  protected override IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (!unlockState.IsEpochRevealed<Colorless1Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Colorless1Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Colorless2Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Colorless2Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Colorless3Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Colorless3Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Colorless4Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Colorless4Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Colorless5Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Colorless5Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    return (IEnumerable<CardModel>) list;
  }
}
