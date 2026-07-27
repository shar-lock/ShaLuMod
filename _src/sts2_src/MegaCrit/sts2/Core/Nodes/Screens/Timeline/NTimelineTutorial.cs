// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NTimelineTutorial
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NTimelineTutorial.cs")]
public class NTimelineTutorial : Control, IScreenContext
{
  private MegaRichTextLabel _text;
  private NAcknowledgeButton _acknowledgeButton;
  private NTimelineScreen _timeline;
  private Tween? _tween;

  public void Init(NTimelineScreen screen)
  {
    this._timeline = screen;
    screen.HideBackButtonImmediately();
  }

  public override void _Ready()
  {
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock");
    this._text = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TutorialText"));
    this._text.Text = $"[center]{new LocString("timeline", "TUTORIAL_TEXT").GetRawText()}[/center]";
    this._acknowledgeButton = ((Node) this).GetNode<NAcknowledgeButton>(NodePath.op_Implicit("%AcknowledgeButton"));
    ((GodotObject) this._acknowledgeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.CloseTutorial)), 0U);
    this.AnimateTutorial();
  }

  private void CloseTutorial(NButton _)
  {
    this._acknowledgeButton.Disable();
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() =>
    {
      TaskHelper.RunSafely(this._timeline.SpawnFirstTimeTimeline());
      ((Node) this).QueueFreeSafely();
    })));
  }

  private void AnimateTutorial()
  {
    this._acknowledgeButton.Disable();
    this._text.VisibleRatio = 0.0f;
    MegaRichTextLabel text = this._text;
    Color modulate = ((CanvasItem) this._text).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) text).Modulate = color;
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._text, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    this._tween.Parallel().TweenProperty((GodotObject) this._text, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0);
    this._tween.Chain().TweenCallback(Callable.From((Action) (() => this._acknowledgeButton.Enable())));
    this._tween.Parallel().TweenProperty((GodotObject) this._acknowledgeButton, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(1.0);
    this._tween.Parallel().TweenProperty((GodotObject) this._acknowledgeButton, NodePath.op_Implicit("position:y"), Variant.op_Implicit(920f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).SetDelay(1.0);
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NTimelineTutorial.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("screen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineTutorial.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTimelineTutorial.MethodName.CloseTutorial, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTimelineTutorial.MethodName.AnimateTutorial, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTimelineTutorial.MethodName.Init) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Init(VariantUtils.ConvertTo<NTimelineScreen>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineTutorial.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTimelineTutorial.MethodName.CloseTutorial) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CloseTutorial(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTimelineTutorial.MethodName.AnimateTutorial) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AnimateTutorial();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTimelineTutorial.MethodName.Init) || StringName.op_Equality(ref method, NTimelineTutorial.MethodName._Ready) || StringName.op_Equality(ref method, NTimelineTutorial.MethodName.CloseTutorial) || StringName.op_Equality(ref method, NTimelineTutorial.MethodName.AnimateTutorial) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._text))
    {
      this._text = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._acknowledgeButton))
    {
      this._acknowledgeButton = VariantUtils.ConvertTo<NAcknowledgeButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._timeline))
    {
      this._timeline = VariantUtils.ConvertTo<NTimelineScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._text))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._text);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._acknowledgeButton))
    {
      value = VariantUtils.CreateFrom<NAcknowledgeButton>(ref this._acknowledgeButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._timeline))
    {
      value = VariantUtils.CreateFrom<NTimelineScreen>(ref this._timeline);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTimelineTutorial.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTimelineTutorial.PropertyName._text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineTutorial.PropertyName._acknowledgeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineTutorial.PropertyName._timeline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineTutorial.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTimelineTutorial.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTimelineTutorial.PropertyName._text, Variant.From<MegaRichTextLabel>(ref this._text));
    info.AddProperty(NTimelineTutorial.PropertyName._acknowledgeButton, Variant.From<NAcknowledgeButton>(ref this._acknowledgeButton));
    info.AddProperty(NTimelineTutorial.PropertyName._timeline, Variant.From<NTimelineScreen>(ref this._timeline));
    info.AddProperty(NTimelineTutorial.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTimelineTutorial.PropertyName._text, ref variant1))
      this._text = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NTimelineTutorial.PropertyName._acknowledgeButton, ref variant2))
      this._acknowledgeButton = ((Variant) ref variant2).As<NAcknowledgeButton>();
    Variant variant3;
    if (info.TryGetProperty(NTimelineTutorial.PropertyName._timeline, ref variant3))
      this._timeline = ((Variant) ref variant3).As<NTimelineScreen>();
    Variant variant4;
    if (!info.TryGetProperty(NTimelineTutorial.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName CloseTutorial = StringName.op_Implicit(nameof (CloseTutorial));
    public static readonly StringName AnimateTutorial = StringName.op_Implicit(nameof (AnimateTutorial));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _text = StringName.op_Implicit(nameof (_text));
    public static readonly StringName _acknowledgeButton = StringName.op_Implicit(nameof (_acknowledgeButton));
    public static readonly StringName _timeline = StringName.op_Implicit(nameof (_timeline));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
