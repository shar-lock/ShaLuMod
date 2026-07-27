// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.Reward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public abstract class Reward
{
  protected Rng? _rngOverride;

  public Player Player { get; }

  protected abstract RewardType RewardType { get; }

  public abstract int RewardsSetIndex { get; }

  public abstract LocString Description { get; }

  public abstract bool IsPopulated { get; }

  public bool SuccessfullySelected { get; private set; }

  protected virtual string? IconPath => (string) null;

  public virtual Vector2 IconPosition => Vector2.Zero;

  protected Reward(Player player) => this.Player = player;

  public abstract void Populate();

  protected abstract Task<bool> OnSelect();

  public virtual Control? CreateIcon()
  {
    if (TestMode.IsOn)
      return (Control) null;
    TextureRect icon = new TextureRect();
    icon.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.IconPath);
    ((Control) icon).SetAnchorsPreset((Control.LayoutPreset) 15L, false);
    icon.ExpandMode = (TextureRect.ExpandModeEnum) 1L;
    return (Control) icon;
  }

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public virtual IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      List<IHoverTip> list = this.ExtraHoverTips.ToList<IHoverTip>();
      if (this.ParentRewardSet != null)
        list.Add((IHoverTip) LinkedRewardSet.HoverTip);
      return (IEnumerable<IHoverTip>) list;
    }
  }

  public LinkedRewardSet? ParentRewardSet { get; set; }

  public virtual void OnSkipped()
  {
  }

  public async Task<bool> SelectUnsynchronized()
  {
    bool success = await this.OnSelect();
    if (success)
    {
      await Hook.AfterRewardTaken(this.Player.RunState, this.Player, this);
      this.SuccessfullySelected = true;
    }
    if (this.ParentRewardSet != null)
    {
      this.ParentRewardSet.RemoveReward(this);
      int num = await this.ParentRewardSet.OnSelect() ? 1 : 0;
    }
    return success;
  }

  public abstract void MarkContentAsSeen();

  public virtual SerializableReward ToSerializable()
  {
    return new SerializableReward()
    {
      RewardType = this.RewardType
    };
  }

  public Reward SetRng(Rng rng)
  {
    this._rngOverride = rng;
    return this;
  }

  public static Reward FromSerializable(SerializableReward save, Player player)
  {
    switch (save.RewardType)
    {
      case RewardType.Card:
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return (Reward) new CardReward(new CardCreationOptions(save.CardPoolIds.Select<ModelId, CardPoolModel>(Reward.\u003C\u003EO.\u003C0\u003E__GetById ?? (Reward.\u003C\u003EO.\u003C0\u003E__GetById = new Func<ModelId, CardPoolModel>(ModelDb.GetById<CardPoolModel>))), save.Source, save.RarityOdds), save.OptionCount, player);
      case RewardType.Gold:
        return (Reward) new GoldReward(save.GoldAmount, player, save.WasGoldStolenBack);
      case RewardType.Potion:
        return (Reward) new PotionReward(player);
      case RewardType.Relic:
        return save.PredeterminedModelId != ModelId.none ? (Reward) new RelicReward(ModelDb.GetById<RelicModel>(save.PredeterminedModelId).ToMutable(), player) : (Reward) new RelicReward(player);
      case RewardType.RemoveCard:
        return (Reward) new CardRemovalReward(player);
      case RewardType.SpecialCard:
        CardModel cardModel = CardModel.FromSerializable(save.SpecialCard);
        player.RunState.AddCard(cardModel, player);
        SpecialCardReward specialCardReward = new SpecialCardReward(cardModel, player);
        if (save.CustomDescriptionEncounterSourceId != ModelId.none)
          specialCardReward.SetCustomDescriptionEncounterSource(save.CustomDescriptionEncounterSourceId);
        return (Reward) specialCardReward;
      default:
        throw new NotImplementedException("Serializing these types of rewards hasn't been implemented yet");
    }
  }
}
