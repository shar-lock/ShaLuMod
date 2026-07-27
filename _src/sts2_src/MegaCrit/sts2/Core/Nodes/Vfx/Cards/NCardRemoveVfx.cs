// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NCardRemoveVfx
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
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NCardRemoveVfx.cs")]
public class NCardRemoveVfx : Control
{
  public const float deleteCardDelay = 0.4f;
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/ui/card/vfx_card_remove");
  [Export]
  private NParticlesContainer _anticipationParticles;
  [Export]
  private NParticlesContainer _slashStartParticles;
  [Export]
  private NParticlesContainer _slashEndParticles;
  [Export]
  private NParticlesContainer _cardParticles;
  [Export]
  private float _anticipationDuration = 0.25f;
  [Export]
  private float _slashEndDelay = 0.1f;
  private NCard _cardNode;

  public static NCardRemoveVfx? Create(NCard cardNode)
  {
    if (TestMode.IsOn)
      return (NCardRemoveVfx) null;
    NCardRemoveVfx ncardRemoveVfx = PreloadManager.Cache.GetScene(NCardRemoveVfx.scenePath).Instantiate<NCardRemoveVfx>((PackedScene.GenEditState) 0L);
    ncardRemoveVfx._cardNode = cardNode;
    return ncardRemoveVfx;
  }

  public override void _Ready()
  {
    this.GlobalPosition = this._cardNode.GlobalPosition;
    this.Rotation = this._cardNode.Rotation;
    this._cardParticles.SetEmitting(false);
    TaskHelper.RunSafely(this.PlayAnimation());
  }

  private async Task PlayAnimation()
  {
    this._anticipationParticles.Restart();
    await Cmd.Wait(this._anticipationDuration);
    this._slashStartParticles.Restart();
    await Cmd.Wait(this._slashEndDelay);
    this._slashEndParticles.Restart();
    this._cardParticles.Restart();
    TaskHelper.RunSafely(this.DelayedFree());
  }

  private async Task DelayedFree()
  {
    await Cmd.Wait(2f);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardRemoveVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardRemoveVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardRemoveVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardRemoveVfx ncardRemoveVfx = NCardRemoveVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardRemoveVfx>(ref ncardRemoveVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardRemoveVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardRemoveVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardRemoveVfx ncardRemoveVfx = NCardRemoveVfx.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NCardRemoveVfx>(ref ncardRemoveVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardRemoveVfx.MethodName.Create) || StringName.op_Equality(ref method, NCardRemoveVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._anticipationParticles))
    {
      this._anticipationParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._slashStartParticles))
    {
      this._slashStartParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._slashEndParticles))
    {
      this._slashEndParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._cardParticles))
    {
      this._cardParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._anticipationDuration))
    {
      this._anticipationDuration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._slashEndDelay))
    {
      this._slashEndDelay = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._cardNode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._anticipationParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._anticipationParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._slashStartParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._slashStartParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._slashEndParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._slashEndParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._cardParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._cardParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._anticipationDuration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._anticipationDuration);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._slashEndDelay))
    {
      value = VariantUtils.CreateFrom<float>(ref this._slashEndDelay);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardRemoveVfx.PropertyName._cardNode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardRemoveVfx.PropertyName._anticipationParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardRemoveVfx.PropertyName._slashStartParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardRemoveVfx.PropertyName._slashEndParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardRemoveVfx.PropertyName._cardParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardRemoveVfx.PropertyName._anticipationDuration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NCardRemoveVfx.PropertyName._slashEndDelay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCardRemoveVfx.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardRemoveVfx.PropertyName._anticipationParticles, Variant.From<NParticlesContainer>(ref this._anticipationParticles));
    info.AddProperty(NCardRemoveVfx.PropertyName._slashStartParticles, Variant.From<NParticlesContainer>(ref this._slashStartParticles));
    info.AddProperty(NCardRemoveVfx.PropertyName._slashEndParticles, Variant.From<NParticlesContainer>(ref this._slashEndParticles));
    info.AddProperty(NCardRemoveVfx.PropertyName._cardParticles, Variant.From<NParticlesContainer>(ref this._cardParticles));
    info.AddProperty(NCardRemoveVfx.PropertyName._anticipationDuration, Variant.From<float>(ref this._anticipationDuration));
    info.AddProperty(NCardRemoveVfx.PropertyName._slashEndDelay, Variant.From<float>(ref this._slashEndDelay));
    info.AddProperty(NCardRemoveVfx.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardRemoveVfx.PropertyName._anticipationParticles, ref variant1))
      this._anticipationParticles = ((Variant) ref variant1).As<NParticlesContainer>();
    Variant variant2;
    if (info.TryGetProperty(NCardRemoveVfx.PropertyName._slashStartParticles, ref variant2))
      this._slashStartParticles = ((Variant) ref variant2).As<NParticlesContainer>();
    Variant variant3;
    if (info.TryGetProperty(NCardRemoveVfx.PropertyName._slashEndParticles, ref variant3))
      this._slashEndParticles = ((Variant) ref variant3).As<NParticlesContainer>();
    Variant variant4;
    if (info.TryGetProperty(NCardRemoveVfx.PropertyName._cardParticles, ref variant4))
      this._cardParticles = ((Variant) ref variant4).As<NParticlesContainer>();
    Variant variant5;
    if (info.TryGetProperty(NCardRemoveVfx.PropertyName._anticipationDuration, ref variant5))
      this._anticipationDuration = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NCardRemoveVfx.PropertyName._slashEndDelay, ref variant6))
      this._slashEndDelay = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (!info.TryGetProperty(NCardRemoveVfx.PropertyName._cardNode, ref variant7))
      return;
    this._cardNode = ((Variant) ref variant7).As<NCard>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _anticipationParticles = StringName.op_Implicit(nameof (_anticipationParticles));
    public static readonly StringName _slashStartParticles = StringName.op_Implicit(nameof (_slashStartParticles));
    public static readonly StringName _slashEndParticles = StringName.op_Implicit(nameof (_slashEndParticles));
    public static readonly StringName _cardParticles = StringName.op_Implicit(nameof (_cardParticles));
    public static readonly StringName _anticipationDuration = StringName.op_Implicit(nameof (_anticipationDuration));
    public static readonly StringName _slashEndDelay = StringName.op_Implicit(nameof (_slashEndDelay));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
  }

  public class SignalName : Control.SignalName
  {
  }
}
