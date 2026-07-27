// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NBounceSparkVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NBounceSparkVfx.cs")]
public class NBounceSparkVfx : Node2D
{
  private const string _scenePath = "res://scenes/vfx/bounce_spark_vfx.tscn";
  [Export]
  private Node2D _particle;
  private Vector2 _velocity;
  private Vector2 _startPosition;
  private float _floorY;
  private const float _targetAlpha = 0.8f;
  private static readonly Vector2 _gravity = new Vector2(0.0f, 1500f);
  private Tween? _tween;

  public static NBounceSparkVfx? Create(Creature target, VfxColor vfxColor = VfxColor.Gold)
  {
    if (TestMode.IsOn)
      return (NBounceSparkVfx) null;
    NBounceSparkVfx nbounceSparkVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/bounce_spark_vfx.tscn").Instantiate<NBounceSparkVfx>((PackedScene.GenEditState) 0L);
    nbounceSparkVfx._startPosition = NCombatRoom.Instance.GetCreatureNode(target).GetBottomOfHitbox();
    return nbounceSparkVfx;
  }

  public override void _Ready()
  {
    this._startPosition = Vector2.op_Addition(this._startPosition, new Vector2(Rng.Chaotic.NextFloat(-120f, 120f), Rng.Chaotic.NextFloat(0.0f, 20f)));
    this._floorY = this._startPosition.Y + Rng.Chaotic.NextFloat(0.0f, 64f);
    this.GlobalPosition = this._startPosition;
    this._velocity = new Vector2(Rng.Chaotic.NextFloat(-400f, 400f), Rng.Chaotic.NextFloat(-800f, -300f));
    float num = Rng.Chaotic.NextFloat(0.8f, 1.2f);
    this.Scale = Vector2.op_Multiply(new Vector2(num, 2f - num), Rng.Chaotic.NextFloat(0.1f, 0.8f));
    ((CanvasItem) this).Modulate = new Color(1f, Rng.Chaotic.NextFloat(0.2f, 0.8f), Rng.Chaotic.NextFloat(0.0f, 0.2f), 0.0f);
    TaskHelper.RunSafely(this.Animate());
  }

  public override void _Process(double delta)
  {
    float num = (float) delta;
    this.Position = Vector2.op_Addition(this.Position, Vector2.op_Multiply(this._velocity, num));
    this._velocity = Vector2.op_Addition(this._velocity, Vector2.op_Multiply(NBounceSparkVfx._gravity, num));
    this.Rotation = MathHelper.GetAngle(this._velocity);
    if ((double) this.Position.Y <= (double) this._floorY)
      return;
    this.Position = new Vector2(this.Position.X, this._floorY);
    this._velocity = new Vector2(this._velocity.X, (float) (-(double) this._velocity.Y * 0.5));
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task Animate()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.8f), 0.10000000149011612);
    this._tween.Chain();
    float num = Rng.Chaotic.NextFloat(0.4f, 1.5f);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), (double) num);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this.Scale, 0.5f)), (double) num);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBounceSparkVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBounceSparkVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBounceSparkVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBounceSparkVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBounceSparkVfx.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBounceSparkVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBounceSparkVfx.MethodName._Ready) || StringName.op_Equality(ref method, NBounceSparkVfx.MethodName._Process) || StringName.op_Equality(ref method, NBounceSparkVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._particle))
    {
      this._particle = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._velocity))
    {
      this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._startPosition))
    {
      this._startPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._floorY))
    {
      this._floorY = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._particle))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._particle);
      return true;
    }
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._velocity))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._velocity);
      return true;
    }
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._startPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._floorY))
    {
      value = VariantUtils.CreateFrom<float>(ref this._floorY);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBounceSparkVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBounceSparkVfx.PropertyName._particle, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NBounceSparkVfx.PropertyName._velocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBounceSparkVfx.PropertyName._startPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NBounceSparkVfx.PropertyName._floorY, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBounceSparkVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBounceSparkVfx.PropertyName._particle, Variant.From<Node2D>(ref this._particle));
    info.AddProperty(NBounceSparkVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
    info.AddProperty(NBounceSparkVfx.PropertyName._startPosition, Variant.From<Vector2>(ref this._startPosition));
    info.AddProperty(NBounceSparkVfx.PropertyName._floorY, Variant.From<float>(ref this._floorY));
    info.AddProperty(NBounceSparkVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBounceSparkVfx.PropertyName._particle, ref variant1))
      this._particle = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NBounceSparkVfx.PropertyName._velocity, ref variant2))
      this._velocity = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NBounceSparkVfx.PropertyName._startPosition, ref variant3))
      this._startPosition = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NBounceSparkVfx.PropertyName._floorY, ref variant4))
      this._floorY = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (!info.TryGetProperty(NBounceSparkVfx.PropertyName._tween, ref variant5))
      return;
    this._tween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _particle = StringName.op_Implicit(nameof (_particle));
    public static readonly StringName _velocity = StringName.op_Implicit(nameof (_velocity));
    public static readonly StringName _startPosition = StringName.op_Implicit(nameof (_startPosition));
    public static readonly StringName _floorY = StringName.op_Implicit(nameof (_floorY));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
