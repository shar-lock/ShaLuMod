// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.CharacterCards
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class CharacterCards : ModifierModel
{
  private ModelId? _characterModel;

  public override LocString Title
  {
    get => ModelDb.GetById<MegaCrit.Sts2.Core.Models.CharacterModel>(this.CharacterModel).CardsModifierTitle;
  }

  public override LocString Description
  {
    get => ModelDb.GetById<MegaCrit.Sts2.Core.Models.CharacterModel>(this.CharacterModel).CardsModifierDescription;
  }

  [SavedProperty]
  public ModelId CharacterModel
  {
    get
    {
      return this._characterModel ?? throw new InvalidOperationException("CharacterCards modifier used without CharacterModel set!");
    }
    set
    {
      this.AssertMutable();
      this._characterModel = value;
    }
  }

  private CardPoolModel CharacterCardPool
  {
    get => ModelDb.GetById<MegaCrit.Sts2.Core.Models.CharacterModel>(this.CharacterModel).CardPool;
  }

  public override IEnumerable<CardModel> ModifyMerchantCardPool(
    Player player,
    IEnumerable<CardModel> options)
  {
    CardPoolModel cardPool = player.Character.CardPool;
    CardModel[] array = options.ToArray<CardModel>();
    return ((IEnumerable<CardModel>) array).Any<CardModel>((Func<CardModel, bool>) (c => c.Pool != cardPool)) ? (IEnumerable<CardModel>) array : ((IEnumerable<CardModel>) array).Concat<CardModel>(this.CharacterCardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint));
  }

  public override CardCreationOptions ModifyCardRewardCreationOptions(
    Player player,
    CardCreationOptions options)
  {
    // ISSUE: object of a compiler-generated type is created
    return options.Flags.HasFlag((Enum) CardCreationFlags.NoCardPoolModifications) || !options.Flags.HasFlag((Enum) CardCreationFlags.IsCardReward) ? options : options.WithCardPools(options.CardPools.Union<CardPoolModel>((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.CharacterCardPool)));
  }

  public override bool IsEquivalent(ModifierModel other)
  {
    return base.IsEquivalent(other) && ((CharacterCards) other)._characterModel == this._characterModel;
  }
}
