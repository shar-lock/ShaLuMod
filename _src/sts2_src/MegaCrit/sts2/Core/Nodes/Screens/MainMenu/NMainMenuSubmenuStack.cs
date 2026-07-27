// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMainMenuSubmenuStack
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuSubmenuStack.cs")]
public class NMainMenuSubmenuStack : NSubmenuStack
{
  [Export]
  private PackedScene _settingsScreenScene;
  [Export]
  private PackedScene _characterSelectScreenScene;
  private NSingleplayerSubmenu? _singleplayerSubmenu;
  private NMultiplayerSubmenu? _multiplayerSubmenu;
  private NMultiplayerHostSubmenu? _multiplayerHostSubmenu;
  private NJoinFriendScreen? _joinFriendSubmenu;
  private NCharacterSelectScreen? _characterSelectSubmenu;
  private NMultiplayerLoadGameScreen? _loadMultiplayerSubmenu;
  private NCompendiumSubmenu? _compendiumSubmenu;
  private NBestiary? _bestiarySubmenu;
  private NRelicCollection? _relicCollectionSubmenu;
  private NPotionLab? _potionLabSubmenu;
  private NCardLibrary? _cardLibrarySubmenu;
  private NRunHistory? _runHistorySubmenu;
  private NStatsScreen? _statsScreen;
  private NTimelineScreen? _timelineScreen;
  private NSettingsScreen? _settingsScreen;
  private NDailyRunScreen? _dailyScreen;
  private NDailyRunLoadScreen? _dailyLoadScreen;
  private NCustomRunScreen? _customRunScreen;
  private NCustomRunLoadScreen? _customRunLoadScreen;
  private NModdingScreen? _moddingScreen;
  private NProfileScreen? _profileScreen;

  public override void _Ready()
  {
    this.GetSubmenuType<NSettingsScreen>();
    this.GetSubmenuType<NCharacterSelectScreen>();
  }

  public override T PushSubmenuType<T>() => (T) this.PushSubmenuType(typeof (T));

  public override T GetSubmenuType<T>() => (T) this.GetSubmenuType(typeof (T));

  public override NSubmenu PushSubmenuType(Type type)
  {
    NSubmenu submenuType = this.GetSubmenuType(type);
    this.Push(submenuType);
    return submenuType;
  }

  public override NSubmenu GetSubmenuType(Type type)
  {
    if (type == typeof (NSingleplayerSubmenu))
    {
      if (this._singleplayerSubmenu == null)
      {
        this._singleplayerSubmenu = NSingleplayerSubmenu.Create();
        ((CanvasItem) this._singleplayerSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._singleplayerSubmenu);
      }
      return (NSubmenu) this._singleplayerSubmenu;
    }
    if (type == typeof (NMultiplayerSubmenu))
    {
      if (this._multiplayerSubmenu == null)
      {
        this._multiplayerSubmenu = NMultiplayerSubmenu.Create();
        ((CanvasItem) this._multiplayerSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._multiplayerSubmenu);
      }
      return (NSubmenu) this._multiplayerSubmenu;
    }
    if (type == typeof (NMultiplayerHostSubmenu))
    {
      if (this._multiplayerHostSubmenu == null)
      {
        this._multiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
        ((CanvasItem) this._multiplayerHostSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._multiplayerHostSubmenu);
      }
      return (NSubmenu) this._multiplayerHostSubmenu;
    }
    if (type == typeof (NJoinFriendScreen))
    {
      if (this._joinFriendSubmenu == null)
      {
        this._joinFriendSubmenu = NJoinFriendScreen.Create();
        ((CanvasItem) this._joinFriendSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._joinFriendSubmenu);
      }
      return (NSubmenu) this._joinFriendSubmenu;
    }
    if (type == typeof (NCharacterSelectScreen))
    {
      if (this._characterSelectSubmenu == null)
      {
        this._characterSelectSubmenu = this._characterSelectScreenScene.Instantiate<NCharacterSelectScreen>((PackedScene.GenEditState) 0L);
        ((CanvasItem) this._characterSelectSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._characterSelectSubmenu);
      }
      return (NSubmenu) this._characterSelectSubmenu;
    }
    if (type == typeof (NMultiplayerLoadGameScreen))
    {
      if (this._loadMultiplayerSubmenu == null)
      {
        this._loadMultiplayerSubmenu = NMultiplayerLoadGameScreen.Create();
        ((CanvasItem) this._loadMultiplayerSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._loadMultiplayerSubmenu);
      }
      return (NSubmenu) this._loadMultiplayerSubmenu;
    }
    if (type == typeof (NCompendiumSubmenu))
    {
      if (this._compendiumSubmenu == null)
      {
        this._compendiumSubmenu = NCompendiumSubmenu.Create();
        ((CanvasItem) this._compendiumSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._compendiumSubmenu);
      }
      return (NSubmenu) this._compendiumSubmenu;
    }
    if (type == typeof (NBestiary))
    {
      if (this._bestiarySubmenu == null)
      {
        this._bestiarySubmenu = NBestiary.Create();
        ((CanvasItem) this._bestiarySubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._bestiarySubmenu);
      }
      return (NSubmenu) this._bestiarySubmenu;
    }
    if (type == typeof (NRelicCollection))
    {
      if (this._relicCollectionSubmenu == null)
      {
        this._relicCollectionSubmenu = NRelicCollection.Create();
        ((CanvasItem) this._relicCollectionSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._relicCollectionSubmenu);
      }
      return (NSubmenu) this._relicCollectionSubmenu;
    }
    if (type == typeof (NPotionLab))
    {
      if (this._potionLabSubmenu == null)
      {
        this._potionLabSubmenu = NPotionLab.Create();
        ((CanvasItem) this._potionLabSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._potionLabSubmenu);
      }
      return (NSubmenu) this._potionLabSubmenu;
    }
    if (type == typeof (NMultiplayerHostSubmenu))
    {
      if (this._multiplayerHostSubmenu == null)
      {
        this._multiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
        ((CanvasItem) this._multiplayerHostSubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._multiplayerHostSubmenu);
      }
      return (NSubmenu) this._multiplayerHostSubmenu;
    }
    if (type == typeof (NCardLibrary))
    {
      if (this._cardLibrarySubmenu == null)
      {
        this._cardLibrarySubmenu = NCardLibrary.Create();
        ((CanvasItem) this._cardLibrarySubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._cardLibrarySubmenu);
      }
      return (NSubmenu) this._cardLibrarySubmenu;
    }
    if (type == typeof (NRunHistory))
    {
      if (this._runHistorySubmenu == null)
      {
        this._runHistorySubmenu = NRunHistory.Create();
        ((CanvasItem) this._runHistorySubmenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._runHistorySubmenu);
      }
      return (NSubmenu) this._runHistorySubmenu;
    }
    if (type == typeof (NStatsScreen))
    {
      if (this._statsScreen == null)
      {
        this._statsScreen = NStatsScreen.Create();
        ((CanvasItem) this._statsScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._statsScreen);
      }
      return (NSubmenu) this._statsScreen;
    }
    if (type == typeof (NTimelineScreen))
    {
      if (this._timelineScreen == null)
      {
        this._timelineScreen = NTimelineScreen.Create();
        ((CanvasItem) this._timelineScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._timelineScreen);
      }
      return (NSubmenu) this._timelineScreen;
    }
    if (type == typeof (NSettingsScreen))
    {
      if (this._settingsScreen == null)
      {
        this._settingsScreen = this._settingsScreenScene.Instantiate<NSettingsScreen>((PackedScene.GenEditState) 0L);
        this._settingsScreen.SetIsInRun(false);
        ((CanvasItem) this._settingsScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._settingsScreen);
      }
      return (NSubmenu) this._settingsScreen;
    }
    if (type == typeof (NDailyRunScreen))
    {
      if (this._dailyScreen == null)
      {
        this._dailyScreen = NDailyRunScreen.Create();
        ((CanvasItem) this._dailyScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._dailyScreen);
      }
      return (NSubmenu) this._dailyScreen;
    }
    if (type == typeof (NDailyRunLoadScreen))
    {
      if (this._dailyLoadScreen == null)
      {
        this._dailyLoadScreen = NDailyRunLoadScreen.Create();
        ((CanvasItem) this._dailyLoadScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._dailyLoadScreen);
      }
      return (NSubmenu) this._dailyLoadScreen;
    }
    if (type == typeof (NCustomRunScreen))
    {
      if (this._customRunScreen == null)
      {
        this._customRunScreen = NCustomRunScreen.Create();
        ((CanvasItem) this._customRunScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._customRunScreen);
      }
      return (NSubmenu) this._customRunScreen;
    }
    if (type == typeof (NCustomRunLoadScreen))
    {
      if (this._customRunLoadScreen == null)
      {
        this._customRunLoadScreen = NCustomRunLoadScreen.Create();
        ((CanvasItem) this._customRunLoadScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._customRunLoadScreen);
      }
      return (NSubmenu) this._customRunLoadScreen;
    }
    if (type == typeof (NModdingScreen))
    {
      if (this._moddingScreen == null)
      {
        this._moddingScreen = NModdingScreen.Create();
        ((CanvasItem) this._moddingScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._moddingScreen);
      }
      return (NSubmenu) this._moddingScreen;
    }
    if (type == typeof (NProfileScreen))
    {
      if (this._profileScreen == null)
      {
        this._profileScreen = NProfileScreen.Create();
        ((CanvasItem) this._profileScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._profileScreen);
      }
      return (NSubmenu) this._profileScreen;
    }
    throw new ArgumentException($"No such submenu {type} in main menu");
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMainMenuSubmenuStack.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NMainMenuSubmenuStack.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMainMenuSubmenuStack.MethodName._Ready) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._settingsScreenScene))
    {
      this._settingsScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene))
    {
      this._characterSelectScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu))
    {
      this._singleplayerSubmenu = VariantUtils.ConvertTo<NSingleplayerSubmenu>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu))
    {
      this._multiplayerSubmenu = VariantUtils.ConvertTo<NMultiplayerSubmenu>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu))
    {
      this._multiplayerHostSubmenu = VariantUtils.ConvertTo<NMultiplayerHostSubmenu>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu))
    {
      this._joinFriendSubmenu = VariantUtils.ConvertTo<NJoinFriendScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu))
    {
      this._characterSelectSubmenu = VariantUtils.ConvertTo<NCharacterSelectScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu))
    {
      this._loadMultiplayerSubmenu = VariantUtils.ConvertTo<NMultiplayerLoadGameScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._compendiumSubmenu))
    {
      this._compendiumSubmenu = VariantUtils.ConvertTo<NCompendiumSubmenu>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._bestiarySubmenu))
    {
      this._bestiarySubmenu = VariantUtils.ConvertTo<NBestiary>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu))
    {
      this._relicCollectionSubmenu = VariantUtils.ConvertTo<NRelicCollection>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._potionLabSubmenu))
    {
      this._potionLabSubmenu = VariantUtils.ConvertTo<NPotionLab>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu))
    {
      this._cardLibrarySubmenu = VariantUtils.ConvertTo<NCardLibrary>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._runHistorySubmenu))
    {
      this._runHistorySubmenu = VariantUtils.ConvertTo<NRunHistory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._statsScreen))
    {
      this._statsScreen = VariantUtils.ConvertTo<NStatsScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._timelineScreen))
    {
      this._timelineScreen = VariantUtils.ConvertTo<NTimelineScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._settingsScreen))
    {
      this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._dailyScreen))
    {
      this._dailyScreen = VariantUtils.ConvertTo<NDailyRunScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._dailyLoadScreen))
    {
      this._dailyLoadScreen = VariantUtils.ConvertTo<NDailyRunLoadScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._customRunScreen))
    {
      this._customRunScreen = VariantUtils.ConvertTo<NCustomRunScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._customRunLoadScreen))
    {
      this._customRunLoadScreen = VariantUtils.ConvertTo<NCustomRunLoadScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._moddingScreen))
    {
      this._moddingScreen = VariantUtils.ConvertTo<NModdingScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._profileScreen))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._profileScreen = VariantUtils.ConvertTo<NProfileScreen>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._settingsScreenScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._settingsScreenScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._characterSelectScreenScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu))
    {
      value = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref this._singleplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu))
    {
      value = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref this._multiplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu))
    {
      value = VariantUtils.CreateFrom<NMultiplayerHostSubmenu>(ref this._multiplayerHostSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu))
    {
      value = VariantUtils.CreateFrom<NJoinFriendScreen>(ref this._joinFriendSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref this._characterSelectSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu))
    {
      value = VariantUtils.CreateFrom<NMultiplayerLoadGameScreen>(ref this._loadMultiplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._compendiumSubmenu))
    {
      value = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref this._compendiumSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._bestiarySubmenu))
    {
      value = VariantUtils.CreateFrom<NBestiary>(ref this._bestiarySubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu))
    {
      value = VariantUtils.CreateFrom<NRelicCollection>(ref this._relicCollectionSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._potionLabSubmenu))
    {
      value = VariantUtils.CreateFrom<NPotionLab>(ref this._potionLabSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu))
    {
      value = VariantUtils.CreateFrom<NCardLibrary>(ref this._cardLibrarySubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._runHistorySubmenu))
    {
      value = VariantUtils.CreateFrom<NRunHistory>(ref this._runHistorySubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._statsScreen))
    {
      value = VariantUtils.CreateFrom<NStatsScreen>(ref this._statsScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._timelineScreen))
    {
      value = VariantUtils.CreateFrom<NTimelineScreen>(ref this._timelineScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._settingsScreen))
    {
      value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._dailyScreen))
    {
      value = VariantUtils.CreateFrom<NDailyRunScreen>(ref this._dailyScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._dailyLoadScreen))
    {
      value = VariantUtils.CreateFrom<NDailyRunLoadScreen>(ref this._dailyLoadScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._customRunScreen))
    {
      value = VariantUtils.CreateFrom<NCustomRunScreen>(ref this._customRunScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._customRunLoadScreen))
    {
      value = VariantUtils.CreateFrom<NCustomRunLoadScreen>(ref this._customRunLoadScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._moddingScreen))
    {
      value = VariantUtils.CreateFrom<NModdingScreen>(ref this._moddingScreen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenuSubmenuStack.PropertyName._profileScreen))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NProfileScreen>(ref this._profileScreen);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._settingsScreenScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._compendiumSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._bestiarySubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._potionLabSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._runHistorySubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._statsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._timelineScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._settingsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._dailyScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._dailyLoadScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._customRunScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._customRunLoadScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._moddingScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuSubmenuStack.PropertyName._profileScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._settingsScreenScene, Variant.From<PackedScene>(ref this._settingsScreenScene));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene, Variant.From<PackedScene>(ref this._characterSelectScreenScene));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu, Variant.From<NSingleplayerSubmenu>(ref this._singleplayerSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu, Variant.From<NMultiplayerSubmenu>(ref this._multiplayerSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu, Variant.From<NMultiplayerHostSubmenu>(ref this._multiplayerHostSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu, Variant.From<NJoinFriendScreen>(ref this._joinFriendSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu, Variant.From<NCharacterSelectScreen>(ref this._characterSelectSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu, Variant.From<NMultiplayerLoadGameScreen>(ref this._loadMultiplayerSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._compendiumSubmenu, Variant.From<NCompendiumSubmenu>(ref this._compendiumSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._bestiarySubmenu, Variant.From<NBestiary>(ref this._bestiarySubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu, Variant.From<NRelicCollection>(ref this._relicCollectionSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._potionLabSubmenu, Variant.From<NPotionLab>(ref this._potionLabSubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu, Variant.From<NCardLibrary>(ref this._cardLibrarySubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._runHistorySubmenu, Variant.From<NRunHistory>(ref this._runHistorySubmenu));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._statsScreen, Variant.From<NStatsScreen>(ref this._statsScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._timelineScreen, Variant.From<NTimelineScreen>(ref this._timelineScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._dailyScreen, Variant.From<NDailyRunScreen>(ref this._dailyScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._dailyLoadScreen, Variant.From<NDailyRunLoadScreen>(ref this._dailyLoadScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._customRunScreen, Variant.From<NCustomRunScreen>(ref this._customRunScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._customRunLoadScreen, Variant.From<NCustomRunLoadScreen>(ref this._customRunLoadScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._moddingScreen, Variant.From<NModdingScreen>(ref this._moddingScreen));
    info.AddProperty(NMainMenuSubmenuStack.PropertyName._profileScreen, Variant.From<NProfileScreen>(ref this._profileScreen));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._settingsScreenScene, ref variant1))
      this._settingsScreenScene = ((Variant) ref variant1).As<PackedScene>();
    Variant variant2;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._characterSelectScreenScene, ref variant2))
      this._characterSelectScreenScene = ((Variant) ref variant2).As<PackedScene>();
    Variant variant3;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._singleplayerSubmenu, ref variant3))
      this._singleplayerSubmenu = ((Variant) ref variant3).As<NSingleplayerSubmenu>();
    Variant variant4;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._multiplayerSubmenu, ref variant4))
      this._multiplayerSubmenu = ((Variant) ref variant4).As<NMultiplayerSubmenu>();
    Variant variant5;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._multiplayerHostSubmenu, ref variant5))
      this._multiplayerHostSubmenu = ((Variant) ref variant5).As<NMultiplayerHostSubmenu>();
    Variant variant6;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._joinFriendSubmenu, ref variant6))
      this._joinFriendSubmenu = ((Variant) ref variant6).As<NJoinFriendScreen>();
    Variant variant7;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._characterSelectSubmenu, ref variant7))
      this._characterSelectSubmenu = ((Variant) ref variant7).As<NCharacterSelectScreen>();
    Variant variant8;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._loadMultiplayerSubmenu, ref variant8))
      this._loadMultiplayerSubmenu = ((Variant) ref variant8).As<NMultiplayerLoadGameScreen>();
    Variant variant9;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._compendiumSubmenu, ref variant9))
      this._compendiumSubmenu = ((Variant) ref variant9).As<NCompendiumSubmenu>();
    Variant variant10;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._bestiarySubmenu, ref variant10))
      this._bestiarySubmenu = ((Variant) ref variant10).As<NBestiary>();
    Variant variant11;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._relicCollectionSubmenu, ref variant11))
      this._relicCollectionSubmenu = ((Variant) ref variant11).As<NRelicCollection>();
    Variant variant12;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._potionLabSubmenu, ref variant12))
      this._potionLabSubmenu = ((Variant) ref variant12).As<NPotionLab>();
    Variant variant13;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._cardLibrarySubmenu, ref variant13))
      this._cardLibrarySubmenu = ((Variant) ref variant13).As<NCardLibrary>();
    Variant variant14;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._runHistorySubmenu, ref variant14))
      this._runHistorySubmenu = ((Variant) ref variant14).As<NRunHistory>();
    Variant variant15;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._statsScreen, ref variant15))
      this._statsScreen = ((Variant) ref variant15).As<NStatsScreen>();
    Variant variant16;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._timelineScreen, ref variant16))
      this._timelineScreen = ((Variant) ref variant16).As<NTimelineScreen>();
    Variant variant17;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._settingsScreen, ref variant17))
      this._settingsScreen = ((Variant) ref variant17).As<NSettingsScreen>();
    Variant variant18;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._dailyScreen, ref variant18))
      this._dailyScreen = ((Variant) ref variant18).As<NDailyRunScreen>();
    Variant variant19;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._dailyLoadScreen, ref variant19))
      this._dailyLoadScreen = ((Variant) ref variant19).As<NDailyRunLoadScreen>();
    Variant variant20;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._customRunScreen, ref variant20))
      this._customRunScreen = ((Variant) ref variant20).As<NCustomRunScreen>();
    Variant variant21;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._customRunLoadScreen, ref variant21))
      this._customRunLoadScreen = ((Variant) ref variant21).As<NCustomRunLoadScreen>();
    Variant variant22;
    if (info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._moddingScreen, ref variant22))
      this._moddingScreen = ((Variant) ref variant22).As<NModdingScreen>();
    Variant variant23;
    if (!info.TryGetProperty(NMainMenuSubmenuStack.PropertyName._profileScreen, ref variant23))
      return;
    this._profileScreen = ((Variant) ref variant23).As<NProfileScreen>();
  }

  public new class MethodName : NSubmenuStack.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public new class PropertyName : NSubmenuStack.PropertyName
  {
    public static readonly StringName _settingsScreenScene = StringName.op_Implicit(nameof (_settingsScreenScene));
    public static readonly StringName _characterSelectScreenScene = StringName.op_Implicit(nameof (_characterSelectScreenScene));
    public static readonly StringName _singleplayerSubmenu = StringName.op_Implicit(nameof (_singleplayerSubmenu));
    public static readonly StringName _multiplayerSubmenu = StringName.op_Implicit(nameof (_multiplayerSubmenu));
    public static readonly StringName _multiplayerHostSubmenu = StringName.op_Implicit(nameof (_multiplayerHostSubmenu));
    public static readonly StringName _joinFriendSubmenu = StringName.op_Implicit(nameof (_joinFriendSubmenu));
    public static readonly StringName _characterSelectSubmenu = StringName.op_Implicit(nameof (_characterSelectSubmenu));
    public static readonly StringName _loadMultiplayerSubmenu = StringName.op_Implicit(nameof (_loadMultiplayerSubmenu));
    public static readonly StringName _compendiumSubmenu = StringName.op_Implicit(nameof (_compendiumSubmenu));
    public static readonly StringName _bestiarySubmenu = StringName.op_Implicit(nameof (_bestiarySubmenu));
    public static readonly StringName _relicCollectionSubmenu = StringName.op_Implicit(nameof (_relicCollectionSubmenu));
    public static readonly StringName _potionLabSubmenu = StringName.op_Implicit(nameof (_potionLabSubmenu));
    public static readonly StringName _cardLibrarySubmenu = StringName.op_Implicit(nameof (_cardLibrarySubmenu));
    public static readonly StringName _runHistorySubmenu = StringName.op_Implicit(nameof (_runHistorySubmenu));
    public static readonly StringName _statsScreen = StringName.op_Implicit(nameof (_statsScreen));
    public static readonly StringName _timelineScreen = StringName.op_Implicit(nameof (_timelineScreen));
    public static readonly StringName _settingsScreen = StringName.op_Implicit(nameof (_settingsScreen));
    public static readonly StringName _dailyScreen = StringName.op_Implicit(nameof (_dailyScreen));
    public static readonly StringName _dailyLoadScreen = StringName.op_Implicit(nameof (_dailyLoadScreen));
    public static readonly StringName _customRunScreen = StringName.op_Implicit(nameof (_customRunScreen));
    public static readonly StringName _customRunLoadScreen = StringName.op_Implicit(nameof (_customRunLoadScreen));
    public static readonly StringName _moddingScreen = StringName.op_Implicit(nameof (_moddingScreen));
    public static readonly StringName _profileScreen = StringName.op_Implicit(nameof (_profileScreen));
  }

  public new class SignalName : NSubmenuStack.SignalName
  {
  }
}
