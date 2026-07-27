// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NMegaLineEdit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Platform;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[ScriptPath("res://src/Core/Nodes/GodotExtensions/NMegaLineEdit.cs")]
public class NMegaLineEdit : LineEdit
{
  public override void _Ready()
  {
    ((Control) this).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.LineEdit.Font);
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    ((Control) this)._GuiInput(inputEvent);
    if (inputEvent is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == 1L && ((InputEvent) eventMouseButton).IsPressed())
      this.OpenKeyboard();
    if (inputEvent.IsActionPressed(MegaInput.select, false, false))
      this.OpenKeyboard();
    if (!inputEvent.IsActionPressed(MegaInput.cancel, false, false) || !this.IsEditing())
      return;
    this.Unedit();
    ((Node) this).GetViewport()?.SetInputAsHandled();
    PlatformUtil.CloseVirtualKeyboard();
  }

  private void OpenKeyboard()
  {
    this.Edit();
    PlatformUtil.OpenVirtualKeyboard();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMegaLineEdit.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaLineEdit.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMegaLineEdit.MethodName.OpenKeyboard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMegaLineEdit.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaLineEdit.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Control) this)._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMegaLineEdit.MethodName.OpenKeyboard) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OpenKeyboard();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMegaLineEdit.MethodName._Ready) || StringName.op_Equality(ref method, NMegaLineEdit.MethodName._GuiInput) || StringName.op_Equality(ref method, NMegaLineEdit.MethodName.OpenKeyboard) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : LineEdit.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName OpenKeyboard = StringName.op_Implicit(nameof (OpenKeyboard));
  }

  public class PropertyName : LineEdit.PropertyName
  {
  }

  public class SignalName : LineEdit.SignalName
  {
  }
}
