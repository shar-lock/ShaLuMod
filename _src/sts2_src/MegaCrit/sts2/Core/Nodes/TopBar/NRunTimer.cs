// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.TopBar.NRunTimer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NRunTimer.cs")]
public class NRunTimer : Control
{
  private MegaLabel _timerLabel;
  private Timer _timer;

  public override void _Ready()
  {
    this._timerLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("TimerLabel"));
    this.ToggleTimer(false);
    ((GodotObject) this).CallDeferred(StringName.op_Implicit("DeferredInit"), Array.Empty<Variant>());
  }

  private void DeferredInit()
  {
    ((GodotObject) NMapScreen.Instance).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.RefreshVisibility)), 0U);
    ((GodotObject) NCapstoneContainer.Instance).Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.RefreshVisibility)), 0U);
    this._timer = new Timer();
    this._timer.WaitTime = 1.0;
    this._timer.Autostart = false;
    ((GodotObject) this._timer).Connect(Timer.SignalName.Timeout, Callable.From((Action) (() =>
    {
      this.RefreshVisibility();
      this.OnTimerTimeout();
    })), 0U);
    ((Node) this).AddChildSafely((Node) this._timer);
    this._timer.Start(-1.0);
  }

  public override void _ExitTree() => this._timer.Stop();

  public void RefreshVisibility()
  {
    if (SaveManager.Instance.PrefsSave.ShowRunTimer)
      this.ToggleTimer(true);
    else
      this.ToggleTimer(NCapstoneContainer.Instance.InUse || ((CanvasItem) NMapScreen.Instance).Visible);
  }

  private void ToggleTimer(bool on) => ((CanvasItem) this).Visible = on;

  private void OnTimerTimeout()
  {
    if (RunManager.Instance.IsGameOver)
      return;
    this._timerLabel.SetTextAutoSize(TimeFormatting.Format((float) RunManager.Instance.RunTime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NRunTimer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunTimer.MethodName.DeferredInit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunTimer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunTimer.MethodName.RefreshVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunTimer.MethodName.ToggleTimer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("on"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRunTimer.MethodName.OnTimerTimeout, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunTimer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunTimer.MethodName.DeferredInit) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DeferredInit();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunTimer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunTimer.MethodName.RefreshVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunTimer.MethodName.ToggleTimer) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleTimer(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRunTimer.MethodName.OnTimerTimeout) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnTimerTimeout();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunTimer.MethodName._Ready) || StringName.op_Equality(ref method, NRunTimer.MethodName.DeferredInit) || StringName.op_Equality(ref method, NRunTimer.MethodName._ExitTree) || StringName.op_Equality(ref method, NRunTimer.MethodName.RefreshVisibility) || StringName.op_Equality(ref method, NRunTimer.MethodName.ToggleTimer) || StringName.op_Equality(ref method, NRunTimer.MethodName.OnTimerTimeout) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunTimer.PropertyName._timerLabel))
    {
      this._timerLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunTimer.PropertyName._timer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._timer = VariantUtils.ConvertTo<Timer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunTimer.PropertyName._timerLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._timerLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunTimer.PropertyName._timer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Timer>(ref this._timer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunTimer.PropertyName._timerLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunTimer.PropertyName._timer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRunTimer.PropertyName._timerLabel, Variant.From<MegaLabel>(ref this._timerLabel));
    info.AddProperty(NRunTimer.PropertyName._timer, Variant.From<Timer>(ref this._timer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunTimer.PropertyName._timerLabel, ref variant1))
      this._timerLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NRunTimer.PropertyName._timer, ref variant2))
      return;
    this._timer = ((Variant) ref variant2).As<Timer>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName DeferredInit = StringName.op_Implicit(nameof (DeferredInit));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName RefreshVisibility = StringName.op_Implicit(nameof (RefreshVisibility));
    public static readonly StringName ToggleTimer = StringName.op_Implicit(nameof (ToggleTimer));
    public static readonly StringName OnTimerTimeout = StringName.op_Implicit(nameof (OnTimerTimeout));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _timerLabel = StringName.op_Implicit(nameof (_timerLabel));
    public static readonly StringName _timer = StringName.op_Implicit(nameof (_timer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
