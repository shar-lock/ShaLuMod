// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NSendFeedbackCartoon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackCartoon.cs")]
public class NSendFeedbackCartoon : TextureRect
{
  private static readonly float _rot1 = Mathf.DegToRad(5f);
  private static readonly float _rot2 = 0.0f;
  private Tween? _tween;
  [Export]
  public bool opposite;

  public void SetRotation1()
  {
    ((Control) this).Rotation = this.opposite ? NSendFeedbackCartoon._rot1 : NSendFeedbackCartoon._rot2;
  }

  public void SetRotation2()
  {
    ((Control) this).Rotation = this.opposite ? NSendFeedbackCartoon._rot2 : NSendFeedbackCartoon._rot1;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NSendFeedbackCartoon.MethodName.SetRotation1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackCartoon.MethodName.SetRotation2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSendFeedbackCartoon.MethodName.SetRotation1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetRotation1();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSendFeedbackCartoon.MethodName.SetRotation2) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetRotation2();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSendFeedbackCartoon.MethodName.SetRotation1) || StringName.op_Equality(ref method, NSendFeedbackCartoon.MethodName.SetRotation2) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackCartoon.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackCartoon.PropertyName.opposite))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.opposite = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackCartoon.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackCartoon.PropertyName.opposite))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this.opposite);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackCartoon.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSendFeedbackCartoon.PropertyName.opposite, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSendFeedbackCartoon.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NSendFeedbackCartoon.PropertyName.opposite, Variant.From<bool>(ref this.opposite));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSendFeedbackCartoon.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NSendFeedbackCartoon.PropertyName.opposite, ref variant2))
      return;
    this.opposite = ((Variant) ref variant2).As<bool>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName SetRotation1 = StringName.op_Implicit(nameof (SetRotation1));
    public static readonly StringName SetRotation2 = StringName.op_Implicit(nameof (SetRotation2));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName opposite = StringName.op_Implicit(nameof (opposite));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
