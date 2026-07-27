// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Animation.NPhobiaAnimationToggler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Animation;

[ScriptPath("res://src/Core/Nodes/Animation/NPhobiaAnimationToggler.cs")]
public class NPhobiaAnimationToggler : Node
{
  [Export]
  private AnimationPlayer? _animationPlayer;

  public override void _Ready()
  {
    base._Ready();
    this.UpdatePhobiaMode();
  }

  public override void _EnterTree()
  {
    ((GodotObject) NGame.Instance)?.Connect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)), 0U);
  }

  public override void _ExitTree()
  {
    ((GodotObject) NGame.Instance)?.Disconnect(NGame.SignalName.PhobiaModeToggled, Callable.From(new Action(this.UpdatePhobiaMode)));
  }

  private void UpdatePhobiaMode()
  {
    if (this._animationPlayer == null)
      return;
    ((AnimationMixer) this._animationPlayer).Active = !SaveManager.Instance.PrefsSave.PhobiaMode;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NPhobiaAnimationToggler.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPhobiaAnimationToggler.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPhobiaAnimationToggler.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPhobiaAnimationToggler.MethodName.UpdatePhobiaMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName.UpdatePhobiaMode) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdatePhobiaMode();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName._Ready) || StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName._EnterTree) || StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName._ExitTree) || StringName.op_Equality(ref method, NPhobiaAnimationToggler.MethodName.UpdatePhobiaMode) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NPhobiaAnimationToggler.PropertyName._animationPlayer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._animationPlayer = VariantUtils.ConvertTo<AnimationPlayer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NPhobiaAnimationToggler.PropertyName._animationPlayer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<AnimationPlayer>(ref this._animationPlayer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPhobiaAnimationToggler.PropertyName._animationPlayer, (PropertyHint) 34L, "AnimationPlayer", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPhobiaAnimationToggler.PropertyName._animationPlayer, Variant.From<AnimationPlayer>(ref this._animationPlayer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NPhobiaAnimationToggler.PropertyName._animationPlayer, ref variant))
      return;
    this._animationPlayer = ((Variant) ref variant).As<AnimationPlayer>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdatePhobiaMode = StringName.op_Implicit(nameof (UpdatePhobiaMode));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _animationPlayer = StringName.op_Implicit(nameof (_animationPlayer));
  }

  public class SignalName : Node.SignalName
  {
  }
}
