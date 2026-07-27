// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMouseHeldMapDrawingInput
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMouseHeldMapDrawingInput.cs")]
public class NMouseHeldMapDrawingInput : NMapDrawingInput
{
  private MouseButton ListeningButton
  {
    get => this.DrawingMode != DrawingMode.Drawing ? (MouseButton) 3L : (MouseButton) 2L;
  }

  public override void _Ready()
  {
    base._Ready();
    NMapDrawings drawings = this._drawings;
    Transform2D globalTransform = ((CanvasItem) this._drawings).GetGlobalTransform();
    Vector2 position = Transform2D.op_Multiply(((Transform2D) ref globalTransform).Inverse(), ((CanvasItem) this).GetGlobalMousePosition());
    DrawingMode? overrideDrawingMode = new DrawingMode?();
    drawings.BeginLineLocal(position, overrideDrawingMode);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    this.ProcessMouseDrawingEvent(inputEvent);
    if (!(inputEvent is InputEventMouseMotion eventMouseMotion) || !this._drawings.IsLocalDrawing())
      return;
    NMapDrawings drawings = this._drawings;
    Transform2D globalTransform = ((CanvasItem) this._drawings).GetGlobalTransform();
    Vector2 position = Transform2D.op_Multiply(((Transform2D) ref globalTransform).Inverse(), ((InputEventMouse) eventMouseMotion).GlobalPosition);
    drawings.UpdateCurrentLinePositionLocal(position);
  }

  private void ProcessMouseDrawingEvent(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != this.ListeningButton || ((InputEvent) eventMouseButton).IsPressed())
      return;
    this.StopDrawing();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMouseHeldMapDrawingInput.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMouseHeldMapDrawingInput.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMouseHeldMapDrawingInput.MethodName.ProcessMouseDrawingEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NMouseHeldMapDrawingInput.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMouseHeldMapDrawingInput.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMouseHeldMapDrawingInput.MethodName.ProcessMouseDrawingEvent) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.ProcessMouseDrawingEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMouseHeldMapDrawingInput.MethodName._Ready) || StringName.op_Equality(ref method, NMouseHeldMapDrawingInput.MethodName._Input) || StringName.op_Equality(ref method, NMouseHeldMapDrawingInput.MethodName.ProcessMouseDrawingEvent) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NMouseHeldMapDrawingInput.PropertyName.ListeningButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    ref godot_variant local = ref value;
    MouseButton listeningButton = this.ListeningButton;
    godot_variant from = VariantUtils.CreateFrom<MouseButton>(ref listeningButton);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMouseHeldMapDrawingInput.PropertyName.ListeningButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NMapDrawingInput.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ProcessMouseDrawingEvent = StringName.op_Implicit(nameof (ProcessMouseDrawingEvent));
  }

  public new class PropertyName : NMapDrawingInput.PropertyName
  {
    public static readonly StringName ListeningButton = StringName.op_Implicit(nameof (ListeningButton));
  }

  public new class SignalName : NMapDrawingInput.SignalName
  {
  }
}
