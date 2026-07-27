// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NBgGroundSpikeVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NBgGroundSpikeVfx.cs")]
public class NBgGroundSpikeVfx : Sprite2D
{
  private const string _scenePath = "res://scenes/vfx/bg_ground_spike_vfx.tscn";
  protected Vector2 _startPosition;
  protected bool _movingRight = true;
  protected VfxColor _vfxColor;
  private Vector2 _velocity;
  private Tween? _tween;

  public static NBgGroundSpikeVfx? Create(Vector2 position, bool movingRight = true, VfxColor vfxColor = VfxColor.Red)
  {
    if (TestMode.IsOn)
      return (NBgGroundSpikeVfx) null;
    NBgGroundSpikeVfx nbgGroundSpikeVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/bg_ground_spike_vfx.tscn").Instantiate<NBgGroundSpikeVfx>((PackedScene.GenEditState) 0L);
    nbgGroundSpikeVfx._startPosition = position;
    nbgGroundSpikeVfx._movingRight = movingRight;
    nbgGroundSpikeVfx._vfxColor = vfxColor;
    return nbgGroundSpikeVfx;
  }

  public override void _Ready()
  {
    ((Node2D) this).Skew = this._movingRight ? Rng.Chaotic.NextFloat(15f, 30f) * 0.0174533f : Rng.Chaotic.NextFloat(-30f, -15f) * 0.0174533f;
    float num = Rng.Chaotic.NextFloat(0.5f, 1.5f);
    ((Node2D) this).Scale = Vector2.op_Multiply(new Vector2(Rng.Chaotic.NextFloat(0.8f, 1.2f), Rng.Chaotic.NextFloat(0.8f, 2f)), num);
    this.AdjustStartPosition();
    ((Node2D) this).GlobalPosition = this._startPosition;
    this._velocity = Vector2.op_Division(new Vector2(this._movingRight ? Rng.Chaotic.NextFloat(50f, 250f) : Rng.Chaotic.NextFloat(-250f, -50f), Rng.Chaotic.NextFloat(-5f, 5f)), num);
    this.SetColor();
    TaskHelper.RunSafely(this.Animate());
  }

  private void SetColor()
  {
    switch (this._vfxColor)
    {
      case VfxColor.Red:
        ((CanvasItem) this).Modulate = new Color(1f, Rng.Chaotic.NextFloat(0.2f, 0.8f), Rng.Chaotic.NextFloat(0.0f, 0.2f), 0.5f);
        break;
      case VfxColor.Purple:
        ((CanvasItem) this).Modulate = new Color(Rng.Chaotic.NextFloat(0.0f, 0.2f), Rng.Chaotic.NextFloat(0.2f, 0.8f), 1f, 0.5f);
        break;
      case VfxColor.White:
        float num1 = Rng.Chaotic.NextFloat(0.2f, 0.8f);
        ((CanvasItem) this).Modulate = new Color(num1, num1, num1, 0.5f);
        break;
      case VfxColor.Cyan:
        float num2 = Rng.Chaotic.NextFloat(0.6f, 1f);
        ((CanvasItem) this).Modulate = new Color(0.2f, num2, num2, 0.5f);
        break;
      case VfxColor.Gold:
        float num3 = Rng.Chaotic.NextFloat(0.6f, 1f);
        ((CanvasItem) this).Modulate = new Color(num3, num3, 0.2f, 1f);
        break;
      default:
        Log.Error($"Color: {this._vfxColor.ToString()} not implemented");
        throw new ArgumentOutOfRangeException();
    }
  }

  protected virtual void AdjustStartPosition()
  {
    this._startPosition = Vector2.op_Addition(this._startPosition, new Vector2(this._movingRight ? Rng.Chaotic.NextFloat(40f, 160f) : Rng.Chaotic.NextFloat(-160f, -40f), Rng.Chaotic.NextFloat(-96f, -10f)));
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task Animate()
  {
    float num = Rng.Chaotic.NextFloat(0.25f, 1f);
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenInterval((double) Rng.Chaotic.NextFloat(0.01f, 0.2f));
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("skew"), Variant.op_Implicit(this._movingRight ? Rng.Chaotic.NextFloat(30f, 60f) * 0.0174533f : Rng.Chaotic.NextFloat(-60f, -30f) * 0.0174533f), (double) num).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(((Node2D) this).Scale, Rng.Chaotic.NextFloat(0.1f, 0.5f))), (double) num).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), (double) num).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public override void _Process(double delta)
  {
    ((Node2D) this).Position = Vector2.op_Addition(((Node2D) this).Position, Vector2.op_Multiply(this._velocity, (float) delta));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NBgGroundSpikeVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Sprite2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("movingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vfxColor"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBgGroundSpikeVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgGroundSpikeVfx.MethodName.SetColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgGroundSpikeVfx.MethodName.AdjustStartPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgGroundSpikeVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBgGroundSpikeVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NBgGroundSpikeVfx nbgGroundSpikeVfx = NBgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NBgGroundSpikeVfx>(ref nbgGroundSpikeVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.SetColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetColor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.AdjustStartPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AdjustStartPosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NBgGroundSpikeVfx nbgGroundSpikeVfx = NBgGroundSpikeVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<VfxColor>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NBgGroundSpikeVfx>(ref nbgGroundSpikeVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.Create) || StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName._Ready) || StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.SetColor) || StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName.AdjustStartPosition) || StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NBgGroundSpikeVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._startPosition))
    {
      this._startPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._movingRight))
    {
      this._movingRight = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._vfxColor))
    {
      this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._velocity))
    {
      this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._startPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._movingRight))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._movingRight);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._vfxColor))
    {
      value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._velocity))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._velocity);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBgGroundSpikeVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NBgGroundSpikeVfx.PropertyName._startPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBgGroundSpikeVfx.PropertyName._movingRight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBgGroundSpikeVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBgGroundSpikeVfx.PropertyName._velocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBgGroundSpikeVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBgGroundSpikeVfx.PropertyName._startPosition, Variant.From<Vector2>(ref this._startPosition));
    info.AddProperty(NBgGroundSpikeVfx.PropertyName._movingRight, Variant.From<bool>(ref this._movingRight));
    info.AddProperty(NBgGroundSpikeVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
    info.AddProperty(NBgGroundSpikeVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
    info.AddProperty(NBgGroundSpikeVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._startPosition, ref variant1))
      this._startPosition = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._movingRight, ref variant2))
      this._movingRight = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._vfxColor, ref variant3))
      this._vfxColor = ((Variant) ref variant3).As<VfxColor>();
    Variant variant4;
    if (info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._velocity, ref variant4))
      this._velocity = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (!info.TryGetProperty(NBgGroundSpikeVfx.PropertyName._tween, ref variant5))
      return;
    this._tween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Sprite2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetColor = StringName.op_Implicit(nameof (SetColor));
    public static readonly StringName AdjustStartPosition = StringName.op_Implicit(nameof (AdjustStartPosition));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Sprite2D.PropertyName
  {
    public static readonly StringName _startPosition = StringName.op_Implicit(nameof (_startPosition));
    public static readonly StringName _movingRight = StringName.op_Implicit(nameof (_movingRight));
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
    public static readonly StringName _velocity = StringName.op_Implicit(nameof (_velocity));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Sprite2D.SignalName
  {
  }
}
