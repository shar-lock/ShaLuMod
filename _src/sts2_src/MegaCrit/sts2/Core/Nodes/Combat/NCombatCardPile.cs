// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCombatCardPile
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCombatCardPile.cs")]
public abstract class NCombatCardPile : NButton
{
  private CardPile? _pile;
  private Player? _localPlayer;
  private MegaLabel _countLabel;
  private Control _icon;
  protected LocString _emptyPileMessage;
  private Tween? _bumpTween;
  private int _currentCount;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.25f);
  private const double _unhoverAnimDur = 0.5;
  private const double _pressDownDur = 0.25;
  private static readonly Color _downColor = Colors.DarkGray;
  private Tween? _positionTween;
  private const double _animDuration = 0.5;
  protected Vector2 _showPosition = new Vector2(100f, 828f);
  protected Vector2 _hidePosition = new Vector2(-160f, 860f);

  protected abstract PileType Pile { get; }

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NCombatCardPile))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this._icon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Icon"));
    this._countLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("CountContainer/Count"));
    this.SetAnimInOutPositions();
  }

  public override void _EnterTree()
  {
    base._EnterTree();
    if (this._pile == null)
      return;
    this._pile.CardAddFinished -= new Action(this.AddCard);
    this._pile.CardAddFinished += new Action(this.AddCard);
    this._pile.CardRemoveFinished -= new Action(this.RemoveCard);
    this._pile.CardRemoveFinished += new Action(this.RemoveCard);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    if (this._pile != null)
    {
      this._pile.CardAddFinished -= new Action(this.AddCard);
      this._pile.CardRemoveFinished -= new Action(this.RemoveCard);
    }
    this._positionTween?.Kill();
    this._bumpTween?.Kill();
  }

  public virtual void Initialize(Player player)
  {
    this._localPlayer = player;
    this._pile = this.Pile.GetPile(this._localPlayer);
    this._pile.CardAddFinished += new Action(this.AddCard);
    this._pile.CardRemoveFinished += new Action(this.RemoveCard);
    this._currentCount = this._pile.Cards.Count;
    this._countLabel.SetTextAutoSize(this._currentCount.ToString());
  }

  protected virtual void SetAnimInOutPositions()
  {
  }

  protected override void OnRelease()
  {
    this._bumpTween?.Kill();
    this._bumpTween = ((Node) this).CreateTween();
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.IsFocused ? NCombatCardPile._hoverScale : Vector2.One), 0.05);
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (this._pile == null || this._localPlayer == null || !CombatManager.Instance.IsInProgress)
      return;
    if (NTargetManager.Instance.IsInSelection)
      NTargetManager.Instance.CancelTargeting();
    if (this._pile.IsEmpty)
    {
      NCapstoneContainer instance1 = NCapstoneContainer.Instance;
      if ((instance1 != null ? (instance1.InUse ? 1 : 0) : 0) != 0)
        NCapstoneContainer.Instance.Close();
      NThoughtBubbleVfx child = NThoughtBubbleVfx.Create(this._emptyPileMessage.GetFormattedText(), this._localPlayer.Creature, new double?(2.0));
      NCombatRoom instance2 = NCombatRoom.Instance;
      if (instance2 == null)
        return;
      ((Node) instance2.CombatVfxContainer).AddChildSafely((Node) child);
    }
    else if (NCapstoneContainer.Instance?.CurrentCapstoneScreen is NCardPileScreen currentCapstoneScreen && currentCapstoneScreen.Pile == this._pile)
      NCapstoneContainer.Instance.Close();
    else
      NCardPileScreen.ShowScreen(this._pile, this.Hotkeys);
  }

  protected override void OnFocus()
  {
    if (this._pile == null)
      return;
    LocString title;
    LocString description;
    switch (this._pile.Type)
    {
      case PileType.Draw:
        title = new LocString("static_hover_tips", "DRAW_PILE.title");
        description = new LocString("static_hover_tips", "DRAW_PILE.description");
        break;
      case PileType.Discard:
        title = new LocString("static_hover_tips", "DISCARD_PILE.title");
        description = new LocString("static_hover_tips", "DISCARD_PILE.description");
        break;
      case PileType.Exhaust:
        title = new LocString("static_hover_tips", "EXHAUST_PILE.title");
        description = new LocString("static_hover_tips", "EXHAUST_PILE.description");
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    string str = ((IEnumerable<string>) this.Hotkeys).FirstOrDefault<string>();
    Key? nullable = new Key?();
    if (str != null)
      nullable = new Key?(NInputManager.Instance.GetShortcutKey(StringName.op_Implicit(str)));
    if (nullable.HasValue)
      title.Add("Hotkey", nullable.Value.ToString());
    else
      title.Add("Hotkey", "None");
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(title, description));
    if (andShow != null)
    {
      NHoverTipSet nhoverTipSet = andShow;
      Vector2 vector2;
      switch (this._pile.Type)
      {
        case PileType.Draw:
          vector2 = Vector2.op_Addition(this.GlobalPosition, new Vector2(14f, -375f));
          break;
        case PileType.Discard:
          vector2 = Vector2.op_Addition(this.GlobalPosition, new Vector2(-320f, -370f));
          break;
        case PileType.Exhaust:
          vector2 = Vector2.op_Addition(this.GlobalPosition, new Vector2(-320f, -125f));
          break;
        default:
          vector2 = andShow.GlobalPosition;
          break;
      }
      nhoverTipSet.GlobalPosition = vector2;
    }
    this._bumpTween?.Kill();
    this._bumpTween = ((Node) this).CreateTween();
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(NCombatCardPile._hoverScale), 0.05);
  }

  protected override void OnUnfocus()
  {
    NHoverTipSet.Remove((Control) this);
    this._bumpTween?.Kill();
    this._bumpTween = ((Node) this).CreateTween().SetParallel(true);
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    this._bumpTween?.Kill();
    this._bumpTween = ((Node) this).CreateTween().SetParallel(true);
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("modulate"), Variant.op_Implicit(NCombatCardPile._downColor), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  protected virtual void AddCard()
  {
    if (this._pile == null)
      return;
    this._currentCount = Math.Min(this._currentCount + 1, this._pile.Cards.Count);
    this._countLabel.SetTextAutoSize(this._currentCount.ToString());
    ((Control) this._countLabel).PivotOffset = Vector2.op_Multiply(((Control) this._countLabel).Size, 0.5f);
    this._bumpTween?.Kill();
    this._bumpTween = ((Node) this).CreateTween().SetParallel(true);
    this._icon.Scale = NCombatCardPile._hoverScale;
    this._bumpTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    ((Control) this._countLabel).Scale = NCombatCardPile._hoverScale;
    this._bumpTween.TweenProperty((GodotObject) this._countLabel, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void RemoveCard()
  {
    if (this._pile == null)
      return;
    this._currentCount = Math.Max(this._currentCount - 1, this._pile.Cards.Count);
    this._countLabel.SetTextAutoSize(this._currentCount.ToString());
    ((Control) this._countLabel).PivotOffset = Vector2.op_Multiply(((Control) this._countLabel).Size, 0.5f);
  }

  public virtual void AnimIn()
  {
    this.Position = this._hidePosition;
    this._positionTween?.Kill();
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._showPosition), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void AnimOut()
  {
    this.Position = this._showPosition;
    this._positionTween?.Kill();
    this._positionTween = ((Node) this).CreateTween();
    this._positionTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._hidePosition), 0.5).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NCombatCardPile.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.SetAnimInOutPositions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.AddCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.RemoveCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatCardPile.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.SetAnimInOutPositions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetAnimInOutPositions();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.AddCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AddCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.RemoveCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RemoveCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatCardPile.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatCardPile.MethodName.AnimOut) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AnimOut();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatCardPile.MethodName._Ready) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NCombatCardPile.MethodName._EnterTree) || StringName.op_Equality(ref method, NCombatCardPile.MethodName._ExitTree) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.SetAnimInOutPositions) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnRelease) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnFocus) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.OnPress) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.AddCard) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.RemoveCard) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.AnimIn) || StringName.op_Equality(ref method, NCombatCardPile.MethodName.AnimOut) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._countLabel))
    {
      this._countLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._bumpTween))
    {
      this._bumpTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._currentCount))
    {
      this._currentCount = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._positionTween))
    {
      this._positionTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._showPosition))
    {
      this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatCardPile.PropertyName._hidePosition))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hidePosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName.Pile))
    {
      ref godot_variant local = ref value;
      PileType pile = this.Pile;
      godot_variant from = VariantUtils.CreateFrom<PileType>(ref pile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._countLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._countLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._bumpTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._bumpTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._currentCount))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentCount);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._positionTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._positionTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatCardPile.PropertyName._showPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatCardPile.PropertyName._hidePosition))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._hidePosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NCombatCardPile.PropertyName.Pile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatCardPile.PropertyName._countLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatCardPile.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatCardPile.PropertyName._bumpTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCombatCardPile.PropertyName._currentCount, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatCardPile.PropertyName._positionTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCombatCardPile.PropertyName._showPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCombatCardPile.PropertyName._hidePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCombatCardPile.PropertyName._countLabel, Variant.From<MegaLabel>(ref this._countLabel));
    info.AddProperty(NCombatCardPile.PropertyName._icon, Variant.From<Control>(ref this._icon));
    info.AddProperty(NCombatCardPile.PropertyName._bumpTween, Variant.From<Tween>(ref this._bumpTween));
    info.AddProperty(NCombatCardPile.PropertyName._currentCount, Variant.From<int>(ref this._currentCount));
    info.AddProperty(NCombatCardPile.PropertyName._positionTween, Variant.From<Tween>(ref this._positionTween));
    info.AddProperty(NCombatCardPile.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
    info.AddProperty(NCombatCardPile.PropertyName._hidePosition, Variant.From<Vector2>(ref this._hidePosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatCardPile.PropertyName._countLabel, ref variant1))
      this._countLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NCombatCardPile.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCombatCardPile.PropertyName._bumpTween, ref variant3))
      this._bumpTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NCombatCardPile.PropertyName._currentCount, ref variant4))
      this._currentCount = ((Variant) ref variant4).As<int>();
    Variant variant5;
    if (info.TryGetProperty(NCombatCardPile.PropertyName._positionTween, ref variant5))
      this._positionTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NCombatCardPile.PropertyName._showPosition, ref variant6))
      this._showPosition = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (!info.TryGetProperty(NCombatCardPile.PropertyName._hidePosition, ref variant7))
      return;
    this._hidePosition = ((Variant) ref variant7).As<Vector2>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public new static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetAnimInOutPositions = StringName.op_Implicit(nameof (SetAnimInOutPositions));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName AddCard = StringName.op_Implicit(nameof (AddCard));
    public static readonly StringName RemoveCard = StringName.op_Implicit(nameof (RemoveCard));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName Pile = StringName.op_Implicit(nameof (Pile));
    public static readonly StringName _countLabel = StringName.op_Implicit(nameof (_countLabel));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _bumpTween = StringName.op_Implicit(nameof (_bumpTween));
    public static readonly StringName _currentCount = StringName.op_Implicit(nameof (_currentCount));
    public static readonly StringName _positionTween = StringName.op_Implicit(nameof (_positionTween));
    public static readonly StringName _showPosition = StringName.op_Implicit(nameof (_showPosition));
    public static readonly StringName _hidePosition = StringName.op_Implicit(nameof (_hidePosition));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
