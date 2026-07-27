// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockRelicsScreen
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
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockRelicsScreen.cs")]
public class NUnlockRelicsScreen : NUnlockScreen
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_relics_screen");
  private Control _relicRow;
  private NCommonBanner _banner;
  private IReadOnlyList<RelicModel> _relics;
  private Tween? _relicTween;
  private const float _relicXOffset = 350f;
  private static readonly Vector2 _relicScale = Vector2.op_Multiply(Vector2.One, 3f);

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NUnlockRelicsScreen._scenePath);
    }
  }

  public static NUnlockRelicsScreen Create()
  {
    return PreloadManager.Cache.GetScene(NUnlockRelicsScreen._scenePath).Instantiate<NUnlockRelicsScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("%Banner"));
    this._banner.label.SetTextAutoSize(new LocString("timeline", "UNLOCK_RELICS_BANNER").GetRawText());
    this._banner.AnimateIn();
    this._relicRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RelicRow"));
    LocString locString = new LocString("timeline", "UNLOCK_RELICS");
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ExplanationText")).Text = $"[center]{locString.GetFormattedText()}[/center]";
  }

  public override void Open()
  {
    base.Open();
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock");
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) (this._relics.Count - 1)), 350f), 0.5f);
    this._relicTween = ((Node) this).CreateTween().SetParallel(true);
    int num = 0;
    foreach (RelicModel relic in (IEnumerable<RelicModel>) this._relics)
    {
      NRelicBasicHolder child = NRelicBasicHolder.Create(relic);
      ((Node) this._relicRow).AddChildSafely((Node) child);
      ((CanvasItem) child).Modulate = StsColors.transparentBlack;
      child.Scale = NUnlockRelicsScreen._relicScale;
      this._relicTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(vector2, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 350f), (float) num))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._relicTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
      ++num;
    }
    ActiveScreenContext.Instance.FocusOnDefaultControl();
  }

  public void SetRelics(IReadOnlyList<RelicModel> relics) => this._relics = relics;

  protected override void OnScreenClose() => NTimelineScreen.Instance.EnableInput();

  public override Control? DefaultFocusedControl
  {
    get
    {
      return ((Node) this._relicRow).GetChildCount(false) != 0 ? ((Node) this._relicRow).GetChild<Control>(((Node) this._relicRow).GetChildCount(false) / 2, false) : (Control) null;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NUnlockRelicsScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockRelicsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockRelicsScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockRelicsScreen.MethodName.OnScreenClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockRelicsScreen nunlockRelicsScreen = NUnlockRelicsScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockRelicsScreen>(ref nunlockRelicsScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.OnScreenClose) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnScreenClose();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockRelicsScreen nunlockRelicsScreen = NUnlockRelicsScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockRelicsScreen>(ref nunlockRelicsScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.Create) || StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.Open) || StringName.op_Equality(ref method, NUnlockRelicsScreen.MethodName.OnScreenClose) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName._relicRow))
    {
      this._relicRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName._relicTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._relicTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName._relicRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockRelicsScreen.PropertyName._relicTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._relicTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockRelicsScreen.PropertyName._relicRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockRelicsScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockRelicsScreen.PropertyName._relicTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockRelicsScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockRelicsScreen.PropertyName._relicRow, Variant.From<Control>(ref this._relicRow));
    info.AddProperty(NUnlockRelicsScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NUnlockRelicsScreen.PropertyName._relicTween, Variant.From<Tween>(ref this._relicTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockRelicsScreen.PropertyName._relicRow, ref variant1))
      this._relicRow = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NUnlockRelicsScreen.PropertyName._banner, ref variant2))
      this._banner = ((Variant) ref variant2).As<NCommonBanner>();
    Variant variant3;
    if (!info.TryGetProperty(NUnlockRelicsScreen.PropertyName._relicTween, ref variant3))
      return;
    this._relicTween = ((Variant) ref variant3).As<Tween>();
  }

  public new class MethodName : NUnlockScreen.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public new static readonly StringName OnScreenClose = StringName.op_Implicit(nameof (OnScreenClose));
  }

  public new class PropertyName : NUnlockScreen.PropertyName
  {
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _relicRow = StringName.op_Implicit(nameof (_relicRow));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _relicTween = StringName.op_Implicit(nameof (_relicTween));
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
