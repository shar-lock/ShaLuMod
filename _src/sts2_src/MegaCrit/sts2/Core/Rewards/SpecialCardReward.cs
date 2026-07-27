// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.SpecialCardReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class SpecialCardReward : Reward
{
  private bool _wasTaken;
  private readonly CardModel? _card;
  private ModelId _customDescriptionEncounterSourceId = ModelId.none;

  private static string RewardIcon
  {
    get => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_special_card.png");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(SpecialCardReward.RewardIcon);
    }
  }

  protected override RewardType RewardType => RewardType.SpecialCard;

  public override int RewardsSetIndex => 4;

  protected override string IconPath => SpecialCardReward.RewardIcon;

  public override LocString Description
  {
    get
    {
      LocString description = (LocString) null;
      if (this._customDescriptionEncounterSourceId != ModelId.none)
        description = ModelDb.GetById<EncounterModel>(this._customDescriptionEncounterSourceId).CustomRewardDescription;
      if (description == null)
        description = new LocString("gameplay_ui", "COMBAT_REWARD_ADD_SPECIAL_CARD");
      description.Add("Card", this._card.Title);
      return description;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard(this._card));
    }
  }

  public SpecialCardReward(CardModel card, Player player)
    : base(player)
  {
    card.AssertMutable();
    this._card = card;
  }

  public void SetCustomDescriptionEncounterSource(ModelId encounterId)
  {
    this._customDescriptionEncounterSourceId = ModelDb.GetByIdOrNull<EncounterModel>(encounterId) != null ? encounterId : throw new ArgumentException($"Encounter {encounterId} does not exist!");
  }

  public override bool IsPopulated => this._card != null;

  public override void Populate()
  {
  }

  protected override async Task<bool> OnSelect()
  {
    Log.Info($"Player {this.Player.NetId} obtained {this._card.Id} from special card reward");
    CardPileAddResult result = await CardPileCmd.Add(this._card, PileType.Deck);
    if (result.success)
      CardCmd.PreviewCardPileAdd(result, 2f);
    this._wasTaken = true;
    return true;
  }

  public override void OnSkipped()
  {
    if (this._wasTaken)
      return;
    this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(this._card, false));
  }

  public override SerializableReward ToSerializable()
  {
    return new SerializableReward()
    {
      RewardType = this.RewardType,
      SpecialCard = this._card.ToSerializable(),
      CustomDescriptionEncounterSourceId = this._customDescriptionEncounterSourceId
    };
  }

  public override void MarkContentAsSeen()
  {
  }
}
