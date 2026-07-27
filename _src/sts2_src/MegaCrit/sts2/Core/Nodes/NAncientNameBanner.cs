// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NAncientNameBanner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.RichTextTags;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NAncientNameBanner.cs")]
public class NAncientNameBanner : Control
{
  private MegaRichTextLabel _titleLabel;
  private RichTextAncientBanner _ancientBannerEffect;
  private MegaLabel _epithetLabel;
  private static readonly string _path = SceneHelper.GetScenePath("ui/ancient_name_banner");
  private AncientEventModel _ancient;
  private Tween? _moveTween;
  private Tween? _tween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NAncientNameBanner._path);
    }
  }

  public static NAncientNameBanner? Create(AncientEventModel ancient)
  {
    if (TestMode.IsOn)
      return (NAncientNameBanner) null;
    NAncientNameBanner nancientNameBanner = PreloadManager.Cache.GetScene(NAncientNameBanner._path).Instantiate<NAncientNameBanner>((PackedScene.GenEditState) 0L);
    nancientNameBanner._ancient = ancient;
    return nancientNameBanner;
  }

  public override void _Ready()
  {
    this._titleLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Title"));
    string upper = this._ancient.Title.GetFormattedText().ToUpper();
    this._ancientBannerEffect = new RichTextAncientBanner();
    this._ancientBannerEffect.CenterCharacter = this.GetTextCenterGlyphIndex(upper, ((Control) this._titleLabel).GetThemeFont(ThemeConstants.RichTextLabel.NormalFont, StringName.op_Implicit("RichTextLabel")), ((Control) this._titleLabel).GetThemeFontSize(ThemeConstants.RichTextLabel.NormalFontSize, StringName.op_Implicit("RichTextLabel")));
    this._titleLabel.InstallEffect(Variant.op_Implicit((GodotObject) this._ancientBannerEffect));
    this._titleLabel.BbcodeEnabled = true;
    this._titleLabel.Text = $"[ancient_banner]{upper}[/ancient_banner]";
    this._epithetLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Epithet"));
    this._epithetLabel.SetTextAutoSize(this._ancient.Epithet.GetFormattedText());
    TaskHelper.RunSafely(this.AnimateVfx());
  }

  private async Task AnimateVfx()
  {
    ((Control) this._epithetLabel).Position = new Vector2(0.0f, 18f);
    ((CanvasItem) this._epithetLabel).Modulate = Colors.Transparent;
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-100f), 4.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 8L);
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateGlyphSpace)), Variant.op_Implicit(1f), Variant.op_Implicit(0.0f), 3.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateTransform)), Variant.op_Implicit(0.0f), Variant.op_Implicit(1f), 3.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L);
    this._tween.TweenProperty((GodotObject) this._epithetLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(42f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).SetDelay(1.0);
    this._tween.TweenProperty((GodotObject) this._epithetLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay(1.5);
    this._tween.Chain();
    this._tween.TweenInterval(1.5);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this._titleLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Red), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._titleLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this._epithetLabel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Red), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._epithetLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (!await this._tween.AwaitFinished((Node) this) || !((Node) this).IsValid())
      return;
    this._moveTween.Kill();
    this._moveTween = ((Node) this).CreateTween().SetParallel(true);
    this.Position = new Vector2(0.0f, -80f);
    ((CanvasItem) this._titleLabel).Modulate = Colors.White;
    this._titleLabel.HorizontalAlignment = (HorizontalAlignment) 0L;
    this._titleLabel.VerticalAlignment = (VerticalAlignment) 2L;
    ((Control) this._titleLabel).Position = Vector2.Zero;
    ((Control) this._titleLabel).AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.NormalFontSize, 54);
    ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor, Colors.Transparent);
    ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.FontShadowColor, Colors.Transparent);
    ((Control) this._titleLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.cream);
    this._epithetLabel.HorizontalAlignment = (HorizontalAlignment) 0L;
    this._epithetLabel.VerticalAlignment = (VerticalAlignment) 2L;
    ((CanvasItem) this._epithetLabel).Modulate = new Color(1f, 1f, 1f, 0.0f);
    ((Control) this._epithetLabel).AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, 18);
    ((Control) this._epithetLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, Colors.Transparent);
    ((Control) this._epithetLabel).AddThemeColorOverride(ThemeConstants.Label.FontShadowColor, Colors.Transparent);
    ((Control) this._epithetLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, StsColors.cream);
    this._moveTween.TweenProperty((GodotObject) this._epithetLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.5f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 8L);
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:x"), Variant.op_Implicit(48f), 2.0).SetEase((Tween.EaseType) 1L).From(Variant.op_Implicit(0)).SetTrans((Tween.TransitionType) 8L);
  }

  private void UpdateTransform(float obj) => this._ancientBannerEffect.Rotation = obj;

  private void UpdateGlyphSpace(float spacing)
  {
    this._ancientBannerEffect.Spacing = spacing * 1000f;
  }

  private float GetTextCenterGlyphIndex(string text, Font font, int fontSize)
  {
    using (TextParagraph textParagraph = new TextParagraph())
    {
      textParagraph.AddString(text, font, fontSize, "", new Variant());
      float num1 = 0.0f;
      Array<Dictionary> glyphs = TextServerManager.Singleton.GetPrimaryInterface().ShapedTextGetGlyphs(textParagraph.GetLineRid(0));
      foreach (IReadOnlyDictionary<Variant, Variant> readOnlyDictionary in glyphs)
      {
        Variant valueOrDefault = CollectionExtensions.GetValueOrDefault<Variant, Variant>(readOnlyDictionary, Variant.op_Implicit("advance"));
        float num2 = ((Variant) ref valueOrDefault).AsSingle();
        num1 += num2;
      }
      float num3 = 0.0f;
      int num4 = 0;
      foreach (IReadOnlyDictionary<Variant, Variant> readOnlyDictionary in glyphs)
      {
        Variant valueOrDefault = CollectionExtensions.GetValueOrDefault<Variant, Variant>(readOnlyDictionary, Variant.op_Implicit("advance"));
        float num5 = ((Variant) ref valueOrDefault).AsSingle();
        num3 += num5;
        if ((double) num3 > (double) num1 * 0.5)
          return (float) num4 + (float) ((double) num1 * 0.5 - ((double) num3 - (double) num5)) / num5;
        ++num4;
      }
      return 0.0f;
    }
  }

  public override void _ExitTree()
  {
    this._moveTween?.Kill();
    this._tween?.Kill();
    ((Control) this._titleLabel).RemoveThemeFontSizeOverride(ThemeConstants.RichTextLabel.NormalFontSize);
    ((Control) this._titleLabel).RemoveThemeColorOverride(ThemeConstants.RichTextLabel.FontOutlineColor);
    ((Control) this._titleLabel).RemoveThemeColorOverride(ThemeConstants.RichTextLabel.FontShadowColor);
    ((Control) this._titleLabel).RemoveThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor);
    ((Control) this._epithetLabel).RemoveThemeFontSizeOverride(ThemeConstants.Label.FontSize);
    ((Control) this._epithetLabel).RemoveThemeColorOverride(ThemeConstants.Label.FontOutlineColor);
    ((Control) this._epithetLabel).RemoveThemeColorOverride(ThemeConstants.Label.FontShadowColor);
    ((Control) this._epithetLabel).RemoveThemeColorOverride(ThemeConstants.Label.FontColor);
    ((Node) this)._ExitTree();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NAncientNameBanner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAncientNameBanner.MethodName.UpdateTransform, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientNameBanner.MethodName.UpdateGlyphSpace, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("spacing"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientNameBanner.MethodName.GetTextCenterGlyphIndex, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("font"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Font"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("fontSize"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAncientNameBanner.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAncientNameBanner.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientNameBanner.MethodName.UpdateTransform) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateTransform(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientNameBanner.MethodName.UpdateGlyphSpace) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateGlyphSpace(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAncientNameBanner.MethodName.GetTextCenterGlyphIndex) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      float centerGlyphIndex = this.GetTextCenterGlyphIndex(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Font>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<float>(ref centerGlyphIndex);
      return true;
    }
    if (!StringName.op_Equality(ref method, NAncientNameBanner.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAncientNameBanner.MethodName._Ready) || StringName.op_Equality(ref method, NAncientNameBanner.MethodName.UpdateTransform) || StringName.op_Equality(ref method, NAncientNameBanner.MethodName.UpdateGlyphSpace) || StringName.op_Equality(ref method, NAncientNameBanner.MethodName.GetTextCenterGlyphIndex) || StringName.op_Equality(ref method, NAncientNameBanner.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._titleLabel))
    {
      this._titleLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._ancientBannerEffect))
    {
      this._ancientBannerEffect = VariantUtils.ConvertTo<RichTextAncientBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._epithetLabel))
    {
      this._epithetLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._moveTween))
    {
      this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._titleLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._titleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._ancientBannerEffect))
    {
      value = VariantUtils.CreateFrom<RichTextAncientBanner>(ref this._ancientBannerEffect);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._epithetLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._epithetLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._moveTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAncientNameBanner.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAncientNameBanner.PropertyName._titleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientNameBanner.PropertyName._ancientBannerEffect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientNameBanner.PropertyName._epithetLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientNameBanner.PropertyName._moveTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAncientNameBanner.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAncientNameBanner.PropertyName._titleLabel, Variant.From<MegaRichTextLabel>(ref this._titleLabel));
    info.AddProperty(NAncientNameBanner.PropertyName._ancientBannerEffect, Variant.From<RichTextAncientBanner>(ref this._ancientBannerEffect));
    info.AddProperty(NAncientNameBanner.PropertyName._epithetLabel, Variant.From<MegaLabel>(ref this._epithetLabel));
    info.AddProperty(NAncientNameBanner.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
    info.AddProperty(NAncientNameBanner.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAncientNameBanner.PropertyName._titleLabel, ref variant1))
      this._titleLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NAncientNameBanner.PropertyName._ancientBannerEffect, ref variant2))
      this._ancientBannerEffect = ((Variant) ref variant2).As<RichTextAncientBanner>();
    Variant variant3;
    if (info.TryGetProperty(NAncientNameBanner.PropertyName._epithetLabel, ref variant3))
      this._epithetLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NAncientNameBanner.PropertyName._moveTween, ref variant4))
      this._moveTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (!info.TryGetProperty(NAncientNameBanner.PropertyName._tween, ref variant5))
      return;
    this._tween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName UpdateTransform = StringName.op_Implicit(nameof (UpdateTransform));
    public static readonly StringName UpdateGlyphSpace = StringName.op_Implicit(nameof (UpdateGlyphSpace));
    public static readonly StringName GetTextCenterGlyphIndex = StringName.op_Implicit(nameof (GetTextCenterGlyphIndex));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _titleLabel = StringName.op_Implicit(nameof (_titleLabel));
    public static readonly StringName _ancientBannerEffect = StringName.op_Implicit(nameof (_ancientBannerEffect));
    public static readonly StringName _epithetLabel = StringName.op_Implicit(nameof (_epithetLabel));
    public static readonly StringName _moveTween = StringName.op_Implicit(nameof (_moveTween));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
