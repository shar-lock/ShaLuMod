// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen.NDeleteProfileButton
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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ProfileScreen/NDeleteProfileButton.cs")]
public class NDeleteProfileButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly LocString _title = new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.title");
  private static readonly LocString _description = new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.description");
  private static readonly LocString _buttonMesssage = new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_BUTTON.label");
  private TextureRect _icon;
  private MegaLabel _label;
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private NProfileScreen _profileScreen;
  private int _profileId;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%MegaLabel"));
    this._label.SetTextAutoSize(NDeleteProfileButton._buttonMesssage.GetFormattedText());
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
  }

  public void Initialize(NProfileScreen profileScreen, int profileId)
  {
    this._profileScreen = profileScreen;
    this._profileId = profileId;
    int? profileAsDeleted = NProfileScreen.forceShowProfileAsDeleted;
    int num = profileId;
    ((CanvasItem) this).Visible = !(profileAsDeleted.GetValueOrDefault() == num & profileAsDeleted.HasValue) && FileAccess.FileExists(UserDataPathProvider.GetProfileScopedPath(profileId, "saves/progress.save"));
  }

  protected override void OnRelease() => TaskHelper.RunSafely(this.ConfirmDeletion());

  private async Task ConfirmDeletion()
  {
    NGenericPopup modalToCreate = NGenericPopup.Create();
    NModalContainer.Instance.Add((Node) modalToCreate);
    NDeleteProfileButton._title.Add("Id", (Decimal) this._profileId);
    NDeleteProfileButton._description.Add("Id", (Decimal) this._profileId);
    if (!await modalToCreate.WaitForConfirmation(NDeleteProfileButton._description, NDeleteProfileButton._title, new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.cancel"), new LocString("main_menu_ui", "PROFILE_SCREEN.DELETE_CONFIRM_POPUP.delete")))
      return;
    Log.Info($"Player clicked yes on confirm deletion popup for {this._profileId}");
    SaveManager.Instance.DeleteProfile(this._profileId);
    NProfileScreen.forceShowProfileAsDeleted = new int?(this._profileId);
    SaveManager.Instance.InitProgressData();
    SaveManager.Instance.InitPrefsData();
    if (this._profileId == SaveManager.Instance.CurrentProfileId)
    {
      NGame.Instance.ReloadMainMenu();
      Callable callable = Callable.From(new Action(NGame.Instance.MainMenu.OpenProfileScreen));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
    else
      this._profileScreen.Refresh();
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDeleteProfileButton._v), Variant.op_Implicit(1.4f), 0.05);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position:y"), Variant.op_Implicit(78f), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(48f));
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDeleteProfileButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.05);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NDeleteProfileButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NDeleteProfileButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeleteProfileButton.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("profileScreen"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("profileId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDeleteProfileButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeleteProfileButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeleteProfileButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDeleteProfileButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NDeleteProfileButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.Initialize(VariantUtils.ConvertTo<NProfileScreen>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDeleteProfileButton.MethodName._Ready) || StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.Initialize) || StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NDeleteProfileButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._profileScreen))
    {
      this._profileScreen = VariantUtils.ConvertTo<NProfileScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._profileId))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._profileId = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._profileScreen))
    {
      value = VariantUtils.CreateFrom<NProfileScreen>(ref this._profileScreen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDeleteProfileButton.PropertyName._profileId))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._profileId);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDeleteProfileButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeleteProfileButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeleteProfileButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeleteProfileButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDeleteProfileButton.PropertyName._profileScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NDeleteProfileButton.PropertyName._profileId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDeleteProfileButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NDeleteProfileButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NDeleteProfileButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NDeleteProfileButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NDeleteProfileButton.PropertyName._profileScreen, Variant.From<NProfileScreen>(ref this._profileScreen));
    info.AddProperty(NDeleteProfileButton.PropertyName._profileId, Variant.From<int>(ref this._profileId));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDeleteProfileButton.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NDeleteProfileButton.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDeleteProfileButton.PropertyName._hsv, ref variant3))
      this._hsv = ((Variant) ref variant3).As<ShaderMaterial>();
    Variant variant4;
    if (info.TryGetProperty(NDeleteProfileButton.PropertyName._tween, ref variant4))
      this._tween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NDeleteProfileButton.PropertyName._profileScreen, ref variant5))
      this._profileScreen = ((Variant) ref variant5).As<NProfileScreen>();
    Variant variant6;
    if (!info.TryGetProperty(NDeleteProfileButton.PropertyName._profileId, ref variant6))
      return;
    this._profileId = ((Variant) ref variant6).As<int>();
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
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _profileScreen = StringName.op_Implicit(nameof (_profileScreen));
    public static readonly StringName _profileId = StringName.op_Implicit(nameof (_profileId));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
