// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardFlyPowerVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardFlyPowerVfx.cs")]
public class NCardFlyPowerVfx : Node2D
{
  private const float _speed = 3000f;
  private const float _scaleOutProportion = 0.9f;
  private const float _initialRotationSpeed = 3.14159274f;
  private const float _maxRotationSpeed = 157.079636f;
  private NCreature _cardOwnerNode;
  private NCardTrailVfx? _vfx;
  private Path2D _swooshPath;
  private Tween? _scaleTween;
  private bool _scalingOut;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_card_power_fly");

  public NCard CardNode { get; private set; }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardFlyPowerVfx._scenePath);
    }
  }

  public static NCardFlyPowerVfx? Create(NCard card)
  {
    if (TestMode.IsOn)
      return (NCardFlyPowerVfx) null;
    NCardFlyPowerVfx ncardFlyPowerVfx = PreloadManager.Cache.GetScene(NCardFlyPowerVfx._scenePath).Instantiate<NCardFlyPowerVfx>((PackedScene.GenEditState) 0L);
    ncardFlyPowerVfx.CardNode = card;
    return ncardFlyPowerVfx;
  }

  public override void _Ready()
  {
    this.GlobalPosition = this.CardNode.GlobalPosition;
    Player owner = this.CardNode.Model.Owner;
    this._cardOwnerNode = NCombatRoom.Instance.GetCreatureNode(owner.Creature);
    this._vfx = NCardTrailVfx.Create((Control) this.CardNode, owner.Character.TrailPath);
    if (this._vfx != null)
      ((Node) this).AddChildSafely((Node) this._vfx);
    Vector2 vector2 = Vector2.op_Subtraction(this._cardOwnerNode.VfxSpawnPosition, this.GlobalPosition);
    this._swooshPath = ((Node) this).GetNode<Path2D>(NodePath.op_Implicit("SwooshPath"));
    this._swooshPath.Curve.SetPointPosition(this._swooshPath.Curve.PointCount - 1, vector2);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._scaleTween?.Kill();
    this._cancelToken.Cancel();
  }

  public float GetDuration() => this.GetDurationInternal() + 0.05f;

  private float GetDurationInternal() => this._swooshPath.Curve.GetBakedLength() / 3000f;

  public async Task PlayAnim()
  {
    SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_power");
    this._scaleTween = ((Node) this).CreateTween();
    this._scaleTween.TweenProperty((GodotObject) this.CardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.1f)), 0.30000001192092896);
    float length = this._swooshPath.Curve.GetBakedLength();
    double timeAccumulator = 0.0;
    float duration = this.GetDurationInternal();
    while (timeAccumulator < (double) duration)
    {
      double num1 = (double) await ((Node) this).AwaitProcessFrame();
      if (!this._cancelToken.IsCancellationRequested)
      {
        double processDeltaTime = ((Node) this).GetProcessDeltaTime();
        timeAccumulator += processDeltaTime;
        float p = (float) timeAccumulator / duration;
        Transform2D transform2D = this._swooshPath.Curve.SampleBakedWithRotation(Ease.QuadIn(p) * length, false);
        this.CardNode.GlobalPosition = Vector2.op_Addition(this.GlobalPosition, transform2D.Origin);
        float num2 = ((Transform2D) ref transform2D).Rotation - this.CardNode.Rotation;
        float num3 = Mathf.Lerp(3.14159274f, 157.079636f, p);
        this.CardNode.Rotation += (float) Mathf.Sign(num2) * Mathf.Min(Mathf.Abs(num2), num3 * (float) processDeltaTime);
        if ((double) p >= 0.89999997615814209 && !this._scalingOut)
        {
          this._scalingOut = true;
          this._scaleTween?.Kill();
          this._scaleTween = ((Node) this).CreateTween();
          this._scaleTween.TweenProperty((GodotObject) this.CardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), (double) (duration - (float) timeAccumulator));
        }
      }
      else
        break;
    }
    NGame.Instance.ScreenShake(ShakeStrength.Medium, ShakeDuration.Short);
    if (this._vfx != null)
      await this._vfx.FadeOut();
    ((Node) this.CardNode).QueueFreeSafely();
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCardFlyPowerVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardFlyPowerVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardFlyPowerVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardFlyPowerVfx.MethodName.GetDuration, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardFlyPowerVfx.MethodName.GetDurationInternal, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardFlyPowerVfx ncardFlyPowerVfx = NCardFlyPowerVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardFlyPowerVfx>(ref ncardFlyPowerVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.GetDuration) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float duration = this.GetDuration();
      ret = VariantUtils.CreateFrom<float>(ref duration);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.GetDurationInternal) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    float durationInternal = this.GetDurationInternal();
    ret = VariantUtils.CreateFrom<float>(ref durationInternal);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardFlyPowerVfx ncardFlyPowerVfx = NCardFlyPowerVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardFlyPowerVfx>(ref ncardFlyPowerVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.GetDuration) || StringName.op_Equality(ref method, NCardFlyPowerVfx.MethodName.GetDurationInternal) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName.CardNode))
    {
      this.CardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._cardOwnerNode))
    {
      this._cardOwnerNode = VariantUtils.ConvertTo<NCreature>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._vfx))
    {
      this._vfx = VariantUtils.ConvertTo<NCardTrailVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._swooshPath))
    {
      this._swooshPath = VariantUtils.ConvertTo<Path2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._scaleTween))
    {
      this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._scalingOut))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._scalingOut = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName.CardNode))
    {
      ref godot_variant local = ref value;
      NCard cardNode = this.CardNode;
      godot_variant from = VariantUtils.CreateFrom<NCard>(ref cardNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._cardOwnerNode))
    {
      value = VariantUtils.CreateFrom<NCreature>(ref this._cardOwnerNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._vfx))
    {
      value = VariantUtils.CreateFrom<NCardTrailVfx>(ref this._vfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._swooshPath))
    {
      value = VariantUtils.CreateFrom<Path2D>(ref this._swooshPath);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._scaleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardFlyPowerVfx.PropertyName._scalingOut))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._scalingOut);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardFlyPowerVfx.PropertyName.CardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyPowerVfx.PropertyName._cardOwnerNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyPowerVfx.PropertyName._vfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyPowerVfx.PropertyName._swooshPath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyPowerVfx.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardFlyPowerVfx.PropertyName._scalingOut, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName cardNode1 = NCardFlyPowerVfx.PropertyName.CardNode;
    NCard cardNode2 = this.CardNode;
    Variant variant = Variant.From<NCard>(ref cardNode2);
    serializationInfo.AddProperty(cardNode1, variant);
    info.AddProperty(NCardFlyPowerVfx.PropertyName._cardOwnerNode, Variant.From<NCreature>(ref this._cardOwnerNode));
    info.AddProperty(NCardFlyPowerVfx.PropertyName._vfx, Variant.From<NCardTrailVfx>(ref this._vfx));
    info.AddProperty(NCardFlyPowerVfx.PropertyName._swooshPath, Variant.From<Path2D>(ref this._swooshPath));
    info.AddProperty(NCardFlyPowerVfx.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
    info.AddProperty(NCardFlyPowerVfx.PropertyName._scalingOut, Variant.From<bool>(ref this._scalingOut));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName.CardNode, ref variant1))
      this.CardNode = ((Variant) ref variant1).As<NCard>();
    Variant variant2;
    if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._cardOwnerNode, ref variant2))
      this._cardOwnerNode = ((Variant) ref variant2).As<NCreature>();
    Variant variant3;
    if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._vfx, ref variant3))
      this._vfx = ((Variant) ref variant3).As<NCardTrailVfx>();
    Variant variant4;
    if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._swooshPath, ref variant4))
      this._swooshPath = ((Variant) ref variant4).As<Path2D>();
    Variant variant5;
    if (info.TryGetProperty(NCardFlyPowerVfx.PropertyName._scaleTween, ref variant5))
      this._scaleTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NCardFlyPowerVfx.PropertyName._scalingOut, ref variant6))
      return;
    this._scalingOut = ((Variant) ref variant6).As<bool>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName GetDuration = StringName.op_Implicit(nameof (GetDuration));
    public static readonly StringName GetDurationInternal = StringName.op_Implicit(nameof (GetDurationInternal));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName CardNode = StringName.op_Implicit(nameof (CardNode));
    public static readonly StringName _cardOwnerNode = StringName.op_Implicit(nameof (_cardOwnerNode));
    public static readonly StringName _vfx = StringName.op_Implicit(nameof (_vfx));
    public static readonly StringName _swooshPath = StringName.op_Implicit(nameof (_swooshPath));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
    public static readonly StringName _scalingOut = StringName.op_Implicit(nameof (_scalingOut));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
