// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.MerchantRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class MerchantRoom : AbstractRoom
{
  private IRunState? _runState;
  private static MerchantDialogueSet? _dialogue;

  public override RoomType RoomType => RoomType.Shop;

  public List<MerchantInventory> Inventories { get; private set; } = new List<MerchantInventory>();

  public override ModelId? ModelId => (ModelId) null;

  public static MerchantDialogueSet Dialogue
  {
    get
    {
      if (MerchantRoom._dialogue != null)
        return MerchantRoom._dialogue;
      MerchantRoom._dialogue = MerchantDialogueSet.CreateFromLocStrings((IEnumerable<LocString>) LocManager.Instance.GetTable("merchant_room").GetLocStringsWithPrefix("MERCHANT.talk."));
      return MerchantRoom._dialogue;
    }
  }

  public MerchantInventory GetLocalInventory()
  {
    return this.Inventories[this._runState.GetPlayerSlotIndex(LocalContext.GetMe((IPlayerCollection) this._runState))];
  }

  public override async Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase)
  {
    if (isRestoringRoomStackBase)
      throw new InvalidOperationException("MerchantRoom does not support room stack reconstruction.");
    this._runState = runState;
    IRunState runState1 = runState;
    foreach (Player player in (IEnumerable<Player>) ((runState1 != null ? (object) runState1.Players : (object) null) ?? (object) Array.Empty<Player>()))
      this.Inventories.Add(MerchantInventory.CreateForNormalMerchant(player));
    await PreloadManager.LoadRoomMerchantAssets();
    NRun instance = NRun.Instance;
    if (instance != null)
    {
      IRunState runState2 = runState;
      instance.SetCurrentRoom((Control) NMerchantRoom.Create(this, (IReadOnlyList<Player>) ((runState2 != null ? (object) runState2.Players : (object) null) ?? (object) Array.Empty<Player>())));
    }
    if (runState == null)
      return;
    await Hook.AfterRoomEntered(runState, (AbstractRoom) this);
  }

  public override Task Exit(IRunState? runState)
  {
    if (TestMode.IsOn)
      return Task.CompletedTask;
    for (int index = 0; index < this.Inventories.Count; ++index)
    {
      MerchantInventory inventory = this.Inventories[index];
      Player player = this._runState.Players[index];
      PlayerMapPointHistoryEntry entry = runState?.CurrentMapPointHistoryEntry?.GetEntry(player.NetId);
      if (entry != null)
      {
        foreach (MerchantCardEntry characterCardEntry in (IEnumerable<MerchantCardEntry>) inventory.CharacterCardEntries)
        {
          if (characterCardEntry.IsStocked)
            entry.CardChoices.Add(new CardChoiceHistoryEntry(characterCardEntry.CreationResult.Card, false));
        }
        foreach (MerchantCardEntry colorlessCardEntry in (IEnumerable<MerchantCardEntry>) inventory.ColorlessCardEntries)
        {
          if (colorlessCardEntry.IsStocked)
            entry.CardChoices.Add(new CardChoiceHistoryEntry(colorlessCardEntry.CreationResult.Card, false));
        }
        foreach (MerchantRelicEntry relicEntry in (IEnumerable<MerchantRelicEntry>) inventory.RelicEntries)
        {
          if (relicEntry.IsStocked)
            entry.RelicChoices.Add(new ModelChoiceHistoryEntry(relicEntry.Model.Id, false));
        }
        foreach (MerchantPotionEntry potionEntry in (IEnumerable<MerchantPotionEntry>) inventory.PotionEntries)
        {
          if (potionEntry.IsStocked)
            entry.PotionChoices.Add(new ModelChoiceHistoryEntry(potionEntry.Model.Id, false));
        }
      }
    }
    return Task.CompletedTask;
  }

  public override Task Resume(AbstractRoom _, IRunState? runState)
  {
    throw new NotImplementedException();
  }
}
