// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NJoinFriendButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendButton.cs")]
public class NJoinFriendButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  public static readonly string scenePath = SceneHelper.GetScenePath("ui/multiplayer/join_friend_button");
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.01f);
  private static readonly Vector2 _pressScale = Vector2.op_Multiply(Vector2.One, 0.99f);
  private const float _defaultV = 0.9f;
  private const float _hoverV = 1.2f;
  private Tween? _tween;
  private ShaderMaterial _hsv;

  public ulong PlayerId { get; private set; }

  public static NJoinFriendButton Create(ulong playerId)
  {
    NJoinFriendButton njoinFriendButton = PreloadManager.Cache.GetScene(NJoinFriendButton.scenePath).Instantiate<NJoinFriendButton>((PackedScene.GenEditState) 0L);
    njoinFriendButton.PlayerId = playerId;
    return njoinFriendButton;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    MegaRichTextLabel node1 = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text"));
    NinePatchRect node2 = ((Node) this).GetNode<NinePatchRect>(NodePath.op_Implicit("Image"));
    node1.Text = $"[center]{PlatformUtil.GetPlayerName(PlatformUtil.PrimaryPlatform, this.PlayerId)}[/center]";
    this._hsv = (ShaderMaterial) ((CanvasItem) node2).Material;
  }

  protected override void OnPress()
  {
    base.OnPress();
    this.Scale = NJoinFriendButton._pressScale;
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
    this.Scale = NJoinFriendButton._hoverScale;
    this._hsv.SetShaderParameter(NJoinFriendButton._v, Variant.op_Implicit(1.2f));
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
    this._hsv.SetShaderParameter(NJoinFriendButton._v, Variant.op_Implicit(newV));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NJoinFriendButton.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NJoinFriendButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NJoinFriendButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NJoinFriendButton njoinFriendButton = NJoinFriendButton.Create(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NJoinFriendButton>(ref njoinFriendButton);
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NJoinFriendButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NJoinFriendButton.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NJoinFriendButton njoinFriendButton = NJoinFriendButton.Create(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NJoinFriendButton>(ref njoinFriendButton);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NJoinFriendButton.MethodName.Create) || StringName.op_Equality(ref method, NJoinFriendButton.MethodName._Ready) || StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnPress) || StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NJoinFriendButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NJoinFriendButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NJoinFriendButton.PropertyName.PlayerId))
    {
      this.PlayerId = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NJoinFriendButton.PropertyName._hsv))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NJoinFriendButton.PropertyName.PlayerId))
    {
      ref godot_variant local = ref value;
      ulong playerId = this.PlayerId;
      godot_variant from = VariantUtils.CreateFrom<ulong>(ref playerId);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NJoinFriendButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NJoinFriendButton.PropertyName._hsv))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NJoinFriendButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NJoinFriendButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NJoinFriendButton.PropertyName.PlayerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName playerId1 = NJoinFriendButton.PropertyName.PlayerId;
    ulong playerId2 = this.PlayerId;
    Variant variant = Variant.From<ulong>(ref playerId2);
    serializationInfo.AddProperty(playerId1, variant);
    info.AddProperty(NJoinFriendButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NJoinFriendButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NJoinFriendButton.PropertyName.PlayerId, ref variant1))
      this.PlayerId = ((Variant) ref variant1).As<ulong>();
    Variant variant2;
    if (info.TryGetProperty(NJoinFriendButton.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NJoinFriendButton.PropertyName._hsv, ref variant3))
      return;
    this._hsv = ((Variant) ref variant3).As<ShaderMaterial>();
  }

  public new class MethodName : NButton.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName PlayerId = StringName.op_Implicit(nameof (PlayerId));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
