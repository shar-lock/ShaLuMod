// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Ftue.NAcceptTutorialsFtue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Ftue;

[ScriptPath("res://src/Core/Nodes/Ftue/NAcceptTutorialsFtue.cs")]
public class NAcceptTutorialsFtue : NFtue
{
  public const string id = "accept_tutorials_ftue";
  private static readonly string _scenePath = SceneHelper.GetScenePath("ftue/accept_tutorials_ftue");
  private NCharacterSelectScreen _charSelectScreen;
  private NVerticalPopup _verticalPopup;
  private Action _onFinished;

  public override void _Ready()
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._verticalPopup.SetText(new LocString("main_menu_ui", "ENABLE_TUTORIALS.title"), new LocString("main_menu_ui", "ENABLE_TUTORIALS.description"));
    this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.confirm"), new Action<NButton>(this.YesTutorials));
    this._verticalPopup.InitNoButton(new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new Action<NButton>(this.NoTutorials));
  }

  public static NAcceptTutorialsFtue? Create(
    NCharacterSelectScreen charSelectScreen,
    Action onFinished)
  {
    if (TestMode.IsOn)
      return (NAcceptTutorialsFtue) null;
    NAcceptTutorialsFtue nacceptTutorialsFtue = PreloadManager.Cache.GetScene(NAcceptTutorialsFtue._scenePath).Instantiate<NAcceptTutorialsFtue>((PackedScene.GenEditState) 0L);
    nacceptTutorialsFtue._charSelectScreen = charSelectScreen;
    nacceptTutorialsFtue._onFinished = onFinished;
    return nacceptTutorialsFtue;
  }

  private void NoTutorials(NButton _)
  {
    SaveManager.Instance.MarkFtueAsComplete("accept_tutorials_ftue");
    SaveManager.Instance.SetFtuesEnabled(false);
    this._onFinished();
    this.CloseFtue();
  }

  private void YesTutorials(NButton _)
  {
    SaveManager.Instance.MarkFtueAsComplete("accept_tutorials_ftue");
    this._onFinished();
    this.CloseFtue();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NAcceptTutorialsFtue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAcceptTutorialsFtue.MethodName.NoTutorials, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAcceptTutorialsFtue.MethodName.YesTutorials, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NAcceptTutorialsFtue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAcceptTutorialsFtue.MethodName.NoTutorials) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.NoTutorials(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAcceptTutorialsFtue.MethodName.YesTutorials) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.YesTutorials(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAcceptTutorialsFtue.MethodName._Ready) || StringName.op_Equality(ref method, NAcceptTutorialsFtue.MethodName.NoTutorials) || StringName.op_Equality(ref method, NAcceptTutorialsFtue.MethodName.YesTutorials) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAcceptTutorialsFtue.PropertyName._charSelectScreen))
    {
      this._charSelectScreen = VariantUtils.ConvertTo<NCharacterSelectScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAcceptTutorialsFtue.PropertyName._verticalPopup))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAcceptTutorialsFtue.PropertyName._charSelectScreen))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref this._charSelectScreen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAcceptTutorialsFtue.PropertyName._verticalPopup))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAcceptTutorialsFtue.PropertyName._charSelectScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAcceptTutorialsFtue.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAcceptTutorialsFtue.PropertyName._charSelectScreen, Variant.From<NCharacterSelectScreen>(ref this._charSelectScreen));
    info.AddProperty(NAcceptTutorialsFtue.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAcceptTutorialsFtue.PropertyName._charSelectScreen, ref variant1))
      this._charSelectScreen = ((Variant) ref variant1).As<NCharacterSelectScreen>();
    Variant variant2;
    if (!info.TryGetProperty(NAcceptTutorialsFtue.PropertyName._verticalPopup, ref variant2))
      return;
    this._verticalPopup = ((Variant) ref variant2).As<NVerticalPopup>();
  }

  public new class MethodName : NFtue.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName NoTutorials = StringName.op_Implicit(nameof (NoTutorials));
    public static readonly StringName YesTutorials = StringName.op_Implicit(nameof (YesTutorials));
  }

  public new class PropertyName : NFtue.PropertyName
  {
    public static readonly StringName _charSelectScreen = StringName.op_Implicit(nameof (_charSelectScreen));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
  }

  public new class SignalName : NFtue.SignalName
  {
  }
}
