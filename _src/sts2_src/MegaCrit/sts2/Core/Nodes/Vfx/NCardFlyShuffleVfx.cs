// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCardFlyShuffleVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NCardFlyShuffleVfx.cs")]
public class NCardFlyShuffleVfx : Control
{
  private NCardTrailVfx? _vfx;
  private Tween? _fadeOutTween;
  private bool _vfxFading;
  private Vector2 _startPos;
  private Vector2 _endPos;
  private float _controlPointOffset;
  private float _duration;
  private float _speed;
  private float _accel;
  private float _arcDir;
  private string _trailPath;
  private CardPile _targetPile;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_card_shuffle_fly");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardFlyShuffleVfx._scenePath);
    }
  }

  public static NCardFlyShuffleVfx? Create(
    CardPile startPile,
    CardPile targetPile,
    string trailPath)
  {
    if (TestMode.IsOn)
      return (NCardFlyShuffleVfx) null;
    SfxCmd.PlayCardSwooshSfx(targetPile, startPile);
    NCardFlyShuffleVfx ncardFlyShuffleVfx = PreloadManager.Cache.GetScene(NCardFlyShuffleVfx._scenePath).Instantiate<NCardFlyShuffleVfx>((PackedScene.GenEditState) 0L);
    ncardFlyShuffleVfx._startPos = startPile.Type.GetTargetPosition((NCard) null);
    ncardFlyShuffleVfx._endPos = targetPile.Type.GetTargetPosition((NCard) null);
    ncardFlyShuffleVfx._trailPath = trailPath;
    ncardFlyShuffleVfx._targetPile = targetPile;
    return ncardFlyShuffleVfx;
  }

  public override void _Ready()
  {
    this._controlPointOffset = Rng.Chaotic.NextFloat(-300f, 400f);
    this._speed = Rng.Chaotic.NextFloat(1.1f, 1.25f);
    this._accel = Rng.Chaotic.NextFloat(2f, 2.5f);
    this._arcDir = (double) this._endPos.Y < 540.0 ? -500f : 500f + this._controlPointOffset;
    this._duration = Rng.Chaotic.NextFloat(1f, 1.75f);
    this._vfx = NCardTrailVfx.Create((Control) this, this._trailPath);
    if (this._vfx != null)
      ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) this._vfx);
    Node parent = ((Node) this).GetParent();
    parent.MoveChildSafely((Node) this, parent.GetChildCount(false) - 1);
    TaskHelper.RunSafely(this.PlayAnim());
  }

  private async Task PlayAnim()
  {
    float time = 0.0f;
    while ((double) time / (double) this._duration <= 1.0)
    {
      double num = (double) await ((Node) this).AwaitProcessFrame();
      if (this._cancelToken.IsCancellationRequested)
        return;
      float processDeltaTime = (float) ((Node) this).GetProcessDeltaTime();
      time += this._speed * processDeltaTime;
      this._speed += this._accel * processDeltaTime;
      Vector2 c0 = Vector2.op_Addition(this._startPos, Vector2.op_Multiply(Vector2.op_Subtraction(this._endPos, this._startPos), 0.5f));
      c0.Y -= this._arcDir;
      this.GlobalPosition = MathHelper.BezierCurve(this._startPos, this._endPos, c0, time / this._duration);
      Vector2 vector2 = Vector2.op_Subtraction(MathHelper.BezierCurve(this._startPos, this._endPos, c0, (time + 0.05f) / this._duration), this.GlobalPosition);
      this.Rotation = ((Vector2) ref vector2).Angle() + 1.57079637f;
    }
    this.GlobalPosition = this._endPos;
    this._targetPile.InvokeCardAddFinished();
    time = 0.0f;
    while ((double) time / (double) this._duration <= 1.0)
    {
      double num = (double) await ((Node) this).AwaitProcessFrame();
      if (this._cancelToken.IsCancellationRequested)
        return;
      float processDeltaTime = (float) ((Node) this).GetProcessDeltaTime();
      time += this._speed * processDeltaTime;
      if ((double) time / (double) this._duration > 0.25 && !this._vfxFading)
      {
        if (this._vfx != null)
          TaskHelper.RunSafely(this._vfx.FadeOut());
        this._vfxFading = true;
      }
      this.Scale = Vector2.op_Multiply(Vector2.One, Mathf.Max(Mathf.Lerp(0.1f, -0.1f, time / this._duration), 0.0f));
    }
    this._fadeOutTween = ((Node) this).CreateTween();
    this._fadeOutTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.800000011920929);
    bool flag = await this._fadeOutTween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public override void _ExitTree()
  {
    this._fadeOutTween?.Kill();
    this._cancelToken.Cancel();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardFlyShuffleVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardFlyShuffleVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardFlyShuffleVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardFlyShuffleVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardFlyShuffleVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCardFlyShuffleVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._vfx))
    {
      this._vfx = VariantUtils.ConvertTo<NCardTrailVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._fadeOutTween))
    {
      this._fadeOutTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._vfxFading))
    {
      this._vfxFading = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._startPos))
    {
      this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._endPos))
    {
      this._endPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._controlPointOffset))
    {
      this._controlPointOffset = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._speed))
    {
      this._speed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._accel))
    {
      this._accel = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._arcDir))
    {
      this._arcDir = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._trailPath))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._trailPath = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._vfx))
    {
      value = VariantUtils.CreateFrom<NCardTrailVfx>(ref this._vfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._fadeOutTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._fadeOutTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._vfxFading))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._vfxFading);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._startPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._endPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._endPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._controlPointOffset))
    {
      value = VariantUtils.CreateFrom<float>(ref this._controlPointOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._speed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._speed);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._accel))
    {
      value = VariantUtils.CreateFrom<float>(ref this._accel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._arcDir))
    {
      value = VariantUtils.CreateFrom<float>(ref this._arcDir);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardFlyShuffleVfx.PropertyName._trailPath))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<string>(ref this._trailPath);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardFlyShuffleVfx.PropertyName._vfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardFlyShuffleVfx.PropertyName._fadeOutTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardFlyShuffleVfx.PropertyName._vfxFading, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardFlyShuffleVfx.PropertyName._startPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardFlyShuffleVfx.PropertyName._endPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyShuffleVfx.PropertyName._controlPointOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyShuffleVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyShuffleVfx.PropertyName._speed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyShuffleVfx.PropertyName._accel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardFlyShuffleVfx.PropertyName._arcDir, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NCardFlyShuffleVfx.PropertyName._trailPath, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._vfx, Variant.From<NCardTrailVfx>(ref this._vfx));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._fadeOutTween, Variant.From<Tween>(ref this._fadeOutTween));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._vfxFading, Variant.From<bool>(ref this._vfxFading));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._endPos, Variant.From<Vector2>(ref this._endPos));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._controlPointOffset, Variant.From<float>(ref this._controlPointOffset));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._speed, Variant.From<float>(ref this._speed));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._accel, Variant.From<float>(ref this._accel));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._arcDir, Variant.From<float>(ref this._arcDir));
    info.AddProperty(NCardFlyShuffleVfx.PropertyName._trailPath, Variant.From<string>(ref this._trailPath));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._vfx, ref variant1))
      this._vfx = ((Variant) ref variant1).As<NCardTrailVfx>();
    Variant variant2;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._fadeOutTween, ref variant2))
      this._fadeOutTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._vfxFading, ref variant3))
      this._vfxFading = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._startPos, ref variant4))
      this._startPos = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._endPos, ref variant5))
      this._endPos = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._controlPointOffset, ref variant6))
      this._controlPointOffset = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._duration, ref variant7))
      this._duration = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._speed, ref variant8))
      this._speed = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._accel, ref variant9))
      this._accel = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._arcDir, ref variant10))
      this._arcDir = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (!info.TryGetProperty(NCardFlyShuffleVfx.PropertyName._trailPath, ref variant11))
      return;
    this._trailPath = ((Variant) ref variant11).As<string>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _vfx = StringName.op_Implicit(nameof (_vfx));
    public static readonly StringName _fadeOutTween = StringName.op_Implicit(nameof (_fadeOutTween));
    public static readonly StringName _vfxFading = StringName.op_Implicit(nameof (_vfxFading));
    public static readonly StringName _startPos = StringName.op_Implicit(nameof (_startPos));
    public static readonly StringName _endPos = StringName.op_Implicit(nameof (_endPos));
    public static readonly StringName _controlPointOffset = StringName.op_Implicit(nameof (_controlPointOffset));
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _speed = StringName.op_Implicit(nameof (_speed));
    public static readonly StringName _accel = StringName.op_Implicit(nameof (_accel));
    public static readonly StringName _arcDir = StringName.op_Implicit(nameof (_arcDir));
    public static readonly StringName _trailPath = StringName.op_Implicit(nameof (_trailPath));
  }

  public class SignalName : Control.SignalName
  {
  }
}
