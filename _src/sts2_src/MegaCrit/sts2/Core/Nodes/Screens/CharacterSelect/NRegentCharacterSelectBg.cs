// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NRegentCharacterSelectBg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NRegentCharacterSelectBg.cs")]
public class NRegentCharacterSelectBg : Control
{
  private MegaSprite _spineController;
  private Control _sphereGuardianHover;
  private Control _decaHover;
  private Control _sentryHover;
  private Control _sneckoHover;
  private Control _cultistHover;
  private Control _shapesHover;
  private Control _amogusHover;

  public override void _Ready()
  {
    this._spineController = new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this).GetNode(NodePath.op_Implicit("SpineSprite"))));
    this._sphereGuardianHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("SphereGuardianHover"));
    ((GodotObject) this._sphereGuardianHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("spheric guardian constellation"))), 0U);
    ((GodotObject) this._sphereGuardianHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
    this._decaHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("DecaHover"));
    ((GodotObject) this._decaHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("deca outline"))), 0U);
    ((GodotObject) this._decaHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
    this._sentryHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("SentryHover"));
    ((GodotObject) this._sentryHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("sentry constellation"))), 0U);
    ((GodotObject) this._sentryHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
    this._sneckoHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("SneckoHover"));
    ((GodotObject) this._sneckoHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("snecko constellation"))), 0U);
    ((GodotObject) this._sneckoHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
    this._cultistHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("CultistHover"));
    ((GodotObject) this._cultistHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("cultist constellation"))), 0U);
    ((GodotObject) this._cultistHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
    this._shapesHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("ShapesHover"));
    ((GodotObject) this._shapesHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("shapes constellation"))), 0U);
    ((GodotObject) this._shapesHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
    this._amogusHover = ((Node) this).GetNode<Control>(NodePath.op_Implicit("AmogusHover"));
    ((GodotObject) this._amogusHover).Connect(Control.SignalName.MouseEntered, Callable.From((Action) (() => this.SetSkin("amogus constellation"))), 0U);
    ((GodotObject) this._amogusHover).Connect(Control.SignalName.MouseExited, Callable.From((Action) (() => this.SetSkin("normal"))), 0U);
  }

  private void SetSkin(string skinName)
  {
    MegaSkeleton skeleton = this._spineController.GetSkeleton();
    if (skeleton == null)
      return;
    skeleton.SetSkin(skeleton.GetData().FindSkin(skinName));
    skeleton.SetSlotsToSetupPose();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NRegentCharacterSelectBg.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRegentCharacterSelectBg.MethodName.SetSkin, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("skinName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRegentCharacterSelectBg.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRegentCharacterSelectBg.MethodName.SetSkin) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetSkin(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRegentCharacterSelectBg.MethodName._Ready) || StringName.op_Equality(ref method, NRegentCharacterSelectBg.MethodName.SetSkin) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._sphereGuardianHover))
    {
      this._sphereGuardianHover = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._decaHover))
    {
      this._decaHover = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._sentryHover))
    {
      this._sentryHover = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._sneckoHover))
    {
      this._sneckoHover = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._cultistHover))
    {
      this._cultistHover = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._shapesHover))
    {
      this._shapesHover = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._amogusHover))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._amogusHover = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._sphereGuardianHover))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._sphereGuardianHover);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._decaHover))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._decaHover);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._sentryHover))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._sentryHover);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._sneckoHover))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._sneckoHover);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._cultistHover))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cultistHover);
      return true;
    }
    if (StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._shapesHover))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._shapesHover);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRegentCharacterSelectBg.PropertyName._amogusHover))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._amogusHover);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._sphereGuardianHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._decaHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._sentryHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._sneckoHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._cultistHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._shapesHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRegentCharacterSelectBg.PropertyName._amogusHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._sphereGuardianHover, Variant.From<Control>(ref this._sphereGuardianHover));
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._decaHover, Variant.From<Control>(ref this._decaHover));
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._sentryHover, Variant.From<Control>(ref this._sentryHover));
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._sneckoHover, Variant.From<Control>(ref this._sneckoHover));
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._cultistHover, Variant.From<Control>(ref this._cultistHover));
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._shapesHover, Variant.From<Control>(ref this._shapesHover));
    info.AddProperty(NRegentCharacterSelectBg.PropertyName._amogusHover, Variant.From<Control>(ref this._amogusHover));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._sphereGuardianHover, ref variant1))
      this._sphereGuardianHover = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._decaHover, ref variant2))
      this._decaHover = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._sentryHover, ref variant3))
      this._sentryHover = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._sneckoHover, ref variant4))
      this._sneckoHover = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._cultistHover, ref variant5))
      this._cultistHover = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._shapesHover, ref variant6))
      this._shapesHover = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (!info.TryGetProperty(NRegentCharacterSelectBg.PropertyName._amogusHover, ref variant7))
      return;
    this._amogusHover = ((Variant) ref variant7).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetSkin = StringName.op_Implicit(nameof (SetSkin));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _sphereGuardianHover = StringName.op_Implicit(nameof (_sphereGuardianHover));
    public static readonly StringName _decaHover = StringName.op_Implicit(nameof (_decaHover));
    public static readonly StringName _sentryHover = StringName.op_Implicit(nameof (_sentryHover));
    public static readonly StringName _sneckoHover = StringName.op_Implicit(nameof (_sneckoHover));
    public static readonly StringName _cultistHover = StringName.op_Implicit(nameof (_cultistHover));
    public static readonly StringName _shapesHover = StringName.op_Implicit(nameof (_shapesHover));
    public static readonly StringName _amogusHover = StringName.op_Implicit(nameof (_amogusHover));
  }

  public class SignalName : Control.SignalName
  {
  }
}
