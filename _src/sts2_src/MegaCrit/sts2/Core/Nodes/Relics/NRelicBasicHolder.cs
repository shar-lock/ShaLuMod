// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Relics.NRelicBasicHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Relics;

[ScriptPath("res://src/Core/Nodes/Relics/NRelicBasicHolder.cs")]
public class NRelicBasicHolder : NButton
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("relics/relic_basic_holder");
  private NRelic _relic;
  private Tween? _hoverTween;
  private RelicModel _model;

  public NRelic Relic => this._relic;

  public static NRelicBasicHolder? Create(RelicModel relic)
  {
    if (TestMode.IsOn)
      return (NRelicBasicHolder) null;
    NRelicBasicHolder nrelicBasicHolder = PreloadManager.Cache.GetScene(NRelicBasicHolder._scenePath).Instantiate<NRelicBasicHolder>((PackedScene.GenEditState) 0L);
    ((Node) nrelicBasicHolder).Name = StringName.op_Implicit($"NRelicBasicHolder-{relic.Id}");
    nrelicBasicHolder._model = relic;
    return nrelicBasicHolder;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._relic = ((Node) this).GetNode<NRelic>(NodePath.op_Implicit("%Relic"));
    this._relic.Model = this._model;
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._hoverTween?.Kill();
  }

  protected override void OnFocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.25f)), 0.05);
    NHoverTipSet.CreateAndShow((Control) this, this._relic.Model.HoverTips)?.SetAlignmentForRelic(this._relic);
  }

  protected override void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._relic.Icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NRelicBasicHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicBasicHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicBasicHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicBasicHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicBasicHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicBasicHolder.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicBasicHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicBasicHolder.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicBasicHolder.MethodName._Ready) || StringName.op_Equality(ref method, NRelicBasicHolder.MethodName._ExitTree) || StringName.op_Equality(ref method, NRelicBasicHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NRelicBasicHolder.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicBasicHolder.PropertyName._relic))
    {
      this._relic = VariantUtils.ConvertTo<NRelic>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicBasicHolder.PropertyName._hoverTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicBasicHolder.PropertyName.Relic))
    {
      ref godot_variant local = ref value;
      NRelic relic = this.Relic;
      godot_variant from = VariantUtils.CreateFrom<NRelic>(ref relic);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicBasicHolder.PropertyName._relic))
    {
      value = VariantUtils.CreateFrom<NRelic>(ref this._relic);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicBasicHolder.PropertyName._hoverTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelicBasicHolder.PropertyName._relic, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicBasicHolder.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicBasicHolder.PropertyName.Relic, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRelicBasicHolder.PropertyName._relic, Variant.From<NRelic>(ref this._relic));
    info.AddProperty(NRelicBasicHolder.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicBasicHolder.PropertyName._relic, ref variant1))
      this._relic = ((Variant) ref variant1).As<NRelic>();
    Variant variant2;
    if (!info.TryGetProperty(NRelicBasicHolder.PropertyName._hoverTween, ref variant2))
      return;
    this._hoverTween = ((Variant) ref variant2).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Relic = StringName.op_Implicit(nameof (Relic));
    public static readonly StringName _relic = StringName.op_Implicit(nameof (_relic));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
