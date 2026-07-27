// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardTrailVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardTrailVfx.cs")]
public class NCardTrailVfx : Node2D
{
  private Control _nodeToFollow;
  private Node2D _sprites;
  private bool _updateSprites = true;
  private Tween? _tween;

  public static NCardTrailVfx? Create(Control card, string characterTrailPath)
  {
    if (TestMode.IsOn)
      return (NCardTrailVfx) null;
    NCardTrailVfx ncardTrailVfx = PreloadManager.Cache.GetScene(characterTrailPath).Instantiate<NCardTrailVfx>((PackedScene.GenEditState) 0L);
    ncardTrailVfx._nodeToFollow = card;
    return ncardTrailVfx;
  }

  public override void _Ready()
  {
    if (NCombatUi.IsDebugHidingPlayContainer)
      ((CanvasItem) this).Visible = false;
    this._sprites = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("Sprites"));
    ((CanvasItem) this._sprites).Modulate = StsColors.transparentWhite;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._sprites, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.5f)), 0.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 7L).SetDelay(0.25);
    this._tween.TweenProperty((GodotObject) this._nodeToFollow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.75f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._sprites, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public override void _Process(double delta)
  {
    if (!this._updateSprites)
      return;
    this.GlobalPosition = this._nodeToFollow.GlobalPosition;
    this.Rotation = this._nodeToFollow.Rotation;
  }

  public async Task FadeOut()
  {
    this._updateSprites = false;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    this.StopParticles(this._tween);
    bool flag = await this._tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  private void StopParticles(Tween tween)
  {
    foreach (Node child in ((Node) this._sprites).GetChildren(false))
    {
      if (child is CpuParticles2D cpuParticles2D)
        tween.TweenProperty((GodotObject) cpuParticles2D, NodePath.op_Implicit("amount"), Variant.op_Implicit(1), 0.5);
    }
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCardTrailVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("characterTrailPath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardTrailVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTrailVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardTrailVfx.MethodName.StopParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tween"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Tween"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardTrailVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardTrailVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NCardTrailVfx ncardTrailVfx = NCardTrailVfx.Create(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NCardTrailVfx>(ref ncardTrailVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTrailVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTrailVfx.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTrailVfx.MethodName.StopParticles) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.StopParticles(VariantUtils.ConvertTo<Tween>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardTrailVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardTrailVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NCardTrailVfx ncardTrailVfx = NCardTrailVfx.Create(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NCardTrailVfx>(ref ncardTrailVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardTrailVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardTrailVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardTrailVfx.MethodName._Process) || StringName.op_Equality(ref method, NCardTrailVfx.MethodName.StopParticles) || StringName.op_Equality(ref method, NCardTrailVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._nodeToFollow))
    {
      this._nodeToFollow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._sprites))
    {
      this._sprites = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._updateSprites))
    {
      this._updateSprites = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._nodeToFollow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._nodeToFollow);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._sprites))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._sprites);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._updateSprites))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._updateSprites);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTrailVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardTrailVfx.PropertyName._nodeToFollow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTrailVfx.PropertyName._sprites, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardTrailVfx.PropertyName._updateSprites, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTrailVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardTrailVfx.PropertyName._nodeToFollow, Variant.From<Control>(ref this._nodeToFollow));
    info.AddProperty(NCardTrailVfx.PropertyName._sprites, Variant.From<Node2D>(ref this._sprites));
    info.AddProperty(NCardTrailVfx.PropertyName._updateSprites, Variant.From<bool>(ref this._updateSprites));
    info.AddProperty(NCardTrailVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardTrailVfx.PropertyName._nodeToFollow, ref variant1))
      this._nodeToFollow = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCardTrailVfx.PropertyName._sprites, ref variant2))
      this._sprites = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NCardTrailVfx.PropertyName._updateSprites, ref variant3))
      this._updateSprites = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (!info.TryGetProperty(NCardTrailVfx.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName StopParticles = StringName.op_Implicit(nameof (StopParticles));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _nodeToFollow = StringName.op_Implicit(nameof (_nodeToFollow));
    public static readonly StringName _sprites = StringName.op_Implicit(nameof (_sprites));
    public static readonly StringName _updateSprites = StringName.op_Implicit(nameof (_updateSprites));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
