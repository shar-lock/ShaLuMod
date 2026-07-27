// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NGroundFireVfx
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
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NGroundFireVfx.cs")]
public class NGroundFireVfx : Node2D
{
  private static readonly StringName _outerColor = new StringName("OuterColor");
  private static readonly StringName _innerColor = new StringName("InnerColor");
  private Tween? _tween;
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/fires/vfx_ground_fire");
  private Node2D _mainFire;
  private GpuParticles2D _ember;
  private GpuParticles2D _flameSprites;
  private VfxColor _vfxColor;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NGroundFireVfx._scenePath);
    }
  }

  public static NGroundFireVfx? Create(Creature target, VfxColor color = VfxColor.Red)
  {
    if (TestMode.IsOn)
      return (NGroundFireVfx) null;
    NCreature creatureNode = NCombatRoom.Instance.GetCreatureNode(target);
    if (creatureNode == null)
      return (NGroundFireVfx) null;
    NGroundFireVfx ngroundFireVfx = PreloadManager.Cache.GetScene(NGroundFireVfx._scenePath).Instantiate<NGroundFireVfx>((PackedScene.GenEditState) 0L);
    ngroundFireVfx._vfxColor = color;
    ngroundFireVfx.GlobalPosition = creatureNode.GetBottomOfHitbox();
    return ngroundFireVfx;
  }

  public override void _Ready()
  {
    this._mainFire = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("MainFire"));
    this._ember = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("Ember"));
    this._flameSprites = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameSprites"));
    this.ApplyColor();
    TaskHelper.RunSafely(this.AnimateIn());
  }

  private void ApplyColor()
  {
    if (this._vfxColor == VfxColor.Red)
      return;
    Color white1 = Colors.White;
    Color white2 = Colors.White;
    Color black;
    // ISSUE: explicit constructor call
    ((Color) ref black).\u002Ector("541b00");
    switch (this._vfxColor)
    {
      case VfxColor.Green:
        // ISSUE: explicit constructor call
        ((Color) ref white1).\u002Ector("2fa800");
        // ISSUE: explicit constructor call
        ((Color) ref white2).\u002Ector("06a000");
        // ISSUE: explicit constructor call
        ((Color) ref black).\u002Ector("541b00");
        goto case VfxColor.Black;
      case VfxColor.Blue:
        // ISSUE: explicit constructor call
        ((Color) ref white1).\u002Ector("0099cd");
        // ISSUE: explicit constructor call
        ((Color) ref white2).\u002Ector("00a3bf");
        black = Colors.Black;
        goto case VfxColor.Black;
      case VfxColor.Purple:
        // ISSUE: explicit constructor call
        ((Color) ref white1).\u002Ector("7821ff");
        // ISSUE: explicit constructor call
        ((Color) ref white2).\u002Ector("3f21ff");
        // ISSUE: explicit constructor call
        ((Color) ref black).\u002Ector("541b00");
        goto case VfxColor.Black;
      case VfxColor.Black:
      case VfxColor.White:
        Node node = ((Node) this._mainFire).GetNode(NodePath.op_Implicit("VfxAdditiveStepFire"));
        ShaderMaterial material = (ShaderMaterial) ((CanvasItem) node.GetNode<Node2D>(NodePath.op_Implicit("SteppedFireMix"))).Material;
        material.SetShaderParameter(NGroundFireVfx._outerColor, Variant.op_Implicit(white1));
        material.SetShaderParameter(NGroundFireVfx._innerColor, Variant.op_Implicit(white2));
        ((ShaderMaterial) ((CanvasItem) node.GetNode<Node2D>(NodePath.op_Implicit("SteppedFireAdd"))).Material).SetShaderParameter(NGroundFireVfx._outerColor, Variant.op_Implicit(black));
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public override void _ExitTree() => this._tween?.Kill();

  private async Task AnimateIn()
  {
    ((CanvasItem) this._mainFire).Modulate = Colors.Transparent;
    this._mainFire.Scale = Vector2.Zero;
    this._ember.Emitting = true;
    Task emberDone = ((GodotObject) this._ember).AwaitSignal(GpuParticles2D.SignalName.Finished, (Node) this);
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._mainFire, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 4f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._mainFire, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (!await this._tween.AwaitFinished((Node) this))
    {
      emberDone = (Task) null;
    }
    else
    {
      this._flameSprites.Emitting = true;
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._tween.TweenProperty((GodotObject) this._mainFire, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
      this._tween.TweenProperty((GodotObject) this._flameSprites, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
      this._tween.TweenProperty((GodotObject) this._mainFire, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 2f)), 2.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
      if (!await this._tween.AwaitFinished((Node) this))
      {
        emberDone = (Task) null;
      }
      else
      {
        this._flameSprites.Emitting = false;
        await emberDone;
        ((Node) this).QueueFreeSafely();
        emberDone = (Task) null;
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NGroundFireVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGroundFireVfx.MethodName.ApplyColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGroundFireVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGroundFireVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGroundFireVfx.MethodName.ApplyColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ApplyColor();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGroundFireVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGroundFireVfx.MethodName._Ready) || StringName.op_Equality(ref method, NGroundFireVfx.MethodName.ApplyColor) || StringName.op_Equality(ref method, NGroundFireVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._mainFire))
    {
      this._mainFire = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._ember))
    {
      this._ember = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._flameSprites))
    {
      this._flameSprites = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._vfxColor))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._vfxColor = VariantUtils.ConvertTo<VfxColor>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._mainFire))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._mainFire);
      return true;
    }
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._ember))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ember);
      return true;
    }
    if (StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._flameSprites))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameSprites);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGroundFireVfx.PropertyName._vfxColor))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<VfxColor>(ref this._vfxColor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGroundFireVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGroundFireVfx.PropertyName._mainFire, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGroundFireVfx.PropertyName._ember, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGroundFireVfx.PropertyName._flameSprites, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NGroundFireVfx.PropertyName._vfxColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGroundFireVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NGroundFireVfx.PropertyName._mainFire, Variant.From<Node2D>(ref this._mainFire));
    info.AddProperty(NGroundFireVfx.PropertyName._ember, Variant.From<GpuParticles2D>(ref this._ember));
    info.AddProperty(NGroundFireVfx.PropertyName._flameSprites, Variant.From<GpuParticles2D>(ref this._flameSprites));
    info.AddProperty(NGroundFireVfx.PropertyName._vfxColor, Variant.From<VfxColor>(ref this._vfxColor));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGroundFireVfx.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NGroundFireVfx.PropertyName._mainFire, ref variant2))
      this._mainFire = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NGroundFireVfx.PropertyName._ember, ref variant3))
      this._ember = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NGroundFireVfx.PropertyName._flameSprites, ref variant4))
      this._flameSprites = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (!info.TryGetProperty(NGroundFireVfx.PropertyName._vfxColor, ref variant5))
      return;
    this._vfxColor = ((Variant) ref variant5).As<VfxColor>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ApplyColor = StringName.op_Implicit(nameof (ApplyColor));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _mainFire = StringName.op_Implicit(nameof (_mainFire));
    public static readonly StringName _ember = StringName.op_Implicit(nameof (_ember));
    public static readonly StringName _flameSprites = StringName.op_Implicit(nameof (_flameSprites));
    public static readonly StringName _vfxColor = StringName.op_Implicit(nameof (_vfxColor));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
