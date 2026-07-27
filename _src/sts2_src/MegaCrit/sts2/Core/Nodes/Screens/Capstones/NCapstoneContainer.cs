// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Capstones.NCapstoneContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Capstones;

[ScriptPath("res://src/Core/Nodes/Screens/Capstones/NCapstoneContainer.cs")]
public class NCapstoneContainer : Control
{
  private Control _backstop;
  private Tween? _backstopFade;
  private 
  #nullable disable
  NCapstoneContainer.ChangedEventHandler backing_Changed;
  private NCapstoneContainer.CapstoneClosedEventHandler backing_CapstoneClosed;

  public 
  #nullable enable
  ICapstoneScreen? CurrentCapstoneScreen { get; private set; }

  public bool InUse => this.CurrentCapstoneScreen != null;

  public static NCapstoneContainer? Instance => NRun.Instance?.GlobalUi.CapstoneContainer;

  public override void _Ready()
  {
    this._backstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("CapstoneBackstop"));
    ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenChanged);
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenChanged);
  }

  public void Open(ICapstoneScreen screen)
  {
    NHoverTipSet.Clear();
    bool flag = this.CurrentCapstoneScreen != null;
    if (flag)
      this.CloseInternal();
    this._backstopFade?.Kill();
    NOverlayStack.Instance.HideOverlays();
    if (!screen.UseSharedBackstop)
      ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
    else if (flag || NOverlayStack.Instance.ScreenCount > 0)
    {
      ((CanvasItem) this._backstop).Modulate = Colors.White;
    }
    else
    {
      this._backstopFade = ((Node) this).CreateTween();
      this._backstopFade.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    }
    this.CurrentCapstoneScreen = screen;
    if (!((Node) this).GetChildren(false).Contains((Node) screen))
      ((Node) this).AddChildSafely((Node) screen);
    ((Node) screen).ProcessMode = (Node.ProcessModeEnum) 0L;
    screen.AfterCapstoneOpened();
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      CombatManager.Instance.Pause();
    ActiveScreenContext.Instance.Update();
    ((GodotObject) this).EmitSignal(NCapstoneContainer.SignalName.Changed, Array.Empty<Variant>());
  }

  public void Close()
  {
    if (this.CurrentCapstoneScreen == null)
      return;
    this.CloseInternal();
    ActiveScreenContext.Instance.Update();
    ((GodotObject) this).EmitSignal(NCapstoneContainer.SignalName.CapstoneClosed, Array.Empty<Variant>());
    ((GodotObject) this).EmitSignal(NCapstoneContainer.SignalName.Changed, Array.Empty<Variant>());
  }

  private void CloseInternal()
  {
    if (RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      CombatManager.Instance.Unpause();
    NOverlayStack.Instance.ShowOverlays();
    if (NOverlayStack.Instance.ScreenCount > 0)
    {
      ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
    }
    else
    {
      this._backstopFade?.Kill();
      this._backstopFade = ((Node) this).CreateTween();
      this._backstopFade.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    }
    ICapstoneScreen currentCapstoneScreen = this.CurrentCapstoneScreen;
    this.CurrentCapstoneScreen = (ICapstoneScreen) null;
    if (currentCapstoneScreen is Node node)
      node.ProcessMode = (Node.ProcessModeEnum) 4L;
    currentCapstoneScreen?.AfterCapstoneClosed();
    NHoverTipSet.Clear();
  }

  public void DisableBackstopInstantly()
  {
    this._backstopFade?.Kill();
    ((CanvasItem) this._backstop).Modulate = Colors.Transparent;
  }

  public void EnableBackstopInstantly()
  {
    this._backstopFade?.Kill();
    ((CanvasItem) this._backstop).Modulate = Colors.White;
  }

  public void CleanUp()
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer)
      return;
    CombatManager.Instance.Unpause();
  }

  private void OnActiveScreenChanged()
  {
    if (!this.InUse)
      return;
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this.CurrentCapstoneScreen))
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
      new MethodInfo(NCapstoneContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName.CloseInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName.DisableBackstopInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName.EnableBackstopInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName.CleanUp, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.MethodName.OnActiveScreenChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName.CloseInternal) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CloseInternal();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName.DisableBackstopInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableBackstopInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName.EnableBackstopInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableBackstopInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneContainer.MethodName.CleanUp) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CleanUp();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCapstoneContainer.MethodName.OnActiveScreenChanged) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenChanged();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCapstoneContainer.MethodName._Ready) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName.Close) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName.CloseInternal) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName.DisableBackstopInstantly) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName.EnableBackstopInstantly) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName.CleanUp) || StringName.op_Equality(ref method, NCapstoneContainer.MethodName.OnActiveScreenChanged) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCapstoneContainer.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCapstoneContainer.PropertyName._backstopFade))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._backstopFade = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCapstoneContainer.PropertyName.InUse))
    {
      ref godot_variant local = ref value;
      bool inUse = this.InUse;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref inUse);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCapstoneContainer.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._backstop);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCapstoneContainer.PropertyName._backstopFade))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._backstopFade);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCapstoneContainer.PropertyName.InUse, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCapstoneContainer.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCapstoneContainer.PropertyName._backstopFade, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCapstoneContainer.PropertyName._backstop, Variant.From<Control>(ref this._backstop));
    info.AddProperty(NCapstoneContainer.PropertyName._backstopFade, Variant.From<Tween>(ref this._backstopFade));
    info.AddSignalEventDelegate(NCapstoneContainer.SignalName.Changed, (Delegate) this.backing_Changed);
    info.AddSignalEventDelegate(NCapstoneContainer.SignalName.CapstoneClosed, (Delegate) this.backing_CapstoneClosed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCapstoneContainer.PropertyName._backstop, ref variant1))
      this._backstop = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCapstoneContainer.PropertyName._backstopFade, ref variant2))
      this._backstopFade = ((Variant) ref variant2).As<Tween>();
    NCapstoneContainer.ChangedEventHandler changedEventHandler;
    if (info.TryGetSignalEventDelegate<NCapstoneContainer.ChangedEventHandler>(NCapstoneContainer.SignalName.Changed, ref changedEventHandler))
      this.backing_Changed = changedEventHandler;
    NCapstoneContainer.CapstoneClosedEventHandler closedEventHandler;
    if (!info.TryGetSignalEventDelegate<NCapstoneContainer.CapstoneClosedEventHandler>(NCapstoneContainer.SignalName.CapstoneClosed, ref closedEventHandler))
      return;
    this.backing_CapstoneClosed = closedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCapstoneContainer.SignalName.Changed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneContainer.SignalName.CapstoneClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NCapstoneContainer.ChangedEventHandler Changed
  {
    add => this.backing_Changed += value;
    remove => this.backing_Changed -= value;
  }

  protected void EmitSignalChanged()
  {
    ((GodotObject) this).EmitSignal(NCapstoneContainer.SignalName.Changed, Array.Empty<Variant>());
  }

  public event NCapstoneContainer.CapstoneClosedEventHandler CapstoneClosed
  {
    add => this.backing_CapstoneClosed += value;
    remove => this.backing_CapstoneClosed -= value;
  }

  protected void EmitSignalCapstoneClosed()
  {
    ((GodotObject) this).EmitSignal(NCapstoneContainer.SignalName.CapstoneClosed, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCapstoneContainer.SignalName.Changed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCapstoneContainer.ChangedEventHandler backingChanged = this.backing_Changed;
      if (backingChanged == null)
        return;
      backingChanged();
    }
    else if (StringName.op_Equality(ref signal, NCapstoneContainer.SignalName.CapstoneClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCapstoneContainer.CapstoneClosedEventHandler backingCapstoneClosed = this.backing_CapstoneClosed;
      if (backingCapstoneClosed == null)
        return;
      backingCapstoneClosed();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCapstoneContainer.SignalName.Changed) || StringName.op_Equality(ref signal, NCapstoneContainer.SignalName.CapstoneClosed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ChangedEventHandler();

  [Signal]
  public delegate void CapstoneClosedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName CloseInternal = StringName.op_Implicit(nameof (CloseInternal));
    public static readonly StringName DisableBackstopInstantly = StringName.op_Implicit(nameof (DisableBackstopInstantly));
    public static readonly StringName EnableBackstopInstantly = StringName.op_Implicit(nameof (EnableBackstopInstantly));
    public static readonly StringName CleanUp = StringName.op_Implicit(nameof (CleanUp));
    public static readonly StringName OnActiveScreenChanged = StringName.op_Implicit(nameof (OnActiveScreenChanged));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName InUse = StringName.op_Implicit(nameof (InUse));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _backstopFade = StringName.op_Implicit(nameof (_backstopFade));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Changed = StringName.op_Implicit(nameof (Changed));
    public static readonly StringName CapstoneClosed = StringName.op_Implicit(nameof (CapstoneClosed));
  }
}
