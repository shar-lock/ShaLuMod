// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NCardGridSelectionScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NCardGridSelectionScreen.cs")]
public abstract class NCardGridSelectionScreen : 
  Control,
  IOverlayScreen,
  IScreenContext,
  ICardSelector
{
  protected NCardGrid _grid;
  protected NPeekButton _peekButton;
  protected IReadOnlyList<CardModel> _cards;
  protected readonly TaskCompletionSource<IEnumerable<CardModel>> _completionSource = new TaskCompletionSource<IEnumerable<CardModel>>();

  public NetScreenType ScreenType => NetScreenType.CardSelection;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NCardGridSelectionScreen))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignalsAndInitGrid();
  }

  protected virtual void ConnectSignalsAndInitGrid()
  {
    this._grid = ((Node) this).GetNode<NCardGrid>(NodePath.op_Implicit("%CardGrid"));
    NCardGrid grid = this._grid;
    IReadOnlyList<CardModel> cards = this._cards;
    int capacity = 1;
    List<SortingOrders> sortingPriority = new List<SortingOrders>(capacity);
    CollectionsMarshal.SetCount<SortingOrders>(sortingPriority, capacity);
    CollectionsMarshal.AsSpan<SortingOrders>(sortingPriority)[0] = SortingOrders.Ascending;
    grid.SetCards(cards, PileType.None, sortingPriority);
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>((Action<NCardHolder>) (h => this.OnCardClicked(h.CardModel))), 0U);
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderAltPressed, Callable.From<NCardHolder>((Action<NCardHolder>) (h => this.ShowCardDetail(h.CardModel))), 0U);
    this._grid.InsetForTopBar();
    this._peekButton = ((Node) this).GetNode<NPeekButton>(NodePath.op_Implicit("%PeekButton"));
    ((GodotObject) this._peekButton).Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>((Action<NPeekButton>) (_ =>
    {
      if (this._peekButton.IsPeeking)
      {
        this.MouseFilter = (Control.MouseFilterEnum) 2L;
      }
      else
      {
        this.MouseFilter = (Control.MouseFilterEnum) 0L;
        ActiveScreenContext.Instance.Update();
      }
    })), 0U);
    Callable callable = Callable.From(new Action(this.SetPeekButtonTargets));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  protected abstract IEnumerable<Control> PeekButtonTargets { get; }

  protected abstract void OnCardClicked(CardModel card);

  public async Task<IEnumerable<CardModel>> CardsSelected() => await this._completionSource.Task;

  public override void _ExitTree() => this._completionSource.TrySetCanceled();

  private void SetPeekButtonTargets()
  {
    HashSet<Control> source = new HashSet<Control>()
    {
      (Control) this._grid
    };
    source.UnionWith(this.PeekButtonTargets);
    this._peekButton.AddTargets(source.ToArray<Control>());
  }

  public virtual void AfterOverlayOpened()
  {
  }

  public virtual void AfterOverlayClosed()
  {
    this._peekButton.SetPeeking(false);
    ((Node) this).QueueFreeSafely();
  }

  public virtual void AfterOverlayShown()
  {
    ((CanvasItem) this).Visible = true;
    if (!CombatManager.Instance.IsInProgress)
      return;
    this._peekButton.Enable();
  }

  public virtual void AfterOverlayHidden()
  {
    ((CanvasItem) this).Visible = false;
    this._peekButton.Disable();
  }

  public bool UseSharedBackstop => true;

  private void ShowCardDetail(CardModel card)
  {
    if (NControllerManager.Instance.IsUsingController)
      return;
    NGame.Instance.GetInspectCardScreen().Open(this._cards.ToList<CardModel>(), this._cards.IndexOf<CardModel>(card), this._grid.IsShowingUpgrades);
  }

  public virtual Control? DefaultFocusedControl
  {
    get
    {
      return this._peekButton.IsPeeking ? NCombatRoom.Instance.DefaultFocusedControl : this._grid.DefaultFocusedControl;
    }
  }

  public virtual Control? FocusedControlFromTopBar
  {
    get
    {
      return this._peekButton.IsPeeking ? NCombatRoom.Instance.FocusedControlFromTopBar : this._grid.FocusedControlFromTopBar;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NCardGridSelectionScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName.ConnectSignalsAndInitGrid, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName.SetPeekButtonTargets, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardGridSelectionScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.ConnectSignalsAndInitGrid) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignalsAndInitGrid();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.SetPeekButtonTargets) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetPeekButtonTargets();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayHidden) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterOverlayHidden();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.ConnectSignalsAndInitGrid) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.SetPeekButtonTargets) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NCardGridSelectionScreen.MethodName.AfterOverlayHidden) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName._grid))
    {
      this._grid = VariantUtils.ConvertTo<NCardGrid>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName._peekButton))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._peekButton = VariantUtils.ConvertTo<NPeekButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName._grid))
    {
      value = VariantUtils.CreateFrom<NCardGrid>(ref this._grid);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardGridSelectionScreen.PropertyName._peekButton))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NPeekButton>(ref this._peekButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardGridSelectionScreen.PropertyName._grid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGridSelectionScreen.PropertyName._peekButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCardGridSelectionScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardGridSelectionScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGridSelectionScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardGridSelectionScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardGridSelectionScreen.PropertyName._grid, Variant.From<NCardGrid>(ref this._grid));
    info.AddProperty(NCardGridSelectionScreen.PropertyName._peekButton, Variant.From<NPeekButton>(ref this._peekButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardGridSelectionScreen.PropertyName._grid, ref variant1))
      this._grid = ((Variant) ref variant1).As<NCardGrid>();
    Variant variant2;
    if (!info.TryGetProperty(NCardGridSelectionScreen.PropertyName._peekButton, ref variant2))
      return;
    this._peekButton = ((Variant) ref variant2).As<NPeekButton>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignalsAndInitGrid = StringName.op_Implicit(nameof (ConnectSignalsAndInitGrid));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetPeekButtonTargets = StringName.op_Implicit(nameof (SetPeekButtonTargets));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName _grid = StringName.op_Implicit(nameof (_grid));
    public static readonly StringName _peekButton = StringName.op_Implicit(nameof (_peekButton));
  }

  public class SignalName : Control.SignalName
  {
  }
}
