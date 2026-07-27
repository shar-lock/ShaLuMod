// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NCapstoneSubmenuStack
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NCapstoneSubmenuStack.cs")]
public class NCapstoneSubmenuStack : Control, ICapstoneScreen, IScreenContext
{
  private static string ScenePath => SceneHelper.GetScenePath("screens/capstone_submenu_stack");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCapstoneSubmenuStack.ScenePath);
    }
  }

  public CapstoneSubmenuType Type { get; private set; }

  public NRunSubmenuStack Stack { get; private set; }

  public NetScreenType ScreenType => this.GetCapstoneSubmenuType();

  public NSubmenu ShowScreen(CapstoneSubmenuType type)
  {
    while (this.Stack.Peek() != null)
      this.Stack.Pop();
    System.Type type1;
    switch (type)
    {
      case CapstoneSubmenuType.Settings:
        type1 = typeof (NSettingsScreen);
        break;
      case CapstoneSubmenuType.Compendium:
        type1 = typeof (NCompendiumSubmenu);
        break;
      case CapstoneSubmenuType.Feedback:
        type1 = typeof (NSendFeedbackScreen);
        break;
      case CapstoneSubmenuType.PauseMenu:
        type1 = typeof (NPauseMenu);
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
    NSubmenu nsubmenu = this.Stack.PushSubmenuType(type1);
    this.Type = type;
    NCapstoneContainer.Instance.Open((ICapstoneScreen) this);
    return nsubmenu;
  }

  private NetScreenType GetCapstoneSubmenuType()
  {
    switch (this.Type)
    {
      case CapstoneSubmenuType.Settings:
        return NetScreenType.Settings;
      case CapstoneSubmenuType.Compendium:
        return NetScreenType.Compendium;
      case CapstoneSubmenuType.Feedback:
        return NetScreenType.Feedback;
      case CapstoneSubmenuType.PauseMenu:
        return NetScreenType.PauseMenu;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public override void _Ready()
  {
    this.Stack = ((Node) this).GetNode<NRunSubmenuStack>(NodePath.op_Implicit("%Submenus"));
    ((GodotObject) this.Stack).Connect(NSubmenuStack.SignalName.StackModified, Callable.From(new Action(this.OnSubmenuStackChanged)), 0U);
  }

  private void OnSubmenuStackChanged()
  {
    if (this.Stack.Peek() != null || NCapstoneContainer.Instance.CurrentCapstoneScreen != this)
      return;
    NCapstoneContainer.Instance.Close();
  }

  public void AfterCapstoneOpened()
  {
    NGlobalUi globalUi = NRun.Instance.GlobalUi;
    globalUi.TopBar.AnimHide();
    globalUi.RelicInventory.AnimHide();
    globalUi.MultiplayerPlayerContainer.AnimHide();
    SfxCmd.Play("event:/sfx/ui/pause_open");
    ((Node) globalUi).MoveChildSafely((Node) globalUi.CardPreviewContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((Node) globalUi).MoveChildSafely((Node) globalUi.GridCardPreviewContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((Node) globalUi).MoveChildSafely((Node) globalUi.EventCardPreviewContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((Node) globalUi).MoveChildSafely((Node) globalUi.MessyCardPreviewContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((Node) globalUi).MoveChildSafely((Node) globalUi.AboveTopBarVfxContainer, ((Node) globalUi.CapstoneContainer).GetIndex(false));
    ((CanvasItem) this).Visible = true;
  }

  public void AfterCapstoneClosed()
  {
    while (this.Stack.Peek() != null)
      this.Stack.Pop();
    SfxCmd.Play("event:/sfx/ui/pause_close");
    NGlobalUi globalUi = NRun.Instance.GlobalUi;
    globalUi.TopBar.AnimShow();
    globalUi.RelicInventory.AnimShow();
    globalUi.MultiplayerPlayerContainer.AnimShow();
    ((Node) globalUi).MoveChildSafely((Node) globalUi.AboveTopBarVfxContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) globalUi).MoveChildSafely((Node) globalUi.MessyCardPreviewContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) globalUi).MoveChildSafely((Node) globalUi.EventCardPreviewContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) globalUi).MoveChildSafely((Node) globalUi.GridCardPreviewContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((Node) globalUi).MoveChildSafely((Node) globalUi.CardPreviewContainer, ((Node) globalUi.TopBar).GetIndex(false) + 1);
    ((CanvasItem) this).Visible = false;
  }

  public bool UseSharedBackstop => true;

  public Control? DefaultFocusedControl => this.Stack.Peek()?.DefaultFocusedControl;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NCapstoneSubmenuStack.MethodName.ShowScreen, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("type"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCapstoneSubmenuStack.MethodName.GetCapstoneSubmenuType, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneSubmenuStack.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneSubmenuStack.MethodName.OnSubmenuStackChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneSubmenuStack.MethodName.AfterCapstoneOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCapstoneSubmenuStack.MethodName.AfterCapstoneClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.ShowScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NSubmenu nsubmenu = this.ShowScreen(VariantUtils.ConvertTo<CapstoneSubmenuType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NSubmenu>(ref nsubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.GetCapstoneSubmenuType) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NetScreenType capstoneSubmenuType = this.GetCapstoneSubmenuType();
      ret = VariantUtils.CreateFrom<NetScreenType>(ref capstoneSubmenuType);
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.OnSubmenuStackChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuStackChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.AfterCapstoneOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterCapstoneOpened();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.AfterCapstoneClosed) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AfterCapstoneClosed();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.ShowScreen) || StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.GetCapstoneSubmenuType) || StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName._Ready) || StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.OnSubmenuStackChanged) || StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.AfterCapstoneOpened) || StringName.op_Equality(ref method, NCapstoneSubmenuStack.MethodName.AfterCapstoneClosed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.Type))
    {
      this.Type = VariantUtils.ConvertTo<CapstoneSubmenuType>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.Stack))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.Stack = VariantUtils.ConvertTo<NRunSubmenuStack>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.Type))
    {
      ref godot_variant local = ref value;
      CapstoneSubmenuType type = this.Type;
      godot_variant from = VariantUtils.CreateFrom<CapstoneSubmenuType>(ref type);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.Stack))
    {
      ref godot_variant local = ref value;
      NRunSubmenuStack stack = this.Stack;
      godot_variant from = VariantUtils.CreateFrom<NRunSubmenuStack>(ref stack);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NCapstoneSubmenuStack.PropertyName.DefaultFocusedControl))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    ref godot_variant local1 = ref value;
    Control defaultFocusedControl = this.DefaultFocusedControl;
    godot_variant from1 = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
    local1 = from1;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NCapstoneSubmenuStack.PropertyName.Type, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCapstoneSubmenuStack.PropertyName.Stack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCapstoneSubmenuStack.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCapstoneSubmenuStack.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCapstoneSubmenuStack.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName type1 = NCapstoneSubmenuStack.PropertyName.Type;
    CapstoneSubmenuType type2 = this.Type;
    Variant variant1 = Variant.From<CapstoneSubmenuType>(ref type2);
    serializationInfo1.AddProperty(type1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName stack1 = NCapstoneSubmenuStack.PropertyName.Stack;
    NRunSubmenuStack stack2 = this.Stack;
    Variant variant2 = Variant.From<NRunSubmenuStack>(ref stack2);
    serializationInfo2.AddProperty(stack1, variant2);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCapstoneSubmenuStack.PropertyName.Type, ref variant1))
      this.Type = ((Variant) ref variant1).As<CapstoneSubmenuType>();
    Variant variant2;
    if (!info.TryGetProperty(NCapstoneSubmenuStack.PropertyName.Stack, ref variant2))
      return;
    this.Stack = ((Variant) ref variant2).As<NRunSubmenuStack>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName ShowScreen = StringName.op_Implicit(nameof (ShowScreen));
    public static readonly StringName GetCapstoneSubmenuType = StringName.op_Implicit(nameof (GetCapstoneSubmenuType));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnSubmenuStackChanged = StringName.op_Implicit(nameof (OnSubmenuStackChanged));
    public static readonly StringName AfterCapstoneOpened = StringName.op_Implicit(nameof (AfterCapstoneOpened));
    public static readonly StringName AfterCapstoneClosed = StringName.op_Implicit(nameof (AfterCapstoneClosed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Type = StringName.op_Implicit(nameof (Type));
    public static readonly StringName Stack = StringName.op_Implicit(nameof (Stack));
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
  }

  public class SignalName : Control.SignalName
  {
  }
}
