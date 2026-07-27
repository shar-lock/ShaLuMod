// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NMerchantRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NMerchantRoom.cs")]
public class NMerchantRoom : Control, IScreenContext, IRoomWithProceedButton
{
  private const float _animVariance = 0.5f;
  private static readonly string _scenePath = SceneHelper.GetScenePath("rooms/merchant_room");
  private readonly List<Player> _players = new List<Player>();
  private MerchantDialogueSet _dialogue;
  private NProceedButton _proceedButton;
  private Control _characterContainer;
  private readonly List<NMerchantCharacter> _playerVisuals = new List<NMerchantCharacter>();

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NMerchantRoom._scenePath);
    }
  }

  public static NMerchantRoom? Instance => NRun.Instance?.MerchantRoom;

  public NProceedButton ProceedButton => this._proceedButton;

  public MerchantRoom Room { get; private set; }

  public NMerchantInventory Inventory { get; private set; }

  public NMerchantButton MerchantButton { get; private set; }

  public IReadOnlyList<NMerchantCharacter> PlayerVisuals
  {
    get => (IReadOnlyList<NMerchantCharacter>) this._playerVisuals;
  }

  public static NMerchantRoom? Create(MerchantRoom room, IReadOnlyList<Player> players)
  {
    if (TestMode.IsOn)
      return (NMerchantRoom) null;
    NMerchantRoom nmerchantRoom = PreloadManager.Cache.GetScene(NMerchantRoom._scenePath).Instantiate<NMerchantRoom>((PackedScene.GenEditState) 0L);
    nmerchantRoom.Room = room;
    nmerchantRoom._players.AddRange((IEnumerable<Player>) players);
    nmerchantRoom._dialogue = MerchantRoom.Dialogue;
    return nmerchantRoom;
  }

  public override void _Ready()
  {
    this._proceedButton = ((Node) this).GetNode<NProceedButton>(NodePath.op_Implicit("%ProceedButton"));
    ((GodotObject) this._proceedButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.HideScreen)), 0U);
    this._proceedButton.UpdateText(NProceedButton.ProceedLoc);
    this._proceedButton.SetPulseState(false);
    this._proceedButton.Enable();
    this.MerchantButton = ((Node) this).GetNode<NMerchantButton>(NodePath.op_Implicit("%MerchantButton"));
    this.MerchantButton.IsLocalPlayerDead = LocalContext.GetMe((IEnumerable<Player>) this._players).Creature.IsDead;
    this.MerchantButton.PlayerDeadLines = this._dialogue.PlayerDeadLines;
    ((GodotObject) this.MerchantButton).Connect(NMerchantButton.SignalName.MerchantOpened, Callable.From<NMerchantButton>(new Action<NMerchantButton>(this.OnMerchantOpened)), 0U);
    this.Inventory = ((Node) this).GetNode<NMerchantInventory>(NodePath.op_Implicit("%Inventory"));
    this.Inventory.MouseFilter = (Control.MouseFilterEnum) 2L;
    this.Inventory.Initialize(this.Room.GetLocalInventory(), this._dialogue);
    this._characterContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterContainer"));
    NMapScreen.Instance.SetTravelEnabled(true);
    NGame.Instance.SetScreenShakeTarget((Control) this);
    this.AfterRoomIsLoaded();
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenUpdated);
    ((GodotObject) NMapScreen.Instance).Connect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
    ((GodotObject) NMapScreen.Instance).Connect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)), 0U);
  }

  public override void _ExitTree()
  {
    NGame.Instance.ClearScreenShakeTarget();
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenUpdated);
    ((GodotObject) NMapScreen.Instance).Disconnect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.ToggleMerchantTrack)));
    ((GodotObject) NMapScreen.Instance).Disconnect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ToggleMerchantTrack)));
  }

  public void FoulPotionThrown(FoulPotion potion)
  {
    SfxCmd.Play("event:/sfx/npcs/merchant/merchant_thank_yous");
    LocString line = Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) this._dialogue.FoulPotionLines);
    if (line == null || this.MerchantButton.PlayDialogue(line) == null)
      return;
    NGame.Instance?.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Short, RumbleStyle.Rumble);
  }

  private void ToggleMerchantTrack() => NRunMusicController.Instance?.ToggleMerchantTrack();

  private void AfterRoomIsLoaded()
  {
    Player me = LocalContext.GetMe((IEnumerable<Player>) this._players);
    this._players.Remove(me);
    this._players.Insert(0, me);
    int num1 = Mathf.CeilToInt(Mathf.Sqrt((float) this._players.Count));
    for (int index1 = 0; index1 < num1; ++index1)
    {
      float num2 = -140f * (float) index1;
      for (int index2 = 0; index2 < num1; ++index2)
      {
        int index3 = index1 * num1 + index2;
        if (index3 < this._players.Count)
        {
          NMerchantCharacter child = PreloadManager.Cache.GetScene(this._players[index3].Character.MerchantAnimPath).Instantiate<NMerchantCharacter>((PackedScene.GenEditState) 0L);
          ((Node) this._characterContainer).AddChildSafely((Node) child);
          ((Node) this._characterContainer).MoveChildSafely((Node) child, 0);
          child.Position = new Vector2(num2, -50f * (float) index1);
          if (index1 > 0)
            ((CanvasItem) child).Modulate = new Color(0.5f, 0.5f, 0.5f, 1f);
          num2 -= 275f;
          this._playerVisuals.Add(child);
        }
        else
          break;
      }
    }
  }

  private void HideScreen(NButton _)
  {
    if (this.MerchantFtueCheck())
      return;
    NMapScreen.Instance.Open();
  }

  private bool MerchantFtueCheck()
  {
    if (SaveManager.Instance.SeenFtue("merchant_ftue"))
      return false;
    NModalContainer.Instance.Add((Node) NMerchantFtue.Create(this));
    SaveManager.Instance.MarkFtueAsComplete("merchant_ftue");
    return true;
  }

  private void OnMerchantOpened(NMerchantButton _) => this.OpenInventory();

  public void OpenInventory()
  {
    if (this.Inventory.IsOpen)
      return;
    this._proceedButton.Disable();
    this.Inventory.Open();
    this.MerchantButton.Disable();
    ((GodotObject) this.Inventory).Connect(NMerchantInventory.SignalName.InventoryClosed, Callable.From((Action) (() =>
    {
      this.MerchantButton.Enable();
      this._proceedButton.Enable();
      this._proceedButton.SetPulseState(true);
    })), 4U);
  }

  private void OnActiveScreenUpdated()
  {
    this.UpdateControllerNavEnabled<NMerchantRoom>();
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
    {
      this.MerchantButton.Enable();
      this._proceedButton.Enable();
    }
    else
    {
      this.MerchantButton.Disable();
      this._proceedButton.Disable();
    }
  }

  public Control DefaultFocusedControl => (Control) this;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NMerchantRoom.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.ToggleMerchantTrack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.AfterRoomIsLoaded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.HideScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.MerchantFtueCheck, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.OnMerchantOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.OpenInventory, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRoom.MethodName.OnActiveScreenUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName.ToggleMerchantTrack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ToggleMerchantTrack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName.AfterRoomIsLoaded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterRoomIsLoaded();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName.HideScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HideScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName.MerchantFtueCheck) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.MerchantFtueCheck();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName.OnMerchantOpened) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMerchantOpened(VariantUtils.ConvertTo<NMerchantButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRoom.MethodName.OpenInventory) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenInventory();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantRoom.MethodName.OnActiveScreenUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantRoom.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantRoom.MethodName._EnterTree) || StringName.op_Equality(ref method, NMerchantRoom.MethodName._ExitTree) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.ToggleMerchantTrack) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.AfterRoomIsLoaded) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.HideScreen) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.MerchantFtueCheck) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.OnMerchantOpened) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.OpenInventory) || StringName.op_Equality(ref method, NMerchantRoom.MethodName.OnActiveScreenUpdated) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName.Inventory))
    {
      this.Inventory = VariantUtils.ConvertTo<NMerchantInventory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName.MerchantButton))
    {
      this.MerchantButton = VariantUtils.ConvertTo<NMerchantButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName._proceedButton))
    {
      this._proceedButton = VariantUtils.ConvertTo<NProceedButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantRoom.PropertyName._characterContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._characterContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName.ProceedButton))
    {
      ref godot_variant local = ref value;
      NProceedButton proceedButton = this.ProceedButton;
      godot_variant from = VariantUtils.CreateFrom<NProceedButton>(ref proceedButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName.Inventory))
    {
      ref godot_variant local = ref value;
      NMerchantInventory inventory = this.Inventory;
      godot_variant from = VariantUtils.CreateFrom<NMerchantInventory>(ref inventory);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName.MerchantButton))
    {
      ref godot_variant local = ref value;
      NMerchantButton merchantButton = this.MerchantButton;
      godot_variant from = VariantUtils.CreateFrom<NMerchantButton>(ref merchantButton);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRoom.PropertyName._proceedButton))
    {
      value = VariantUtils.CreateFrom<NProceedButton>(ref this._proceedButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantRoom.PropertyName._characterContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._characterContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMerchantRoom.PropertyName._proceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRoom.PropertyName.ProceedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRoom.PropertyName._characterContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRoom.PropertyName.Inventory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRoom.PropertyName.MerchantButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRoom.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName inventory1 = NMerchantRoom.PropertyName.Inventory;
    NMerchantInventory inventory2 = this.Inventory;
    Variant variant1 = Variant.From<NMerchantInventory>(ref inventory2);
    serializationInfo1.AddProperty(inventory1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName merchantButton1 = NMerchantRoom.PropertyName.MerchantButton;
    NMerchantButton merchantButton2 = this.MerchantButton;
    Variant variant2 = Variant.From<NMerchantButton>(ref merchantButton2);
    serializationInfo2.AddProperty(merchantButton1, variant2);
    info.AddProperty(NMerchantRoom.PropertyName._proceedButton, Variant.From<NProceedButton>(ref this._proceedButton));
    info.AddProperty(NMerchantRoom.PropertyName._characterContainer, Variant.From<Control>(ref this._characterContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantRoom.PropertyName.Inventory, ref variant1))
      this.Inventory = ((Variant) ref variant1).As<NMerchantInventory>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantRoom.PropertyName.MerchantButton, ref variant2))
      this.MerchantButton = ((Variant) ref variant2).As<NMerchantButton>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantRoom.PropertyName._proceedButton, ref variant3))
      this._proceedButton = ((Variant) ref variant3).As<NProceedButton>();
    Variant variant4;
    if (!info.TryGetProperty(NMerchantRoom.PropertyName._characterContainer, ref variant4))
      return;
    this._characterContainer = ((Variant) ref variant4).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ToggleMerchantTrack = StringName.op_Implicit(nameof (ToggleMerchantTrack));
    public static readonly StringName AfterRoomIsLoaded = StringName.op_Implicit(nameof (AfterRoomIsLoaded));
    public static readonly StringName HideScreen = StringName.op_Implicit(nameof (HideScreen));
    public static readonly StringName MerchantFtueCheck = StringName.op_Implicit(nameof (MerchantFtueCheck));
    public static readonly StringName OnMerchantOpened = StringName.op_Implicit(nameof (OnMerchantOpened));
    public static readonly StringName OpenInventory = StringName.op_Implicit(nameof (OpenInventory));
    public static readonly StringName OnActiveScreenUpdated = StringName.op_Implicit(nameof (OnActiveScreenUpdated));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ProceedButton = StringName.op_Implicit(nameof (ProceedButton));
    public static readonly StringName Inventory = StringName.op_Implicit(nameof (Inventory));
    public static readonly StringName MerchantButton = StringName.op_Implicit(nameof (MerchantButton));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _proceedButton = StringName.op_Implicit(nameof (_proceedButton));
    public static readonly StringName _characterContainer = StringName.op_Implicit(nameof (_characterContainer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
