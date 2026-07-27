// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NItemThrowVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NItemThrowVfx.cs")]
public class NItemThrowVfx : Node2D
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_item_throw");
  private const float _baseItemSize = 80f;
  [Export]
  private Sprite2D? _itemSprite;
  [Export]
  private float _flightTime;
  [Export]
  private float _heightMultiplier;
  [Export]
  private Curve? _horizontalCurve;
  [Export]
  private Curve? _verticalCurve;
  [Export]
  private float _rotationMultiplier;
  [Export]
  private Curve? _rotationInfluenceCurve;
  private Vector2 _sourcePosition;
  private Vector2 _targetPosition;

  public static NItemThrowVfx? Create(
    Vector2 sourcePosition,
    Vector2 targetPosition,
    Texture2D? itemTexture,
    Vector2? scale = null)
  {
    if (TestMode.IsOn)
      return (NItemThrowVfx) null;
    NItemThrowVfx nitemThrowVfx = PreloadManager.Cache.GetScene(NItemThrowVfx.scenePath).Instantiate<NItemThrowVfx>((PackedScene.GenEditState) 0L);
    nitemThrowVfx._sourcePosition = sourcePosition;
    nitemThrowVfx._targetPosition = targetPosition;
    if (nitemThrowVfx._itemSprite != null)
    {
      ((CanvasItem) nitemThrowVfx._itemSprite).Visible = false;
      ((Node2D) nitemThrowVfx._itemSprite).Scale = scale ?? Vector2.One;
      if (itemTexture != null)
      {
        nitemThrowVfx._itemSprite.Texture = itemTexture;
        Sprite2D itemSprite = nitemThrowVfx._itemSprite;
        ((Node2D) itemSprite).Scale = Vector2.op_Multiply(((Node2D) itemSprite).Scale, 80f / (float) itemTexture.GetWidth());
      }
    }
    return nitemThrowVfx;
  }

  public override void _Ready() => TaskHelper.RunSafely(this.ThrowItem());

  private async Task ThrowItem()
  {
    ((CanvasItem) this._itemSprite).Visible = true;
    ((Node2D) this._itemSprite).GlobalPosition = this._sourcePosition;
    ((Node2D) this._itemSprite).RotationDegrees = Rng.Chaotic.NextFloat(360f);
    double timer = 0.0;
    while (timer < (double) this._flightTime)
    {
      double processDeltaTime = ((Node) this).GetProcessDeltaTime();
      float num1 = (float) timer / this._flightTime;
      float num2 = this._horizontalCurve.Sample(num1);
      float num3 = this._verticalCurve.Sample(num1);
      ((Node2D) this._itemSprite).Rotate((float) Mathf.DegToRad((double) this._rotationInfluenceCurve.Sample(num1) * (double) this._rotationMultiplier * processDeltaTime));
      ((Node2D) this._itemSprite).GlobalPosition = Vector2.op_Addition(((Vector2) ref this._sourcePosition).Lerp(this._targetPosition, num2), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Up, num3), this._heightMultiplier));
      timer += processDeltaTime;
      double num4 = (double) await ((Node) this).AwaitProcessFrame();
    }
    ((CanvasItem) this._itemSprite).Visible = false;
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NItemThrowVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NItemThrowVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NItemThrowVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._itemSprite))
    {
      this._itemSprite = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._flightTime))
    {
      this._flightTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._heightMultiplier))
    {
      this._heightMultiplier = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._horizontalCurve))
    {
      this._horizontalCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._verticalCurve))
    {
      this._verticalCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._rotationMultiplier))
    {
      this._rotationMultiplier = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._rotationInfluenceCurve))
    {
      this._rotationInfluenceCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._sourcePosition))
    {
      this._sourcePosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._targetPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._targetPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._itemSprite))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._itemSprite);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._flightTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._flightTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._heightMultiplier))
    {
      value = VariantUtils.CreateFrom<float>(ref this._heightMultiplier);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._horizontalCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._horizontalCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._verticalCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._verticalCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._rotationMultiplier))
    {
      value = VariantUtils.CreateFrom<float>(ref this._rotationMultiplier);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._rotationInfluenceCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._rotationInfluenceCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._sourcePosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._sourcePosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NItemThrowVfx.PropertyName._targetPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._targetPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NItemThrowVfx.PropertyName._itemSprite, (PropertyHint) 34L, "Sprite2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NItemThrowVfx.PropertyName._flightTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NItemThrowVfx.PropertyName._heightMultiplier, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NItemThrowVfx.PropertyName._horizontalCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NItemThrowVfx.PropertyName._verticalCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NItemThrowVfx.PropertyName._rotationMultiplier, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NItemThrowVfx.PropertyName._rotationInfluenceCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NItemThrowVfx.PropertyName._sourcePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NItemThrowVfx.PropertyName._targetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NItemThrowVfx.PropertyName._itemSprite, Variant.From<Sprite2D>(ref this._itemSprite));
    info.AddProperty(NItemThrowVfx.PropertyName._flightTime, Variant.From<float>(ref this._flightTime));
    info.AddProperty(NItemThrowVfx.PropertyName._heightMultiplier, Variant.From<float>(ref this._heightMultiplier));
    info.AddProperty(NItemThrowVfx.PropertyName._horizontalCurve, Variant.From<Curve>(ref this._horizontalCurve));
    info.AddProperty(NItemThrowVfx.PropertyName._verticalCurve, Variant.From<Curve>(ref this._verticalCurve));
    info.AddProperty(NItemThrowVfx.PropertyName._rotationMultiplier, Variant.From<float>(ref this._rotationMultiplier));
    info.AddProperty(NItemThrowVfx.PropertyName._rotationInfluenceCurve, Variant.From<Curve>(ref this._rotationInfluenceCurve));
    info.AddProperty(NItemThrowVfx.PropertyName._sourcePosition, Variant.From<Vector2>(ref this._sourcePosition));
    info.AddProperty(NItemThrowVfx.PropertyName._targetPosition, Variant.From<Vector2>(ref this._targetPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._itemSprite, ref variant1))
      this._itemSprite = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._flightTime, ref variant2))
      this._flightTime = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._heightMultiplier, ref variant3))
      this._heightMultiplier = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._horizontalCurve, ref variant4))
      this._horizontalCurve = ((Variant) ref variant4).As<Curve>();
    Variant variant5;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._verticalCurve, ref variant5))
      this._verticalCurve = ((Variant) ref variant5).As<Curve>();
    Variant variant6;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._rotationMultiplier, ref variant6))
      this._rotationMultiplier = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._rotationInfluenceCurve, ref variant7))
      this._rotationInfluenceCurve = ((Variant) ref variant7).As<Curve>();
    Variant variant8;
    if (info.TryGetProperty(NItemThrowVfx.PropertyName._sourcePosition, ref variant8))
      this._sourcePosition = ((Variant) ref variant8).As<Vector2>();
    Variant variant9;
    if (!info.TryGetProperty(NItemThrowVfx.PropertyName._targetPosition, ref variant9))
      return;
    this._targetPosition = ((Variant) ref variant9).As<Vector2>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _itemSprite = StringName.op_Implicit(nameof (_itemSprite));
    public static readonly StringName _flightTime = StringName.op_Implicit(nameof (_flightTime));
    public static readonly StringName _heightMultiplier = StringName.op_Implicit(nameof (_heightMultiplier));
    public static readonly StringName _horizontalCurve = StringName.op_Implicit(nameof (_horizontalCurve));
    public static readonly StringName _verticalCurve = StringName.op_Implicit(nameof (_verticalCurve));
    public static readonly StringName _rotationMultiplier = StringName.op_Implicit(nameof (_rotationMultiplier));
    public static readonly StringName _rotationInfluenceCurve = StringName.op_Implicit(nameof (_rotationInfluenceCurve));
    public static readonly StringName _sourcePosition = StringName.op_Implicit(nameof (_sourcePosition));
    public static readonly StringName _targetPosition = StringName.op_Implicit(nameof (_targetPosition));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
