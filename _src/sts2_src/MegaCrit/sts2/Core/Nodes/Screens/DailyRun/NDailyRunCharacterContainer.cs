// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunCharacterContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunCharacterContainer.cs")]
public class NDailyRunCharacterContainer : Control
{
  private static readonly LocString _ascensionLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.ASCENSION");
  private Control _characterIconContainer;
  private MegaLabel _playerNameLabel;
  private MegaLabel _characterNameLabel;
  private MegaLabel _ascensionLabel;
  private MegaLabel _ascensionNumberLabel;
  private Control _readyIndicator;

  public override void _Ready()
  {
    this._characterIconContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterIconContainer"));
    this._playerNameLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%PlayerNameLabel"));
    this._characterNameLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CharacterNameLabel"));
    this._ascensionLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AscensionLabel"));
    this._ascensionNumberLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AscensionNumberLabel"));
    this._readyIndicator = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ReadyIndicator"));
  }

  public void Fill(
    CharacterModel character,
    ulong playerId,
    int ascension,
    INetGameService netService)
  {
    NDailyRunCharacterContainer._ascensionLoc.Add(nameof (ascension), (Decimal) ascension);
    bool flag = netService.Type.IsMultiplayer();
    Control icon = character.Icon;
    foreach (Node child in ((Node) this._characterIconContainer).GetChildren(false))
      ((Node) this._characterIconContainer).RemoveChildSafely(child);
    ((CanvasItem) this._playerNameLabel).Visible = flag;
    ((CanvasItem) this._characterNameLabel).Modulate = flag ? StsColors.cream : StsColors.gold;
    ((Node) this._characterIconContainer).AddChildSafely((Node) icon);
    this._characterNameLabel.SetTextAutoSize(character.Title.GetFormattedText());
    this._playerNameLabel.SetTextAutoSize(PlatformUtil.GetPlayerNameRaw(netService.Platform, playerId));
    this._ascensionLabel.SetTextAutoSize(NDailyRunCharacterContainer._ascensionLoc.GetFormattedText());
    this._ascensionNumberLabel.SetTextAutoSize(ascension.ToString());
  }

  public void SetIsReady(bool isReady) => ((CanvasItem) this._readyIndicator).Visible = isReady;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDailyRunCharacterContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunCharacterContainer.MethodName.SetIsReady, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isReady"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunCharacterContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDailyRunCharacterContainer.MethodName.SetIsReady) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetIsReady(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDailyRunCharacterContainer.MethodName._Ready) || StringName.op_Equality(ref method, NDailyRunCharacterContainer.MethodName.SetIsReady) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._characterIconContainer))
    {
      this._characterIconContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._playerNameLabel))
    {
      this._playerNameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._characterNameLabel))
    {
      this._characterNameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._ascensionLabel))
    {
      this._ascensionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel))
    {
      this._ascensionNumberLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._readyIndicator))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._readyIndicator = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._characterIconContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterIconContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._playerNameLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._playerNameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._characterNameLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._characterNameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._ascensionLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionNumberLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunCharacterContainer.PropertyName._readyIndicator))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._readyIndicator);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDailyRunCharacterContainer.PropertyName._characterIconContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunCharacterContainer.PropertyName._playerNameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunCharacterContainer.PropertyName._characterNameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunCharacterContainer.PropertyName._ascensionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunCharacterContainer.PropertyName._readyIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDailyRunCharacterContainer.PropertyName._characterIconContainer, Variant.From<Control>(ref this._characterIconContainer));
    info.AddProperty(NDailyRunCharacterContainer.PropertyName._playerNameLabel, Variant.From<MegaLabel>(ref this._playerNameLabel));
    info.AddProperty(NDailyRunCharacterContainer.PropertyName._characterNameLabel, Variant.From<MegaLabel>(ref this._characterNameLabel));
    info.AddProperty(NDailyRunCharacterContainer.PropertyName._ascensionLabel, Variant.From<MegaLabel>(ref this._ascensionLabel));
    info.AddProperty(NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel, Variant.From<MegaLabel>(ref this._ascensionNumberLabel));
    info.AddProperty(NDailyRunCharacterContainer.PropertyName._readyIndicator, Variant.From<Control>(ref this._readyIndicator));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._characterIconContainer, ref variant1))
      this._characterIconContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._playerNameLabel, ref variant2))
      this._playerNameLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._characterNameLabel, ref variant3))
      this._characterNameLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._ascensionLabel, ref variant4))
      this._ascensionLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._ascensionNumberLabel, ref variant5))
      this._ascensionNumberLabel = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (!info.TryGetProperty(NDailyRunCharacterContainer.PropertyName._readyIndicator, ref variant6))
      return;
    this._readyIndicator = ((Variant) ref variant6).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetIsReady = StringName.op_Implicit(nameof (SetIsReady));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _characterIconContainer = StringName.op_Implicit(nameof (_characterIconContainer));
    public static readonly StringName _playerNameLabel = StringName.op_Implicit(nameof (_playerNameLabel));
    public static readonly StringName _characterNameLabel = StringName.op_Implicit(nameof (_characterNameLabel));
    public static readonly StringName _ascensionLabel = StringName.op_Implicit(nameof (_ascensionLabel));
    public static readonly StringName _ascensionNumberLabel = StringName.op_Implicit(nameof (_ascensionNumberLabel));
    public static readonly StringName _readyIndicator = StringName.op_Implicit(nameof (_readyIndicator));
  }

  public class SignalName : Control.SignalName
  {
  }
}
