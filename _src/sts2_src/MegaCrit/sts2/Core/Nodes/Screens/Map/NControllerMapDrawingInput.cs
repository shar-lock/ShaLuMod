// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NControllerMapDrawingInput
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NControllerMapDrawingInput.cs")]
public class NControllerMapDrawingInput : NMapDrawingInput
{
  private const string _scenePath = "res://scenes/screens/map/controller_map_drawing_input.tscn";
  private Vector2 _eraserIconPos = new Vector2(-34f, -76f);
  private Vector2 _drawingIconPos = new Vector2(-10f, -76f);
  private bool _isPressed;
  private Texture2D _cursorTex;
  private Texture2D _cursorTiltedTex;
  private Control _cursor;
  private Vector2 _direction;

  public static NMapDrawingInput Create()
  {
    return (NMapDrawingInput) PreloadManager.Cache.GetScene("res://scenes/screens/map/controller_map_drawing_input.tscn").Instantiate<NControllerMapDrawingInput>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    base._Ready();
    this._cursor = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Cursor"));
    this.TryGrabFocus();
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.GuiFocusChanged, Callable.From<Control>((Action<Control>) (_ => this.StopDrawing())), 0U);
    if (this.DrawingMode == DrawingMode.Drawing)
    {
      this._cursorTex = (Texture2D) ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill.png"));
      this._cursorTiltedTex = (Texture2D) ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_quill_tilted.png"));
    }
    else
    {
      this._cursorTex = (Texture2D) ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser.png"));
      this._cursorTiltedTex = (Texture2D) ImageTexture.CreateFromImage(PreloadManager.Cache.GetAsset<Image>("res://images/packed/common_ui/cursor_eraser_tilted.png"));
    }
    ((Node) this._cursor).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect")).Texture = this._cursorTex;
    ((Control) ((Node) this._cursor).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"))).Position = this.DrawingMode == DrawingMode.Drawing ? this._drawingIconPos : this._eraserIconPos;
  }

  public override void _Process(double delta)
  {
    Transform2D globalTransform;
    if (Input.IsActionPressed(MegaInput.select, false))
    {
      if (!this._isPressed)
      {
        NMapDrawings drawings = this._drawings;
        globalTransform = ((CanvasItem) this._drawings).GetGlobalTransform();
        Vector2 position = Transform2D.op_Multiply(((Transform2D) ref globalTransform).Inverse(), this._cursor.GlobalPosition);
        DrawingMode? overrideDrawingMode = new DrawingMode?();
        drawings.BeginLineLocal(position, overrideDrawingMode);
        ((Node) this._cursor).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect")).Texture = this._cursorTiltedTex;
        this._isPressed = true;
      }
    }
    else if (this._isPressed)
    {
      this._drawings.StopLineLocal();
      ((Node) this._cursor).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect")).Texture = this._cursorTex;
      this._isPressed = false;
    }
    this._direction = NControllerManager.Instance.GetLeftAnalogStickDirection();
    if ((double) ((Vector2) ref this._direction).Length() < 0.10000000149011612)
      this._direction = Vector2.op_Addition(this._direction, Input.GetVector(Controller.dPadLeft, Controller.dPadRight, Controller.dPadUp, Controller.dPadDown, -1f));
    if ((double) ((Vector2) ref this._direction).Length() <= 0.0)
      return;
    Control cursor1 = this._cursor;
    cursor1.GlobalPosition = Vector2.op_Addition(cursor1.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(this._direction, 700f), (float) delta));
    Control cursor2 = this._cursor;
    Vector2 globalPosition = this._cursor.GlobalPosition;
    Vector2 vector2 = ((Vector2) ref globalPosition).Clamp(NGame.Instance.GlobalPosition, Vector2.op_Addition(NGame.Instance.GlobalPosition, NGame.Instance.Size));
    cursor2.GlobalPosition = vector2;
    if (!this._drawings.IsLocalDrawing())
      return;
    NMapDrawings drawings1 = this._drawings;
    globalTransform = ((CanvasItem) this._drawings).GetGlobalTransform();
    Vector2 position1 = Transform2D.op_Multiply(((Transform2D) ref globalTransform).Inverse(), this._cursor.GlobalPosition);
    drawings1.UpdateCurrentLinePositionLocal(position1);
  }

  public override void _Input(InputEvent input)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !input.IsActionPressed(MegaInput.cancel, false, false))
      return;
    this.StopDrawing();
    ActiveScreenContext.Instance.FocusOnDefaultControl();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NControllerMapDrawingInput.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerMapDrawingInput.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerMapDrawingInput.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerMapDrawingInput.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMapDrawingInput nmapDrawingInput = NControllerMapDrawingInput.Create();
      ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName._Input) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMapDrawingInput nmapDrawingInput = NControllerMapDrawingInput.Create();
      ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName.Create) || StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName._Ready) || StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName._Process) || StringName.op_Equality(ref method, NControllerMapDrawingInput.MethodName._Input) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._eraserIconPos))
    {
      this._eraserIconPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._drawingIconPos))
    {
      this._drawingIconPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._isPressed))
    {
      this._isPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._cursorTex))
    {
      this._cursorTex = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._cursorTiltedTex))
    {
      this._cursorTiltedTex = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._cursor))
    {
      this._cursor = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._direction))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._direction = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._eraserIconPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._eraserIconPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._drawingIconPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._drawingIconPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._isPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isPressed);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._cursorTex))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._cursorTex);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._cursorTiltedTex))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._cursorTiltedTex);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._cursor))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cursor);
      return true;
    }
    if (!StringName.op_Equality(ref name, NControllerMapDrawingInput.PropertyName._direction))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._direction);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NControllerMapDrawingInput.PropertyName._eraserIconPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NControllerMapDrawingInput.PropertyName._drawingIconPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NControllerMapDrawingInput.PropertyName._isPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NControllerMapDrawingInput.PropertyName._cursorTex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NControllerMapDrawingInput.PropertyName._cursorTiltedTex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NControllerMapDrawingInput.PropertyName._cursor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NControllerMapDrawingInput.PropertyName._direction, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NControllerMapDrawingInput.PropertyName._eraserIconPos, Variant.From<Vector2>(ref this._eraserIconPos));
    info.AddProperty(NControllerMapDrawingInput.PropertyName._drawingIconPos, Variant.From<Vector2>(ref this._drawingIconPos));
    info.AddProperty(NControllerMapDrawingInput.PropertyName._isPressed, Variant.From<bool>(ref this._isPressed));
    info.AddProperty(NControllerMapDrawingInput.PropertyName._cursorTex, Variant.From<Texture2D>(ref this._cursorTex));
    info.AddProperty(NControllerMapDrawingInput.PropertyName._cursorTiltedTex, Variant.From<Texture2D>(ref this._cursorTiltedTex));
    info.AddProperty(NControllerMapDrawingInput.PropertyName._cursor, Variant.From<Control>(ref this._cursor));
    info.AddProperty(NControllerMapDrawingInput.PropertyName._direction, Variant.From<Vector2>(ref this._direction));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._eraserIconPos, ref variant1))
      this._eraserIconPos = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._drawingIconPos, ref variant2))
      this._drawingIconPos = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._isPressed, ref variant3))
      this._isPressed = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._cursorTex, ref variant4))
      this._cursorTex = ((Variant) ref variant4).As<Texture2D>();
    Variant variant5;
    if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._cursorTiltedTex, ref variant5))
      this._cursorTiltedTex = ((Variant) ref variant5).As<Texture2D>();
    Variant variant6;
    if (info.TryGetProperty(NControllerMapDrawingInput.PropertyName._cursor, ref variant6))
      this._cursor = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (!info.TryGetProperty(NControllerMapDrawingInput.PropertyName._direction, ref variant7))
      return;
    this._direction = ((Variant) ref variant7).As<Vector2>();
  }

  public new class MethodName : NMapDrawingInput.MethodName
  {
    public new static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
  }

  public new class PropertyName : NMapDrawingInput.PropertyName
  {
    public static readonly StringName _eraserIconPos = StringName.op_Implicit(nameof (_eraserIconPos));
    public static readonly StringName _drawingIconPos = StringName.op_Implicit(nameof (_drawingIconPos));
    public static readonly StringName _isPressed = StringName.op_Implicit(nameof (_isPressed));
    public static readonly StringName _cursorTex = StringName.op_Implicit(nameof (_cursorTex));
    public static readonly StringName _cursorTiltedTex = StringName.op_Implicit(nameof (_cursorTiltedTex));
    public static readonly StringName _cursor = StringName.op_Implicit(nameof (_cursor));
    public static readonly StringName _direction = StringName.op_Implicit(nameof (_direction));
  }

  public new class SignalName : NMapDrawingInput.SignalName
  {
  }
}
