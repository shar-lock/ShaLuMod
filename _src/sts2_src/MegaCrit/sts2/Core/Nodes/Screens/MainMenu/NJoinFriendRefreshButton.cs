// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NJoinFriendRefreshButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendRefreshButton.cs")]
public class NJoinFriendRefreshButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.01f);
  private static readonly Vector2 _pressScale = Vector2.op_Multiply(Vector2.One, 0.99f);
  private const float _defaultV = 0.9f;
  private const float _hoverV = 1.2f;
  private Tween? _tween;
  private ShaderMaterial _hsv;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.accept)
      };
    }
  }

  public ulong PlayerId { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(new LocString("main_menu_ui", "JOIN_FRIENDS_MENU.refresh").GetFormattedText());
    this._hsv = (ShaderMaterial) ((CanvasItem) this).Material;
  }

  protected override void OnPress()
  {
    base.OnPress();
    this.Scale = NJoinFriendRefreshButton._pressScale;
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this.Scale = Vector2.One;
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this.Scale = NJoinFriendRefreshButton._hoverScale;
    this._hsv.SetShaderParameter(NJoinFriendRefreshButton._v, Variant.op_Implicit(1.2f));
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1.2f), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderParam(float newV)
  {
    this._hsv.SetShaderParameter(NJoinFriendRefreshButton._v, Variant.op_Implicit(newV));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NJoinFriendRefreshButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendRefreshButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendRefreshButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendRefreshButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendRefreshButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendRefreshButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("newV"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName._Ready) || StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnPress) || StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NJoinFriendRefreshButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName.PlayerId))
    {
      this.PlayerId = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName._hsv))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName.PlayerId))
    {
      ref godot_variant local = ref value;
      ulong playerId = this.PlayerId;
      godot_variant from = VariantUtils.CreateFrom<ulong>(ref playerId);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NJoinFriendRefreshButton.PropertyName._hsv))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NJoinFriendRefreshButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendRefreshButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NJoinFriendRefreshButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NJoinFriendRefreshButton.PropertyName.PlayerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName playerId1 = NJoinFriendRefreshButton.PropertyName.PlayerId;
    ulong playerId2 = this.PlayerId;
    Variant variant = Variant.From<ulong>(ref playerId2);
    serializationInfo.AddProperty(playerId1, variant);
    info.AddProperty(NJoinFriendRefreshButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NJoinFriendRefreshButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NJoinFriendRefreshButton.PropertyName.PlayerId, ref variant1))
      this.PlayerId = ((Variant) ref variant1).As<ulong>();
    Variant variant2;
    if (info.TryGetProperty(NJoinFriendRefreshButton.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NJoinFriendRefreshButton.PropertyName._hsv, ref variant3))
      return;
    this._hsv = ((Variant) ref variant3).As<ShaderMaterial>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName PlayerId = StringName.op_Implicit(nameof (PlayerId));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
