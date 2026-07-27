// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Potions.NPotionContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Potions;

[ScriptPath("res://src/Core/Nodes/Potions/NPotionContainer.cs")]
public class NPotionContainer : Control
{
  private Player? _player;
  private readonly List<NPotionHolder> _holders = new List<NPotionHolder>();
  private Control _potionHolders;
  private Control _potionErrorBg;
  private NButton _potionShortcutButton;
  private Tween? _potionsFullTween;
  private Vector2 _potionHolderInitPos;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private NPotionHolder? _focusedHolder;

  public override void _Ready()
  {
    Callable callable = Callable.From(new Action(this.UpdateNavigation));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  public override void _EnterTree()
  {
    this._cts = new CancellationTokenSource();
    this._potionHolders = ((Node) this).GetNode<Control>(NodePath.op_Implicit("MarginContainer/PotionHolders"));
    this._potionErrorBg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("PotionErrorBg"));
    this._potionShortcutButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("PotionShortcutButton"));
    ((CanvasItem) this._potionErrorBg).Modulate = Colors.Transparent;
    ((GodotObject) this._potionShortcutButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnPotionShortcutPressed)), 0U);
    CombatManager.Instance.CombatSetUp += new Action<CombatState>(this.OnCombatSetUp);
    this.ConnectPlayerEvents();
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    this.DisconnectPlayerEvents();
    this._player = (Player) null;
    ((GodotObject) this._potionShortcutButton).Disconnect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnPotionShortcutPressed)));
    CombatManager.Instance.CombatSetUp -= new Action<CombatState>(this.OnCombatSetUp);
  }

  public void Initialize(IRunState runState)
  {
    this.DisconnectPlayerEvents();
    this._player = LocalContext.GetMe((IPlayerCollection) runState);
    this.ConnectPlayerEvents();
    this.GrowPotionHolders(this._player.MaxPotionCount);
    foreach (PotionModel potion in this._player.Potions)
      this.Add(potion, true);
  }

  private void ConnectPlayerEvents()
  {
    if (this._player == null)
      return;
    this._player.AddPotionFailed += new Action(this.PlayAddFailedAnim);
    this._player.PotionProcured += new Action<PotionModel>(this.OnPotionProcured);
    this._player.UsedPotionRemoved += new Action<PotionModel>(this.OnUsedPotionRemoved);
    this._player.PotionDiscarded += new Action<PotionModel>(this.Discard);
    this._player.MaxPotionCountChanged += new Action<int>(this.GrowPotionHolders);
    this._player.RelicObtained += new Action<RelicModel>(this.OnRelicsUpdated);
    this._player.RelicRemoved += new Action<RelicModel>(this.OnRelicsUpdated);
  }

  private void DisconnectPlayerEvents()
  {
    if (this._player == null)
      return;
    this._player.AddPotionFailed -= new Action(this.PlayAddFailedAnim);
    this._player.PotionProcured -= new Action<PotionModel>(this.OnPotionProcured);
    this._player.UsedPotionRemoved -= new Action<PotionModel>(this.OnUsedPotionRemoved);
    this._player.PotionDiscarded -= new Action<PotionModel>(this.Discard);
    this._player.MaxPotionCountChanged -= new Action<int>(this.GrowPotionHolders);
    this._player.RelicObtained -= new Action<RelicModel>(this.OnRelicsUpdated);
    this._player.RelicRemoved -= new Action<RelicModel>(this.OnRelicsUpdated);
  }

  private void GrowPotionHolders(int newMaxPotionSlots)
  {
    for (int count = this._holders.Count; count < newMaxPotionSlots; ++count)
    {
      NPotionHolder node = NPotionHolder.Create(true);
      this._holders.Add(node);
      ((Node) this._potionHolders).AddChildSafely((Node) node);
      ((GodotObject) node).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this.OnPotionHolderFocused(node))), 0U);
      ((GodotObject) node).Connect(Control.SignalName.FocusExited, Callable.From((Action) (() => this.OnPotionHolderUnfocused(node))), 0U);
      ((GodotObject) node).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.OnPotionHolderFocused(node))), 0U);
      ((GodotObject) node).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.OnPotionHolderUnfocused(node))), 0U);
    }
    this.UpdateNavigation();
  }

  private void OnRelicsUpdated(RelicModel _)
  {
    Callable callable = Callable.From(new Action(this.UpdateNavigation));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void UpdateNavigation()
  {
    Control control = (Control) NRun.Instance.GlobalUi.RelicInventory.RelicNodes.FirstOrDefault<NRelicInventoryHolder>();
    if (control == null)
      return;
    for (int index = 0; index < this._holders.Count; ++index)
    {
      this._holders[index].FocusNeighborLeft = index > 0 ? ((Node) this._holders[index - 1]).GetPath() : ((Node) NRun.Instance.GlobalUi.TopBar.Gold).GetPath();
      this._holders[index].FocusNeighborRight = index < this._holders.Count - 1 ? ((Node) this._holders[index + 1]).GetPath() : ((Node) NRun.Instance.GlobalUi.TopBar.RoomIcon).GetPath();
      this._holders[index].FocusNeighborBottom = ((Node) control).GetPath();
      this._holders[index].FocusNeighborTop = ((Node) this._holders[index]).GetPath();
    }
  }

  private void Add(PotionModel potion, bool isInitialization)
  {
    if (this._holders.All<NPotionHolder>((Func<NPotionHolder, bool>) (h => h.HasPotion)))
      return;
    if (!isInitialization)
      this.PotionFtueCheck();
    NPotion potion1 = NPotion.Create(potion);
    potion1.Position = new Vector2(-30f, -30f);
    this._holders[potion.Owner.PotionSlots.IndexOf<PotionModel>(potion)].AddPotion(potion1);
  }

  public void AnimatePotion(PotionModel potion, Vector2? startPosition = null)
  {
    if (!LocalContext.IsMine(potion))
      return;
    TaskHelper.RunSafely(this._holders.First<NPotionHolder>((Func<NPotionHolder, bool>) (n => n.Potion != null && n.Potion.Model == potion)).Potion.PlayNewlyAcquiredAnimation(startPosition));
  }

  public void OnPotionUseOrDiscardCanceled(PotionModel potion)
  {
    NPotionHolder npotionHolder = this._holders.FirstOrDefault<NPotionHolder>((Func<NPotionHolder, bool>) (n => n.Potion?.Model == potion));
    if (npotionHolder != null)
      npotionHolder.CancelPotionUseOrDiscard();
    else
      Log.Error($"Tried to cancel potion use for potion {potion} but a holder for it does not exist in the player's belt!");
  }

  private void PotionFtueCheck()
  {
    if (SaveManager.Instance.SeenFtue("obtain_potion_ftue"))
      return;
    NModalContainer.Instance.Add((Node) NObtainPotionFtue.Create());
    SaveManager.Instance.MarkFtueAsComplete("obtain_potion_ftue");
  }

  private void PlayAddFailedAnim()
  {
    if (this._potionsFullTween != null && this._potionsFullTween.IsRunning())
    {
      this._potionsFullTween?.Kill();
      this._potionHolders.Position = this._potionHolderInitPos;
    }
    this._potionsFullTween = ((Node) this).CreateTween().SetParallel(true);
    this._potionHolderInitPos = this._potionHolders.Position;
    this._potionsFullTween.TweenMethod(Callable.From<float>((Action<float>) (t => this._potionHolders.Position = Vector2.op_Addition(this._potionHolderInitPos, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 3f), Mathf.Sin(t * 5f)), Mathf.Sin(t * 0.5f))))), Variant.op_Implicit(0.0f), Variant.op_Implicit(6.28318548f), 0.5);
    this._potionsFullTween.TweenProperty((GodotObject) this._potionErrorBg, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.15);
    this._potionsFullTween.TweenProperty((GodotObject) this._potionErrorBg, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Transparent), 0.5).SetDelay(0.35);
  }

  private void Discard(PotionModel potion)
  {
    NPotionHolder holder = this._holders.First<NPotionHolder>((Func<NPotionHolder, bool>) (n => n.Potion != null && n.Potion.Model == potion));
    this.OnPotionHolderUnfocused(holder);
    holder.DiscardPotion();
  }

  private void RemoveUsed(PotionModel potion)
  {
    NPotionHolder holder = this._holders.First<NPotionHolder>((Func<NPotionHolder, bool>) (n => n.Potion != null && n.Potion.Model == potion));
    this.OnPotionHolderUnfocused(holder);
    holder.RemoveUsedPotion();
  }

  private void OnPotionProcured(PotionModel potion) => this.Add(potion, false);

  private void OnUsedPotionRemoved(PotionModel potion) => this.RemoveUsed(potion);

  private void OnPotionHolderFocused(NPotionHolder holder)
  {
    if (this._focusedHolder == holder || holder.Potion == null)
      return;
    RunManager.Instance.HoveredModelTracker.OnLocalPotionHovered(holder.Potion.Model);
    this._focusedHolder = holder;
  }

  private void OnPotionHolderUnfocused(NPotionHolder holder)
  {
    if (this._focusedHolder != holder)
      return;
    RunManager.Instance.HoveredModelTracker.OnLocalPotionUnhovered();
    this._focusedHolder = (NPotionHolder) null;
  }

  private void OnCombatSetUp(CombatState _) => TaskHelper.RunSafely(this.ShinePotions());

  private async Task ShinePotions()
  {
    await Cmd.Wait(1f, this._cts.Token);
    foreach (NPotionHolder holder in this._holders)
      await TaskHelper.RunSafely(holder.ShineOnStartOfCombat());
  }

  private void OnPotionShortcutPressed(NButton _)
  {
    Viewport viewport = ((Node) this).GetViewport();
    if (viewport == null)
      ((Node) this._potionHolders).GetChild<Control>(0, false).TryGrabFocus();
    else if (viewport.GuiGetFocusOwner() != null && ((Node) NRun.Instance.GlobalUi.TopBar).IsAncestorOf((Node) viewport.GuiGetFocusOwner()))
      ActiveScreenContext.Instance.FocusOnDefaultControl();
    else
      ((Node) this._potionHolders).GetChild<Control>(0, false).TryGrabFocus();
  }

  public Control? FirstPotionControl => (Control) this._holders.FirstOrDefault<NPotionHolder>();

  public Control? LastPotionControl => (Control) this._holders.LastOrDefault<NPotionHolder>();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NPotionContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.ConnectPlayerEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.DisconnectPlayerEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.GrowPotionHolders, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("newMaxPotionSlots"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.PotionFtueCheck, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.PlayAddFailedAnim, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.OnPotionHolderFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.OnPotionHolderUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPotionContainer.MethodName.OnPotionShortcutPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.ConnectPlayerEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectPlayerEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.DisconnectPlayerEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectPlayerEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.GrowPotionHolders) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.GrowPotionHolders(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.UpdateNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.PotionFtueCheck) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PotionFtueCheck();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.PlayAddFailedAnim) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayAddFailedAnim();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.OnPotionHolderFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPotionHolderFocused(VariantUtils.ConvertTo<NPotionHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotionContainer.MethodName.OnPotionHolderUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPotionHolderUnfocused(VariantUtils.ConvertTo<NPotionHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotionContainer.MethodName.OnPotionShortcutPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnPotionShortcutPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotionContainer.MethodName._Ready) || StringName.op_Equality(ref method, NPotionContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NPotionContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NPotionContainer.MethodName.ConnectPlayerEvents) || StringName.op_Equality(ref method, NPotionContainer.MethodName.DisconnectPlayerEvents) || StringName.op_Equality(ref method, NPotionContainer.MethodName.GrowPotionHolders) || StringName.op_Equality(ref method, NPotionContainer.MethodName.UpdateNavigation) || StringName.op_Equality(ref method, NPotionContainer.MethodName.PotionFtueCheck) || StringName.op_Equality(ref method, NPotionContainer.MethodName.PlayAddFailedAnim) || StringName.op_Equality(ref method, NPotionContainer.MethodName.OnPotionHolderFocused) || StringName.op_Equality(ref method, NPotionContainer.MethodName.OnPotionHolderUnfocused) || StringName.op_Equality(ref method, NPotionContainer.MethodName.OnPotionShortcutPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionHolders))
    {
      this._potionHolders = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionErrorBg))
    {
      this._potionErrorBg = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionShortcutButton))
    {
      this._potionShortcutButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionsFullTween))
    {
      this._potionsFullTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionHolderInitPos))
    {
      this._potionHolderInitPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionContainer.PropertyName._focusedHolder))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._focusedHolder = VariantUtils.ConvertTo<NPotionHolder>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName.FirstPotionControl))
    {
      ref godot_variant local = ref value;
      Control firstPotionControl = this.FirstPotionControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref firstPotionControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName.LastPotionControl))
    {
      ref godot_variant local = ref value;
      Control lastPotionControl = this.LastPotionControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref lastPotionControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionHolders))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionHolders);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionErrorBg))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionErrorBg);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionShortcutButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._potionShortcutButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionsFullTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._potionsFullTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotionContainer.PropertyName._potionHolderInitPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._potionHolderInitPos);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotionContainer.PropertyName._focusedHolder))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NPotionHolder>(ref this._focusedHolder);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName._potionHolders, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName._potionErrorBg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName._potionShortcutButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName._potionsFullTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NPotionContainer.PropertyName._potionHolderInitPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName._focusedHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName.FirstPotionControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotionContainer.PropertyName.LastPotionControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPotionContainer.PropertyName._potionHolders, Variant.From<Control>(ref this._potionHolders));
    info.AddProperty(NPotionContainer.PropertyName._potionErrorBg, Variant.From<Control>(ref this._potionErrorBg));
    info.AddProperty(NPotionContainer.PropertyName._potionShortcutButton, Variant.From<NButton>(ref this._potionShortcutButton));
    info.AddProperty(NPotionContainer.PropertyName._potionsFullTween, Variant.From<Tween>(ref this._potionsFullTween));
    info.AddProperty(NPotionContainer.PropertyName._potionHolderInitPos, Variant.From<Vector2>(ref this._potionHolderInitPos));
    info.AddProperty(NPotionContainer.PropertyName._focusedHolder, Variant.From<NPotionHolder>(ref this._focusedHolder));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPotionContainer.PropertyName._potionHolders, ref variant1))
      this._potionHolders = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NPotionContainer.PropertyName._potionErrorBg, ref variant2))
      this._potionErrorBg = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NPotionContainer.PropertyName._potionShortcutButton, ref variant3))
      this._potionShortcutButton = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NPotionContainer.PropertyName._potionsFullTween, ref variant4))
      this._potionsFullTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NPotionContainer.PropertyName._potionHolderInitPos, ref variant5))
      this._potionHolderInitPos = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (!info.TryGetProperty(NPotionContainer.PropertyName._focusedHolder, ref variant6))
      return;
    this._focusedHolder = ((Variant) ref variant6).As<NPotionHolder>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ConnectPlayerEvents = StringName.op_Implicit(nameof (ConnectPlayerEvents));
    public static readonly StringName DisconnectPlayerEvents = StringName.op_Implicit(nameof (DisconnectPlayerEvents));
    public static readonly StringName GrowPotionHolders = StringName.op_Implicit(nameof (GrowPotionHolders));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
    public static readonly StringName PotionFtueCheck = StringName.op_Implicit(nameof (PotionFtueCheck));
    public static readonly StringName PlayAddFailedAnim = StringName.op_Implicit(nameof (PlayAddFailedAnim));
    public static readonly StringName OnPotionHolderFocused = StringName.op_Implicit(nameof (OnPotionHolderFocused));
    public static readonly StringName OnPotionHolderUnfocused = StringName.op_Implicit(nameof (OnPotionHolderUnfocused));
    public static readonly StringName OnPotionShortcutPressed = StringName.op_Implicit(nameof (OnPotionShortcutPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName FirstPotionControl = StringName.op_Implicit(nameof (FirstPotionControl));
    public static readonly StringName LastPotionControl = StringName.op_Implicit(nameof (LastPotionControl));
    public static readonly StringName _potionHolders = StringName.op_Implicit(nameof (_potionHolders));
    public static readonly StringName _potionErrorBg = StringName.op_Implicit(nameof (_potionErrorBg));
    public static readonly StringName _potionShortcutButton = StringName.op_Implicit(nameof (_potionShortcutButton));
    public static readonly StringName _potionsFullTween = StringName.op_Implicit(nameof (_potionsFullTween));
    public static readonly StringName _potionHolderInitPos = StringName.op_Implicit(nameof (_potionHolderInitPos));
    public static readonly StringName _focusedHolder = StringName.op_Implicit(nameof (_focusedHolder));
  }

  public class SignalName : Control.SignalName
  {
  }
}
