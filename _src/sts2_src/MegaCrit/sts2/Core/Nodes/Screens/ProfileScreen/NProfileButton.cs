// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen.NProfileButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NProfileButton.cs")]
public class NProfileButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private MegaRichTextLabel _title;
  private MegaRichTextLabel _description;
  private Control _currentProfileIndicator;
  private NProfileIcon _profileIcon;
  private NDeleteProfileButton _deleteButton;
  private TextureRect _background;
  private ShaderMaterial _hsv;
  private NProfileScreen? _profileScreen;
  private Tween? _tween;
  private int _profileId;

  public static IEnumerable<string> AssetPaths => NProfileIcon.AssetPaths;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._background = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Background"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._background).Material;
    this._title = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Title"));
    this._description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Info"));
    this._profileIcon = ((Node) this).GetNode<NProfileIcon>(NodePath.op_Implicit("%ProfileIcon"));
    this._currentProfileIndicator = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CurrentProfileIndicator"));
  }

  public void Initialize(NProfileScreen profileScreen, int profileId)
  {
    this._profileScreen = profileScreen;
    this._profileId = profileId;
    LocString locString1 = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.title");
    locString1.Add("Id", (Decimal) profileId);
    this._title.Text = locString1.GetFormattedText();
    GodotFileIo godotFileIo = new GodotFileIo(UserDataPathProvider.GetProfileScopedPath(profileId, "saves"));
    this._profileIcon.SetProfileId(profileId);
    ((CanvasItem) this._currentProfileIndicator).Visible = SaveManager.Instance.CurrentProfileId == profileId;
    string path1 = "progress.save";
    int? profileAsDeleted = NProfileScreen.forceShowProfileAsDeleted;
    int num = profileId;
    if (profileAsDeleted.GetValueOrDefault() == num & profileAsDeleted.HasValue || !godotFileIo.FileExists(path1))
    {
      this._description.Text = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.empty").GetFormattedText();
    }
    else
    {
      LocString locString2 = new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.description");
      if (SaveManager.Instance.CurrentProfileId == profileId)
      {
        locString2.Add("Playtime", TimeFormatting.Format((float) SaveManager.Instance.Progress.TotalPlaytime));
      }
      else
      {
        JsonNode jsonNode;
        long time;
        if (JsonSerializer.Deserialize<JsonObject>(godotFileIo.ReadFile(path1), JsonSerializationUtility.GetTypeInfo<JsonObject>()).TryGetPropertyValue("total_playtime", ref jsonNode) && jsonNode is JsonValue jsonValue && jsonValue.TryGetValue<long>(ref time))
          locString2.Add("Playtime", TimeFormatting.Format((float) time));
        else
          locString2.Add("Playtime", "???");
      }
      DateTimeOffset dateTimeOffset = godotFileIo.GetLastModifiedTime(path1);
      string path2 = "current_run.save";
      if (godotFileIo.FileExists(path2))
      {
        DateTimeOffset lastModifiedTime = godotFileIo.GetLastModifiedTime(path2);
        dateTimeOffset = dateTimeOffset > lastModifiedTime ? dateTimeOffset : lastModifiedTime;
      }
      string path3 = "current_run_mp.save";
      if (godotFileIo.FileExists(path3))
      {
        DateTimeOffset lastModifiedTime = godotFileIo.GetLastModifiedTime(path3);
        dateTimeOffset = dateTimeOffset > lastModifiedTime ? dateTimeOffset : lastModifiedTime;
      }
      DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
      string variable = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, TimeZoneInfo.Local).ToString(new LocString("main_menu_ui", "PROFILE_SCREEN.BUTTON.dateFormat").GetFormattedText(), (IFormatProvider) dateTimeFormat);
      locString2.Add("LastUpdatedTime", variable);
      this._description.Text = locString2.GetFormattedText();
    }
  }

  protected override void OnRelease()
  {
    if (SaveManager.Instance.CurrentProfileId == this._profileId)
      NGame.Instance.MainMenu.SubmenuStack.Pop();
    else
      TaskHelper.RunSafely(this.SwitchToThisProfile());
  }

  private async Task SwitchToThisProfile()
  {
    this._profileScreen?.ShowLoading();
    double num1 = (double) await ((Node) this).AwaitProcessFrame();
    double num2 = (double) await ((Node) this).AwaitProcessFrame();
    SaveManager.Instance.SwitchProfileId(this._profileId);
    ReadSaveResult<PrefsSave> prefsReadResult = SaveManager.Instance.InitPrefsData();
    ReadSaveResult<SerializableProgress> progressReadResult = SaveManager.Instance.InitProgressData();
    NGame.Instance.ReloadMainMenu();
    NGame.Instance.CheckShowSaveFileError(progressReadResult, prefsReadResult, new ReadSaveResult<SettingsSave>(new SettingsSave()));
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.03f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProfileButton._v), Variant.op_Implicit(1.3f), 0.05);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProfileButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NProfileButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NProfileButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileButton.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("profileScreen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("profileId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NProfileButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProfileButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NProfileButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileButton.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Initialize(VariantUtils.ConvertTo<NProfileScreen>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProfileButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NProfileButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NProfileButton.MethodName._Ready) || StringName.op_Equality(ref method, NProfileButton.MethodName.Initialize) || StringName.op_Equality(ref method, NProfileButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NProfileButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NProfileButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NProfileButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._currentProfileIndicator))
    {
      this._currentProfileIndicator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._profileIcon))
    {
      this._profileIcon = VariantUtils.ConvertTo<NProfileIcon>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._deleteButton))
    {
      this._deleteButton = VariantUtils.ConvertTo<NDeleteProfileButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._background))
    {
      this._background = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._profileScreen))
    {
      this._profileScreen = VariantUtils.ConvertTo<NProfileScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NProfileButton.PropertyName._profileId))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._profileId = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._currentProfileIndicator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._currentProfileIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._profileIcon))
    {
      value = VariantUtils.CreateFrom<NProfileIcon>(ref this._profileIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._deleteButton))
    {
      value = VariantUtils.CreateFrom<NDeleteProfileButton>(ref this._deleteButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._background))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._background);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._profileScreen))
    {
      value = VariantUtils.CreateFrom<NProfileScreen>(ref this._profileScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NProfileButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NProfileButton.PropertyName._profileId))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._profileId);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._currentProfileIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._profileIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._deleteButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._profileScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProfileButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NProfileButton.PropertyName._profileId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NProfileButton.PropertyName._title, Variant.From<MegaRichTextLabel>(ref this._title));
    info.AddProperty(NProfileButton.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NProfileButton.PropertyName._currentProfileIndicator, Variant.From<Control>(ref this._currentProfileIndicator));
    info.AddProperty(NProfileButton.PropertyName._profileIcon, Variant.From<NProfileIcon>(ref this._profileIcon));
    info.AddProperty(NProfileButton.PropertyName._deleteButton, Variant.From<NDeleteProfileButton>(ref this._deleteButton));
    info.AddProperty(NProfileButton.PropertyName._background, Variant.From<TextureRect>(ref this._background));
    info.AddProperty(NProfileButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NProfileButton.PropertyName._profileScreen, Variant.From<NProfileScreen>(ref this._profileScreen));
    info.AddProperty(NProfileButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NProfileButton.PropertyName._profileId, Variant.From<int>(ref this._profileId));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NProfileButton.PropertyName._title, ref variant1))
      this._title = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NProfileButton.PropertyName._description, ref variant2))
      this._description = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NProfileButton.PropertyName._currentProfileIndicator, ref variant3))
      this._currentProfileIndicator = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NProfileButton.PropertyName._profileIcon, ref variant4))
      this._profileIcon = ((Variant) ref variant4).As<NProfileIcon>();
    Variant variant5;
    if (info.TryGetProperty(NProfileButton.PropertyName._deleteButton, ref variant5))
      this._deleteButton = ((Variant) ref variant5).As<NDeleteProfileButton>();
    Variant variant6;
    if (info.TryGetProperty(NProfileButton.PropertyName._background, ref variant6))
      this._background = ((Variant) ref variant6).As<TextureRect>();
    Variant variant7;
    if (info.TryGetProperty(NProfileButton.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NProfileButton.PropertyName._profileScreen, ref variant8))
      this._profileScreen = ((Variant) ref variant8).As<NProfileScreen>();
    Variant variant9;
    if (info.TryGetProperty(NProfileButton.PropertyName._tween, ref variant9))
      this._tween = ((Variant) ref variant9).As<Tween>();
    Variant variant10;
    if (!info.TryGetProperty(NProfileButton.PropertyName._profileId, ref variant10))
      return;
    this._profileId = ((Variant) ref variant10).As<int>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _currentProfileIndicator = StringName.op_Implicit(nameof (_currentProfileIndicator));
    public static readonly StringName _profileIcon = StringName.op_Implicit(nameof (_profileIcon));
    public static readonly StringName _deleteButton = StringName.op_Implicit(nameof (_deleteButton));
    public static readonly StringName _background = StringName.op_Implicit(nameof (_background));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _profileScreen = StringName.op_Implicit(nameof (_profileScreen));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _profileId = StringName.op_Implicit(nameof (_profileId));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
