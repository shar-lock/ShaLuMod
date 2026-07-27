// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NCursorManager
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
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NCursorManager.cs")]
public class NCursorManager : Node
{
  private static readonly Vector2 _defaultHotSpot = new Vector2(14f, 5f);
  private static readonly Vector2 _inspectHotSpot = new Vector2(12f, 12f);
  [Export]
  private Image _cursorTilted;
  [Export]
  private Image _cursorNotTilted;
  [Export]
  private Image _cursorInspect;
  private Image? _overriddenCursorTilted;
  private Image? _overriddenCursorNotTilted;
  private Vector2? _overriddenHotSpot;
  private Image? _lastSetCursor;
  private bool _isDown;
  private bool _isUsingController;
  private bool _shouldShowCursor = true;

  private Image CursorTilted => this._overriddenCursorTilted ?? this._cursorTilted;

  private Image CursorNotTilted => this._overriddenCursorNotTilted ?? this._cursorNotTilted;

  private Vector2 HotSpot => this._overriddenHotSpot ?? NCursorManager._defaultHotSpot;

  public override void _EnterTree()
  {
    Input.SetCustomMouseCursor((Resource) this._cursorInspect, (Input.CursorShape) 16L /*0x10*/, new Vector2?(NCursorManager._inspectHotSpot));
    this.UpdateCursor();
  }

  public override void _Ready()
  {
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From((Action) (() => this.SetIsUsingController(true))), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From((Action) (() => this.SetIsUsingController(false))), 0U);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L && eventMouseButton.ButtonIndex != 2L && eventMouseButton.ButtonIndex != 3L)
      return;
    if (((InputEvent) eventMouseButton).IsPressed() && !this._isDown)
    {
      this._isDown = true;
      this.UpdateCursor();
    }
    else
    {
      if (!((InputEvent) eventMouseButton).IsReleased() || !this._isDown)
        return;
      this._isDown = false;
      this.UpdateCursor();
    }
  }

  public void StopOverridingCursor()
  {
    this._overriddenCursorTilted = (Image) null;
    this._overriddenCursorNotTilted = (Image) null;
    this._overriddenHotSpot = new Vector2?();
    this.UpdateCursor();
  }

  public void OverrideCursor(Image cursorTilted, Image cursorNotTilted, Vector2 hotspot)
  {
    this._overriddenCursorTilted = cursorTilted;
    this._overriddenCursorNotTilted = cursorNotTilted;
    this._overriddenHotSpot = new Vector2?(hotspot);
    this.UpdateCursor();
  }

  private void UpdateCursor()
  {
    if (Input.MouseMode == 1L)
      return;
    Image image = this._isDown ? this.CursorTilted : this.CursorNotTilted;
    if (image == this._lastSetCursor)
      return;
    Input.SetCustomMouseCursor((Resource) image, (Input.CursorShape) 0L, new Vector2?(this.HotSpot));
    this._lastSetCursor = image;
  }

  private void SetIsUsingController(bool isUsingController)
  {
    this._isUsingController = isUsingController;
    this.RefreshCursorShown();
  }

  public void SetCursorShown(bool show)
  {
    this._shouldShowCursor = show;
    this.RefreshCursorShown();
  }

  private void RefreshCursorShown()
  {
    bool flag = !this._isUsingController && this._shouldShowCursor;
    Input.MouseMode = flag ? (Input.MouseModeEnum) 0L : (Input.MouseModeEnum) 1L;
    if (flag)
      return;
    this._lastSetCursor = (Image) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NCursorManager.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName.StopOverridingCursor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName.OverrideCursor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cursorTilted"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Image"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cursorNotTilted"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Image"), false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("hotspot"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName.UpdateCursor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName.SetIsUsingController, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isUsingController"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName.SetCursorShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("show"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCursorManager.MethodName.RefreshCursorShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCursorManager.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName.StopOverridingCursor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopOverridingCursor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName.OverrideCursor) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OverrideCursor(VariantUtils.ConvertTo<Image>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Image>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName.UpdateCursor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateCursor();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName.SetIsUsingController) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsUsingController(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCursorManager.MethodName.SetCursorShown) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCursorShown(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCursorManager.MethodName.RefreshCursorShown) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RefreshCursorShown();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCursorManager.MethodName._EnterTree) || StringName.op_Equality(ref method, NCursorManager.MethodName._Ready) || StringName.op_Equality(ref method, NCursorManager.MethodName._Input) || StringName.op_Equality(ref method, NCursorManager.MethodName.StopOverridingCursor) || StringName.op_Equality(ref method, NCursorManager.MethodName.OverrideCursor) || StringName.op_Equality(ref method, NCursorManager.MethodName.UpdateCursor) || StringName.op_Equality(ref method, NCursorManager.MethodName.SetIsUsingController) || StringName.op_Equality(ref method, NCursorManager.MethodName.SetCursorShown) || StringName.op_Equality(ref method, NCursorManager.MethodName.RefreshCursorShown) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._cursorTilted))
    {
      this._cursorTilted = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._cursorNotTilted))
    {
      this._cursorNotTilted = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._cursorInspect))
    {
      this._cursorInspect = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._overriddenCursorTilted))
    {
      this._overriddenCursorTilted = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._overriddenCursorNotTilted))
    {
      this._overriddenCursorNotTilted = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._lastSetCursor))
    {
      this._lastSetCursor = VariantUtils.ConvertTo<Image>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._isDown))
    {
      this._isDown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._isUsingController))
    {
      this._isUsingController = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCursorManager.PropertyName._shouldShowCursor))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._shouldShowCursor = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName.CursorTilted))
    {
      ref godot_variant local = ref value;
      Image cursorTilted = this.CursorTilted;
      godot_variant from = VariantUtils.CreateFrom<Image>(ref cursorTilted);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName.CursorNotTilted))
    {
      ref godot_variant local = ref value;
      Image cursorNotTilted = this.CursorNotTilted;
      godot_variant from = VariantUtils.CreateFrom<Image>(ref cursorNotTilted);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName.HotSpot))
    {
      ref godot_variant local = ref value;
      Vector2 hotSpot = this.HotSpot;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hotSpot);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._cursorTilted))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._cursorTilted);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._cursorNotTilted))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._cursorNotTilted);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._cursorInspect))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._cursorInspect);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._overriddenCursorTilted))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._overriddenCursorTilted);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._overriddenCursorNotTilted))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._overriddenCursorNotTilted);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._lastSetCursor))
    {
      value = VariantUtils.CreateFrom<Image>(ref this._lastSetCursor);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._isDown))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDown);
      return true;
    }
    if (StringName.op_Equality(ref name, NCursorManager.PropertyName._isUsingController))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isUsingController);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCursorManager.PropertyName._shouldShowCursor))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._shouldShowCursor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName._cursorTilted, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName._cursorNotTilted, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName._cursorInspect, (PropertyHint) 17L, "Image", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName._overriddenCursorTilted, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName._overriddenCursorNotTilted, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName.CursorTilted, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName.CursorNotTilted, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCursorManager.PropertyName.HotSpot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCursorManager.PropertyName._lastSetCursor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCursorManager.PropertyName._isDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCursorManager.PropertyName._isUsingController, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCursorManager.PropertyName._shouldShowCursor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCursorManager.PropertyName._cursorTilted, Variant.From<Image>(ref this._cursorTilted));
    info.AddProperty(NCursorManager.PropertyName._cursorNotTilted, Variant.From<Image>(ref this._cursorNotTilted));
    info.AddProperty(NCursorManager.PropertyName._cursorInspect, Variant.From<Image>(ref this._cursorInspect));
    info.AddProperty(NCursorManager.PropertyName._overriddenCursorTilted, Variant.From<Image>(ref this._overriddenCursorTilted));
    info.AddProperty(NCursorManager.PropertyName._overriddenCursorNotTilted, Variant.From<Image>(ref this._overriddenCursorNotTilted));
    info.AddProperty(NCursorManager.PropertyName._lastSetCursor, Variant.From<Image>(ref this._lastSetCursor));
    info.AddProperty(NCursorManager.PropertyName._isDown, Variant.From<bool>(ref this._isDown));
    info.AddProperty(NCursorManager.PropertyName._isUsingController, Variant.From<bool>(ref this._isUsingController));
    info.AddProperty(NCursorManager.PropertyName._shouldShowCursor, Variant.From<bool>(ref this._shouldShowCursor));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCursorManager.PropertyName._cursorTilted, ref variant1))
      this._cursorTilted = ((Variant) ref variant1).As<Image>();
    Variant variant2;
    if (info.TryGetProperty(NCursorManager.PropertyName._cursorNotTilted, ref variant2))
      this._cursorNotTilted = ((Variant) ref variant2).As<Image>();
    Variant variant3;
    if (info.TryGetProperty(NCursorManager.PropertyName._cursorInspect, ref variant3))
      this._cursorInspect = ((Variant) ref variant3).As<Image>();
    Variant variant4;
    if (info.TryGetProperty(NCursorManager.PropertyName._overriddenCursorTilted, ref variant4))
      this._overriddenCursorTilted = ((Variant) ref variant4).As<Image>();
    Variant variant5;
    if (info.TryGetProperty(NCursorManager.PropertyName._overriddenCursorNotTilted, ref variant5))
      this._overriddenCursorNotTilted = ((Variant) ref variant5).As<Image>();
    Variant variant6;
    if (info.TryGetProperty(NCursorManager.PropertyName._lastSetCursor, ref variant6))
      this._lastSetCursor = ((Variant) ref variant6).As<Image>();
    Variant variant7;
    if (info.TryGetProperty(NCursorManager.PropertyName._isDown, ref variant7))
      this._isDown = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NCursorManager.PropertyName._isUsingController, ref variant8))
      this._isUsingController = ((Variant) ref variant8).As<bool>();
    Variant variant9;
    if (!info.TryGetProperty(NCursorManager.PropertyName._shouldShowCursor, ref variant9))
      return;
    this._shouldShowCursor = ((Variant) ref variant9).As<bool>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName StopOverridingCursor = StringName.op_Implicit(nameof (StopOverridingCursor));
    public static readonly StringName OverrideCursor = StringName.op_Implicit(nameof (OverrideCursor));
    public static readonly StringName UpdateCursor = StringName.op_Implicit(nameof (UpdateCursor));
    public static readonly StringName SetIsUsingController = StringName.op_Implicit(nameof (SetIsUsingController));
    public static readonly StringName SetCursorShown = StringName.op_Implicit(nameof (SetCursorShown));
    public static readonly StringName RefreshCursorShown = StringName.op_Implicit(nameof (RefreshCursorShown));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName CursorTilted = StringName.op_Implicit(nameof (CursorTilted));
    public static readonly StringName CursorNotTilted = StringName.op_Implicit(nameof (CursorNotTilted));
    public static readonly StringName HotSpot = StringName.op_Implicit(nameof (HotSpot));
    public static readonly StringName _cursorTilted = StringName.op_Implicit(nameof (_cursorTilted));
    public static readonly StringName _cursorNotTilted = StringName.op_Implicit(nameof (_cursorNotTilted));
    public static readonly StringName _cursorInspect = StringName.op_Implicit(nameof (_cursorInspect));
    public static readonly StringName _overriddenCursorTilted = StringName.op_Implicit(nameof (_overriddenCursorTilted));
    public static readonly StringName _overriddenCursorNotTilted = StringName.op_Implicit(nameof (_overriddenCursorNotTilted));
    public static readonly StringName _lastSetCursor = StringName.op_Implicit(nameof (_lastSetCursor));
    public static readonly StringName _isDown = StringName.op_Implicit(nameof (_isDown));
    public static readonly StringName _isUsingController = StringName.op_Implicit(nameof (_isUsingController));
    public static readonly StringName _shouldShowCursor = StringName.op_Implicit(nameof (_shouldShowCursor));
  }

  public class SignalName : Node.SignalName
  {
  }
}
