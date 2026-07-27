// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSingleplayerSubmenu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSingleplayerSubmenu.cs")]
public class NSingleplayerSubmenu : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/singleplayer_submenu");
  private NSubmenuButton _standardButton;
  private NSubmenuButton _dailyButton;
  private NSubmenuButton _customButton;
  private const string _keyStandard = "STANDARD";
  private const string _keyDaily = "DAILY";
  private const string _keyCustom = "CUSTOM";

  protected override Control InitialFocusedControl => (Control) this._standardButton;

  public static NSingleplayerSubmenu? Create()
  {
    return TestMode.IsOn ? (NSingleplayerSubmenu) null : PreloadManager.Cache.GetScene(NSingleplayerSubmenu._scenePath).Instantiate<NSingleplayerSubmenu>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._standardButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("StandardButton"));
    ((GodotObject) this._standardButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCharacterSelect)), 0U);
    this._standardButton.SetIconAndLocalization("STANDARD");
    this._dailyButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("DailyButton"));
    ((GodotObject) this._dailyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenDailyScreen)), 0U);
    this._dailyButton.SetIconAndLocalization("DAILY");
    this._customButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("CustomRunButton"));
    ((GodotObject) this._customButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCustomScreen)), 0U);
    this._customButton.SetIconAndLocalization("CUSTOM");
  }

  private void RefreshButtons()
  {
    this._dailyButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<DailyRunEpoch>());
    this._customButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<CustomAndSeedsEpoch>());
    this._dailyButton.RefreshLabels();
    this._customButton.RefreshLabels();
  }

  public override void OnSubmenuOpened() => this.RefreshButtons();

  private void OpenCharacterSelect(NButton _)
  {
    NCharacterSelectScreen submenuType = this._stack.GetSubmenuType<NCharacterSelectScreen>();
    submenuType.InitializeSingleplayer();
    this._stack.Push((NSubmenu) submenuType);
  }

  private void OpenDailyScreen(NButton _) => this.OpenDailyScreen();

  private void OpenDailyScreen()
  {
    NDailyRunScreen submenuType = this._stack.GetSubmenuType<NDailyRunScreen>();
    submenuType.InitializeSingleplayer();
    this._stack.Push((NSubmenu) submenuType);
  }

  private void OpenCustomScreen(NButton _)
  {
    NCustomRunScreen submenuType = this._stack.GetSubmenuType<NCustomRunScreen>();
    submenuType.InitializeSingleplayer();
    this._stack.Push((NSubmenu) submenuType);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NSingleplayerSubmenu.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName.RefreshButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName.OpenCharacterSelect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName.OpenDailyScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName.OpenDailyScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSingleplayerSubmenu.MethodName.OpenCustomScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSingleplayerSubmenu nsingleplayerSubmenu = NSingleplayerSubmenu.Create();
      ret = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref nsingleplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.RefreshButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenCharacterSelect) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenCharacterSelect(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenDailyScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenDailyScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenDailyScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenDailyScreen();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenCustomScreen) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OpenCustomScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSingleplayerSubmenu nsingleplayerSubmenu = NSingleplayerSubmenu.Create();
      ret = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref nsingleplayerSubmenu);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.Create) || StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName._Ready) || StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.RefreshButtons) || StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenCharacterSelect) || StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenDailyScreen) || StringName.op_Equality(ref method, NSingleplayerSubmenu.MethodName.OpenCustomScreen) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName._standardButton))
    {
      this._standardButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName._dailyButton))
    {
      this._dailyButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName._customButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._customButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName._standardButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._standardButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName._dailyButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._dailyButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSingleplayerSubmenu.PropertyName._customButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._customButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSingleplayerSubmenu.PropertyName._standardButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSingleplayerSubmenu.PropertyName._dailyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSingleplayerSubmenu.PropertyName._customButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSingleplayerSubmenu.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSingleplayerSubmenu.PropertyName._standardButton, Variant.From<NSubmenuButton>(ref this._standardButton));
    info.AddProperty(NSingleplayerSubmenu.PropertyName._dailyButton, Variant.From<NSubmenuButton>(ref this._dailyButton));
    info.AddProperty(NSingleplayerSubmenu.PropertyName._customButton, Variant.From<NSubmenuButton>(ref this._customButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSingleplayerSubmenu.PropertyName._standardButton, ref variant1))
      this._standardButton = ((Variant) ref variant1).As<NSubmenuButton>();
    Variant variant2;
    if (info.TryGetProperty(NSingleplayerSubmenu.PropertyName._dailyButton, ref variant2))
      this._dailyButton = ((Variant) ref variant2).As<NSubmenuButton>();
    Variant variant3;
    if (!info.TryGetProperty(NSingleplayerSubmenu.PropertyName._customButton, ref variant3))
      return;
    this._customButton = ((Variant) ref variant3).As<NSubmenuButton>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshButtons = StringName.op_Implicit(nameof (RefreshButtons));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public static readonly StringName OpenCharacterSelect = StringName.op_Implicit(nameof (OpenCharacterSelect));
    public static readonly StringName OpenDailyScreen = StringName.op_Implicit(nameof (OpenDailyScreen));
    public static readonly StringName OpenCustomScreen = StringName.op_Implicit(nameof (OpenCustomScreen));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _standardButton = StringName.op_Implicit(nameof (_standardButton));
    public static readonly StringName _dailyButton = StringName.op_Implicit(nameof (_dailyButton));
    public static readonly StringName _customButton = StringName.op_Implicit(nameof (_customButton));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
