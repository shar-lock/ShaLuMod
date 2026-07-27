// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NContinueRunInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NContinueRunInfo.cs")]
public class NContinueRunInfo : Control
{
  private Tween? _visTween;
  private Vector2 _initPosition;
  private Control _runInfoContainer;
  private Control _errorContainer;
  private MegaRichTextLabel _dateLabel;
  private MegaRichTextLabel _goldLabel;
  private MegaRichTextLabel _healthLabel;
  private MegaRichTextLabel _progressLabel;
  private MegaRichTextLabel _ascensionLabel;
  private TextureRect _charIcon;
  private bool _isShown;

  public bool HasResult { get; private set; }

  public override void _Ready()
  {
    this._initPosition = this.Position;
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    this._runInfoContainer = (Control) ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("%RunInfoContainer"));
    this._errorContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ErrorContainer"));
    this._dateLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DateLabel"));
    this._goldLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%GoldLabel"));
    this._healthLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%HealthLabel"));
    this._progressLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ProgressLabel"));
    this._charIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%CharacterIcon"));
    this._ascensionLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%AscensionLabel"));
  }

  public void AnimShow()
  {
    this._visTween?.Kill();
    this._visTween = ((Node) this).CreateTween().SetParallel(true);
    this._visTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(this._initPosition, new Vector2(0.0f, -20f))), 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._visTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.20000000298023224);
    this._isShown = true;
  }

  public void AnimHide()
  {
    if (!this._isShown)
      return;
    this._visTween?.Kill();
    this._visTween = ((Node) this).CreateTween().SetParallel(true);
    this._visTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._initPosition), 0.20000000298023224);
    this._visTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.20000000298023224);
    this._isShown = false;
  }

  public void SetResult(ReadSaveResult<SerializableRun>? result)
  {
    if (result != null && result.Success)
    {
      if (result.SaveData != null)
      {
        try
        {
          this.ShowInfo(result.SaveData);
          goto label_6;
        }
        catch (Exception ex)
        {
          Log.Warn($"Exception occurred while displaying continue run info: {ex}");
          this.ShowError();
          goto label_6;
        }
      }
    }
    if (result != null)
      this.ShowError();
label_6:
    this.HasResult = result != null;
  }

  private void ShowInfo(SerializableRun save)
  {
    ((CanvasItem) this._errorContainer).Visible = false;
    ((CanvasItem) this._runInfoContainer).Visible = true;
    DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
    string variable = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(save.SaveTime).UtcDateTime, TimeZoneInfo.Local).ToString(new LocString("main_menu_ui", "CONTINUE_RUN_INFO.savedTimeFormat").GetRawText(), (IFormatProvider) dateTimeFormat);
    LocString locString = new LocString("main_menu_ui", "CONTINUE_RUN_INFO.saved");
    locString.Add("LastSavedTime", variable);
    this._dateLabel.Text = locString.GetFormattedText();
    if (save.Ascension > 0)
      this._ascensionLabel.Text = $"{new LocString("main_menu_ui", "CONTINUE_RUN_INFO.ascension").GetFormattedText()} {save.Ascension}";
    else
      ((CanvasItem) this._ascensionLabel).Visible = false;
    ActModel byId = ModelDb.GetById<ActModel>(save.Acts[save.CurrentActIndex].Id);
    SerializablePlayer player = save.Players[0];
    this._charIcon.Texture = ModelDb.GetById<CharacterModel>(player.CharacterId).IconTexture;
    string formattedText1 = byId.Title.GetFormattedText();
    string formattedText2 = new LocString("main_menu_ui", "CONTINUE_RUN_INFO.floor").GetFormattedText();
    int count = save.VisitedMapCoords.Count;
    for (int index = 0; index < save.CurrentActIndex; ++index)
      count += ModelDb.GetById<ActModel>(save.Acts[index].Id).GetNumberOfFloors(save.Players.Count > 1);
    this._progressLabel.Text = $"{formattedText1} [blue]- {formattedText2} {count}[/blue]";
    this._healthLabel.Text = $"[red]{player.CurrentHp}/{player.MaxHp}[/red]";
    this._goldLabel.Text = $"[gold]{player.Gold}[/gold]";
  }

  private void ShowError()
  {
    ((CanvasItem) this._runInfoContainer).Visible = false;
    ((CanvasItem) this._errorContainer).Visible = true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NContinueRunInfo.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NContinueRunInfo.MethodName.AnimShow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NContinueRunInfo.MethodName.AnimHide, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NContinueRunInfo.MethodName.ShowError, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NContinueRunInfo.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NContinueRunInfo.MethodName.AnimShow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimShow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NContinueRunInfo.MethodName.AnimHide) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHide();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NContinueRunInfo.MethodName.ShowError) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShowError();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NContinueRunInfo.MethodName._Ready) || StringName.op_Equality(ref method, NContinueRunInfo.MethodName.AnimShow) || StringName.op_Equality(ref method, NContinueRunInfo.MethodName.AnimHide) || StringName.op_Equality(ref method, NContinueRunInfo.MethodName.ShowError) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName.HasResult))
    {
      this.HasResult = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._visTween))
    {
      this._visTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._initPosition))
    {
      this._initPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._runInfoContainer))
    {
      this._runInfoContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._errorContainer))
    {
      this._errorContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._dateLabel))
    {
      this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._goldLabel))
    {
      this._goldLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._healthLabel))
    {
      this._healthLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._progressLabel))
    {
      this._progressLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._ascensionLabel))
    {
      this._ascensionLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._charIcon))
    {
      this._charIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._isShown))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isShown = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName.HasResult))
    {
      ref godot_variant local = ref value;
      bool hasResult = this.HasResult;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasResult);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._visTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._visTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._initPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._initPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._runInfoContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._runInfoContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._errorContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._errorContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._dateLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._goldLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._goldLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._healthLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._healthLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._progressLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._progressLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._ascensionLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._ascensionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._charIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._charIcon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NContinueRunInfo.PropertyName._isShown))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isShown);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._visTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NContinueRunInfo.PropertyName._initPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._runInfoContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._errorContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._dateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._goldLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._healthLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._progressLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._ascensionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NContinueRunInfo.PropertyName._charIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NContinueRunInfo.PropertyName._isShown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NContinueRunInfo.PropertyName.HasResult, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName hasResult1 = NContinueRunInfo.PropertyName.HasResult;
    bool hasResult2 = this.HasResult;
    Variant variant = Variant.From<bool>(ref hasResult2);
    serializationInfo.AddProperty(hasResult1, variant);
    info.AddProperty(NContinueRunInfo.PropertyName._visTween, Variant.From<Tween>(ref this._visTween));
    info.AddProperty(NContinueRunInfo.PropertyName._initPosition, Variant.From<Vector2>(ref this._initPosition));
    info.AddProperty(NContinueRunInfo.PropertyName._runInfoContainer, Variant.From<Control>(ref this._runInfoContainer));
    info.AddProperty(NContinueRunInfo.PropertyName._errorContainer, Variant.From<Control>(ref this._errorContainer));
    info.AddProperty(NContinueRunInfo.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
    info.AddProperty(NContinueRunInfo.PropertyName._goldLabel, Variant.From<MegaRichTextLabel>(ref this._goldLabel));
    info.AddProperty(NContinueRunInfo.PropertyName._healthLabel, Variant.From<MegaRichTextLabel>(ref this._healthLabel));
    info.AddProperty(NContinueRunInfo.PropertyName._progressLabel, Variant.From<MegaRichTextLabel>(ref this._progressLabel));
    info.AddProperty(NContinueRunInfo.PropertyName._ascensionLabel, Variant.From<MegaRichTextLabel>(ref this._ascensionLabel));
    info.AddProperty(NContinueRunInfo.PropertyName._charIcon, Variant.From<TextureRect>(ref this._charIcon));
    info.AddProperty(NContinueRunInfo.PropertyName._isShown, Variant.From<bool>(ref this._isShown));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName.HasResult, ref variant1))
      this.HasResult = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._visTween, ref variant2))
      this._visTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._initPosition, ref variant3))
      this._initPosition = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._runInfoContainer, ref variant4))
      this._runInfoContainer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._errorContainer, ref variant5))
      this._errorContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._dateLabel, ref variant6))
      this._dateLabel = ((Variant) ref variant6).As<MegaRichTextLabel>();
    Variant variant7;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._goldLabel, ref variant7))
      this._goldLabel = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._healthLabel, ref variant8))
      this._healthLabel = ((Variant) ref variant8).As<MegaRichTextLabel>();
    Variant variant9;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._progressLabel, ref variant9))
      this._progressLabel = ((Variant) ref variant9).As<MegaRichTextLabel>();
    Variant variant10;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._ascensionLabel, ref variant10))
      this._ascensionLabel = ((Variant) ref variant10).As<MegaRichTextLabel>();
    Variant variant11;
    if (info.TryGetProperty(NContinueRunInfo.PropertyName._charIcon, ref variant11))
      this._charIcon = ((Variant) ref variant11).As<TextureRect>();
    Variant variant12;
    if (!info.TryGetProperty(NContinueRunInfo.PropertyName._isShown, ref variant12))
      return;
    this._isShown = ((Variant) ref variant12).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimShow = StringName.op_Implicit(nameof (AnimShow));
    public static readonly StringName AnimHide = StringName.op_Implicit(nameof (AnimHide));
    public static readonly StringName ShowError = StringName.op_Implicit(nameof (ShowError));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName HasResult = StringName.op_Implicit(nameof (HasResult));
    public static readonly StringName _visTween = StringName.op_Implicit(nameof (_visTween));
    public static readonly StringName _initPosition = StringName.op_Implicit(nameof (_initPosition));
    public static readonly StringName _runInfoContainer = StringName.op_Implicit(nameof (_runInfoContainer));
    public static readonly StringName _errorContainer = StringName.op_Implicit(nameof (_errorContainer));
    public static readonly StringName _dateLabel = StringName.op_Implicit(nameof (_dateLabel));
    public static readonly StringName _goldLabel = StringName.op_Implicit(nameof (_goldLabel));
    public static readonly StringName _healthLabel = StringName.op_Implicit(nameof (_healthLabel));
    public static readonly StringName _progressLabel = StringName.op_Implicit(nameof (_progressLabel));
    public static readonly StringName _ascensionLabel = StringName.op_Implicit(nameof (_ascensionLabel));
    public static readonly StringName _charIcon = StringName.op_Implicit(nameof (_charIcon));
    public static readonly StringName _isShown = StringName.op_Implicit(nameof (_isShown));
  }

  public class SignalName : Control.SignalName
  {
  }
}
