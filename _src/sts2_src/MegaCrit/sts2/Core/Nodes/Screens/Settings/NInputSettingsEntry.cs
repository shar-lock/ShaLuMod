// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NInputSettingsEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NInputSettingsEntry.cs")]
public class NInputSettingsEntry : NButton
{
  private static readonly Dictionary<StringName, string> _commandToLocTitle = new Dictionary<StringName, string>()
  {
    {
      MegaInput.accept,
      "endTurn"
    },
    {
      MegaInput.select,
      "confirmCard"
    },
    {
      MegaInput.viewDiscardPile,
      "viewDiscard"
    },
    {
      MegaInput.viewDrawPile,
      "viewDraw"
    },
    {
      MegaInput.viewDeckAndTabLeft,
      "viewDeck"
    },
    {
      MegaInput.viewExhaustPileAndTabRight,
      "viewExhaust"
    },
    {
      MegaInput.viewMap,
      "viewMap"
    },
    {
      MegaInput.cancel,
      "cancel"
    },
    {
      MegaInput.peek,
      "peek"
    },
    {
      MegaInput.up,
      "up"
    },
    {
      MegaInput.topPanel,
      "topPanel"
    },
    {
      MegaInput.down,
      "down"
    },
    {
      MegaInput.left,
      "left"
    },
    {
      MegaInput.right,
      "right"
    },
    {
      MegaInput.selectCard1,
      "selectCard1"
    },
    {
      MegaInput.selectCard2,
      "selectCard2"
    },
    {
      MegaInput.selectCard3,
      "selectCard3"
    },
    {
      MegaInput.selectCard4,
      "selectCard4"
    },
    {
      MegaInput.selectCard5,
      "selectCard5"
    },
    {
      MegaInput.selectCard6,
      "selectCard6"
    },
    {
      MegaInput.selectCard7,
      "selectCard7"
    },
    {
      MegaInput.selectCard8,
      "selectCard8"
    },
    {
      MegaInput.selectCard9,
      "selectCard9"
    },
    {
      MegaInput.selectCard10,
      "selectCard10"
    },
    {
      MegaInput.releaseCard,
      "releaseCard"
    }
  };
  private const string _scenePath = "res://scenes/screens/settings_screen/input_settings_entry.tscn";
  private Control _bg;
  private MegaRichTextLabel _inputLabel;
  private MegaRichTextLabel _keyBindingLabel;
  private TextureRect _controllerBindingIcon;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/screens/settings_screen/input_settings_entry.tscn");
    }
  }

  public StringName InputName { get; private set; }

  public static NInputSettingsEntry Create(string commandName)
  {
    NInputSettingsEntry ninputSettingsEntry = ResourceLoader.Load<PackedScene>("res://scenes/screens/settings_screen/input_settings_entry.tscn", (string) null, (ResourceLoader.CacheMode) 1L).Instantiate<NInputSettingsEntry>((PackedScene.GenEditState) 0L);
    ninputSettingsEntry.InputName = StringName.op_Implicit(commandName);
    return ninputSettingsEntry;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._inputLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%InputLabel"));
    this._keyBindingLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%KeyBindingInputLabel"));
    this._controllerBindingIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%ControllerBindingIcon"));
    this._bg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Bg"));
    this._inputLabel.Text = new LocString("settings_ui", "INPUT_SETTINGS.INPUT_TITLE." + NInputSettingsEntry._commandToLocTitle[this.InputName]).GetFormattedText();
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateInput)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateInput)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateInput)), 0U);
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.UpdateInput)), 0U);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    ((GodotObject) NInputManager.Instance).Disconnect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateInput)));
    ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateInput)));
    ((GodotObject) NControllerManager.Instance).Disconnect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateInput)));
    ((GodotObject) this).Disconnect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.UpdateInput)));
  }

  private void UpdateInput()
  {
    if (!((CanvasItem) this).IsVisibleInTree())
      return;
    if (NInputManager.remappableKeyboardInputs.Contains<StringName>(this.InputName))
    {
      Key shortcutKey = NInputManager.Instance.GetShortcutKey(this.InputName);
      this._keyBindingLabel.Text = shortcutKey != null ? shortcutKey.ToString() : "";
    }
    else
      this._keyBindingLabel.Text = "";
    if (NInputManager.remappableControllerInputs.Contains<StringName>(this.InputName))
      this._controllerBindingIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(this.InputName));
    ((CanvasItem) this._controllerBindingIcon).Modulate = NControllerManager.Instance.ShouldAllowControllerRebinding ? Colors.White : new Color(1f, 1f, 1f, 0.15f);
  }

  protected override void OnFocus() => ((CanvasItem) this._bg).Visible = true;

  protected override void OnUnfocus() => ((CanvasItem) this._bg).Visible = false;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NInputSettingsEntry.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("commandName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NInputSettingsEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsEntry.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsEntry.MethodName.UpdateInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsEntry.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInputSettingsEntry.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NInputSettingsEntry ninputSettingsEntry = NInputSettingsEntry.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NInputSettingsEntry>(ref ninputSettingsEntry);
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsEntry.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsEntry.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.UpdateInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateInput();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NInputSettingsEntry ninputSettingsEntry = NInputSettingsEntry.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NInputSettingsEntry>(ref ninputSettingsEntry);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.Create) || StringName.op_Equality(ref method, NInputSettingsEntry.MethodName._Ready) || StringName.op_Equality(ref method, NInputSettingsEntry.MethodName._ExitTree) || StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.UpdateInput) || StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.OnFocus) || StringName.op_Equality(ref method, NInputSettingsEntry.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName.InputName))
    {
      this.InputName = VariantUtils.ConvertTo<StringName>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._bg))
    {
      this._bg = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._inputLabel))
    {
      this._inputLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._keyBindingLabel))
    {
      this._keyBindingLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._controllerBindingIcon))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._controllerBindingIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName.InputName))
    {
      ref godot_variant local = ref value;
      StringName inputName = this.InputName;
      godot_variant from = VariantUtils.CreateFrom<StringName>(ref inputName);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._bg))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bg);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._inputLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._inputLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._keyBindingLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._keyBindingLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInputSettingsEntry.PropertyName._controllerBindingIcon))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._controllerBindingIcon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 21L, NInputSettingsEntry.PropertyName.InputName, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsEntry.PropertyName._bg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsEntry.PropertyName._inputLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsEntry.PropertyName._keyBindingLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInputSettingsEntry.PropertyName._controllerBindingIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName inputName1 = NInputSettingsEntry.PropertyName.InputName;
    StringName inputName2 = this.InputName;
    Variant variant = Variant.From<StringName>(ref inputName2);
    serializationInfo.AddProperty(inputName1, variant);
    info.AddProperty(NInputSettingsEntry.PropertyName._bg, Variant.From<Control>(ref this._bg));
    info.AddProperty(NInputSettingsEntry.PropertyName._inputLabel, Variant.From<MegaRichTextLabel>(ref this._inputLabel));
    info.AddProperty(NInputSettingsEntry.PropertyName._keyBindingLabel, Variant.From<MegaRichTextLabel>(ref this._keyBindingLabel));
    info.AddProperty(NInputSettingsEntry.PropertyName._controllerBindingIcon, Variant.From<TextureRect>(ref this._controllerBindingIcon));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NInputSettingsEntry.PropertyName.InputName, ref variant1))
      this.InputName = ((Variant) ref variant1).As<StringName>();
    Variant variant2;
    if (info.TryGetProperty(NInputSettingsEntry.PropertyName._bg, ref variant2))
      this._bg = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NInputSettingsEntry.PropertyName._inputLabel, ref variant3))
      this._inputLabel = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NInputSettingsEntry.PropertyName._keyBindingLabel, ref variant4))
      this._keyBindingLabel = ((Variant) ref variant4).As<MegaRichTextLabel>();
    Variant variant5;
    if (!info.TryGetProperty(NInputSettingsEntry.PropertyName._controllerBindingIcon, ref variant5))
      return;
    this._controllerBindingIcon = ((Variant) ref variant5).As<TextureRect>();
  }

  public new class MethodName : NButton.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateInput = StringName.op_Implicit(nameof (UpdateInput));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName InputName = StringName.op_Implicit(nameof (InputName));
    public static readonly StringName _bg = StringName.op_Implicit(nameof (_bg));
    public static readonly StringName _inputLabel = StringName.op_Implicit(nameof (_inputLabel));
    public static readonly StringName _keyBindingLabel = StringName.op_Implicit(nameof (_keyBindingLabel));
    public static readonly StringName _controllerBindingIcon = StringName.op_Implicit(nameof (_controllerBindingIcon));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
