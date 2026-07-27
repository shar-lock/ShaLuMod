// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsGradientMask
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsGradientMask.cs")]
public class NSettingsGradientMask : TextureRect
{
  private const float _fadeOffset = -8f;
  private const float _fadeSize = 16f;
  private NSettingsTabManager _tabContainer;
  private GradientTexture2D _texture;

  public override void _Ready()
  {
    this._tabContainer = ((Node) ((Node) this).GetAncestorOfType<NSettingsScreen>()).GetNode<NSettingsTabManager>(NodePath.op_Implicit("%SettingsTabManager"));
    this._texture = (GradientTexture2D) this.Texture;
    ((GodotObject) this).Connect(Control.SignalName.Resized, Callable.From(new Action(this.OnResized)), 0U);
    this.OnResized();
  }

  private void OnResized()
  {
    float num1 = (float) (1.0 - ((double) this._tabContainer.Position.Y + (double) this._tabContainer.Size.Y - 8.0 + 16.0) / (double) ((Control) this).Size.Y);
    float num2 = num1 + 16f / ((Control) this).Size.Y;
    this._texture.Gradient.SetOffset(2, num1);
    this._texture.Gradient.SetOffset(3, num2);
    this._texture.Gradient.SetColor(2, Colors.White);
    this._texture.Gradient.SetColor(3, StsColors.transparentWhite);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NSettingsGradientMask.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsGradientMask.MethodName.OnResized, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsGradientMask.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsGradientMask.MethodName.OnResized) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnResized();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsGradientMask.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsGradientMask.MethodName.OnResized) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsGradientMask.PropertyName._tabContainer))
    {
      this._tabContainer = VariantUtils.ConvertTo<NSettingsTabManager>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsGradientMask.PropertyName._texture))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._texture = VariantUtils.ConvertTo<GradientTexture2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsGradientMask.PropertyName._tabContainer))
    {
      value = VariantUtils.CreateFrom<NSettingsTabManager>(ref this._tabContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsGradientMask.PropertyName._texture))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GradientTexture2D>(ref this._texture);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsGradientMask.PropertyName._tabContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsGradientMask.PropertyName._texture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSettingsGradientMask.PropertyName._tabContainer, Variant.From<NSettingsTabManager>(ref this._tabContainer));
    info.AddProperty(NSettingsGradientMask.PropertyName._texture, Variant.From<GradientTexture2D>(ref this._texture));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsGradientMask.PropertyName._tabContainer, ref variant1))
      this._tabContainer = ((Variant) ref variant1).As<NSettingsTabManager>();
    Variant variant2;
    if (!info.TryGetProperty(NSettingsGradientMask.PropertyName._texture, ref variant2))
      return;
    this._texture = ((Variant) ref variant2).As<GradientTexture2D>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnResized = StringName.op_Implicit(nameof (OnResized));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _tabContainer = StringName.op_Implicit(nameof (_tabContainer));
    public static readonly StringName _texture = StringName.op_Implicit(nameof (_texture));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
