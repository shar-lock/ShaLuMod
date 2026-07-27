// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NRunSubmenuStack
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NRunSubmenuStack.cs")]
public class NRunSubmenuStack : NSubmenuStack
{
  [Export]
  private PackedScene _settingsScreenScene;
  [Export]
  private PackedScene _pauseMenuScene;
  [Export]
  private PackedScene _statsScreenScene;
  [Export]
  private PackedScene _runHistoryScreenScene;
  private NCompendiumSubmenu? _compendiumSubmenu;
  private NBestiary? _bestiarySubmenu;
  private NRelicCollection? _relicCollectionSubmenu;
  private NPotionLab? _potionLabSubmenu;
  private NCardLibrary? _cardLibrarySubmenu;
  private NRunHistory? _runHistoryScreen;
  private NSettingsScreen? _settingsScreen;
  private NStatsScreen? _statsScreen;
  private NPauseMenu? _pauseMenu;

  public override void _Ready() => this.GetSubmenuType<NSettingsScreen>();

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
      if (this._runHistoryScreen == null)
      {
        this._runHistoryScreen = this._runHistoryScreenScene.Instantiate<NRunHistory>((PackedScene.GenEditState) 0L);
        ((CanvasItem) this._runHistoryScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._runHistoryScreen);
      }
      return (NSubmenu) this._runHistoryScreen;
    }
    if (type == typeof (NSettingsScreen))
    {
      if (this._settingsScreen == null)
      {
        this._settingsScreen = this._settingsScreenScene.Instantiate<NSettingsScreen>((PackedScene.GenEditState) 0L);
        this._settingsScreen.SetIsInRun(true);
        ((CanvasItem) this._settingsScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._settingsScreen);
      }
      return (NSubmenu) this._settingsScreen;
    }
    if (type == typeof (NStatsScreen))
    {
      if (this._statsScreen == null)
      {
        this._statsScreen = this._statsScreenScene.Instantiate<NStatsScreen>((PackedScene.GenEditState) 0L);
        ((CanvasItem) this._statsScreen).Visible = false;
        ((Node) this).AddChildSafely((Node) this._statsScreen);
      }
      return (NSubmenu) this._statsScreen;
    }
    if (type == typeof (NPauseMenu))
    {
      if (this._pauseMenu == null)
      {
        this._pauseMenu = this._pauseMenuScene.Instantiate<NPauseMenu>((PackedScene.GenEditState) 0L);
        ((CanvasItem) this._pauseMenu).Visible = false;
        ((Node) this).AddChildSafely((Node) this._pauseMenu);
      }
      return (NSubmenu) this._pauseMenu;
    }
    throw new ArgumentException($"No such submenu of type {type} in run");
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NRunSubmenuStack.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NRunSubmenuStack.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunSubmenuStack.MethodName._Ready) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._settingsScreenScene))
    {
      this._settingsScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._pauseMenuScene))
    {
      this._pauseMenuScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._statsScreenScene))
    {
      this._statsScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._runHistoryScreenScene))
    {
      this._runHistoryScreenScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._compendiumSubmenu))
    {
      this._compendiumSubmenu = VariantUtils.ConvertTo<NCompendiumSubmenu>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._bestiarySubmenu))
    {
      this._bestiarySubmenu = VariantUtils.ConvertTo<NBestiary>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._relicCollectionSubmenu))
    {
      this._relicCollectionSubmenu = VariantUtils.ConvertTo<NRelicCollection>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._potionLabSubmenu))
    {
      this._potionLabSubmenu = VariantUtils.ConvertTo<NPotionLab>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._cardLibrarySubmenu))
    {
      this._cardLibrarySubmenu = VariantUtils.ConvertTo<NCardLibrary>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._runHistoryScreen))
    {
      this._runHistoryScreen = VariantUtils.ConvertTo<NRunHistory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._settingsScreen))
    {
      this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._statsScreen))
    {
      this._statsScreen = VariantUtils.ConvertTo<NStatsScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._pauseMenu))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._pauseMenu = VariantUtils.ConvertTo<NPauseMenu>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._settingsScreenScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._settingsScreenScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._pauseMenuScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._pauseMenuScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._statsScreenScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._statsScreenScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._runHistoryScreenScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._runHistoryScreenScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._compendiumSubmenu))
    {
      value = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref this._compendiumSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._bestiarySubmenu))
    {
      value = VariantUtils.CreateFrom<NBestiary>(ref this._bestiarySubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._relicCollectionSubmenu))
    {
      value = VariantUtils.CreateFrom<NRelicCollection>(ref this._relicCollectionSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._potionLabSubmenu))
    {
      value = VariantUtils.CreateFrom<NPotionLab>(ref this._potionLabSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._cardLibrarySubmenu))
    {
      value = VariantUtils.CreateFrom<NCardLibrary>(ref this._cardLibrarySubmenu);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._runHistoryScreen))
    {
      value = VariantUtils.CreateFrom<NRunHistory>(ref this._runHistoryScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._settingsScreen))
    {
      value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._statsScreen))
    {
      value = VariantUtils.CreateFrom<NStatsScreen>(ref this._statsScreen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunSubmenuStack.PropertyName._pauseMenu))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NPauseMenu>(ref this._pauseMenu);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._settingsScreenScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._pauseMenuScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._statsScreenScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._runHistoryScreenScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._compendiumSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._bestiarySubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._relicCollectionSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._potionLabSubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._cardLibrarySubmenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._runHistoryScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._settingsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._statsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunSubmenuStack.PropertyName._pauseMenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRunSubmenuStack.PropertyName._settingsScreenScene, Variant.From<PackedScene>(ref this._settingsScreenScene));
    info.AddProperty(NRunSubmenuStack.PropertyName._pauseMenuScene, Variant.From<PackedScene>(ref this._pauseMenuScene));
    info.AddProperty(NRunSubmenuStack.PropertyName._statsScreenScene, Variant.From<PackedScene>(ref this._statsScreenScene));
    info.AddProperty(NRunSubmenuStack.PropertyName._runHistoryScreenScene, Variant.From<PackedScene>(ref this._runHistoryScreenScene));
    info.AddProperty(NRunSubmenuStack.PropertyName._compendiumSubmenu, Variant.From<NCompendiumSubmenu>(ref this._compendiumSubmenu));
    info.AddProperty(NRunSubmenuStack.PropertyName._bestiarySubmenu, Variant.From<NBestiary>(ref this._bestiarySubmenu));
    info.AddProperty(NRunSubmenuStack.PropertyName._relicCollectionSubmenu, Variant.From<NRelicCollection>(ref this._relicCollectionSubmenu));
    info.AddProperty(NRunSubmenuStack.PropertyName._potionLabSubmenu, Variant.From<NPotionLab>(ref this._potionLabSubmenu));
    info.AddProperty(NRunSubmenuStack.PropertyName._cardLibrarySubmenu, Variant.From<NCardLibrary>(ref this._cardLibrarySubmenu));
    info.AddProperty(NRunSubmenuStack.PropertyName._runHistoryScreen, Variant.From<NRunHistory>(ref this._runHistoryScreen));
    info.AddProperty(NRunSubmenuStack.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
    info.AddProperty(NRunSubmenuStack.PropertyName._statsScreen, Variant.From<NStatsScreen>(ref this._statsScreen));
    info.AddProperty(NRunSubmenuStack.PropertyName._pauseMenu, Variant.From<NPauseMenu>(ref this._pauseMenu));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._settingsScreenScene, ref variant1))
      this._settingsScreenScene = ((Variant) ref variant1).As<PackedScene>();
    Variant variant2;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._pauseMenuScene, ref variant2))
      this._pauseMenuScene = ((Variant) ref variant2).As<PackedScene>();
    Variant variant3;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._statsScreenScene, ref variant3))
      this._statsScreenScene = ((Variant) ref variant3).As<PackedScene>();
    Variant variant4;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._runHistoryScreenScene, ref variant4))
      this._runHistoryScreenScene = ((Variant) ref variant4).As<PackedScene>();
    Variant variant5;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._compendiumSubmenu, ref variant5))
      this._compendiumSubmenu = ((Variant) ref variant5).As<NCompendiumSubmenu>();
    Variant variant6;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._bestiarySubmenu, ref variant6))
      this._bestiarySubmenu = ((Variant) ref variant6).As<NBestiary>();
    Variant variant7;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._relicCollectionSubmenu, ref variant7))
      this._relicCollectionSubmenu = ((Variant) ref variant7).As<NRelicCollection>();
    Variant variant8;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._potionLabSubmenu, ref variant8))
      this._potionLabSubmenu = ((Variant) ref variant8).As<NPotionLab>();
    Variant variant9;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._cardLibrarySubmenu, ref variant9))
      this._cardLibrarySubmenu = ((Variant) ref variant9).As<NCardLibrary>();
    Variant variant10;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._runHistoryScreen, ref variant10))
      this._runHistoryScreen = ((Variant) ref variant10).As<NRunHistory>();
    Variant variant11;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._settingsScreen, ref variant11))
      this._settingsScreen = ((Variant) ref variant11).As<NSettingsScreen>();
    Variant variant12;
    if (info.TryGetProperty(NRunSubmenuStack.PropertyName._statsScreen, ref variant12))
      this._statsScreen = ((Variant) ref variant12).As<NStatsScreen>();
    Variant variant13;
    if (!info.TryGetProperty(NRunSubmenuStack.PropertyName._pauseMenu, ref variant13))
      return;
    this._pauseMenu = ((Variant) ref variant13).As<NPauseMenu>();
  }

  public new class MethodName : NSubmenuStack.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public new class PropertyName : NSubmenuStack.PropertyName
  {
    public static readonly StringName _settingsScreenScene = StringName.op_Implicit(nameof (_settingsScreenScene));
    public static readonly StringName _pauseMenuScene = StringName.op_Implicit(nameof (_pauseMenuScene));
    public static readonly StringName _statsScreenScene = StringName.op_Implicit(nameof (_statsScreenScene));
    public static readonly StringName _runHistoryScreenScene = StringName.op_Implicit(nameof (_runHistoryScreenScene));
    public static readonly StringName _compendiumSubmenu = StringName.op_Implicit(nameof (_compendiumSubmenu));
    public static readonly StringName _bestiarySubmenu = StringName.op_Implicit(nameof (_bestiarySubmenu));
    public static readonly StringName _relicCollectionSubmenu = StringName.op_Implicit(nameof (_relicCollectionSubmenu));
    public static readonly StringName _potionLabSubmenu = StringName.op_Implicit(nameof (_potionLabSubmenu));
    public static readonly StringName _cardLibrarySubmenu = StringName.op_Implicit(nameof (_cardLibrarySubmenu));
    public static readonly StringName _runHistoryScreen = StringName.op_Implicit(nameof (_runHistoryScreen));
    public static readonly StringName _settingsScreen = StringName.op_Implicit(nameof (_settingsScreen));
    public static readonly StringName _statsScreen = StringName.op_Implicit(nameof (_statsScreen));
    public static readonly StringName _pauseMenu = StringName.op_Implicit(nameof (_pauseMenu));
  }

  public new class SignalName : NSubmenuStack.SignalName
  {
  }
}
