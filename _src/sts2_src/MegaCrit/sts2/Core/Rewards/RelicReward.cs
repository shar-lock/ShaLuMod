// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.RelicReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class RelicReward : Reward
{
  private readonly RelicRarity _rarity;
  private RelicModel? _predeterminedRelic;
  private RelicModel? _relic;
  private bool _wasTaken;

  protected override RewardType RewardType => RewardType.Relic;

  public override int RewardsSetIndex => 3;

  public RelicRarity Rarity => this._rarity;

  public RelicModel? ClaimedRelic { get; private set; }

  public RelicModel? Relic => this._relic;

  public override LocString Description => this._relic.Title;

  protected override IEnumerable<IHoverTip> ExtraHoverTips => this._relic.HoverTips;

  public RelicReward(Player player)
    : base(player)
  {
  }

  public RelicReward(RelicModel relic, Player player)
    : base(player)
  {
    relic.AssertMutable();
    this._predeterminedRelic = relic;
    this._relic = relic;
  }

  public RelicReward(RelicRarity rarity, Player player)
    : base(player)
  {
    this._rarity = rarity;
  }

  public override bool IsPopulated => this._relic != null;

  public override void Populate()
  {
    if (this._relic != null)
      return;
    if (this._rarity == RelicRarity.None)
    {
      if (this._rngOverride != null)
        this._relic = RelicFactory.PullNextRelicFromFront(this.Player, this._rngOverride).ToMutable();
      else
        this._relic = RelicFactory.PullNextRelicFromFront(this.Player).ToMutable();
    }
    else
      this._relic = RelicFactory.PullNextRelicFromFront(this.Player, this._rarity).ToMutable();
  }

  [PreserveBaseOverrides]
  TextureRect Reward.CreateIcon()
  {
    TextureRect texture = new TextureRect();
    texture.Texture = this._relic.BigIcon;
    ((CanvasItem) texture).Material = (Material) ((Resource) PreloadManager.Cache.GetMaterial("res://materials/ui/relic_mat.tres")).Duplicate(true);
    this._relic.UpdateTexture(texture);
    ((Control) texture).SetAnchorsPreset((Control.LayoutPreset) 15L, false);
    texture.ExpandMode = (TextureRect.ExpandModeEnum) 1L;
    return texture;
  }

  protected override async Task<bool> OnSelect()
  {
    Log.Info($"Player {this.Player.NetId} obtained {this._relic.Id} from relic reward");
    this.ClaimedRelic = await RelicCmd.Obtain(this._relic, this.Player);
    this._wasTaken = true;
    return true;
  }

  public override void OnSkipped()
  {
    if (this._wasTaken)
      return;
    this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).RelicChoices.Add(new ModelChoiceHistoryEntry(this._relic.Id, false));
  }

  public override void MarkContentAsSeen() => SaveManager.Instance.MarkRelicAsSeen(this._relic);

  public override SerializableReward ToSerializable()
  {
    SerializableReward serializable = base.ToSerializable();
    if (this._predeterminedRelic != null)
      serializable.PredeterminedModelId = this._predeterminedRelic.Id;
    return serializable;
  }
}
