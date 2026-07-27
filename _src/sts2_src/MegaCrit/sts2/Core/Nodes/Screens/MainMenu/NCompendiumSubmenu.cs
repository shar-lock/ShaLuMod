// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NCompendiumSubmenu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NCompendiumSubmenu.cs")]
public class NCompendiumSubmenu : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/compendium_submenu");
  private NButton _confirmButton;
  private NShortSubmenuButton _cardLibraryButton;
  private NShortSubmenuButton _relicCollectionButton;
  private NShortSubmenuButton _potionLabButton;
  private NShortSubmenuButton _bestiaryButton;
  private NCompendiumBottomButton _leaderboardsButton;
  private NCompendiumBottomButton _statisticsButton;
  private NCompendiumBottomButton _runHistoryButton;
  private IRunState _runState;

  protected override Control InitialFocusedControl => (Control) this._cardLibraryButton;

  public static NCompendiumSubmenu? Create()
  {
    return TestMode.IsOn ? (NCompendiumSubmenu) null : PreloadManager.Cache.GetScene(NCompendiumSubmenu._scenePath).Instantiate<NCompendiumSubmenu>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._cardLibraryButton = ((Node) this).GetNode<NShortSubmenuButton>(NodePath.op_Implicit("%CardLibraryButton"));
    ((GodotObject) this._cardLibraryButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCardLibrary)), 0U);
    this._cardLibraryButton.SetIconAndLocalization("COMPENDIUM_CARD_LIBRARY");
    this._relicCollectionButton = ((Node) this).GetNode<NShortSubmenuButton>(NodePath.op_Implicit("%RelicCollectionButton"));
    ((GodotObject) this._relicCollectionButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenRelicCollection)), 0U);
    this._relicCollectionButton.SetIconAndLocalization("COMPENDIUM_RELIC_COLLECTION");
    this._potionLabButton = ((Node) this).GetNode<NShortSubmenuButton>(NodePath.op_Implicit("%PotionLabButton"));
    ((GodotObject) this._potionLabButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenPotionLab)), 0U);
    this._potionLabButton.SetIconAndLocalization("COMPENDIUM_POTION_LAB");
    this._bestiaryButton = ((Node) this).GetNode<NShortSubmenuButton>(NodePath.op_Implicit("%BestiaryButton"));
    ((GodotObject) this._bestiaryButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenBestiary)), 0U);
    this._bestiaryButton.SetIconAndLocalization("COMPENDIUM_BESTIARY");
    this._leaderboardsButton = ((Node) this).GetNode<NCompendiumBottomButton>(NodePath.op_Implicit("%LeaderboardsButton"));
    ((GodotObject) this._leaderboardsButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenLeaderboards)), 0U);
    this._leaderboardsButton.SetLocalization("LEADERBOARDS");
    this._statisticsButton = ((Node) this).GetNode<NCompendiumBottomButton>(NodePath.op_Implicit("%StatisticsButton"));
    ((GodotObject) this._statisticsButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenStatistics)), 0U);
    this._statisticsButton.SetLocalization("STATISTICS");
    this._runHistoryButton = ((Node) this).GetNode<NCompendiumBottomButton>(NodePath.op_Implicit("%RunHistoryButton"));
    ((GodotObject) this._runHistoryButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenRunHistory)), 0U);
    this._runHistoryButton.SetLocalization("RUN_HISTORY");
    int capacity1 = 4;
    List<Control> controlList1 = new List<Control>(capacity1);
    CollectionsMarshal.SetCount<Control>(controlList1, capacity1);
    Span<Control> span1 = CollectionsMarshal.AsSpan<Control>(controlList1);
    int num1 = 0;
    span1[num1] = (Control) this._cardLibraryButton;
    int num2 = num1 + 1;
    span1[num2] = (Control) this._relicCollectionButton;
    int num3 = num2 + 1;
    span1[num3] = (Control) this._potionLabButton;
    int num4 = num3 + 1;
    span1[num4] = (Control) this._bestiaryButton;
    List<Control> controlList2 = controlList1;
    int capacity2 = 3;
    List<Control> controlList3 = new List<Control>(capacity2);
    CollectionsMarshal.SetCount<Control>(controlList3, capacity2);
    Span<Control> span2 = CollectionsMarshal.AsSpan<Control>(controlList3);
    int num5 = 0;
    span2[num5] = (Control) this._leaderboardsButton;
    int num6 = num5 + 1;
    span2[num6] = (Control) this._statisticsButton;
    int num7 = num6 + 1;
    span2[num7] = (Control) this._runHistoryButton;
    List<Control> controlList4 = controlList3;
    for (int index = 0; index < controlList2.Count; ++index)
    {
      controlList2[index].FocusNeighborTop = ((Node) controlList2[index]).GetPath();
      controlList2[index].FocusNeighborLeft = index > 0 ? ((Node) controlList2[index - 1]).GetPath() : ((Node) controlList2[index]).GetPath();
      controlList2[index].FocusNeighborRight = index < controlList2.Count - 1 ? ((Node) controlList2[index + 1]).GetPath() : ((Node) controlList2[index]).GetPath();
    }
    for (int index = 0; index < controlList4.Count; ++index)
    {
      controlList4[index].FocusNeighborBottom = ((Node) controlList4[index]).GetPath();
      controlList4[index].FocusNeighborLeft = index > 0 ? ((Node) controlList4[index - 1]).GetPath() : ((Node) controlList4[index]).GetPath();
      controlList4[index].FocusNeighborRight = index < controlList4.Count - 1 ? ((Node) controlList4[index + 1]).GetPath() : ((Node) controlList4[index]).GetPath();
    }
    controlList2[0].FocusNeighborBottom = ((Node) controlList4[0]).GetPath();
    controlList2[1].FocusNeighborBottom = ((Node) controlList4[0]).GetPath();
    controlList2[2].FocusNeighborBottom = ((Node) controlList4[1]).GetPath();
    controlList2[3].FocusNeighborBottom = ((Node) controlList4[2]).GetPath();
    controlList4[0].FocusNeighborTop = ((Node) controlList2[1]).GetPath();
    controlList4[1].FocusNeighborTop = ((Node) controlList2[2]).GetPath();
    controlList4[2].FocusNeighborTop = ((Node) controlList2[3]).GetPath();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    ((CanvasItem) this._leaderboardsButton).Visible = false;
    ((CanvasItem) this._runHistoryButton).Visible = NRunHistory.CanBeShown();
    ((CanvasItem) this._bestiaryButton).Visible = NBestiary.CanBeShown();
  }

  public void Initialize(IRunState runState) => this._runState = runState;

  private void OpenCardLibrary(NButton _)
  {
    NCardLibrary submenuType = this._stack.GetSubmenuType<NCardLibrary>();
    submenuType.Initialize(this._runState);
    this._stack.Push((NSubmenu) submenuType);
  }

  private void OpenRelicCollection(NButton _) => this._stack.PushSubmenuType<NRelicCollection>();

  private void OpenPotionLab(NButton _) => this._stack.PushSubmenuType<NPotionLab>();

  private void OpenBestiary(NButton _) => this.OpenBestiary();

  public NBestiary OpenBestiary() => this._stack.PushSubmenuType<NBestiary>();

  private void OpenLeaderboards(NButton _)
  {
  }

  private void OpenStatistics(NButton _) => this._stack.PushSubmenuType<NStatsScreen>();

  private void OpenRunHistory(NButton _) => this._stack.PushSubmenuType<NRunHistory>();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NCompendiumSubmenu.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenCardLibrary, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenRelicCollection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenPotionLab, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenBestiary, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenBestiary, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenLeaderboards, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenStatistics, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCompendiumSubmenu.MethodName.OpenRunHistory, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCompendiumSubmenu ncompendiumSubmenu = NCompendiumSubmenu.Create();
      ret = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref ncompendiumSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenCardLibrary) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenCardLibrary(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenRelicCollection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenRelicCollection(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenPotionLab) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenPotionLab(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenBestiary) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenBestiary(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenBestiary) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiary nbestiary = this.OpenBestiary();
      ret = VariantUtils.CreateFrom<NBestiary>(ref nbestiary);
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenLeaderboards) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenLeaderboards(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenStatistics) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenStatistics(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenRunHistory) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OpenRunHistory(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCompendiumSubmenu ncompendiumSubmenu = NCompendiumSubmenu.Create();
      ret = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref ncompendiumSubmenu);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.Create) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName._Ready) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenCardLibrary) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenRelicCollection) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenPotionLab) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenBestiary) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenLeaderboards) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenStatistics) || StringName.op_Equality(ref method, NCompendiumSubmenu.MethodName.OpenRunHistory) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._cardLibraryButton))
    {
      this._cardLibraryButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._relicCollectionButton))
    {
      this._relicCollectionButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._potionLabButton))
    {
      this._potionLabButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._bestiaryButton))
    {
      this._bestiaryButton = VariantUtils.ConvertTo<NShortSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._leaderboardsButton))
    {
      this._leaderboardsButton = VariantUtils.ConvertTo<NCompendiumBottomButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._statisticsButton))
    {
      this._statisticsButton = VariantUtils.ConvertTo<NCompendiumBottomButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._runHistoryButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._runHistoryButton = VariantUtils.ConvertTo<NCompendiumBottomButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._cardLibraryButton))
    {
      value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._cardLibraryButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._relicCollectionButton))
    {
      value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._relicCollectionButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._potionLabButton))
    {
      value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._potionLabButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._bestiaryButton))
    {
      value = VariantUtils.CreateFrom<NShortSubmenuButton>(ref this._bestiaryButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._leaderboardsButton))
    {
      value = VariantUtils.CreateFrom<NCompendiumBottomButton>(ref this._leaderboardsButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._statisticsButton))
    {
      value = VariantUtils.CreateFrom<NCompendiumBottomButton>(ref this._statisticsButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCompendiumSubmenu.PropertyName._runHistoryButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NCompendiumBottomButton>(ref this._runHistoryButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._cardLibraryButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._relicCollectionButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._potionLabButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._bestiaryButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._leaderboardsButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._statisticsButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName._runHistoryButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCompendiumSubmenu.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCompendiumSubmenu.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._cardLibraryButton, Variant.From<NShortSubmenuButton>(ref this._cardLibraryButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._relicCollectionButton, Variant.From<NShortSubmenuButton>(ref this._relicCollectionButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._potionLabButton, Variant.From<NShortSubmenuButton>(ref this._potionLabButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._bestiaryButton, Variant.From<NShortSubmenuButton>(ref this._bestiaryButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._leaderboardsButton, Variant.From<NCompendiumBottomButton>(ref this._leaderboardsButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._statisticsButton, Variant.From<NCompendiumBottomButton>(ref this._statisticsButton));
    info.AddProperty(NCompendiumSubmenu.PropertyName._runHistoryButton, Variant.From<NCompendiumBottomButton>(ref this._runHistoryButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._confirmButton, ref variant1))
      this._confirmButton = ((Variant) ref variant1).As<NButton>();
    Variant variant2;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._cardLibraryButton, ref variant2))
      this._cardLibraryButton = ((Variant) ref variant2).As<NShortSubmenuButton>();
    Variant variant3;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._relicCollectionButton, ref variant3))
      this._relicCollectionButton = ((Variant) ref variant3).As<NShortSubmenuButton>();
    Variant variant4;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._potionLabButton, ref variant4))
      this._potionLabButton = ((Variant) ref variant4).As<NShortSubmenuButton>();
    Variant variant5;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._bestiaryButton, ref variant5))
      this._bestiaryButton = ((Variant) ref variant5).As<NShortSubmenuButton>();
    Variant variant6;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._leaderboardsButton, ref variant6))
      this._leaderboardsButton = ((Variant) ref variant6).As<NCompendiumBottomButton>();
    Variant variant7;
    if (info.TryGetProperty(NCompendiumSubmenu.PropertyName._statisticsButton, ref variant7))
      this._statisticsButton = ((Variant) ref variant7).As<NCompendiumBottomButton>();
    Variant variant8;
    if (!info.TryGetProperty(NCompendiumSubmenu.PropertyName._runHistoryButton, ref variant8))
      return;
    this._runHistoryButton = ((Variant) ref variant8).As<NCompendiumBottomButton>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public static readonly StringName OpenCardLibrary = StringName.op_Implicit(nameof (OpenCardLibrary));
    public static readonly StringName OpenRelicCollection = StringName.op_Implicit(nameof (OpenRelicCollection));
    public static readonly StringName OpenPotionLab = StringName.op_Implicit(nameof (OpenPotionLab));
    public static readonly StringName OpenBestiary = StringName.op_Implicit(nameof (OpenBestiary));
    public static readonly StringName OpenLeaderboards = StringName.op_Implicit(nameof (OpenLeaderboards));
    public static readonly StringName OpenStatistics = StringName.op_Implicit(nameof (OpenStatistics));
    public static readonly StringName OpenRunHistory = StringName.op_Implicit(nameof (OpenRunHistory));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _cardLibraryButton = StringName.op_Implicit(nameof (_cardLibraryButton));
    public static readonly StringName _relicCollectionButton = StringName.op_Implicit(nameof (_relicCollectionButton));
    public static readonly StringName _potionLabButton = StringName.op_Implicit(nameof (_potionLabButton));
    public static readonly StringName _bestiaryButton = StringName.op_Implicit(nameof (_bestiaryButton));
    public static readonly StringName _leaderboardsButton = StringName.op_Implicit(nameof (_leaderboardsButton));
    public static readonly StringName _statisticsButton = StringName.op_Implicit(nameof (_statisticsButton));
    public static readonly StringName _runHistoryButton = StringName.op_Implicit(nameof (_runHistoryButton));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
