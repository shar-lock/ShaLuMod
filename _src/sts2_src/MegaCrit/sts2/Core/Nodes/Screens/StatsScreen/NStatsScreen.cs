// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NStatsScreen
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
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NStatsScreen.cs")]
public class NStatsScreen : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/stats_screen/stats_screen");
  private NStatsTabManager _statsTabManager;
  private NSettingsTab _statsTab;
  private NSettingsTab _achievementsTab;
  private NGeneralStatsGrid _statsGrid;
  private Tween? _screenTween;

  public static string[] AssetPaths
  {
    get
    {
      string scenePath = NStatsScreen._scenePath;
      string[] assetPaths1 = NGeneralStatsGrid.AssetPaths;
      int index = 0;
      string[] assetPaths2 = new string[1 + assetPaths1.Length];
      assetPaths2[index] = scenePath;
      int num1 = index + 1;
      ReadOnlySpan<string> readOnlySpan = new ReadOnlySpan<string>(assetPaths1);
      readOnlySpan.CopyTo(new Span<string>(assetPaths2).Slice(num1, readOnlySpan.Length));
      int num2 = num1 + readOnlySpan.Length;
      return assetPaths2;
    }
  }

  public static NStatsScreen? Create()
  {
    return TestMode.IsOn ? (NStatsScreen) null : PreloadManager.Cache.GetScene(NStatsScreen._scenePath).Instantiate<NStatsScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._statsTab = ((Node) this).GetNode<NSettingsTab>(NodePath.op_Implicit("%StatsTab"));
    this._statsTab.SetLabel(new LocString("stats_screen", "TAB_STATS.header").GetFormattedText());
    this._achievementsTab = ((Node) this).GetNode<NSettingsTab>(NodePath.op_Implicit("%Achievements"));
    this._achievementsTab.SetLabel(new LocString("stats_screen", "TAB_ACHIEVEMENT.header").GetFormattedText());
    ((GodotObject) this._statsTab).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.OpenStatsMenu())), 0U);
    this._statsTabManager = ((Node) this).GetNode<NStatsTabManager>(NodePath.op_Implicit("%Tabs"));
    this._statsGrid = ((Node) this).GetNode<NGeneralStatsGrid>(NodePath.op_Implicit("%StatsGrid"));
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%OverallStatsHeader")).SetTextAutoSize(new LocString("main_menu_ui", "STATISTICS.OVERALL.title").GetFormattedText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CharacterStatsHeader")).SetTextAutoSize(new LocString("main_menu_ui", "STATISTICS.title").GetFormattedText());
    this._achievementsTab.Disable();
  }

  public override void OnSubmenuOpened()
  {
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween();
    this._screenTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    ((CanvasItem) this).Visible = true;
    this.OpenStatsMenu();
    this._statsTabManager.ResetTabs();
  }

  protected override Control InitialFocusedControl => this._statsGrid.DefaultFocusedControl;

  private void OpenStatsMenu()
  {
    ((CanvasItem) this._statsGrid).Visible = true;
    this._statsGrid.LoadStats();
    ActiveScreenContext.Instance.Update();
  }

  private void OpenAchievementsMenu()
  {
    ((CanvasItem) this._statsGrid).Visible = false;
    ActiveScreenContext.Instance.Update();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NStatsScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatsScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatsScreen.MethodName.OpenStatsMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatsScreen.MethodName.OpenAchievementsMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStatsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NStatsScreen nstatsScreen = NStatsScreen.Create();
      ret = VariantUtils.CreateFrom<NStatsScreen>(ref nstatsScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NStatsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatsScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatsScreen.MethodName.OpenStatsMenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenStatsMenu();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NStatsScreen.MethodName.OpenAchievementsMenu) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OpenAchievementsMenu();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStatsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NStatsScreen nstatsScreen = NStatsScreen.Create();
      ret = VariantUtils.CreateFrom<NStatsScreen>(ref nstatsScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStatsScreen.MethodName.Create) || StringName.op_Equality(ref method, NStatsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NStatsScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NStatsScreen.MethodName.OpenStatsMenu) || StringName.op_Equality(ref method, NStatsScreen.MethodName.OpenAchievementsMenu) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._statsTabManager))
    {
      this._statsTabManager = VariantUtils.ConvertTo<NStatsTabManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._statsTab))
    {
      this._statsTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._achievementsTab))
    {
      this._achievementsTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._statsGrid))
    {
      this._statsGrid = VariantUtils.ConvertTo<NGeneralStatsGrid>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStatsScreen.PropertyName._screenTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._statsTabManager))
    {
      value = VariantUtils.CreateFrom<NStatsTabManager>(ref this._statsTabManager);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._statsTab))
    {
      value = VariantUtils.CreateFrom<NSettingsTab>(ref this._statsTab);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._achievementsTab))
    {
      value = VariantUtils.CreateFrom<NSettingsTab>(ref this._achievementsTab);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsScreen.PropertyName._statsGrid))
    {
      value = VariantUtils.CreateFrom<NGeneralStatsGrid>(ref this._statsGrid);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStatsScreen.PropertyName._screenTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NStatsScreen.PropertyName._statsTabManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsScreen.PropertyName._statsTab, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsScreen.PropertyName._achievementsTab, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsScreen.PropertyName._statsGrid, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsScreen.PropertyName._screenTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NStatsScreen.PropertyName._statsTabManager, Variant.From<NStatsTabManager>(ref this._statsTabManager));
    info.AddProperty(NStatsScreen.PropertyName._statsTab, Variant.From<NSettingsTab>(ref this._statsTab));
    info.AddProperty(NStatsScreen.PropertyName._achievementsTab, Variant.From<NSettingsTab>(ref this._achievementsTab));
    info.AddProperty(NStatsScreen.PropertyName._statsGrid, Variant.From<NGeneralStatsGrid>(ref this._statsGrid));
    info.AddProperty(NStatsScreen.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NStatsScreen.PropertyName._statsTabManager, ref variant1))
      this._statsTabManager = ((Variant) ref variant1).As<NStatsTabManager>();
    Variant variant2;
    if (info.TryGetProperty(NStatsScreen.PropertyName._statsTab, ref variant2))
      this._statsTab = ((Variant) ref variant2).As<NSettingsTab>();
    Variant variant3;
    if (info.TryGetProperty(NStatsScreen.PropertyName._achievementsTab, ref variant3))
      this._achievementsTab = ((Variant) ref variant3).As<NSettingsTab>();
    Variant variant4;
    if (info.TryGetProperty(NStatsScreen.PropertyName._statsGrid, ref variant4))
      this._statsGrid = ((Variant) ref variant4).As<NGeneralStatsGrid>();
    Variant variant5;
    if (!info.TryGetProperty(NStatsScreen.PropertyName._screenTween, ref variant5))
      return;
    this._screenTween = ((Variant) ref variant5).As<Tween>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public static readonly StringName OpenStatsMenu = StringName.op_Implicit(nameof (OpenStatsMenu));
    public static readonly StringName OpenAchievementsMenu = StringName.op_Implicit(nameof (OpenAchievementsMenu));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _statsTabManager = StringName.op_Implicit(nameof (_statsTabManager));
    public static readonly StringName _statsTab = StringName.op_Implicit(nameof (_statsTab));
    public static readonly StringName _achievementsTab = StringName.op_Implicit(nameof (_achievementsTab));
    public static readonly StringName _statsGrid = StringName.op_Implicit(nameof (_statsGrid));
    public static readonly StringName _screenTween = StringName.op_Implicit(nameof (_screenTween));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
