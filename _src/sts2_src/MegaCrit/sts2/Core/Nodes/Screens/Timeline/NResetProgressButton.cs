// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.NResetProgressButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/NResetProgressButton.cs")]
public class NResetProgressButton : Control
{
  private MegaLabel _disclaimer;

  public override void _Ready()
  {
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnMouseEntered)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnMouseExited)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.GuiInput, Callable.From<InputEvent>(new Action<InputEvent>(this.OnGuiInput)), 0U);
    this._disclaimer = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CloseScreenDisclaimer"));
  }

  private void OnMouseExited() => this.Scale = Vector2.One;

  private void OnMouseEntered() => this.Scale = Vector2.op_Multiply(Vector2.One, 1.1f);

  private void OnGuiInput(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L || eventMouseButton.Pressed)
      return;
    SaveManager.Instance.ResetTimelineProgress();
    ((CanvasItem) this._disclaimer).Visible = true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NResetProgressButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResetProgressButton.MethodName.OnMouseExited, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResetProgressButton.MethodName.OnMouseEntered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResetProgressButton.MethodName.OnGuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NResetProgressButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResetProgressButton.MethodName.OnMouseExited) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnMouseExited();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResetProgressButton.MethodName.OnMouseEntered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnMouseEntered();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NResetProgressButton.MethodName.OnGuiInput) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnGuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NResetProgressButton.MethodName._Ready) || StringName.op_Equality(ref method, NResetProgressButton.MethodName.OnMouseExited) || StringName.op_Equality(ref method, NResetProgressButton.MethodName.OnMouseEntered) || StringName.op_Equality(ref method, NResetProgressButton.MethodName.OnGuiInput) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NResetProgressButton.PropertyName._disclaimer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._disclaimer = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NResetProgressButton.PropertyName._disclaimer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._disclaimer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NResetProgressButton.PropertyName._disclaimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NResetProgressButton.PropertyName._disclaimer, Variant.From<MegaLabel>(ref this._disclaimer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NResetProgressButton.PropertyName._disclaimer, ref variant))
      return;
    this._disclaimer = ((Variant) ref variant).As<MegaLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnMouseExited = StringName.op_Implicit(nameof (OnMouseExited));
    public static readonly StringName OnMouseEntered = StringName.op_Implicit(nameof (OnMouseEntered));
    public static readonly StringName OnGuiInput = StringName.op_Implicit(nameof (OnGuiInput));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _disclaimer = StringName.op_Implicit(nameof (_disclaimer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
