// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NControllerManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.ControllerInput.ControllerConfigs;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NControllerManager.cs")]
public class NControllerManager : Node
{
  private IControllerInputStrategy? _inputStrategy;
  private static readonly Vector2 _offscreenPos = Vector2.op_Multiply(Vector2.One, -1000f);
  private Vector2 _lastMousePosition;
  private int _skipMouseCheckFrames;
  private const float _warpDisplacementThresholdSq = 250000f;
  private MegaLabel _label;
  private Tween? _notifyTween;
  private 
  #nullable disable
  NControllerManager.ControllerDetectedEventHandler backing_ControllerDetected;
  private NControllerManager.MouseDetectedEventHandler backing_MouseDetected;
  private NControllerManager.ControllerTypeChangedEventHandler backing_ControllerTypeChanged;

  public static 
  #nullable enable
  NControllerManager? Instance
  {
    get
    {
      return NGame.Instance == null ? (NControllerManager) null : NGame.Instance.InputManager.ControllerManager;
    }
  }

  public bool ShouldAllowControllerRebinding
  {
    get
    {
      IControllerInputStrategy inputStrategy = this._inputStrategy;
      return inputStrategy == null || inputStrategy.ShouldAllowControllerRebinding;
    }
  }

  public bool IsUsingController { get; private set; }

  public async Task Init()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnScreenContextChanged);
    this._label = this.GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    ((CanvasItem) this._label).Modulate = Colors.Transparent;
    this._inputStrategy = (IControllerInputStrategy) new SteamControllerInputStrategy();
    await this._inputStrategy.Init();
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.OnScreenContextChanged);
  }

  public override void _Process(double delta)
  {
    if (this._skipMouseCheckFrames > 0)
      --this._skipMouseCheckFrames;
    if (!NGame.IsGameFocusedWindow())
      return;
    this._inputStrategy?.ProcessInput();
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (this.IsUsingController)
      this.CheckForMouseInput(inputEvent);
    else
      this.CheckForControllerInput(inputEvent);
  }

  public void OnControllerTypeChanged() => this.EmitSignalControllerTypeChanged();

  private void CheckForMouseInput(InputEvent inputEvent)
  {
    bool flag1 = inputEvent is InputEventMouseButton;
    int num;
    if (inputEvent is InputEventMouseMotion eventMouseMotion)
    {
      Vector2 velocity = eventMouseMotion.Velocity;
      if ((double) ((Vector2) ref velocity).LengthSquared() > 100.0 && this._skipMouseCheckFrames <= 0)
      {
        Vector2 relative = eventMouseMotion.Relative;
        num = (double) ((Vector2) ref relative).LengthSquared() <= 250000.0 ? 1 : 0;
        goto label_4;
      }
    }
    num = 0;
label_4:
    bool flag2 = num != 0;
    Viewport viewport = this.GetViewport();
    if (!(flag1 | flag2))
      return;
    this.IsUsingController = false;
    Input.WarpMouse(this._lastMousePosition);
    viewport?.GuiReleaseFocus();
    ((GodotObject) this).EmitSignal(NControllerManager.SignalName.MouseDetected, Array.Empty<Variant>());
    this.ControlModeChanged();
  }

  private void CheckForControllerInput(InputEvent inputEvent)
  {
    if (!NGame.IsGameFocusedWindow() || !((IEnumerable<StringName>) Controller.AllControllerInputs).Any<StringName>((Func<StringName, bool>) (i => inputEvent.IsActionPressed(i, false, false))))
      return;
    this.IsUsingController = true;
    Viewport viewport = this.GetViewport();
    if (viewport != null)
    {
      Vector2I position1 = DisplayServer.MouseGetPosition();
      Vector2I position2 = DisplayServer.WindowGetPosition(0);
      this._lastMousePosition = new Vector2((float) (position1.X - position2.X), (float) (position1.Y - position2.Y));
      viewport.WarpMouse(NControllerManager._offscreenPos);
      this._skipMouseCheckFrames = 2;
    }
    ActiveScreenContext.Instance.FocusOnDefaultControl();
    ((GodotObject) this).EmitSignal(NControllerManager.SignalName.ControllerDetected, Array.Empty<Variant>());
    this.ControlModeChanged();
    viewport?.SetInputAsHandled();
  }

  private void ControlModeChanged()
  {
    this._notifyTween?.Kill();
    this._notifyTween = this.CreateTween();
    this._notifyTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    this._notifyTween.TweenInterval(0.5);
    this._notifyTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Transparent), 0.75);
    if (this.IsUsingController)
    {
      this._label.SetTextAutoSize(new LocString("main_menu_ui", "CONTROLLER_DETECTED").GetFormattedText());
      Log.Info("CONTROLLER DETECTED: " + (this._inputStrategy != null ? this._inputStrategy.GetControllerName() : "NONE"));
    }
    else
      this._label.SetTextAutoSize(new LocString("main_menu_ui", "MOUSE_DETECTED").GetFormattedText());
  }

  private void OnScreenContextChanged()
  {
    if (this.IsUsingController)
    {
      Callable callable = Callable.From((Action) (() => ActiveScreenContext.Instance.FocusOnDefaultControl()));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
    else
    {
      Vector2 mousePosition = this.GetViewport().GetMousePosition();
      using (InputEventMouseMotion eventMouseMotion = new InputEventMouseMotion())
      {
        ((InputEventMouse) eventMouseMotion).Position = mousePosition;
        ((InputEventMouse) eventMouseMotion).GlobalPosition = mousePosition;
        Input.ParseInputEvent((InputEvent) eventMouseMotion);
      }
    }
  }

  public Texture2D? GetHotkeyIcon(string hotkey) => this._inputStrategy?.GetHotkeyIcon(hotkey);

  public Vector2 GetLeftAnalogStickDirection()
  {
    IControllerInputStrategy inputStrategy = this._inputStrategy;
    return inputStrategy == null ? Vector2.Zero : inputStrategy.GetLeftAnalogStickDirection();
  }

  public Dictionary<StringName, StringName> GetDefaultControllerInputMap
  {
    get
    {
      return this._inputStrategy == null ? new SteamControllerConfig().DefaultControllerInputMap : this._inputStrategy.GetDefaultControllerInputMap;
    }
  }

  public ControllerMappingType ControllerMappingType
  {
    get
    {
      return this._inputStrategy == null ? ControllerMappingType.Default : this._inputStrategy.ControllerConfig.ControllerMappingType;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NControllerManager.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.OnControllerTypeChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.CheckForMouseInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.CheckForControllerInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.ControlModeChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.OnScreenContextChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.GetHotkeyIcon, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("hotkey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NControllerManager.MethodName.GetLeftAnalogStickDirection, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NControllerManager.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName.OnControllerTypeChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnControllerTypeChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName.CheckForMouseInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CheckForMouseInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName.CheckForControllerInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CheckForControllerInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName.ControlModeChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ControlModeChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName.OnScreenContextChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenContextChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NControllerManager.MethodName.GetHotkeyIcon) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Texture2D hotkeyIcon = this.GetHotkeyIcon(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref hotkeyIcon);
      return true;
    }
    if (!StringName.op_Equality(ref method, NControllerManager.MethodName.GetLeftAnalogStickDirection) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    Vector2 analogStickDirection = this.GetLeftAnalogStickDirection();
    ret = VariantUtils.CreateFrom<Vector2>(ref analogStickDirection);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NControllerManager.MethodName._ExitTree) || StringName.op_Equality(ref method, NControllerManager.MethodName._Process) || StringName.op_Equality(ref method, NControllerManager.MethodName._Input) || StringName.op_Equality(ref method, NControllerManager.MethodName.OnControllerTypeChanged) || StringName.op_Equality(ref method, NControllerManager.MethodName.CheckForMouseInput) || StringName.op_Equality(ref method, NControllerManager.MethodName.CheckForControllerInput) || StringName.op_Equality(ref method, NControllerManager.MethodName.ControlModeChanged) || StringName.op_Equality(ref method, NControllerManager.MethodName.OnScreenContextChanged) || StringName.op_Equality(ref method, NControllerManager.MethodName.GetHotkeyIcon) || StringName.op_Equality(ref method, NControllerManager.MethodName.GetLeftAnalogStickDirection) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName.IsUsingController))
    {
      this.IsUsingController = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName._lastMousePosition))
    {
      this._lastMousePosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName._skipMouseCheckFrames))
    {
      this._skipMouseCheckFrames = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NControllerManager.PropertyName._notifyTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._notifyTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName.ShouldAllowControllerRebinding))
    {
      ref godot_variant local = ref value;
      bool controllerRebinding = this.ShouldAllowControllerRebinding;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref controllerRebinding);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName.IsUsingController))
    {
      ref godot_variant local = ref value;
      bool isUsingController = this.IsUsingController;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isUsingController);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName.ControllerMappingType))
    {
      ref godot_variant local = ref value;
      ControllerMappingType controllerMappingType = this.ControllerMappingType;
      godot_variant from = VariantUtils.CreateFrom<ControllerMappingType>(ref controllerMappingType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName._lastMousePosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._lastMousePosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName._skipMouseCheckFrames))
    {
      value = VariantUtils.CreateFrom<int>(ref this._skipMouseCheckFrames);
      return true;
    }
    if (StringName.op_Equality(ref name, NControllerManager.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (!StringName.op_Equality(ref name, NControllerManager.PropertyName._notifyTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._notifyTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NControllerManager.PropertyName.ShouldAllowControllerRebinding, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NControllerManager.PropertyName._lastMousePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NControllerManager.PropertyName._skipMouseCheckFrames, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NControllerManager.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NControllerManager.PropertyName._notifyTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NControllerManager.PropertyName.IsUsingController, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NControllerManager.PropertyName.ControllerMappingType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isUsingController1 = NControllerManager.PropertyName.IsUsingController;
    bool isUsingController2 = this.IsUsingController;
    Variant variant = Variant.From<bool>(ref isUsingController2);
    serializationInfo.AddProperty(isUsingController1, variant);
    info.AddProperty(NControllerManager.PropertyName._lastMousePosition, Variant.From<Vector2>(ref this._lastMousePosition));
    info.AddProperty(NControllerManager.PropertyName._skipMouseCheckFrames, Variant.From<int>(ref this._skipMouseCheckFrames));
    info.AddProperty(NControllerManager.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NControllerManager.PropertyName._notifyTween, Variant.From<Tween>(ref this._notifyTween));
    info.AddSignalEventDelegate(NControllerManager.SignalName.ControllerDetected, (Delegate) this.backing_ControllerDetected);
    info.AddSignalEventDelegate(NControllerManager.SignalName.MouseDetected, (Delegate) this.backing_MouseDetected);
    info.AddSignalEventDelegate(NControllerManager.SignalName.ControllerTypeChanged, (Delegate) this.backing_ControllerTypeChanged);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NControllerManager.PropertyName.IsUsingController, ref variant1))
      this.IsUsingController = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NControllerManager.PropertyName._lastMousePosition, ref variant2))
      this._lastMousePosition = ((Variant) ref variant2).As<Vector2>();
    Variant variant3;
    if (info.TryGetProperty(NControllerManager.PropertyName._skipMouseCheckFrames, ref variant3))
      this._skipMouseCheckFrames = ((Variant) ref variant3).As<int>();
    Variant variant4;
    if (info.TryGetProperty(NControllerManager.PropertyName._label, ref variant4))
      this._label = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NControllerManager.PropertyName._notifyTween, ref variant5))
      this._notifyTween = ((Variant) ref variant5).As<Tween>();
    NControllerManager.ControllerDetectedEventHandler detectedEventHandler1;
    if (info.TryGetSignalEventDelegate<NControllerManager.ControllerDetectedEventHandler>(NControllerManager.SignalName.ControllerDetected, ref detectedEventHandler1))
      this.backing_ControllerDetected = detectedEventHandler1;
    NControllerManager.MouseDetectedEventHandler detectedEventHandler2;
    if (info.TryGetSignalEventDelegate<NControllerManager.MouseDetectedEventHandler>(NControllerManager.SignalName.MouseDetected, ref detectedEventHandler2))
      this.backing_MouseDetected = detectedEventHandler2;
    NControllerManager.ControllerTypeChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NControllerManager.ControllerTypeChangedEventHandler>(NControllerManager.SignalName.ControllerTypeChanged, ref changedEventHandler))
      return;
    this.backing_ControllerTypeChanged = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NControllerManager.SignalName.ControllerDetected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerManager.SignalName.MouseDetected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NControllerManager.SignalName.ControllerTypeChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NControllerManager.ControllerDetectedEventHandler ControllerDetected
  {
    add => this.backing_ControllerDetected += value;
    remove => this.backing_ControllerDetected -= value;
  }

  protected void EmitSignalControllerDetected()
  {
    ((GodotObject) this).EmitSignal(NControllerManager.SignalName.ControllerDetected, Array.Empty<Variant>());
  }

  public event NControllerManager.MouseDetectedEventHandler MouseDetected
  {
    add => this.backing_MouseDetected += value;
    remove => this.backing_MouseDetected -= value;
  }

  protected void EmitSignalMouseDetected()
  {
    ((GodotObject) this).EmitSignal(NControllerManager.SignalName.MouseDetected, Array.Empty<Variant>());
  }

  public event NControllerManager.ControllerTypeChangedEventHandler ControllerTypeChanged
  {
    add => this.backing_ControllerTypeChanged += value;
    remove => this.backing_ControllerTypeChanged -= value;
  }

  protected void EmitSignalControllerTypeChanged()
  {
    ((GodotObject) this).EmitSignal(NControllerManager.SignalName.ControllerTypeChanged, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NControllerManager.SignalName.ControllerDetected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NControllerManager.ControllerDetectedEventHandler controllerDetected = this.backing_ControllerDetected;
      if (controllerDetected == null)
        return;
      controllerDetected();
    }
    else if (StringName.op_Equality(ref signal, NControllerManager.SignalName.MouseDetected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NControllerManager.MouseDetectedEventHandler backingMouseDetected = this.backing_MouseDetected;
      if (backingMouseDetected == null)
        return;
      backingMouseDetected();
    }
    else if (StringName.op_Equality(ref signal, NControllerManager.SignalName.ControllerTypeChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NControllerManager.ControllerTypeChangedEventHandler controllerTypeChanged = this.backing_ControllerTypeChanged;
      if (controllerTypeChanged == null)
        return;
      controllerTypeChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NControllerManager.SignalName.ControllerDetected) || StringName.op_Equality(ref signal, NControllerManager.SignalName.MouseDetected) || StringName.op_Equality(ref signal, NControllerManager.SignalName.ControllerTypeChanged) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ControllerDetectedEventHandler();

  [Signal]
  public delegate void MouseDetectedEventHandler();

  [Signal]
  public delegate void ControllerTypeChangedEventHandler();

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName OnControllerTypeChanged = StringName.op_Implicit(nameof (OnControllerTypeChanged));
    public static readonly StringName CheckForMouseInput = StringName.op_Implicit(nameof (CheckForMouseInput));
    public static readonly StringName CheckForControllerInput = StringName.op_Implicit(nameof (CheckForControllerInput));
    public static readonly StringName ControlModeChanged = StringName.op_Implicit(nameof (ControlModeChanged));
    public static readonly StringName OnScreenContextChanged = StringName.op_Implicit(nameof (OnScreenContextChanged));
    public static readonly StringName GetHotkeyIcon = StringName.op_Implicit(nameof (GetHotkeyIcon));
    public static readonly StringName GetLeftAnalogStickDirection = StringName.op_Implicit(nameof (GetLeftAnalogStickDirection));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName ShouldAllowControllerRebinding = StringName.op_Implicit(nameof (ShouldAllowControllerRebinding));
    public static readonly StringName IsUsingController = StringName.op_Implicit(nameof (IsUsingController));
    public static readonly StringName ControllerMappingType = StringName.op_Implicit(nameof (ControllerMappingType));
    public static readonly StringName _lastMousePosition = StringName.op_Implicit(nameof (_lastMousePosition));
    public static readonly StringName _skipMouseCheckFrames = StringName.op_Implicit(nameof (_skipMouseCheckFrames));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _notifyTween = StringName.op_Implicit(nameof (_notifyTween));
  }

  public class SignalName : Node.SignalName
  {
    public static readonly StringName ControllerDetected = StringName.op_Implicit(nameof (ControllerDetected));
    public static readonly StringName MouseDetected = StringName.op_Implicit(nameof (MouseDetected));
    public static readonly StringName ControllerTypeChanged = StringName.op_Implicit(nameof (ControllerTypeChanged));
  }
}
