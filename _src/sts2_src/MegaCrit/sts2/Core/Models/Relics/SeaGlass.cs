// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SeaGlass
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public class SeaGlass : RelicModel
{
  private const string _characterKey = "Character";
  private ModelId? _characterId;

  public override LocString Title
  {
    get
    {
      return this.Character == null ? new LocString("relics", this.Id.Entry + ".title") : new LocString("relics", $"{this.Id.Entry}.{this.Character.Id.Entry}.title");
    }
  }

  [SavedProperty]
  public ModelId? CharacterId
  {
    get => this._characterId;
    set
    {
      this.AssertMutable();
      this._characterId = value;
      ((StringVar) this.DynamicVars["Character"]).StringValue = this.Character.Title.GetFormattedText();
    }
  }

  private CharacterModel? Character
  {
    get
    {
      return !(this.CharacterId != (ModelId) null) ? (CharacterModel) null : ModelDb.GetById<CharacterModel>(this.CharacterId);
    }
  }

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(15),
        (DynamicVar) new StringVar("Character")
      });
    }
  }

  public override async Task AfterObtained()
  {
    if (this.CharacterId == (ModelId) null)
    {
      Log.Error("Sea Glass was obtained without a character ID assigned! This could be a bug, or the player could have used the console. Defaulting to Ironclad");
      this.CharacterId = ModelDb.Character<Ironclad>().Id;
    }
    int cardCount = this.DynamicVars.Cards.IntValue / 3;
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options1 = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Character.CardPool), (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Common)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options2 = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Character.CardPool), (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Uncommon)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
    // ISSUE: object of a compiler-generated type is created
    CardCreationOptions options3 = CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Character.CardPool), (Func<CardModel, bool>) (c => c.Rarity == CardRarity.Rare)).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications);
    List<CardCreationResult> list1 = CardFactory.CreateForReward(this.Owner, cardCount, options1).ToList<CardCreationResult>();
    List<CardCreationResult> list2 = CardFactory.CreateForReward(this.Owner, cardCount, options2).ToList<CardCreationResult>();
    List<CardCreationResult> list3 = CardFactory.CreateForReward(this.Owner, cardCount, options3).ToList<CardCreationResult>();
    List<CardCreationResult> list4 = list1.Concat<CardCreationResult>((IEnumerable<CardCreationResult>) list2).Concat<CardCreationResult>((IEnumerable<CardCreationResult>) list3).ToList<CardCreationResult>();
    CardSelectorPrefs prefs = new CardSelectorPrefs(RelicModel.L10NLookup(this.Id.Entry + ".selectionScreenPrompt"), 0, list4.Count);
    foreach (CardModel simpleGridForReward in await CardSelectCmd.FromSimpleGridForRewards((PlayerChoiceContext) new BlockingPlayerChoiceContext(), list4, this.Owner, prefs))
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(simpleGridForReward, PileType.Deck));
  }
}
