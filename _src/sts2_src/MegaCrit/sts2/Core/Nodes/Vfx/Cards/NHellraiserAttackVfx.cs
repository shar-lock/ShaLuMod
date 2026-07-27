// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NHellraiserAttackVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NHellraiserAttackVfx.cs")]
public class NHellraiserAttackVfx : Node2D
{
  public override void _Ready()
  {
    TextureRect node = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Sword"));
    node.FlipH = Rng.Chaotic.NextBool();
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(90), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.30000001192092896).From(Variant.op_Implicit(Colors.Red));
    tween.Chain().TweenInterval(0.10000000149011612);
    tween.Chain();
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(300), 0.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.30000001192092896);
    tween.Chain().TweenCallback(Callable.From(new Action(this.OnTweenFinished)));
  }

  private void OnTweenFinished() => ((Node) this).QueueFreeSafely();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NHellraiserAttackVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHellraiserAttackVfx.MethodName.OnTweenFinished, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHellraiserAttackVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHellraiserAttackVfx.MethodName.OnTweenFinished) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnTweenFinished();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHellraiserAttackVfx.MethodName._Ready) || StringName.op_Equality(ref method, NHellraiserAttackVfx.MethodName.OnTweenFinished) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnTweenFinished = StringName.op_Implicit(nameof (OnTweenFinished));
  }

  public class PropertyName : Node2D.PropertyName
  {
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
