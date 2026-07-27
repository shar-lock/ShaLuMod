// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.IroncladCardPool
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

public sealed class IroncladCardPool : CardPoolModel
{
  public override string Title => "ironclad";

  public override string EnergyColorName => "ironclad";

  public override string CardFrameMaterialPath => "card_frame_red";

  public override Color DeckEntryCardColor => new Color("D62000");

  public override Color EnergyOutlineColor => new Color("802020");

  public override bool IsColorless => false;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[90]
    {
      (CardModel) ModelDb.Card<Aggression>(),
      (CardModel) ModelDb.Card<Anger>(),
      (CardModel) ModelDb.Card<Armaments>(),
      (CardModel) ModelDb.Card<AshenStrike>(),
      (CardModel) ModelDb.Card<Barricade>(),
      (CardModel) ModelDb.Card<Bash>(),
      (CardModel) ModelDb.Card<BattleTrance>(),
      (CardModel) ModelDb.Card<Blaze>(),
      (CardModel) ModelDb.Card<BloodWall>(),
      (CardModel) ModelDb.Card<Bloodletting>(),
      (CardModel) ModelDb.Card<Bludgeon>(),
      (CardModel) ModelDb.Card<BodySlam>(),
      (CardModel) ModelDb.Card<Brand>(),
      (CardModel) ModelDb.Card<Break>(),
      (CardModel) ModelDb.Card<Breakthrough>(),
      (CardModel) ModelDb.Card<Bully>(),
      (CardModel) ModelDb.Card<BurningPact>(),
      (CardModel) ModelDb.Card<Cascade>(),
      (CardModel) ModelDb.Card<Cinder>(),
      (CardModel) ModelDb.Card<Colossus>(),
      (CardModel) ModelDb.Card<Conflagration>(),
      (CardModel) ModelDb.Card<Corruption>(),
      (CardModel) ModelDb.Card<CrimsonMantle>(),
      (CardModel) ModelDb.Card<Cruelty>(),
      (CardModel) ModelDb.Card<DarkEmbrace>(),
      (CardModel) ModelDb.Card<DefendIronclad>(),
      (CardModel) ModelDb.Card<DemonForm>(),
      (CardModel) ModelDb.Card<DemonicShield>(),
      (CardModel) ModelDb.Card<Dismantle>(),
      (CardModel) ModelDb.Card<Dominate>(),
      (CardModel) ModelDb.Card<DrumOfBattle>(),
      (CardModel) ModelDb.Card<EvilEye>(),
      (CardModel) ModelDb.Card<ExpectAFight>(),
      (CardModel) ModelDb.Card<Feed>(),
      (CardModel) ModelDb.Card<FeelNoPain>(),
      (CardModel) ModelDb.Card<FiendFire>(),
      (CardModel) ModelDb.Card<FightMe>(),
      (CardModel) ModelDb.Card<FlameBarrier>(),
      (CardModel) ModelDb.Card<ForgottenRitual>(),
      (CardModel) ModelDb.Card<Havoc>(),
      (CardModel) ModelDb.Card<Headbutt>(),
      (CardModel) ModelDb.Card<Hellraiser>(),
      (CardModel) ModelDb.Card<Hemokinesis>(),
      (CardModel) ModelDb.Card<HowlFromBeyond>(),
      (CardModel) ModelDb.Card<Impervious>(),
      (CardModel) ModelDb.Card<InfernalBlade>(),
      (CardModel) ModelDb.Card<Inferno>(),
      (CardModel) ModelDb.Card<Inflame>(),
      (CardModel) ModelDb.Card<IronWave>(),
      (CardModel) ModelDb.Card<Juggernaut>(),
      (CardModel) ModelDb.Card<Juggling>(),
      (CardModel) ModelDb.Card<Mangle>(),
      (CardModel) ModelDb.Card<Midnight>(),
      (CardModel) ModelDb.Card<MoltenFist>(),
      (CardModel) ModelDb.Card<NotYet>(),
      (CardModel) ModelDb.Card<Offering>(),
      (CardModel) ModelDb.Card<OneTwoPunch>(),
      (CardModel) ModelDb.Card<Outrage>(),
      (CardModel) ModelDb.Card<PactsEnd>(),
      (CardModel) ModelDb.Card<PerfectedStrike>(),
      (CardModel) ModelDb.Card<Pillage>(),
      (CardModel) ModelDb.Card<PommelStrike>(),
      (CardModel) ModelDb.Card<PrimalForce>(),
      (CardModel) ModelDb.Card<Pyre>(),
      (CardModel) ModelDb.Card<Rage>(),
      (CardModel) ModelDb.Card<Rampage>(),
      (CardModel) ModelDb.Card<Rupture>(),
      (CardModel) ModelDb.Card<SecondWind>(),
      (CardModel) ModelDb.Card<SetupStrike>(),
      (CardModel) ModelDb.Card<ShrugItOff>(),
      (CardModel) ModelDb.Card<Spite>(),
      (CardModel) ModelDb.Card<Stampede>(),
      (CardModel) ModelDb.Card<Stoke>(),
      (CardModel) ModelDb.Card<Stomp>(),
      (CardModel) ModelDb.Card<StoneArmor>(),
      (CardModel) ModelDb.Card<StrikeIronclad>(),
      (CardModel) ModelDb.Card<SwordBoomerang>(),
      (CardModel) ModelDb.Card<Tank>(),
      (CardModel) ModelDb.Card<Taunt>(),
      (CardModel) ModelDb.Card<TearAsunder>(),
      (CardModel) ModelDb.Card<Thrash>(),
      (CardModel) ModelDb.Card<Thunderclap>(),
      (CardModel) ModelDb.Card<Tremble>(),
      (CardModel) ModelDb.Card<TrueGrit>(),
      (CardModel) ModelDb.Card<TwinStrike>(),
      (CardModel) ModelDb.Card<Unmovable>(),
      (CardModel) ModelDb.Card<Unrelenting>(),
      (CardModel) ModelDb.Card<Uppercut>(),
      (CardModel) ModelDb.Card<Vicious>(),
      (CardModel) ModelDb.Card<Whirlwind>()
    };
  }

  protected override IEnumerable<CardModel> FilterThroughEpochs(
    UnlockState unlockState,
    IEnumerable<CardModel> cards)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (!unlockState.IsEpochRevealed<Ironclad2Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Ironclad2Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Ironclad5Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Ironclad5Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    if (!unlockState.IsEpochRevealed<Ironclad7Epoch>())
      list.RemoveAll((Predicate<CardModel>) (c => Ironclad7Epoch.Cards.Any<CardModel>((Func<CardModel, bool>) (card => card.Id == c.Id))));
    return (IEnumerable<CardModel>) list;
  }
}
