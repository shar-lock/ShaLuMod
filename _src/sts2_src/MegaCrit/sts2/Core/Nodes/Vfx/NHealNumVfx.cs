// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NHealNumVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NHealNumVfx.cs")]
public class NHealNumVfx : Node2D
{
  private NCreature _creatureNode;
  private string _text;
  private Tween? _tween;
  private Vector2 _velocity;
  private static readonly float _deceleration = 2000f;
  private static readonly Vector2 _positionOffset = new Vector2(0.0f, -100f);
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_heal_num");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NHealNumVfx._scenePath);
    }
  }

  public static NHealNumVfx? Create(Creature target, Decimal amount)
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
    if (creatureNode == null || !creatureNode.IsInteractable)
      return (NHealNumVfx) null;
    NHealNumVfx nhealNumVfx = PreloadManager.Cache.GetScene(NHealNumVfx._scenePath).Instantiate<NHealNumVfx>((PackedScene.GenEditState) 0L);
    nhealNumVfx._creatureNode = creatureNode;
    nhealNumVfx._text = ((int) amount).ToString();
    return nhealNumVfx;
  }

  public override void _Ready()
  {
    MegaLabel node = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    node.SetTextAutoSize(this._text);
    this.GlobalPosition = Vector2.op_Addition(Vector2.op_Addition(this._creatureNode.VfxSpawnPosition, NHealNumVfx._positionOffset), new Vector2(Rng.Chaotic.NextFloat(-10f, 10f), Rng.Chaotic.NextFloat(-5f, 5f)));
    this._velocity = new Vector2(Rng.Chaotic.NextFloat(-100f, 100f), Rng.Chaotic.NextFloat(-600f, -300f));
    ((Control) node).Scale = Vector2.op_Multiply(Vector2.One, Rng.Chaotic.NextFloat(1.2f, 1.3f));
    this.RotationDegrees = Rng.Chaotic.NextFloat(-5f, 5f);
    TaskHelper.RunSafely(this.AnimVfx());
  }

  private async Task AnimVfx()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.30000001192092896).SetDelay(1.0).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L).From(Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2.5f)));
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public override void _Process(double delta)
  {
    float num = (float) delta;
    this.Position = Vector2.op_Addition(this.Position, Vector2.op_Multiply(this._velocity, num));
    if ((double) ((Vector2) ref this._velocity).LengthSquared() <= 9.9999999747524271E-07)
      return;
    Vector2 velocity = this._velocity;
    Vector2 vector2_1 = Vector2.op_Multiply(Vector2.op_Multiply(((Vector2) ref this._velocity).Normalized(), NHealNumVfx._deceleration), num);
    Vector2 vector2_2 = ((Vector2) ref vector2_1).LimitLength(((Vector2) ref this._velocity).Length());
    this._velocity = Vector2.op_Subtraction(velocity, vector2_2);
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NHealNumVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealNumVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealNumVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHealNumVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealNumVfx.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHealNumVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHealNumVfx.MethodName._Ready) || StringName.op_Equality(ref method, NHealNumVfx.MethodName._Process) || StringName.op_Equality(ref method, NHealNumVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHealNumVfx.PropertyName._creatureNode))
    {
      this._creatureNode = VariantUtils.ConvertTo<NCreature>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealNumVfx.PropertyName._text))
    {
      this._text = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealNumVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHealNumVfx.PropertyName._velocity))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._velocity = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHealNumVfx.PropertyName._creatureNode))
    {
      value = VariantUtils.CreateFrom<NCreature>(ref this._creatureNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealNumVfx.PropertyName._text))
    {
      value = VariantUtils.CreateFrom<string>(ref this._text);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealNumVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHealNumVfx.PropertyName._velocity))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._velocity);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHealNumVfx.PropertyName._creatureNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NHealNumVfx.PropertyName._text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealNumVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHealNumVfx.PropertyName._velocity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHealNumVfx.PropertyName._creatureNode, Variant.From<NCreature>(ref this._creatureNode));
    info.AddProperty(NHealNumVfx.PropertyName._text, Variant.From<string>(ref this._text));
    info.AddProperty(NHealNumVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NHealNumVfx.PropertyName._velocity, Variant.From<Vector2>(ref this._velocity));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHealNumVfx.PropertyName._creatureNode, ref variant1))
      this._creatureNode = ((Variant) ref variant1).As<NCreature>();
    Variant variant2;
    if (info.TryGetProperty(NHealNumVfx.PropertyName._text, ref variant2))
      this._text = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(NHealNumVfx.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (!info.TryGetProperty(NHealNumVfx.PropertyName._velocity, ref variant4))
      return;
    this._velocity = ((Variant) ref variant4).As<Vector2>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _creatureNode = StringName.op_Implicit(nameof (_creatureNode));
    public static readonly StringName _text = StringName.op_Implicit(nameof (_text));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _velocity = StringName.op_Implicit(nameof (_velocity));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
