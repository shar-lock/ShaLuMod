// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NAchievementHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NAchievementHolder.cs")]
public class NAchievementHolder : Control
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private const string _scenePath = "screens/stats_screen/achievement_holder";
  private TextureRect _border;
  private TextureRect _icon;
  private TextureRect _lock;
  private ShaderMaterial _iconHsv;
  private ShaderMaterial _borderHsv;
  private MegaRichTextLabel _infoLabel;
  private MegaLabel _date;
  private Achievement _achievement;
  private Tween? _tween;

  public bool IsUnlocked { get; private set; }

  public static NAchievementHolder? Create(Achievement achievement)
  {
    if (TestMode.IsOn)
      return (NAchievementHolder) null;
    NAchievementHolder nachievementHolder = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("screens/stats_screen/achievement_holder")).Instantiate<NAchievementHolder>((PackedScene.GenEditState) 0L);
    nachievementHolder._achievement = achievement;
    nachievementHolder.IsUnlocked = AchievementsUtil.IsUnlocked(achievement);
    return nachievementHolder;
  }

  public override void _Ready()
  {
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._border = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Border"));
    this._lock = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Lock"));
    this._borderHsv = (ShaderMaterial) ((CanvasItem) this._border).Material;
    this._iconHsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
    this._infoLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%InfoText"));
    this._date = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%DateText"));
    this.RefreshUnlocked();
  }

  public static string GetPathForAchievement(Enum achievement)
  {
    return ImageHelper.GetImagePath($"packed/achievements/unlocked/{StringHelper.SnakeCase(achievement.ToString()).ToLowerInvariant()}.png");
  }

  public void RefreshUnlocked()
  {
    this.IsUnlocked = AchievementsUtil.IsUnlocked(this._achievement);
    string lowerInvariant = StringHelper.SnakeCase(this._achievement.ToString()).ToLowerInvariant();
    this._icon.Texture = PreloadManager.Cache.GetTexture2D(NAchievementHolder.GetPathForAchievement((Enum) this._achievement));
    string upperInvariant = lowerInvariant.ToUpperInvariant();
    this._infoLabel.Text = !this.IsUnlocked ? $"[b][red]{new LocString("achievements", "LOCKED.title").GetRawText()}[/red][/b]\n{new LocString("achievements", upperInvariant + ".description").GetFormattedText()}" : $"[b][gold]{new LocString("achievements", upperInvariant + ".title").GetRawText()}[/gold][/b]\n{new LocString("achievements", upperInvariant + ".description").GetFormattedText()}";
    this.SetLockVisuals();
    this.SetDateLabel();
  }

  private void SetLockVisuals()
  {
    ((CanvasItem) this._lock).Visible = !this.IsUnlocked;
    if (this.IsUnlocked)
    {
      this._borderHsv.SetShaderParameter(NAchievementHolder._h, Variant.op_Implicit(1f));
      this._borderHsv.SetShaderParameter(NAchievementHolder._s, Variant.op_Implicit(1f));
      this._borderHsv.SetShaderParameter(NAchievementHolder._v, Variant.op_Implicit(1f));
      this._iconHsv.SetShaderParameter(NAchievementHolder._s, Variant.op_Implicit(1f));
      this._iconHsv.SetShaderParameter(NAchievementHolder._v, Variant.op_Implicit(1f));
    }
    else
    {
      this._borderHsv.SetShaderParameter(NAchievementHolder._h, Variant.op_Implicit(0.4f));
      this._borderHsv.SetShaderParameter(NAchievementHolder._s, Variant.op_Implicit(0.4f));
      this._borderHsv.SetShaderParameter(NAchievementHolder._v, Variant.op_Implicit(0.8f));
      this._iconHsv.SetShaderParameter(NAchievementHolder._s, Variant.op_Implicit(0.2f));
      this._iconHsv.SetShaderParameter(NAchievementHolder._v, Variant.op_Implicit(0.5f));
    }
  }

  private void SetDateLabel()
  {
    ((CanvasItem) this._date).Visible = this.IsUnlocked;
    if (!this.IsUnlocked)
      return;
    long seconds;
    if (!SaveManager.Instance.Progress.UnlockedAchievements.TryGetValue(this._achievement, out seconds))
    {
      ((CanvasItem) this._date).Visible = false;
    }
    else
    {
      DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
      DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime, TimeZoneInfo.Local);
      LocString locString = new LocString("achievements", "UNLOCK_DATE.text");
      string variable = dateTime.ToString(new LocString("achievements", "UNLOCK_DATE.format").GetRawText(), (IFormatProvider) dateTimeFormat);
      locString.Add("Date", variable);
      this._date.SetTextAutoSize(locString.GetFormattedText());
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NAchievementHolder.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("achievement"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAchievementHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAchievementHolder.MethodName.RefreshUnlocked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAchievementHolder.MethodName.SetLockVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAchievementHolder.MethodName.SetDateLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAchievementHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAchievementHolder nachievementHolder = NAchievementHolder.Create(VariantUtils.ConvertTo<Achievement>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NAchievementHolder>(ref nachievementHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NAchievementHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAchievementHolder.MethodName.RefreshUnlocked) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshUnlocked();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAchievementHolder.MethodName.SetLockVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetLockVisuals();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAchievementHolder.MethodName.SetDateLabel) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetDateLabel();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAchievementHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAchievementHolder nachievementHolder = NAchievementHolder.Create(VariantUtils.ConvertTo<Achievement>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NAchievementHolder>(ref nachievementHolder);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAchievementHolder.MethodName.Create) || StringName.op_Equality(ref method, NAchievementHolder.MethodName._Ready) || StringName.op_Equality(ref method, NAchievementHolder.MethodName.RefreshUnlocked) || StringName.op_Equality(ref method, NAchievementHolder.MethodName.SetLockVisuals) || StringName.op_Equality(ref method, NAchievementHolder.MethodName.SetDateLabel) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName.IsUnlocked))
    {
      this.IsUnlocked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._border))
    {
      this._border = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._lock))
    {
      this._lock = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._iconHsv))
    {
      this._iconHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._borderHsv))
    {
      this._borderHsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._date))
    {
      this._date = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._achievement))
    {
      this._achievement = VariantUtils.ConvertTo<Achievement>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAchievementHolder.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName.IsUnlocked))
    {
      ref godot_variant local = ref value;
      bool isUnlocked = this.IsUnlocked;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isUnlocked);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._border))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._border);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._lock))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._lock);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._iconHsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._iconHsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._borderHsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._borderHsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._date))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._date);
      return true;
    }
    if (StringName.op_Equality(ref name, NAchievementHolder.PropertyName._achievement))
    {
      value = VariantUtils.CreateFrom<Achievement>(ref this._achievement);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAchievementHolder.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._border, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._lock, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._iconHsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._borderHsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._date, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NAchievementHolder.PropertyName.IsUnlocked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NAchievementHolder.PropertyName._achievement, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAchievementHolder.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isUnlocked1 = NAchievementHolder.PropertyName.IsUnlocked;
    bool isUnlocked2 = this.IsUnlocked;
    Variant variant = Variant.From<bool>(ref isUnlocked2);
    serializationInfo.AddProperty(isUnlocked1, variant);
    info.AddProperty(NAchievementHolder.PropertyName._border, Variant.From<TextureRect>(ref this._border));
    info.AddProperty(NAchievementHolder.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NAchievementHolder.PropertyName._lock, Variant.From<TextureRect>(ref this._lock));
    info.AddProperty(NAchievementHolder.PropertyName._iconHsv, Variant.From<ShaderMaterial>(ref this._iconHsv));
    info.AddProperty(NAchievementHolder.PropertyName._borderHsv, Variant.From<ShaderMaterial>(ref this._borderHsv));
    info.AddProperty(NAchievementHolder.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
    info.AddProperty(NAchievementHolder.PropertyName._date, Variant.From<MegaLabel>(ref this._date));
    info.AddProperty(NAchievementHolder.PropertyName._achievement, Variant.From<Achievement>(ref this._achievement));
    info.AddProperty(NAchievementHolder.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAchievementHolder.PropertyName.IsUnlocked, ref variant1))
      this.IsUnlocked = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._border, ref variant2))
      this._border = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._icon, ref variant3))
      this._icon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._lock, ref variant4))
      this._lock = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._iconHsv, ref variant5))
      this._iconHsv = ((Variant) ref variant5).As<ShaderMaterial>();
    Variant variant6;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._borderHsv, ref variant6))
      this._borderHsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._infoLabel, ref variant7))
      this._infoLabel = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._date, ref variant8))
      this._date = ((Variant) ref variant8).As<MegaLabel>();
    Variant variant9;
    if (info.TryGetProperty(NAchievementHolder.PropertyName._achievement, ref variant9))
      this._achievement = ((Variant) ref variant9).As<Achievement>();
    Variant variant10;
    if (!info.TryGetProperty(NAchievementHolder.PropertyName._tween, ref variant10))
      return;
    this._tween = ((Variant) ref variant10).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshUnlocked = StringName.op_Implicit(nameof (RefreshUnlocked));
    public static readonly StringName SetLockVisuals = StringName.op_Implicit(nameof (SetLockVisuals));
    public static readonly StringName SetDateLabel = StringName.op_Implicit(nameof (SetDateLabel));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsUnlocked = StringName.op_Implicit(nameof (IsUnlocked));
    public static readonly StringName _border = StringName.op_Implicit(nameof (_border));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _lock = StringName.op_Implicit(nameof (_lock));
    public static readonly StringName _iconHsv = StringName.op_Implicit(nameof (_iconHsv));
    public static readonly StringName _borderHsv = StringName.op_Implicit(nameof (_borderHsv));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _date = StringName.op_Implicit(nameof (_date));
    public static readonly StringName _achievement = StringName.op_Implicit(nameof (_achievement));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
