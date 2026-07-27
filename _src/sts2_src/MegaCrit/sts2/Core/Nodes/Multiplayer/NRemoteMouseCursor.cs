// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteMouseCursor
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteMouseCursor.cs")]
public class NRemoteMouseCursor : Control
{
  private const string _scenePath = "ui/multiplayer/remote_mouse_cursor";
  private TextureRect _textureRect;
  private Vector2? _previousPosition;
  private Vector2? _nextPosition;
  private ulong _lastPositionUpdateMsec;
  private Vector2 _defaultHotspot;
  private Vector2 _drawingHotspot;
  private Vector2 _erasingHotspot;
  [Export]
  private Image _defaultCursorImage;
  [Export]
  private Image _tiltedCursorImage;
  [Export]
  private Image _defaultDrawingImage;
  [Export]
  private Image _tiltedDrawingImage;
  [Export]
  private Image _defaultErasingImage;
  [Export]
  private Image _tiltedErasingImage;
  private ImageTexture _defaultCursorTexture;
  private ImageTexture _tiltedCursorTexture;
  private ImageTexture _defaultDrawingTexture;
  private ImageTexture _tiltedDrawingTexture;
  private ImageTexture _defaultErasingTexture;
  private ImageTexture _tiltedErasingTexture;
  private DrawingMode _drawingMode;

  public ulong PlayerId { get; private set; }

  public static NRemoteMouseCursor Create(ulong playerId)
  {
    NRemoteMouseCursor nremoteMouseCursor = PreloadManager.Cache.GetAsset<PackedScene>(SceneHelper.GetScenePath("ui/multiplayer/remote_mouse_cursor")).Instantiate<NRemoteMouseCursor>((PackedScene.GenEditState) 0L);
    nremoteMouseCursor.PlayerId = playerId;
    return nremoteMouseCursor;
  }

  public override void _Ready()
  {
    this._textureRect = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._defaultHotspot = Vector2.op_UnaryNegation(((Control) this._textureRect).Position);
    this._drawingHotspot = NMapDrawings.drawingCursorHotspot;
    this._erasingHotspot = NMapDrawings.erasingCursorHotspot;
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
    this._defaultCursorTexture = ImageTexture.CreateFromImage(this._defaultCursorImage);
    this._tiltedCursorTexture = ImageTexture.CreateFromImage(this._tiltedCursorImage);
    this._defaultDrawingTexture = ImageTexture.CreateFromImage(this._defaultDrawingImage);
    this._tiltedDrawingTexture = ImageTexture.CreateFromImage(this._tiltedDrawingImage);
    this._defaultErasingTexture = ImageTexture.CreateFromImage(this._defaultErasingImage);
    this._tiltedErasingTexture = ImageTexture.CreateFromImage(this._tiltedErasingImage);
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.RefreshSize)), 0U);
  }

  public void SetNextPosition(Vector2 position)
  {
    if (!this._nextPosition.HasValue)
      this._nextPosition = new Vector2?(position);
    this._previousPosition = this._nextPosition;
    this._nextPosition = new Vector2?(position);
    this._lastPositionUpdateMsec = Time.GetTicksMsec();
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 0L;
  }

  public override void _Process(double delta)
  {
    if (!this._previousPosition.HasValue || !this._nextPosition.HasValue)
      return;
    float num = (float) (Time.GetTicksMsec() - this._lastPositionUpdateMsec) / 50f;
    Vector2 vector2 = this._previousPosition.Value;
    this.Position = ((Vector2) ref vector2).Lerp(this._nextPosition.Value, Mathf.Clamp(num, 0.0f, 1f));
    if ((double) num < 1.0)
      return;
    ((Node) this).ProcessMode = (Node.ProcessModeEnum) 4L;
  }

  public void UpdateImage(bool isDown, DrawingMode drawingMode)
  {
    this._textureRect.Texture = this.GetTexture(isDown, drawingMode);
    this._drawingMode = drawingMode;
    this.RefreshSize();
  }

  private Vector2 GetHotspot(DrawingMode drawingMode)
  {
    Vector2 hotspot;
    switch (drawingMode)
    {
      case DrawingMode.None:
        hotspot = Vector2.op_UnaryNegation(this._defaultHotspot);
        break;
      case DrawingMode.Drawing:
        hotspot = Vector2.op_UnaryNegation(this._drawingHotspot);
        break;
      case DrawingMode.Erasing:
        hotspot = Vector2.op_UnaryNegation(this._erasingHotspot);
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) drawingMode);
        break;
    }
    return hotspot;
  }

  private Texture2D GetTexture(bool isDown, DrawingMode drawingMode)
  {
    ImageTexture texture;
    switch (drawingMode)
    {
      case DrawingMode.None:
        texture = isDown ? this._tiltedCursorTexture : this._defaultCursorTexture;
        break;
      case DrawingMode.Drawing:
        texture = isDown ? this._defaultDrawingTexture : this._tiltedDrawingTexture;
        break;
      case DrawingMode.Erasing:
        texture = isDown ? this._defaultErasingTexture : this._tiltedErasingTexture;
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) drawingMode);
        break;
    }
    return (Texture2D) texture;
  }

  public void RefreshSize()
  {
    if (OS.GetName() == "Windows")
    {
      float num = (float) DisplayServer.ScreenGetDpi(-1) / 96f;
      Transform2D stretchTransform = ((Node) this).GetViewport().GetStretchTransform();
      Vector2 scale = ((Transform2D) ref stretchTransform).Scale;
      Vector2 vector2 = ((Vector2) ref scale).Inverse();
      ((Control) this._textureRect).Size = Vector2.op_Multiply(Vector2.op_Multiply(this._textureRect.Texture.GetSize(), vector2), num);
      ((Control) this._textureRect).Position = Vector2.op_Multiply(Vector2.op_Multiply(this.GetHotspot(this._drawingMode), vector2), num);
    }
    else
    {
      ((Control) this._textureRect).Size = this._textureRect.Texture.GetSize();
      ((Control) this._textureRect).Position = this.GetHotspot(this._drawingMode);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NRemoteMouseCursor.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName.SetNextPosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName.UpdateImage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isDown"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("drawingMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName.GetHotspot, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("drawingMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName.GetTexture, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isDown"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("drawingMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteMouseCursor.MethodName.RefreshSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRemoteMouseCursor nremoteMouseCursor = NRemoteMouseCursor.Create(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NRemoteMouseCursor>(ref nremoteMouseCursor);
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.SetNextPosition) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetNextPosition(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.UpdateImage) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateImage(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DrawingMode>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.GetHotspot) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Vector2 hotspot = this.GetHotspot(VariantUtils.ConvertTo<DrawingMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Vector2>(ref hotspot);
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.GetTexture) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      Texture2D texture = this.GetTexture(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DrawingMode>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref texture);
      return true;
    }
    if (!StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.RefreshSize) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RefreshSize();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NRemoteMouseCursor nremoteMouseCursor = NRemoteMouseCursor.Create(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NRemoteMouseCursor>(ref nremoteMouseCursor);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.Create) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName._Ready) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.SetNextPosition) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName._Process) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.UpdateImage) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.GetHotspot) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.GetTexture) || StringName.op_Equality(ref method, NRemoteMouseCursor.MethodName.RefreshSize) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName.PlayerId))
    {
      this.PlayerId = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._textureRect))
    {
      this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec))
    {
      this._lastPositionUpdateMsec = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultHotspot))
    {
      this._defaultHotspot = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._drawingHotspot))
    {
      this._drawingHotspot = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._erasingHotspot))
    {
      this._erasingHotspot = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultCursorImage))
    {
      this._defaultCursorImage = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedCursorImage))
    {
      this._tiltedCursorImage = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultDrawingImage))
    {
      this._defaultDrawingImage = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedDrawingImage))
    {
      this._tiltedDrawingImage = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultErasingImage))
    {
      this._defaultErasingImage = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedErasingImage))
    {
      this._tiltedErasingImage = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultCursorTexture))
    {
      this._defaultCursorTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedCursorTexture))
    {
      this._tiltedCursorTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultDrawingTexture))
    {
      this._defaultDrawingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedDrawingTexture))
    {
      this._tiltedDrawingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultErasingTexture))
    {
      this._defaultErasingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedErasingTexture))
    {
      this._tiltedErasingTexture = VariantUtils.ConvertTo<ImageTexture>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._drawingMode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._drawingMode = VariantUtils.ConvertTo<DrawingMode>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName.PlayerId))
    {
      ref godot_variant local = ref value;
      ulong playerId = this.PlayerId;
      godot_variant from = VariantUtils.CreateFrom<ulong>(ref playerId);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._textureRect))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._lastPositionUpdateMsec);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultHotspot))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._defaultHotspot);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._drawingHotspot))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._drawingHotspot);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._erasingHotspot))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._erasingHotspot);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultCursorImage))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._defaultCursorImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedCursorImage))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._tiltedCursorImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultDrawingImage))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._defaultDrawingImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedDrawingImage))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._tiltedDrawingImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultErasingImage))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._defaultErasingImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedErasingImage))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._tiltedErasingImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultCursorTexture))
    {
      value = VariantUtils.CreateFrom<ImageTexture>(ref this._defaultCursorTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedCursorTexture))
    {
      value = VariantUtils.CreateFrom<ImageTexture>(ref this._tiltedCursorTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultDrawingTexture))
    {
      value = VariantUtils.CreateFrom<ImageTexture>(ref this._defaultDrawingTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedDrawingTexture))
    {
      value = VariantUtils.CreateFrom<ImageTexture>(ref this._tiltedDrawingTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._defaultErasingTexture))
    {
      value = VariantUtils.CreateFrom<ImageTexture>(ref this._defaultErasingTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._tiltedErasingTexture))
    {
      value = VariantUtils.CreateFrom<ImageTexture>(ref this._tiltedErasingTexture);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteMouseCursor.PropertyName._drawingMode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<DrawingMode>(ref this._drawingMode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._textureRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRemoteMouseCursor.PropertyName.PlayerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRemoteMouseCursor.PropertyName._defaultHotspot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRemoteMouseCursor.PropertyName._drawingHotspot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRemoteMouseCursor.PropertyName._erasingHotspot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._defaultCursorImage, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._tiltedCursorImage, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._defaultDrawingImage, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._tiltedDrawingImage, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._defaultErasingImage, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._tiltedErasingImage, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._defaultCursorTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._tiltedCursorTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._defaultDrawingTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._tiltedDrawingTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._defaultErasingTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteMouseCursor.PropertyName._tiltedErasingTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRemoteMouseCursor.PropertyName._drawingMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName playerId1 = NRemoteMouseCursor.PropertyName.PlayerId;
    ulong playerId2 = this.PlayerId;
    Variant variant = Variant.From<ulong>(ref playerId2);
    serializationInfo.AddProperty(playerId1, variant);
    info.AddProperty(NRemoteMouseCursor.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
    info.AddProperty(NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec, Variant.From<ulong>(ref this._lastPositionUpdateMsec));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultHotspot, Variant.From<Vector2>(ref this._defaultHotspot));
    info.AddProperty(NRemoteMouseCursor.PropertyName._drawingHotspot, Variant.From<Vector2>(ref this._drawingHotspot));
    info.AddProperty(NRemoteMouseCursor.PropertyName._erasingHotspot, Variant.From<Vector2>(ref this._erasingHotspot));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultCursorImage, Variant.From<Image>(ref this._defaultCursorImage));
    info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedCursorImage, Variant.From<Image>(ref this._tiltedCursorImage));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultDrawingImage, Variant.From<Image>(ref this._defaultDrawingImage));
    info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingImage, Variant.From<Image>(ref this._tiltedDrawingImage));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultErasingImage, Variant.From<Image>(ref this._defaultErasingImage));
    info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedErasingImage, Variant.From<Image>(ref this._tiltedErasingImage));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultCursorTexture, Variant.From<ImageTexture>(ref this._defaultCursorTexture));
    info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedCursorTexture, Variant.From<ImageTexture>(ref this._tiltedCursorTexture));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultDrawingTexture, Variant.From<ImageTexture>(ref this._defaultDrawingTexture));
    info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingTexture, Variant.From<ImageTexture>(ref this._tiltedDrawingTexture));
    info.AddProperty(NRemoteMouseCursor.PropertyName._defaultErasingTexture, Variant.From<ImageTexture>(ref this._defaultErasingTexture));
    info.AddProperty(NRemoteMouseCursor.PropertyName._tiltedErasingTexture, Variant.From<ImageTexture>(ref this._tiltedErasingTexture));
    info.AddProperty(NRemoteMouseCursor.PropertyName._drawingMode, Variant.From<DrawingMode>(ref this._drawingMode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName.PlayerId, ref variant1))
      this.PlayerId = ((Variant) ref variant1).As<ulong>();
    Variant variant2;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._textureRect, ref variant2))
      this._textureRect = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._lastPositionUpdateMsec, ref variant3))
      this._lastPositionUpdateMsec = ((Variant) ref variant3).As<ulong>();
    Variant variant4;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultHotspot, ref variant4))
      this._defaultHotspot = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._drawingHotspot, ref variant5))
      this._drawingHotspot = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._erasingHotspot, ref variant6))
      this._erasingHotspot = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultCursorImage, ref variant7))
      this._defaultCursorImage = ((Variant) ref variant7).As<Image>();
    Variant variant8;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedCursorImage, ref variant8))
      this._tiltedCursorImage = ((Variant) ref variant8).As<Image>();
    Variant variant9;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultDrawingImage, ref variant9))
      this._defaultDrawingImage = ((Variant) ref variant9).As<Image>();
    Variant variant10;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingImage, ref variant10))
      this._tiltedDrawingImage = ((Variant) ref variant10).As<Image>();
    Variant variant11;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultErasingImage, ref variant11))
      this._defaultErasingImage = ((Variant) ref variant11).As<Image>();
    Variant variant12;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedErasingImage, ref variant12))
      this._tiltedErasingImage = ((Variant) ref variant12).As<Image>();
    Variant variant13;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultCursorTexture, ref variant13))
      this._defaultCursorTexture = ((Variant) ref variant13).As<ImageTexture>();
    Variant variant14;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedCursorTexture, ref variant14))
      this._tiltedCursorTexture = ((Variant) ref variant14).As<ImageTexture>();
    Variant variant15;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultDrawingTexture, ref variant15))
      this._defaultDrawingTexture = ((Variant) ref variant15).As<ImageTexture>();
    Variant variant16;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedDrawingTexture, ref variant16))
      this._tiltedDrawingTexture = ((Variant) ref variant16).As<ImageTexture>();
    Variant variant17;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._defaultErasingTexture, ref variant17))
      this._defaultErasingTexture = ((Variant) ref variant17).As<ImageTexture>();
    Variant variant18;
    if (info.TryGetProperty(NRemoteMouseCursor.PropertyName._tiltedErasingTexture, ref variant18))
      this._tiltedErasingTexture = ((Variant) ref variant18).As<ImageTexture>();
    Variant variant19;
    if (!info.TryGetProperty(NRemoteMouseCursor.PropertyName._drawingMode, ref variant19))
      return;
    this._drawingMode = ((Variant) ref variant19).As<DrawingMode>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetNextPosition = StringName.op_Implicit(nameof (SetNextPosition));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName UpdateImage = StringName.op_Implicit(nameof (UpdateImage));
    public static readonly StringName GetHotspot = StringName.op_Implicit(nameof (GetHotspot));
    public static readonly StringName GetTexture = StringName.op_Implicit(nameof (GetTexture));
    public static readonly StringName RefreshSize = StringName.op_Implicit(nameof (RefreshSize));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName PlayerId = StringName.op_Implicit(nameof (PlayerId));
    public static readonly StringName _textureRect = StringName.op_Implicit(nameof (_textureRect));
    public static readonly StringName _lastPositionUpdateMsec = StringName.op_Implicit(nameof (_lastPositionUpdateMsec));
    public static readonly StringName _defaultHotspot = StringName.op_Implicit(nameof (_defaultHotspot));
    public static readonly StringName _drawingHotspot = StringName.op_Implicit(nameof (_drawingHotspot));
    public static readonly StringName _erasingHotspot = StringName.op_Implicit(nameof (_erasingHotspot));
    public static readonly StringName _defaultCursorImage = StringName.op_Implicit(nameof (_defaultCursorImage));
    public static readonly StringName _tiltedCursorImage = StringName.op_Implicit(nameof (_tiltedCursorImage));
    public static readonly StringName _defaultDrawingImage = StringName.op_Implicit(nameof (_defaultDrawingImage));
    public static readonly StringName _tiltedDrawingImage = StringName.op_Implicit(nameof (_tiltedDrawingImage));
    public static readonly StringName _defaultErasingImage = StringName.op_Implicit(nameof (_defaultErasingImage));
    public static readonly StringName _tiltedErasingImage = StringName.op_Implicit(nameof (_tiltedErasingImage));
    public static readonly StringName _defaultCursorTexture = StringName.op_Implicit(nameof (_defaultCursorTexture));
    public static readonly StringName _tiltedCursorTexture = StringName.op_Implicit(nameof (_tiltedCursorTexture));
    public static readonly StringName _defaultDrawingTexture = StringName.op_Implicit(nameof (_defaultDrawingTexture));
    public static readonly StringName _tiltedDrawingTexture = StringName.op_Implicit(nameof (_tiltedDrawingTexture));
    public static readonly StringName _defaultErasingTexture = StringName.op_Implicit(nameof (_defaultErasingTexture));
    public static readonly StringName _tiltedErasingTexture = StringName.op_Implicit(nameof (_tiltedErasingTexture));
    public static readonly StringName _drawingMode = StringName.op_Implicit(nameof (_drawingMode));
  }

  public class SignalName : Control.SignalName
  {
  }
}
