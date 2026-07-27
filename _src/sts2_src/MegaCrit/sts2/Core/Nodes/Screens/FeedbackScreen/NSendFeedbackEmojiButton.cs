// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NSendFeedbackEmojiButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackEmojiButton.cs")]
public class NSendFeedbackEmojiButton : NButton
{
  private static readonly Color _defaultColor = new Color(0.1f, 0.1f, 0.1f, 1f);
  private Tween? _tween;
  private bool _isSelected;
  private NSelectionReticle _selectionReticle;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
    this.PivotOffset = Vector2.op_Multiply(this.Size, 0.5f);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this.Scale = Vector2.op_Multiply(Vector2.One, 1.2f);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    base.OnFocus();
    if (this._isSelected)
      return;
    this._selectionReticle.OnDeselect();
    this.Scale = Vector2.One;
  }

  public void SetSelected(bool isSelected)
  {
    if (isSelected)
    {
      this.Scale = Vector2.op_Multiply(Vector2.One, 1.2f);
      ((CanvasItem) this).SelfModulate = StsColors.blueGlow;
    }
    else
    {
      this.Scale = Vector2.One;
      ((CanvasItem) this).SelfModulate = NSendFeedbackEmojiButton._defaultColor;
    }
    this._isSelected = isSelected;
  }

  public void FlashError()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(StsColors.redGlow), 0.10000000149011612);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(NSendFeedbackEmojiButton._defaultColor), 0.10000000149011612);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(StsColors.redGlow), 0.10000000149011612);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(NSendFeedbackEmojiButton._defaultColor), 0.10000000149011612);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSendFeedbackEmojiButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackEmojiButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackEmojiButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackEmojiButton.MethodName.SetSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isSelected"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackEmojiButton.MethodName.FlashError, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.SetSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSelected(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.FlashError) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.FlashError();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName._Ready) || StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.SetSelected) || StringName.op_Equality(ref method, NSendFeedbackEmojiButton.MethodName.FlashError) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackEmojiButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackEmojiButton.PropertyName._isSelected))
    {
      this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackEmojiButton.PropertyName._selectionReticle))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackEmojiButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackEmojiButton.PropertyName._isSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackEmojiButton.PropertyName._selectionReticle))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackEmojiButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSendFeedbackEmojiButton.PropertyName._isSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackEmojiButton.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSendFeedbackEmojiButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NSendFeedbackEmojiButton.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
    info.AddProperty(NSendFeedbackEmojiButton.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSendFeedbackEmojiButton.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NSendFeedbackEmojiButton.PropertyName._isSelected, ref variant2))
      this._isSelected = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (!info.TryGetProperty(NSendFeedbackEmojiButton.PropertyName._selectionReticle, ref variant3))
      return;
    this._selectionReticle = ((Variant) ref variant3).As<NSelectionReticle>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName SetSelected = StringName.op_Implicit(nameof (SetSelected));
    public static readonly StringName FlashError = StringName.op_Implicit(nameof (FlashError));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _isSelected = StringName.op_Implicit(nameof (_isSelected));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
