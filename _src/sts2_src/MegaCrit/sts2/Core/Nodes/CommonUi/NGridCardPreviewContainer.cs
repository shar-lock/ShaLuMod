// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NGridCardPreviewContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NGridCardPreviewContainer.cs")]
public class NGridCardPreviewContainer : Control
{
  private int? _forcedMaxColumns;

  public override void _Ready()
  {
    ((GodotObject) this).Connect(Node.SignalName.ChildEnteredTree, Callable.From<Node>(new Action<Node>(this.ReformatElements)), 0U);
    ((GodotObject) this).Connect(Node.SignalName.ChildExitingTree, Callable.From<Node>(new Action<Node>(this.CheckAnyChildrenPresent)), 0U);
  }

  public void ForceMaxColumnsUntilEmpty(int maxColumns)
  {
    this._forcedMaxColumns = new int?(maxColumns);
  }

  private void ReformatElements(Node _)
  {
    Vector2 vector2_1 = Vector2.op_Addition(Vector2.op_Division(this.Size, 2f), Vector2.op_Multiply(Vector2.Down, 50f));
    int childCount = ((Node) this).GetChildCount(false);
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    int num1 = Mathf.FloorToInt(((Rect2) ref viewportRect).Size.X / (NCard.defaultSize.X + 25f));
    int num2 = Mathf.CeilToInt((float) childCount / (float) num1);
    if (this._forcedMaxColumns.HasValue)
      num1 = Math.Min(num1, this._forcedMaxColumns.Value);
    for (int index = 0; index < childCount; ++index)
    {
      int num3 = index / num1;
      int num4 = index % num1;
      int num5 = Math.Min(num1, childCount - num3 * num1);
      float num6 = (float) ((double) -(num2 - 1) * ((double) NCard.defaultSize.Y + 25.0) * 0.5);
      float num7 = (float) ((double) -(num5 - 1) * ((double) NCard.defaultSize.X + 25.0) * 0.5);
      Vector2 vector2_2 = Vector2.op_Addition(vector2_1, new Vector2(num7 + (NCard.defaultSize.X + 25f) * (float) num4, num6 + (NCard.defaultSize.Y + 25f) * (float) num3));
      Node child = ((Node) this).GetChild(index, false);
      if (child is Node2D node2D)
        node2D.Position = vector2_2;
      else
        ((Control) child).Position = vector2_2;
    }
  }

  private void CheckAnyChildrenPresent(Node _)
  {
    if (((Node) this).GetChildCount(false) != 0)
      return;
    this._forcedMaxColumns = new int?();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NGridCardPreviewContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardPreviewContainer.MethodName.ForceMaxColumnsUntilEmpty, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("maxColumns"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGridCardPreviewContainer.MethodName.ReformatElements, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGridCardPreviewContainer.MethodName.CheckAnyChildrenPresent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName.ForceMaxColumnsUntilEmpty) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ForceMaxColumnsUntilEmpty(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName.ReformatElements) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ReformatElements(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName.CheckAnyChildrenPresent) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.CheckAnyChildrenPresent(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName._Ready) || StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName.ForceMaxColumnsUntilEmpty) || StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName.ReformatElements) || StringName.op_Equality(ref method, NGridCardPreviewContainer.MethodName.CheckAnyChildrenPresent) || base.HasGodotClassMethod(ref method);
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

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ForceMaxColumnsUntilEmpty = StringName.op_Implicit(nameof (ForceMaxColumnsUntilEmpty));
    public static readonly StringName ReformatElements = StringName.op_Implicit(nameof (ReformatElements));
    public static readonly StringName CheckAnyChildrenPresent = StringName.op_Implicit(nameof (CheckAnyChildrenPresent));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
