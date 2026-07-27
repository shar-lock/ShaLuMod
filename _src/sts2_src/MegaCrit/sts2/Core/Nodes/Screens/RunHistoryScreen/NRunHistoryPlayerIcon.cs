// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NRunHistoryPlayerIcon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRunHistoryPlayerIcon.cs")]
public class NRunHistoryPlayerIcon : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  public static readonly string scenePath = SceneHelper.GetScenePath("screens/run_history_screen/run_history_player_icon");
  private readonly List<IHoverTip> _hoverTips = new List<IHoverTip>();
  private Control _achievementLock;
  private Control _ascensionIcon;
  private MegaLabel _ascensionLabel;
  private NSelectionReticle _selectionReticle;
  private ShaderMaterial _hsv;
  private TextureRect _icon;
  private Tween? _tween;
  private static readonly Vector2 _enabledScale = Vector2.op_Multiply(Vector2.One, 1.1f);
  private static readonly Vector2 _disabledScale = Vector2.op_Multiply(Vector2.One, 0.95f);

  public RunHistoryPlayer Player { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._achievementLock = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AchievementLock"));
    this._ascensionIcon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AscensionIcon"));
    this._ascensionLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AscensionLabel"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).GetMaterial();
    this.Deselect();
  }

  public void LoadRun(RunHistoryPlayer player, RunHistory history)
  {
    this.Player = player;
    CharacterModel character = SaveUtil.CharacterOrDeprecated(player.Character);
    this._icon.Texture = character.IconTexture;
    LocString locString1 = new LocString("ascension", "PORTRAIT_TITLE");
    locString1.Add("character", character.Title);
    locString1.Add("ascension", (Decimal) history.Ascension);
    LocString locString2 = new LocString("ascension", "PORTRAIT_DESCRIPTION");
    List<string> variable = new List<string>();
    for (int level = 1; level <= history.Ascension; ++level)
      variable.Add(AscensionHelper.GetTitle(level).GetFormattedText());
    locString2.Add("ascensions", (IList<string>) variable);
    ((CanvasItem) this._achievementLock).Visible = history.GameMode.AreAchievementsAndEpochsLocked();
    ((CanvasItem) this._ascensionIcon).Visible = false;
    this._ascensionLabel.SetTextAutoSize(history.Ascension > 0 ? history.Ascension.ToString() : string.Empty);
    LocString description = new LocString("run_history", "PLAYER_HOVER");
    if (history.Players.Count > 1)
    {
      description.Add("PlayerName", PlatformUtil.GetPlayerName(history.PlatformType, player.Id));
      description.Add("CharacterName", character.Title.GetFormattedText());
    }
    else
    {
      description.Add("PlayerName", character.Title.GetFormattedText());
      description.Add("CharacterName", string.Empty);
    }
    if (history.Ascension > 0 || history.GameMode.AreAchievementsAndEpochsLocked())
      this._hoverTips.Add((IHoverTip) AscensionHelper.GetHoverTip(character, history.Ascension, history.GameMode.AreAchievementsAndEpochsLocked()));
    else
      this._hoverTips.Add((IHoverTip) new HoverTip(description));
  }

  public void Select()
  {
    this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._s, Variant.op_Implicit(1f));
    this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._v, Variant.op_Implicit(1f));
    ((CanvasItem) this._ascensionIcon).Visible = this._ascensionLabel.Text != string.Empty;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(NRunHistoryPlayerIcon._enabledScale), 0.05);
  }

  public void Deselect()
  {
    this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._s, Variant.op_Implicit(0.3f));
    this._hsv.SetShaderParameter(NRunHistoryPlayerIcon._v, Variant.op_Implicit(0.55f));
    ((CanvasItem) this._ascensionIcon).Visible = false;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(NRunHistoryPlayerIcon._disabledScale), 0.05);
  }

  protected override void OnFocus()
  {
    NHoverTipSet.CreateAndShow((Control) this, (IEnumerable<IHoverTip>) this._hoverTips)?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    this._selectionReticle.OnDeselect();
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NRunHistoryPlayerIcon.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistoryPlayerIcon.MethodName.Select, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistoryPlayerIcon.MethodName.Deselect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistoryPlayerIcon.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistoryPlayerIcon.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.Select) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Select();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.Deselect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Deselect();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName._Ready) || StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.Select) || StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.Deselect) || StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.OnFocus) || StringName.op_Equality(ref method, NRunHistoryPlayerIcon.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._achievementLock))
    {
      this._achievementLock = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._ascensionIcon))
    {
      this._ascensionIcon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._ascensionLabel))
    {
      this._ascensionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._achievementLock))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._achievementLock);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._ascensionIcon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._ascensionIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._ascensionLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._ascensionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunHistoryPlayerIcon.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._achievementLock, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._ascensionIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._ascensionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistoryPlayerIcon.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._achievementLock, Variant.From<Control>(ref this._achievementLock));
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._ascensionIcon, Variant.From<Control>(ref this._ascensionIcon));
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._ascensionLabel, Variant.From<MegaLabel>(ref this._ascensionLabel));
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NRunHistoryPlayerIcon.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._achievementLock, ref variant1))
      this._achievementLock = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._ascensionIcon, ref variant2))
      this._ascensionIcon = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._ascensionLabel, ref variant3))
      this._ascensionLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._selectionReticle, ref variant4))
      this._selectionReticle = ((Variant) ref variant4).As<NSelectionReticle>();
    Variant variant5;
    if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._hsv, ref variant5))
      this._hsv = ((Variant) ref variant5).As<ShaderMaterial>();
    Variant variant6;
    if (info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._icon, ref variant6))
      this._icon = ((Variant) ref variant6).As<TextureRect>();
    Variant variant7;
    if (!info.TryGetProperty(NRunHistoryPlayerIcon.PropertyName._tween, ref variant7))
      return;
    this._tween = ((Variant) ref variant7).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Select = StringName.op_Implicit(nameof (Select));
    public static readonly StringName Deselect = StringName.op_Implicit(nameof (Deselect));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _achievementLock = StringName.op_Implicit(nameof (_achievementLock));
    public static readonly StringName _ascensionIcon = StringName.op_Implicit(nameof (_ascensionIcon));
    public static readonly StringName _ascensionLabel = StringName.op_Implicit(nameof (_ascensionLabel));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
