// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NAncientBgContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NAncientBgContainer.cs")]
public class NAncientBgContainer : Control
{
  private Window _window;
  private const float _ratioMin = 1.3333f;
  private const float _ratioNormal = 1.7777f;
  private const float _ratioMax = 2.3333f;
  private Vector2 _pos43 = new Vector2(-140f, 110f);
  private Vector2 _scale43 = new Vector2(1f, 1f);
  private Vector2 _pos169 = new Vector2(0.0f, 40f);
  private Vector2 _scale169 = new Vector2(0.89f, 0.89f);
  private Vector2 _pos219 = new Vector2(330f, 40f);
  private Vector2 _scale219 = new Vector2(1f, 1f);

  public override void _Ready()
  {
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.OnWindowChange();
  }

  private void OnWindowChange()
  {
    float num1 = Mathf.Clamp(this.Size.X / this.Size.Y, 1.3333f, 2.3333f);
    this.PivotOffset = Vector2.op_Multiply(this.Size, 0.5f);
    if ((double) num1 < 1.7776999473571777)
    {
      float num2 = Mathf.InverseLerp(1.3333f, 1.7777f, num1);
      this.Position = ((Vector2) ref this._pos43).Lerp(this._pos169, num2);
      this.Scale = ((Vector2) ref this._scale43).Lerp(this._scale169, num2);
    }
    else
    {
      float num3 = Mathf.InverseLerp(1.7777f, 2.3333f, num1);
      this.Position = ((Vector2) ref this._pos169).Lerp(this._pos219, num3);
      this.Scale = ((Vector2) ref this._scale169).Lerp(this._scale219, num3);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NAncientBgContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientBgContainer.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAncientBgContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAncientBgContainer.MethodName.OnWindowChange) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnWindowChange();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAncientBgContainer.MethodName._Ready) || StringName.op_Equality(ref method, NAncientBgContainer.MethodName.OnWindowChange) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._window))
    {
      this._window = VariantUtils.ConvertTo<Window>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._pos43))
    {
      this._pos43 = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._scale43))
    {
      this._scale43 = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._pos169))
    {
      this._pos169 = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._scale169))
    {
      this._scale169 = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._pos219))
    {
      this._pos219 = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._scale219))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._scale219 = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._window))
    {
      value = VariantUtils.CreateFrom<Window>(ref this._window);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._pos43))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._pos43);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._scale43))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._scale43);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._pos169))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._pos169);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._scale169))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._scale169);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._pos219))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._pos219);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientBgContainer.PropertyName._scale219))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._scale219);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAncientBgContainer.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientBgContainer.PropertyName._pos43, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientBgContainer.PropertyName._scale43, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientBgContainer.PropertyName._pos169, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientBgContainer.PropertyName._scale169, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientBgContainer.PropertyName._pos219, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAncientBgContainer.PropertyName._scale219, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAncientBgContainer.PropertyName._window, Variant.From<Window>(ref this._window));
    info.AddProperty(NAncientBgContainer.PropertyName._pos43, Variant.From<Vector2>(ref this._pos43));
    info.AddProperty(NAncientBgContainer.PropertyName._scale43, Variant.From<Vector2>(ref this._scale43));
    info.AddProperty(NAncientBgContainer.PropertyName._pos169, Variant.From<Vector2>(ref this._pos169));
    info.AddProperty(NAncientBgContainer.PropertyName._scale169, Variant.From<Vector2>(ref this._scale169));
    info.AddProperty(NAncientBgContainer.PropertyName._pos219, Variant.From<Vector2>(ref this._pos219));
    info.AddProperty(NAncientBgContainer.PropertyName._scale219, Variant.From<Vector2>(ref this._scale219));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAncientBgContainer.PropertyName._window, ref variant1))
      this._window = ((Variant) ref variant1).As<Window>();
    Variant variant2;
    if (info.TryGetProperty(NAncientBgContainer.PropertyName._pos43, ref variant2))
      this._pos43 = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NAncientBgContainer.PropertyName._scale43, ref variant3))
      this._scale43 = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NAncientBgContainer.PropertyName._pos169, ref variant4))
      this._pos169 = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NAncientBgContainer.PropertyName._scale169, ref variant5))
      this._scale169 = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NAncientBgContainer.PropertyName._pos219, ref variant6))
      this._pos219 = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (!info.TryGetProperty(NAncientBgContainer.PropertyName._scale219, ref variant7))
      return;
    this._scale219 = ((Variant) ref variant7).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
    public static readonly StringName _pos43 = StringName.op_Implicit(nameof (_pos43));
    public static readonly StringName _scale43 = StringName.op_Implicit(nameof (_scale43));
    public static readonly StringName _pos169 = StringName.op_Implicit(nameof (_pos169));
    public static readonly StringName _scale169 = StringName.op_Implicit(nameof (_scale169));
    public static readonly StringName _pos219 = StringName.op_Implicit(nameof (_pos219));
    public static readonly StringName _scale219 = StringName.op_Implicit(nameof (_scale219));
  }

  public class SignalName : Control.SignalName
  {
  }
}
