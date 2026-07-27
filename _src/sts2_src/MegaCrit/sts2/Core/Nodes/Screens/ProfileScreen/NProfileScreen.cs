// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen.NProfileScreen
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
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NProfileScreen.cs")]
public class NProfileScreen : NSubmenu
{
  public static int? forceShowProfileAsDeleted;
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/profiles/profile_screen");
  private Control _loadingOverlay;
  private readonly List<NProfileButton> _profileButtons = new List<NProfileButton>();
  private readonly List<NDeleteProfileButton> _deleteButtons = new List<NDeleteProfileButton>();

  public static string[] AssetPaths
  {
    get
    {
      List<string> stringList = new List<string>();
      stringList.Add(NProfileScreen._scenePath);
      stringList.AddRange(NProfileButton.AssetPaths);
      return stringList.ToArray();
    }
  }

  protected override Control InitialFocusedControl
  {
    get => (Control) this._profileButtons[SaveManager.Instance.CurrentProfileId - 1];
  }

  public static NProfileScreen? Create()
  {
    return TestMode.IsOn ? (NProfileScreen) null : PreloadManager.Cache.GetScene(NProfileScreen._scenePath).Instantiate<NProfileScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._loadingOverlay = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LoadingOverlay"));
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ChooseProfileMessage")).SetTextAutoSize(new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.chooseProfileMessage").GetFormattedText());
    this._profileButtons.Add(((Node) this).GetNode<NProfileButton>(NodePath.op_Implicit("%ProfileButton1")));
    this._profileButtons.Add(((Node) this).GetNode<NProfileButton>(NodePath.op_Implicit("%ProfileButton2")));
    this._profileButtons.Add(((Node) this).GetNode<NProfileButton>(NodePath.op_Implicit("%ProfileButton3")));
    this._deleteButtons.Add(((Node) this).GetNode<NDeleteProfileButton>(NodePath.op_Implicit("%DeleteProfileButton1")));
    this._deleteButtons.Add(((Node) this).GetNode<NDeleteProfileButton>(NodePath.op_Implicit("%DeleteProfileButton2")));
    this._deleteButtons.Add(((Node) this).GetNode<NDeleteProfileButton>(NodePath.op_Implicit("%DeleteProfileButton3")));
    if (this._profileButtons.Count != 3)
      Log.Error($"There are {this._profileButtons.Count} profile buttons, but max profile count in ProfileSaveManager is {3}! This might result in subtle bugs");
    for (int index = 0; index < this._profileButtons.Count; ++index)
    {
      this._profileButtons[index].FocusNeighborTop = ((Node) this._profileButtons[index]).GetPath();
      this._profileButtons[index].FocusNeighborBottom = ((Node) this._deleteButtons[index]).GetPath();
      NProfileButton profileButton = this._profileButtons[index];
      NodePath path1;
      if (index <= 0)
      {
        List<NProfileButton> profileButtons = this._profileButtons;
        path1 = ((Node) profileButtons[profileButtons.Count - 1]).GetPath();
      }
      else
        path1 = ((Node) this._profileButtons[index - 1]).GetPath();
      profileButton.FocusNeighborLeft = path1;
      this._profileButtons[index].FocusNeighborRight = index < this._profileButtons.Count - 1 ? ((Node) this._profileButtons[index + 1]).GetPath() : ((Node) this._profileButtons[0]).GetPath();
      this._deleteButtons[index].FocusNeighborTop = ((Node) this._profileButtons[index]).GetPath();
      this._deleteButtons[index].FocusNeighborBottom = ((Node) this._deleteButtons[index]).GetPath();
      NDeleteProfileButton deleteButton = this._deleteButtons[index];
      NodePath path2;
      if (index <= 0)
      {
        List<NDeleteProfileButton> deleteButtons = this._deleteButtons;
        path2 = ((Node) deleteButtons[deleteButtons.Count - 1]).GetPath();
      }
      else
        path2 = ((Node) this._deleteButtons[index - 1]).GetPath();
      deleteButton.FocusNeighborLeft = path2;
      this._deleteButtons[index].FocusNeighborRight = index < this._deleteButtons.Count - 1 ? ((Node) this._deleteButtons[index + 1]).GetPath() : ((Node) this._deleteButtons[0]).GetPath();
    }
  }

  public override void OnSubmenuOpened() => this.Refresh();

  public void ShowLoading() => ((CanvasItem) this._loadingOverlay).Visible = true;

  public void Refresh()
  {
    for (int index = 0; index < this._profileButtons.Count; ++index)
      this._profileButtons[index].Initialize(this, index + 1);
    for (int index = 0; index < this._deleteButtons.Count; ++index)
      this._deleteButtons[index].Initialize(this, index + 1);
    NProfileScreen.forceShowProfileAsDeleted = new int?();
    ActiveScreenContext.Instance.Update();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NProfileScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileScreen.MethodName.ShowLoading, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileScreen.MethodName.Refresh, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NProfileScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NProfileScreen nprofileScreen = NProfileScreen.Create();
      ret = VariantUtils.CreateFrom<NProfileScreen>(ref nprofileScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileScreen.MethodName.ShowLoading) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowLoading();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NProfileScreen.MethodName.Refresh) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Refresh();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NProfileScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NProfileScreen nprofileScreen = NProfileScreen.Create();
      ret = VariantUtils.CreateFrom<NProfileScreen>(ref nprofileScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NProfileScreen.MethodName.Create) || StringName.op_Equality(ref method, NProfileScreen.MethodName._Ready) || StringName.op_Equality(ref method, NProfileScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NProfileScreen.MethodName.ShowLoading) || StringName.op_Equality(ref method, NProfileScreen.MethodName.Refresh) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NProfileScreen.PropertyName._loadingOverlay))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NProfileScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NProfileScreen.PropertyName._loadingOverlay))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NProfileScreen.PropertyName._loadingOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NProfileScreen.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NProfileScreen.PropertyName._loadingOverlay, ref variant))
      return;
    this._loadingOverlay = ((Variant) ref variant).As<Control>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public static readonly StringName ShowLoading = StringName.op_Implicit(nameof (ShowLoading));
    public static readonly StringName Refresh = StringName.op_Implicit(nameof (Refresh));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _loadingOverlay = StringName.op_Implicit(nameof (_loadingOverlay));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
