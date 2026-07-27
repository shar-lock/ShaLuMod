// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens.NUnlockCardsScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline.UnlockScreens;

[ScriptPath("res://src/Core/Nodes/Screens/Timeline/UnlockScreens/NUnlockCardsScreen.cs")]
public class NUnlockCardsScreen : NUnlockScreen
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("timeline_screen/unlock_cards_screen");
  private Control _cardRow;
  private NCommonBanner _banner;
  private readonly List<NGridCardHolder> _holders = new List<NGridCardHolder>();
  private IReadOnlyList<CardModel> _cards;
  private Tween? _cardTween;
  private const float _cardXOffset = 350f;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NUnlockCardsScreen._scenePath);
    }
  }

  public static NUnlockCardsScreen Create()
  {
    return PreloadManager.Cache.GetScene(NUnlockCardsScreen._scenePath).Instantiate<NUnlockCardsScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("%Banner"));
    this._banner.label.SetTextAutoSize(new LocString("timeline", "UNLOCK_CARDS_BANNER").GetRawText());
    this._banner.AnimateIn();
    this._cardRow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardRow"));
    LocString locString = new LocString("timeline", "UNLOCK_CARDS");
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ExplanationText")).Text = $"[center]{locString.GetFormattedText()}[/center]";
  }

  public override void Open()
  {
    base.Open();
    SfxCmd.Play("event:/sfx/ui/timeline/ui_timeline_unlock");
    Vector2 vector2 = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Left, (float) (this._cards.Count - 1)), 350f), 0.5f);
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    int num = 0;
    foreach (CardModel card in (IEnumerable<CardModel>) this._cards)
    {
      NCard cardNode = NCard.Create(card);
      NGridCardHolder child = NGridCardHolder.Create(cardNode);
      ((Node) this._cardRow).AddChildSafely((Node) child);
      cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      child.Scale = child.SmallScale;
      this._cardTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(Vector2.op_Addition(child.Position, vector2), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, 350f), (float) num))), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      this._cardTween.TweenProperty((GodotObject) child, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Colors.Black));
      cardNode.ActivateRewardScreenGlow();
      this._holders.Add(child);
      ++num;
    }
    ActiveScreenContext.Instance.FocusOnDefaultControl();
  }

  public void SetCards(IReadOnlyList<CardModel> cards) => this._cards = cards;

  protected override void OnScreenPreClose()
  {
    foreach (NCardHolder holder in this._holders)
      holder.CardNode?.KillRarityGlow();
  }

  protected override void OnScreenClose() => NTimelineScreen.Instance.EnableInput();

  public override void _ExitTree()
  {
    foreach (Node node in ((IEnumerable) ((Node) this._cardRow).GetChildren(false)).OfType<NGridCardHolder>())
      node.QueueFreeSafely();
  }

  public override Control? DefaultFocusedControl
  {
    get
    {
      return this._holders.Count != 0 ? (Control) this._holders[this._holders.Count / 2] : (Control) null;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NUnlockCardsScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCardsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCardsScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCardsScreen.MethodName.OnScreenPreClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCardsScreen.MethodName.OnScreenClose, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUnlockCardsScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockCardsScreen nunlockCardsScreen = NUnlockCardsScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockCardsScreen>(ref nunlockCardsScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.OnScreenPreClose) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenPreClose();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.OnScreenClose) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenClose();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NUnlockCardsScreen nunlockCardsScreen = NUnlockCardsScreen.Create();
      ret = VariantUtils.CreateFrom<NUnlockCardsScreen>(ref nunlockCardsScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.Create) || StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.Open) || StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.OnScreenPreClose) || StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName.OnScreenClose) || StringName.op_Equality(ref method, NUnlockCardsScreen.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName._cardRow))
    {
      this._cardRow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName._cardTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName._cardRow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardRow);
      return true;
    }
    if (StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUnlockCardsScreen.PropertyName._cardTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUnlockCardsScreen.PropertyName._cardRow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCardsScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCardsScreen.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUnlockCardsScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NUnlockCardsScreen.PropertyName._cardRow, Variant.From<Control>(ref this._cardRow));
    info.AddProperty(NUnlockCardsScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NUnlockCardsScreen.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUnlockCardsScreen.PropertyName._cardRow, ref variant1))
      this._cardRow = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NUnlockCardsScreen.PropertyName._banner, ref variant2))
      this._banner = ((Variant) ref variant2).As<NCommonBanner>();
    Variant variant3;
    if (!info.TryGetProperty(NUnlockCardsScreen.PropertyName._cardTween, ref variant3))
      return;
    this._cardTween = ((Variant) ref variant3).As<Tween>();
  }

  public new class MethodName : NUnlockScreen.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public new static readonly StringName OnScreenPreClose = StringName.op_Implicit(nameof (OnScreenPreClose));
    public new static readonly StringName OnScreenClose = StringName.op_Implicit(nameof (OnScreenClose));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NUnlockScreen.PropertyName
  {
    public new static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _cardRow = StringName.op_Implicit(nameof (_cardRow));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
  }

  public new class SignalName : NUnlockScreen.SignalName
  {
  }
}
