// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.MendRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public sealed class MendRestSiteOption(Player owner) : RestSiteOption(owner)
{
  private const string _hasTargetKey = "HasTarget";
  private const string _playerNameKey = "Name";
  private readonly HealVar _healVar = new HealVar(0M);
  private LocString? _description;

  public override string OptionId => "MEND";

  public override LocString Description
  {
    get
    {
      if (this._description == null)
      {
        this._description = base.Description;
        this._description.Add("HasTarget", false);
        this._description.Add("Name", "");
        this._description.Add((DynamicVar) this._healVar);
      }
      return this._description;
    }
  }

  public static Decimal GetHealAmount(Player player)
  {
    return Hook.ModifyRestSiteHealAmount(player.RunState, player.Creature, HealRestSiteOption.GetBaseHealAmount(player.Creature));
  }

  public override async Task<bool> OnSelect()
  {
    uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(this.Owner);
    Player target = (Player) null;
    if (LocalContext.IsMe(this.Owner))
    {
      NRestSiteRoom.Instance.AnimateDescriptionDown();
      NRestSiteButton buttonForOption = NRestSiteRoom.Instance.GetButtonForOption((RestSiteOption) this);
      Vector2 startPosition = Vector2.op_Addition(buttonForOption.GlobalPosition, Vector2.op_Division(buttonForOption.Size, 2f));
      bool usingController = NControllerManager.Instance.IsUsingController;
      NTargetManager targetManager = NTargetManager.Instance;
      targetManager.StartTargeting(TargetType.AnyPlayer, startPosition, usingController ? TargetMode.Controller : TargetMode.ClickMouseToTarget, new Func<bool>(this.ShouldCancelTargeting), new Func<Node, bool>(this.AllowHoveringNode));
      if (usingController)
      {
        List<NRestSiteCharacter> list = NRestSiteRoom.Instance.characterAnims.Where<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player != this.Owner)).ToList<NRestSiteCharacter>();
        for (int index = 0; index < list.Count; ++index)
        {
          list[index].Hitbox.SetFocusMode((Control.FocusModeEnum) 2L);
          list[index].Hitbox.FocusNeighborTop = ((Node) list[index].Hitbox).GetPath();
          list[index].Hitbox.FocusNeighborBottom = ((Node) list[index].Hitbox).GetPath();
          Control hitbox = list[index].Hitbox;
          NodePath path;
          if (index <= 0)
          {
            List<NRestSiteCharacter> nrestSiteCharacterList = list;
            path = ((Node) nrestSiteCharacterList[nrestSiteCharacterList.Count - 1].Hitbox).GetPath();
          }
          else
            path = ((Node) list[index - 1].Hitbox).GetPath();
          hitbox.FocusNeighborLeft = path;
          list[index].Hitbox.FocusNeighborRight = index < list.Count - 1 ? ((Node) list[index + 1].Hitbox).GetPath() : ((Node) list[0].Hitbox).GetPath();
        }
        NRestSiteCharacter nrestSiteCharacter = list.FirstOrDefault<NRestSiteCharacter>();
        if (nrestSiteCharacter != null)
          nrestSiteCharacter.Hitbox.TryGrabFocus();
      }
      ((GodotObject) targetManager).Connect(NTargetManager.SignalName.NodeHovered, Callable.From<Node>(new Action<Node>(this.OnNodeHovered)), 0U);
      ((GodotObject) targetManager).Connect(NTargetManager.SignalName.NodeUnhovered, Callable.From<Node>(new Action<Node>(this.OnNodeUnhovered)), 0U);
      try
      {
        target = this.NodeToPlayer(await targetManager.SelectionFinished());
        RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(this.Owner, choiceId, PlayerChoiceResult.FromPlayerId(target?.NetId));
      }
      finally
      {
        ((GodotObject) targetManager).Disconnect(NTargetManager.SignalName.NodeHovered, Callable.From<Node>(new Action<Node>(this.OnNodeHovered)));
        ((GodotObject) targetManager).Disconnect(NTargetManager.SignalName.NodeUnhovered, Callable.From<Node>(new Action<Node>(this.OnNodeUnhovered)));
        if (usingController)
        {
          foreach (NRestSiteCharacter characterAnim in NRestSiteRoom.Instance.characterAnims)
            characterAnim.Hitbox.SetFocusMode((Control.FocusModeEnum) 0L);
        }
      }
      targetManager = (NTargetManager) null;
    }
    else
    {
      ulong? nullable = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(this.Owner, choiceId)).AsPlayerId();
      if (nullable.HasValue)
        target = this.Owner.RunState.GetPlayer(nullable.Value);
    }
    NRestSiteRoom.Instance?.AnimateDescriptionUp();
    this.Description.Add("HasTarget", false);
    NRestSiteRoom.Instance?.GetButtonForOption((RestSiteOption) this)?.RefreshTextState();
    if (target == null)
      return false;
    await CreatureCmd.Heal(target.Creature, MendRestSiteOption.GetHealAmount(target));
    await Hook.AfterRestSiteHeal(target.RunState, target, false);
    if (TestMode.IsOff)
    {
      Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("vfx/vfx_cross_heal")).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
      NRestSiteCharacter characterForPlayer = NRestSiteRoom.Instance?.GetCharacterForPlayer(target);
      if (characterForPlayer != null)
        ((Node) characterForPlayer).AddChildSafely((Node) child);
      child.Position = Vector2.Zero;
    }
    return true;
  }

  private void OnNodeHovered(Node node)
  {
    Player player = this.NodeToPlayer(node);
    if (player == null)
      return;
    this.Description.Add("HasTarget", true);
    this.Description.Add("Name", PlatformUtil.GetPlayerName(RunManager.Instance.NetService.Platform, player.NetId));
    this._healVar.BaseValue = HealRestSiteOption.GetBaseHealAmount(player.Creature);
    this._healVar.PreviewValue = MendRestSiteOption.GetHealAmount(player);
    NRestSiteRoom.Instance?.GetButtonForOption((RestSiteOption) this)?.RefreshTextState();
  }

  private void OnNodeUnhovered(Node _)
  {
    this.Description.Add("HasTarget", false);
    NRestSiteRoom.Instance?.GetButtonForOption((RestSiteOption) this)?.RefreshTextState();
  }

  private Player? NodeToPlayer(Node? node)
  {
    Player player;
    switch (node)
    {
      case null:
        return (Player) null;
      case NMultiplayerPlayerState nmultiplayerPlayerState:
        player = nmultiplayerPlayerState.Player;
        break;
      case NRestSiteCharacter nrestSiteCharacter:
        player = nrestSiteCharacter.Player;
        break;
      default:
        player = (Player) null;
        break;
    }
    return player;
  }

  private bool ShouldCancelTargeting()
  {
    return NOverlayStack.Instance.ScreenCount > 0 || NCapstoneContainer.Instance.InUse;
  }

  private bool AllowHoveringNode(Node node) => !LocalContext.IsMe(this.NodeToPlayer(node));
}
