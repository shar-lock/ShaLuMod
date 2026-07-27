// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NAncientDialogueLine
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NAncientDialogueLine.cs")]
public class NAncientDialogueLine : NButton
{
  private const string _scenePath = "res://scenes/events/ancient_dialogue_line.tscn";
  private AncientDialogueLine _line;
  private AncientEventModel _ancient;
  private CharacterModel _character;
  private Control _iconNode;
  private Tween? _tween;
  private float _targetAlpha = 1f;

  protected override string? HoveredSfx => (string) null;

  protected override string? ClickedSfx => (string) null;

  public static NAncientDialogueLine Create(
    AncientDialogueLine line,
    AncientEventModel ancient,
    CharacterModel character)
  {
    NAncientDialogueLine nancientDialogueLine = PreloadManager.Cache.GetScene("res://scenes/events/ancient_dialogue_line.tscn").Instantiate<NAncientDialogueLine>((PackedScene.GenEditState) 0L);
    nancientDialogueLine._line = line;
    nancientDialogueLine._ancient = ancient;
    nancientDialogueLine._character = character;
    return nancientDialogueLine;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    LocString lineText = this._line.LineText;
    this._character.AddDetailsTo(lineText);
    this._ancient.DynamicVars.AddTo(lineText);
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text")).Text = lineText.GetFormattedText();
    switch (this._line.Speaker)
    {
      case AncientDialogueSpeaker.Ancient:
        this.SetAncientAsSpeaker();
        break;
      case AncientDialogueSpeaker.Character:
        this.SetCharacterAsSpeaker();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
  }

  public void PlaySfx() => SfxCmd.Play(this._line.GetSfxOrFallbackPath());

  private void SetAncientAsSpeaker()
  {
    Control node1 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AncientIcon"));
    ((Node) node1).GetNode<TextureRect>(NodePath.op_Implicit("Icon")).Texture = this._ancient.RunHistoryIcon;
    ((Node) node1).GetNode<TextureRect>(NodePath.op_Implicit("Icon/Outline")).Texture = this._ancient.RunHistoryIconOutline;
    this._iconNode = node1;
    Control node2 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DialogueTailLeft"));
    ((CanvasItem) node2).Visible = true;
    ((Control) ((Node) this).GetNode<MarginContainer>(NodePath.op_Implicit("%TextContainer"))).AddThemeConstantOverride(ThemeConstants.MarginContainer.MarginLeft, 48 /*0x30*/);
    ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Bubble"))).SelfModulate = this._ancient.DialogueColor;
    ((CanvasItem) node2).SelfModulate = this._ancient.DialogueColor;
  }

  private void SetCharacterAsSpeaker()
  {
    Control node1 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterIcon"));
    ((Node) node1).GetNode<TextureRect>(NodePath.op_Implicit("Icon")).Texture = this._character.IconTexture;
    ((Node) node1).GetNode<TextureRect>(NodePath.op_Implicit("Icon/Outline")).Texture = this._character.IconOutlineTexture;
    this._iconNode = node1;
    Control node2 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DialogueTailRight"));
    ((CanvasItem) node2).Visible = true;
    ((Control) ((Node) this).GetNode<MarginContainer>(NodePath.op_Implicit("%TextContainer"))).AddThemeConstantOverride(ThemeConstants.MarginContainer.MarginRight, 46);
    ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Bubble"))).SelfModulate = this._character.DialogueColor;
    ((CanvasItem) node2).SelfModulate = this._character.DialogueColor;
  }

  public void SetSpeakerIconVisible() => ((CanvasItem) this._iconNode).Visible = true;

  public void SetTransparency(float alpha)
  {
    this._targetAlpha = alpha;
    ((CanvasItem) this).Modulate = new Color(1f, 1f, 1f, alpha);
  }

  public void FadeInStaleDialogue() => this.OnAnimInSetVisible();

  public void FadeOutStaleDialogue()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.1);
  }

  public void OnAnimInSetVisible()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.1);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NAncientDialogueLine.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.PlaySfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.SetAncientAsSpeaker, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.SetCharacterAsSpeaker, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.SetSpeakerIconVisible, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.SetTransparency, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("alpha"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.FadeInStaleDialogue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.FadeOutStaleDialogue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientDialogueLine.MethodName.OnAnimInSetVisible, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.PlaySfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlaySfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetAncientAsSpeaker) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetAncientAsSpeaker();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetCharacterAsSpeaker) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetCharacterAsSpeaker();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetSpeakerIconVisible) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetSpeakerIconVisible();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetTransparency) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTransparency(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.FadeInStaleDialogue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FadeInStaleDialogue();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.FadeOutStaleDialogue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FadeOutStaleDialogue();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.OnAnimInSetVisible) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnAnimInSetVisible();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAncientDialogueLine.MethodName._Ready) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.PlaySfx) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetAncientAsSpeaker) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetCharacterAsSpeaker) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetSpeakerIconVisible) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.SetTransparency) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.FadeInStaleDialogue) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.FadeOutStaleDialogue) || StringName.op_Equality(ref method, NAncientDialogueLine.MethodName.OnAnimInSetVisible) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName._iconNode))
    {
      this._iconNode = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName._targetAlpha))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._targetAlpha = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName.HoveredSfx))
    {
      ref godot_variant local = ref value;
      string hoveredSfx = this.HoveredSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref hoveredSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName.ClickedSfx))
    {
      ref godot_variant local = ref value;
      string clickedSfx = this.ClickedSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref clickedSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName._iconNode))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._iconNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientDialogueLine.PropertyName._targetAlpha))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._targetAlpha);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NAncientDialogueLine.PropertyName.HoveredSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NAncientDialogueLine.PropertyName.ClickedSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientDialogueLine.PropertyName._iconNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientDialogueLine.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NAncientDialogueLine.PropertyName._targetAlpha, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAncientDialogueLine.PropertyName._iconNode, Variant.From<Control>(ref this._iconNode));
    info.AddProperty(NAncientDialogueLine.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NAncientDialogueLine.PropertyName._targetAlpha, Variant.From<float>(ref this._targetAlpha));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAncientDialogueLine.PropertyName._iconNode, ref variant1))
      this._iconNode = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NAncientDialogueLine.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (!info.TryGetProperty(NAncientDialogueLine.PropertyName._targetAlpha, ref variant3))
      return;
    this._targetAlpha = ((Variant) ref variant3).As<float>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName PlaySfx = StringName.op_Implicit(nameof (PlaySfx));
    public static readonly StringName SetAncientAsSpeaker = StringName.op_Implicit(nameof (SetAncientAsSpeaker));
    public static readonly StringName SetCharacterAsSpeaker = StringName.op_Implicit(nameof (SetCharacterAsSpeaker));
    public static readonly StringName SetSpeakerIconVisible = StringName.op_Implicit(nameof (SetSpeakerIconVisible));
    public static readonly StringName SetTransparency = StringName.op_Implicit(nameof (SetTransparency));
    public static readonly StringName FadeInStaleDialogue = StringName.op_Implicit(nameof (FadeInStaleDialogue));
    public static readonly StringName FadeOutStaleDialogue = StringName.op_Implicit(nameof (FadeOutStaleDialogue));
    public static readonly StringName OnAnimInSetVisible = StringName.op_Implicit(nameof (OnAnimInSetVisible));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName HoveredSfx = StringName.op_Implicit(nameof (HoveredSfx));
    public new static readonly StringName ClickedSfx = StringName.op_Implicit(nameof (ClickedSfx));
    public static readonly StringName _iconNode = StringName.op_Implicit(nameof (_iconNode));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _targetAlpha = StringName.op_Implicit(nameof (_targetAlpha));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
