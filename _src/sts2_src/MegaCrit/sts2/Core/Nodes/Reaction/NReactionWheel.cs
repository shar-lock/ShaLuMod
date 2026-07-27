// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Reaction.NReactionWheel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Reaction;

[ScriptPath("res://src/Core/Nodes/Reaction/NReactionWheel.cs")]
public class NReactionWheel : Control
{
  private static readonly StringName _reactWheel = new StringName("react_wheel");
  private const float _centerRadius = 70f;
  private NReactionWheelWedge _rightWedge;
  private NReactionWheelWedge _downRightWedge;
  private NReactionWheelWedge _downWedge;
  private NReactionWheelWedge _downLeftWedge;
  private NReactionWheelWedge _leftWedge;
  private NReactionWheelWedge _upLeftWedge;
  private NReactionWheelWedge _upWedge;
  private NReactionWheelWedge _upRightWedge;
  private TextureRect _marker;
  private bool _ignoreNextMouseInput;
  private Vector2 _centerPosition;
  private NReactionWheelWedge? _selectedWedge;
  private Player? _localPlayer;

  public override void _Ready()
  {
    this._rightWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("RightWedge"));
    this._downRightWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("DownRightWedge"));
    this._downWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("DownWedge"));
    this._downLeftWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("DownLeftWedge"));
    this._leftWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("LeftWedge"));
    this._upLeftWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("UpLeftWedge"));
    this._upWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("UpWedge"));
    this._upRightWedge = ((Node) this).GetNode<NReactionWheelWedge>(NodePath.op_Implicit("UpRightWedge"));
    this._marker = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Marker"));
    ((CanvasItem) this).Visible = false;
  }

  public override void _EnterTree()
  {
    RunManager.Instance.RunStarted += new Action<RunState>(this.OnRunStarted);
  }

  public override void _ExitTree()
  {
    RunManager.Instance.RunStarted -= new Action<RunState>(this.OnRunStarted);
  }

  public override void _Notification(int what)
  {
    if (!((CanvasItem) this).Visible || what != 2017)
      return;
    ((CanvasItem) this).Visible = false;
  }

  public override void _Input(InputEvent inputEvent)
  {
    bool flag1;
    switch (((Node) this).GetViewport().GuiGetFocusOwner())
    {
      case TextEdit _:
      case LineEdit _:
        flag1 = true;
        break;
      default:
        flag1 = false;
        break;
    }
    bool flag2 = flag1;
    if (!NGame.Instance.ReactionContainer.InMultiplayer)
    {
      if (!((CanvasItem) this).Visible)
        return;
      this.HideWheel();
    }
    else if (inputEvent is InputEventMouseMotion eventMouseMotion)
    {
      if (this._ignoreNextMouseInput)
      {
        this._ignoreNextMouseInput = false;
      }
      else
      {
        if (!((CanvasItem) this).Visible)
          return;
        this.MoveMarker(eventMouseMotion.Relative);
        this._ignoreNextMouseInput = true;
        this.WarpMouseBackToOriginalPosition();
      }
    }
    else if (inputEvent.IsActionPressed(NReactionWheel._reactWheel, false, false) && !flag2)
    {
      ((CanvasItem) this).Visible = true;
      if (this._localPlayer != null)
        this._marker.Texture = (Texture2D) this._localPlayer.Character.MapMarker;
      this._centerPosition = ((Node) this).GetViewport().GetMousePosition();
      ((Control) this._marker).Position = Vector2.op_Multiply(Vector2.op_Subtraction(this.Size, ((Control) this._marker).Size), 0.5f);
      this.GlobalPosition = Vector2.op_Subtraction(this._centerPosition, Vector2.op_Multiply(Vector2.op_Multiply(this.Size, this.Scale), 0.5f));
      Input.MouseMode = (Input.MouseModeEnum) 1L;
    }
    else
    {
      if (!inputEvent.IsActionReleased(NReactionWheel._reactWheel, false) || !((CanvasItem) this).Visible)
        return;
      this.HideWheel();
      this.React();
    }
  }

  private void OnRunStarted(RunState runState)
  {
    this._localPlayer = LocalContext.GetMe((IPlayerCollection) runState);
  }

  private void HideWheel()
  {
    Input.MouseMode = (Input.MouseModeEnum) 0L;
    this.WarpMouseBackToOriginalPosition();
    ((CanvasItem) this).Visible = false;
  }

  private void WarpMouseBackToOriginalPosition()
  {
    Input.WarpMouse(Transform2D.op_Multiply(((CanvasItem) this).GetViewportTransform(), this._centerPosition));
  }

  private void React()
  {
    if (this._selectedWedge == null)
      return;
    NGame.Instance.ReactionContainer.DoLocalReaction(this._selectedWedge.Reaction, this._centerPosition);
  }

  private void MoveMarker(Vector2 relative)
  {
    Vector2 vector2_1 = Vector2.op_Multiply(Vector2.op_Subtraction(this.Size, ((Control) this._marker).Size), 0.5f);
    Vector2 vector2_2 = Vector2.op_Addition(Vector2.op_Subtraction(((Control) this._marker).Position, vector2_1), relative);
    vector2_2 = ((Vector2) ref vector2_2).LimitLength(70f);
    ((Control) this._marker).Position = Vector2.op_Addition(vector2_1, vector2_2);
    float angle = Mathf.Atan2(vector2_2.Y, vector2_2.X);
    ((Control) this._marker).Rotation = angle - 1.57079637f;
    NReactionWheelWedge selectedWedge = this.GetSelectedWedge(angle);
    if (this._selectedWedge == selectedWedge)
      return;
    this._selectedWedge?.OnDeselected();
    this._selectedWedge = selectedWedge;
    this._selectedWedge?.OnSelected();
  }

  private NReactionWheelWedge GetSelectedWedge(float angle)
  {
    switch ((int) ((double) Mathf.Wrap(angle + 0.3926991f, 0.0f, 6.28318548f) / 0.78539818525314331))
    {
      case 0:
        return this._rightWedge;
      case 1:
        return this._downRightWedge;
      case 2:
        return this._downWedge;
      case 3:
        return this._downLeftWedge;
      case 4:
        return this._leftWedge;
      case 5:
        return this._upLeftWedge;
      case 6:
        return this._upWedge;
      case 7:
        return this._upRightWedge;
      default:
        throw new InvalidOperationException();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NReactionWheel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName.HideWheel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName.WarpMouseBackToOriginalPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName.React, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName.MoveMarker, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("relative"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NReactionWheel.MethodName.GetSelectedWedge, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("TextureRect"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("angle"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName.HideWheel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideWheel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName.WarpMouseBackToOriginalPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.WarpMouseBackToOriginalPosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName.React) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.React();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionWheel.MethodName.MoveMarker) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.MoveMarker(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NReactionWheel.MethodName.GetSelectedWedge) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NReactionWheelWedge selectedWedge = this.GetSelectedWedge(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<NReactionWheelWedge>(ref selectedWedge);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NReactionWheel.MethodName._Ready) || StringName.op_Equality(ref method, NReactionWheel.MethodName._EnterTree) || StringName.op_Equality(ref method, NReactionWheel.MethodName._ExitTree) || StringName.op_Equality(ref method, NReactionWheel.MethodName._Notification) || StringName.op_Equality(ref method, NReactionWheel.MethodName._Input) || StringName.op_Equality(ref method, NReactionWheel.MethodName.HideWheel) || StringName.op_Equality(ref method, NReactionWheel.MethodName.WarpMouseBackToOriginalPosition) || StringName.op_Equality(ref method, NReactionWheel.MethodName.React) || StringName.op_Equality(ref method, NReactionWheel.MethodName.MoveMarker) || StringName.op_Equality(ref method, NReactionWheel.MethodName.GetSelectedWedge) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._rightWedge))
    {
      this._rightWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._downRightWedge))
    {
      this._downRightWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._downWedge))
    {
      this._downWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._downLeftWedge))
    {
      this._downLeftWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._leftWedge))
    {
      this._leftWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._upLeftWedge))
    {
      this._upLeftWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._upWedge))
    {
      this._upWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._upRightWedge))
    {
      this._upRightWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._marker))
    {
      this._marker = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._ignoreNextMouseInput))
    {
      this._ignoreNextMouseInput = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._centerPosition))
    {
      this._centerPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NReactionWheel.PropertyName._selectedWedge))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._selectedWedge = VariantUtils.ConvertTo<NReactionWheelWedge>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._rightWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._rightWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._downRightWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._downRightWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._downWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._downWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._downLeftWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._downLeftWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._leftWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._leftWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._upLeftWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._upLeftWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._upWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._upWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._upRightWedge))
    {
      value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._upRightWedge);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._marker))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._marker);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._ignoreNextMouseInput))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._ignoreNextMouseInput);
      return true;
    }
    if (StringName.op_Equality(ref name, NReactionWheel.PropertyName._centerPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._centerPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NReactionWheel.PropertyName._selectedWedge))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NReactionWheelWedge>(ref this._selectedWedge);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._rightWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._downRightWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._downWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._downLeftWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._leftWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._upLeftWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._upWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._upRightWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._marker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NReactionWheel.PropertyName._ignoreNextMouseInput, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NReactionWheel.PropertyName._centerPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NReactionWheel.PropertyName._selectedWedge, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NReactionWheel.PropertyName._rightWedge, Variant.From<NReactionWheelWedge>(ref this._rightWedge));
    info.AddProperty(NReactionWheel.PropertyName._downRightWedge, Variant.From<NReactionWheelWedge>(ref this._downRightWedge));
    info.AddProperty(NReactionWheel.PropertyName._downWedge, Variant.From<NReactionWheelWedge>(ref this._downWedge));
    info.AddProperty(NReactionWheel.PropertyName._downLeftWedge, Variant.From<NReactionWheelWedge>(ref this._downLeftWedge));
    info.AddProperty(NReactionWheel.PropertyName._leftWedge, Variant.From<NReactionWheelWedge>(ref this._leftWedge));
    info.AddProperty(NReactionWheel.PropertyName._upLeftWedge, Variant.From<NReactionWheelWedge>(ref this._upLeftWedge));
    info.AddProperty(NReactionWheel.PropertyName._upWedge, Variant.From<NReactionWheelWedge>(ref this._upWedge));
    info.AddProperty(NReactionWheel.PropertyName._upRightWedge, Variant.From<NReactionWheelWedge>(ref this._upRightWedge));
    info.AddProperty(NReactionWheel.PropertyName._marker, Variant.From<TextureRect>(ref this._marker));
    info.AddProperty(NReactionWheel.PropertyName._ignoreNextMouseInput, Variant.From<bool>(ref this._ignoreNextMouseInput));
    info.AddProperty(NReactionWheel.PropertyName._centerPosition, Variant.From<Vector2>(ref this._centerPosition));
    info.AddProperty(NReactionWheel.PropertyName._selectedWedge, Variant.From<NReactionWheelWedge>(ref this._selectedWedge));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NReactionWheel.PropertyName._rightWedge, ref variant1))
      this._rightWedge = ((Variant) ref variant1).As<NReactionWheelWedge>();
    Variant variant2;
    if (info.TryGetProperty(NReactionWheel.PropertyName._downRightWedge, ref variant2))
      this._downRightWedge = ((Variant) ref variant2).As<NReactionWheelWedge>();
    Variant variant3;
    if (info.TryGetProperty(NReactionWheel.PropertyName._downWedge, ref variant3))
      this._downWedge = ((Variant) ref variant3).As<NReactionWheelWedge>();
    Variant variant4;
    if (info.TryGetProperty(NReactionWheel.PropertyName._downLeftWedge, ref variant4))
      this._downLeftWedge = ((Variant) ref variant4).As<NReactionWheelWedge>();
    Variant variant5;
    if (info.TryGetProperty(NReactionWheel.PropertyName._leftWedge, ref variant5))
      this._leftWedge = ((Variant) ref variant5).As<NReactionWheelWedge>();
    Variant variant6;
    if (info.TryGetProperty(NReactionWheel.PropertyName._upLeftWedge, ref variant6))
      this._upLeftWedge = ((Variant) ref variant6).As<NReactionWheelWedge>();
    Variant variant7;
    if (info.TryGetProperty(NReactionWheel.PropertyName._upWedge, ref variant7))
      this._upWedge = ((Variant) ref variant7).As<NReactionWheelWedge>();
    Variant variant8;
    if (info.TryGetProperty(NReactionWheel.PropertyName._upRightWedge, ref variant8))
      this._upRightWedge = ((Variant) ref variant8).As<NReactionWheelWedge>();
    Variant variant9;
    if (info.TryGetProperty(NReactionWheel.PropertyName._marker, ref variant9))
      this._marker = ((Variant) ref variant9).As<TextureRect>();
    Variant variant10;
    if (info.TryGetProperty(NReactionWheel.PropertyName._ignoreNextMouseInput, ref variant10))
      this._ignoreNextMouseInput = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (info.TryGetProperty(NReactionWheel.PropertyName._centerPosition, ref variant11))
      this._centerPosition = ((Variant) ref variant11).As<Vector2>();
    Variant variant12;
    if (!info.TryGetProperty(NReactionWheel.PropertyName._selectedWedge, ref variant12))
      return;
    this._selectedWedge = ((Variant) ref variant12).As<NReactionWheelWedge>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName HideWheel = StringName.op_Implicit(nameof (HideWheel));
    public static readonly StringName WarpMouseBackToOriginalPosition = StringName.op_Implicit(nameof (WarpMouseBackToOriginalPosition));
    public static readonly StringName React = StringName.op_Implicit(nameof (React));
    public static readonly StringName MoveMarker = StringName.op_Implicit(nameof (MoveMarker));
    public static readonly StringName GetSelectedWedge = StringName.op_Implicit(nameof (GetSelectedWedge));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _rightWedge = StringName.op_Implicit(nameof (_rightWedge));
    public static readonly StringName _downRightWedge = StringName.op_Implicit(nameof (_downRightWedge));
    public static readonly StringName _downWedge = StringName.op_Implicit(nameof (_downWedge));
    public static readonly StringName _downLeftWedge = StringName.op_Implicit(nameof (_downLeftWedge));
    public static readonly StringName _leftWedge = StringName.op_Implicit(nameof (_leftWedge));
    public static readonly StringName _upLeftWedge = StringName.op_Implicit(nameof (_upLeftWedge));
    public static readonly StringName _upWedge = StringName.op_Implicit(nameof (_upWedge));
    public static readonly StringName _upRightWedge = StringName.op_Implicit(nameof (_upRightWedge));
    public static readonly StringName _marker = StringName.op_Implicit(nameof (_marker));
    public static readonly StringName _ignoreNextMouseInput = StringName.op_Implicit(nameof (_ignoreNextMouseInput));
    public static readonly StringName _centerPosition = StringName.op_Implicit(nameof (_centerPosition));
    public static readonly StringName _selectedWedge = StringName.op_Implicit(nameof (_selectedWedge));
  }

  public class SignalName : Control.SignalName
  {
  }
}
