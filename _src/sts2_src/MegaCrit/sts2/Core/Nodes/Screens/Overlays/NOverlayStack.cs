// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Overlays.NOverlayStack
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Overlays;

[ScriptPath("res://src/Core/Nodes/Screens/Overlays/NOverlayStack.cs")]
public class NOverlayStack : Control
{
  private readonly List<IOverlayScreen> _overlays = new List<IOverlayScreen>();
  private Control _backstop;
  private Tween? _backstopFade;
  private 
  #nullable disable
  NOverlayStack.ChangedEventHandler backing_Changed;

  public static 
  #nullable enable
  NOverlayStack? Instance => NRun.Instance?.GlobalUi.Overlays;

  public int ScreenCount => this._overlays.Count;

  private static bool StackIsCovered
  {
    get
    {
      NMapScreen instance1 = NMapScreen.Instance;
      if ((instance1 != null ? (instance1.IsOpen ? 1 : 0) : 0) != 0)
        return true;
      NCapstoneContainer instance2 = NCapstoneContainer.Instance;
      return instance2 != null && instance2.InUse;
    }
  }

  public override void _Ready()
  {
    this._backstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("OverlayBackstop"));
    ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
    this._backstop.MouseFilter = (Control.MouseFilterEnum) 2L;
    Callable callable1 = Callable.From<Error>((Func<Error>) (() => ((GodotObject) NMapScreen.Instance).Connect(NMapScreen.SignalName.Opened, Callable.From(new Action(this.HideOverlays)), 0U)));
    ((Callable) ref callable1).CallDeferred(Array.Empty<Variant>());
    Callable callable2 = Callable.From<Error>((Func<Error>) (() => ((GodotObject) NMapScreen.Instance).Connect(NMapScreen.SignalName.Closed, Callable.From(new Action(this.ShowOverlays)), 0U)));
    ((Callable) ref callable2).CallDeferred(Array.Empty<Variant>());
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenChanged);
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenChanged);
    this.Clear();
  }

  public void Push(IOverlayScreen screen)
  {
    this.Peek()?.AfterOverlayHidden();
    ((Node) this).AddChildSafely((Node) screen);
    this._overlays.Add(screen);
    screen.AfterOverlayOpened();
    screen.AfterOverlayShown();
    this._backstop.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._backstopFade?.Kill();
    ((Node) this).MoveChildSafely((Node) this._backstop, this._overlays.IndexOf(screen));
    bool stackIsCovered = NOverlayStack.StackIsCovered;
    if (stackIsCovered)
      screen.AfterOverlayHidden();
    if (!screen.UseSharedBackstop | stackIsCovered)
      ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
    else if (this.ScreenCount == 1)
      this.ShowBackstop();
    else
      ((CanvasItem) this._backstop).Modulate = Colors.White;
    ActiveScreenContext.Instance.Update();
    ((GodotObject) this).EmitSignal(NOverlayStack.SignalName.Changed, Array.Empty<Variant>());
  }

  public void Remove(IOverlayScreen screen)
  {
    bool flag = screen == this.Peek();
    if (flag)
    {
      this.HideBackstop();
      screen.AfterOverlayHidden();
    }
    screen.AfterOverlayClosed();
    this._overlays.Remove(screen);
    if (flag)
    {
      IOverlayScreen overlayScreen = this.Peek();
      if (overlayScreen != null)
      {
        this._backstop.MouseFilter = (Control.MouseFilterEnum) 0L;
        ((Node) this).MoveChildSafely((Node) this._backstop, this._overlays.IndexOf(overlayScreen));
        if (overlayScreen.UseSharedBackstop)
          ((CanvasItem) this._backstop).Modulate = Colors.White;
        else
          this.HideBackstop();
        overlayScreen.AfterOverlayShown();
      }
      else
        this.HideBackstop();
    }
    ActiveScreenContext.Instance.Update();
    ((GodotObject) this).EmitSignal(NOverlayStack.SignalName.Changed, Array.Empty<Variant>());
  }

  public void Clear()
  {
    for (IOverlayScreen screen = this.Peek(); screen != null; screen = this.Peek())
      this.Remove(screen);
  }

  public void HideOverlays()
  {
    ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
    this.Peek()?.AfterOverlayHidden();
  }

  public void ShowOverlays()
  {
    IOverlayScreen overlayScreen = this.Peek();
    if (overlayScreen == null || NMapScreen.Instance.IsOpen)
      return;
    ((CanvasItem) this._backstop).Modulate = overlayScreen.UseSharedBackstop ? Colors.White : Colors.Transparent;
    overlayScreen.AfterOverlayShown();
  }

  public void ShowBackstop()
  {
    IOverlayScreen overlayScreen = this.Peek();
    if ((overlayScreen != null ? (!overlayScreen.UseSharedBackstop ? 1 : 0) : 0) != 0)
      return;
    this._backstop.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._backstopFade?.Kill();
    this._backstopFade = ((Node) this).CreateTween();
    this._backstopFade.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public void HideBackstop()
  {
    IOverlayScreen overlayScreen = this.Peek();
    if ((overlayScreen != null ? (!overlayScreen.UseSharedBackstop ? 1 : 0) : 0) != 0)
      return;
    this._backstop.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._backstopFade?.Kill();
    if (this.ScreenCount <= 1)
    {
      this._backstopFade = ((Node) this).CreateTween();
      this._backstopFade.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    }
    else
      ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
  }

  public IOverlayScreen? Peek() => this._overlays.LastOrDefault<IOverlayScreen>();

  private void OnActiveScreenChanged()
  {
    IOverlayScreen screen = this.Peek();
    if (screen == null)
      return;
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) screen))
      this.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 2L;
    else
      this.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NOverlayStack.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName.Clear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName.HideOverlays, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName.ShowOverlays, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName.ShowBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName.HideBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOverlayStack.MethodName.OnActiveScreenChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName.Clear) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Clear();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName.HideOverlays) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideOverlays();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName.ShowOverlays) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowOverlays();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName.ShowBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowBackstop();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOverlayStack.MethodName.HideBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideBackstop();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NOverlayStack.MethodName.OnActiveScreenChanged) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenChanged();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOverlayStack.MethodName._Ready) || StringName.op_Equality(ref method, NOverlayStack.MethodName._EnterTree) || StringName.op_Equality(ref method, NOverlayStack.MethodName._ExitTree) || StringName.op_Equality(ref method, NOverlayStack.MethodName.Clear) || StringName.op_Equality(ref method, NOverlayStack.MethodName.HideOverlays) || StringName.op_Equality(ref method, NOverlayStack.MethodName.ShowOverlays) || StringName.op_Equality(ref method, NOverlayStack.MethodName.ShowBackstop) || StringName.op_Equality(ref method, NOverlayStack.MethodName.HideBackstop) || StringName.op_Equality(ref method, NOverlayStack.MethodName.OnActiveScreenChanged) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOverlayStack.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOverlayStack.PropertyName._backstopFade))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._backstopFade = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOverlayStack.PropertyName.ScreenCount))
    {
      ref godot_variant local = ref value;
      int screenCount = this.ScreenCount;
      godot_variant from = VariantUtils.CreateFrom<int>(ref screenCount);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NOverlayStack.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._backstop);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOverlayStack.PropertyName._backstopFade))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._backstopFade);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NOverlayStack.PropertyName.ScreenCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOverlayStack.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOverlayStack.PropertyName._backstopFade, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NOverlayStack.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
    info.AddProperty(NOverlayStack.PropertyName._backstopFade, Variant.From<Tween>(ref this._backstopFade));
    info.AddSignalEventDelegate(NOverlayStack.SignalName.Changed, (Delegate) this.backing_Changed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOverlayStack.PropertyName._backstop, ref variant1))
      this._backstop = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NOverlayStack.PropertyName._backstopFade, ref variant2))
      this._backstopFade = ((Variant) ref variant2).As<Tween>();
    NOverlayStack.ChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NOverlayStack.ChangedEventHandler>(NOverlayStack.SignalName.Changed, ref changedEventHandler))
      return;
    this.backing_Changed = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NOverlayStack.SignalName.Changed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NOverlayStack.ChangedEventHandler Changed
  {
    add => this.backing_Changed += value;
    remove => this.backing_Changed -= value;
  }

  protected void EmitSignalChanged()
  {
    ((GodotObject) this).EmitSignal(NOverlayStack.SignalName.Changed, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NOverlayStack.SignalName.Changed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NOverlayStack.ChangedEventHandler backingChanged = this.backing_Changed;
      if (backingChanged == null)
        return;
      backingChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NOverlayStack.SignalName.Changed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ChangedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Clear = StringName.op_Implicit(nameof (Clear));
    public static readonly StringName HideOverlays = StringName.op_Implicit(nameof (HideOverlays));
    public static readonly StringName ShowOverlays = StringName.op_Implicit(nameof (ShowOverlays));
    public static readonly StringName ShowBackstop = StringName.op_Implicit(nameof (ShowBackstop));
    public static readonly StringName HideBackstop = StringName.op_Implicit(nameof (HideBackstop));
    public static readonly StringName OnActiveScreenChanged = StringName.op_Implicit(nameof (OnActiveScreenChanged));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScreenCount = StringName.op_Implicit(nameof (ScreenCount));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _backstopFade = StringName.op_Implicit(nameof (_backstopFade));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Changed = StringName.op_Implicit(nameof (Changed));
  }
}
