// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockPotionsScreen
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
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockPotionsScreen.cs")]
public class NUnlockPotionsScreen : NUnlockScreen
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_potions_screen");
  private Control _potionRow;
  private NCommonBanner _banner;
  private IReadOnlyList<PotionModel> _potions;
  private Tween? _potionTween;
  private const float _potionXOffset = 350f;
  private static readonly Vector2 _potionScale = Vector2.op_Multiply(Vector2.One, 3f);

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NUnlockPotionsScreen._scenePath);
    }
  }

  public static NUnlockPotionsScreen Create()
  {
    return PreloadManager.Cache.GetScene(NUnlockPotionsScreen._scenePath).Instantiate<NUnlockPotionsScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("%Banner"));
    this._banner.label.SetTextAutoSize(new LocString("timeline", "UNLOCK_POTIONS_BANNER").GetRawText());
    this._banner.AnimateIn();
    this._potionRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PotionRow"));
    LocString locString = new LocString("timeline", "UNLOCK_POTIONS");
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ExplanationText")).Text = $"[center]{locString.GetFormattedText()}[/center]";
  }

  public override void Open()
  {
    base.Open();
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock");
    this._potionTween = ((Node) this).CreateTween().SetParallel(true);
    int num = -1;
    foreach (PotionModel potion1 in (IEnumerable<PotionModel>) this._potions)
    {
      NPotionHolder child = NPotionHolder.Create(false);
      ((Node) this._potionRow).AddChildSafely((Node) child);
      child.Position = this.Position;
      ((CanvasItem) child).Modulate = StsColors.transparentBlack;
      child.Scale = NUnlockPotionsScreen._potionScale;
      NPotion potion2 = NPotion.Create(potion1.ToMutable());
      child.AddPotion(potion2);
      potion2.Position = Vector2.Zero;
      this._potionTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(this.Position, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 350f), (float) num))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._potionTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
      ++num;
    }
    ActiveScreenContext.Instance.FocusOnDefaultControl();
  }

  public void SetPotions(IReadOnlyList<PotionModel> potions) => this._potions = potions;

  protected override void OnScreenClose() => NTimelineScreen.Instance.EnableInput();

  public override Control? DefaultFocusedControl
  {
    get
    {
      return ((Node) this._potionRow).GetChildCount(false) != 0 ? ((Node) this._potionRow).GetChild<Control>(((Node) this._potionRow).GetChildCount(false) / 2, false) : (Control) null;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NUnlockPotionsScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockPotionsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockPotionsScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockPotionsScreen.MethodName.OnScreenClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockPotionsScreen nunlockPotionsScreen = NUnlockPotionsScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockPotionsScreen>(ref nunlockPotionsScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.OnScreenClose) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockPotionsScreen nunlockPotionsScreen = NUnlockPotionsScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockPotionsScreen>(ref nunlockPotionsScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.Create) || StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.Open) || StringName.op_Equality(ref method, NUnlockPotionsScreen.MethodName.OnScreenClose) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName._potionRow))
    {
      this._potionRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName._potionTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._potionTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName._potionRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockPotionsScreen.PropertyName._potionTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._potionTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockPotionsScreen.PropertyName._potionRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockPotionsScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockPotionsScreen.PropertyName._potionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockPotionsScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockPotionsScreen.PropertyName._potionRow, Variant.From<Control>(ref this._potionRow));
    info.AddProperty(NUnlockPotionsScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NUnlockPotionsScreen.PropertyName._potionTween, Variant.From<Tween>(ref this._potionTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockPotionsScreen.PropertyName._potionRow, ref variant1))
      this._potionRow = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NUnlockPotionsScreen.PropertyName._banner, ref variant2))
      this._banner = ((Variant) ref variant2).As<NCommonBanner>();
    Variant variant3;
    if (!info.TryGetProperty(NUnlockPotionsScreen.PropertyName._potionTween, ref variant3))
      return;
    this._potionTween = ((Variant) ref variant3).As<Tween>();
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
    public static readonly StringName _potionRow = StringName.op_Implicit(nameof (_potionRow));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _potionTween = StringName.op_Implicit(nameof (_potionTween));
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
