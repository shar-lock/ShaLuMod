// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Examples.NSaturationExampleVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Examples;

[ScriptPath("res://src/Core/Nodes/Vfx/Examples/NSaturationExampleVfx.cs")]
public class NSaturationExampleVfx : Node
{
  private WorldEnvironment _env;
  private Tween? _tween;

  public override void _Ready()
  {
    this._env = this.GetNode<WorldEnvironment>(NodePath.op_Implicit("%WorldEnvironment"));
    this._tween = this.CreateTween();
    this._tween.SetLoops(0);
    this._tween.TweenProperty((GodotObject) this._env, NodePath.op_Implicit("environment:adjustment_saturation"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._env, NodePath.op_Implicit("environment:adjustment_saturation"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 7L);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventKey inputEventKey) || !((InputEvent) inputEventKey).IsReleased() || inputEventKey.Keycode != 51L)
      return;
    if (this._tween.IsRunning())
    {
      Log.Info("Pausing Saturation");
      this._tween.Pause();
    }
    else
    {
      Log.Info("Resuming Saturation");
      this._tween.Play();
    }
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NSaturationExampleVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSaturationExampleVfx.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSaturationExampleVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSaturationExampleVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSaturationExampleVfx.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSaturationExampleVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSaturationExampleVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSaturationExampleVfx.MethodName._Input) || StringName.op_Equality(ref method, NSaturationExampleVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSaturationExampleVfx.PropertyName._env))
    {
      this._env = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSaturationExampleVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSaturationExampleVfx.PropertyName._env))
    {
      value = VariantUtils.CreateFrom<WorldEnvironment>(ref this._env);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSaturationExampleVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSaturationExampleVfx.PropertyName._env, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSaturationExampleVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSaturationExampleVfx.PropertyName._env, Variant.From<WorldEnvironment>(ref this._env));
    info.AddProperty(NSaturationExampleVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSaturationExampleVfx.PropertyName._env, ref variant1))
      this._env = ((Variant) ref variant1).As<WorldEnvironment>();
    Variant variant2;
    if (!info.TryGetProperty(NSaturationExampleVfx.PropertyName._tween, ref variant2))
      return;
    this._tween = ((Variant) ref variant2).As<Tween>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _env = StringName.op_Implicit(nameof (_env));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node.SignalName
  {
  }
}
