// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NInputSettingsPanel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NInputSettingsPanel.cs")]
public class NInputSettingsPanel : NSettingsPanel
{
  private float _minPadding = 50f;
  private NInputSettingsEntry? _listeningEntry;
  private NButton _resetToDefaultButton;
  private MegaRichTextLabel _commandHeader;
  private MegaRichTextLabel _keyboardHeader;
  private MegaRichTextLabel _controllerHeader;
  private MegaRichTextLabel _steamInputPrompt;

  public override void _Ready()
  {
    base._Ready();
    this._resetToDefaultButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%ResetToDefaultButton"));
    this._commandHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%CommandHeader"));
    this._keyboardHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%KeyboardHeader"));
    this._controllerHeader = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ControllerHeader"));
    this._steamInputPrompt = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%SteamInputPrompt"));
    ((GodotObject) this._resetToDefaultButton).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => NInputManager.Instance.ResetToDefaults())), 0U);
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnViewportSizeChange)), 0U);
    this._commandHeader.Text = new LocString("settings_ui", "INPUT_SETTINGS.COMMAND_HEADER").GetFormattedText();
    this._keyboardHeader.Text = new LocString("settings_ui", "INPUT_SETTINGS.KEYBOARD_HEADER").GetFormattedText();
    this._controllerHeader.Text = new LocString("settings_ui", "INPUT_SETTINGS.CONTROLLER_HEADER").GetFormattedText();
    this._steamInputPrompt.Text = new LocString("settings_ui", "INPUT_SETTINGS.STEAM_INPUT_DETECTED").GetFormattedText();
    foreach (StringName stringName in (IEnumerable<StringName>) NInputManager.remappableControllerInputs.Concat<StringName>((IEnumerable<StringName>) NInputManager.remappableKeyboardInputs).Distinct<StringName>().ToList<StringName>())
    {
      NInputSettingsEntry entry = NInputSettingsEntry.Create(StringName.op_Implicit(stringName));
      ((GodotObject) entry).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.SetAsListeningEntry(entry))), 0U);
      ((Node) this.Content).AddChildSafely((Node) entry);
    }
    this.UpdateNavigation();
  }

  private async Task RefreshSize()
  {
    double num1 = (double) await ((Node) this).AwaitProcessFrame();
    double num2 = (double) await ((Node) this).AwaitProcessFrame();
    Vector2 size = ((Node) this).GetParent<Control>().Size;
    Vector2 minimumSize = ((Control) this.Content).GetMinimumSize();
    if ((double) minimumSize.Y + (double) this._minPadding < (double) size.Y)
      return;
    this.Size = new Vector2(((Control) this.Content).Size.X, minimumSize.Y + size.Y * 0.4f);
  }

  private void OnViewportSizeChange() => TaskHelper.RunSafely(this.RefreshSize());

  protected override void OnVisibilityChange()
  {
    base.OnVisibilityChange();
    this._listeningEntry = (NInputSettingsEntry) null;
    ((CanvasItem) this._steamInputPrompt).Visible = !NControllerManager.Instance.ShouldAllowControllerRebinding;
    TaskHelper.RunSafely(this.RefreshSize());
  }

  private void SetAsListeningEntry(NInputSettingsEntry entry) => this._listeningEntry = entry;

  public override void _UnhandledKeyInput(InputEvent inputEvent)
  {
    if (this._listeningEntry == null || !NInputManager.remappableKeyboardInputs.Contains<StringName>(this._listeningEntry.InputName) || !(inputEvent is InputEventKey inputEventKey))
      return;
    NInputManager.Instance.ModifyShortcutKey(this._listeningEntry.InputName, inputEventKey.Keycode);
    ((Node) this).GetViewport()?.SetInputAsHandled();
    this._listeningEntry = (NInputSettingsEntry) null;
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (this._listeningEntry == null)
      return;
    foreach (StringName allControllerInput in Controller.AllControllerInputs)
    {
      if (inputEvent.IsActionReleased(allControllerInput, false))
      {
        if (NInputManager.remappableControllerInputs.Contains<StringName>(this._listeningEntry.InputName) && NControllerManager.Instance.ShouldAllowControllerRebinding)
          NInputManager.Instance.ModifyControllerButton(this._listeningEntry.InputName, allControllerInput);
        ((Node) this).GetViewport()?.SetInputAsHandled();
        this._listeningEntry = (NInputSettingsEntry) null;
        break;
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NInputSettingsPanel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsPanel.MethodName.OnViewportSizeChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsPanel.MethodName.OnVisibilityChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsPanel.MethodName.SetAsListeningEntry, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInputSettingsPanel.MethodName._UnhandledKeyInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NInputSettingsPanel.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInputSettingsPanel.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsPanel.MethodName.OnViewportSizeChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnViewportSizeChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsPanel.MethodName.OnVisibilityChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsPanel.MethodName.SetAsListeningEntry) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAsListeningEntry(VariantUtils.ConvertTo<NInputSettingsEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsPanel.MethodName._UnhandledKeyInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._UnhandledKeyInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NInputSettingsPanel.MethodName._Input) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NInputSettingsPanel.MethodName._Ready) || StringName.op_Equality(ref method, NInputSettingsPanel.MethodName.OnViewportSizeChange) || StringName.op_Equality(ref method, NInputSettingsPanel.MethodName.OnVisibilityChange) || StringName.op_Equality(ref method, NInputSettingsPanel.MethodName.SetAsListeningEntry) || StringName.op_Equality(ref method, NInputSettingsPanel.MethodName._UnhandledKeyInput) || StringName.op_Equality(ref method, NInputSettingsPanel.MethodName._Input) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._minPadding))
    {
      this._minPadding = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._listeningEntry))
    {
      this._listeningEntry = VariantUtils.ConvertTo<NInputSettingsEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._resetToDefaultButton))
    {
      this._resetToDefaultButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._commandHeader))
    {
      this._commandHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._keyboardHeader))
    {
      this._keyboardHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._controllerHeader))
    {
      this._controllerHeader = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._steamInputPrompt))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._steamInputPrompt = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._minPadding))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minPadding);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._listeningEntry))
    {
      value = VariantUtils.CreateFrom<NInputSettingsEntry>(ref this._listeningEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._resetToDefaultButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._resetToDefaultButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._commandHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._commandHeader);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._keyboardHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._keyboardHeader);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._controllerHeader))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._controllerHeader);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInputSettingsPanel.PropertyName._steamInputPrompt))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._steamInputPrompt);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NInputSettingsPanel.PropertyName._minPadding, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsPanel.PropertyName._listeningEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsPanel.PropertyName._resetToDefaultButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsPanel.PropertyName._commandHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsPanel.PropertyName._keyboardHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsPanel.PropertyName._controllerHeader, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsPanel.PropertyName._steamInputPrompt, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NInputSettingsPanel.PropertyName._minPadding, Variant.From<float>(ref this._minPadding));
    info.AddProperty(NInputSettingsPanel.PropertyName._listeningEntry, Variant.From<NInputSettingsEntry>(ref this._listeningEntry));
    info.AddProperty(NInputSettingsPanel.PropertyName._resetToDefaultButton, Variant.From<NButton>(ref this._resetToDefaultButton));
    info.AddProperty(NInputSettingsPanel.PropertyName._commandHeader, Variant.From<MegaRichTextLabel>(ref this._commandHeader));
    info.AddProperty(NInputSettingsPanel.PropertyName._keyboardHeader, Variant.From<MegaRichTextLabel>(ref this._keyboardHeader));
    info.AddProperty(NInputSettingsPanel.PropertyName._controllerHeader, Variant.From<MegaRichTextLabel>(ref this._controllerHeader));
    info.AddProperty(NInputSettingsPanel.PropertyName._steamInputPrompt, Variant.From<MegaRichTextLabel>(ref this._steamInputPrompt));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NInputSettingsPanel.PropertyName._minPadding, ref variant1))
      this._minPadding = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NInputSettingsPanel.PropertyName._listeningEntry, ref variant2))
      this._listeningEntry = ((Variant) ref variant2).As<NInputSettingsEntry>();
    Variant variant3;
    if (info.TryGetProperty(NInputSettingsPanel.PropertyName._resetToDefaultButton, ref variant3))
      this._resetToDefaultButton = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NInputSettingsPanel.PropertyName._commandHeader, ref variant4))
      this._commandHeader = ((Variant) ref variant4).As<MegaRichTextLabel>();
    Variant variant5;
    if (info.TryGetProperty(NInputSettingsPanel.PropertyName._keyboardHeader, ref variant5))
      this._keyboardHeader = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NInputSettingsPanel.PropertyName._controllerHeader, ref variant6))
      this._controllerHeader = ((Variant) ref variant6).As<MegaRichTextLabel>();
    Variant variant7;
    if (!info.TryGetProperty(NInputSettingsPanel.PropertyName._steamInputPrompt, ref variant7))
      return;
    this._steamInputPrompt = ((Variant) ref variant7).As<MegaRichTextLabel>();
  }

  public new class MethodName : NSettingsPanel.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnViewportSizeChange = StringName.op_Implicit(nameof (OnViewportSizeChange));
    public new static readonly StringName OnVisibilityChange = StringName.op_Implicit(nameof (OnVisibilityChange));
    public static readonly StringName SetAsListeningEntry = StringName.op_Implicit(nameof (SetAsListeningEntry));
    public static readonly StringName _UnhandledKeyInput = StringName.op_Implicit(nameof (_UnhandledKeyInput));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
  }

  public new class PropertyName : NSettingsPanel.PropertyName
  {
    public new static readonly StringName _minPadding = StringName.op_Implicit(nameof (_minPadding));
    public static readonly StringName _listeningEntry = StringName.op_Implicit(nameof (_listeningEntry));
    public static readonly StringName _resetToDefaultButton = StringName.op_Implicit(nameof (_resetToDefaultButton));
    public static readonly StringName _commandHeader = StringName.op_Implicit(nameof (_commandHeader));
    public static readonly StringName _keyboardHeader = StringName.op_Implicit(nameof (_keyboardHeader));
    public static readonly StringName _controllerHeader = StringName.op_Implicit(nameof (_controllerHeader));
    public static readonly StringName _steamInputPrompt = StringName.op_Implicit(nameof (_steamInputPrompt));
  }

  public new class SignalName : NSettingsPanel.SignalName
  {
  }
}
