// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Map.NMapDrawingInput
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Map;

[ScriptPath("res://src/Core/Nodes/Screens/Map/NMapDrawingInput.cs")]
public abstract class NMapDrawingInput : Control
{
  protected NMapDrawings _drawings;
  private 
  #nullable disable
  NMapDrawingInput.FinishedEventHandler backing_Finished;

  public DrawingMode DrawingMode { get; private set; }

  public static 
  #nullable enable
  NMapDrawingInput Create(NMapDrawings drawings, DrawingMode drawingMode, bool stopOnMouseRelease = false)
  {
    NMapDrawingInput nmapDrawingInput = !stopOnMouseRelease ? (!NControllerManager.Instance.IsUsingController ? (NMapDrawingInput) new NMouseModeMapDrawingInput() : NControllerMapDrawingInput.Create()) : (NMapDrawingInput) new NMouseHeldMapDrawingInput();
    nmapDrawingInput._drawings = drawings;
    nmapDrawingInput.DrawingMode = drawingMode;
    nmapDrawingInput._drawings.SetDrawingModeLocal(drawingMode);
    return nmapDrawingInput;
  }

  public override void _Ready()
  {
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.StopDrawing)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.StopDrawing)), 0U);
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.StopDrawing);
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.StopDrawing);
  }

  public void StopDrawing()
  {
    if (this._drawings.IsLocalDrawing())
      this._drawings.StopLineLocal();
    this._drawings.SetDrawingModeLocal(DrawingMode.None);
    this.EmitSignalFinished();
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMapDrawingInput.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("drawings"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("drawingMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("stopOnMouseRelease"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMapDrawingInput.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawingInput.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawingInput.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapDrawingInput.MethodName.StopDrawing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapDrawingInput.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NMapDrawingInput nmapDrawingInput = NMapDrawingInput.Create(VariantUtils.ConvertTo<NMapDrawings>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DrawingMode>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawingInput.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawingInput.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapDrawingInput.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapDrawingInput.MethodName.StopDrawing) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StopDrawing();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapDrawingInput.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NMapDrawingInput nmapDrawingInput = NMapDrawingInput.Create(VariantUtils.ConvertTo<NMapDrawings>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<DrawingMode>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NMapDrawingInput>(ref nmapDrawingInput);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapDrawingInput.MethodName.Create) || StringName.op_Equality(ref method, NMapDrawingInput.MethodName._Ready) || StringName.op_Equality(ref method, NMapDrawingInput.MethodName._EnterTree) || StringName.op_Equality(ref method, NMapDrawingInput.MethodName._ExitTree) || StringName.op_Equality(ref method, NMapDrawingInput.MethodName.StopDrawing) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapDrawingInput.PropertyName.DrawingMode))
    {
      this.DrawingMode = VariantUtils.ConvertTo<DrawingMode>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapDrawingInput.PropertyName._drawings))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._drawings = VariantUtils.ConvertTo<NMapDrawings>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapDrawingInput.PropertyName.DrawingMode))
    {
      ref godot_variant local = ref value;
      DrawingMode drawingMode = this.DrawingMode;
      godot_variant from = VariantUtils.CreateFrom<DrawingMode>(ref drawingMode);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapDrawingInput.PropertyName._drawings))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NMapDrawings>(ref this._drawings);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapDrawingInput.PropertyName._drawings, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMapDrawingInput.PropertyName.DrawingMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName drawingMode1 = NMapDrawingInput.PropertyName.DrawingMode;
    DrawingMode drawingMode2 = this.DrawingMode;
    Variant variant = Variant.From<DrawingMode>(ref drawingMode2);
    serializationInfo.AddProperty(drawingMode1, variant);
    info.AddProperty(NMapDrawingInput.PropertyName._drawings, Variant.From<NMapDrawings>(ref this._drawings));
    info.AddSignalEventDelegate(NMapDrawingInput.SignalName.Finished, (Delegate) this.backing_Finished);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapDrawingInput.PropertyName.DrawingMode, ref variant1))
      this.DrawingMode = ((Variant) ref variant1).As<DrawingMode>();
    Variant variant2;
    if (info.TryGetProperty(NMapDrawingInput.PropertyName._drawings, ref variant2))
      this._drawings = ((Variant) ref variant2).As<NMapDrawings>();
    NMapDrawingInput.FinishedEventHandler finishedEventHandler;
    if (!info.TryGetSignalEventDelegate<NMapDrawingInput.FinishedEventHandler>(NMapDrawingInput.SignalName.Finished, ref finishedEventHandler))
      return;
    this.backing_Finished = finishedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMapDrawingInput.SignalName.Finished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NMapDrawingInput.FinishedEventHandler Finished
  {
    add => this.backing_Finished += value;
    remove => this.backing_Finished -= value;
  }

  protected void EmitSignalFinished()
  {
    ((GodotObject) this).EmitSignal(NMapDrawingInput.SignalName.Finished, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NMapDrawingInput.SignalName.Finished) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMapDrawingInput.FinishedEventHandler backingFinished = this.backing_Finished;
      if (backingFinished == null)
        return;
      backingFinished();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NMapDrawingInput.SignalName.Finished) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void FinishedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName StopDrawing = StringName.op_Implicit(nameof (StopDrawing));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DrawingMode = StringName.op_Implicit(nameof (DrawingMode));
    public static readonly StringName _drawings = StringName.op_Implicit(nameof (_drawings));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Finished = StringName.op_Implicit(nameof (Finished));
  }
}
