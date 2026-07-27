// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.FoulPotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class FoulPotion : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Event;

  public override PotionUsage Usage => PotionUsage.AnyTime;

  public override TargetType TargetType
  {
    get
    {
      return !CombatManager.Instance.IsInProgress ? TargetType.TargetedNoCreature : TargetType.AllEnemies;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(12M, ValueProp.Unpowered),
        (DynamicVar) new GoldVar(100)
      });
    }
  }

  public override bool PassesCustomUsabilityCheck
  {
    get
    {
      if (CombatManager.Instance.IsInProgress)
        return true;
      AbstractRoom currentRoom = this.Owner.RunState.CurrentRoom;
      bool flag;
      switch (currentRoom)
      {
        case MerchantRoom _:
label_4:
          flag = true;
          break;
        case EventRoom eventRoom:
          if (!(eventRoom.CanonicalEvent is FakeMerchant))
            goto default;
          goto label_4;
        default:
          flag = false;
          break;
      }
      return flag && FoulPotion.GetFoulPotionMerchantTarget(currentRoom).button != null;
    }
  }

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    if (CombatManager.Instance.IsInProgress)
    {
      Creature creature = this.Owner.Creature;
      DamageVar damage = this.DynamicVars.Damage;
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature.CombatState.Creatures.Where<Creature>((Func<Creature, bool>) (c => !c.IsPet)), damage.BaseValue, damage.Props, creature, (CardModel) null, (CardPlay) null);
    }
    else if (this.Owner.RunState.CurrentRoom is MerchantRoom)
    {
      NMerchantRoom merchantRoom = NRun.Instance?.MerchantRoom;
      if (merchantRoom != null)
      {
        this.ShowPotionVfx(merchantRoom.MerchantButton);
        merchantRoom.FoulPotionThrown(this);
      }
      await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
    }
    else
    {
      if (!(this.Owner.RunState.CurrentRoom is EventRoom currentRoom) || !(currentRoom.CanonicalEvent is FakeMerchant))
        return;
      EventModel localMutableEvent = currentRoom.LocalMutableEvent;
      if (localMutableEvent == null || !(localMutableEvent.Node is NFakeMerchant node))
        return;
      this.ShowPotionVfx(node.MerchantButton);
      List<Task> taskList = new List<Task>();
      foreach (Player player in (IEnumerable<Player>) this.Owner.RunState.Players)
      {
        FakeMerchant eventForPlayer = (FakeMerchant) RunManager.Instance.EventSynchronizer.GetEventForPlayer(player);
        taskList.Add(eventForPlayer.FoulPotionThrown(this));
      }
      await Task.WhenAll((IEnumerable<Task>) taskList);
    }
  }

  private void ShowPotionVfx(NMerchantButton? merchantButton)
  {
    if (TestMode.IsOn || merchantButton == null)
      return;
    Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("vfx/vfx_slime_impact")).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    ((Node) merchantButton).GetParent().AddChildSafely((Node) child);
    child.GlobalPosition = merchantButton.GlobalPosition;
  }

  public static (NMerchantButton? button, Control? screenContext) GetFoulPotionMerchantTarget(
    AbstractRoom room)
  {
    if (room.RoomType == RoomType.Shop)
    {
      NMerchantRoom instance = NMerchantRoom.Instance;
      if (instance != null)
      {
        NMerchantInventory inventory = instance.Inventory;
        if (inventory != null && !inventory.IsOpen)
          return (instance.MerchantButton, (Control) instance);
      }
    }
    if (room is EventRoom eventRoom && eventRoom.CanonicalEvent is FakeMerchant)
    {
      EventModel localMutableEvent = eventRoom.LocalMutableEvent;
      if (localMutableEvent != null && localMutableEvent.Node is NFakeMerchant node)
      {
        NMerchantInventory inventory = node.Inventory;
        if (inventory != null && !inventory.IsOpen)
          return (node.MerchantButton, (Control) node);
      }
    }
    return ((NMerchantButton) null, (Control) null);
  }
}
