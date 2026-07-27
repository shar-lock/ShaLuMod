// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.addons.mega_text.MegaRichTextLabel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Text;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.RichTextTags;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.addons.mega_text;

[Tool]
[ScriptPath("res://addons/mega_text/MegaRichTextLabel.cs")]
public class MegaRichTextLabel : RichTextLabel
{
  private static TextParagraph? _cachedParagraph = new TextParagraph();
  private const float _sizeComparisonEpsilon = 0.01f;
  private bool _isAutoSizeEnabled = true;
  private int _minFontSize = 8;
  private int _maxFontSize = 100;
  private int _lastSetSize;
  private bool _isVerticallyBound = true;
  private bool _isHorizontallyBound;
  private bool _needsResize = true;
  private bool _effectsInstalled;
  private Vector2 _lastAdjustedSize;
  private static readonly AbstractMegaRichTextEffect[] _textEffects = new AbstractMegaRichTextEffect[14]
  {
    (AbstractMegaRichTextEffect) new RichTextAqua(),
    (AbstractMegaRichTextEffect) new RichTextBlue(),
    (AbstractMegaRichTextEffect) new RichTextFadeIn(),
    (AbstractMegaRichTextEffect) new RichTextFlyIn(),
    (AbstractMegaRichTextEffect) new RichTextGold(),
    (AbstractMegaRichTextEffect) new RichTextGreen(),
    (AbstractMegaRichTextEffect) new RichTextJitter(),
    (AbstractMegaRichTextEffect) new RichTextOrange(),
    (AbstractMegaRichTextEffect) new RichTextPink(),
    (AbstractMegaRichTextEffect) new RichTextPurple(),
    (AbstractMegaRichTextEffect) new RichTextRed(),
    (AbstractMegaRichTextEffect) new RichTextScramble(),
    (AbstractMegaRichTextEffect) new RichTextSine(),
    (AbstractMegaRichTextEffect) new RichTextThinkyDots()
  };
  private bool _isAutoSizing;

  public static void DisposeCachedParagraph()
  {
    ((GodotObject) MegaRichTextLabel._cachedParagraph)?.Dispose();
    MegaRichTextLabel._cachedParagraph = (TextParagraph) null;
  }

  [Export]
  public bool AutoSizeEnabled
  {
    get => this._isAutoSizeEnabled;
    set
    {
      if (value && this.FitContent)
      {
        GD.PushWarning("Auto Size is not compatible with Fit Content, disabling Auto Size...");
        this._isAutoSizeEnabled = false;
      }
      else
      {
        if (this.AutoSizeEnabled == value)
          return;
        this._isAutoSizeEnabled = value;
        if (!Engine.IsEditorHint())
          return;
        this.AdjustFontSize();
      }
    }
  }

  [Export]
  public int MinFontSize
  {
    get => this._minFontSize;
    set
    {
      if (this._minFontSize == value)
        return;
      this._minFontSize = value;
      if (!Engine.IsEditorHint())
        return;
      this.AdjustFontSize();
    }
  }

  [Export]
  public int MaxFontSize
  {
    get => this._maxFontSize;
    set
    {
      if (this._maxFontSize == value)
        return;
      this._maxFontSize = value;
      if (!Engine.IsEditorHint())
        return;
      this.AdjustFontSize();
    }
  }

  [Export]
  public bool IsVerticallyBound
  {
    get => this._isVerticallyBound;
    set
    {
      this._isVerticallyBound = value;
      if (!Engine.IsEditorHint())
        return;
      this.AdjustFontSize();
    }
  }

  [Export]
  public bool IsHorizontallyBound
  {
    get => this._isHorizontallyBound;
    set
    {
      this._isHorizontallyBound = value;
      if (!Engine.IsEditorHint())
        return;
      this.AdjustFontSize();
    }
  }

  public override void _Ready()
  {
    MegaLabelHelper.AssertThemeFontOverride((Control) this, ThemeConstants.RichTextLabel.NormalFont);
    this.RefreshFont();
    this.InstallEffectsIfNeeded();
    this.AdjustFontSize();
    this.ParseBbcode(this.Text);
  }

  public void RefreshFont()
  {
    ((Control) this).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.RichTextLabel.NormalFont);
    ((Control) this).ApplyLocaleFontSubstitution(FontType.Bold, ThemeConstants.RichTextLabel.BoldFont);
    ((Control) this).ApplyLocaleFontSubstitution(FontType.Italic, ThemeConstants.RichTextLabel.ItalicsFont);
  }

  public override void _Notification(int what)
  {
    switch (what)
    {
      case 40:
        if ((double) ((Vector2) ref this._lastAdjustedSize).DistanceSquaredTo(((Control) this).Size) < 9.9999997473787516E-05 || !this.AutoSizeEnabled)
          break;
        this._needsResize = true;
        this.AdjustFontSize();
        break;
      case 9001:
        this.CustomEffects.Clear();
        break;
      case 9002:
        this.InstallEffectsIfNeeded();
        break;
    }
  }

  private void InstallEffectsIfNeeded()
  {
    if (this._effectsInstalled && this.CustomEffects.Count > 0 || !this.BbcodeEnabled)
      return;
    Array array = new Array();
    foreach (AbstractMegaRichTextEffect textEffect in MegaRichTextLabel._textEffects)
      array.Add(Variant.op_Implicit((GodotObject) textEffect));
    this.CustomEffects = array;
    this._effectsInstalled = true;
  }

  private bool HasEffect(AbstractMegaRichTextEffect effect)
  {
    return this.CustomEffects.Contains(Variant.op_Implicit((GodotObject) effect));
  }

  public void SetTextAutoSize(string text)
  {
    if (base.Text == text)
      return;
    base.Text = text;
    this.InstallEffectsIfNeeded();
    if (!this.AutoSizeEnabled)
      return;
    this._needsResize = true;
    ((GodotObject) this).CallDeferred(StringName.op_Implicit("AdjustFontSize"), Array.Empty<Variant>());
  }

  public string Text
  {
    get => base.Text;
    set => this.SetTextAutoSize(value);
  }

  private void AdjustFontSize()
  {
    TextParagraph cachedParagraph = MegaRichTextLabel._cachedParagraph;
    if (!this.AutoSizeEnabled || this._isAutoSizing || cachedParagraph == null || !this._needsResize)
      return;
    this._isAutoSizing = true;
    try
    {
      this._needsResize = true;
      this._lastAdjustedSize = ((Control) this).Size;
      Font themeFont = ((Control) this).GetThemeFont(ThemeConstants.RichTextLabel.NormalFont, StringName.op_Implicit("RichTextLabel"));
      float themeConstant = (float) ((Control) this).GetThemeConstant(ThemeConstants.RichTextLabel.LineSpacing, StringName.op_Implicit("RichTextLabel"));
      Rect2 rect = ((Control) this).GetRect();
      Vector2 size = ((Rect2) ref rect).Size;
      List<BbcodeObject> bbcode = MegaLabelHelper.ParseBbcode(this.Text);
      if (!MegaLabelHelper.IsTooBig(cachedParagraph, bbcode, themeFont, this.MaxFontSize, themeConstant, size, this._isHorizontallyBound, this._isVerticallyBound))
        this.SetFontSize(this.MaxFontSize);
      else if (this._lastSetSize >= this.MinFontSize && this._lastSetSize < this.MaxFontSize && !MegaLabelHelper.IsTooBig(cachedParagraph, bbcode, themeFont, this._lastSetSize, themeConstant, size, this._isHorizontallyBound, this._isVerticallyBound) && MegaLabelHelper.IsTooBig(cachedParagraph, bbcode, themeFont, this._lastSetSize + 1, themeConstant, size, this._isHorizontallyBound, this._isVerticallyBound))
      {
        this.SetFontSize(this._lastSetSize);
      }
      else
      {
        int num1 = this.MinFontSize;
        int num2 = this.MaxFontSize;
        while (num2 >= num1)
        {
          int fontSize = num1 + (num2 - num1) / 2;
          if (MegaLabelHelper.IsTooBig(cachedParagraph, bbcode, themeFont, fontSize, themeConstant, size, this._isHorizontallyBound, this._isVerticallyBound))
            num2 = fontSize - 1;
          else
            num1 = fontSize + 1;
        }
        this.SetFontSize(Math.Min(num1, num2));
      }
    }
    finally
    {
      this._isAutoSizing = false;
    }
  }

  private void SetFontSize(int size)
  {
    if (this._lastSetSize == size)
      return;
    this._lastSetSize = size;
    ((Control) this).AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.NormalFontSize, size);
    if (!this.BbcodeEnabled)
      return;
    ((Control) this).AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.BoldFontSize, size);
    ((Control) this).AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.BoldItalicsFontSize, size);
    ((Control) this).AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.ItalicsFontSize, size);
    ((Control) this).AddThemeFontSizeOverride(ThemeConstants.RichTextLabel.MonoFontSize, size);
    this.ParseBbcode(this.Text);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(MegaRichTextLabel.MethodName.DisposeCachedParagraph, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName.RefreshFont, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName.InstallEffectsIfNeeded, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName.HasEffect, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("effect"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("RichTextEffect"), false)
      }, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName.SetTextAutoSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName.AdjustFontSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaRichTextLabel.MethodName.SetFontSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.DisposeCachedParagraph) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      MegaRichTextLabel.DisposeCachedParagraph();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.RefreshFont) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshFont();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.InstallEffectsIfNeeded) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InstallEffectsIfNeeded();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.HasEffect) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = this.HasEffect(VariantUtils.ConvertTo<AbstractMegaRichTextEffect>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.SetTextAutoSize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTextAutoSize(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.AdjustFontSize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AdjustFontSize();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.SetFontSize) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetFontSize(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.DisposeCachedParagraph) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      MegaRichTextLabel.DisposeCachedParagraph();
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.DisposeCachedParagraph) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName._Ready) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.RefreshFont) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName._Notification) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.InstallEffectsIfNeeded) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.HasEffect) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.SetTextAutoSize) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.AdjustFontSize) || StringName.op_Equality(ref method, MegaRichTextLabel.MethodName.SetFontSize) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.AutoSizeEnabled))
    {
      this.AutoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.MinFontSize))
    {
      this.MinFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.MaxFontSize))
    {
      this.MaxFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.IsVerticallyBound))
    {
      this.IsVerticallyBound = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.IsHorizontallyBound))
    {
      this.IsHorizontallyBound = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.Text))
    {
      this.Text = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isAutoSizeEnabled))
    {
      this._isAutoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._minFontSize))
    {
      this._minFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._maxFontSize))
    {
      this._maxFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._lastSetSize))
    {
      this._lastSetSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isVerticallyBound))
    {
      this._isVerticallyBound = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isHorizontallyBound))
    {
      this._isHorizontallyBound = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._needsResize))
    {
      this._needsResize = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._effectsInstalled))
    {
      this._effectsInstalled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._lastAdjustedSize))
    {
      this._lastAdjustedSize = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isAutoSizing))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isAutoSizing = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.AutoSizeEnabled))
    {
      ref godot_variant local = ref value;
      bool autoSizeEnabled = this.AutoSizeEnabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref autoSizeEnabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.MinFontSize))
    {
      ref godot_variant local = ref value;
      int minFontSize = this.MinFontSize;
      godot_variant from = VariantUtils.CreateFrom<int>(ref minFontSize);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.MaxFontSize))
    {
      ref godot_variant local = ref value;
      int maxFontSize = this.MaxFontSize;
      godot_variant from = VariantUtils.CreateFrom<int>(ref maxFontSize);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.IsVerticallyBound))
    {
      ref godot_variant local = ref value;
      bool isVerticallyBound = this.IsVerticallyBound;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isVerticallyBound);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.IsHorizontallyBound))
    {
      ref godot_variant local = ref value;
      bool horizontallyBound = this.IsHorizontallyBound;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref horizontallyBound);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName.Text))
    {
      ref godot_variant local = ref value;
      string text = this.Text;
      godot_variant from = VariantUtils.CreateFrom<string>(ref text);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isAutoSizeEnabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isAutoSizeEnabled);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._minFontSize))
    {
      value = VariantUtils.CreateFrom<int>(ref this._minFontSize);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._maxFontSize))
    {
      value = VariantUtils.CreateFrom<int>(ref this._maxFontSize);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._lastSetSize))
    {
      value = VariantUtils.CreateFrom<int>(ref this._lastSetSize);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isVerticallyBound))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isVerticallyBound);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isHorizontallyBound))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHorizontallyBound);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._needsResize))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._needsResize);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._effectsInstalled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._effectsInstalled);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._lastAdjustedSize))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._lastAdjustedSize);
      return true;
    }
    if (!StringName.op_Equality(ref name, MegaRichTextLabel.PropertyName._isAutoSizing))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isAutoSizing);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName._isAutoSizeEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, MegaRichTextLabel.PropertyName._minFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, MegaRichTextLabel.PropertyName._maxFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, MegaRichTextLabel.PropertyName._lastSetSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName._isVerticallyBound, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName._isHorizontallyBound, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName._needsResize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName._effectsInstalled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, MegaRichTextLabel.PropertyName._lastAdjustedSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName.AutoSizeEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, MegaRichTextLabel.PropertyName.MinFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, MegaRichTextLabel.PropertyName.MaxFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName.IsVerticallyBound, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName.IsHorizontallyBound, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, MegaRichTextLabel.PropertyName._isAutoSizing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, MegaRichTextLabel.PropertyName.Text, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName autoSizeEnabled1 = MegaRichTextLabel.PropertyName.AutoSizeEnabled;
    bool autoSizeEnabled2 = this.AutoSizeEnabled;
    Variant variant1 = Variant.From<bool>(ref autoSizeEnabled2);
    serializationInfo1.AddProperty(autoSizeEnabled1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName minFontSize1 = MegaRichTextLabel.PropertyName.MinFontSize;
    int minFontSize2 = this.MinFontSize;
    Variant variant2 = Variant.From<int>(ref minFontSize2);
    serializationInfo2.AddProperty(minFontSize1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName maxFontSize1 = MegaRichTextLabel.PropertyName.MaxFontSize;
    int maxFontSize2 = this.MaxFontSize;
    Variant variant3 = Variant.From<int>(ref maxFontSize2);
    serializationInfo3.AddProperty(maxFontSize1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName isVerticallyBound1 = MegaRichTextLabel.PropertyName.IsVerticallyBound;
    bool isVerticallyBound2 = this.IsVerticallyBound;
    Variant variant4 = Variant.From<bool>(ref isVerticallyBound2);
    serializationInfo4.AddProperty(isVerticallyBound1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName horizontallyBound1 = MegaRichTextLabel.PropertyName.IsHorizontallyBound;
    bool horizontallyBound2 = this.IsHorizontallyBound;
    Variant variant5 = Variant.From<bool>(ref horizontallyBound2);
    serializationInfo5.AddProperty(horizontallyBound1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName text1 = MegaRichTextLabel.PropertyName.Text;
    string text2 = this.Text;
    Variant variant6 = Variant.From<string>(ref text2);
    serializationInfo6.AddProperty(text1, variant6);
    info.AddProperty(MegaRichTextLabel.PropertyName._isAutoSizeEnabled, Variant.From<bool>(ref this._isAutoSizeEnabled));
    info.AddProperty(MegaRichTextLabel.PropertyName._minFontSize, Variant.From<int>(ref this._minFontSize));
    info.AddProperty(MegaRichTextLabel.PropertyName._maxFontSize, Variant.From<int>(ref this._maxFontSize));
    info.AddProperty(MegaRichTextLabel.PropertyName._lastSetSize, Variant.From<int>(ref this._lastSetSize));
    info.AddProperty(MegaRichTextLabel.PropertyName._isVerticallyBound, Variant.From<bool>(ref this._isVerticallyBound));
    info.AddProperty(MegaRichTextLabel.PropertyName._isHorizontallyBound, Variant.From<bool>(ref this._isHorizontallyBound));
    info.AddProperty(MegaRichTextLabel.PropertyName._needsResize, Variant.From<bool>(ref this._needsResize));
    info.AddProperty(MegaRichTextLabel.PropertyName._effectsInstalled, Variant.From<bool>(ref this._effectsInstalled));
    info.AddProperty(MegaRichTextLabel.PropertyName._lastAdjustedSize, Variant.From<Vector2>(ref this._lastAdjustedSize));
    info.AddProperty(MegaRichTextLabel.PropertyName._isAutoSizing, Variant.From<bool>(ref this._isAutoSizing));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName.AutoSizeEnabled, ref variant1))
      this.AutoSizeEnabled = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName.MinFontSize, ref variant2))
      this.MinFontSize = ((Variant) ref variant2).As<int>();
    Variant variant3;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName.MaxFontSize, ref variant3))
      this.MaxFontSize = ((Variant) ref variant3).As<int>();
    Variant variant4;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName.IsVerticallyBound, ref variant4))
      this.IsVerticallyBound = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName.IsHorizontallyBound, ref variant5))
      this.IsHorizontallyBound = ((Variant) ref variant5).As<bool>();
    Variant variant6;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName.Text, ref variant6))
      this.Text = ((Variant) ref variant6).As<string>();
    Variant variant7;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isAutoSizeEnabled, ref variant7))
      this._isAutoSizeEnabled = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._minFontSize, ref variant8))
      this._minFontSize = ((Variant) ref variant8).As<int>();
    Variant variant9;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._maxFontSize, ref variant9))
      this._maxFontSize = ((Variant) ref variant9).As<int>();
    Variant variant10;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._lastSetSize, ref variant10))
      this._lastSetSize = ((Variant) ref variant10).As<int>();
    Variant variant11;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isVerticallyBound, ref variant11))
      this._isVerticallyBound = ((Variant) ref variant11).As<bool>();
    Variant variant12;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._isHorizontallyBound, ref variant12))
      this._isHorizontallyBound = ((Variant) ref variant12).As<bool>();
    Variant variant13;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._needsResize, ref variant13))
      this._needsResize = ((Variant) ref variant13).As<bool>();
    Variant variant14;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._effectsInstalled, ref variant14))
      this._effectsInstalled = ((Variant) ref variant14).As<bool>();
    Variant variant15;
    if (info.TryGetProperty(MegaRichTextLabel.PropertyName._lastAdjustedSize, ref variant15))
      this._lastAdjustedSize = ((Variant) ref variant15).As<Vector2>();
    Variant variant16;
    if (!info.TryGetProperty(MegaRichTextLabel.PropertyName._isAutoSizing, ref variant16))
      return;
    this._isAutoSizing = ((Variant) ref variant16).As<bool>();
  }

  public class MethodName : RichTextLabel.MethodName
  {
    public static readonly StringName DisposeCachedParagraph = StringName.op_Implicit(nameof (DisposeCachedParagraph));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshFont = StringName.op_Implicit(nameof (RefreshFont));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName InstallEffectsIfNeeded = StringName.op_Implicit(nameof (InstallEffectsIfNeeded));
    public static readonly StringName HasEffect = StringName.op_Implicit(nameof (HasEffect));
    public static readonly StringName SetTextAutoSize = StringName.op_Implicit(nameof (SetTextAutoSize));
    public static readonly StringName AdjustFontSize = StringName.op_Implicit(nameof (AdjustFontSize));
    public static readonly StringName SetFontSize = StringName.op_Implicit(nameof (SetFontSize));
  }

  public class PropertyName : RichTextLabel.PropertyName
  {
    public static readonly StringName AutoSizeEnabled = StringName.op_Implicit(nameof (AutoSizeEnabled));
    public static readonly StringName MinFontSize = StringName.op_Implicit(nameof (MinFontSize));
    public static readonly StringName MaxFontSize = StringName.op_Implicit(nameof (MaxFontSize));
    public static readonly StringName IsVerticallyBound = StringName.op_Implicit(nameof (IsVerticallyBound));
    public static readonly StringName IsHorizontallyBound = StringName.op_Implicit(nameof (IsHorizontallyBound));
    public static readonly StringName Text = StringName.op_Implicit(nameof (Text));
    public static readonly StringName _isAutoSizeEnabled = StringName.op_Implicit(nameof (_isAutoSizeEnabled));
    public static readonly StringName _minFontSize = StringName.op_Implicit(nameof (_minFontSize));
    public static readonly StringName _maxFontSize = StringName.op_Implicit(nameof (_maxFontSize));
    public static readonly StringName _lastSetSize = StringName.op_Implicit(nameof (_lastSetSize));
    public static readonly StringName _isVerticallyBound = StringName.op_Implicit(nameof (_isVerticallyBound));
    public static readonly StringName _isHorizontallyBound = StringName.op_Implicit(nameof (_isHorizontallyBound));
    public static readonly StringName _needsResize = StringName.op_Implicit(nameof (_needsResize));
    public static readonly StringName _effectsInstalled = StringName.op_Implicit(nameof (_effectsInstalled));
    public static readonly StringName _lastAdjustedSize = StringName.op_Implicit(nameof (_lastAdjustedSize));
    public static readonly StringName _isAutoSizing = StringName.op_Implicit(nameof (_isAutoSizing));
  }

  public class SignalName : RichTextLabel.SignalName
  {
  }
}
