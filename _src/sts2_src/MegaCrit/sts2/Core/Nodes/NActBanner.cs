// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NActBanner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NActBanner.cs")]
public class NActBanner : Control
{
  private MegaLabel _actNumber;
  private MegaLabel _actName;
  private ColorRect _banner;
  private static readonly string _path = SceneHelper.GetScenePath("ui/act_banner");
  private ActModel _act;
  private int _actIndex;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NActBanner._path);
    }
  }

  public static NActBanner? Create(ActModel act, int actIndex)
  {
    if (TestMode.IsOn)
      return (NActBanner) null;
    NActBanner nactBanner = PreloadManager.Cache.GetScene(NActBanner._path).Instantiate<NActBanner>((PackedScene.GenEditState) 0L);
    nactBanner._act = act;
    nactBanner._actIndex = actIndex;
    return nactBanner;
  }

  public override void _Ready()
  {
    this._actNumber = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("ActNumber"));
    this._actName = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("ActName"));
    this._banner = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("%Banner"));
    LocString locString = new LocString("gameplay_ui", "ACT_NUMBER");
    locString.Add("actNumber", (Decimal) (this._actIndex + 1));
    this._actNumber.SetTextAutoSize(locString.GetFormattedText());
    this._actName.SetTextAutoSize(this._act.Title.GetFormattedText());
    TaskHelper.RunSafely(this.AnimateVfx());
  }

  private async Task AnimateVfx()
  {
    ((CanvasItem) this._banner).Modulate = StsColors.transparentBlack;
    ((CanvasItem) this._actName).Modulate = StsColors.transparentWhite;
    ((CanvasItem) this._actNumber).Modulate = StsColors.transparentWhite;
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    tween.TweenProperty((GodotObject) this._banner, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.5).SetDelay(0.5);
    tween.TweenProperty((GodotObject) this._actName, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay(0.25);
    tween.TweenProperty((GodotObject) this._actNumber, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.0).SetDelay(0.5);
    tween.TweenProperty((GodotObject) this._actNumber, NodePath.op_Implicit("position:y"), Variant.op_Implicit(440f), 1.25).SetDelay(0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L).From(Variant.op_Implicit(450f));
    tween.Chain();
    tween.TweenInterval(SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.5 : 2.0);
    tween.Chain();
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    bool flag = await tween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NActBanner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NActBanner.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NActBanner.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NActBanner.PropertyName._actNumber))
    {
      this._actNumber = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NActBanner.PropertyName._actName))
    {
      this._actName = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NActBanner.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NActBanner.PropertyName._actIndex))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._actIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NActBanner.PropertyName._actNumber))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._actNumber);
      return true;
    }
    if (StringName.op_Equality(ref name, NActBanner.PropertyName._actName))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._actName);
      return true;
    }
    if (StringName.op_Equality(ref name, NActBanner.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._banner);
      return true;
    }
    if (!StringName.op_Equality(ref name, NActBanner.PropertyName._actIndex))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._actIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NActBanner.PropertyName._actNumber, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NActBanner.PropertyName._actName, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NActBanner.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NActBanner.PropertyName._actIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NActBanner.PropertyName._actNumber, Variant.From<MegaLabel>(ref this._actNumber));
    info.AddProperty(NActBanner.PropertyName._actName, Variant.From<MegaLabel>(ref this._actName));
    info.AddProperty(NActBanner.PropertyName._banner, Variant.From<ColorRect>(ref this._banner));
    info.AddProperty(NActBanner.PropertyName._actIndex, Variant.From<int>(ref this._actIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NActBanner.PropertyName._actNumber, ref variant1))
      this._actNumber = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NActBanner.PropertyName._actName, ref variant2))
      this._actName = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NActBanner.PropertyName._banner, ref variant3))
      this._banner = ((Variant) ref variant3).As<ColorRect>();
    Variant variant4;
    if (!info.TryGetProperty(NActBanner.PropertyName._actIndex, ref variant4))
      return;
    this._actIndex = ((Variant) ref variant4).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _actNumber = StringName.op_Implicit(nameof (_actNumber));
    public static readonly StringName _actName = StringName.op_Implicit(nameof (_actName));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _actIndex = StringName.op_Implicit(nameof (_actIndex));
  }

  public class SignalName : Control.SignalName
  {
  }
}
