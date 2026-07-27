// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NResolutionDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResolutionDropdown.cs")]
public class NResolutionDropdown : NSettingsDropdown
{
  [Export]
  private PackedScene _dropdownItemScene;
  private Control _arrow;
  private static Vector2I _currentResolution;

  public static NResolutionDropdown? Instance { get; private set; }

  public override void _EnterTree() => NResolutionDropdown.Instance = this;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._arrow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Arrow"));
    ((GodotObject) NGame.Instance).Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
    this.RefreshEnabled();
    this.RefreshCurrentlySelectedResolution();
    this.PopulateDropdownItems();
  }

  public void RefreshCurrentlySelectedResolution()
  {
    if (!this.IsEnabled)
      return;
    NResolutionDropdown._currentResolution = DisplayServer.WindowGetSize(0);
    this._currentOptionLabel.SetTextAutoSize($"{NResolutionDropdown._currentResolution.X} x {NResolutionDropdown._currentResolution.Y}");
  }

  public void PopulateDropdownItems()
  {
    this.ClearDropdownItems();
    Vector2I size = DisplayServer.ScreenGetSize(SaveManager.Instance.SettingsSave.TargetDisplay);
    foreach (Vector2I resolutionWhite in NResolutionDropdown.GetResolutionWhiteList())
    {
      if (NResolutionDropdown.DoesResolutionFit(resolutionWhite, size))
      {
        NResolutionDropdownItem child = this._dropdownItemScene.Instantiate<NResolutionDropdownItem>((PackedScene.GenEditState) 0L);
        ((Node) this._dropdownItems).AddChildSafely((Node) child);
        ((GodotObject) child).Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
        child.Init(resolutionWhite);
      }
    }
    ((Node) this._dropdownItems).GetParent<NDropdownContainer>().RefreshLayout();
  }

  private void OnWindowChange(bool isAutoAspectRatio)
  {
    this.RefreshEnabled();
    this.RefreshCurrentlySelectedResolution();
  }

  private void RefreshEnabled()
  {
    if (SaveManager.Instance.SettingsSave.Fullscreen || PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
      this.Disable();
    else
      this.Enable();
  }

  protected override void OnEnable()
  {
    ((CanvasItem) this._currentOptionLabel).Modulate = StsColors.gold;
    ((CanvasItem) this._arrow).Visible = true;
    this.RefreshCurrentlySelectedResolution();
  }

  protected override void OnDisable()
  {
    this._currentOptionLabel.SetTextAutoSize("N/A");
    ((CanvasItem) this._currentOptionLabel).Modulate = StsColors.gray;
    ((CanvasItem) this._arrow).Visible = false;
  }

  private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
  {
    NResolutionDropdownItem nresolutionDropdownItem = (NResolutionDropdownItem) nDropdownItem;
    if (Vector2I.op_Equality(nresolutionDropdownItem.resolution, NResolutionDropdown._currentResolution))
      return;
    this.CloseDropdown();
    SaveManager.Instance.SettingsSave.WindowPosition = Vector2I.op_Subtraction(DisplayServer.WindowGetPosition(0), DisplayServer.ScreenGetPosition(SaveManager.Instance.SettingsSave.TargetDisplay));
    SaveManager.Instance.SettingsSave.WindowSize = nresolutionDropdownItem.resolution;
    Log.Info($"Setting window size to {nresolutionDropdownItem.resolution} from dropdown");
    NGame.Instance.ApplyDisplaySettings();
  }

  private static bool DoesResolutionFit(Vector2I resolution, Vector2I boundaryResolution)
  {
    return resolution.X <= boundaryResolution.X && resolution.Y <= boundaryResolution.Y;
  }

  private static List<Vector2I> GetResolutionWhiteList()
  {
    int capacity = 26;
    List<Vector2I> resolutionWhiteList = new List<Vector2I>(capacity);
    CollectionsMarshal.SetCount<Vector2I>(resolutionWhiteList, capacity);
    Span<Vector2I> span = CollectionsMarshal.AsSpan<Vector2I>(resolutionWhiteList);
    int num1 = 0;
    span[num1] = new Vector2I(1024 /*0x0400*/, 768 /*0x0300*/);
    int num2 = num1 + 1;
    span[num2] = new Vector2I(1152, 864);
    int num3 = num2 + 1;
    span[num3] = new Vector2I(1280 /*0x0500*/, 720);
    int num4 = num3 + 1;
    span[num4] = new Vector2I(1280 /*0x0500*/, 800);
    int num5 = num4 + 1;
    span[num5] = new Vector2I(1280 /*0x0500*/, 960);
    int num6 = num5 + 1;
    span[num6] = new Vector2I(1366, 768 /*0x0300*/);
    int num7 = num6 + 1;
    span[num7] = new Vector2I(1400, 1050);
    int num8 = num7 + 1;
    span[num8] = new Vector2I(1440, 900);
    int num9 = num8 + 1;
    span[num9] = new Vector2I(1440, 1080);
    int num10 = num9 + 1;
    span[num10] = new Vector2I(1600, 900);
    int num11 = num10 + 1;
    span[num11] = new Vector2I(1600, 1200);
    int num12 = num11 + 1;
    span[num12] = new Vector2I(1680, 1050);
    int num13 = num12 + 1;
    span[num13] = new Vector2I(1856, 1392);
    int num14 = num13 + 1;
    span[num14] = new Vector2I(1920, 1080);
    int num15 = num14 + 1;
    span[num15] = new Vector2I(1920, 1200);
    int num16 = num15 + 1;
    span[num16] = new Vector2I(1920, 1440);
    int num17 = num16 + 1;
    span[num17] = new Vector2I(2048 /*0x0800*/, 1536 /*0x0600*/);
    int num18 = num17 + 1;
    span[num18] = new Vector2I(2560 /*0x0A00*/, 1080);
    int num19 = num18 + 1;
    span[num19] = new Vector2I(2560 /*0x0A00*/, 1440);
    int num20 = num19 + 1;
    span[num20] = new Vector2I(2560 /*0x0A00*/, 1600);
    int num21 = num20 + 1;
    span[num21] = new Vector2I(3200, 1800);
    int num22 = num21 + 1;
    span[num22] = new Vector2I(3440, 1440);
    int num23 = num22 + 1;
    span[num23] = new Vector2I(3840 /*0x0F00*/, 1600);
    int num24 = num23 + 1;
    span[num24] = new Vector2I(3840 /*0x0F00*/, 2160);
    int num25 = num24 + 1;
    span[num25] = new Vector2I(3840 /*0x0F00*/, 2400);
    int num26 = num25 + 1;
    span[num26] = new Vector2I(7680, 4320);
    return resolutionWhiteList;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NResolutionDropdown.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.RefreshCurrentlySelectedResolution, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.PopulateDropdownItems, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isAutoAspectRatio"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.RefreshEnabled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("nDropdownItem"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NResolutionDropdown.MethodName.DoesResolutionFit, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 6L, StringName.op_Implicit("resolution"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 6L, StringName.op_Implicit("boundaryResolution"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.RefreshCurrentlySelectedResolution) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshCurrentlySelectedResolution();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.PopulateDropdownItems) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PopulateDropdownItems();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnWindowChange(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.RefreshEnabled) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshEnabled();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnDropdownItemSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NResolutionDropdown.MethodName.DoesResolutionFit) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = NResolutionDropdown.DoesResolutionFit(VariantUtils.ConvertTo<Vector2I>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2I>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NResolutionDropdown.MethodName.DoesResolutionFit) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      bool flag = NResolutionDropdown.DoesResolutionFit(VariantUtils.ConvertTo<Vector2I>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2I>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NResolutionDropdown.MethodName._EnterTree) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.RefreshCurrentlySelectedResolution) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.PopulateDropdownItems) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.RefreshEnabled) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnEnable) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnDisable) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.OnDropdownItemSelected) || StringName.op_Equality(ref method, NResolutionDropdown.MethodName.DoesResolutionFit) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NResolutionDropdown.PropertyName._dropdownItemScene))
    {
      this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NResolutionDropdown.PropertyName._arrow))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._arrow = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NResolutionDropdown.PropertyName._dropdownItemScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
      return true;
    }
    if (!StringName.op_Equality(ref name, NResolutionDropdown.PropertyName._arrow))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._arrow);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NResolutionDropdown.PropertyName._dropdownItemScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NResolutionDropdown.PropertyName._arrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NResolutionDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
    info.AddProperty(NResolutionDropdown.PropertyName._arrow, Variant.From<Control>(ref this._arrow));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NResolutionDropdown.PropertyName._dropdownItemScene, ref variant1))
      this._dropdownItemScene = ((Variant) ref variant1).As<PackedScene>();
    Variant variant2;
    if (!info.TryGetProperty(NResolutionDropdown.PropertyName._arrow, ref variant2))
      return;
    this._arrow = ((Variant) ref variant2).As<Control>();
  }

  public new class MethodName : NSettingsDropdown.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshCurrentlySelectedResolution = StringName.op_Implicit(nameof (RefreshCurrentlySelectedResolution));
    public static readonly StringName PopulateDropdownItems = StringName.op_Implicit(nameof (PopulateDropdownItems));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName RefreshEnabled = StringName.op_Implicit(nameof (RefreshEnabled));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public static readonly StringName OnDropdownItemSelected = StringName.op_Implicit(nameof (OnDropdownItemSelected));
    public static readonly StringName DoesResolutionFit = StringName.op_Implicit(nameof (DoesResolutionFit));
  }

  public new class PropertyName : NSettingsDropdown.PropertyName
  {
    public static readonly StringName _dropdownItemScene = StringName.op_Implicit(nameof (_dropdownItemScene));
    public static readonly StringName _arrow = StringName.op_Implicit(nameof (_arrow));
  }

  public new class SignalName : NSettingsDropdown.SignalName
  {
  }
}
