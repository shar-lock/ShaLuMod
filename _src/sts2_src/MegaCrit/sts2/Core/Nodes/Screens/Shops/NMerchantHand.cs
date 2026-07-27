// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantHand
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantHand.cs")]
public class NMerchantHand : Node
{
  private Vector2 _startPos;
  private Vector2 _targetPos;
  private Control? _targetNode;
  private Vector2 _targetOffset;
  private MegaBone? _bone;
  private FastNoiseLite _noise;
  private float _time;
  private CancellationTokenSource? _stopPointingToken;
  private Control _rug;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._rug = ((Node) this._parent).GetParent<Control>();
    this._startPos = this._parent.GlobalPosition;
    this._targetPos = this._startPos;
    this._noise = new FastNoiseLite();
    this._noise.NoiseType = (FastNoiseLite.NoiseTypeEnum) 3L;
    this._noise.Frequency = 1f;
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState =>
    {
      this._bone = this._animController.GetSkeleton()?.FindBone("rotate_me");
      animState.SetAnimation("default");
    }));
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._stopPointingToken?.Cancel();
  }

  public override void _Process(double delta)
  {
    if (this._targetNode != null && GodotObject.IsInstanceValid((GodotObject) this._targetNode))
      this._targetPos = Vector2.op_Addition(this._targetNode.GlobalPosition, this._targetOffset);
    this._time += (float) delta;
    float num1 = ((Noise) this._noise).GetNoise1D(this._time * 0.1f) + 0.4f;
    float num2 = ((Noise) this._noise).GetNoise1D((float) (((double) this._time + 0.25) * 0.10000000149011612)) - 0.5f;
    Node2D parent = this._parent;
    Vector2 globalPosition = this._parent.GlobalPosition;
    Vector2 vector2 = ((Vector2) ref globalPosition).Lerp(Vector2.op_Addition(this._targetPos, Vector2.op_Multiply(new Vector2(num1, num2), 100f)), (float) delta * 4f);
    parent.GlobalPosition = vector2;
    this._bone?.SetRotation(Mathf.Lerp(-10f, 10f, (float) (((double) this._parent.Position.X - (double) this._rug.Size.X * 0.5 - 50.0) * 0.0099999997764825821)));
  }

  public void PointAtTarget(Control target, Vector2 offset)
  {
    this._stopPointingToken?.Cancel();
    this._targetNode = target;
    this._targetOffset = offset;
  }

  public void StopPointing(float lingerTime)
  {
    this._targetNode = (Control) null;
    this._stopPointingToken?.Cancel();
    this._stopPointingToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.WaitAndReturn(this._stopPointingToken, lingerTime));
  }

  private async Task WaitAndReturn(CancellationTokenSource cancelToken, float lingerTime)
  {
    float num;
    for (float num1 = 0.0f; (double) num1 < (double) lingerTime; num1 = num + await this.AwaitProcessFrame())
    {
      if (cancelToken.IsCancellationRequested || !this.IsValid() || !this.IsInsideTree())
        return;
      num = num1;
    }
    this._targetPos = this._startPos;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMerchantHand.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantHand.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantHand.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantHand.MethodName.PointAtTarget, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("target"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("offset"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMerchantHand.MethodName.StopPointing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("lingerTime"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantHand.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantHand.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantHand.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantHand.MethodName.PointAtTarget) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.PointAtTarget(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantHand.MethodName.StopPointing) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StopPointing(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantHand.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantHand.MethodName._ExitTree) || StringName.op_Equality(ref method, NMerchantHand.MethodName._Process) || StringName.op_Equality(ref method, NMerchantHand.MethodName.PointAtTarget) || StringName.op_Equality(ref method, NMerchantHand.MethodName.StopPointing) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._startPos))
    {
      this._startPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._targetPos))
    {
      this._targetPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._targetNode))
    {
      this._targetNode = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._targetOffset))
    {
      this._targetOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._noise))
    {
      this._noise = VariantUtils.ConvertTo<FastNoiseLite>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._time))
    {
      this._time = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._rug))
    {
      this._rug = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantHand.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._startPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._targetPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._targetNode))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._targetNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._targetOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._noise))
    {
      value = VariantUtils.CreateFrom<FastNoiseLite>(ref this._noise);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._time))
    {
      value = VariantUtils.CreateFrom<float>(ref this._time);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantHand.PropertyName._rug))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rug);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantHand.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NMerchantHand.PropertyName._startPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMerchantHand.PropertyName._targetPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantHand.PropertyName._targetNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMerchantHand.PropertyName._targetOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantHand.PropertyName._noise, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NMerchantHand.PropertyName._time, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantHand.PropertyName._rug, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantHand.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMerchantHand.PropertyName._startPos, Variant.From<Vector2>(ref this._startPos));
    info.AddProperty(NMerchantHand.PropertyName._targetPos, Variant.From<Vector2>(ref this._targetPos));
    info.AddProperty(NMerchantHand.PropertyName._targetNode, Variant.From<Control>(ref this._targetNode));
    info.AddProperty(NMerchantHand.PropertyName._targetOffset, Variant.From<Vector2>(ref this._targetOffset));
    info.AddProperty(NMerchantHand.PropertyName._noise, Variant.From<FastNoiseLite>(ref this._noise));
    info.AddProperty(NMerchantHand.PropertyName._time, Variant.From<float>(ref this._time));
    info.AddProperty(NMerchantHand.PropertyName._rug, Variant.From<Control>(ref this._rug));
    info.AddProperty(NMerchantHand.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantHand.PropertyName._startPos, ref variant1))
      this._startPos = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantHand.PropertyName._targetPos, ref variant2))
      this._targetPos = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantHand.PropertyName._targetNode, ref variant3))
      this._targetNode = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NMerchantHand.PropertyName._targetOffset, ref variant4))
      this._targetOffset = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NMerchantHand.PropertyName._noise, ref variant5))
      this._noise = ((Variant) ref variant5).As<FastNoiseLite>();
    Variant variant6;
    if (info.TryGetProperty(NMerchantHand.PropertyName._time, ref variant6))
      this._time = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NMerchantHand.PropertyName._rug, ref variant7))
      this._rug = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (!info.TryGetProperty(NMerchantHand.PropertyName._parent, ref variant8))
      return;
    this._parent = ((Variant) ref variant8).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName PointAtTarget = StringName.op_Implicit(nameof (PointAtTarget));
    public static readonly StringName StopPointing = StringName.op_Implicit(nameof (StopPointing));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _startPos = StringName.op_Implicit(nameof (_startPos));
    public static readonly StringName _targetPos = StringName.op_Implicit(nameof (_targetPos));
    public static readonly StringName _targetNode = StringName.op_Implicit(nameof (_targetNode));
    public static readonly StringName _targetOffset = StringName.op_Implicit(nameof (_targetOffset));
    public static readonly StringName _noise = StringName.op_Implicit(nameof (_noise));
    public static readonly StringName _time = StringName.op_Implicit(nameof (_time));
    public static readonly StringName _rug = StringName.op_Implicit(nameof (_rug));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
