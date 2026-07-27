// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantCard.cs")]
public class NMerchantCard : NMerchantSlot
{
  private Node2D _saleVisual;
  private Control _cardHolder;
  private NCard? _cardNode;
  private Tween? _hoverTween;
  private MerchantCardEntry _cardEntry;

  public bool IsShowingUpgradedCard => this._cardNode?.Model?.IsUpgraded.GetValueOrDefault();

  public override MerchantEntry Entry => (MerchantEntry) this._cardEntry;

  protected override CanvasItem Visual => (CanvasItem) this._cardHolder;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._cardHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardHolder"));
    this._saleVisual = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%SaleVisual"));
  }

  public void FillSlot(MerchantCardEntry cardEntry)
  {
    this._cardEntry = cardEntry;
    cardEntry.EntryUpdated += new Action(((NMerchantSlot) this).UpdateVisual);
    cardEntry.PurchaseFailed += new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    cardEntry.PurchaseCompleted += new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
    this.UpdateVisual();
  }

  protected override void UpdateVisual()
  {
    base.UpdateVisual();
    if (this._cardEntry.CreationResult == null)
    {
      ((CanvasItem) this).Visible = false;
      this.MouseFilter = (Control.MouseFilterEnum) 2L;
      this.ClearHoverTip();
    }
    else
    {
      if (this._cardNode != null && this._cardNode.Model != this._cardEntry.CreationResult.Card)
      {
        ((Node) this._cardNode).QueueFreeSafely();
        this._cardNode = (NCard) null;
      }
      if (this._cardNode == null)
      {
        this._cardNode = NCard.Create(this._cardEntry.CreationResult.Card);
        ((Node) this._cardHolder).AddChildSafely((Node) this._cardNode);
        this._cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      }
      this._costLabel.SetTextAutoSize(this._cardEntry.Cost.ToString());
      ((CanvasItem) this._saleVisual).Visible = this._cardEntry.IsOnSale;
      if (!this._cardEntry.EnoughGold)
        ((CanvasItem) this._costLabel).Modulate = StsColors.red;
      else
        ((CanvasItem) this._costLabel).Modulate = this._cardEntry.IsOnSale ? StsColors.green : StsColors.cream;
    }
  }

  public void OnInventoryOpened()
  {
    CardCreationResult creationResult = this._cardEntry.CreationResult;
    if ((creationResult != null ? (creationResult.HasBeenModified ? 1 : 0) : 0) == 0)
      return;
    TaskHelper.RunSafely(this.DoRelicFlash());
  }

  private async Task DoRelicFlash()
  {
    await ((GodotObject) ((Node) this).GetTree().CreateTimer(0.4, true, false, false)).AwaitSignal(SceneTreeTimer.SignalName.Timeout, (Node) this);
    foreach (RelicModel modifyingRelic in this._cardEntry.CreationResult.ModifyingRelics)
    {
      modifyingRelic.Flash();
      this._cardNode?.FlashRelicOnCard(modifyingRelic);
    }
  }

  protected override async Task OnTryPurchase(MerchantInventory? inventory)
  {
    int num = await this._cardEntry.OnTryPurchaseWrapper(inventory) ? 1 : 0;
  }

  protected void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
  {
    this.TriggerMerchantHandToPointHere();
    NRun.Instance?.GlobalUi.ReparentCard(this._cardNode);
    NRun instance = NRun.Instance;
    if (instance != null)
      instance.GlobalUi.TopBar.TrailContainer.AddChildSafely((Node) NCardFlyVfx.Create(this._cardNode, PileType.Deck, true, this._cardNode.Model.Owner.Character.TrailPath));
    this._cardNode = (NCard) null;
    this.UpdateVisual();
  }

  protected override void OnPreview()
  {
    this.ClearHoverTip();
    NInspectCardScreen inspectCardScreen = NGame.Instance.GetInspectCardScreen();
    int capacity = 1;
    List<CardModel> cards = new List<CardModel>(capacity);
    CollectionsMarshal.SetCount<CardModel>(cards, capacity);
    CollectionsMarshal.AsSpan<CardModel>(cards)[0] = this._cardNode.Model;
    inspectCardScreen.Open(cards, 0);
  }

  protected override void CreateHoverTip()
  {
    NHoverTipSet.CreateAndShow((Control) this, this._cardEntry.CreationResult.Card.HoverTips)?.SetAlignment((Control) this._hitbox, HoverTip.GetHoverTipAlignment((Control) this));
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    NCard cardNode = this._cardNode;
    if (cardNode != null)
      ((Node) cardNode).QueueFreeSafely();
    this._cardEntry.EntryUpdated -= new Action(((NMerchantSlot) this).UpdateVisual);
    this._cardEntry.PurchaseFailed -= new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    this._cardEntry.PurchaseCompleted -= new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NMerchantCard.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCard.MethodName.UpdateVisual, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCard.MethodName.OnInventoryOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCard.MethodName.OnPreview, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCard.MethodName.CreateHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCard.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantCard.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCard.MethodName.UpdateVisual) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisual();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCard.MethodName.OnInventoryOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnInventoryOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCard.MethodName.OnPreview) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPreview();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCard.MethodName.CreateHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantCard.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantCard.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantCard.MethodName.UpdateVisual) || StringName.op_Equality(ref method, NMerchantCard.MethodName.OnInventoryOpened) || StringName.op_Equality(ref method, NMerchantCard.MethodName.OnPreview) || StringName.op_Equality(ref method, NMerchantCard.MethodName.CreateHoverTip) || StringName.op_Equality(ref method, NMerchantCard.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName._saleVisual))
    {
      this._saleVisual = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName._cardHolder))
    {
      this._cardHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName._cardNode))
    {
      this._cardNode = VariantUtils.ConvertTo<NCard>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantCard.PropertyName._hoverTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName.IsShowingUpgradedCard))
    {
      ref godot_variant local = ref value;
      bool showingUpgradedCard = this.IsShowingUpgradedCard;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showingUpgradedCard);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName.Visual))
    {
      ref godot_variant local = ref value;
      CanvasItem visual = this.Visual;
      godot_variant from = VariantUtils.CreateFrom<CanvasItem>(ref visual);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName._saleVisual))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._saleVisual);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName._cardHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCard.PropertyName._cardNode))
    {
      value = VariantUtils.CreateFrom<NCard>(ref this._cardNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantCard.PropertyName._hoverTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMerchantCard.PropertyName._saleVisual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCard.PropertyName._cardHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCard.PropertyName._cardNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCard.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantCard.PropertyName.IsShowingUpgradedCard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCard.PropertyName.Visual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMerchantCard.PropertyName._saleVisual, Variant.From<Node2D>(ref this._saleVisual));
    info.AddProperty(NMerchantCard.PropertyName._cardHolder, Variant.From<Control>(ref this._cardHolder));
    info.AddProperty(NMerchantCard.PropertyName._cardNode, Variant.From<NCard>(ref this._cardNode));
    info.AddProperty(NMerchantCard.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantCard.PropertyName._saleVisual, ref variant1))
      this._saleVisual = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantCard.PropertyName._cardHolder, ref variant2))
      this._cardHolder = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantCard.PropertyName._cardNode, ref variant3))
      this._cardNode = ((Variant) ref variant3).As<NCard>();
    Variant variant4;
    if (!info.TryGetProperty(NMerchantCard.PropertyName._hoverTween, ref variant4))
      return;
    this._hoverTween = ((Variant) ref variant4).As<Tween>();
  }

  public new class MethodName : NMerchantSlot.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName UpdateVisual = StringName.op_Implicit(nameof (UpdateVisual));
    public static readonly StringName OnInventoryOpened = StringName.op_Implicit(nameof (OnInventoryOpened));
    public new static readonly StringName OnPreview = StringName.op_Implicit(nameof (OnPreview));
    public new static readonly StringName CreateHoverTip = StringName.op_Implicit(nameof (CreateHoverTip));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NMerchantSlot.PropertyName
  {
    public static readonly StringName IsShowingUpgradedCard = StringName.op_Implicit(nameof (IsShowingUpgradedCard));
    public new static readonly StringName Visual = StringName.op_Implicit(nameof (Visual));
    public static readonly StringName _saleVisual = StringName.op_Implicit(nameof (_saleVisual));
    public static readonly StringName _cardHolder = StringName.op_Implicit(nameof (_cardHolder));
    public static readonly StringName _cardNode = StringName.op_Implicit(nameof (_cardNode));
    public new static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
  }

  public new class SignalName : NMerchantSlot.SignalName
  {
  }
}
