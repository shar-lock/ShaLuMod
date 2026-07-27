// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockScreen.cs")]
public abstract class NUnlockScreen : Control, IScreenContext
{
  private NUnlockConfirmButton? _unlockConfirmButton;
  private Tween? _tween;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NUnlockScreen))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected void ConnectSignals()
  {
    this._unlockConfirmButton = ((Node) this).GetNode<NUnlockConfirmButton>(NodePath.op_Implicit("ConfirmButton"));
    ((GodotObject) this._unlockConfirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => TaskHelper.RunSafely(this.Close()))), 0U);
  }

  public virtual void Open()
  {
    NTimelineScreen.Instance.DisableInput();
    NTimelineScreen.Instance.CurrentUnlockScreen = this;
    this._unlockConfirmButton?.Disable();
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() => this._unlockConfirmButton?.Enable())));
  }

  protected async Task Close()
  {
    Log.Info($"Closing: {((Node) this).Name}");
    if (NTimelineScreen.Instance.CurrentUnlockScreen == this)
      NTimelineScreen.Instance.CurrentUnlockScreen = (NUnlockScreen) null;
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this.OnScreenPreClose();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentBlack), 1.0);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    this.OnScreenClose();
    if (NTimelineScreen.Instance.IsScreenQueued())
      NTimelineScreen.Instance.OpenQueuedScreen();
    else
      await NTimelineScreen.Instance.HideBackstopAndShowUi(true);
    ((Node) this).QueueFreeSafely();
  }

  protected virtual void OnScreenPreClose()
  {
  }

  protected virtual void OnScreenClose()
  {
  }

  public virtual Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NUnlockScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockScreen.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockScreen.MethodName.OnScreenPreClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockScreen.MethodName.OnScreenClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockScreen.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockScreen.MethodName.OnScreenPreClose) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenPreClose();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockScreen.MethodName.OnScreenClose) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnScreenClose();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockScreen.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NUnlockScreen.MethodName.Open) || StringName.op_Equality(ref method, NUnlockScreen.MethodName.OnScreenPreClose) || StringName.op_Equality(ref method, NUnlockScreen.MethodName.OnScreenClose) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockScreen.PropertyName._unlockConfirmButton))
    {
      this._unlockConfirmButton = VariantUtils.ConvertTo<NUnlockConfirmButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockScreen.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockScreen.PropertyName._unlockConfirmButton))
    {
      value = VariantUtils.CreateFrom<NUnlockConfirmButton>(ref this._unlockConfirmButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockScreen.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockScreen.PropertyName._unlockConfirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NUnlockScreen.PropertyName._unlockConfirmButton, Variant.From<NUnlockConfirmButton>(ref this._unlockConfirmButton));
    info.AddProperty(NUnlockScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockScreen.PropertyName._unlockConfirmButton, ref variant1))
      this._unlockConfirmButton = ((Variant) ref variant1).As<NUnlockConfirmButton>();
    Variant variant2;
    if (!info.TryGetProperty(NUnlockScreen.PropertyName._tween, ref variant2))
      return;
    this._tween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public static readonly StringName OnScreenPreClose = StringName.op_Implicit(nameof (OnScreenPreClose));
    public static readonly StringName OnScreenClose = StringName.op_Implicit(nameof (OnScreenClose));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _unlockConfirmButton = StringName.op_Implicit(nameof (_unlockConfirmButton));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
