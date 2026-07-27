// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NAchievementsGrid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NAchievementsGrid.cs")]
public class NAchievementsGrid : Control
{
  private Control _achievementsContainer;
  private bool _scrollbarPressed;
  private Vector2 _startDragPos;
  private Vector2 _targetDragPos;
  private bool _isDragging;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<Achievement>) Enum.GetValues<Achievement>()).Select<Achievement, string>((Func<Achievement, string>) (a => NAchievementHolder.GetPathForAchievement((Enum) a)));
    }
  }

  private float ScrollLimitBottom => -this._achievementsContainer.Size.Y + this.Size.Y;

  public override void _Ready()
  {
    this._achievementsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AchievementsContainer"));
    List<NAchievementHolder> nachievementHolderList = new List<NAchievementHolder>();
    foreach (Achievement achievement in Enum.GetValues<Achievement>())
    {
      NAchievementHolder child = NAchievementHolder.Create(achievement);
      if (child.IsUnlocked)
        ((Node) this._achievementsContainer).AddChildSafely((Node) child);
      else
        nachievementHolderList.Add(child);
    }
    foreach (Node child in nachievementHolderList)
      ((Node) this._achievementsContainer).AddChildSafely(child);
  }

  private void OnAchievementsChanged()
  {
    foreach (NAchievementHolder nachievementHolder in ((IEnumerable) ((Node) this._achievementsContainer).GetChildren(false)).OfType<NAchievementHolder>())
      nachievementHolder.RefreshUnlocked();
  }

  public override void _EnterTree()
  {
    AchievementsUtil.AchievementsChanged += new Action(this.OnAchievementsChanged);
  }

  public override void _ExitTree()
  {
    AchievementsUtil.AchievementsChanged -= new Action(this.OnAchievementsChanged);
  }

  public Control DefaultFocusedControl
  {
    get
    {
      return (Control) ((IEnumerable) ((Node) this._achievementsContainer).GetChildren(false)).OfType<NAchievementHolder>().First<NAchievementHolder>();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NAchievementsGrid.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAchievementsGrid.MethodName.OnAchievementsChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAchievementsGrid.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAchievementsGrid.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAchievementsGrid.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAchievementsGrid.MethodName.OnAchievementsChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAchievementsChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAchievementsGrid.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAchievementsGrid.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAchievementsGrid.MethodName._Ready) || StringName.op_Equality(ref method, NAchievementsGrid.MethodName.OnAchievementsChanged) || StringName.op_Equality(ref method, NAchievementsGrid.MethodName._EnterTree) || StringName.op_Equality(ref method, NAchievementsGrid.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._achievementsContainer))
    {
      this._achievementsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._scrollbarPressed))
    {
      this._scrollbarPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._startDragPos))
    {
      this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._targetDragPos))
    {
      this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._isDragging))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isDragging = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName.ScrollLimitBottom))
    {
      ref godot_variant local = ref value;
      float scrollLimitBottom = this.ScrollLimitBottom;
      godot_variant from = VariantUtils.CreateFrom<float>(ref scrollLimitBottom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._achievementsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._achievementsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._scrollbarPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._scrollbarPressed);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._startDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._targetDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAchievementsGrid.PropertyName._isDragging))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isDragging);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAchievementsGrid.PropertyName._achievementsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NAchievementsGrid.PropertyName._scrollbarPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAchievementsGrid.PropertyName._startDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NAchievementsGrid.PropertyName._targetDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NAchievementsGrid.PropertyName._isDragging, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NAchievementsGrid.PropertyName.ScrollLimitBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementsGrid.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAchievementsGrid.PropertyName._achievementsContainer, Variant.From<Control>(ref this._achievementsContainer));
    info.AddProperty(NAchievementsGrid.PropertyName._scrollbarPressed, Variant.From<bool>(ref this._scrollbarPressed));
    info.AddProperty(NAchievementsGrid.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
    info.AddProperty(NAchievementsGrid.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
    info.AddProperty(NAchievementsGrid.PropertyName._isDragging, Variant.From<bool>(ref this._isDragging));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAchievementsGrid.PropertyName._achievementsContainer, ref variant1))
      this._achievementsContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NAchievementsGrid.PropertyName._scrollbarPressed, ref variant2))
      this._scrollbarPressed = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NAchievementsGrid.PropertyName._startDragPos, ref variant3))
      this._startDragPos = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NAchievementsGrid.PropertyName._targetDragPos, ref variant4))
      this._targetDragPos = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (!info.TryGetProperty(NAchievementsGrid.PropertyName._isDragging, ref variant5))
      return;
    this._isDragging = ((Variant) ref variant5).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAchievementsChanged = StringName.op_Implicit(nameof (OnAchievementsChanged));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName ScrollLimitBottom = StringName.op_Implicit(nameof (ScrollLimitBottom));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _achievementsContainer = StringName.op_Implicit(nameof (_achievementsContainer));
    public static readonly StringName _scrollbarPressed = StringName.op_Implicit(nameof (_scrollbarPressed));
    public static readonly StringName _startDragPos = StringName.op_Implicit(nameof (_startDragPos));
    public static readonly StringName _targetDragPos = StringName.op_Implicit(nameof (_targetDragPos));
    public static readonly StringName _isDragging = StringName.op_Implicit(nameof (_isDragging));
  }

  public class SignalName : Control.SignalName
  {
  }
}
