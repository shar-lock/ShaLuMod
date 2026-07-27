// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NCardExhaustQuickVfx
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardExhaustQuickVfx.cs")]
public class NCardExhaustQuickVfx : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_exhaust_quick");
  [Export]
  private NParticlesContainer _anticipationParticlesContainer;
  [Export]
  private NParticlesContainer _particlesContainer;
  [Export]
  private float _anticipationDuration = 0.4f;
  private bool _isFinishing;
  private NCard _cardNode;

  public static NCardExhaustQuickVfx? Create(NCard cardNode)
  {
    if (TestMode.IsOn)
      return (NCardExhaustQuickVfx) null;
    NCardExhaustQuickVfx child = PreloadManager.Cache.GetScene(NCardExhaustQuickVfx.scenePath).Instantiate<NCardExhaustQuickVfx>((PackedScene.GenEditState) 0L);
    child._cardNode = cardNode;
    ((CanvasItem) child._anticipationParticlesContainer).Modulate = new Color(1f, 1f, 1f, 1f);
    cardNode.CardVfxContainer.AddChildSafely((Node) child);
    return child;
  }

  public async Task PlayAnimation()
  {
    this._isFinishing = false;
    this._anticipationParticlesContainer.Restart();
    await Cmd.Wait(this._anticipationDuration);
    this._isFinishing = true;
    ((CanvasItem) this._anticipationParticlesContainer).Modulate = new Color(1f, 1f, 1f, 0.0f);
    Node parent = ((Node) this).GetParent();
    Vector2 globalPosition = this.GlobalPosition;
    float rotation = this.Rotation;
    Vector2 scale = this._cardNode.Scale;
    if (parent != null)
      parent.RemoveChildSafely((Node) this);
    if (NCombatRoom.Instance != null)
    {
      ((Node) NCombatRoom.Instance.Ui).AddChildSafely((Node) this);
      this.GlobalPosition = globalPosition;
      this.Rotation = rotation;
      this.Scale = scale;
    }
    ((Node) this._cardNode).QueueFreeSafely();
    this._particlesContainer.Restart();
    TaskHelper.RunSafely(this.DelayedFree());
  }

  private async Task DelayedFree()
  {
    await Cmd.Wait(2f);
    ((Node) this).QueueFreeSafely();
  }

  public override void _ExitTree()
  {
    if (this._isFinishing || !GodotObject.IsInstanceValid((GodotObject) this._cardNode))
      return;
    ((Node) this._cardNode).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardExhaustQuickVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardExhaustQuickVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardExhaustQuickVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardExhaustQuickVfx ncardExhaustQuickVfx = NCardExhaustQuickVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardExhaustQuickVfx>(ref ncardExhaustQuickVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardExhaustQuickVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NCardExhaustQuickVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardExhaustQuickVfx ncardExhaustQuickVfx = NCardExhaustQuickVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardExhaustQuickVfx>(ref ncardExhaustQuickVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardExhaustQuickVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardExhaustQuickVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer))
    {
      this._anticipationParticlesContainer = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._particlesContainer))
    {
      this._particlesContainer = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._anticipationDuration))
    {
      this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._isFinishing))
    {
      this._isFinishing = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._cardNode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._anticipationParticlesContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._particlesContainer))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._particlesContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._anticipationDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._isFinishing))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isFinishing);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardExhaustQuickVfx.PropertyName._cardNode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardExhaustQuickVfx.PropertyName._particlesContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardExhaustQuickVfx.PropertyName._anticipationDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NCardExhaustQuickVfx.PropertyName._isFinishing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardExhaustQuickVfx.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer, Variant.From<NParticlesContainer>(ref this._anticipationParticlesContainer));
    info.AddProperty(NCardExhaustQuickVfx.PropertyName._particlesContainer, Variant.From<NParticlesContainer>(ref this._particlesContainer));
    info.AddProperty(NCardExhaustQuickVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
    info.AddProperty(NCardExhaustQuickVfx.PropertyName._isFinishing, Variant.From<bool>(ref this._isFinishing));
    info.AddProperty(NCardExhaustQuickVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._anticipationParticlesContainer, ref variant1))
      this._anticipationParticlesContainer = ((Variant) ref variant1).As<NParticlesContainer>();
    Variant variant2;
    if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._particlesContainer, ref variant2))
      this._particlesContainer = ((Variant) ref variant2).As<NParticlesContainer>();
    Variant variant3;
    if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._anticipationDuration, ref variant3))
      this._anticipationDuration = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._isFinishing, ref variant4))
      this._isFinishing = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (!info.TryGetProperty(NCardExhaustQuickVfx.PropertyName._cardNode, ref variant5))
      return;
    this._cardNode = ((Variant) ref variant5).As<NCard>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _anticipationParticlesContainer = StringName.op_Implicit(nameof (_anticipationParticlesContainer));
    public static readonly StringName _particlesContainer = StringName.op_Implicit(nameof (_particlesContainer));
    public static readonly StringName _anticipationDuration = StringName.op_Implicit(nameof (_anticipationDuration));
    public static readonly StringName _isFinishing = StringName.op_Implicit(nameof (_isFinishing));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
  }

  public class SignalName : Control.SignalName
  {
  }
}
