// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NStabVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NStabVfx.cs")]
public class NStabVfx : Node2D
{
  private const string _scenePath = "res://scenes/vfx/stab_vfx.tscn";
  private Node2D _primaryVfx;
  private Node2D _secondaryVfx;
  private Vector2 _creatureCenter;
  private VfxColor _vfxColor;
  private bool _facingEnemies;
  private Tween? _tween;

  public static NStabVfx? Create(Creature? target, bool facingEnemies = false, VfxColor vfxColor = VfxColor.Red)
  {
    if (TestMode.IsOn)
      return (NStabVfx) null;
    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
    if (creatureNode == null)
      return (NStabVfx) null;
    Vector2 vfxSpawnPosition = creatureNode.VfxSpawnPosition;
    NStabVfx nstabVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/stab_vfx.tscn").Instantiate<NStabVfx>((PackedScene.GenEditState) 0L);
    nstabVfx._vfxColor = vfxColor;
    nstabVfx._facingEnemies = facingEnemies;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(facingEnemies ? Rng.Chaotic.NextFloat(0.0f, 48f) : Rng.Chaotic.NextFloat(-48f, 0.0f), Rng.Chaotic.NextFloat(-50f, 50f));
    nstabVfx._creatureCenter = Vector2.op_Addition(vfxSpawnPosition, vector2);
    return nstabVfx;
  }

  public override void _Ready()
  {
    this._primaryVfx = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("Primary"));
    this._secondaryVfx = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Secondary"));
    this._primaryVfx.GlobalPosition = this.GenerateSpawnPosition();
    this._primaryVfx.Rotation = MathHelper.GetAngle(Vector2.op_Subtraction(this._primaryVfx.GlobalPosition, this._creatureCenter)) + 1.57079637f;
    this.SetColor();
    TaskHelper.RunSafely(this.Animate());
  }

  private void SetColor()
  {
    switch (this._vfxColor)
    {
      case VfxColor.Green:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("00A52F");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("FFCB2D");
        break;
      case VfxColor.Blue:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("007BDD");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("00EFF6");
        break;
      case VfxColor.Purple:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("A803FF");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("00EFF3");
        break;
      case VfxColor.Black:
        break;
      case VfxColor.White:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("808080");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("FFFFFF");
        break;
      case VfxColor.Cyan:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("009599");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("5CDCFF");
        break;
      case VfxColor.Gold:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("EBA800");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("FFE39C");
        break;
      default:
        ((CanvasItem) this._primaryVfx).SelfModulate = new Color("FF0000");
        ((CanvasItem) this._secondaryVfx).SelfModulate = new Color("FFCB2D");
        break;
    }
  }

  private Vector2 GenerateSpawnPosition()
  {
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(Rng.Chaotic.NextFloat(-12f, 12f), Rng.Chaotic.NextFloat(-64f, 64f));
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(this._facingEnemies ? -200f : 200f, 0.0f);
    return Vector2.op_Addition(Vector2.op_Addition(this._creatureCenter, vector2_1), vector2_2);
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task Animate()
  {
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
    this._tween.TweenProperty((GodotObject) this._primaryVfx, NodePath.op_Implicit("position"), Variant.op_Implicit(this._creatureCenter), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 6L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25).SetDelay(0.25);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NStabVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStabVfx.MethodName.SetColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStabVfx.MethodName.GenerateSpawnPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStabVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStabVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStabVfx.MethodName.SetColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetColor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStabVfx.MethodName.GenerateSpawnPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 spawnPosition = this.GenerateSpawnPosition();
      ret = VariantUtils.CreateFrom<Vector2>(ref spawnPosition);
      return true;
    }
    if (!StringName.op_Equality(ref method, NStabVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStabVfx.MethodName._Ready) || StringName.op_Equality(ref method, NStabVfx.MethodName.SetColor) || StringName.op_Equality(ref method, NStabVfx.MethodName.GenerateSpawnPosition) || StringName.op_Equality(ref method, NStabVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._primaryVfx))
    {
      this._primaryVfx = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._secondaryVfx))
    {
      this._secondaryVfx = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._creatureCenter))
    {
      this._creatureCenter = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._vfxColor))
    {
      this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._facingEnemies))
    {
      this._facingEnemies = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStabVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._primaryVfx))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._primaryVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._secondaryVfx))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._secondaryVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._creatureCenter))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._creatureCenter);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._vfxColor))
    {
      value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NStabVfx.PropertyName._facingEnemies))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._facingEnemies);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStabVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NStabVfx.PropertyName._primaryVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStabVfx.PropertyName._secondaryVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NStabVfx.PropertyName._creatureCenter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NStabVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NStabVfx.PropertyName._facingEnemies, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStabVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NStabVfx.PropertyName._primaryVfx, Variant.From<Node2D>(ref this._primaryVfx));
    info.AddProperty(NStabVfx.PropertyName._secondaryVfx, Variant.From<Node2D>(ref this._secondaryVfx));
    info.AddProperty(NStabVfx.PropertyName._creatureCenter, Variant.From<Vector2>(ref this._creatureCenter));
    info.AddProperty(NStabVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
    info.AddProperty(NStabVfx.PropertyName._facingEnemies, Variant.From<bool>(ref this._facingEnemies));
    info.AddProperty(NStabVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NStabVfx.PropertyName._primaryVfx, ref variant1))
      this._primaryVfx = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NStabVfx.PropertyName._secondaryVfx, ref variant2))
      this._secondaryVfx = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NStabVfx.PropertyName._creatureCenter, ref variant3))
      this._creatureCenter = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NStabVfx.PropertyName._vfxColor, ref variant4))
      this._vfxColor = ((Variant) ref variant4).As<VfxColor>();
    Variant variant5;
    if (info.TryGetProperty(NStabVfx.PropertyName._facingEnemies, ref variant5))
      this._facingEnemies = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (!info.TryGetProperty(NStabVfx.PropertyName._tween, ref variant6))
      return;
    this._tween = ((Variant) ref variant6).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetColor = StringName.op_Implicit(nameof (SetColor));
    public static readonly StringName GenerateSpawnPosition = StringName.op_Implicit(nameof (GenerateSpawnPosition));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _primaryVfx = StringName.op_Implicit(nameof (_primaryVfx));
    public static readonly StringName _secondaryVfx = StringName.op_Implicit(nameof (_secondaryVfx));
    public static readonly StringName _creatureCenter = StringName.op_Implicit(nameof (_creatureCenter));
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
    public static readonly StringName _facingEnemies = StringName.op_Implicit(nameof (_facingEnemies));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
