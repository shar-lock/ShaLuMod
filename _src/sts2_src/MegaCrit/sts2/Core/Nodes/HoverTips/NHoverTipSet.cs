// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.HoverTips.NHoverTipSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.HoverTips;

[ScriptPath("res://src/Core/Nodes/HoverTips/NHoverTipSet.cs")]
public class NHoverTipSet : Control
{
  public static bool shouldBlockHoverTips = false;
  private static readonly StringName _cardHoverTipContainerStr = new StringName("cardHoverTipContainer");
  private static readonly StringName _textHoverTipContainerStr = new StringName("textHoverTipContainer");
  private const float _hoverTipSpacing = 5f;
  private const float _hoverTipWidth = 360f;
  private const string _tipScenePath = "res://scenes/ui/hover_tip.tscn";
  private const string _tipSetScenePath = "res://scenes/ui/hover_tip_set.tscn";
  private const string _debuffMatPath = "res://materials/ui/hover_tip_debuff.tres";
  private static readonly Dictionary<Control, NHoverTipSet> _activeHoverTips = new Dictionary<Control, NHoverTipSet>();
  private VFlowContainer _textHoverTipContainer;
  private NHoverTipCardContainer _cardHoverTipContainer;
  private Control _owner;
  private bool _followOwner;
  private Vector2 _followOffset;
  private Vector2 _extraOffset = Vector2.Zero;

  private static Node HoverTipsContainer => NGame.Instance.HoverTipsContainer;

  private Vector2 TextHoverTipDimensions => ((Control) this._textHoverTipContainer).Size;

  private Vector2 CardHoverTipDimensions => this._cardHoverTipContainer.Size;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[3]
      {
        "res://scenes/ui/hover_tip.tscn",
        "res://scenes/ui/hover_tip_set.tscn",
        "res://materials/ui/hover_tip_debuff.tres"
      });
    }
  }

  public void SetFollowOwner()
  {
    this._followOwner = true;
    this._followOffset = Vector2.op_Subtraction(this._owner.GlobalPosition, this.GlobalPosition);
  }

  public static NHoverTipSet? CreateAndShow(
    Control owner,
    IHoverTip hoverTip,
    HoverTipAlignment alignment = HoverTipAlignment.None)
  {
    // ISSUE: object of a compiler-generated type is created
    return NHoverTipSet.CreateAndShow(owner, (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(hoverTip), alignment);
  }

  public static NHoverTipSet? CreateAndShow(
    Control owner,
    IEnumerable<IHoverTip> hoverTips,
    HoverTipAlignment alignment = HoverTipAlignment.None)
  {
    if (NHoverTipSet.shouldBlockHoverTips)
      return (NHoverTipSet) null;
    NHoverTipSet child = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip_set.tscn").Instantiate<NHoverTipSet>((PackedScene.GenEditState) 0L);
    NHoverTipSet.HoverTipsContainer.AddChildSafely((Node) child);
    NHoverTipSet._activeHoverTips.Add(owner, child);
    child.Init(owner, hoverTips);
    if (NGame.IsDebugHidingHoverTips)
      ((CanvasItem) child).Visible = false;
    ((GodotObject) owner).Connect(Node.SignalName.TreeExiting, Callable.From((Action) (() => NHoverTipSet.Remove(owner))), 0U);
    child.SetAlignment(owner, alignment);
    return child;
  }

  public static NHoverTipSet CreateAndShowMapPointHistory(
    Control owner,
    NMapPointHistoryHoverTip historyHoverTip)
  {
    NHoverTipSet child = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip_set.tscn").Instantiate<NHoverTipSet>((PackedScene.GenEditState) 0L);
    child._owner = owner;
    NHoverTipSet.HoverTipsContainer.AddChildSafely((Node) child);
    NHoverTipSet._activeHoverTips.Add(owner, child);
    ((Node) child._textHoverTipContainer).AddChildSafely((Node) historyHoverTip);
    if (NGame.IsDebugHidingHoverTips)
      ((CanvasItem) child).Visible = false;
    ((GodotObject) owner).Connect(Node.SignalName.TreeExiting, Callable.From((Action) (() => NHoverTipSet.Remove(owner))), 0U);
    return child;
  }

  public override void _Ready()
  {
    this._textHoverTipContainer = new VFlowContainer();
    ((Node) this._textHoverTipContainer).Name = NHoverTipSet._textHoverTipContainerStr;
    ((Control) this._textHoverTipContainer).MouseFilter = (Control.MouseFilterEnum) 2L;
    ((Node) this).AddChildSafely((Node) this._textHoverTipContainer);
    this._cardHoverTipContainer = new NHoverTipCardContainer();
    ((Node) this._cardHoverTipContainer).Name = NHoverTipSet._cardHoverTipContainerStr;
    this._cardHoverTipContainer.MouseFilter = (Control.MouseFilterEnum) 2L;
    ((Node) this).AddChildSafely((Node) this._cardHoverTipContainer);
  }

  public override void _Process(double delta)
  {
    if (!this._followOwner || this._owner == null)
      return;
    this.GlobalPosition = Vector2.op_Addition(Vector2.op_Subtraction(this._owner.GlobalPosition, this._followOffset), this._extraOffset);
  }

  private void Init(Control owner, IEnumerable<IHoverTip> hoverTips)
  {
    this._owner = owner;
    foreach (IHoverTip removeDupe in IHoverTip.RemoveDupes(hoverTips))
    {
      if (removeDupe is HoverTip hoverTip)
      {
        Control child = PreloadManager.Cache.GetScene("res://scenes/ui/hover_tip.tscn").Instantiate<Control>((PackedScene.GenEditState) 0L);
        ((Node) this._textHoverTipContainer).AddChildSafely((Node) child);
        MegaLabel node = ((Node) child).GetNode<MegaLabel>(NodePath.op_Implicit("%Title"));
        if (hoverTip.Title == null)
          ((CanvasItem) node).Visible = false;
        else
          node.SetTextAutoSize(hoverTip.Title);
        ((Node) child).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description")).Text = hoverTip.Description;
        ((Node) child).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description")).AutowrapMode = hoverTip.ShouldOverrideTextOverflow ? (TextServer.AutowrapMode) 0L : (TextServer.AutowrapMode) 3L;
        ((Node) child).GetNode<TextureRect>(NodePath.op_Implicit("%Icon")).Texture = hoverTip.Icon;
        if (hoverTip.IsDebuff)
          ((Node) child).GetNode<CanvasItem>(NodePath.op_Implicit("%Bg")).Material = PreloadManager.Cache.GetMaterial("res://materials/ui/hover_tip_debuff.tres");
        child.ResetSize();
        double num1 = (double) ((Control) this._textHoverTipContainer).Size.Y + (double) child.Size.Y + 5.0;
        Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
        double num2 = (double) ((Rect2) ref viewportRect).Size.Y - 50.0;
        if (num1 < num2)
          ((Control) this._textHoverTipContainer).Size = new Vector2(360f, (float) ((double) ((Control) this._textHoverTipContainer).Size.Y + (double) child.Size.Y + 5.0));
        else
          ((FlowContainer) this._textHoverTipContainer).Alignment = (FlowContainer.AlignmentMode) 1L;
      }
      else
        this._cardHoverTipContainer.Add((CardHoverTip) removeDupe);
      switch (removeDupe.CanonicalModel)
      {
        case CardModel card:
          SaveManager.Instance.MarkCardAsSeen(card);
          continue;
        case RelicModel relic:
          SaveManager.Instance.MarkRelicAsSeen(relic);
          continue;
        case PotionModel potion:
          SaveManager.Instance.MarkPotionAsSeen(potion);
          continue;
        default:
          continue;
      }
    }
  }

  public void SetAlignment(Control node, HoverTipAlignment alignment)
  {
    if (alignment != HoverTipAlignment.None)
    {
      ((Control) this._textHoverTipContainer).Position = Vector2.Zero;
      if (alignment == HoverTipAlignment.Left)
      {
        ((Control) this._textHoverTipContainer).GlobalPosition = node.GlobalPosition;
        VFlowContainer hoverTipContainer = this._textHoverTipContainer;
        ((Control) hoverTipContainer).Position = Vector2.op_Addition(((Control) hoverTipContainer).Position, Vector2.op_Multiply(Vector2.Left, ((Control) this._textHoverTipContainer).Size.X));
        ((FlowContainer) this._textHoverTipContainer).ReverseFill = true;
        this._cardHoverTipContainer.LayoutResizeAndReposition(Vector2.op_Addition(node.GlobalPosition, Vector2.op_Multiply(new Vector2(node.Size.X, 0.0f), node.Scale)), HoverTipAlignment.Right);
      }
      else if (alignment == HoverTipAlignment.Right)
      {
        this._cardHoverTipContainer.LayoutResizeAndReposition(node.GlobalPosition, HoverTipAlignment.Left);
        ((Control) this._textHoverTipContainer).GlobalPosition = Vector2.op_Addition(node.GlobalPosition, Vector2.op_Multiply(new Vector2(node.Size.X, 0.0f), node.Scale));
      }
      else if (alignment == HoverTipAlignment.Center)
      {
        this.GlobalPosition = Vector2.op_Addition(node.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, node.Size.Y), 1.5f));
        NHoverTipCardContainer hoverTipContainer = this._cardHoverTipContainer;
        hoverTipContainer.GlobalPosition = Vector2.op_Addition(hoverTipContainer.GlobalPosition, Vector2.op_Multiply(Vector2.Down, ((Control) this._textHoverTipContainer).Size.Y));
        this._cardHoverTipContainer.LayoutResizeAndReposition(this._cardHoverTipContainer.GlobalPosition, alignment);
      }
    }
    this.CorrectVerticalOverflow();
    this.CorrectHorizontalOverflow();
  }

  public void SetAlignmentForRelic(NRelic relic)
  {
    HoverTipAlignment hoverTipAlignment = HoverTip.GetHoverTipAlignment((Control) relic);
    Vector2 size = ((Control) relic.Icon).Size;
    Transform2D globalTransform = ((CanvasItem) relic).GetGlobalTransform();
    Vector2 scale = ((Transform2D) ref globalTransform).Scale;
    Vector2 vector2 = Vector2.op_Multiply(size, scale);
    ((Control) this._textHoverTipContainer).GlobalPosition = Vector2.op_Addition(relic.GlobalPosition, Vector2.op_Multiply(Vector2.Down, vector2.Y + 10f));
    if (hoverTipAlignment == HoverTipAlignment.Left)
    {
      VFlowContainer hoverTipContainer = this._textHoverTipContainer;
      ((Control) hoverTipContainer).Position = Vector2.op_Addition(((Control) hoverTipContainer).Position, Vector2.op_Multiply(Vector2.Left, ((Control) this._textHoverTipContainer).Size.X - vector2.X));
    }
    this._cardHoverTipContainer.LayoutResizeAndReposition(Vector2.op_Addition(((Control) this._textHoverTipContainer).GlobalPosition, Vector2.op_Multiply(Vector2.Down, ((Control) this._textHoverTipContainer).Size.Y)), hoverTipAlignment);
    if (hoverTipAlignment == HoverTipAlignment.Left)
      this._cardHoverTipContainer.GlobalPosition = new Vector2(((Control) this._textHoverTipContainer).GlobalPosition.X, this._cardHoverTipContainer.GlobalPosition.Y);
    Rect2 rect2 = ((CanvasItem) NGame.Instance).GetViewportRect();
    float y = ((Rect2) ref rect2).Size.Y;
    if ((double) relic.GlobalPosition.Y > (double) y * 0.75)
      ((Control) this._textHoverTipContainer).GlobalPosition = Vector2.op_Addition(relic.GlobalPosition, Vector2.op_Multiply(Vector2.Up, ((Control) this._textHoverTipContainer).Size.Y));
    this.CorrectVerticalOverflow();
    this.CorrectHorizontalOverflow();
    rect2 = ((Control) this._textHoverTipContainer).GetRect();
    if (!((Rect2) ref rect2).Intersects(this._cardHoverTipContainer.GetRect(), false))
      return;
    if (hoverTipAlignment == HoverTipAlignment.Left)
      this._cardHoverTipContainer.GlobalPosition = Vector2.op_Addition(((Control) this._textHoverTipContainer).GlobalPosition, Vector2.op_Multiply(((Control) this._textHoverTipContainer).Size.X, Vector2.Right));
    else
      this._cardHoverTipContainer.GlobalPosition = Vector2.op_Addition(((Control) this._textHoverTipContainer).GlobalPosition, Vector2.op_Multiply(this._cardHoverTipContainer.Size.X, Vector2.Left));
    this.CorrectVerticalOverflow();
  }

  public void SetAlignmentForCardHolder(NCardHolder holder)
  {
    HoverTipAlignment hoverTipAlignment = HoverTip.GetHoverTipAlignment((Control) holder);
    ((Control) this._textHoverTipContainer).Position = Vector2.Zero;
    Control hitbox = (Control) holder.Hitbox;
    if (hoverTipAlignment == HoverTipAlignment.Left)
    {
      ((Control) this._textHoverTipContainer).GlobalPosition = hitbox.GlobalPosition;
      VFlowContainer hoverTipContainer = this._textHoverTipContainer;
      ((Control) hoverTipContainer).Position = Vector2.op_Addition(((Control) hoverTipContainer).Position, Vector2.op_Subtraction(Vector2.op_Multiply(Vector2.Left, ((Control) this._textHoverTipContainer).Size.X), new Vector2(10f, 0.0f)));
      ((FlowContainer) this._textHoverTipContainer).ReverseFill = true;
      this._cardHoverTipContainer.LayoutResizeAndReposition(Vector2.op_Addition(hitbox.GlobalPosition, Vector2.op_Multiply(new Vector2(hitbox.Size.X, 0.0f), hitbox.Scale)), HoverTipAlignment.Right);
    }
    else
    {
      Vector2 globalStartLocation = hitbox.GlobalPosition;
      if (holder.CardModel != null && (holder.CardModel.CurrentStarCost > 0 || holder.CardModel.HasStarCostX))
        globalStartLocation = Vector2.op_Addition(globalStartLocation, Vector2.op_Multiply(Vector2.Left, 15f));
      this._cardHoverTipContainer.LayoutResizeAndReposition(globalStartLocation, HoverTipAlignment.Left);
      ((Control) this._textHoverTipContainer).GlobalPosition = Vector2.op_Addition(hitbox.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(hitbox.Size.X + 10f, 0.0f), hitbox.Scale), holder.Scale));
    }
    this.CorrectVerticalOverflow();
    this.CorrectHorizontalOverflow();
    this.SetFollowOwner();
  }

  private void CorrectVerticalOverflow()
  {
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    float y = ((Rect2) ref viewportRect).Size.Y;
    if ((double) ((Control) this._textHoverTipContainer).GlobalPosition.Y + (double) ((Control) this._textHoverTipContainer).Size.Y > (double) y)
      ((Control) this._textHoverTipContainer).GlobalPosition = new Vector2(((Control) this._textHoverTipContainer).GlobalPosition.X, y - ((Control) this._textHoverTipContainer).Size.Y);
    if ((double) this._cardHoverTipContainer.GlobalPosition.Y + (double) this._cardHoverTipContainer.Size.Y <= (double) y)
      return;
    this._cardHoverTipContainer.GlobalPosition = new Vector2(this._cardHoverTipContainer.GlobalPosition.X, y - this._cardHoverTipContainer.Size.Y);
  }

  private void CorrectHorizontalOverflow()
  {
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    float x1 = ((Rect2) ref viewportRect).Size.X;
    Vector2 globalPosition1 = this._cardHoverTipContainer.GlobalPosition;
    float x2 = this._cardHoverTipContainer.Size.X;
    Vector2 globalPosition2 = ((Control) this._textHoverTipContainer).GlobalPosition;
    float x3 = ((Control) this._textHoverTipContainer).Size.X;
    if ((double) globalPosition1.X + (double) x2 <= (double) x1 && (double) globalPosition2.X + (double) x3 > (double) x1)
      ((Control) this._textHoverTipContainer).GlobalPosition = new Vector2(globalPosition1.X - x3, globalPosition1.Y);
    else if ((double) globalPosition1.X + (double) x2 > (double) x1 || (double) globalPosition2.X + (double) x3 > (double) x1)
    {
      this._cardHoverTipContainer.GlobalPosition = new Vector2(globalPosition2.X + x3 - x2, globalPosition1.Y);
      VFlowContainer hoverTipContainer = this._textHoverTipContainer;
      ((Control) hoverTipContainer).GlobalPosition = Vector2.op_Addition(((Control) hoverTipContainer).GlobalPosition, Vector2.op_Multiply(Vector2.Left, x2));
    }
    else
    {
      if ((double) globalPosition1.X >= 0.0 && (double) globalPosition2.X >= 0.0)
        return;
      this._cardHoverTipContainer.GlobalPosition = new Vector2(globalPosition2.X, globalPosition1.Y);
      VFlowContainer hoverTipContainer = this._textHoverTipContainer;
      ((Control) hoverTipContainer).GlobalPosition = Vector2.op_Addition(((Control) hoverTipContainer).GlobalPosition, Vector2.op_Multiply(Vector2.Right, x2));
    }
  }

  public static void Clear()
  {
    foreach (Control key in NHoverTipSet._activeHoverTips.Keys)
      NHoverTipSet.Remove(key);
  }

  public static void Remove(Control owner)
  {
    NHoverTipSet nhoverTipSet;
    if (!NHoverTipSet._activeHoverTips.TryGetValue(owner, out nhoverTipSet))
      return;
    ((Node) nhoverTipSet).QueueFreeSafely();
    NHoverTipSet._activeHoverTips.Remove(owner);
  }

  public void SetExtraFollowOffset(Vector2 offset) => this._extraOffset = offset;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NHoverTipSet.MethodName.SetFollowOwner, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.CreateAndShowMapPointHistory, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("owner"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("historyHoverTip"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("MarginContainer"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.SetAlignment, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("alignment"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.SetAlignmentForRelic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("relic"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.SetAlignmentForCardHolder, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("holder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.CorrectVerticalOverflow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.CorrectHorizontalOverflow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.Clear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.Remove, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("owner"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NHoverTipSet.MethodName.SetExtraFollowOffset, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("offset"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetFollowOwner) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFollowOwner();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.CreateAndShowMapPointHistory) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHoverTipSet showMapPointHistory = NHoverTipSet.CreateAndShowMapPointHistory(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NMapPointHistoryHoverTip>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHoverTipSet>(ref showMapPointHistory);
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetAlignment) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.SetAlignment(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<HoverTipAlignment>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetAlignmentForRelic) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAlignmentForRelic(VariantUtils.ConvertTo<NRelic>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetAlignmentForCardHolder) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetAlignmentForCardHolder(VariantUtils.ConvertTo<NCardHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.CorrectVerticalOverflow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CorrectVerticalOverflow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.CorrectHorizontalOverflow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CorrectHorizontalOverflow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.Clear) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NHoverTipSet.Clear();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.Remove) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHoverTipSet.Remove(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetExtraFollowOffset) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetExtraFollowOffset(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.CreateAndShowMapPointHistory) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NHoverTipSet showMapPointHistory = NHoverTipSet.CreateAndShowMapPointHistory(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NMapPointHistoryHoverTip>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NHoverTipSet>(ref showMapPointHistory);
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.Clear) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NHoverTipSet.Clear();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHoverTipSet.MethodName.Remove) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NHoverTipSet.Remove(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetFollowOwner) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.CreateAndShowMapPointHistory) || StringName.op_Equality(ref method, NHoverTipSet.MethodName._Ready) || StringName.op_Equality(ref method, NHoverTipSet.MethodName._Process) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetAlignment) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetAlignmentForRelic) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetAlignmentForCardHolder) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.CorrectVerticalOverflow) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.CorrectHorizontalOverflow) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.Clear) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.Remove) || StringName.op_Equality(ref method, NHoverTipSet.MethodName.SetExtraFollowOffset) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._textHoverTipContainer))
    {
      this._textHoverTipContainer = VariantUtils.ConvertTo<VFlowContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._cardHoverTipContainer))
    {
      this._cardHoverTipContainer = VariantUtils.ConvertTo<NHoverTipCardContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._owner))
    {
      this._owner = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._followOwner))
    {
      this._followOwner = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._followOffset))
    {
      this._followOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHoverTipSet.PropertyName._extraOffset))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._extraOffset = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName.TextHoverTipDimensions))
    {
      ref godot_variant local = ref value;
      Vector2 hoverTipDimensions = this.TextHoverTipDimensions;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverTipDimensions);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName.CardHoverTipDimensions))
    {
      ref godot_variant local = ref value;
      Vector2 hoverTipDimensions = this.CardHoverTipDimensions;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverTipDimensions);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._textHoverTipContainer))
    {
      value = VariantUtils.CreateFrom<VFlowContainer>(ref this._textHoverTipContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._cardHoverTipContainer))
    {
      value = VariantUtils.CreateFrom<NHoverTipCardContainer>(ref this._cardHoverTipContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._owner))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._owner);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._followOwner))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._followOwner);
      return true;
    }
    if (StringName.op_Equality(ref name, NHoverTipSet.PropertyName._followOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._followOffset);
      return true;
    }
    if (!StringName.op_Equality(ref name, NHoverTipSet.PropertyName._extraOffset))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._extraOffset);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NHoverTipSet.PropertyName._textHoverTipContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHoverTipSet.PropertyName._cardHoverTipContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHoverTipSet.PropertyName.TextHoverTipDimensions, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHoverTipSet.PropertyName.CardHoverTipDimensions, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NHoverTipSet.PropertyName._owner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NHoverTipSet.PropertyName._followOwner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHoverTipSet.PropertyName._followOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NHoverTipSet.PropertyName._extraOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NHoverTipSet.PropertyName._textHoverTipContainer, Variant.From<VFlowContainer>(ref this._textHoverTipContainer));
    info.AddProperty(NHoverTipSet.PropertyName._cardHoverTipContainer, Variant.From<NHoverTipCardContainer>(ref this._cardHoverTipContainer));
    info.AddProperty(NHoverTipSet.PropertyName._owner, Variant.From<Control>(ref this._owner));
    info.AddProperty(NHoverTipSet.PropertyName._followOwner, Variant.From<bool>(ref this._followOwner));
    info.AddProperty(NHoverTipSet.PropertyName._followOffset, Variant.From<Vector2>(ref this._followOffset));
    info.AddProperty(NHoverTipSet.PropertyName._extraOffset, Variant.From<Vector2>(ref this._extraOffset));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NHoverTipSet.PropertyName._textHoverTipContainer, ref variant1))
      this._textHoverTipContainer = ((Variant) ref variant1).As<VFlowContainer>();
    Variant variant2;
    if (info.TryGetProperty(NHoverTipSet.PropertyName._cardHoverTipContainer, ref variant2))
      this._cardHoverTipContainer = ((Variant) ref variant2).As<NHoverTipCardContainer>();
    Variant variant3;
    if (info.TryGetProperty(NHoverTipSet.PropertyName._owner, ref variant3))
      this._owner = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NHoverTipSet.PropertyName._followOwner, ref variant4))
      this._followOwner = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (info.TryGetProperty(NHoverTipSet.PropertyName._followOffset, ref variant5))
      this._followOffset = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (!info.TryGetProperty(NHoverTipSet.PropertyName._extraOffset, ref variant6))
      return;
    this._extraOffset = ((Variant) ref variant6).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName SetFollowOwner = StringName.op_Implicit(nameof (SetFollowOwner));
    public static readonly StringName CreateAndShowMapPointHistory = StringName.op_Implicit(nameof (CreateAndShowMapPointHistory));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName SetAlignment = StringName.op_Implicit(nameof (SetAlignment));
    public static readonly StringName SetAlignmentForRelic = StringName.op_Implicit(nameof (SetAlignmentForRelic));
    public static readonly StringName SetAlignmentForCardHolder = StringName.op_Implicit(nameof (SetAlignmentForCardHolder));
    public static readonly StringName CorrectVerticalOverflow = StringName.op_Implicit(nameof (CorrectVerticalOverflow));
    public static readonly StringName CorrectHorizontalOverflow = StringName.op_Implicit(nameof (CorrectHorizontalOverflow));
    public static readonly StringName Clear = StringName.op_Implicit(nameof (Clear));
    public static readonly StringName Remove = StringName.op_Implicit(nameof (Remove));
    public static readonly StringName SetExtraFollowOffset = StringName.op_Implicit(nameof (SetExtraFollowOffset));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName TextHoverTipDimensions = StringName.op_Implicit(nameof (TextHoverTipDimensions));
    public static readonly StringName CardHoverTipDimensions = StringName.op_Implicit(nameof (CardHoverTipDimensions));
    public static readonly StringName _textHoverTipContainer = StringName.op_Implicit(nameof (_textHoverTipContainer));
    public static readonly StringName _cardHoverTipContainer = StringName.op_Implicit(nameof (_cardHoverTipContainer));
    public static readonly StringName _owner = StringName.op_Implicit(nameof (_owner));
    public static readonly StringName _followOwner = StringName.op_Implicit(nameof (_followOwner));
    public static readonly StringName _followOffset = StringName.op_Implicit(nameof (_followOffset));
    public static readonly StringName _extraOffset = StringName.op_Implicit(nameof (_extraOffset));
  }

  public class SignalName : Control.SignalName
  {
  }
}
