// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NCharacterSelectScreenBg
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NCharacterSelectScreenBg.cs")]
public class NCharacterSelectScreenBg : Control
{
  private Window _window;
  private const float _sixteenByNine = 1.77777779f;
  private const float _fourByThree = 1.33333337f;
  private static readonly float _defaultBgScale = 1.1f;
  private static readonly float _narrowBgScale = 1.153f;

  public override void _Ready()
  {
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
  }

  private void OnWindowChange()
  {
    float num = Mathf.Max(1.33333337f, (float) this._window.Size.X / (float) this._window.Size.Y);
    if ((double) num < 1.7777777910232544)
      this.Scale = Vector2.op_Multiply(Vector2.One, Mathf.Remap(Ease.CubicOut((float) (((double) num - 1.3333333730697632) / 0.44444441795349121)), 0.0f, 1f, NCharacterSelectScreenBg._defaultBgScale * NCharacterSelectScreenBg._narrowBgScale, NCharacterSelectScreenBg._defaultBgScale));
    else
      this.Scale = Vector2.op_Multiply(Vector2.One, NCharacterSelectScreenBg._defaultBgScale);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCharacterSelectScreenBg.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreenBg.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCharacterSelectScreenBg.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCharacterSelectScreenBg.MethodName.OnWindowChange) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnWindowChange();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCharacterSelectScreenBg.MethodName._Ready) || StringName.op_Equality(ref method, NCharacterSelectScreenBg.MethodName.OnWindowChange) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NCharacterSelectScreenBg.PropertyName._window))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._window = VariantUtils.ConvertTo<Window>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NCharacterSelectScreenBg.PropertyName._window))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Window>(ref this._window);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreenBg.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCharacterSelectScreenBg.PropertyName._window, Variant.From<Window>(ref this._window));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NCharacterSelectScreenBg.PropertyName._window, ref variant))
      return;
    this._window = ((Variant) ref variant).As<Window>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
  }

  public class SignalName : Control.SignalName
  {
  }
}
