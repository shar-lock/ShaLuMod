// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen.NModdingScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModdingScreen.cs")]
public class NModdingScreen : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/modding/modding_screen");
  private NModInfoContainer _modInfoContainer;
  private NScrollableContainer _scrollableContainer;
  private Control _modRowContainer;
  private Control _pendingChangesWarning;

  protected override Control? InitialFocusedControl
  {
    get
    {
      return (Control) ((IEnumerable) ((Node) this._modRowContainer).GetChildren(false)).OfType<NModMenuRow>().FirstOrDefault<NModMenuRow>();
    }
  }

  public static string[] AssetPaths
  {
    get => new string[1]{ NModdingScreen._scenePath };
  }

  public static NModdingScreen? Create()
  {
    return TestMode.IsOn ? (NModdingScreen) null : PreloadManager.Cache.GetScene(NModdingScreen._scenePath).Instantiate<NModdingScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._modInfoContainer = ((Node) this).GetNode<NModInfoContainer>(NodePath.op_Implicit("%ModInfoContainer"));
    this._scrollableContainer = ((Node) this).GetNode<NScrollableContainer>(NodePath.op_Implicit("%ModsScrollContainer"));
    this._modRowContainer = ((Node) this._scrollableContainer).GetNode<Control>(NodePath.op_Implicit("Mask/Content"));
    this._pendingChangesWarning = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PendingChangesLabel"));
    NButton node1 = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%GetModsButton"));
    NButton node2 = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%MakeModsButton"));
    foreach (Node child in ((Node) this._modRowContainer).GetChildren(false))
      child.QueueFreeSafely();
    foreach (Mod mod in (IEnumerable<Mod>) ModManager.Mods)
      this.OnNewModDetected(mod);
    ((GodotObject) node1).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnGetModsPressed)), 0U);
    ((GodotObject) node2).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMakeModsPressed)), 0U);
    ((Node) node1).GetNode<MegaLabel>(NodePath.op_Implicit("Visuals/Label")).SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.GET_MODS_BUTTON").GetFormattedText());
    ((Node) node2).GetNode<MegaLabel>(NodePath.op_Implicit("Visuals/Label")).SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.MAKE_MODS_BUTTON").GetFormattedText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%InstalledModsTitle")).SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.INSTALLED_MODS_TITLE").GetFormattedText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%PendingChangesLabel")).SetTextAutoSize(new LocString("settings_ui", "MODDING_SCREEN.PENDING_CHANGES_WARNING").GetFormattedText());
    ((CanvasItem) this._pendingChangesWarning).Visible = false;
    ModManager.OnModDetected += new Action<Mod>(this.OnNewModDetected);
    this.ConnectSignals();
  }

  public override void OnSubmenuOpened()
  {
    if (ModManager.PlayerAgreedToModLoading || ModManager.Mods.Count <= 0)
      return;
    NModalContainer.Instance.Add((Node) NConfirmModLoadingPopup.Create());
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    foreach (NModMenuRow nmodMenuRow in ((IEnumerable) ((Node) this._modRowContainer).GetChildren(false)).OfType<NModMenuRow>())
      nmodMenuRow.SetSelected(false);
    this._modInfoContainer.Clear();
  }

  private void OnGetModsPressed(NButton _)
  {
    PlatformUtil.OpenUrl("https://steamcommunity.com/app/2868840/workshop/");
  }

  private void OnMakeModsPressed(NButton _)
  {
    PlatformUtil.OpenUrl("https://github.com/Alchyr/ModTemplate-StS2");
  }

  public void OnRowSelected(NModMenuRow row)
  {
    row.SetSelected(true);
    this._modInfoContainer.Fill(row.Mod);
    foreach (NModMenuRow nmodMenuRow in ((IEnumerable) ((Node) this._modRowContainer).GetChildren(false)).OfType<NModMenuRow>())
    {
      if (nmodMenuRow != row)
        nmodMenuRow.SetSelected(false);
    }
  }

  private void OnNewModDetected(Mod mod)
  {
    ((Node) this._modRowContainer).AddChildSafely((Node) NModMenuRow.Create(this, mod));
    this.OnModEnabledOrDisabled();
    this._scrollableContainer.DisableScrollingIfContentFits();
  }

  public void OnModEnabledOrDisabled()
  {
    foreach (Mod mod in (IEnumerable<Mod>) ModManager.Mods)
    {
      ModSettings modSettings = SaveManager.Instance.SettingsSave.ModSettings;
      bool flag = modSettings != null && modSettings.IsModDisabled(mod);
      if (mod.state == ModLoadState.Disabled && !flag || mod.state == ModLoadState.Loaded & flag)
      {
        ((CanvasItem) this._pendingChangesWarning).Visible = true;
        return;
      }
    }
    ((CanvasItem) this._pendingChangesWarning).Visible = false;
  }

  public override void _ExitTree()
  {
    ModManager.OnModDetected -= new Action<Mod>(this.OnNewModDetected);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NModdingScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName.OnGetModsPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName.OnMakeModsPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName.OnRowSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("row"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName.OnModEnabledOrDisabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModdingScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NModdingScreen nmoddingScreen = NModdingScreen.Create();
      ret = VariantUtils.CreateFrom<NModdingScreen>(ref nmoddingScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.OnGetModsPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnGetModsPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.OnMakeModsPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMakeModsPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.OnRowSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRowSelected(VariantUtils.ConvertTo<NModMenuRow>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.OnModEnabledOrDisabled) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnModEnabledOrDisabled();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NModdingScreen.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NModdingScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NModdingScreen nmoddingScreen = NModdingScreen.Create();
      ret = VariantUtils.CreateFrom<NModdingScreen>(ref nmoddingScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NModdingScreen.MethodName.Create) || StringName.op_Equality(ref method, NModdingScreen.MethodName._Ready) || StringName.op_Equality(ref method, NModdingScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NModdingScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NModdingScreen.MethodName.OnGetModsPressed) || StringName.op_Equality(ref method, NModdingScreen.MethodName.OnMakeModsPressed) || StringName.op_Equality(ref method, NModdingScreen.MethodName.OnRowSelected) || StringName.op_Equality(ref method, NModdingScreen.MethodName.OnModEnabledOrDisabled) || StringName.op_Equality(ref method, NModdingScreen.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName._modInfoContainer))
    {
      this._modInfoContainer = VariantUtils.ConvertTo<NModInfoContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName._scrollableContainer))
    {
      this._scrollableContainer = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName._modRowContainer))
    {
      this._modRowContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModdingScreen.PropertyName._pendingChangesWarning))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._pendingChangesWarning = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName._modInfoContainer))
    {
      value = VariantUtils.CreateFrom<NModInfoContainer>(ref this._modInfoContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName._scrollableContainer))
    {
      value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._scrollableContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NModdingScreen.PropertyName._modRowContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._modRowContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModdingScreen.PropertyName._pendingChangesWarning))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._pendingChangesWarning);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NModdingScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModdingScreen.PropertyName._modInfoContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModdingScreen.PropertyName._scrollableContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModdingScreen.PropertyName._modRowContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModdingScreen.PropertyName._pendingChangesWarning, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NModdingScreen.PropertyName._modInfoContainer, Variant.From<NModInfoContainer>(ref this._modInfoContainer));
    info.AddProperty(NModdingScreen.PropertyName._scrollableContainer, Variant.From<NScrollableContainer>(ref this._scrollableContainer));
    info.AddProperty(NModdingScreen.PropertyName._modRowContainer, Variant.From<Control>(ref this._modRowContainer));
    info.AddProperty(NModdingScreen.PropertyName._pendingChangesWarning, Variant.From<Control>(ref this._pendingChangesWarning));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NModdingScreen.PropertyName._modInfoContainer, ref variant1))
      this._modInfoContainer = ((Variant) ref variant1).As<NModInfoContainer>();
    Variant variant2;
    if (info.TryGetProperty(NModdingScreen.PropertyName._scrollableContainer, ref variant2))
      this._scrollableContainer = ((Variant) ref variant2).As<NScrollableContainer>();
    Variant variant3;
    if (info.TryGetProperty(NModdingScreen.PropertyName._modRowContainer, ref variant3))
      this._modRowContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NModdingScreen.PropertyName._pendingChangesWarning, ref variant4))
      return;
    this._pendingChangesWarning = ((Variant) ref variant4).As<Control>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName OnGetModsPressed = StringName.op_Implicit(nameof (OnGetModsPressed));
    public static readonly StringName OnMakeModsPressed = StringName.op_Implicit(nameof (OnMakeModsPressed));
    public static readonly StringName OnRowSelected = StringName.op_Implicit(nameof (OnRowSelected));
    public static readonly StringName OnModEnabledOrDisabled = StringName.op_Implicit(nameof (OnModEnabledOrDisabled));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _modInfoContainer = StringName.op_Implicit(nameof (_modInfoContainer));
    public static readonly StringName _scrollableContainer = StringName.op_Implicit(nameof (_scrollableContainer));
    public static readonly StringName _modRowContainer = StringName.op_Implicit(nameof (_modRowContainer));
    public static readonly StringName _pendingChangesWarning = StringName.op_Implicit(nameof (_pendingChangesWarning));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
