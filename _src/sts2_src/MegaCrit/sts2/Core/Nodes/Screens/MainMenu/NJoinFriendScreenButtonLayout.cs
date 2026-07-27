// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NJoinFriendScreenButtonLayout
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[Tool]
[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NJoinFriendScreenButtonLayout.cs")]
public class NJoinFriendScreenButtonLayout : Container
{
  public override void _Notification(int what)
  {
    if (what != 51)
      return;
    this.LayoutChildren();
  }

  private void LayoutChildren()
  {
    Control[] array = ((IEnumerable) ((Node) this).GetChildren(false)).OfType<Control>().ToArray<Control>();
    if (array.Length == 0)
      return;
    Vector2 size = array[0].Size;
    int num1 = (int) ((double) ((Control) this).Size.Y / (double) size.Y);
    int num2 = (int) Math.Ceiling((double) array.Length / (double) num1);
    float num3 = (float) (((double) ((Control) this).Size.X - (double) num2 * (double) size.X) * 0.5);
    for (int index = 0; index < array.Length; ++index)
    {
      int num4 = index / num2;
      int num5 = index - num4 * num2;
      array[index].Position = Vector2.op_Addition(Vector2.op_Multiply(new Vector2((float) num5, (float) num4), size), Vector2.op_Multiply(Vector2.Right, num3));
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NJoinFriendScreenButtonLayout.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NJoinFriendScreenButtonLayout.MethodName.LayoutChildren, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NJoinFriendScreenButtonLayout.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NJoinFriendScreenButtonLayout.MethodName.LayoutChildren) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.LayoutChildren();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NJoinFriendScreenButtonLayout.MethodName._Notification) || StringName.op_Equality(ref method, NJoinFriendScreenButtonLayout.MethodName.LayoutChildren) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : Container.MethodName
  {
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName LayoutChildren = StringName.op_Implicit(nameof (LayoutChildren));
  }

  public class PropertyName : Container.PropertyName
  {
  }

  public class SignalName : Container.SignalName
  {
  }
}
