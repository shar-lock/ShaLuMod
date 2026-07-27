// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCombatSceneContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCombatSceneContainer.cs")]
public class NCombatSceneContainer : Control
{
  private const float _sixteenByNine = 1.77777779f;
  private const float _maxNarrowRatio = 1.33333337f;
  private Window _window;
  private Control _bgContainer;

  public override void _Ready()
  {
    this._bgContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BgContainer"));
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
  }

  private void OnWindowChange()
  {
    float num = (float) this._window.Size.X / (float) this._window.Size.Y;
    if ((double) num < 1.7777777910232544 && SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto)
      this._bgContainer.Scale = Vector2.op_Multiply(Vector2.One, Mathf.Max(MathHelper.Remap(num, 1.33333337f, 1.77777779f, 1.08f, 0.9f), 1.08f));
    else
      this._bgContainer.Scale = Vector2.op_Multiply(Vector2.One, 0.9f);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCombatSceneContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatSceneContainer.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatSceneContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatSceneContainer.MethodName.OnWindowChange) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnWindowChange();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatSceneContainer.MethodName._Ready) || StringName.op_Equality(ref method, NCombatSceneContainer.MethodName.OnWindowChange) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatSceneContainer.PropertyName._window))
    {
      this._window = VariantUtils.ConvertTo<Window>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatSceneContainer.PropertyName._bgContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._bgContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatSceneContainer.PropertyName._window))
    {
      value = VariantUtils.CreateFrom<Window>(ref this._window);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatSceneContainer.PropertyName._bgContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._bgContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatSceneContainer.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatSceneContainer.PropertyName._bgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCombatSceneContainer.PropertyName._window, Variant.From<Window>(ref this._window));
    info.AddProperty(NCombatSceneContainer.PropertyName._bgContainer, Variant.From<Control>(ref this._bgContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatSceneContainer.PropertyName._window, ref variant1))
      this._window = ((Variant) ref variant1).As<Window>();
    Variant variant2;
    if (!info.TryGetProperty(NCombatSceneContainer.PropertyName._bgContainer, ref variant2))
      return;
    this._bgContainer = ((Variant) ref variant2).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
    public static readonly StringName _bgContainer = StringName.op_Implicit(nameof (_bgContainer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
