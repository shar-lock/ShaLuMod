// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NCardExhaustVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardExhaustVfx.cs")]
public class NCardExhaustVfx : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_exhaust");
  [Export]
  private Control _cardParentContainer;
  [Export]
  private Control _materialContainer;
  [Export]
  private NParticlesContainer _particlesContainer;
  [Export]
  private float _exhaustDuration = 0.4f;
  [Export]
  private Curve _exhaustCurve;
  [Export]
  private Vector2 _erosionBaseRange;
  [Export]
  private Vector2 _particleHeightRange;
  private NCard _cardNode;
  private Vector2 _position;
  private static readonly StringName _erosionBaseParameter = new StringName("instance_shader_parameters/erosion_base");
  private static readonly StringName _erosionOffsetParameter = new StringName("instance_shader_parameters/erosion_texture_x_offset");

  public static NCardExhaustVfx? Create(NCard cardNode)
  {
    if (TestMode.IsOn)
      return (NCardExhaustVfx) null;
    NCardExhaustVfx ncardExhaustVfx = PreloadManager.Cache.GetScene(NCardExhaustVfx.scenePath).Instantiate<NCardExhaustVfx>((PackedScene.GenEditState) 0L);
    ncardExhaustVfx.SetParticlesPlaying(false);
    ncardExhaustVfx.SetProgress(0.0f);
    ncardExhaustVfx._position = cardNode.GlobalPosition;
    Node parent = ((Node) cardNode).GetParent();
    if (parent != null)
      parent.RemoveChildSafely((Node) cardNode);
    ((Node) ncardExhaustVfx._cardParentContainer).AddChildSafely((Node) cardNode);
    ncardExhaustVfx._cardNode = cardNode;
    return ncardExhaustVfx;
  }

  private void SetParticlesPlaying(bool isPlaying)
  {
    this._particlesContainer.SetEmitting(isPlaying);
  }

  private void SetProgress(float progress)
  {
    float num1 = this._exhaustCurve.Sample(progress);
    float num2 = Mathf.Lerp(this._erosionBaseRange.X, this._erosionBaseRange.Y, num1);
    float num3 = Mathf.Lerp(this._particleHeightRange.X, this._particleHeightRange.Y, num1);
    ((GodotObject) this._materialContainer).Set(NCardExhaustVfx._erosionBaseParameter, Variant.op_Implicit(num2));
    this._particlesContainer.Position = new Vector2(0.0f, num3);
  }

  public async Task PlayAnimation()
  {
    this.GlobalPosition = this._position;
    this._cardNode.Position = Vector2.op_Division(this._cardParentContainer.Size, 2f);
    ((CanvasItem) this._materialContainer).SelfModulate = new Color(1f, 1f, 1f, 1f);
    this.SetParticlesPlaying(true);
    this.SetProgress(0.0f);
    ((GodotObject) this._materialContainer).Set(NCardExhaustVfx._erosionOffsetParameter, Variant.op_Implicit(GD.Randf()));
    float num;
    for (float num1 = 0.0f; (double) num1 < (double) this._exhaustDuration; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      this.SetProgress(num1 / this._exhaustDuration);
      num = num1;
    }
    this.SetProgress(1f);
    this.SetParticlesPlaying(false);
    ((CanvasItem) this._materialContainer).SelfModulate = new Color(1f, 1f, 1f, 0.0f);
    TaskHelper.RunSafely(this.DelayedFree());
  }

  private async Task DelayedFree()
  {
    await Cmd.Wait(2f);
    ((Node) this._cardNode).QueueFreeSafely();
    ((Node) this).QueueFreeSafely();
  }

  public override void _ExitTree()
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this._cardNode) || !((Node) this).IsAncestorOf((Node) this._cardNode))
      return;
    ((Node) this._cardNode).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCardExhaustVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardExhaustVfx.MethodName.SetParticlesPlaying, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isPlaying"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardExhaustVfx.MethodName.SetProgress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("progress"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardExhaustVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardExhaustVfx ncardExhaustVfx = NCardExhaustVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardExhaustVfx>(ref ncardExhaustVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.SetParticlesPlaying) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetParticlesPlaying(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.SetProgress) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetProgress(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardExhaustVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardExhaustVfx ncardExhaustVfx = NCardExhaustVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardExhaustVfx>(ref ncardExhaustVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.SetParticlesPlaying) || StringName.op_Equality(ref method, NCardExhaustVfx.MethodName.SetProgress) || StringName.op_Equality(ref method, NCardExhaustVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._cardParentContainer))
    {
      this._cardParentContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._materialContainer))
    {
      this._materialContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._particlesContainer))
    {
      this._particlesContainer = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._exhaustDuration))
    {
      this._exhaustDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._exhaustCurve))
    {
      this._exhaustCurve = VariantUtils.ConvertTo<Curve>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._erosionBaseRange))
    {
      this._erosionBaseRange = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._particleHeightRange))
    {
      this._particleHeightRange = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._cardNode))
    {
      this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._position))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._position = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._cardParentContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardParentContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._materialContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._materialContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._particlesContainer))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._particlesContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._exhaustDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._exhaustDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._exhaustCurve))
    {
      value = VariantUtils.CreateFrom<Curve>(ref this._exhaustCurve);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._erosionBaseRange))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._erosionBaseRange);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._particleHeightRange))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._particleHeightRange);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._cardNode))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardExhaustVfx.PropertyName._position))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._position);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardExhaustVfx.PropertyName._cardParentContainer, (PropertyHint) 34L, "Control", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardExhaustVfx.PropertyName._materialContainer, (PropertyHint) 34L, "Control", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardExhaustVfx.PropertyName._particlesContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardExhaustVfx.PropertyName._exhaustDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardExhaustVfx.PropertyName._exhaustCurve, (PropertyHint) 17L, "Curve", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NCardExhaustVfx.PropertyName._erosionBaseRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NCardExhaustVfx.PropertyName._particleHeightRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardExhaustVfx.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardExhaustVfx.PropertyName._position, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardExhaustVfx.PropertyName._cardParentContainer, Variant.From<Control>(ref this._cardParentContainer));
    info.AddProperty(NCardExhaustVfx.PropertyName._materialContainer, Variant.From<Control>(ref this._materialContainer));
    info.AddProperty(NCardExhaustVfx.PropertyName._particlesContainer, Variant.From<NParticlesContainer>(ref this._particlesContainer));
    info.AddProperty(NCardExhaustVfx.PropertyName._exhaustDuration, Variant.From<float>(ref this._exhaustDuration));
    info.AddProperty(NCardExhaustVfx.PropertyName._exhaustCurve, Variant.From<Curve>(ref this._exhaustCurve));
    info.AddProperty(NCardExhaustVfx.PropertyName._erosionBaseRange, Variant.From<Vector2>(ref this._erosionBaseRange));
    info.AddProperty(NCardExhaustVfx.PropertyName._particleHeightRange, Variant.From<Vector2>(ref this._particleHeightRange));
    info.AddProperty(NCardExhaustVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
    info.AddProperty(NCardExhaustVfx.PropertyName._position, Variant.From<Vector2>(ref this._position));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._cardParentContainer, ref variant1))
      this._cardParentContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._materialContainer, ref variant2))
      this._materialContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._particlesContainer, ref variant3))
      this._particlesContainer = ((Variant) ref variant3).As<NParticlesContainer>();
    Variant variant4;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._exhaustDuration, ref variant4))
      this._exhaustDuration = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._exhaustCurve, ref variant5))
      this._exhaustCurve = ((Variant) ref variant5).As<Curve>();
    Variant variant6;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._erosionBaseRange, ref variant6))
      this._erosionBaseRange = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._particleHeightRange, ref variant7))
      this._particleHeightRange = ((Variant) ref variant7).As<Vector2>();
    Variant variant8;
    if (info.TryGetProperty(NCardExhaustVfx.PropertyName._cardNode, ref variant8))
      this._cardNode = ((Variant) ref variant8).As<NCard>();
    Variant variant9;
    if (!info.TryGetProperty(NCardExhaustVfx.PropertyName._position, ref variant9))
      return;
    this._position = ((Variant) ref variant9).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName SetParticlesPlaying = StringName.op_Implicit(nameof (SetParticlesPlaying));
    public static readonly StringName SetProgress = StringName.op_Implicit(nameof (SetProgress));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _cardParentContainer = StringName.op_Implicit(nameof (_cardParentContainer));
    public static readonly StringName _materialContainer = StringName.op_Implicit(nameof (_materialContainer));
    public static readonly StringName _particlesContainer = StringName.op_Implicit(nameof (_particlesContainer));
    public static readonly StringName _exhaustDuration = StringName.op_Implicit(nameof (_exhaustDuration));
    public static readonly StringName _exhaustCurve = StringName.op_Implicit(nameof (_exhaustCurve));
    public static readonly StringName _erosionBaseRange = StringName.op_Implicit(nameof (_erosionBaseRange));
    public static readonly StringName _particleHeightRange = StringName.op_Implicit(nameof (_particleHeightRange));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
    public static readonly StringName _position = StringName.op_Implicit(nameof (_position));
  }

  public class SignalName : Control.SignalName
  {
  }
}
