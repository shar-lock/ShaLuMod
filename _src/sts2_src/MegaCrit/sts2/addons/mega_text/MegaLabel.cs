// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.addons.mega_text.MegaLabel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization.Fonts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.addons.mega_text;

[Tool]
[ScriptPath("res://addons/mega_text/MegaLabel.cs")]
public class MegaLabel : Label
{
  private static TextParagraph? _cachedParagraph = new TextParagraph();
  private const float _sizeComparisonEpsilon = 0.01f;
  private bool _autoSizeEnabled = true;
  private int _minFontSize = 8;
  private int _maxFontSize = 100;
  private int _lastSetSize;
  private Vector2 _lastAdjustedSize;

  public static void DisposeCachedParagraph()
  {
    ((GodotObject) MegaLabel._cachedParagraph)?.Dispose();
    MegaLabel._cachedParagraph = (TextParagraph) null;
  }

  [Export]
  public bool AutoSizeEnabled
  {
    get => this._autoSizeEnabled;
    set
    {
      if (this._autoSizeEnabled == value)
        return;
      this._autoSizeEnabled = value;
      if (!Engine.IsEditorHint())
        return;
      this.AdjustFontSize();
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

  public override void _Ready()
  {
    MegaLabelHelper.AssertThemeFontOverride((Control) this, ThemeConstants.Label.Font);
    this.RefreshFont();
    this.AdjustFontSize();
  }

  public void RefreshFont()
  {
    ((Control) this).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
  }

  public override void _Notification(int what)
  {
    if (what != 40 || (double) ((Vector2) ref this._lastAdjustedSize).DistanceSquaredTo(((Control) this).Size) < 9.9999997473787516E-05)
      return;
    this.AdjustFontSize();
  }

  public void SetTextAutoSize(string text)
  {
    if (this.Text == text)
      return;
    this.Text = text;
    this.AdjustFontSize();
  }

  private void SetFontSize(int size)
  {
    if (this._lastSetSize == size)
      return;
    this._lastSetSize = size;
    if (!((Control) this).HasThemeFont(ThemeConstants.Label.Font, (StringName) null))
      return;
    ((Control) this).AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, size);
  }

  private void AdjustFontSize()
  {
    TextParagraph cachedParagraph = MegaLabel._cachedParagraph;
    if (!this.AutoSizeEnabled || cachedParagraph == null)
      return;
    this._lastAdjustedSize = ((Control) this).Size;
    Font themeFont = ((Control) this).GetThemeFont(ThemeConstants.Label.Font, StringName.op_Implicit("Label"));
    float themeConstant = (float) ((Control) this).GetThemeConstant(ThemeConstants.Label.LineSpacing, StringName.op_Implicit("Label"));
    Rect2 rect = ((Control) this).GetRect();
    Vector2 size = ((Rect2) ref rect).Size;
    bool wrap = this.AutowrapMode > 0L;
    if (!MegaLabelHelper.IsTooBig(cachedParagraph, this.Text, themeFont, this.MaxFontSize, themeConstant, wrap, size))
      this.SetFontSize(this.MaxFontSize);
    else if (this._lastSetSize >= this.MinFontSize && this._lastSetSize < this.MaxFontSize && !MegaLabelHelper.IsTooBig(cachedParagraph, this.Text, themeFont, this._lastSetSize, themeConstant, wrap, size) && MegaLabelHelper.IsTooBig(cachedParagraph, this.Text, themeFont, this._lastSetSize + 1, themeConstant, wrap, size))
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
        if (fontSize == this.MaxFontSize || MegaLabelHelper.IsTooBig(cachedParagraph, this.Text, themeFont, fontSize, themeConstant, wrap, size))
          num2 = fontSize - 1;
        else
          num1 = fontSize + 1;
      }
      this.SetFontSize(Math.Min(num1, num2));
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(MegaLabel.MethodName.DisposeCachedParagraph, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaLabel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaLabel.MethodName.RefreshFont, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(MegaLabel.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(MegaLabel.MethodName.SetTextAutoSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(MegaLabel.MethodName.SetFontSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(MegaLabel.MethodName.AdjustFontSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, MegaLabel.MethodName.DisposeCachedParagraph) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      MegaLabel.DisposeCachedParagraph();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaLabel.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaLabel.MethodName.RefreshFont) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshFont();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaLabel.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaLabel.MethodName.SetTextAutoSize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTextAutoSize(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, MegaLabel.MethodName.SetFontSize) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetFontSize(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, MegaLabel.MethodName.AdjustFontSize) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AdjustFontSize();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, MegaLabel.MethodName.DisposeCachedParagraph) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      MegaLabel.DisposeCachedParagraph();
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, MegaLabel.MethodName.DisposeCachedParagraph) || StringName.op_Equality(ref method, MegaLabel.MethodName._Ready) || StringName.op_Equality(ref method, MegaLabel.MethodName.RefreshFont) || StringName.op_Equality(ref method, MegaLabel.MethodName._Notification) || StringName.op_Equality(ref method, MegaLabel.MethodName.SetTextAutoSize) || StringName.op_Equality(ref method, MegaLabel.MethodName.SetFontSize) || StringName.op_Equality(ref method, MegaLabel.MethodName.AdjustFontSize) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName.AutoSizeEnabled))
    {
      this.AutoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName.MinFontSize))
    {
      this.MinFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName.MaxFontSize))
    {
      this.MaxFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._autoSizeEnabled))
    {
      this._autoSizeEnabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._minFontSize))
    {
      this._minFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._maxFontSize))
    {
      this._maxFontSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._lastSetSize))
    {
      this._lastSetSize = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, MegaLabel.PropertyName._lastAdjustedSize))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lastAdjustedSize = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName.AutoSizeEnabled))
    {
      ref godot_variant local = ref value;
      bool autoSizeEnabled = this.AutoSizeEnabled;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref autoSizeEnabled);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName.MinFontSize))
    {
      ref godot_variant local = ref value;
      int minFontSize = this.MinFontSize;
      godot_variant from = VariantUtils.CreateFrom<int>(ref minFontSize);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName.MaxFontSize))
    {
      ref godot_variant local = ref value;
      int maxFontSize = this.MaxFontSize;
      godot_variant from = VariantUtils.CreateFrom<int>(ref maxFontSize);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._autoSizeEnabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._autoSizeEnabled);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._minFontSize))
    {
      value = VariantUtils.CreateFrom<int>(ref this._minFontSize);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._maxFontSize))
    {
      value = VariantUtils.CreateFrom<int>(ref this._maxFontSize);
      return true;
    }
    if (StringName.op_Equality(ref name, MegaLabel.PropertyName._lastSetSize))
    {
      value = VariantUtils.CreateFrom<int>(ref this._lastSetSize);
      return true;
    }
    if (!StringName.op_Equality(ref name, MegaLabel.PropertyName._lastAdjustedSize))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._lastAdjustedSize);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, MegaLabel.PropertyName._autoSizeEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, MegaLabel.PropertyName._minFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, MegaLabel.PropertyName._maxFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, MegaLabel.PropertyName._lastSetSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, MegaLabel.PropertyName._lastAdjustedSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, MegaLabel.PropertyName.AutoSizeEnabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, MegaLabel.PropertyName.MinFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, MegaLabel.PropertyName.MaxFontSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName autoSizeEnabled1 = MegaLabel.PropertyName.AutoSizeEnabled;
    bool autoSizeEnabled2 = this.AutoSizeEnabled;
    Variant variant1 = Variant.From<bool>(ref autoSizeEnabled2);
    serializationInfo1.AddProperty(autoSizeEnabled1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName minFontSize1 = MegaLabel.PropertyName.MinFontSize;
    int minFontSize2 = this.MinFontSize;
    Variant variant2 = Variant.From<int>(ref minFontSize2);
    serializationInfo2.AddProperty(minFontSize1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName maxFontSize1 = MegaLabel.PropertyName.MaxFontSize;
    int maxFontSize2 = this.MaxFontSize;
    Variant variant3 = Variant.From<int>(ref maxFontSize2);
    serializationInfo3.AddProperty(maxFontSize1, variant3);
    info.AddProperty(MegaLabel.PropertyName._autoSizeEnabled, Variant.From<bool>(ref this._autoSizeEnabled));
    info.AddProperty(MegaLabel.PropertyName._minFontSize, Variant.From<int>(ref this._minFontSize));
    info.AddProperty(MegaLabel.PropertyName._maxFontSize, Variant.From<int>(ref this._maxFontSize));
    info.AddProperty(MegaLabel.PropertyName._lastSetSize, Variant.From<int>(ref this._lastSetSize));
    info.AddProperty(MegaLabel.PropertyName._lastAdjustedSize, Variant.From<Vector2>(ref this._lastAdjustedSize));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(MegaLabel.PropertyName.AutoSizeEnabled, ref variant1))
      this.AutoSizeEnabled = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(MegaLabel.PropertyName.MinFontSize, ref variant2))
      this.MinFontSize = ((Variant) ref variant2).As<int>();
    Variant variant3;
    if (info.TryGetProperty(MegaLabel.PropertyName.MaxFontSize, ref variant3))
      this.MaxFontSize = ((Variant) ref variant3).As<int>();
    Variant variant4;
    if (info.TryGetProperty(MegaLabel.PropertyName._autoSizeEnabled, ref variant4))
      this._autoSizeEnabled = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(MegaLabel.PropertyName._minFontSize, ref variant5))
      this._minFontSize = ((Variant) ref variant5).As<int>();
    Variant variant6;
    if (info.TryGetProperty(MegaLabel.PropertyName._maxFontSize, ref variant6))
      this._maxFontSize = ((Variant) ref variant6).As<int>();
    Variant variant7;
    if (info.TryGetProperty(MegaLabel.PropertyName._lastSetSize, ref variant7))
      this._lastSetSize = ((Variant) ref variant7).As<int>();
    Variant variant8;
    if (!info.TryGetProperty(MegaLabel.PropertyName._lastAdjustedSize, ref variant8))
      return;
    this._lastAdjustedSize = ((Variant) ref variant8).As<Vector2>();
  }

  public class MethodName : Label.MethodName
  {
    public static readonly StringName DisposeCachedParagraph = StringName.op_Implicit(nameof (DisposeCachedParagraph));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshFont = StringName.op_Implicit(nameof (RefreshFont));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName SetTextAutoSize = StringName.op_Implicit(nameof (SetTextAutoSize));
    public static readonly StringName SetFontSize = StringName.op_Implicit(nameof (SetFontSize));
    public static readonly StringName AdjustFontSize = StringName.op_Implicit(nameof (AdjustFontSize));
  }

  public class PropertyName : Label.PropertyName
  {
    public static readonly StringName AutoSizeEnabled = StringName.op_Implicit(nameof (AutoSizeEnabled));
    public static readonly StringName MinFontSize = StringName.op_Implicit(nameof (MinFontSize));
    public static readonly StringName MaxFontSize = StringName.op_Implicit(nameof (MaxFontSize));
    public static readonly StringName _autoSizeEnabled = StringName.op_Implicit(nameof (_autoSizeEnabled));
    public static readonly StringName _minFontSize = StringName.op_Implicit(nameof (_minFontSize));
    public static readonly StringName _maxFontSize = StringName.op_Implicit(nameof (_maxFontSize));
    public static readonly StringName _lastSetSize = StringName.op_Implicit(nameof (_lastSetSize));
    public static readonly StringName _lastAdjustedSize = StringName.op_Implicit(nameof (_lastAdjustedSize));
  }

  public class SignalName : Label.SignalName
  {
  }
}
