// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryCharacterFilter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryCharacterFilter.cs")]
public class NBestiaryCharacterFilter : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_character_filter");
  private bool _isSelected;
  private bool _isLocked;
  public CharacterModel? character;
  private TextureRect _image;
  private ShaderMaterial _hsv;
  private NSelectionReticle _controllerSelectionReticle;
  private Tween? _tween;
  private const float _focusedMultiplier = 1.2f;
  private const float _pressDownMultiplier = 0.8f;
  private static readonly Vector2 _enabledScale = Vector2.op_Multiply(Vector2.One, 1.2f);
  private static readonly Vector2 _disabledScale = Vector2.op_Multiply(Vector2.One, 0.95f);
  public int kills;
  public int deaths;
  private 
  #nullable disable
  NBestiaryCharacterFilter.ToggledEventHandler backing_Toggled;

  public int Total => this.kills + this.deaths;

  private double WinRateValue
  {
    get => this.Total <= 0 ? 0.0 : (double) this.kills / (double) this.Total * 100.0;
  }

  public 
  #nullable enable
  string WinRate
  {
    get
    {
      if (this.WinRateValue % 1.0 != 0.0)
        return $"{this.WinRateValue:F1}";
      return $"{this.WinRateValue:F0}";
    }
  }

  public string BestiarySeenQuote
  {
    get
    {
      return this.character != null ? this.character.BestiarySeenQuote.GetFormattedText() : string.Empty;
    }
  }

  public LocString? BestiaryKillQuote => this.character?.BestiaryKillQuote;

  public bool IsSelected
  {
    get => this._isSelected;
    set
    {
      this._isSelected = value;
      this.OnToggle();
    }
  }

  public bool IsLocked
  {
    get => this._isLocked;
    set
    {
      this._isLocked = value;
      this.SetLockedState();
    }
  }

  public static NBestiaryCharacterFilter Create(CharacterModel? character)
  {
    NBestiaryCharacterFilter nbestiaryCharacterFilter = PreloadManager.Cache.GetAsset<PackedScene>(NBestiaryCharacterFilter._scenePath).Instantiate<NBestiaryCharacterFilter>((PackedScene.GenEditState) 0L);
    nbestiaryCharacterFilter.character = character;
    return nbestiaryCharacterFilter;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Image"));
    if (this.character != null)
    {
      this._image.Texture = this.character.IconTexture;
      ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Shadow")).Texture = this.character.IconTexture;
    }
    this._controllerSelectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).GetMaterial();
  }

  private void OnToggle()
  {
    this._tween?.Kill();
    this._hsv.SetShaderParameter(NBestiaryCharacterFilter._s, Variant.op_Implicit(this._isSelected ? 1.1f : 0.5f));
    this._hsv.SetShaderParameter(NBestiaryCharacterFilter._v, Variant.op_Implicit(this._isSelected ? 1.1f : 0.75f));
    if (!this._isSelected)
    {
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(NBestiaryCharacterFilter._disabledScale), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    else
    {
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(NBestiaryCharacterFilter._enabledScale), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    }
  }

  private void SetLockedState()
  {
    ((CanvasItem) this._image).SelfModulate = this._isLocked ? new Color(1f, 1f, 1f, 0.2f) : Colors.White;
  }

  protected override void OnRelease()
  {
    if (this._isSelected || this._isLocked)
      return;
    base.OnRelease();
    this.IsSelected = !this.IsSelected;
    ((GodotObject) this).EmitSignal(NBestiaryCharacterFilter.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  protected override void OnFocus()
  {
    if (this._isSelected || this._isLocked)
      return;
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._isSelected ? NBestiaryCharacterFilter._enabledScale : NBestiaryCharacterFilter._disabledScale, 1.2f)), 0.05);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._controllerSelectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    if (this._isSelected || this._isLocked)
      return;
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._isSelected ? NBestiaryCharacterFilter._enabledScale : NBestiaryCharacterFilter._disabledScale), 0.3);
    this._controllerSelectionReticle.OnDeselect();
  }

  protected override void OnPress()
  {
    if (this._isSelected || this._isLocked)
      return;
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._isSelected ? NBestiaryCharacterFilter._enabledScale : NBestiaryCharacterFilter._disabledScale, 0.8f)), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void Deselect() => this.IsSelected = false;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NBestiaryCharacterFilter.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.OnToggle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.SetLockedState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryCharacterFilter.MethodName.Deselect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnToggle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.SetLockedState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetLockedState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.Deselect) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Deselect();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnToggle) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.SetLockedState) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnRelease) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnFocus) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.OnPress) || StringName.op_Equality(ref method, NBestiaryCharacterFilter.MethodName.Deselect) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.IsSelected))
    {
      this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.IsLocked))
    {
      this.IsLocked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._isSelected))
    {
      this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._isLocked))
    {
      this._isLocked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle))
    {
      this._controllerSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.kills))
    {
      this.kills = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.deaths))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.deaths = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.Total))
    {
      ref godot_variant local = ref value;
      int total = this.Total;
      godot_variant from = VariantUtils.CreateFrom<int>(ref total);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.WinRateValue))
    {
      ref godot_variant local = ref value;
      double winRateValue = this.WinRateValue;
      godot_variant from = VariantUtils.CreateFrom<double>(ref winRateValue);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.WinRate))
    {
      ref godot_variant local = ref value;
      string winRate = this.WinRate;
      godot_variant from = VariantUtils.CreateFrom<string>(ref winRate);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.BestiarySeenQuote))
    {
      ref godot_variant local = ref value;
      string bestiarySeenQuote = this.BestiarySeenQuote;
      godot_variant from = VariantUtils.CreateFrom<string>(ref bestiarySeenQuote);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.IsSelected))
    {
      ref godot_variant local = ref value;
      bool isSelected = this.IsSelected;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSelected);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.IsLocked))
    {
      ref godot_variant local = ref value;
      bool isLocked = this.IsLocked;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isLocked);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._isSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._isLocked))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isLocked);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerSelectionReticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.kills))
    {
      value = VariantUtils.CreateFrom<int>(ref this.kills);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryCharacterFilter.PropertyName.deaths))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this.deaths);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NBestiaryCharacterFilter.PropertyName._isSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiaryCharacterFilter.PropertyName._isLocked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryCharacterFilter.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryCharacterFilter.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryCharacterFilter.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBestiaryCharacterFilter.PropertyName.kills, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBestiaryCharacterFilter.PropertyName.deaths, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NBestiaryCharacterFilter.PropertyName.Total, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NBestiaryCharacterFilter.PropertyName.WinRateValue, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NBestiaryCharacterFilter.PropertyName.WinRate, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NBestiaryCharacterFilter.PropertyName.BestiarySeenQuote, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiaryCharacterFilter.PropertyName.IsSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiaryCharacterFilter.PropertyName.IsLocked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName isSelected1 = NBestiaryCharacterFilter.PropertyName.IsSelected;
    bool isSelected2 = this.IsSelected;
    Variant variant1 = Variant.From<bool>(ref isSelected2);
    serializationInfo1.AddProperty(isSelected1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName isLocked1 = NBestiaryCharacterFilter.PropertyName.IsLocked;
    bool isLocked2 = this.IsLocked;
    Variant variant2 = Variant.From<bool>(ref isLocked2);
    serializationInfo2.AddProperty(isLocked1, variant2);
    info.AddProperty(NBestiaryCharacterFilter.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName._isLocked, Variant.From<bool>(ref this._isLocked));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle, Variant.From<NSelectionReticle>(ref this._controllerSelectionReticle));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName.kills, Variant.From<int>(ref this.kills));
    info.AddProperty(NBestiaryCharacterFilter.PropertyName.deaths, Variant.From<int>(ref this.deaths));
    info.AddSignalEventDelegate(NBestiaryCharacterFilter.SignalName.Toggled, (Delegate) this.backing_Toggled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.IsSelected, ref variant1))
      this.IsSelected = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.IsLocked, ref variant2))
      this.IsLocked = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._isSelected, ref variant3))
      this._isSelected = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._isLocked, ref variant4))
      this._isLocked = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._image, ref variant5))
      this._image = ((Variant) ref variant5).As<TextureRect>();
    Variant variant6;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._hsv, ref variant6))
      this._hsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._controllerSelectionReticle, ref variant7))
      this._controllerSelectionReticle = ((Variant) ref variant7).As<NSelectionReticle>();
    Variant variant8;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName._tween, ref variant8))
      this._tween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.kills, ref variant9))
      this.kills = ((Variant) ref variant9).As<int>();
    Variant variant10;
    if (info.TryGetProperty(NBestiaryCharacterFilter.PropertyName.deaths, ref variant10))
      this.deaths = ((Variant) ref variant10).As<int>();
    NBestiaryCharacterFilter.ToggledEventHandler toggledEventHandler;
    if (!info.TryGetSignalEventDelegate<NBestiaryCharacterFilter.ToggledEventHandler>(NBestiaryCharacterFilter.SignalName.Toggled, ref toggledEventHandler))
      return;
    this.backing_Toggled = toggledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NBestiaryCharacterFilter.SignalName.Toggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("filter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NBestiaryCharacterFilter.ToggledEventHandler Toggled
  {
    add => this.backing_Toggled += value;
    remove => this.backing_Toggled -= value;
  }

  protected void EmitSignalToggled(NBestiaryCharacterFilter filter)
  {
    ((GodotObject) this).EmitSignal(NBestiaryCharacterFilter.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) filter)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NBestiaryCharacterFilter.SignalName.Toggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NBestiaryCharacterFilter.ToggledEventHandler backingToggled = this.backing_Toggled;
      if (backingToggled == null)
        return;
      backingToggled(VariantUtils.ConvertTo<NBestiaryCharacterFilter>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NBestiaryCharacterFilter.SignalName.Toggled) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ToggledEventHandler(
  #nullable enable
  NBestiaryCharacterFilter filter);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnToggle = StringName.op_Implicit(nameof (OnToggle));
    public static readonly StringName SetLockedState = StringName.op_Implicit(nameof (SetLockedState));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName Deselect = StringName.op_Implicit(nameof (Deselect));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Total = StringName.op_Implicit(nameof (Total));
    public static readonly StringName WinRateValue = StringName.op_Implicit(nameof (WinRateValue));
    public static readonly StringName WinRate = StringName.op_Implicit(nameof (WinRate));
    public static readonly StringName BestiarySeenQuote = StringName.op_Implicit(nameof (BestiarySeenQuote));
    public static readonly StringName IsSelected = StringName.op_Implicit(nameof (IsSelected));
    public static readonly StringName IsLocked = StringName.op_Implicit(nameof (IsLocked));
    public static readonly StringName _isSelected = StringName.op_Implicit(nameof (_isSelected));
    public static readonly StringName _isLocked = StringName.op_Implicit(nameof (_isLocked));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _controllerSelectionReticle = StringName.op_Implicit(nameof (_controllerSelectionReticle));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName kills = StringName.op_Implicit(nameof (kills));
    public static readonly StringName deaths = StringName.op_Implicit(nameof (deaths));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Toggled = StringName.op_Implicit(nameof (Toggled));
  }
}
