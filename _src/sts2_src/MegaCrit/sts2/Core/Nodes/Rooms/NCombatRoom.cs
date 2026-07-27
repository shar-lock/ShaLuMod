// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NCombatRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NCombatRoom.cs")]
public class NCombatRoom : Control, IScreenContext, IRoomWithProceedButton
{
  private const float _centerSafeZone = 150f;
  private const float _defaultPadding = 70f;
  private const float _minimumAutoPadding = 5f;
  private const float _minAlternatingYPos = 40f;
  private const float _maxAlternatingYPos = 60f;
  private const float _alternateYPosBeginPadding = 30f;
  private const float _yPos = 200f;
  private static readonly LocString _waitingLoc = new LocString("gameplay_ui", "MULTIPLAYER_WAITING");
  private readonly List<NCreature> _creatureNodes = new List<NCreature>();
  private readonly List<NCreature> _removingCreatureNodes = new List<NCreature>();
  private Control _allyContainer;
  private Control _enemyContainer;
  private const string _scenePath = "res://scenes/rooms/combat_room.tscn";
  private NRadialBlurVfx _radialBlur;
  private NProceedButton _proceedButton;
  private Control _waitingForOtherPlayersOverlay;
  private ICombatRoomVisuals _visuals;
  private Window _window;

  public static NCombatRoom? Instance => NRun.Instance?.CombatRoom;

  public event Action? ProceedButtonPressed;

  public Creature? LastTargetedCreature { get; set; }

  public NCombatUi Ui { get; private set; }

  public IEnumerable<NCreature> CreatureNodes => (IEnumerable<NCreature>) this._creatureNodes;

  public IEnumerable<NCreature> RemovingCreatureNodes
  {
    get
    {
      return this._removingCreatureNodes.Where<NCreature>(NCombatRoom.\u003C\u003EO.\u003C0\u003E__IsInstanceValid ?? (NCombatRoom.\u003C\u003EO.\u003C0\u003E__IsInstanceValid = new Func<NCreature, bool>(GodotObject.IsInstanceValid)));
    }
  }

  public Control SceneContainer { get; private set; }

  private Control BgContainer { get; set; }

  public NCombatBackground Background { get; private set; }

  public NProceedButton ProceedButton => this._proceedButton;

  public Control BackCombatVfxContainer { get; private set; }

  public Control CombatVfxContainer { get; private set; }

  public ulong CreatedMsec { get; private set; }

  public CombatRoomMode Mode { get; private set; }

  private Control? EncounterSlots { get; set; }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/rooms/combat_room.tscn");
    }
  }

  public static NCombatRoom? Create(ICombatRoomVisuals visuals, CombatRoomMode mode)
  {
    if (TestMode.IsOn)
      return (NCombatRoom) null;
    NCombatRoom ncombatRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/combat_room.tscn").Instantiate<NCombatRoom>((PackedScene.GenEditState) 0L);
    ncombatRoom._visuals = visuals;
    ncombatRoom.Mode = mode;
    return ncombatRoom;
  }

  public override void _Ready()
  {
    this.Ui = ((Node) this).GetNode<NCombatUi>(NodePath.op_Implicit("%CombatUi"));
    this.Ui.Deactivate();
    this.SceneContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CombatSceneContainer"));
    this._allyContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AllyContainer"));
    this._enemyContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EnemyContainer"));
    this.BackCombatVfxContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BackCombatVfxContainer"));
    this.CombatVfxContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CombatVfxContainer"));
    this._radialBlur = ((Node) this).GetNode<NRadialBlurVfx>(NodePath.op_Implicit("RadialBlur"));
    this._waitingForOtherPlayersOverlay = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%WaitingForOtherPlayers"));
    ((Node) this._waitingForOtherPlayersOverlay).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(NCombatRoom._waitingLoc.GetRawText());
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    if (this.Mode == CombatRoomMode.VisualOnly && NEventRoom.Instance == null)
    {
      ((CanvasItem) this._proceedButton).Visible = true;
      ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnProceedButtonPressed)), 0U);
      this._proceedButton.Enable();
    }
    this.BgContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BgContainer"));
    this.CreatedMsec = Time.GetTicksMsec();
    Log.Info($"Creating NCombatRoom with mode={this.Mode} encounter={this._visuals.Encounter.Id.Entry}.");
    this.CreateAllyNodes();
    if (this.Mode != CombatRoomMode.FinishedCombat)
      this.CreateEnemyNodes();
    if (this.Mode != CombatRoomMode.ActiveCombat)
    {
      foreach (NCreature creatureNode in this._creatureNodes)
      {
        creatureNode.Hitbox.FocusMode = (Control.FocusModeEnum) 0L;
        this.SetCreatureIsInteractable(creatureNode.Entity, false);
      }
    }
    this.SceneContainer.Scale = Vector2.op_Multiply(Vector2.One, this._visuals.Encounter.GetCameraScaling());
    Control sceneContainer = this.SceneContainer;
    sceneContainer.Position = Vector2.op_Addition(sceneContainer.Position, this._visuals.Encounter.GetCameraOffset());
    ((CanvasItem) this.SceneContainer).ZIndex = -10;
    ((CanvasItem) this.CombatVfxContainer).ZIndex = -9;
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.AdjustCreatureScaleForAspectRatio)), 0U);
    this.AdjustCreatureScaleForAspectRatio();
    NGame.Instance.SetScreenShakeTarget(this.SceneContainer);
  }

  public static Rng GenerateBackgroundRngForCurrentPoint(IRunState state)
  {
    uint num = 0;
    if (state.CurrentMapCoord.HasValue)
      num = (uint) (state.CurrentMapCoord.Value.row + state.CurrentMapCoord.Value.col * 747);
    return new Rng(state.Rng.Seed + (ulong) num);
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenUpdated);
    if (this.Mode != CombatRoomMode.ActiveCombat)
      return;
    this.SubscribeToCombatEvents();
  }

  public override void _ExitTree()
  {
    NGame.Instance.ClearScreenShakeTarget();
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenUpdated);
    CombatManager.Instance.CombatSetUp -= new Action<CombatState>(this.OnCombatSetUp);
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.RestrictControllerNavigation);
    CombatManager.Instance.CombatWon -= new Action<CombatRoom>(this.RestrictControllerNavigation);
  }

  private void SubscribeToCombatEvents()
  {
    CombatManager.Instance.CombatSetUp += new Action<CombatState>(this.OnCombatSetUp);
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.RestrictControllerNavigation);
    CombatManager.Instance.CombatWon += new Action<CombatRoom>(this.RestrictControllerNavigation);
  }

  private void OnCombatSetUp(CombatState state)
  {
    this.Ui.Activate(((CombatRoom) this._visuals).CombatState);
    if (this.Background != null)
      return;
    this.SetUpBackground(state.RunState);
  }

  public void SetUpBackground(IRunState state)
  {
    if (this.Background != null)
      Log.Warn("Tried to set up background twice!");
    this.Background = this._visuals.Encounter.CreateBackground(this._visuals.Act, NCombatRoom.GenerateBackgroundRngForCurrentPoint(state));
    ((Node) this.BgContainer).AddChildSafely((Node) this.Background);
  }

  private void AdjustCreatureScaleForAspectRatio()
  {
    this._allyContainer.Scale = Vector2.One;
    this._enemyContainer.Scale = Vector2.One;
    this._enemyContainer.Position = Vector2.op_Multiply(this.SceneContainer.Size, 0.5f);
    float num1 = 0.0f;
    foreach (NCreature creatureNode in this._creatureNodes)
      num1 = Math.Max(creatureNode.GlobalPosition.X + creatureNode.Visuals.Bounds.Size.X * 0.5f * this.SceneContainer.Scale.X, num1);
    float num2 = num1 + 15f;
    if ((double) num2 <= (double) this.Size.X)
      return;
    float num3 = this.Size.X / num2;
    this._allyContainer.Scale = Vector2.op_Multiply(Vector2.One, num3);
    this._enemyContainer.Scale = Vector2.op_Multiply(Vector2.One, num3);
    Control enemyContainer = this._enemyContainer;
    enemyContainer.Position = Vector2.op_Addition(enemyContainer.Position, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, num2 - this.Size.X), num3));
  }

  private void CreateAllyNodes()
  {
    List<Creature> allies = this._visuals.Allies.ToList<Creature>();
    foreach (Creature creature in allies)
      this.AddCreature(creature);
    NCombatRoom.PositionPlayersAndPets(this._creatureNodes.Where<NCreature>((Func<NCreature, bool>) (c => allies.Contains(c.Entity))).ToList<NCreature>(), this._visuals.Encounter.GetCameraScaling(), this._visuals.Encounter.FullyCenterPlayers);
  }

  private void CreateEnemyNodes()
  {
    if (this._visuals.Encounter.HasScene)
      this.EncounterSlots = this._visuals.Encounter.CreateScene();
    if (this.EncounterSlots != null)
    {
      ((Node) this._enemyContainer).AddChildSafely((Node) this.EncounterSlots);
      Control encounterSlots = this.EncounterSlots;
      encounterSlots.Position = Vector2.op_Subtraction(encounterSlots.Position, Vector2.op_Multiply(new Vector2(1920f, 1080f), 0.5f));
    }
    List<Creature> enemies = this._visuals.Enemies.ToList<Creature>();
    foreach (Creature creature in enemies)
      this.AddCreature(creature);
    List<NCreature> list = this._creatureNodes.Where<NCreature>((Func<NCreature, bool>) (c => enemies.Contains(c.Entity))).ToList<NCreature>();
    if (this.EncounterSlots != null)
      this.PositionCreaturesWithSlots(list);
    else
      this.PositionEnemies(list, this._visuals.Encounter.GetCameraScaling());
    this.RandomizeEnemyScalesAndHues();
  }

  private void PositionCreaturesWithSlots(List<NCreature> creatures)
  {
    foreach (NCreature creature in creatures)
    {
      string slotName = creature.Entity.SlotName;
      creature.GlobalPosition = ((Node2D) ((Node) this.EncounterSlots).GetNode<Marker2D>(NodePath.op_Implicit(slotName))).GlobalPosition;
    }
  }

  private void PositionEnemies(List<NCreature> creatures, float scaling)
  {
    float num1 = 960f / scaling;
    float num2 = 70f;
    float num3 = creatures.Sum<NCreature>((Func<NCreature, float>) (n => n.Visuals.Bounds.Size.X));
    float num4 = num3 + (float) (creatures.Count - 1) * num2;
    float num5 = Math.Max((float) (((double) num1 - (double) num4) * 0.5), 150f);
    float num6 = 0.0f;
    if ((double) num5 + (double) num4 > (double) num1)
    {
      num2 = Math.Max((num1 - 150f - num3) / (float) (creatures.Count - 1), 5f);
      float num7 = num3 + (float) (creatures.Count - 1) * num2;
      num5 = (float) (((double) num1 - (double) num7) * 0.5);
      if ((double) num2 < 30.0)
        num6 = float.Lerp(60f, 40f, (float) (((double) num2 - 5.0) / 25.0));
      if ((double) num5 + (double) num7 > (double) num1)
        Log.Warn($"Creatures for current encounter ({this._visuals.Encounter.Title.GetFormattedText()}) are being displayed off-screen because they are too wide!");
    }
    for (int index = 0; index < creatures.Count; ++index)
    {
      NCreature creature = creatures[index];
      creature.Position = new Vector2(num5 + creature.Visuals.Bounds.Size.X * 0.5f, (float) (200.0 - (index % 2 != 0 ? (double) num6 : 0.0)));
      num5 += creature.Visuals.Bounds.Size.X + num2;
    }
  }

  public static void PositionPlayersAndPets(
    List<NCreature> creatureNodes,
    float scaling,
    bool fullyCenterPlayers)
  {
    List<NCombatRoom.PlayerAndPets> source = new List<NCombatRoom.PlayerAndPets>();
    foreach (NCreature creatureNode in creatureNodes)
    {
      if (creatureNode.Entity.IsPlayer)
      {
        NCombatRoom.PlayerAndPets playerAndPets = new NCombatRoom.PlayerAndPets()
        {
          player = creatureNode,
          pets = new List<NCreature>()
        };
        if (LocalContext.IsMe(creatureNode.Entity))
          source.Insert(0, playerAndPets);
        else
          source.Add(playerAndPets);
      }
    }
    foreach (NCreature creatureNode in creatureNodes)
    {
      NCreature creature = creatureNode;
      if (!creature.Entity.IsPlayer)
        source.First<NCombatRoom.PlayerAndPets>((Func<NCombatRoom.PlayerAndPets, bool>) (p => p.player.Entity.Player == creature.Entity.PetOwner)).pets.Add(creature);
    }
    float num1 = 960f / scaling;
    float num2 = 70f;
    int count = (int) Math.Ceiling(Math.Sqrt((double) source.Count));
    int num3 = (int) Math.Ceiling((double) source.Count / (double) count);
    float num4 = creatureNodes.Take<NCreature>(count).Sum<NCreature>((Func<NCreature, float>) (n => n.Visuals.Bounds.Size.X));
    float num5 = num4 + (float) (count - 1) * num2;
    float num6 = num4 * 0.33f;
    float num7 = num3 > 1 ? num6 / (float) (num3 - 1) : 0.0f;
    float num8 = num3 > 1 ? 120f / (float) (num3 - 1) : 0.0f;
    float num9;
    if (fullyCenterPlayers)
    {
      num9 = creatureNodes.First<NCreature>((Func<NCreature, bool>) (c => c.Entity.IsPlayer)).Visuals.Bounds.Size.X * -0.5f;
    }
    else
    {
      num9 = Math.Max((float) (((double) num1 - (double) num5) * 0.5), 150f);
      if (source.Count >= count * 2)
        num4 += num6;
      if ((double) num9 + (double) num5 > (double) num1)
      {
        num2 = (num1 - 150f - num4) / (float) (count - 1);
        float num10 = num4 + (float) (count - 1) * num2;
        num9 = (float) (((double) num1 - (double) num10) * 0.5);
      }
    }
    for (int index1 = 0; index1 < count; ++index1)
    {
      float targetXPos = num9 + num7 * (float) index1;
      for (int index2 = 0; index2 < count; ++index2)
      {
        int index3 = index1 * count + index2;
        if (index3 < source.Count)
        {
          NCombatRoom.PlayerAndPets playerAndPets = source[index3];
          NCreature player = playerAndPets.player;
          List<NCreature> pets = playerAndPets.pets;
          player.Position = new Vector2((float) (-(double) targetXPos - (double) player.Visuals.Bounds.Size.X * 0.5), (float) (200.0 - (double) num8 * (double) index1));
          if (LocalContext.IsMe(player.Entity) && player.Entity.Player.Character is Necrobinder)
          {
            NCreature osty = (NCreature) null;
            for (int index4 = 0; index4 < pets.Count; ++index4)
            {
              NCreature ncreature = pets[index4];
              if (ncreature.Entity.Monster is Osty)
              {
                osty = ncreature;
                pets.RemoveAt(index4);
                break;
              }
            }
            NCombatRoom.PositionLocalPlayerOsty(ref targetXPos, player.Position.Y, player, osty);
          }
          float num11 = pets.Count > 1 ? player.Visuals.Bounds.Size.X / (float) (pets.Count - 1) : 0.0f;
          for (int index5 = 0; index5 < pets.Count; ++index5)
          {
            NCreature ncreature = pets[index5];
            ncreature.Position = new Vector2((float) (-(double) targetXPos + 20.0 - (double) index5 * (double) num11 - (double) ncreature.Visuals.Bounds.Size.X * 0.5), player.Position.Y + 10f);
          }
          if (index1 > 0)
          {
            ((CanvasItem) playerAndPets.player.Visuals).Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
            foreach (NCreature ncreature in pets)
              ((CanvasItem) ncreature.Visuals).Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
          }
          targetXPos += playerAndPets.player.Visuals.Bounds.Size.X + num2;
        }
        else
          break;
      }
    }
    foreach (NCombatRoom.PlayerAndPets playerAndPets in source)
    {
      ((Node) playerAndPets.player).GetParent().MoveChildSafely((Node) playerAndPets.player, 0);
      for (int index = 0; index < playerAndPets.pets.Count; ++index)
      {
        NCreature pet = playerAndPets.pets[index];
        ((Node) pet).GetParent().MoveChildSafely((Node) pet, index + 1);
        if (!LocalContext.IsMe(playerAndPets.player.Entity))
          ((CanvasItem) pet.Visuals.Bounds).Visible = false;
      }
    }
  }

  private static void PositionLocalPlayerOsty(
    ref float targetXPos,
    float playerYPosition,
    NCreature player,
    NCreature? osty)
  {
    NCreature ncreature = player;
    Vector2 position = player.Position;
    position.X = player.Position.X - 150f;
    Vector2 vector2 = position;
    ncreature.Position = vector2;
    if (osty != null)
      osty.Position = Vector2.op_Addition(new Vector2(-targetXPos, playerYPosition), NCreature.GetOstyOffsetFromPlayer(osty.Entity));
    targetXPos += 100f;
  }

  public NCreature? GetCreatureNode(Creature? creature)
  {
    return creature != null ? this.CreatureNodes.FirstOrDefault<NCreature>((Func<NCreature, bool>) (c => c.Entity == creature)) : (NCreature) null;
  }

  public void RemoveCreatureNode(NCreature node)
  {
    this._creatureNodes.Remove(node);
    this._removingCreatureNodes.Add(node);
    this.UpdateCreatureNavigation();
    if (((Node) this).GetViewport().GuiGetFocusOwner() == node.Hitbox)
      this._creatureNodes[0].Hitbox.TryGrabFocus();
    TaskHelper.RunSafely(this.RemoveCreatureWhenGone(node));
  }

  private async Task RemoveCreatureWhenGone(NCreature node)
  {
    if (node.DeathAnimationTask != null)
      await node.DeathAnimationTask;
    if (!((Node) this).IsValid())
      return;
    this._removingCreatureNodes.Remove(node);
  }

  public void AddCreature(Creature creature)
  {
    NCreature child = NCreature.Create(creature);
    this._creatureNodes.Add(child);
    if (creature.IsPlayer || creature.PetOwner != null)
      ((Node) this._allyContainer).AddChildSafely((Node) child);
    else
      ((Node) this._enemyContainer).AddChildSafely((Node) child);
    if (creature.SlotName != null)
    {
      if (this.EncounterSlots == null)
        throw new InvalidOperationException($"Creature {creature} has slot name '{creature.SlotName}' but NCombatRoom.EncounterSlots is null.");
      child.GlobalPosition = ((Node2D) ((Node) this.EncounterSlots).GetNode<Marker2D>(NodePath.op_Implicit(creature.SlotName))).GlobalPosition;
    }
    this.UpdateCreatureNavigation();
    if (creature.PetOwner == null)
      return;
    NCreature creatureNode = this.GetCreatureNode(creature.PetOwner.Creature);
    Player player = creatureNode.Entity.Player;
    List<NCreature> list = this._creatureNodes.Where<NCreature>((Func<NCreature, bool>) (c =>
    {
      if (c.Entity.PetOwner != player)
        return false;
      return !(c.Entity.Monster is Osty) || !LocalContext.IsMe(player);
    })).ToList<NCreature>();
    ((Node) child).GetParent().MoveChildSafely((Node) child, ((Node) creatureNode).GetIndex(false) + 1);
    if (creature.Monster is Osty && LocalContext.IsMe(player))
    {
      child.OstyScaleToSize((float) creature.MaxHp, 0.0);
      ((Node) child).GetParent().MoveChildSafely((Node) child, ((Node) creatureNode).GetIndex(false));
    }
    else
    {
      float num = list.Count > 1 ? creatureNode.Visuals.Bounds.Size.X / (float) (list.Count - 1) : 0.0f;
      for (int index = 0; index < list.Count; ++index)
      {
        NCreature ncreature = list[index];
        ncreature.Position = new Vector2((float) ((double) creatureNode.Position.X - 20.0 + (double) index * (double) num + (double) ncreature.Visuals.Bounds.Size.X * 0.5), creatureNode.Position.Y + 10f);
        ncreature.ToggleIsInteractable(false);
      }
      if ((double) creatureNode.Position.Y >= 199.0)
        return;
      ((CanvasItem) child.Visuals).Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
    }
  }

  public void SetCreatureIsInteractable(Creature? creature, bool on)
  {
    this.GetCreatureNode(creature)?.ToggleIsInteractable(on);
    this.UpdateCreatureNavigation();
  }

  private void UpdateCreatureNavigation()
  {
    List<NCreature> list = this._creatureNodes.Where<NCreature>((Func<NCreature, bool>) (c => c.IsInteractable)).OrderBy<NCreature, float>((Func<NCreature, float>) (n => n.GlobalPosition.X)).ToList<NCreature>();
    for (int index = 0; index < list.Count; ++index)
    {
      Control hitbox = list[index].Hitbox;
      NodePath path;
      if (index <= 0)
      {
        List<NCreature> ncreatureList = list;
        path = ((Node) ncreatureList[ncreatureList.Count - 1].Hitbox).GetPath();
      }
      else
        path = ((Node) list[index - 1].Hitbox).GetPath();
      hitbox.FocusNeighborLeft = path;
      list[index].Hitbox.FocusNeighborRight = index < list.Count - 1 ? ((Node) list[index + 1].Hitbox).GetPath() : ((Node) list[0].Hitbox).GetPath();
      list[index].Hitbox.FocusNeighborBottom = ((Node) this.Ui.Hand.CardHolderContainer).GetPath();
      list[index].Hitbox.FocusNeighborTop = ((Node) list[index].Hitbox).GetPath();
      list[index].UpdateNavigation();
    }
    this.Ui.Hand.CardHolderContainer.FocusNeighborTop = ((Node) NCombatRoom.Instance.CreatureNodes.FirstOrDefault<NCreature>()?.Hitbox).GetPath();
  }

  public Control DefaultFocusedControl => this.Ui.Hand.CardHolderContainer;

  public Control FocusedControlFromTopBar
  {
    get
    {
      return this._creatureNodes.FirstOrDefault<NCreature>((Func<NCreature, bool>) (c =>
      {
        if (c != null && c.IsInteractable)
        {
          Control hitbox = c.Hitbox;
          if (hitbox != null)
            return hitbox.FocusMode == 2L;
        }
        return false;
      }))?.Hitbox ?? this.DefaultFocusedControl;
    }
  }

  private void OnActiveScreenUpdated()
  {
    if (!CombatManager.Instance.IsInProgress)
      return;
    this.UpdateControllerNavEnabled<NCombatRoom>();
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
      this.Ui.Enable();
    else
      this.Ui.Disable();
  }

  private void RestrictControllerNavigation(CombatRoom _)
  {
    this.RestrictControllerNavigation((IEnumerable<Control>) Array.Empty<Control>());
  }

  public void RestrictControllerNavigation(IEnumerable<Control> whitelist)
  {
    foreach (NCreature creatureNode in this._creatureNodes)
    {
      Control hitbox = creatureNode.Hitbox;
      bool flag = whitelist.Contains<Control>(hitbox);
      hitbox.FocusMode = flag ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
      hitbox.FocusNeighborBottom = ((Node) creatureNode.Hitbox).GetPath();
      creatureNode.OrbManager?.SetFocusBehaviorRecursive(flag ? (Control.FocusBehaviorRecursiveEnum) 0L : (Control.FocusBehaviorRecursiveEnum) 1L);
    }
    this.Ui.Hand.DisableControllerNavigation();
  }

  public void EnableControllerNavigation()
  {
    if (!((Node) this).IsInsideTree())
      return;
    foreach (NCreature creatureNode in this._creatureNodes)
    {
      if (!creatureNode.Entity.IsDead)
      {
        creatureNode.Hitbox.FocusMode = (Control.FocusModeEnum) 2L;
        creatureNode.Hitbox.FocusNeighborBottom = ((Node) this.Ui.Hand.CardHolderContainer).GetPath();
        creatureNode.OrbManager?.SetFocusBehaviorRecursive((Control.FocusBehaviorRecursiveEnum) 0L);
      }
    }
    this.UpdateCreatureNavigation();
    this.Ui.Hand.EnableControllerNavigation();
  }

  private void RandomizeEnemyScalesAndHues()
  {
    Dictionary<Type, List<NCreature>> dictionary = new Dictionary<Type, List<NCreature>>();
    foreach (NCreature creatureNode in this._creatureNodes)
    {
      if (creatureNode.Entity.Side != CombatSide.Player && creatureNode.Entity.Monster != null)
      {
        Type type = creatureNode.Entity.Monster.GetType();
        List<NCreature> ncreatureList;
        if (!dictionary.TryGetValue(type, out ncreatureList))
        {
          ncreatureList = new List<NCreature>();
          dictionary[type] = ncreatureList;
        }
        ncreatureList.Add(creatureNode);
      }
    }
    foreach (KeyValuePair<Type, List<NCreature>> keyValuePair in dictionary)
    {
      if (keyValuePair.Value.Count != 1)
      {
        foreach (NCreature ncreature in keyValuePair.Value)
        {
          MonsterModel monster = ncreature.Entity.Monster;
          int num1 = monster.Creature.MonsterMaxHpBeforeModification.Value;
          float num2 = Mathf.Clamp(monster.MaxInitialHp != monster.MinInitialHp ? (float) (((double) (num1 - monster.MinInitialHp) / (double) (monster.MaxInitialHp - monster.MinInitialHp) - 0.5) * 2.0) : 0.0f, 0.0f, 1f);
          float num3 = float.Lerp(0.1f, 0.15f, Mathf.Clamp(Mathf.InverseLerp(250f, 100f, Math.Max(ncreature.Visuals.Bounds.Size.X, ncreature.Visuals.Bounds.Size.Y)), 0.0f, 1f));
          ncreature.SetScaleAndHue((float) (1.0 + (double) num2 * (double) num3), Rng.Chaotic.NextFloat(0.05f));
        }
      }
    }
  }

  public void RadialBlur(VfxPosition vfxPosition = VfxPosition.Center)
  {
    this._radialBlur.Activate(vfxPosition);
  }

  public void ShakeOstyIfDead(Player owner)
  {
    this._creatureNodes.FirstOrDefault<NCreature>((Func<NCreature, bool>) (c => c.Entity.Monster is Osty && c.Entity.PetOwner == owner))?.AnimShake();
  }

  public void PlaySplashVfx(Creature target, Color tint)
  {
    NCreature creatureNode = this.GetCreatureNode(target);
    if (creatureNode == null)
      return;
    Control combatVfxContainer = this.CombatVfxContainer;
    ((Node) combatVfxContainer).AddChildSafely((Node) NSplashVfx.Create(creatureNode.GetBottomOfHitbox(), tint));
    ((Node) combatVfxContainer).AddChildSafely((Node) NLiquidOverlayVfx.Create(target, tint));
  }

  public void SetWaitingForOtherPlayersOverlayVisible(bool visible)
  {
    ((CanvasItem) this._waitingForOtherPlayersOverlay).Visible = visible;
  }

  private void OnProceedButtonPressed(NButton button)
  {
    this._proceedButton.Disable();
    Action proceedButtonPressed = this.ProceedButtonPressed;
    if (proceedButtonPressed == null)
      return;
    proceedButtonPressed();
  }

  public void TransitionToActiveCombat(CombatRoom combatRoom)
  {
    this.Mode = this.Mode == CombatRoomMode.VisualOnly ? CombatRoomMode.ActiveCombat : throw new InvalidOperationException($"Cannot transition to {"ActiveCombat"} from {this.Mode}.");
    this._visuals = (ICombatRoomVisuals) combatRoom;
    foreach (NCreature creatureNode in this._creatureNodes)
    {
      creatureNode.Hitbox.FocusMode = (Control.FocusModeEnum) 2L;
      this.SetCreatureIsInteractable(creatureNode.Entity, true);
    }
    this.SubscribeToCombatEvents();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(15)
    {
      new MethodInfo(NCombatRoom.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.SubscribeToCombatEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.AdjustCreatureScaleForAspectRatio, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.CreateAllyNodes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.CreateEnemyNodes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.RemoveCreatureNode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.UpdateCreatureNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.EnableControllerNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.RandomizeEnemyScalesAndHues, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.RadialBlur, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vfxPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.SetWaitingForOtherPlayersOverlayVisible, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("visible"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatRoom.MethodName.OnProceedButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.SubscribeToCombatEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SubscribeToCombatEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.AdjustCreatureScaleForAspectRatio) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AdjustCreatureScaleForAspectRatio();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.CreateAllyNodes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateAllyNodes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.CreateEnemyNodes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateEnemyNodes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.RemoveCreatureNode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemoveCreatureNode(VariantUtils.ConvertTo<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.UpdateCreatureNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateCreatureNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.OnActiveScreenUpdated) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnActiveScreenUpdated();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.EnableControllerNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableControllerNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.RandomizeEnemyScalesAndHues) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RandomizeEnemyScalesAndHues();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.RadialBlur) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RadialBlur(VariantUtils.ConvertTo<VfxPosition>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatRoom.MethodName.SetWaitingForOtherPlayersOverlayVisible) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetWaitingForOtherPlayersOverlayVisible(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatRoom.MethodName.OnProceedButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnProceedButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatRoom.MethodName._Ready) || StringName.op_Equality(ref method, NCombatRoom.MethodName._EnterTree) || StringName.op_Equality(ref method, NCombatRoom.MethodName._ExitTree) || StringName.op_Equality(ref method, NCombatRoom.MethodName.SubscribeToCombatEvents) || StringName.op_Equality(ref method, NCombatRoom.MethodName.AdjustCreatureScaleForAspectRatio) || StringName.op_Equality(ref method, NCombatRoom.MethodName.CreateAllyNodes) || StringName.op_Equality(ref method, NCombatRoom.MethodName.CreateEnemyNodes) || StringName.op_Equality(ref method, NCombatRoom.MethodName.RemoveCreatureNode) || StringName.op_Equality(ref method, NCombatRoom.MethodName.UpdateCreatureNavigation) || StringName.op_Equality(ref method, NCombatRoom.MethodName.OnActiveScreenUpdated) || StringName.op_Equality(ref method, NCombatRoom.MethodName.EnableControllerNavigation) || StringName.op_Equality(ref method, NCombatRoom.MethodName.RandomizeEnemyScalesAndHues) || StringName.op_Equality(ref method, NCombatRoom.MethodName.RadialBlur) || StringName.op_Equality(ref method, NCombatRoom.MethodName.SetWaitingForOtherPlayersOverlayVisible) || StringName.op_Equality(ref method, NCombatRoom.MethodName.OnProceedButtonPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.Ui))
    {
      this.Ui = VariantUtils.ConvertTo<NCombatUi>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.SceneContainer))
    {
      this.SceneContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.BgContainer))
    {
      this.BgContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.Background))
    {
      this.Background = VariantUtils.ConvertTo<NCombatBackground>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.BackCombatVfxContainer))
    {
      this.BackCombatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.CombatVfxContainer))
    {
      this.CombatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.CreatedMsec))
    {
      this.CreatedMsec = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.Mode))
    {
      this.Mode = VariantUtils.ConvertTo<CombatRoomMode>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.EncounterSlots))
    {
      this.EncounterSlots = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._allyContainer))
    {
      this._allyContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._enemyContainer))
    {
      this._enemyContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._radialBlur))
    {
      this._radialBlur = VariantUtils.ConvertTo<NRadialBlurVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._waitingForOtherPlayersOverlay))
    {
      this._waitingForOtherPlayersOverlay = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatRoom.PropertyName._window))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._window = VariantUtils.ConvertTo<Window>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.Ui))
    {
      ref godot_variant local = ref value;
      NCombatUi ui = this.Ui;
      godot_variant from = VariantUtils.CreateFrom<NCombatUi>(ref ui);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.SceneContainer))
    {
      ref godot_variant local = ref value;
      Control sceneContainer = this.SceneContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref sceneContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.BgContainer))
    {
      ref godot_variant local = ref value;
      Control bgContainer = this.BgContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref bgContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.Background))
    {
      ref godot_variant local = ref value;
      NCombatBackground background = this.Background;
      godot_variant from = VariantUtils.CreateFrom<NCombatBackground>(ref background);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.ProceedButton))
    {
      ref godot_variant local = ref value;
      NProceedButton proceedButton = this.ProceedButton;
      godot_variant from = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.BackCombatVfxContainer))
    {
      ref godot_variant local = ref value;
      Control combatVfxContainer = this.BackCombatVfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref combatVfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.CombatVfxContainer))
    {
      ref godot_variant local = ref value;
      Control combatVfxContainer = this.CombatVfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref combatVfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.CreatedMsec))
    {
      ref godot_variant local = ref value;
      ulong createdMsec = this.CreatedMsec;
      godot_variant from = VariantUtils.CreateFrom<ulong>(ref createdMsec);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.Mode))
    {
      ref godot_variant local = ref value;
      CombatRoomMode mode = this.Mode;
      godot_variant from = VariantUtils.CreateFrom<CombatRoomMode>(ref mode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.EncounterSlots))
    {
      ref godot_variant local = ref value;
      Control encounterSlots = this.EncounterSlots;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref encounterSlots);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._allyContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._allyContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._enemyContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._enemyContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._radialBlur))
    {
      value = VariantUtils.CreateFrom<NRadialBlurVfx>(ref this._radialBlur);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatRoom.PropertyName._waitingForOtherPlayersOverlay))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._waitingForOtherPlayersOverlay);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatRoom.PropertyName._window))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Window>(ref this._window);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.Ui, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName._allyContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName._enemyContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.SceneContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.BgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.Background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName._radialBlur, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.ProceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName._waitingForOtherPlayersOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.BackCombatVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.CombatVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCombatRoom.PropertyName.CreatedMsec, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCombatRoom.PropertyName.Mode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.EncounterSlots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatRoom.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName ui1 = NCombatRoom.PropertyName.Ui;
    NCombatUi ui2 = this.Ui;
    Variant variant1 = Variant.From<NCombatUi>(ref ui2);
    serializationInfo1.AddProperty(ui1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName sceneContainer1 = NCombatRoom.PropertyName.SceneContainer;
    Control sceneContainer2 = this.SceneContainer;
    Variant variant2 = Variant.From<Control>(ref sceneContainer2);
    serializationInfo2.AddProperty(sceneContainer1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName bgContainer1 = NCombatRoom.PropertyName.BgContainer;
    Control bgContainer2 = this.BgContainer;
    Variant variant3 = Variant.From<Control>(ref bgContainer2);
    serializationInfo3.AddProperty(bgContainer1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName background1 = NCombatRoom.PropertyName.Background;
    NCombatBackground background2 = this.Background;
    Variant variant4 = Variant.From<NCombatBackground>(ref background2);
    serializationInfo4.AddProperty(background1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName combatVfxContainer1 = NCombatRoom.PropertyName.BackCombatVfxContainer;
    Control combatVfxContainer2 = this.BackCombatVfxContainer;
    Variant variant5 = Variant.From<Control>(ref combatVfxContainer2);
    serializationInfo5.AddProperty(combatVfxContainer1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName combatVfxContainer3 = NCombatRoom.PropertyName.CombatVfxContainer;
    Control combatVfxContainer4 = this.CombatVfxContainer;
    Variant variant6 = Variant.From<Control>(ref combatVfxContainer4);
    serializationInfo6.AddProperty(combatVfxContainer3, variant6);
    GodotSerializationInfo serializationInfo7 = info;
    StringName createdMsec1 = NCombatRoom.PropertyName.CreatedMsec;
    ulong createdMsec2 = this.CreatedMsec;
    Variant variant7 = Variant.From<ulong>(ref createdMsec2);
    serializationInfo7.AddProperty(createdMsec1, variant7);
    GodotSerializationInfo serializationInfo8 = info;
    StringName mode1 = NCombatRoom.PropertyName.Mode;
    CombatRoomMode mode2 = this.Mode;
    Variant variant8 = Variant.From<CombatRoomMode>(ref mode2);
    serializationInfo8.AddProperty(mode1, variant8);
    GodotSerializationInfo serializationInfo9 = info;
    StringName encounterSlots1 = NCombatRoom.PropertyName.EncounterSlots;
    Control encounterSlots2 = this.EncounterSlots;
    Variant variant9 = Variant.From<Control>(ref encounterSlots2);
    serializationInfo9.AddProperty(encounterSlots1, variant9);
    info.AddProperty(NCombatRoom.PropertyName._allyContainer, Variant.From<Control>(ref this._allyContainer));
    info.AddProperty(NCombatRoom.PropertyName._enemyContainer, Variant.From<Control>(ref this._enemyContainer));
    info.AddProperty(NCombatRoom.PropertyName._radialBlur, Variant.From<NRadialBlurVfx>(ref this._radialBlur));
    info.AddProperty(NCombatRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NCombatRoom.PropertyName._waitingForOtherPlayersOverlay, Variant.From<Control>(ref this._waitingForOtherPlayersOverlay));
    info.AddProperty(NCombatRoom.PropertyName._window, Variant.From<Window>(ref this._window));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatRoom.PropertyName.Ui, ref variant1))
      this.Ui = ((Variant) ref variant1).As<NCombatUi>();
    Variant variant2;
    if (info.TryGetProperty(NCombatRoom.PropertyName.SceneContainer, ref variant2))
      this.SceneContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCombatRoom.PropertyName.BgContainer, ref variant3))
      this.BgContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NCombatRoom.PropertyName.Background, ref variant4))
      this.Background = ((Variant) ref variant4).As<NCombatBackground>();
    Variant variant5;
    if (info.TryGetProperty(NCombatRoom.PropertyName.BackCombatVfxContainer, ref variant5))
      this.BackCombatVfxContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCombatRoom.PropertyName.CombatVfxContainer, ref variant6))
      this.CombatVfxContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NCombatRoom.PropertyName.CreatedMsec, ref variant7))
      this.CreatedMsec = ((Variant) ref variant7).As<ulong>();
    Variant variant8;
    if (info.TryGetProperty(NCombatRoom.PropertyName.Mode, ref variant8))
      this.Mode = ((Variant) ref variant8).As<CombatRoomMode>();
    Variant variant9;
    if (info.TryGetProperty(NCombatRoom.PropertyName.EncounterSlots, ref variant9))
      this.EncounterSlots = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NCombatRoom.PropertyName._allyContainer, ref variant10))
      this._allyContainer = ((Variant) ref variant10).As<Control>();
    Variant variant11;
    if (info.TryGetProperty(NCombatRoom.PropertyName._enemyContainer, ref variant11))
      this._enemyContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NCombatRoom.PropertyName._radialBlur, ref variant12))
      this._radialBlur = ((Variant) ref variant12).As<NRadialBlurVfx>();
    Variant variant13;
    if (info.TryGetProperty(NCombatRoom.PropertyName._proceedButton, ref variant13))
      this._proceedButton = ((Variant) ref variant13).As<NProceedButton>();
    Variant variant14;
    if (info.TryGetProperty(NCombatRoom.PropertyName._waitingForOtherPlayersOverlay, ref variant14))
      this._waitingForOtherPlayersOverlay = ((Variant) ref variant14).As<Control>();
    Variant variant15;
    if (!info.TryGetProperty(NCombatRoom.PropertyName._window, ref variant15))
      return;
    this._window = ((Variant) ref variant15).As<Window>();
  }

  private struct PlayerAndPets
  {
    public 
    #nullable enable
    NCreature player;
    public List<NCreature> pets;
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SubscribeToCombatEvents = StringName.op_Implicit(nameof (SubscribeToCombatEvents));
    public static readonly StringName AdjustCreatureScaleForAspectRatio = StringName.op_Implicit(nameof (AdjustCreatureScaleForAspectRatio));
    public static readonly StringName CreateAllyNodes = StringName.op_Implicit(nameof (CreateAllyNodes));
    public static readonly StringName CreateEnemyNodes = StringName.op_Implicit(nameof (CreateEnemyNodes));
    public static readonly StringName RemoveCreatureNode = StringName.op_Implicit(nameof (RemoveCreatureNode));
    public static readonly StringName UpdateCreatureNavigation = StringName.op_Implicit(nameof (UpdateCreatureNavigation));
    public static readonly StringName OnActiveScreenUpdated = StringName.op_Implicit(nameof (OnActiveScreenUpdated));
    public static readonly StringName EnableControllerNavigation = StringName.op_Implicit(nameof (EnableControllerNavigation));
    public static readonly StringName RandomizeEnemyScalesAndHues = StringName.op_Implicit(nameof (RandomizeEnemyScalesAndHues));
    public static readonly StringName RadialBlur = StringName.op_Implicit(nameof (RadialBlur));
    public static readonly StringName SetWaitingForOtherPlayersOverlayVisible = StringName.op_Implicit(nameof (SetWaitingForOtherPlayersOverlayVisible));
    public static readonly StringName OnProceedButtonPressed = StringName.op_Implicit(nameof (OnProceedButtonPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Ui = StringName.op_Implicit(nameof (Ui));
    public static readonly StringName SceneContainer = StringName.op_Implicit(nameof (SceneContainer));
    public static readonly StringName BgContainer = StringName.op_Implicit(nameof (BgContainer));
    public static readonly StringName Background = StringName.op_Implicit(nameof (Background));
    public static readonly StringName ProceedButton = StringName.op_Implicit(nameof (ProceedButton));
    public static readonly StringName BackCombatVfxContainer = StringName.op_Implicit(nameof (BackCombatVfxContainer));
    public static readonly StringName CombatVfxContainer = StringName.op_Implicit(nameof (CombatVfxContainer));
    public static readonly StringName CreatedMsec = StringName.op_Implicit(nameof (CreatedMsec));
    public static readonly StringName Mode = StringName.op_Implicit(nameof (Mode));
    public static readonly StringName EncounterSlots = StringName.op_Implicit(nameof (EncounterSlots));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _allyContainer = StringName.op_Implicit(nameof (_allyContainer));
    public static readonly StringName _enemyContainer = StringName.op_Implicit(nameof (_enemyContainer));
    public static readonly StringName _radialBlur = StringName.op_Implicit(nameof (_radialBlur));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _waitingForOtherPlayersOverlay = StringName.op_Implicit(nameof (_waitingForOtherPlayersOverlay));
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
  }

  public class SignalName : Control.SignalName
  {
  }
}
