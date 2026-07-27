// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NTargetingArrow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NTargetingArrow.cs")]
public class NTargetingArrow : Node2D
{
  private const int _segmentCount = 19;
  private Vector2 _fromPos;
  private Control? _fromControl;
  private Vector2 _toPosition;
  private Vector2? _currentArrowPos;
  private static readonly string _segmentHeadPath = ImageHelper.GetImagePath("ui/combat/targeting_arrow_head.png");
  private static readonly string _segmentBlockPath = ImageHelper.GetImagePath("ui/combat/targeting_arrow_segment.png");
  private Sprite2D[] _segments = new Sprite2D[19];
  private Sprite2D _arrowHead;
  private Tween? _arrowHeadTween;
  private bool _initialized;
  private bool _followMouse;
  private const float _segmentScaleStart = 0.28f;
  private const float _segmentScaleEnd = 0.42f;
  private static readonly Vector2 _arrowHeadDefaultScale = Vector2.op_Multiply(Vector2.One, 0.95f);
  private static readonly Vector2 _arrowHeadHoverScale = Vector2.op_Multiply(Vector2.One, 1.05f);

  private Vector2 From
  {
    get
    {
      Control fromControl = this._fromControl;
      return fromControl == null ? this._fromPos : fromControl.GlobalPosition;
    }
  }

  private static Texture2D SegmentHead
  {
    get => PreloadManager.Cache.GetTexture2D(NTargetingArrow._segmentHeadPath);
  }

  private static Texture2D SegmentBlock
  {
    get => PreloadManager.Cache.GetTexture2D(NTargetingArrow._segmentBlockPath);
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NTargetingArrow._segmentHeadPath,
        NTargetingArrow._segmentBlockPath
      });
    }
  }

  public override void _Ready()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    for (int index = 0; index < 19; ++index)
    {
      this._segments[index] = new Sprite2D();
      this._segments[index].Texture = NTargetingArrow.SegmentBlock;
      ((Node) this).AddChildSafely((Node) this._segments[index]);
    }
    this._arrowHead = new Sprite2D();
    this._arrowHead.Texture = NTargetingArrow.SegmentHead;
    ((Node) this).AddChildSafely((Node) this._arrowHead);
    this.StopDrawing();
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).Visible)
      return;
    if (this._followMouse)
    {
      this.UpdateDrawingTo(((Node) this).GetViewport().GetMousePosition());
      this.UpdateArrowPosition(this._toPosition);
    }
    else
    {
      if (!this._currentArrowPos.HasValue)
        return;
      Vector2 vector2 = this._currentArrowPos.Value;
      this._currentArrowPos = new Vector2?(((Vector2) ref vector2).Lerp(this._toPosition, (float) delta * 14f));
      this.UpdateArrowPosition(this._currentArrowPos.Value);
    }
  }

  private void UpdateArrowPosition(Vector2 targetPos)
  {
    Sprite2D arrowHead1 = this._arrowHead;
    Vector2 vector2_1 = targetPos;
    Vector2 vector2_2 = new Vector2(0.0f, 88f);
    Vector2 vector2_3 = ((Vector2) ref vector2_2).Rotated(((Node2D) this._arrowHead).Rotation);
    Vector2 vector2_4 = Vector2.op_Addition(vector2_1, vector2_3);
    ((Node2D) arrowHead1).Position = vector2_4;
    Vector2 vector2_5 = targetPos;
    Vector2 vector2_6 = new Vector2(0.0f, 40f);
    Vector2 vector2_7 = ((Vector2) ref vector2_6).Rotated(((Node2D) this._arrowHead).Rotation);
    Vector2 finalPos = Vector2.op_Addition(vector2_5, vector2_7);
    Vector2 zero = Vector2.Zero;
    zero.X = this.From.X - (float) (((double) ((Node2D) this._arrowHead).Position.X - (double) this.From.X) * 0.25);
    zero.Y = (double) this.From.Y <= 540.0 ? (float) ((double) ((Node2D) this._arrowHead).Position.Y * 0.75 + (double) this.From.Y * 0.25) : ((Node2D) this._arrowHead).Position.Y + (float) (((double) ((Node2D) this._arrowHead).Position.Y - (double) this.From.Y) * 0.5);
    Sprite2D arrowHead2 = this._arrowHead;
    Vector2 vector2_8 = Vector2.op_Subtraction(targetPos, zero);
    double num = (double) ((Vector2) ref vector2_8).Angle() + 1.5707963705062866;
    ((Node2D) arrowHead2).Rotation = (float) num;
    this.UpdateSegments(this.From, finalPos, zero);
  }

  public void SetHighlightingOn(bool isEnemy)
  {
    this._arrowHeadTween?.Kill();
    this._arrowHeadTween = ((Node) this).CreateTween();
    this._arrowHeadTween.TweenProperty((GodotObject) this._arrowHead, NodePath.op_Implicit("scale"), Variant.op_Implicit(NTargetingArrow._arrowHeadHoverScale), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 6L);
    ((CanvasItem) this).Modulate = isEnemy ? StsColors.targetingArrowEnemy : StsColors.targetingArrowAlly;
  }

  public void SetHighlightingOff()
  {
    this._arrowHeadTween?.Kill();
    ((Node2D) this._arrowHead).Scale = NTargetingArrow._arrowHeadDefaultScale;
    ((CanvasItem) this).Modulate = Colors.White;
  }

  private void UpdateSegments(Vector2 initialPos, Vector2 finalPos, Vector2 controlPoint)
  {
    Vector2 vector2;
    for (int index = 0; index < 19; ++index)
    {
      ((Node2D) this._segments[index]).Scale = Vector2.op_Multiply(Vector2.One, Mathf.Lerp(0.28f, 0.42f, (float) ((double) index * 2.0 / 19.0)));
      ((Node2D) this._segments[index]).Position = MathHelper.BezierCurve(initialPos, finalPos, controlPoint, (float) index / 20f);
      if (index == 0)
      {
        ((Node2D) this._segments[index]).Rotation = (double) ((Node2D) this._segments[index]).GlobalPosition.Y > 540.0 ? 0.0f : 3.14159274f;
      }
      else
      {
        Sprite2D segment = this._segments[index];
        vector2 = Vector2.op_Subtraction(((Node2D) this._segments[index]).Position, ((Node2D) this._segments[index - 1]).Position);
        double num = (double) ((Vector2) ref vector2).Angle() + 1.5707963705062866;
        ((Node2D) segment).Rotation = (float) num;
      }
    }
    Sprite2D segment1 = this._segments[0];
    vector2 = Vector2.op_Subtraction(((Node2D) this._segments[0]).Position, ((Node2D) this._segments[1]).Position);
    double num1 = (double) ((Vector2) ref vector2).Angle() - 1.5707963705062866;
    ((Node2D) segment1).Rotation = (float) num1;
  }

  public void StartDrawingFrom(Vector2 from, bool usingController)
  {
    this._followMouse = !usingController;
    if (this._followMouse)
      Input.MouseMode = (Input.MouseModeEnum) 1L;
    this._fromPos = from;
    if (usingController)
    {
      this._currentArrowPos = new Vector2?(from);
      this._toPosition = from;
    }
    else
      this._currentArrowPos = new Vector2?();
    ((CanvasItem) this).Visible = !NCombatUi.IsDebugHideTargetingUi;
  }

  public void StartDrawingFrom(Control control, bool usingController)
  {
    this._followMouse = !usingController;
    ((CanvasItem) this).ZIndex = ((CanvasItem) control).ZIndex + 1;
    this._fromControl = control;
    if (usingController)
    {
      this._currentArrowPos = new Vector2?(control.GlobalPosition);
      this._toPosition = control.GlobalPosition;
    }
    else
    {
      Input.MouseMode = (Input.MouseModeEnum) 1L;
      this._currentArrowPos = new Vector2?();
    }
    ((CanvasItem) this).Visible = !NCombatUi.IsDebugHideTargetingUi;
  }

  public void StopDrawing()
  {
    if (this._followMouse)
      Input.MouseMode = (Input.MouseModeEnum) 0L;
    this._fromControl = (Control) null;
    this._currentArrowPos = new Vector2?();
    ((CanvasItem) this).Visible = false;
    this.SetHighlightingOff();
  }

  public override void _ExitTree()
  {
    if (!this._followMouse)
      return;
    Input.MouseMode = (Input.MouseModeEnum) 0L;
  }

  public void UpdateDrawingTo(Vector2 position)
  {
    this._toPosition = position;
    if (this._followMouse || this._currentArrowPos.HasValue)
      return;
    this._currentArrowPos = new Vector2?(this._toPosition);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NTargetingArrow.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.UpdateArrowPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetPos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.SetHighlightingOn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isEnemy"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.SetHighlightingOff, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.UpdateSegments, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("initialPos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("finalPos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("controlPoint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.StartDrawingFrom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("from"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("usingController"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.StopDrawing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTargetingArrow.MethodName.UpdateDrawingTo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName.UpdateArrowPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateArrowPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName.SetHighlightingOn) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetHighlightingOn(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName.SetHighlightingOff) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetHighlightingOff();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName.UpdateSegments) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.UpdateSegments(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName.StartDrawingFrom) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.StartDrawingFrom(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName.StopDrawing) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopDrawing();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTargetingArrow.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTargetingArrow.MethodName.UpdateDrawingTo) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateDrawingTo(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTargetingArrow.MethodName._Ready) || StringName.op_Equality(ref method, NTargetingArrow.MethodName._Process) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.UpdateArrowPosition) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.SetHighlightingOn) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.SetHighlightingOff) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.UpdateSegments) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.StartDrawingFrom) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.StopDrawing) || StringName.op_Equality(ref method, NTargetingArrow.MethodName._ExitTree) || StringName.op_Equality(ref method, NTargetingArrow.MethodName.UpdateDrawingTo) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._fromPos))
    {
      this._fromPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._fromControl))
    {
      this._fromControl = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._toPosition))
    {
      this._toPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._segments))
    {
      this._segments = VariantUtils.ConvertToSystemArrayOfGodotObject<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._arrowHead))
    {
      this._arrowHead = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._arrowHeadTween))
    {
      this._arrowHeadTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._initialized))
    {
      this._initialized = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTargetingArrow.PropertyName._followMouse))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._followMouse = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName.From))
    {
      ref godot_variant local = ref value;
      Vector2 from1 = this.From;
      godot_variant from2 = VariantUtils.CreateFrom<Vector2>(ref from1);
      local = from2;
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._fromPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._fromPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._fromControl))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._fromControl);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._toPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._toPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._segments))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._segments);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._arrowHead))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._arrowHead);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._arrowHeadTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._arrowHeadTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NTargetingArrow.PropertyName._initialized))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._initialized);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTargetingArrow.PropertyName._followMouse))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._followMouse);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NTargetingArrow.PropertyName._fromPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTargetingArrow.PropertyName._fromControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NTargetingArrow.PropertyName._toPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NTargetingArrow.PropertyName.From, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NTargetingArrow.PropertyName._segments, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTargetingArrow.PropertyName._arrowHead, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTargetingArrow.PropertyName._arrowHeadTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTargetingArrow.PropertyName._initialized, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTargetingArrow.PropertyName._followMouse, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTargetingArrow.PropertyName._fromPos, Variant.From<Vector2>(ref this._fromPos));
    info.AddProperty(NTargetingArrow.PropertyName._fromControl, Variant.From<Control>(ref this._fromControl));
    info.AddProperty(NTargetingArrow.PropertyName._toPosition, Variant.From<Vector2>(ref this._toPosition));
    info.AddProperty(NTargetingArrow.PropertyName._segments, Variant.CreateFrom((GodotObject[]) this._segments));
    info.AddProperty(NTargetingArrow.PropertyName._arrowHead, Variant.From<Sprite2D>(ref this._arrowHead));
    info.AddProperty(NTargetingArrow.PropertyName._arrowHeadTween, Variant.From<Tween>(ref this._arrowHeadTween));
    info.AddProperty(NTargetingArrow.PropertyName._initialized, Variant.From<bool>(ref this._initialized));
    info.AddProperty(NTargetingArrow.PropertyName._followMouse, Variant.From<bool>(ref this._followMouse));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._fromPos, ref variant1))
      this._fromPos = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._fromControl, ref variant2))
      this._fromControl = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._toPosition, ref variant3))
      this._toPosition = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._segments, ref variant4))
      this._segments = ((Variant) ref variant4).AsGodotObjectArray<Sprite2D>();
    Variant variant5;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._arrowHead, ref variant5))
      this._arrowHead = ((Variant) ref variant5).As<Sprite2D>();
    Variant variant6;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._arrowHeadTween, ref variant6))
      this._arrowHeadTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NTargetingArrow.PropertyName._initialized, ref variant7))
      this._initialized = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (!info.TryGetProperty(NTargetingArrow.PropertyName._followMouse, ref variant8))
      return;
    this._followMouse = ((Variant) ref variant8).As<bool>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName UpdateArrowPosition = StringName.op_Implicit(nameof (UpdateArrowPosition));
    public static readonly StringName SetHighlightingOn = StringName.op_Implicit(nameof (SetHighlightingOn));
    public static readonly StringName SetHighlightingOff = StringName.op_Implicit(nameof (SetHighlightingOff));
    public static readonly StringName UpdateSegments = StringName.op_Implicit(nameof (UpdateSegments));
    public static readonly StringName StartDrawingFrom = StringName.op_Implicit(nameof (StartDrawingFrom));
    public static readonly StringName StopDrawing = StringName.op_Implicit(nameof (StopDrawing));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateDrawingTo = StringName.op_Implicit(nameof (UpdateDrawingTo));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName From = StringName.op_Implicit(nameof (From));
    public static readonly StringName _fromPos = StringName.op_Implicit(nameof (_fromPos));
    public static readonly StringName _fromControl = StringName.op_Implicit(nameof (_fromControl));
    public static readonly StringName _toPosition = StringName.op_Implicit(nameof (_toPosition));
    public static readonly StringName _segments = StringName.op_Implicit(nameof (_segments));
    public static readonly StringName _arrowHead = StringName.op_Implicit(nameof (_arrowHead));
    public static readonly StringName _arrowHeadTween = StringName.op_Implicit(nameof (_arrowHeadTween));
    public static readonly StringName _initialized = StringName.op_Implicit(nameof (_initialized));
    public static readonly StringName _followMouse = StringName.op_Implicit(nameof (_followMouse));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
