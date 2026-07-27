// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.HoveredModelTracker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;

public class HoveredModelTracker
{
  private readonly PeerInputSynchronizer _inputSynchronizer;
  private CardModel? _localSelectedCard;
  private PotionModel? _localSelectedPotion;
  private CardModel? _localHoveredCard;
  private RelicModel? _localHoveredRelic;
  private PotionModel? _localHoveredPotion;
  private readonly List<AbstractModel?> _hoveredModels = new List<AbstractModel>();
  private readonly IPlayerCollection _playerCollection;

  public event Action<ulong>? HoverChanged;

  public HoveredModelTracker(
    PeerInputSynchronizer inputSynchronizer,
    IPlayerCollection playerCollection)
  {
    this._inputSynchronizer = inputSynchronizer;
    this._playerCollection = playerCollection;
    foreach (Player player in (IEnumerable<Player>) playerCollection.Players)
      this._hoveredModels.Add((AbstractModel) null);
    this._inputSynchronizer.StateChanged += new Action<ulong>(this.OnPlayerStateChanged);
    this._inputSynchronizer.StateRemoved += new Action<ulong>(this.OnPlayerStateRemoved);
  }

  private void OnPlayerStateChanged(ulong playerId)
  {
    Player player = this._playerCollection.GetPlayer(playerId);
    int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(player);
    if (playerSlotIndex >= this._hoveredModels.Count)
      return;
    HoveredModelData hoveredModelData = this._inputSynchronizer.GetHoveredModelData(playerId);
    AbstractModel abstractModel;
    switch (hoveredModelData.type)
    {
      case HoveredModelType.None:
        abstractModel = (AbstractModel) null;
        break;
      case HoveredModelType.Card:
        ref NetCombatCard? local = ref hoveredModelData.hoveredCombatCard;
        abstractModel = local.HasValue ? (AbstractModel) local.GetValueOrDefault().ToCardModelOrNull() : (AbstractModel) null;
        break;
      case HoveredModelType.Relic:
        int? hoveredRelicIndex = hoveredModelData.hoveredRelicIndex;
        int count1 = player.Relics.Count;
        abstractModel = hoveredRelicIndex.GetValueOrDefault() < count1 & hoveredRelicIndex.HasValue ? (AbstractModel) player.Relics[hoveredModelData.hoveredRelicIndex.Value] : (AbstractModel) null;
        break;
      case HoveredModelType.Potion:
        int? hoveredPotionIndex1 = hoveredModelData.hoveredPotionIndex;
        int count2 = player.PotionSlots.Count;
        PotionModel potionModel;
        if (hoveredPotionIndex1.GetValueOrDefault() < count2 & hoveredPotionIndex1.HasValue)
        {
          int? hoveredPotionIndex2 = hoveredModelData.hoveredPotionIndex;
          int num = 0;
          if (hoveredPotionIndex2.GetValueOrDefault() >= num & hoveredPotionIndex2.HasValue)
          {
            potionModel = player.GetPotionAtSlotIndex(hoveredModelData.hoveredPotionIndex.Value);
            goto label_11;
          }
        }
        potionModel = (PotionModel) null;
label_11:
        abstractModel = (AbstractModel) potionModel;
        break;
      default:
        throw new InvalidOperationException($"Unsupported hover type {hoveredModelData.type}");
    }
    if (abstractModel == null && hoveredModelData.type != HoveredModelType.None)
      abstractModel = ModelDb.GetById<AbstractModel>(hoveredModelData.hoveredModelId);
    if (this._hoveredModels[playerSlotIndex] == abstractModel)
      return;
    this._hoveredModels[playerSlotIndex] = abstractModel;
    Action<ulong> hoverChanged = this.HoverChanged;
    if (hoverChanged == null)
      return;
    hoverChanged(playerId);
  }

  private void OnPlayerStateRemoved(ulong playerId)
  {
    int playerSlotIndex = this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId));
    if (this._hoveredModels[playerSlotIndex] == null)
      return;
    this._hoveredModels[playerSlotIndex] = (AbstractModel) null;
    Action<ulong> hoverChanged = this.HoverChanged;
    if (hoverChanged == null)
      return;
    hoverChanged(playerId);
  }

  public AbstractModel? GetHoveredModel(ulong playerId)
  {
    return this._hoveredModels[this._playerCollection.GetPlayerSlotIndex(this._playerCollection.GetPlayer(playerId))];
  }

  public void OnLocalCardSelected(CardModel cardModel)
  {
    this._localSelectedCard = cardModel;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalCardDeselected()
  {
    this._localSelectedCard = (CardModel) null;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalPotionSelected(PotionModel potionModel)
  {
    this._localSelectedPotion = potionModel;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalPotionDeselected()
  {
    this._localSelectedPotion = (PotionModel) null;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalCardHovered(CardModel cardModel)
  {
    this._localHoveredCard = cardModel;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalCardUnhovered()
  {
    this._localHoveredCard = (CardModel) null;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalRelicHovered(RelicModel relicModel)
  {
    this._localHoveredRelic = relicModel;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalRelicUnhovered()
  {
    this._localHoveredRelic = (RelicModel) null;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalPotionHovered(PotionModel potionModel)
  {
    this._localHoveredPotion = potionModel;
    this.SynchronizeLocalHoveredModel();
  }

  public void OnLocalPotionUnhovered()
  {
    this._localHoveredPotion = (PotionModel) null;
    this.SynchronizeLocalHoveredModel();
  }

  private void SynchronizeLocalHoveredModel()
  {
    AbstractModel model = (AbstractModel) this._localSelectedCard;
    if (model == null)
    {
      PotionModel localSelectedPotion = this._localSelectedPotion;
      if (localSelectedPotion == null)
      {
        CardModel localHoveredCard = this._localHoveredCard;
        if (localHoveredCard == null)
        {
          PotionModel localHoveredPotion = this._localHoveredPotion;
          model = localHoveredPotion != null ? (AbstractModel) localHoveredPotion : (AbstractModel) this._localHoveredRelic;
        }
        else
          model = (AbstractModel) localHoveredCard;
      }
      else
        model = (AbstractModel) localSelectedPotion;
    }
    this._inputSynchronizer.SyncLocalHoveredModel(model);
  }
}
