// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockCharacterScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Timeline;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockCharacterScreen.cs")]
public class NUnlockCharacterScreen : NUnlockScreen
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_character_screen");
  private MegaRichTextLabel _topLabel;
  private MegaRichTextLabel _bottomLabel;
  private Control _spineAnchor;
  private NCreatureVisuals _creatureVisuals;
  private GpuParticles2D _rareGlow;
  private EpochModel _epoch;
  private CharacterModel _character;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NUnlockCharacterScreen._scenePath);
    }
  }

  public static NUnlockCharacterScreen Create(EpochModel epoch, CharacterModel character)
  {
    NUnlockCharacterScreen nunlockCharacterScreen = PreloadManager.Cache.GetScene(NUnlockCharacterScreen._scenePath).Instantiate<NUnlockCharacterScreen>((PackedScene.GenEditState) 0L);
    nunlockCharacterScreen._character = character;
    nunlockCharacterScreen._epoch = epoch;
    return nunlockCharacterScreen;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._topLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TopLabel"));
    this._bottomLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._spineAnchor = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SpineAnchor"));
    this._rareGlow = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("%RareGlow"));
    this._topLabel.Text = new LocString("epochs", this._epoch.Id + ".unlock").GetFormattedText();
    this._bottomLabel.Text = new LocString("epochs", this._epoch.Id + ".unlockText").GetFormattedText();
    ((CanvasItem) this._topLabel).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._bottomLabel).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._spineAnchor).Modulate = StsColors.transparentBlack;
    this._creatureVisuals = this._character.CreateVisuals();
    ((Node) this._spineAnchor).AddChildSafely((Node) this._creatureVisuals);
  }

  public override void Open()
  {
    base.Open();
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock");
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._spineAnchor, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25).SetDelay(0.25);
    this._tween.TweenProperty((GodotObject) this._topLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetDelay(1.0);
    this._tween.TweenProperty((GodotObject) this._bottomLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetDelay(1.5);
    this._tween.TweenProperty((GodotObject) this._rareGlow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(1.0);
    this._creatureVisuals.SpineAnimation.AddAnimation("idle_loop");
    this._creatureVisuals.SpineAnimation.AddAnimation("attack", 0.5f, false);
    this._creatureVisuals.SpineAnimation.AddAnimation("idle_loop");
  }

  protected override void OnScreenPreClose()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this._rareGlow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
  }

  protected override void OnScreenClose() => NTimelineScreen.Instance.EnableInput();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NUnlockCharacterScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCharacterScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCharacterScreen.MethodName.OnScreenPreClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCharacterScreen.MethodName.OnScreenClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName.OnScreenPreClose) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenPreClose();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName.OnScreenClose) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnScreenClose();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName.Open) || StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName.OnScreenPreClose) || StringName.op_Equality(ref method, NUnlockCharacterScreen.MethodName.OnScreenClose) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._topLabel))
    {
      this._topLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._bottomLabel))
    {
      this._bottomLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._spineAnchor))
    {
      this._spineAnchor = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._creatureVisuals))
    {
      this._creatureVisuals = VariantUtils.ConvertTo<NCreatureVisuals>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._rareGlow))
    {
      this._rareGlow = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._topLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._topLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._bottomLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._bottomLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._spineAnchor))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._spineAnchor);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._creatureVisuals))
    {
      value = VariantUtils.CreateFrom<NCreatureVisuals>(ref this._creatureVisuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._rareGlow))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._rareGlow);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockCharacterScreen.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockCharacterScreen.PropertyName._topLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCharacterScreen.PropertyName._bottomLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCharacterScreen.PropertyName._spineAnchor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCharacterScreen.PropertyName._creatureVisuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCharacterScreen.PropertyName._rareGlow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCharacterScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockCharacterScreen.PropertyName._topLabel, Variant.From<MegaRichTextLabel>(ref this._topLabel));
    info.AddProperty(NUnlockCharacterScreen.PropertyName._bottomLabel, Variant.From<MegaRichTextLabel>(ref this._bottomLabel));
    info.AddProperty(NUnlockCharacterScreen.PropertyName._spineAnchor, Variant.From<Control>(ref this._spineAnchor));
    info.AddProperty(NUnlockCharacterScreen.PropertyName._creatureVisuals, Variant.From<NCreatureVisuals>(ref this._creatureVisuals));
    info.AddProperty(NUnlockCharacterScreen.PropertyName._rareGlow, Variant.From<GpuParticles2D>(ref this._rareGlow));
    info.AddProperty(NUnlockCharacterScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._topLabel, ref variant1))
      this._topLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._bottomLabel, ref variant2))
      this._bottomLabel = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._spineAnchor, ref variant3))
      this._spineAnchor = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._creatureVisuals, ref variant4))
      this._creatureVisuals = ((Variant) ref variant4).As<NCreatureVisuals>();
    Variant variant5;
    if (info.TryGetProperty(NUnlockCharacterScreen.PropertyName._rareGlow, ref variant5))
      this._rareGlow = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (!info.TryGetProperty(NUnlockCharacterScreen.PropertyName._tween, ref variant6))
      return;
    this._tween = ((Variant) ref variant6).As<Tween>();
  }

  public new class MethodName : NUnlockScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public new static readonly StringName OnScreenPreClose = StringName.op_Implicit(nameof (OnScreenPreClose));
    public new static readonly StringName OnScreenClose = StringName.op_Implicit(nameof (OnScreenClose));
  }

  public new class PropertyName : NUnlockScreen.PropertyName
  {
    public static readonly StringName _topLabel = StringName.op_Implicit(nameof (_topLabel));
    public static readonly StringName _bottomLabel = StringName.op_Implicit(nameof (_bottomLabel));
    public static readonly StringName _spineAnchor = StringName.op_Implicit(nameof (_spineAnchor));
    public static readonly StringName _creatureVisuals = StringName.op_Implicit(nameof (_creatureVisuals));
    public static readonly StringName _rareGlow = StringName.op_Implicit(nameof (_rareGlow));
    public new static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
