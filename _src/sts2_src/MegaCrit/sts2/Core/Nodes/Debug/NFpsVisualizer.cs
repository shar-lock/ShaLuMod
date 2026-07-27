// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.NFpsVisualizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[ScriptPath("res://src/Core/Nodes/Debug/NFpsVisualizer.cs")]
public class NFpsVisualizer : TextureRect
{
  private Label? _label;
  [Export]
  private Texture2D _happy;
  [Export]
  private Texture2D _content;
  [Export]
  private Texture2D _neutral;
  [Export]
  private Texture2D _sad;
  private 
  #nullable disable
  NFpsVisualizer.MouseReleasedEventHandler backing_MouseReleased;

  public override void _Ready()
  {
    if (!OS.HasFeature("editor"))
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this._label = ((Node) this).GetNode<Label>(NodePath.op_Implicit("Label"));
      ((GodotObject) this).Connect(NFpsVisualizer.SignalName.MouseReleased, Callable.From<InputEvent>(new Action<InputEvent>(this.HandleMouseRelease)), 0U);
    }
  }

  private void HandleMouseRelease(
  #nullable enable
  InputEvent inputEvent) => ((Node) this).QueueFreeSafely();

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L || !((InputEvent) eventMouseButton).IsReleased())
      return;
    ((GodotObject) this).EmitSignal(NFpsVisualizer.SignalName.MouseReleased, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) inputEvent)
    });
  }

  public override void _Process(double delta)
  {
    if (this._label == null)
      return;
    double framesPerSecond = Engine.GetFramesPerSecond();
    this.Texture = framesPerSecond >= 58.0 ? this._happy : (framesPerSecond >= 50.0 ? this._content : (framesPerSecond >= 30.0 ? this._neutral : this._sad));
    this._label.Text = framesPerSecond.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NFpsVisualizer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFpsVisualizer.MethodName.HandleMouseRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFpsVisualizer.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFpsVisualizer.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFpsVisualizer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFpsVisualizer.MethodName.HandleMouseRelease) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HandleMouseRelease(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFpsVisualizer.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Control) this)._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFpsVisualizer.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFpsVisualizer.MethodName._Ready) || StringName.op_Equality(ref method, NFpsVisualizer.MethodName.HandleMouseRelease) || StringName.op_Equality(ref method, NFpsVisualizer.MethodName._GuiInput) || StringName.op_Equality(ref method, NFpsVisualizer.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<Label>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._happy))
    {
      this._happy = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._content))
    {
      this._content = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._neutral))
    {
      this._neutral = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._sad))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._sad = VariantUtils.ConvertTo<Texture2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<Label>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._happy))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._happy);
      return true;
    }
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._content))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._content);
      return true;
    }
    if (StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._neutral))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._neutral);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFpsVisualizer.PropertyName._sad))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Texture2D>(ref this._sad);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFpsVisualizer.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFpsVisualizer.PropertyName._happy, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NFpsVisualizer.PropertyName._content, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NFpsVisualizer.PropertyName._neutral, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NFpsVisualizer.PropertyName._sad, (PropertyHint) 17L, "Texture2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFpsVisualizer.PropertyName._label, Variant.From<Label>(ref this._label));
    info.AddProperty(NFpsVisualizer.PropertyName._happy, Variant.From<Texture2D>(ref this._happy));
    info.AddProperty(NFpsVisualizer.PropertyName._content, Variant.From<Texture2D>(ref this._content));
    info.AddProperty(NFpsVisualizer.PropertyName._neutral, Variant.From<Texture2D>(ref this._neutral));
    info.AddProperty(NFpsVisualizer.PropertyName._sad, Variant.From<Texture2D>(ref this._sad));
    info.AddSignalEventDelegate(NFpsVisualizer.SignalName.MouseReleased, (Delegate) this.backing_MouseReleased);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFpsVisualizer.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<Label>();
    Variant variant2;
    if (info.TryGetProperty(NFpsVisualizer.PropertyName._happy, ref variant2))
      this._happy = ((Variant) ref variant2).As<Texture2D>();
    Variant variant3;
    if (info.TryGetProperty(NFpsVisualizer.PropertyName._content, ref variant3))
      this._content = ((Variant) ref variant3).As<Texture2D>();
    Variant variant4;
    if (info.TryGetProperty(NFpsVisualizer.PropertyName._neutral, ref variant4))
      this._neutral = ((Variant) ref variant4).As<Texture2D>();
    Variant variant5;
    if (info.TryGetProperty(NFpsVisualizer.PropertyName._sad, ref variant5))
      this._sad = ((Variant) ref variant5).As<Texture2D>();
    NFpsVisualizer.MouseReleasedEventHandler releasedEventHandler;
    if (!info.TryGetSignalEventDelegate<NFpsVisualizer.MouseReleasedEventHandler>(NFpsVisualizer.SignalName.MouseReleased, ref releasedEventHandler))
      return;
    this.backing_MouseReleased = releasedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NFpsVisualizer.SignalName.MouseReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  public event NFpsVisualizer.MouseReleasedEventHandler MouseReleased
  {
    add => this.backing_MouseReleased += value;
    remove => this.backing_MouseReleased -= value;
  }

  protected void EmitSignalMouseReleased(InputEvent inputEvent)
  {
    ((GodotObject) this).EmitSignal(NFpsVisualizer.SignalName.MouseReleased, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) inputEvent)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NFpsVisualizer.SignalName.MouseReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NFpsVisualizer.MouseReleasedEventHandler backingMouseReleased = this.backing_MouseReleased;
      if (backingMouseReleased == null)
        return;
      backingMouseReleased(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NFpsVisualizer.SignalName.MouseReleased) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void MouseReleasedEventHandler(
  #nullable enable
  InputEvent inputEvent);

  public class MethodName : TextureRect.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName HandleMouseRelease = StringName.op_Implicit(nameof (HandleMouseRelease));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _happy = StringName.op_Implicit(nameof (_happy));
    public static readonly StringName _content = StringName.op_Implicit(nameof (_content));
    public static readonly StringName _neutral = StringName.op_Implicit(nameof (_neutral));
    public static readonly StringName _sad = StringName.op_Implicit(nameof (_sad));
  }

  public class SignalName : TextureRect.SignalName
  {
    public static readonly StringName MouseReleased = StringName.op_Implicit(nameof (MouseReleased));
  }
}
