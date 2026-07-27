// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NResetGameplayButton
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
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NResetGameplayButton.cs")]
public class NResetGameplayButton : NSettingsButton
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this.PivotOffset = Vector2.op_Multiply(this.Size, 0.5f);
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label")).SetTextAutoSize(new LocString("settings_ui", "RESET_SETTINGS_BUTTON").GetFormattedText());
  }

  private async Task ResetSettingsAfterConfirmation()
  {
    NGenericPopup modalToCreate = NGenericPopup.Create();
    NModalContainer.Instance.Add((Node) modalToCreate);
    if (!await modalToCreate.WaitForConfirmation(new LocString("settings_ui", "RESET_GAMEPLAY_CONFIRMATION.body"), new LocString("settings_ui", "RESET_CONFIRMATION.header"), new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new LocString("main_menu_ui", "GENERIC_POPUP.confirm")))
      return;
    Log.Info("Player reset general settings");
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    settingsSave.LimitFpsInBackground = true;
    settingsSave.SkipIntroLogo = false;
    PrefsSave prefsSave = SaveManager.Instance.PrefsSave;
    prefsSave.ScreenShakeOptionIndex = 2;
    prefsSave.FastMode = FastModeType.Normal;
    prefsSave.ShowRunTimer = false;
    prefsSave.ShowCardIndices = false;
    prefsSave.PhobiaMode = false;
    prefsSave.IsLongPressEnabled = false;
    prefsSave.UploadData = true;
    prefsSave.TextEffectsEnabled = true;
    prefsSave.ShowMultiplayerDrawings = true;
    ((GodotObject) NGame.Instance)?.EmitSignal(NGame.SignalName.PhobiaModeToggled, Array.Empty<Variant>());
    foreach (IResettableSettingNode resettableSettingNode in ((Node) ((Node) this).GetAncestorOfType<NSettingsPanel>()).GetChildrenRecursive<IResettableSettingNode>())
      resettableSettingNode.SetFromSettings();
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    TaskHelper.RunSafely(this.ResetSettingsAfterConfirmation());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NResetGameplayButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NResetGameplayButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NResetGameplayButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NResetGameplayButton.MethodName.OnRelease) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnRelease();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NResetGameplayButton.MethodName._Ready) || StringName.op_Equality(ref method, NResetGameplayButton.MethodName.OnRelease) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NSettingsButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
  }

  public new class PropertyName : NSettingsButton.PropertyName
  {
  }

  public new class SignalName : NSettingsButton.SignalName
  {
  }
}
