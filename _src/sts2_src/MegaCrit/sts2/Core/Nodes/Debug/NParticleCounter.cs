// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.NParticleCounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[ScriptPath("res://src/Core/Nodes/Debug/NParticleCounter.cs")]
public class NParticleCounter : Control
{
  private static readonly StringName _toggleParticleCounter = new StringName("toggle_particle_counter");
  private const float _secondsPerUpdate = 5f;
  private TextureRect _icon;
  private Label? _label;
  private double _secondsSinceLastUpdate;
  private int _totalParticles;
  private int _updateCount;

  public override void _Ready()
  {
    if (!OS.HasFeature("editor"))
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
      this._label = ((Node) this).GetNode<Label>(NodePath.op_Implicit("%Label"));
      ((CanvasItem) this).Visible = false;
    }
  }

  public override void _Input(InputEvent inputEvent) => this.CheckForHotkey(inputEvent);

  private void CheckForHotkey(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(NParticleCounter._toggleParticleCounter, false) || NDevConsole.IsConsoleVisible)
      return;
    ((CanvasItem) this).Visible = !((CanvasItem) this).Visible;
  }

  public override void _Process(double delta)
  {
    if (!((CanvasItem) this).Visible || this._label == null)
      return;
    this._secondsSinceLastUpdate += delta;
    if ((this._totalParticles > 1 || this._updateCount > 500) && this._secondsSinceLastUpdate < 5.0)
      return;
    ++this._updateCount;
    this._secondsSinceLastUpdate = 0.0;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    foreach (Node node in NParticleCounter.GetChildrenRecursive((Node) ((Node) this).GetTree().Root))
    {
      if (!(node is CpuParticles2D cpuParticles2D))
      {
        if (node is GpuParticles2D gpuParticles2D)
        {
          num3 += gpuParticles2D.Amount;
          ++num4;
        }
      }
      else
      {
        num1 += cpuParticles2D.Amount;
        ++num2;
      }
    }
    this._totalParticles = num1 + num3;
    this._label.Text = $"All particles: {this._totalParticles}\nCPU particles: {num1} in {num2} node{(num2 == 1 ? "" : "s")}\nGPU particles: {num3} in {num4} node{(num4 == 1 ? "" : "s")}";
  }

  private static List<Node> GetChildrenRecursive(Node root)
  {
    int capacity = 1;
    List<Node> nodeList = new List<Node>(capacity);
    CollectionsMarshal.SetCount<Node>(nodeList, capacity);
    CollectionsMarshal.AsSpan<Node>(nodeList)[0] = root;
    List<Node> childrenRecursive = nodeList;
    foreach (Node child in root.GetChildren(false))
      childrenRecursive.AddRange((IEnumerable<Node>) NParticleCounter.GetChildrenRecursive(child));
    return childrenRecursive;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NParticleCounter.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NParticleCounter.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NParticleCounter.MethodName.CheckForHotkey, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NParticleCounter.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NParticleCounter.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NParticleCounter.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NParticleCounter.MethodName.CheckForHotkey) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.CheckForHotkey(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NParticleCounter.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NParticleCounter.MethodName._Ready) || StringName.op_Equality(ref method, NParticleCounter.MethodName._Input) || StringName.op_Equality(ref method, NParticleCounter.MethodName.CheckForHotkey) || StringName.op_Equality(ref method, NParticleCounter.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<Label>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._secondsSinceLastUpdate))
    {
      this._secondsSinceLastUpdate = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._totalParticles))
    {
      this._totalParticles = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NParticleCounter.PropertyName._updateCount))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._updateCount = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<Label>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._secondsSinceLastUpdate))
    {
      value = VariantUtils.CreateFrom<double>(ref this._secondsSinceLastUpdate);
      return true;
    }
    if (StringName.op_Equality(ref name, NParticleCounter.PropertyName._totalParticles))
    {
      value = VariantUtils.CreateFrom<int>(ref this._totalParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NParticleCounter.PropertyName._updateCount))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._updateCount);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NParticleCounter.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NParticleCounter.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NParticleCounter.PropertyName._secondsSinceLastUpdate, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NParticleCounter.PropertyName._totalParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NParticleCounter.PropertyName._updateCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NParticleCounter.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NParticleCounter.PropertyName._label, Variant.From<Label>(ref this._label));
    info.AddProperty(NParticleCounter.PropertyName._secondsSinceLastUpdate, Variant.From<double>(ref this._secondsSinceLastUpdate));
    info.AddProperty(NParticleCounter.PropertyName._totalParticles, Variant.From<int>(ref this._totalParticles));
    info.AddProperty(NParticleCounter.PropertyName._updateCount, Variant.From<int>(ref this._updateCount));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NParticleCounter.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NParticleCounter.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<Label>();
    Variant variant3;
    if (info.TryGetProperty(NParticleCounter.PropertyName._secondsSinceLastUpdate, ref variant3))
      this._secondsSinceLastUpdate = ((Variant) ref variant3).As<double>();
    Variant variant4;
    if (info.TryGetProperty(NParticleCounter.PropertyName._totalParticles, ref variant4))
      this._totalParticles = ((Variant) ref variant4).As<int>();
    Variant variant5;
    if (!info.TryGetProperty(NParticleCounter.PropertyName._updateCount, ref variant5))
      return;
    this._updateCount = ((Variant) ref variant5).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName CheckForHotkey = StringName.op_Implicit(nameof (CheckForHotkey));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _secondsSinceLastUpdate = StringName.op_Implicit(nameof (_secondsSinceLastUpdate));
    public static readonly StringName _totalParticles = StringName.op_Implicit(nameof (_totalParticles));
    public static readonly StringName _updateCount = StringName.op_Implicit(nameof (_updateCount));
  }

  public class SignalName : Control.SignalName
  {
  }
}
