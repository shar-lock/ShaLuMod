// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NHellraiserSwordVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NHellraiserSwordVfx.cs")]
public class NHellraiserSwordVfx : Control
{
  private static readonly StringName _swordStr = new StringName("Sword");
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/vfx_hellraiser/hellraiser_sword_vfx");
  private TextureRect _sword;
  public float posY;
  public Color targetColor;

  public static NHellraiserSwordVfx? Create()
  {
    return TestMode.IsOn ? (NHellraiserSwordVfx) null : PreloadManager.Cache.GetScene(NHellraiserSwordVfx._scenePath).Instantiate<NHellraiserSwordVfx>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    ((Node) this).Name = NHellraiserSwordVfx._swordStr;
    this._sword = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._sword.FlipH = Rng.Chaotic.NextBool();
    ((Control) this._sword).RotationDegrees = Rng.Chaotic.NextFloat(-20f, 20f);
    ((Control) this._sword).Position = new Vector2(25f, 200f);
    this.Scale = Vector2.op_Multiply(new Vector2(Rng.Chaotic.NextFloat(0.7f, 0.9f), Rng.Chaotic.NextFloat(0.8f, 1.2f)), Rng.Chaotic.NextFloat(1f, 2f));
    this.Position = Vector2.op_Addition(this.Position, new Vector2(Rng.Chaotic.NextGaussianFloat(min: -500f, max: 500f), this.posY));
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    tween.TweenInterval(Rng.Chaotic.NextDouble() * 0.8);
    tween.Chain();
    tween.TweenProperty((GodotObject) this._sword, NodePath.op_Implicit("position:y"), Variant.op_Implicit(80f), 0.25).From(Variant.op_Implicit(300f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this.targetColor), 0.25).From(Variant.op_Implicit(Colors.Red));
    tween.Chain().TweenInterval(0.25);
    tween.Chain();
    tween.TweenProperty((GodotObject) this._sword, NodePath.op_Implicit("position:y"), Variant.op_Implicit(200f), 0.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    tween.Chain().TweenCallback(Callable.From(new Action(this.OnTweenFinished)));
  }

  private void OnTweenFinished() => ((Node) this).QueueFreeSafely();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NHellraiserSwordVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHellraiserSwordVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHellraiserSwordVfx.MethodName.OnTweenFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NHellraiserSwordVfx nhellraiserSwordVfx = NHellraiserSwordVfx.Create();
      ret = VariantUtils.CreateFrom<NHellraiserSwordVfx>(ref nhellraiserSwordVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName.OnTweenFinished) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnTweenFinished();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NHellraiserSwordVfx nhellraiserSwordVfx = NHellraiserSwordVfx.Create();
      ret = VariantUtils.CreateFrom<NHellraiserSwordVfx>(ref nhellraiserSwordVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName.Create) || StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName._Ready) || StringName.op_Equality(ref method, NHellraiserSwordVfx.MethodName.OnTweenFinished) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHellraiserSwordVfx.PropertyName._sword))
    {
      this._sword = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHellraiserSwordVfx.PropertyName.posY))
    {
      this.posY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHellraiserSwordVfx.PropertyName.targetColor))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.targetColor = VariantUtils.ConvertTo<Color>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHellraiserSwordVfx.PropertyName._sword))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._sword);
      return true;
    }
    if (StringName.op_Equality(ref name, NHellraiserSwordVfx.PropertyName.posY))
    {
      value = VariantUtils.CreateFrom<float>(ref this.posY);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHellraiserSwordVfx.PropertyName.targetColor))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Color>(ref this.targetColor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHellraiserSwordVfx.PropertyName._sword, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHellraiserSwordVfx.PropertyName.posY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NHellraiserSwordVfx.PropertyName.targetColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHellraiserSwordVfx.PropertyName._sword, Variant.From<TextureRect>(ref this._sword));
    info.AddProperty(NHellraiserSwordVfx.PropertyName.posY, Variant.From<float>(ref this.posY));
    info.AddProperty(NHellraiserSwordVfx.PropertyName.targetColor, Variant.From<Color>(ref this.targetColor));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHellraiserSwordVfx.PropertyName._sword, ref variant1))
      this._sword = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NHellraiserSwordVfx.PropertyName.posY, ref variant2))
      this.posY = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (!info.TryGetProperty(NHellraiserSwordVfx.PropertyName.targetColor, ref variant3))
      return;
    this.targetColor = ((Variant) ref variant3).As<Color>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnTweenFinished = StringName.op_Implicit(nameof (OnTweenFinished));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _sword = StringName.op_Implicit(nameof (_sword));
    public static readonly StringName posY = StringName.op_Implicit(nameof (posY));
    public static readonly StringName targetColor = StringName.op_Implicit(nameof (targetColor));
  }

  public class SignalName : Control.SignalName
  {
  }
}
