// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NHealthBar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NHealthBar.cs")]
public class NHealthBar : Control
{
  private Control _hpForegroundContainer;
  private Control _hpForeground;
  private Control _poisonForeground;
  private Control _doomForeground;
  private Control _hpMiddleground;
  private MegaLabel _hpLabel;
  private Control _blockContainer;
  private MegaLabel _blockLabel;
  private Control _blockOutline;
  private Creature _creature;
  private Creature? _blockTrackingCreature;
  private readonly LocString _healthBarDead = new LocString("gameplay_ui", "HEALTH_BAR.DEAD");
  private TextureRect _infinityTex;
  private Tween? _blockTween;
  private Tween? _hpLabelFadeTween;
  private Tween? _middlegroundTween;
  private Vector2 _originalBlockPosition;
  private int _currentHpOnLastRefresh = -1;
  private int _maxHpOnLastRefresh = -1;
  private float _expectedMaxFgWidth = -1f;
  private const float _minSize = 12f;
  private static readonly Vector2 _blockAnimOffset = new Vector2(0.0f, 20f);
  private static readonly Color _defaultFontColor = StsColors.cream;
  private static readonly Color _defaultFontOutlineColor = new Color("900000");
  private static readonly Color _blockOutlineColor = new Color("1B3045");
  private static readonly Color _invincibleOutlineColor = new Color("4C4370");
  private static readonly Color _redForegroundColor = new Color("F1373E");
  private static readonly Color _blockHpForegroundColor = new Color("3B6FA3");
  private static readonly Color _invincibleForegroundColor = new Color("C5BBED");
  private const float _foregroundContainerInset = 10f;

  private float MaxFgWidth
  {
    get
    {
      return (double) this._expectedMaxFgWidth <= 0.0 ? this._hpForegroundContainer.Size.X : this._expectedMaxFgWidth;
    }
  }

  public Control HpBarContainer { get; private set; }

  public void SetCreature(Creature creature)
  {
    this._creature = this._creature == null ? creature : throw new InvalidOperationException("Creature was already set.");
    this._hpForeground.OffsetRight = this.GetFgWidth(this._creature.CurrentHp) - this.MaxFgWidth;
    this._hpMiddleground.OffsetRight = this._hpForeground.OffsetRight - 2f;
  }

  public override void _Ready()
  {
    this.HpBarContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%HpBarContainer"));
    this._hpForegroundContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%HpForegroundContainer"));
    this._hpMiddleground = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%HpMiddleground"));
    this._hpForeground = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%HpForeground"));
    this._poisonForeground = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PoisonForeground"));
    this._doomForeground = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DoomForeground"));
    this._hpLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%HpLabel"));
    this._blockContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BlockContainer"));
    this._blockLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%BlockLabel"));
    this._blockOutline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BlockOutline"));
    this._infinityTex = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%InfinityTex"));
    this._originalBlockPosition = this._blockContainer.Position;
  }

  private void DebugToggleVisibility()
  {
    ((CanvasItem) this).Visible = !NCombatUi.IsDebugHidingHpBar;
  }

  public void UpdateLayoutForCreatureBounds(Control bounds)
  {
    float num1 = 24f;
    float? barSizeReduction = this._creature.Monster?.HpBarSizeReduction;
    float valueOrDefault = (barSizeReduction.HasValue ? new float?(num1 - barSizeReduction.GetValueOrDefault()) : new float?()).GetValueOrDefault();
    this.HpBarContainer.GlobalPosition = new Vector2(bounds.GlobalPosition.X - valueOrDefault * 0.5f, this.HpBarContainer.GlobalPosition.Y);
    this.SetHpBarContainerSizeWithOffsets(new Vector2(bounds.Size.X + valueOrDefault, this.HpBarContainer.Size.Y));
    float num2 = this._blockContainer.Size.X * 0.5f;
    this._blockContainer.GlobalPosition = new Vector2(bounds.GlobalPosition.X - num2, this._blockContainer.GlobalPosition.Y);
    this._originalBlockPosition = this._blockContainer.Position;
  }

  public void UpdateWidthRelativeToReferenceValue(float refMaxHp, float refWidth)
  {
    Vector2 size = this.HpBarContainer.Size;
    size.X = (float) this._creature.MaxHp / refMaxHp * refWidth;
    this.SetHpBarContainerSizeWithOffsetsImmediately(size);
  }

  private void SetHpBarContainerSizeWithOffsets(Vector2 size)
  {
    Callable callable = Callable.From((Action) (() => this.SetHpBarContainerSizeWithOffsetsImmediately(size)));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  private void SetHpBarContainerSizeWithOffsetsImmediately(Vector2 size)
  {
    Vector2 size1 = this.HpBarContainer.Size;
    if (((Vector2) ref size1).IsEqualApprox(size))
      return;
    this._middlegroundTween?.Kill();
    this.HpBarContainer.Size = size;
    this._expectedMaxFgWidth = size.X - 10f;
    this._hpForeground.OffsetRight = this.GetFgWidth(this._creature.CurrentHp, this._expectedMaxFgWidth) - this._expectedMaxFgWidth;
    this._hpMiddleground.OffsetRight = this._hpForeground.OffsetRight - 2f;
  }

  public void RefreshValues()
  {
    this.RefreshBlockUi();
    this.RefreshForeground();
    this.RefreshMiddleground();
    this.RefreshText();
  }

  private void RefreshMiddleground()
  {
    if (this._creature.CurrentHp <= 0)
    {
      ((CanvasItem) this._hpMiddleground).Visible = false;
    }
    else
    {
      ((CanvasItem) this._hpMiddleground).Visible = true;
      Control hpMiddleground = this._hpMiddleground;
      Vector2 position = this._hpMiddleground.Position;
      position.X = 1f;
      Vector2 vector2 = position;
      hpMiddleground.Position = vector2;
      int currentHp = this._creature.CurrentHp;
      int maxHp = this._creature.MaxHp;
      if (currentHp == this._currentHpOnLastRefresh && maxHp == this._maxHpOnLastRefresh)
        return;
      this._currentHpOnLastRefresh = currentHp;
      this._maxHpOnLastRefresh = maxHp;
      float num = ((CanvasItem) this._poisonForeground).Visible ? this._poisonForeground.OffsetRight : this._hpForeground.OffsetRight;
      bool flag = (double) num >= (double) this._hpMiddleground.OffsetRight;
      ++this._hpMiddleground.OffsetRight;
      this._middlegroundTween?.Kill();
      this._middlegroundTween = ((Node) this).CreateTween();
      this._middlegroundTween.TweenProperty((GodotObject) this._hpMiddleground, NodePath.op_Implicit("offset_right"), Variant.op_Implicit(num - 2f), 1.0).SetDelay(flag ? 0.0 : 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
  }

  private void RefreshForeground()
  {
    if (this._creature.CurrentHp <= 0)
    {
      ((CanvasItem) this._poisonForeground).Visible = false;
      ((CanvasItem) this._doomForeground).Visible = false;
      ((CanvasItem) this._hpForeground).Visible = false;
    }
    else
    {
      ((CanvasItem) this._hpForeground).Visible = true;
      float num1 = this.GetFgWidth(this._creature.CurrentHp) - this.MaxFgWidth;
      this._hpForeground.OffsetRight = num1;
      if (this._creature.HpDisplay.IsInfinite())
      {
        ((CanvasItem) this._hpForeground).SelfModulate = NHealthBar._invincibleForegroundColor;
        ((CanvasItem) this._poisonForeground).Visible = false;
        ((CanvasItem) this._doomForeground).Visible = false;
      }
      else
      {
        int powerAmount = this._creature.GetPowerAmount<DoomPower>();
        PoisonPower power = this._creature.GetPower<PoisonPower>();
        int totalDamageNextTurn = power != null ? power.CalculateTotalDamageNextTurn() : 0;
        if (this._creature.HasPower<PoisonPower>())
        {
          if (totalDamageNextTurn > 0)
          {
            ((CanvasItem) this._poisonForeground).Visible = true;
            if (this.IsPoisonLethal(totalDamageNextTurn))
            {
              this._poisonForeground.OffsetLeft = 0.0f;
              this._poisonForeground.OffsetRight = num1;
              ((CanvasItem) this._hpForeground).Visible = false;
            }
            else
            {
              float fgWidth = this.GetFgWidth(this._creature.CurrentHp - totalDamageNextTurn);
              this._hpForeground.OffsetRight = fgWidth - this.MaxFgWidth;
              ((CanvasItem) this._hpForeground).Visible = true;
              int patchMarginLeft = ((NinePatchRect) this._poisonForeground).PatchMarginLeft;
              this._poisonForeground.OffsetLeft = Math.Max(0.0f, fgWidth - (float) patchMarginLeft);
              this._poisonForeground.OffsetRight = num1;
            }
          }
          else
            ((CanvasItem) this._poisonForeground).Visible = false;
        }
        else
        {
          ((CanvasItem) this._poisonForeground).Visible = false;
          this._poisonForeground.OffsetLeft = 0.0f;
        }
        if (this._creature.HasPower<DoomPower>())
        {
          if (powerAmount > 0)
          {
            ((CanvasItem) this._doomForeground).Visible = true;
            float num2 = this.GetFgWidth(powerAmount) - this.MaxFgWidth;
            if (this.IsDoomLethal(powerAmount, totalDamageNextTurn))
            {
              if (!this.IsPoisonLethal(totalDamageNextTurn))
              {
                this._doomForeground.OffsetRight = this._hpForeground.OffsetRight;
                ((CanvasItem) this._hpForeground).Visible = false;
              }
              else
              {
                ((CanvasItem) this._hpForeground).Visible = false;
                ((CanvasItem) this._doomForeground).Visible = false;
              }
            }
            else
            {
              int patchMarginRight = ((NinePatchRect) this._doomForeground).PatchMarginRight;
              this._doomForeground.OffsetRight = Math.Min(0.0f, num2 + (float) patchMarginRight);
              ((CanvasItem) this._hpForeground).Visible = true;
            }
          }
          else
            ((CanvasItem) this._doomForeground).Visible = false;
        }
        else
          ((CanvasItem) this._doomForeground).Visible = false;
      }
    }
  }

  private void RefreshBlockUi()
  {
    if (this._creature.Block <= 0)
    {
      Creature trackingCreature = this._blockTrackingCreature;
      if (trackingCreature == null || trackingCreature.Block <= 0)
      {
        if (((CanvasItem) this._blockContainer).Visible)
        {
          NBlockBrokenVfx child = NBlockBrokenVfx.Create();
          if (child != null)
          {
            ((Node) this).AddChildSafely((Node) child);
            ((Node2D) child).GlobalPosition = Vector2.op_Addition(this._blockContainer.GlobalPosition, Vector2.op_Multiply(this._blockContainer.Size, 0.5f));
          }
        }
        ((CanvasItem) this._blockContainer).Visible = false;
        ((CanvasItem) this._blockOutline).Visible = false;
        ((CanvasItem) this._hpForeground).SelfModulate = NHealthBar._redForegroundColor;
        return;
      }
    }
    ((CanvasItem) this._blockOutline).Visible = true;
    ((CanvasItem) this._hpForeground).SelfModulate = NHealthBar._blockHpForegroundColor;
    if (this._creature.Block <= 0)
      return;
    ((CanvasItem) this._blockContainer).Visible = true;
    this._blockLabel.SetTextAutoSize(this._creature.Block.ToString());
  }

  private void RefreshText()
  {
    if (this._creature.CurrentHp <= 0)
    {
      ((Control) this._hpLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, NHealthBar._defaultFontColor);
      ((Control) this._hpLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, NHealthBar._defaultFontOutlineColor);
      this._hpLabel.SetTextAutoSize(this._healthBarDead.GetRawText());
    }
    else
    {
      ((CanvasItem) this._doomForeground).Modulate = this._creature.HpDisplay.IsInfinite() ? Colors.Transparent : Colors.White;
      ((CanvasItem) this._infinityTex).Visible = this._creature.HpDisplay == HpDisplay.InfiniteWithoutNumbers;
      ((CanvasItem) this._hpLabel).Visible = this._creature.HpDisplay.ShowsNumbers();
      if (!this._creature.HpDisplay.ShowsNumbers())
        return;
      PoisonPower power = this._creature.GetPower<PoisonPower>();
      int totalDamageNextTurn = power != null ? power.CalculateTotalDamageNextTurn() : 0;
      int powerAmount = this._creature.GetPowerAmount<DoomPower>();
      Color defaultFontColor;
      Color color;
      if (this._creature.HpDisplay == HpDisplay.InfiniteWithNumbers)
      {
        defaultFontColor = NHealthBar._defaultFontColor;
        color = NHealthBar._invincibleOutlineColor;
      }
      else if (this.IsPoisonLethal(totalDamageNextTurn))
      {
        // ISSUE: explicit constructor call
        ((Color) ref defaultFontColor).\u002Ector("76FF40");
        // ISSUE: explicit constructor call
        ((Color) ref color).\u002Ector("074700");
      }
      else if (this.IsDoomLethal(powerAmount, totalDamageNextTurn))
      {
        // ISSUE: explicit constructor call
        ((Color) ref defaultFontColor).\u002Ector("FB8DFF");
        // ISSUE: explicit constructor call
        ((Color) ref color).\u002Ector("2D1263");
      }
      else
      {
        if (this._creature.Block <= 0)
        {
          Creature trackingCreature = this._blockTrackingCreature;
          if (trackingCreature == null || trackingCreature.Block <= 0)
          {
            defaultFontColor = NHealthBar._defaultFontColor;
            color = NHealthBar._defaultFontOutlineColor;
            goto label_13;
          }
        }
        defaultFontColor = NHealthBar._defaultFontColor;
        color = NHealthBar._blockOutlineColor;
      }
label_13:
      ((Control) this._hpLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, defaultFontColor);
      ((Control) this._hpLabel).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, color);
      this._hpLabel.SetTextAutoSize($"{this._creature.CurrentHp}/{this._creature.MaxHp}");
    }
  }

  private bool IsPoisonLethal(int poisonDamage)
  {
    return poisonDamage > 0 && this._creature.HasPower<PoisonPower>() && poisonDamage >= this._creature.CurrentHp;
  }

  private bool IsDoomLethal(int doomAmount, int poisonDamage)
  {
    return doomAmount > 0 && this._creature.HasPower<DoomPower>() && doomAmount >= this._creature.CurrentHp - poisonDamage;
  }

  private float GetFgWidth(int amount) => this.GetFgWidth(amount, this.MaxFgWidth);

  private float GetFgWidth(int amount, float maxFgWidth)
  {
    return this._creature.MaxHp <= 0 ? 0.0f : Math.Max((float) amount / (float) this._creature.MaxHp * maxFgWidth, this._creature.CurrentHp > 0 ? 12f : 0.0f);
  }

  public void FadeOutHpLabel(float duration, float finalAlpha)
  {
    this._hpLabelFadeTween?.Kill();
    this._hpLabelFadeTween = ((Node) this).CreateTween();
    this._hpLabelFadeTween.TweenProperty((GodotObject) this._hpLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(finalAlpha), (double) duration).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void FadeInHpLabel(float duration)
  {
    this._hpLabelFadeTween?.Kill();
    this._hpLabelFadeTween = ((Node) this).CreateTween();
    this._hpLabelFadeTween.TweenProperty((GodotObject) this._hpLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), (double) duration);
  }

  public void AnimateInBlock(int oldBlock, int blockGain)
  {
    if (oldBlock != 0 || blockGain == 0)
      return;
    ((CanvasItem) this._blockContainer).Visible = true;
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
      return;
    ((CanvasItem) this._blockContainer).Modulate = StsColors.transparentWhite;
    this._blockContainer.Position = Vector2.op_Subtraction(this._originalBlockPosition, NHealthBar._blockAnimOffset);
    this._blockTween?.Kill();
    this._blockTween = ((Node) this).CreateTween().SetParallel(true);
    this._blockTween.TweenProperty((GodotObject) this._blockContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._blockTween.TweenProperty((GodotObject) this._blockContainer, NodePath.op_Implicit("position"), Variant.op_Implicit(this._originalBlockPosition), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
  }

  public void TrackBlockStatus(Creature creature) => this._blockTrackingCreature = creature;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NHealthBar.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.DebugToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.UpdateLayoutForCreatureBounds, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("bounds"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.UpdateWidthRelativeToReferenceValue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("refMaxHp"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("refWidth"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.SetHpBarContainerSizeWithOffsets, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.SetHpBarContainerSizeWithOffsetsImmediately, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.RefreshValues, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.RefreshMiddleground, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.RefreshForeground, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.RefreshBlockUi, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.RefreshText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.IsPoisonLethal, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("poisonDamage"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.IsDoomLethal, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("doomAmount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("poisonDamage"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.GetFgWidth, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("amount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.GetFgWidth, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("amount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("maxFgWidth"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.FadeOutHpLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("finalAlpha"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.FadeInHpLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHealthBar.MethodName.AnimateInBlock, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldBlock"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("blockGain"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHealthBar.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.DebugToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.UpdateLayoutForCreatureBounds) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateLayoutForCreatureBounds(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.UpdateWidthRelativeToReferenceValue) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.UpdateWidthRelativeToReferenceValue(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.SetHpBarContainerSizeWithOffsets) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetHpBarContainerSizeWithOffsets(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.SetHpBarContainerSizeWithOffsetsImmediately) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetHpBarContainerSizeWithOffsetsImmediately(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshValues) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshValues();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshMiddleground) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshMiddleground();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshForeground) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshForeground();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshBlockUi) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshBlockUi();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.IsPoisonLethal) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      bool flag = this.IsPoisonLethal(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.IsDoomLethal) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      bool flag = this.IsDoomLethal(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.GetFgWidth) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      float fgWidth = this.GetFgWidth(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<float>(ref fgWidth);
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.GetFgWidth) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      float fgWidth = this.GetFgWidth(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<float>(ref fgWidth);
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.FadeOutHpLabel) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.FadeOutHpLabel(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHealthBar.MethodName.FadeInHpLabel) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.FadeInHpLabel(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHealthBar.MethodName.AnimateInBlock) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AnimateInBlock(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHealthBar.MethodName._Ready) || StringName.op_Equality(ref method, NHealthBar.MethodName.DebugToggleVisibility) || StringName.op_Equality(ref method, NHealthBar.MethodName.UpdateLayoutForCreatureBounds) || StringName.op_Equality(ref method, NHealthBar.MethodName.UpdateWidthRelativeToReferenceValue) || StringName.op_Equality(ref method, NHealthBar.MethodName.SetHpBarContainerSizeWithOffsets) || StringName.op_Equality(ref method, NHealthBar.MethodName.SetHpBarContainerSizeWithOffsetsImmediately) || StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshValues) || StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshMiddleground) || StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshForeground) || StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshBlockUi) || StringName.op_Equality(ref method, NHealthBar.MethodName.RefreshText) || StringName.op_Equality(ref method, NHealthBar.MethodName.IsPoisonLethal) || StringName.op_Equality(ref method, NHealthBar.MethodName.IsDoomLethal) || StringName.op_Equality(ref method, NHealthBar.MethodName.GetFgWidth) || StringName.op_Equality(ref method, NHealthBar.MethodName.FadeOutHpLabel) || StringName.op_Equality(ref method, NHealthBar.MethodName.FadeInHpLabel) || StringName.op_Equality(ref method, NHealthBar.MethodName.AnimateInBlock) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName.HpBarContainer))
    {
      this.HpBarContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpForegroundContainer))
    {
      this._hpForegroundContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpForeground))
    {
      this._hpForeground = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._poisonForeground))
    {
      this._poisonForeground = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._doomForeground))
    {
      this._doomForeground = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpMiddleground))
    {
      this._hpMiddleground = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpLabel))
    {
      this._hpLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockContainer))
    {
      this._blockContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockLabel))
    {
      this._blockLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockOutline))
    {
      this._blockOutline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._infinityTex))
    {
      this._infinityTex = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockTween))
    {
      this._blockTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpLabelFadeTween))
    {
      this._hpLabelFadeTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._middlegroundTween))
    {
      this._middlegroundTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._originalBlockPosition))
    {
      this._originalBlockPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._currentHpOnLastRefresh))
    {
      this._currentHpOnLastRefresh = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._maxHpOnLastRefresh))
    {
      this._maxHpOnLastRefresh = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHealthBar.PropertyName._expectedMaxFgWidth))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._expectedMaxFgWidth = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName.MaxFgWidth))
    {
      ref godot_variant local = ref value;
      float maxFgWidth = this.MaxFgWidth;
      godot_variant from = VariantUtils.CreateFrom<float>(ref maxFgWidth);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName.HpBarContainer))
    {
      ref godot_variant local = ref value;
      Control hpBarContainer = this.HpBarContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref hpBarContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpForegroundContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hpForegroundContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpForeground))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hpForeground);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._poisonForeground))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._poisonForeground);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._doomForeground))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._doomForeground);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpMiddleground))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hpMiddleground);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._hpLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._blockContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._blockLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockOutline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._blockOutline);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._infinityTex))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._infinityTex);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._blockTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._blockTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._hpLabelFadeTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hpLabelFadeTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._middlegroundTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._middlegroundTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._originalBlockPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._originalBlockPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._currentHpOnLastRefresh))
    {
      value = VariantUtils.CreateFrom<int>(ref this._currentHpOnLastRefresh);
      return true;
    }
    if (StringName.op_Equality(ref name, NHealthBar.PropertyName._maxHpOnLastRefresh))
    {
      value = VariantUtils.CreateFrom<int>(ref this._maxHpOnLastRefresh);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHealthBar.PropertyName._expectedMaxFgWidth))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._expectedMaxFgWidth);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._hpForegroundContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._hpForeground, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._poisonForeground, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._doomForeground, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._hpMiddleground, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._hpLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._blockContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._blockLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._blockOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._infinityTex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._blockTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._hpLabelFadeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName._middlegroundTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHealthBar.PropertyName._originalBlockPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NHealthBar.PropertyName._currentHpOnLastRefresh, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NHealthBar.PropertyName._maxHpOnLastRefresh, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHealthBar.PropertyName._expectedMaxFgWidth, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NHealthBar.PropertyName.MaxFgWidth, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHealthBar.PropertyName.HpBarContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName hpBarContainer1 = NHealthBar.PropertyName.HpBarContainer;
    Control hpBarContainer2 = this.HpBarContainer;
    Variant variant = Variant.From<Control>(ref hpBarContainer2);
    serializationInfo.AddProperty(hpBarContainer1, variant);
    info.AddProperty(NHealthBar.PropertyName._hpForegroundContainer, Variant.From<Control>(ref this._hpForegroundContainer));
    info.AddProperty(NHealthBar.PropertyName._hpForeground, Variant.From<Control>(ref this._hpForeground));
    info.AddProperty(NHealthBar.PropertyName._poisonForeground, Variant.From<Control>(ref this._poisonForeground));
    info.AddProperty(NHealthBar.PropertyName._doomForeground, Variant.From<Control>(ref this._doomForeground));
    info.AddProperty(NHealthBar.PropertyName._hpMiddleground, Variant.From<Control>(ref this._hpMiddleground));
    info.AddProperty(NHealthBar.PropertyName._hpLabel, Variant.From<MegaLabel>(ref this._hpLabel));
    info.AddProperty(NHealthBar.PropertyName._blockContainer, Variant.From<Control>(ref this._blockContainer));
    info.AddProperty(NHealthBar.PropertyName._blockLabel, Variant.From<MegaLabel>(ref this._blockLabel));
    info.AddProperty(NHealthBar.PropertyName._blockOutline, Variant.From<Control>(ref this._blockOutline));
    info.AddProperty(NHealthBar.PropertyName._infinityTex, Variant.From<TextureRect>(ref this._infinityTex));
    info.AddProperty(NHealthBar.PropertyName._blockTween, Variant.From<Tween>(ref this._blockTween));
    info.AddProperty(NHealthBar.PropertyName._hpLabelFadeTween, Variant.From<Tween>(ref this._hpLabelFadeTween));
    info.AddProperty(NHealthBar.PropertyName._middlegroundTween, Variant.From<Tween>(ref this._middlegroundTween));
    info.AddProperty(NHealthBar.PropertyName._originalBlockPosition, Variant.From<Vector2>(ref this._originalBlockPosition));
    info.AddProperty(NHealthBar.PropertyName._currentHpOnLastRefresh, Variant.From<int>(ref this._currentHpOnLastRefresh));
    info.AddProperty(NHealthBar.PropertyName._maxHpOnLastRefresh, Variant.From<int>(ref this._maxHpOnLastRefresh));
    info.AddProperty(NHealthBar.PropertyName._expectedMaxFgWidth, Variant.From<float>(ref this._expectedMaxFgWidth));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHealthBar.PropertyName.HpBarContainer, ref variant1))
      this.HpBarContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NHealthBar.PropertyName._hpForegroundContainer, ref variant2))
      this._hpForegroundContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NHealthBar.PropertyName._hpForeground, ref variant3))
      this._hpForeground = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NHealthBar.PropertyName._poisonForeground, ref variant4))
      this._poisonForeground = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NHealthBar.PropertyName._doomForeground, ref variant5))
      this._doomForeground = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NHealthBar.PropertyName._hpMiddleground, ref variant6))
      this._hpMiddleground = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NHealthBar.PropertyName._hpLabel, ref variant7))
      this._hpLabel = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NHealthBar.PropertyName._blockContainer, ref variant8))
      this._blockContainer = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NHealthBar.PropertyName._blockLabel, ref variant9))
      this._blockLabel = ((Variant) ref variant9).As<MegaLabel>();
    Variant variant10;
    if (info.TryGetProperty(NHealthBar.PropertyName._blockOutline, ref variant10))
      this._blockOutline = ((Variant) ref variant10).As<Control>();
    Variant variant11;
    if (info.TryGetProperty(NHealthBar.PropertyName._infinityTex, ref variant11))
      this._infinityTex = ((Variant) ref variant11).As<TextureRect>();
    Variant variant12;
    if (info.TryGetProperty(NHealthBar.PropertyName._blockTween, ref variant12))
      this._blockTween = ((Variant) ref variant12).As<Tween>();
    Variant variant13;
    if (info.TryGetProperty(NHealthBar.PropertyName._hpLabelFadeTween, ref variant13))
      this._hpLabelFadeTween = ((Variant) ref variant13).As<Tween>();
    Variant variant14;
    if (info.TryGetProperty(NHealthBar.PropertyName._middlegroundTween, ref variant14))
      this._middlegroundTween = ((Variant) ref variant14).As<Tween>();
    Variant variant15;
    if (info.TryGetProperty(NHealthBar.PropertyName._originalBlockPosition, ref variant15))
      this._originalBlockPosition = ((Variant) ref variant15).As<Vector2>();
    Variant variant16;
    if (info.TryGetProperty(NHealthBar.PropertyName._currentHpOnLastRefresh, ref variant16))
      this._currentHpOnLastRefresh = ((Variant) ref variant16).As<int>();
    Variant variant17;
    if (info.TryGetProperty(NHealthBar.PropertyName._maxHpOnLastRefresh, ref variant17))
      this._maxHpOnLastRefresh = ((Variant) ref variant17).As<int>();
    Variant variant18;
    if (!info.TryGetProperty(NHealthBar.PropertyName._expectedMaxFgWidth, ref variant18))
      return;
    this._expectedMaxFgWidth = ((Variant) ref variant18).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName DebugToggleVisibility = StringName.op_Implicit(nameof (DebugToggleVisibility));
    public static readonly StringName UpdateLayoutForCreatureBounds = StringName.op_Implicit(nameof (UpdateLayoutForCreatureBounds));
    public static readonly StringName UpdateWidthRelativeToReferenceValue = StringName.op_Implicit(nameof (UpdateWidthRelativeToReferenceValue));
    public static readonly StringName SetHpBarContainerSizeWithOffsets = StringName.op_Implicit(nameof (SetHpBarContainerSizeWithOffsets));
    public static readonly StringName SetHpBarContainerSizeWithOffsetsImmediately = StringName.op_Implicit(nameof (SetHpBarContainerSizeWithOffsetsImmediately));
    public static readonly StringName RefreshValues = StringName.op_Implicit(nameof (RefreshValues));
    public static readonly StringName RefreshMiddleground = StringName.op_Implicit(nameof (RefreshMiddleground));
    public static readonly StringName RefreshForeground = StringName.op_Implicit(nameof (RefreshForeground));
    public static readonly StringName RefreshBlockUi = StringName.op_Implicit(nameof (RefreshBlockUi));
    public static readonly StringName RefreshText = StringName.op_Implicit(nameof (RefreshText));
    public static readonly StringName IsPoisonLethal = StringName.op_Implicit(nameof (IsPoisonLethal));
    public static readonly StringName IsDoomLethal = StringName.op_Implicit(nameof (IsDoomLethal));
    public static readonly StringName GetFgWidth = StringName.op_Implicit(nameof (GetFgWidth));
    public static readonly StringName FadeOutHpLabel = StringName.op_Implicit(nameof (FadeOutHpLabel));
    public static readonly StringName FadeInHpLabel = StringName.op_Implicit(nameof (FadeInHpLabel));
    public static readonly StringName AnimateInBlock = StringName.op_Implicit(nameof (AnimateInBlock));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName MaxFgWidth = StringName.op_Implicit(nameof (MaxFgWidth));
    public static readonly StringName HpBarContainer = StringName.op_Implicit(nameof (HpBarContainer));
    public static readonly StringName _hpForegroundContainer = StringName.op_Implicit(nameof (_hpForegroundContainer));
    public static readonly StringName _hpForeground = StringName.op_Implicit(nameof (_hpForeground));
    public static readonly StringName _poisonForeground = StringName.op_Implicit(nameof (_poisonForeground));
    public static readonly StringName _doomForeground = StringName.op_Implicit(nameof (_doomForeground));
    public static readonly StringName _hpMiddleground = StringName.op_Implicit(nameof (_hpMiddleground));
    public static readonly StringName _hpLabel = StringName.op_Implicit(nameof (_hpLabel));
    public static readonly StringName _blockContainer = StringName.op_Implicit(nameof (_blockContainer));
    public static readonly StringName _blockLabel = StringName.op_Implicit(nameof (_blockLabel));
    public static readonly StringName _blockOutline = StringName.op_Implicit(nameof (_blockOutline));
    public static readonly StringName _infinityTex = StringName.op_Implicit(nameof (_infinityTex));
    public static readonly StringName _blockTween = StringName.op_Implicit(nameof (_blockTween));
    public static readonly StringName _hpLabelFadeTween = StringName.op_Implicit(nameof (_hpLabelFadeTween));
    public static readonly StringName _middlegroundTween = StringName.op_Implicit(nameof (_middlegroundTween));
    public static readonly StringName _originalBlockPosition = StringName.op_Implicit(nameof (_originalBlockPosition));
    public static readonly StringName _currentHpOnLastRefresh = StringName.op_Implicit(nameof (_currentHpOnLastRefresh));
    public static readonly StringName _maxHpOnLastRefresh = StringName.op_Implicit(nameof (_maxHpOnLastRefresh));
    public static readonly StringName _expectedMaxFgWidth = StringName.op_Implicit(nameof (_expectedMaxFgWidth));
  }

  public class SignalName : Control.SignalName
  {
  }
}
