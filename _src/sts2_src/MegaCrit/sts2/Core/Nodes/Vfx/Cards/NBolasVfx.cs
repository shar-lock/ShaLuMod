// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NBolasVfx
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NBolasVfx.cs")]
public class NBolasVfx : Node2D
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/cards/bolas_vfx");
  private Node2D _bola2;
  private Node2D _bola3;
  private Vector2 _startPosition;
  private Vector2 _controlPosition;
  private Vector2 _endPosition;
  private float _rotationSpeed = 30f;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NBolasVfx._scenePath);
    }
  }

  public static NBolasVfx? Create(Creature owner, Creature target)
  {
    if (TestMode.IsOn)
      return (NBolasVfx) null;
    if (target.IsDead)
      return (NBolasVfx) null;
    NBolasVfx nbolasVfx = PreloadManager.Cache.GetScene(NBolasVfx._scenePath).Instantiate<NBolasVfx>((PackedScene.GenEditState) 0L);
    nbolasVfx.GlobalPosition = NCombatRoom.Instance.GetCreatureNode(owner).VfxSpawnPosition;
    nbolasVfx._startPosition = nbolasVfx.GlobalPosition;
    nbolasVfx._endPosition = NCombatRoom.Instance.GetCreatureNode(target).VfxSpawnPosition;
    float num = Mathf.Min(nbolasVfx._startPosition.Y, nbolasVfx._endPosition.Y) - Rng.Chaotic.NextFloat(400f, 500f);
    nbolasVfx._controlPosition = new Vector2((float) (((double) nbolasVfx._startPosition.X + (double) nbolasVfx._endPosition.X) * 0.5), num);
    return nbolasVfx;
  }

  public override void _Ready()
  {
    TaskHelper.RunSafely(this.FlyBolasFly());
    this._bola2 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("Bola2"));
    this._bola3 = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("Bola3"));
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task FlyBolasFly()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).From(Variant.op_Implicit(0.0f));
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.FollowCurve)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 0.60000002384185791).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._tween.Chain();
    this._tween.TweenInterval(0.15000000596046448);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.15000000596046448);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  private void FollowCurve(float progressPercent)
  {
    this.GlobalPosition = MathHelper.BezierCurve(this._startPosition, this._endPosition, this._controlPosition, progressPercent);
  }

  public override void _Process(double delta)
  {
    float num = (float) delta;
    this.Rotation += num * -this._rotationSpeed;
    this._rotationSpeed -= num * 12f;
    Node2D bola2 = this._bola2;
    bola2.Position = Vector2.op_Subtraction(bola2.Position, new Vector2(150f * num, 0.0f));
    Node2D bola3 = this._bola3;
    bola3.Position = Vector2.op_Subtraction(bola3.Position, new Vector2(-150f * num, 0.0f));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NBolasVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBolasVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBolasVfx.MethodName.FollowCurve, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("progressPercent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBolasVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NBolasVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBolasVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBolasVfx.MethodName.FollowCurve) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.FollowCurve(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBolasVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBolasVfx.MethodName._Ready) || StringName.op_Equality(ref method, NBolasVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NBolasVfx.MethodName.FollowCurve) || StringName.op_Equality(ref method, NBolasVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._bola2))
    {
      this._bola2 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._bola3))
    {
      this._bola3 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._startPosition))
    {
      this._startPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._controlPosition))
    {
      this._controlPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._endPosition))
    {
      this._endPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._rotationSpeed))
    {
      this._rotationSpeed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBolasVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._bola2))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._bola2);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._bola3))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._bola3);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._startPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._controlPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._controlPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._endPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._endPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NBolasVfx.PropertyName._rotationSpeed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._rotationSpeed);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBolasVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBolasVfx.PropertyName._bola2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBolasVfx.PropertyName._bola3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBolasVfx.PropertyName._startPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBolasVfx.PropertyName._controlPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBolasVfx.PropertyName._endPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NBolasVfx.PropertyName._rotationSpeed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBolasVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NBolasVfx.PropertyName._bola2, Variant.From<Node2D>(ref this._bola2));
    info.AddProperty(NBolasVfx.PropertyName._bola3, Variant.From<Node2D>(ref this._bola3));
    info.AddProperty(NBolasVfx.PropertyName._startPosition, Variant.From<Vector2>(ref this._startPosition));
    info.AddProperty(NBolasVfx.PropertyName._controlPosition, Variant.From<Vector2>(ref this._controlPosition));
    info.AddProperty(NBolasVfx.PropertyName._endPosition, Variant.From<Vector2>(ref this._endPosition));
    info.AddProperty(NBolasVfx.PropertyName._rotationSpeed, Variant.From<float>(ref this._rotationSpeed));
    info.AddProperty(NBolasVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBolasVfx.PropertyName._bola2, ref variant1))
      this._bola2 = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NBolasVfx.PropertyName._bola3, ref variant2))
      this._bola3 = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NBolasVfx.PropertyName._startPosition, ref variant3))
      this._startPosition = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NBolasVfx.PropertyName._controlPosition, ref variant4))
      this._controlPosition = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NBolasVfx.PropertyName._endPosition, ref variant5))
      this._endPosition = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NBolasVfx.PropertyName._rotationSpeed, ref variant6))
      this._rotationSpeed = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (!info.TryGetProperty(NBolasVfx.PropertyName._tween, ref variant7))
      return;
    this._tween = ((Variant) ref variant7).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName FollowCurve = StringName.op_Implicit(nameof (FollowCurve));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _bola2 = StringName.op_Implicit(nameof (_bola2));
    public static readonly StringName _bola3 = StringName.op_Implicit(nameof (_bola3));
    public static readonly StringName _startPosition = StringName.op_Implicit(nameof (_startPosition));
    public static readonly StringName _controlPosition = StringName.op_Implicit(nameof (_controlPosition));
    public static readonly StringName _endPosition = StringName.op_Implicit(nameof (_endPosition));
    public static readonly StringName _rotationSpeed = StringName.op_Implicit(nameof (_rotationSpeed));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
