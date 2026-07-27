// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.Custom.NFakeMerchant
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events.Custom;

[ScriptPath("res://src/Core/Nodes/Events/Custom/NFakeMerchant.cs")]
public class NFakeMerchant : Control, ICustomEventNode, IScreenContext
{
  private const float _animVariance = 0.5f;
  private readonly List<Player> _players = new List<Player>();
  private FakeMerchant _event;
  private MerchantDialogueSet _dialogue;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private NProceedButton _proceedButton;
  private Control _characterContainer;
  private Control _inputBlocker;

  public NMerchantInventory Inventory { get; private set; }

  public NMerchantButton MerchantButton { get; private set; }

  public IScreenContext CurrentScreenContext
  {
    get => !this.Inventory.IsOpen ? (IScreenContext) this : (IScreenContext) this.Inventory;
  }

  public void Initialize(EventModel eventModel)
  {
    this._event = (FakeMerchant) eventModel;
    this._dialogue = FakeMerchant.Dialogue;
    this._players.AddRange((IEnumerable<Player>) this._event.Owner.RunState.Players);
  }

  public override void _Ready()
  {
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.HideScreen)), 0U);
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    this._proceedButton.SetPulseState(false);
    this._proceedButton.Enable();
    this.MerchantButton = ((Node) this).GetNode<NMerchantButton>(NodePath.op_Implicit("%MerchantButton"));
    if (this._event.StartedFight)
    {
      ((CanvasItem) this.MerchantButton).Hide();
      if (LocalContext.GetMe((IEnumerable<Player>) this._players).GetRelic<FakeMerchantsRug>() != null)
        new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%FakeMerchantBackground")))).GetSkeleton()?.FindBone("rug")?.Hide();
    }
    else
    {
      this.MerchantButton.IsLocalPlayerDead = LocalContext.GetMe((IEnumerable<Player>) this._players).Creature.IsDead;
      this.MerchantButton.PlayerDeadLines = this._dialogue.PlayerDeadLines;
      ((GodotObject) this.MerchantButton).Connect(NMerchantButton.SignalName.MerchantOpened, Callable.From<NMerchantButton>(new Action<NMerchantButton>(this.OnMerchantOpened)), 0U);
    }
    this.Inventory = ((Node) this).GetNode<NMerchantInventory>(NodePath.op_Implicit("%Inventory"));
    this.Inventory.MouseFilter = (Control.MouseFilterEnum) 2L;
    this.Inventory.Initialize(this._event.Inventory, this._dialogue);
    this._characterContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterContainer"));
    this._inputBlocker = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%InputBlocker"));
    NMapScreen.Instance.SetTravelEnabled(true);
    NGame.Instance.SetScreenShakeTarget((Control) this);
    this.AfterRoomIsLoaded();
  }

  public override void _EnterTree()
  {
    this._cts = new CancellationTokenSource();
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenUpdated);
    ((GodotObject) NMapScreen.Instance).Connect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
    ((GodotObject) NMapScreen.Instance).Connect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
  }

  public override void _ExitTree()
  {
    this._cts.Cancel();
    NGame.Instance.ClearScreenShakeTarget();
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenUpdated);
    ((GodotObject) NMapScreen.Instance).Disconnect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)));
    ((GodotObject) NMapScreen.Instance).Disconnect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)));
  }

  public async Task FoulPotionThrown()
  {
    this.MerchantButton.Disable();
    NSpeechBubbleVfx nspeechBubbleVfx = this.MerchantButton.PlayDialogue(Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) this._dialogue.FoulPotionLines));
    if (nspeechBubbleVfx == null)
      return;
    await Cmd.Wait((float) nspeechBubbleVfx.SecondsToDisplay - 1f, this._cts.Token);
  }

  private void ToggleMerchantTrack()
  {
  }

  private void AfterRoomIsLoaded()
  {
    Player me = LocalContext.GetMe((IEnumerable<Player>) this._players);
    this._players.Remove(me);
    this._players.Insert(0, me);
    int num1 = Mathf.CeilToInt(Mathf.Sqrt((float) this._players.Count));
    for (int index1 = 0; index1 < num1; ++index1)
    {
      float num2 = -75f * (float) index1;
      for (int index2 = 0; index2 < num1; ++index2)
      {
        int index3 = index1 * num1 + index2;
        if (index3 < this._players.Count)
        {
          NCreatureVisuals visuals = this._players[index3].Character.CreateVisuals();
          ((Node) this._characterContainer).AddChildSafely((Node) visuals);
          this.StartCharacterAnimation(visuals);
          ((Node) this._characterContainer).MoveChildSafely((Node) visuals, 0);
          visuals.Position = new Vector2(num2, -50f * (float) index1);
          if (index1 > 0)
            ((CanvasItem) visuals).Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
          num2 -= (float) ((double) visuals.Bounds.Size.X * 0.5 + 25.0);
        }
        else
          break;
      }
    }
    if (this._event.StartedFight)
      return;
    TaskHelper.RunSafely(this.ShowWelcomeDialogue());
  }

  private async Task ShowWelcomeDialogue()
  {
    LocString line = Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) this._dialogue.WelcomeLines);
    if (line == null)
    {
      line = (LocString) null;
    }
    else
    {
      await Cmd.Wait(0.75f, this._cts.Token);
      SfxCmd.Play("event:/sfx/npcs/reverse_merchant/reverse_merchant_laugh");
      this.MerchantButton.PlayDialogue(line, 4.0);
      line = (LocString) null;
    }
  }

  private void StartCharacterAnimation(NCreatureVisuals visuals)
  {
    visuals.SpineAnimation.SetAnimation("relaxed_loop");
    using (MegaTrackEntry currentTrack = visuals.SpineAnimation.GetCurrentTrack())
    {
      if (currentTrack == null)
        return;
      currentTrack.SetLoop(true);
      currentTrack.SetTimeScale(Rng.Chaotic.NextFloat(0.9f, 1.1f));
      float animationEnd = currentTrack.GetAnimationEnd();
      currentTrack.SetTrackTime((animationEnd + Rng.Chaotic.NextFloat(-0.5f, 0.5f)) % animationEnd);
    }
  }

  private void HideScreen(NButton _) => NMapScreen.Instance.Open();

  private void OnMerchantOpened(NMerchantButton _) => this.OpenInventory();

  private void OpenInventory()
  {
    if (this.Inventory.IsOpen)
      return;
    this._proceedButton.Disable();
    this.Inventory.Open();
    this.MerchantButton.Disable();
    ((GodotObject) this.Inventory).Connect(NMerchantInventory.SignalName.InventoryClosed, Callable.From((Action) (() =>
    {
      this.MerchantButton.Enable();
      this.ShowProceedButton();
    })), 4U);
  }

  private void ShowProceedButton()
  {
    this._proceedButton.Enable();
    this._proceedButton.SetPulseState(true);
  }

  private void OnActiveScreenUpdated()
  {
    this.UpdateControllerNavEnabled<NFakeMerchant>();
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
    {
      this.MerchantButton.Enable();
      if (this._proceedButton.IsEnabled)
        return;
      this._proceedButton.Enable();
    }
    else
    {
      this.MerchantButton.Disable();
      this._proceedButton.Disable();
    }
  }

  public Control? DefaultFocusedControl => (Control) null;

  public void BlockInput()
  {
    this._inputBlocker.MouseFilter = (Control.MouseFilterEnum) 0L;
    NHotkeyManager.Instance.AddBlockingScreen((Node) this._inputBlocker);
  }

  public void UnblockInput()
  {
    this._inputBlocker.MouseFilter = (Control.MouseFilterEnum) 2L;
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this._inputBlocker);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NFakeMerchant.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.ToggleMerchantTrack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.AfterRoomIsLoaded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.StartCharacterAnimation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("visuals"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.HideScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.OnMerchantOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.OpenInventory, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.ShowProceedButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.OnActiveScreenUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.BlockInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFakeMerchant.MethodName.UnblockInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.ToggleMerchantTrack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ToggleMerchantTrack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.AfterRoomIsLoaded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterRoomIsLoaded();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.StartCharacterAnimation) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StartCharacterAnimation(VariantUtils.ConvertTo<NCreatureVisuals>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.HideScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HideScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.OnMerchantOpened) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMerchantOpened(VariantUtils.ConvertTo<NMerchantButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.OpenInventory) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenInventory();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.ShowProceedButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowProceedButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.OnActiveScreenUpdated) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnActiveScreenUpdated();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFakeMerchant.MethodName.BlockInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BlockInput();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFakeMerchant.MethodName.UnblockInput) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UnblockInput();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFakeMerchant.MethodName._Ready) || StringName.op_Equality(ref method, NFakeMerchant.MethodName._EnterTree) || StringName.op_Equality(ref method, NFakeMerchant.MethodName._ExitTree) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.ToggleMerchantTrack) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.AfterRoomIsLoaded) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.StartCharacterAnimation) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.HideScreen) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.OnMerchantOpened) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.OpenInventory) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.ShowProceedButton) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.OnActiveScreenUpdated) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.BlockInput) || StringName.op_Equality(ref method, NFakeMerchant.MethodName.UnblockInput) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName.Inventory))
    {
      this.Inventory = VariantUtils.ConvertTo<NMerchantInventory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName.MerchantButton))
    {
      this.MerchantButton = VariantUtils.ConvertTo<NMerchantButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName._characterContainer))
    {
      this._characterContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFakeMerchant.PropertyName._inputBlocker))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._inputBlocker = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName.Inventory))
    {
      ref godot_variant local = ref value;
      NMerchantInventory inventory = this.Inventory;
      godot_variant from = VariantUtils.CreateFrom<NMerchantInventory>(ref inventory);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName.MerchantButton))
    {
      ref godot_variant local = ref value;
      NMerchantButton merchantButton = this.MerchantButton;
      godot_variant from = VariantUtils.CreateFrom<NMerchantButton>(ref merchantButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NFakeMerchant.PropertyName._characterContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFakeMerchant.PropertyName._inputBlocker))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._inputBlocker);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFakeMerchant.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFakeMerchant.PropertyName._characterContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFakeMerchant.PropertyName._inputBlocker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFakeMerchant.PropertyName.Inventory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFakeMerchant.PropertyName.MerchantButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFakeMerchant.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName inventory1 = NFakeMerchant.PropertyName.Inventory;
    NMerchantInventory inventory2 = this.Inventory;
    Variant variant1 = Variant.From<NMerchantInventory>(ref inventory2);
    serializationInfo1.AddProperty(inventory1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName merchantButton1 = NFakeMerchant.PropertyName.MerchantButton;
    NMerchantButton merchantButton2 = this.MerchantButton;
    Variant variant2 = Variant.From<NMerchantButton>(ref merchantButton2);
    serializationInfo2.AddProperty(merchantButton1, variant2);
    info.AddProperty(NFakeMerchant.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NFakeMerchant.PropertyName._characterContainer, Variant.From<Control>(ref this._characterContainer));
    info.AddProperty(NFakeMerchant.PropertyName._inputBlocker, Variant.From<Control>(ref this._inputBlocker));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFakeMerchant.PropertyName.Inventory, ref variant1))
      this.Inventory = ((Variant) ref variant1).As<NMerchantInventory>();
    Variant variant2;
    if (info.TryGetProperty(NFakeMerchant.PropertyName.MerchantButton, ref variant2))
      this.MerchantButton = ((Variant) ref variant2).As<NMerchantButton>();
    Variant variant3;
    if (info.TryGetProperty(NFakeMerchant.PropertyName._proceedButton, ref variant3))
      this._proceedButton = ((Variant) ref variant3).As<NProceedButton>();
    Variant variant4;
    if (info.TryGetProperty(NFakeMerchant.PropertyName._characterContainer, ref variant4))
      this._characterContainer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (!info.TryGetProperty(NFakeMerchant.PropertyName._inputBlocker, ref variant5))
      return;
    this._inputBlocker = ((Variant) ref variant5).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ToggleMerchantTrack = StringName.op_Implicit(nameof (ToggleMerchantTrack));
    public static readonly StringName AfterRoomIsLoaded = StringName.op_Implicit(nameof (AfterRoomIsLoaded));
    public static readonly StringName StartCharacterAnimation = StringName.op_Implicit(nameof (StartCharacterAnimation));
    public static readonly StringName HideScreen = StringName.op_Implicit(nameof (HideScreen));
    public static readonly StringName OnMerchantOpened = StringName.op_Implicit(nameof (OnMerchantOpened));
    public static readonly StringName OpenInventory = StringName.op_Implicit(nameof (OpenInventory));
    public static readonly StringName ShowProceedButton = StringName.op_Implicit(nameof (ShowProceedButton));
    public static readonly StringName OnActiveScreenUpdated = StringName.op_Implicit(nameof (OnActiveScreenUpdated));
    public static readonly StringName BlockInput = StringName.op_Implicit(nameof (BlockInput));
    public static readonly StringName UnblockInput = StringName.op_Implicit(nameof (UnblockInput));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Inventory = StringName.op_Implicit(nameof (Inventory));
    public static readonly StringName MerchantButton = StringName.op_Implicit(nameof (MerchantButton));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _characterContainer = StringName.op_Implicit(nameof (_characterContainer));
    public static readonly StringName _inputBlocker = StringName.op_Implicit(nameof (_inputBlocker));
  }

  public class SignalName : Control.SignalName
  {
  }
}
