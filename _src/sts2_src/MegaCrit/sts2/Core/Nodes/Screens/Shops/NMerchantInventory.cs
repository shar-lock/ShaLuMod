// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantInventory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantInventory.cs")]
public class NMerchantInventory : Control, IScreenContext
{
  private const float _openPosition = 80f;
  private const float _closedPosition = -1000f;
  private Control? _characterCardContainer;
  private Control? _colorlessCardContainer;
  protected Control? _relicContainer;
  private Control? _potionContainer;
  private NMerchantCardRemoval? _cardRemovalNode;
  private NBackButton _backButton;
  private NMerchantDialogue _merchantDialogue;
  private Tween? _inventoryTween;
  private Control _slotsContainer;
  private ColorRect _backstop;
  private Control _inputBlocker;
  private bool _isInputBlocked;
  private NMerchantSlot? _lastSlot;
  private 
  #nullable disable
  NMerchantInventory.InventoryClosedEventHandler backing_InventoryClosed;

  public 
  #nullable enable
  MerchantInventory? Inventory { get; private set; }

  public bool IsOpen { get; private set; }

  public NMerchantHand MerchantHand { get; private set; }

  public override void _Ready()
  {
    this._merchantDialogue = ((Node) this).GetNode<NMerchantDialogue>(NodePath.op_Implicit("%Dialogue"));
    ((CanvasItem) this._merchantDialogue).Modulate = Colors.Transparent;
    this._slotsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SlotsContainer"));
    this._slotsContainer.Position = new Vector2(this._slotsContainer.Position.X, -1000f);
    this._backstop = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("Backstop"));
    this.MerchantHand = ((Node) this).GetNode<NMerchantHand>(NodePath.op_Implicit("%MerchantHand"));
    this._characterCardContainer = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("%CharacterCards"));
    this._colorlessCardContainer = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("%ColorlessCards"));
    this._relicContainer = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("%Relics"));
    this._potionContainer = ((Node) this).GetNodeOrNull<Control>(NodePath.op_Implicit("%Potions"));
    this._cardRemovalNode = ((Node) this).GetNodeOrNull<NMerchantCardRemoval>(NodePath.op_Implicit("%MerchantCardRemoval"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%BackButton"));
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.Close())), 0U);
    this._backButton.Disable();
    this._inputBlocker = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%InputBlocker"));
    NGame.Instance.SetScreenShakeTarget((Control) this);
  }

  public override void _EnterTree()
  {
    ActiveScreenContext.Instance.Updated += new Action(this.OnActiveScreenUpdated);
    this.SubscribeToEntries();
  }

  public override void _ExitTree()
  {
    ActiveScreenContext.Instance.Updated -= new Action(this.OnActiveScreenUpdated);
    if (this.Inventory == null)
      return;
    foreach (MerchantEntry allEntry in this.Inventory.AllEntries)
    {
      allEntry.PurchaseCompleted -= new Action<PurchaseStatus, MerchantEntry>(this.OnPurchaseCompleted);
      allEntry.PurchaseFailed -= new Action<PurchaseStatus>(this._merchantDialogue.ShowForPurchaseAttempt);
    }
  }

  public void Initialize(MerchantInventory inventory, MerchantDialogueSet dialogue)
  {
    this.Inventory = this.Inventory == null ? inventory : throw new InvalidOperationException("Merchant inventory already populated.");
    for (int index = 0; index < this.Inventory.CharacterCardEntries.Count; ++index)
    {
      NMerchantCard child = ((Node) this._characterCardContainer).GetChild<NMerchantCard>(index, false);
      child.Initialize(this);
      child.FillSlot(this.Inventory.CharacterCardEntries[index]);
    }
    for (int index = 0; index < this.Inventory.ColorlessCardEntries.Count; ++index)
    {
      NMerchantCard child = ((Node) this._colorlessCardContainer).GetChild<NMerchantCard>(index, false);
      child.Initialize(this);
      child.FillSlot(this.Inventory.ColorlessCardEntries[index]);
    }
    for (int index = 0; index < this.Inventory.RelicEntries.Count; ++index)
    {
      NMerchantRelic child = ((Node) this._relicContainer).GetChild<NMerchantRelic>(index, false);
      child.Initialize(this);
      child.FillSlot(this.Inventory.RelicEntries[index]);
    }
    for (int index = 0; index < this.Inventory.PotionEntries.Count; ++index)
    {
      NMerchantPotion child = ((Node) this._potionContainer).GetChild<NMerchantPotion>(index, false);
      child.Initialize(this);
      child.FillSlot(this.Inventory.PotionEntries[index]);
    }
    if (this.Inventory.CardRemovalEntry != null)
    {
      this._cardRemovalNode.Initialize(this);
      this._cardRemovalNode.FillSlot(this.Inventory.CardRemovalEntry);
    }
    this.SubscribeToEntries();
    this.UpdateNavigation();
    foreach (NMerchantSlot allSlot in this.GetAllSlots())
    {
      NMerchantSlot slot = allSlot;
      ((GodotObject) slot).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._lastSlot = slot)), 0U);
    }
    this._merchantDialogue.Initialize(dialogue);
  }

  public void Open()
  {
    if (!SaveManager.Instance.SeenFtue("merchant_ftue"))
      SaveManager.Instance.MarkFtueAsComplete("merchant_ftue");
    TaskHelper.RunSafely(this.DoOpenAnimation());
    this.MouseFilter = (Control.MouseFilterEnum) 0L;
    if (!this._isInputBlocked)
      this._backButton.Enable();
    foreach (NMerchantCard cardSlot in this.GetCardSlots())
      cardSlot.OnInventoryOpened();
    SfxCmd.Play("event:/sfx/npcs/merchant/merchant_welcome");
    this.IsOpen = true;
    ActiveScreenContext.Instance.Update();
    this._merchantDialogue.ShowOnInventoryOpen();
  }

  private void SubscribeToEntries()
  {
    if (!((Node) this).IsNodeReady() || this.Inventory == null)
      return;
    foreach (MerchantEntry allEntry in this.Inventory.AllEntries)
    {
      allEntry.PurchaseCompleted += new Action<PurchaseStatus, MerchantEntry>(this.OnPurchaseCompleted);
      allEntry.PurchaseFailed += new Action<PurchaseStatus>(this._merchantDialogue.ShowForPurchaseAttempt);
    }
  }

  private async Task DoOpenAnimation()
  {
    this._inventoryTween?.Kill();
    this._inventoryTween = ((Node) this).CreateTween().SetParallel(true);
    this._inventoryTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.8f), 1.0).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L).FromCurrent();
    this._inventoryTween.TweenProperty((GodotObject) this._slotsContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(80f), 0.699999988079071).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 2L).FromCurrent();
    bool flag = await this._inventoryTween.AwaitFinished((Node) this);
  }

  private void Close()
  {
    this.MerchantHand.StopPointing(0.0f);
    this._inventoryTween?.Kill();
    this._inventoryTween = ((Node) this).CreateTween().SetParallel(true);
    this._inventoryTween.TweenProperty((GodotObject) this._backstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.800000011920929).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L).FromCurrent();
    this._inventoryTween.TweenProperty((GodotObject) this._slotsContainer, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-1000f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).FromCurrent();
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._backButton.Disable();
    this._lastSlot = (NMerchantSlot) null;
    this.IsOpen = false;
    ActiveScreenContext.Instance.Update();
    this.EmitSignalInventoryClosed();
  }

  private void OnPurchaseCompleted(PurchaseStatus status, MerchantEntry entry)
  {
    this.UpdateNavigation();
    NMerchantSlot lastSlot = this.GetAllSlots().FirstOrDefault<NMerchantSlot>((Func<NMerchantSlot, bool>) (s => s.Entry == entry));
    if (lastSlot != null && !this._isInputBlocked)
    {
      NMerchantSlot control = Enumerable.MinBy<NMerchantSlot, float>(this.GetAllSlots().Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (s => ((CanvasItem) s).Visible && s != lastSlot)), (Func<NMerchantSlot, float>) (s =>
      {
        Vector2 vector2 = Vector2.op_Subtraction(s.GlobalPosition, lastSlot.GlobalPosition);
        return ((Vector2) ref vector2).Length();
      }));
      if (control != null)
        control.TryGrabFocus();
    }
    SfxCmd.Play("event:/sfx/npcs/merchant/merchant_thank_yous");
    this._merchantDialogue.ShowForPurchaseAttempt(status);
  }

  public void OnCardRemovalUsed() => this._cardRemovalNode.OnCardRemovalUsed();

  public IEnumerable<NMerchantSlot> GetAllSlots()
  {
    List<NMerchantSlot> allSlots = new List<NMerchantSlot>();
    allSlots.AddRange((IEnumerable<NMerchantSlot>) this.GetCardSlots());
    if (this._relicContainer != null)
      allSlots.AddRange((IEnumerable<NMerchantSlot>) ((IEnumerable) ((Node) this._relicContainer).GetChildren(false)).OfType<NMerchantRelic>());
    if (this._potionContainer != null)
      allSlots.AddRange((IEnumerable<NMerchantSlot>) ((IEnumerable) ((Node) this._potionContainer).GetChildren(false)).OfType<NMerchantPotion>());
    if (this._cardRemovalNode != null)
      allSlots.Add((NMerchantSlot) this._cardRemovalNode);
    return (IEnumerable<NMerchantSlot>) allSlots;
  }

  private IEnumerable<NMerchantCard> GetCardSlots()
  {
    return ((IEnumerable<IEnumerable<Node>>) new IEnumerable<Node>[2]
    {
      (IEnumerable<Node>) (((Node) this._characterCardContainer)?.GetChildren(false) ?? new Array<Node>()),
      (IEnumerable<Node>) (((Node) this._colorlessCardContainer)?.GetChildren(false) ?? new Array<Node>())
    }).SelectMany<IEnumerable<Node>, Node>((Func<IEnumerable<Node>, IEnumerable<Node>>) (n => n)).OfType<NMerchantCard>();
  }

  protected virtual void UpdateNavigation()
  {
    this.UpdateHorizontalNavigation();
    this.UpdateVerticalNavigation();
  }

  private void UpdateHorizontalNavigation()
  {
    Control characterCardContainer = this._characterCardContainer;
    List<NMerchantSlot> source = (characterCardContainer != null ? ((IEnumerable) ((Node) characterCardContainer).GetChildren(false)).OfType<NMerchantSlot>().Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (c => ((CanvasItem) c).Visible)).ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    Control colorlessCardContainer = this._colorlessCardContainer;
    List<NMerchantSlot> nmerchantSlotList1 = (colorlessCardContainer != null ? ((IEnumerable) ((Node) colorlessCardContainer).GetChildren(false)).OfType<NMerchantSlot>().Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (c => ((CanvasItem) c).Visible)).ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    Control relicContainer = this._relicContainer;
    List<NMerchantSlot> second = (relicContainer != null ? ((IEnumerable) ((Node) relicContainer).GetChildren(false)).OfType<NMerchantSlot>().Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (c => ((CanvasItem) c).Visible)).ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    Control potionContainer = this._potionContainer;
    List<NMerchantSlot> nmerchantSlotList2 = (potionContainer != null ? ((IEnumerable) ((Node) potionContainer).GetChildren(false)).OfType<NMerchantSlot>().Where<NMerchantSlot>((Func<NMerchantSlot, bool>) (c => ((CanvasItem) c).Visible)).ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    List<NMerchantSlot> list1 = source.ToList<NMerchantSlot>();
    // ISSUE: object of a compiler-generated type is created
    List<NMerchantSlot> list2 = nmerchantSlotList1.Concat<NMerchantSlot>((IEnumerable<NMerchantSlot>) second).Concat<NMerchantSlot>(this._cardRemovalNode != null ? (IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlySingleElementList<NMerchantSlot>((NMerchantSlot) this._cardRemovalNode) : (IEnumerable<NMerchantSlot>) Array.Empty<NMerchantSlot>()).ToList<NMerchantSlot>();
    // ISSUE: object of a compiler-generated type is created
    List<NMerchantSlot> list3 = nmerchantSlotList1.Concat<NMerchantSlot>((IEnumerable<NMerchantSlot>) nmerchantSlotList2).Concat<NMerchantSlot>(this._cardRemovalNode != null ? (IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlySingleElementList<NMerchantSlot>((NMerchantSlot) this._cardRemovalNode) : (IEnumerable<NMerchantSlot>) Array.Empty<NMerchantSlot>()).ToList<NMerchantSlot>();
    for (int index = 0; index < list1.Count; ++index)
    {
      list1[index].FocusNeighborLeft = index > 0 ? ((Node) list1[index - 1]).GetPath() : ((Node) list1[index]).GetPath();
      list1[index].FocusNeighborRight = index < list1.Count - 1 ? ((Node) list1[index + 1]).GetPath() : ((Node) list1[index]).GetPath();
    }
    for (int index = 0; index < list3.Count; ++index)
    {
      list3[index].FocusNeighborLeft = index > 0 ? ((Node) list3[index - 1]).GetPath() : ((Node) list3[index]).GetPath();
      list3[index].FocusNeighborRight = index < list3.Count - 1 ? ((Node) list3[index + 1]).GetPath() : ((Node) list3[index]).GetPath();
    }
    for (int index = 0; index < list2.Count; ++index)
    {
      list2[index].FocusNeighborLeft = index > 0 ? ((Node) list2[index - 1]).GetPath() : ((Node) list2[index]).GetPath();
      list2[index].FocusNeighborRight = index < list2.Count - 1 ? ((Node) list2[index + 1]).GetPath() : ((Node) list2[index]).GetPath();
    }
    if (second.Count != 0 || nmerchantSlotList2.Count <= 0)
      return;
    if (this._cardRemovalNode != null)
      this._cardRemovalNode.FocusNeighborLeft = ((Node) nmerchantSlotList2.Last<NMerchantSlot>()).GetPath();
    if (nmerchantSlotList1.Count <= 0)
      return;
    nmerchantSlotList1.Last<NMerchantSlot>().FocusNeighborRight = ((Node) nmerchantSlotList2.First<NMerchantSlot>()).GetPath();
  }

  private void UpdateVerticalNavigation()
  {
    Control characterCardContainer = this._characterCardContainer;
    List<NMerchantSlot> source = (characterCardContainer != null ? ((IEnumerable) ((Node) characterCardContainer).GetChildren(false)).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    Control colorlessCardContainer = this._colorlessCardContainer;
    List<NMerchantSlot> first = (colorlessCardContainer != null ? ((IEnumerable) ((Node) colorlessCardContainer).GetChildren(false)).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    Control relicContainer = this._relicContainer;
    List<NMerchantSlot> second1 = (relicContainer != null ? ((IEnumerable) ((Node) relicContainer).GetChildren(false)).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    Control potionContainer = this._potionContainer;
    List<NMerchantSlot> second2 = (potionContainer != null ? ((IEnumerable) ((Node) potionContainer).GetChildren(false)).OfType<NMerchantSlot>().ToList<NMerchantSlot>() : (List<NMerchantSlot>) null) ?? new List<NMerchantSlot>();
    List<NMerchantSlot> list1 = source.ToList<NMerchantSlot>();
    // ISSUE: object of a compiler-generated type is created
    List<NMerchantSlot> list2 = first.Concat<NMerchantSlot>((IEnumerable<NMerchantSlot>) second1).Concat<NMerchantSlot>(this._cardRemovalNode != null ? (IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlySingleElementList<NMerchantSlot>((NMerchantSlot) this._cardRemovalNode) : (IEnumerable<NMerchantSlot>) Array.Empty<NMerchantSlot>()).ToList<NMerchantSlot>();
    // ISSUE: object of a compiler-generated type is created
    List<NMerchantSlot> list3 = first.Concat<NMerchantSlot>((IEnumerable<NMerchantSlot>) second2).Concat<NMerchantSlot>(this._cardRemovalNode != null ? (IEnumerable<NMerchantSlot>) new \u003C\u003Ez__ReadOnlySingleElementList<NMerchantSlot>((NMerchantSlot) this._cardRemovalNode) : (IEnumerable<NMerchantSlot>) Array.Empty<NMerchantSlot>()).ToList<NMerchantSlot>();
    for (int index = 0; index < list1.Count; ++index)
    {
      list1[index].FocusNeighborTop = ((Node) list1[index]).GetPath();
      if (list2.Count > 0)
      {
        Control closestStockedSlot = this.GetClosestStockedSlot(index, list2);
        if (closestStockedSlot != null)
        {
          list1[index].FocusNeighborBottom = ((Node) closestStockedSlot).GetPath();
          continue;
        }
      }
      Control closestStockedSlot1 = this.GetClosestStockedSlot(index, list3);
      if (closestStockedSlot1 != null)
        list1[index].FocusNeighborBottom = ((Node) closestStockedSlot1).GetPath();
      else
        list1[index].FocusNeighborBottom = ((Node) list1[index]).GetPath();
    }
    for (int index = 0; index < list3.Count; ++index)
    {
      if (list2.Count > 0)
      {
        Control closestStockedSlot = this.GetClosestStockedSlot(index, list2);
        if (closestStockedSlot != null)
        {
          list3[index].FocusNeighborTop = ((Node) closestStockedSlot).GetPath();
          goto label_16;
        }
      }
      Control closestStockedSlot2 = this.GetClosestStockedSlot(index, list1);
      if (closestStockedSlot2 != null)
        list3[index].FocusNeighborTop = ((Node) closestStockedSlot2).GetPath();
      else
        list3[index].FocusNeighborTop = ((Node) list3[index]).GetPath();
label_16:
      list3[index].FocusNeighborBottom = ((Node) list3[index]).GetPath();
    }
    for (int index = 0; index < list2.Count; ++index)
    {
      if (list1.Count > 0)
      {
        Control closestStockedSlot = this.GetClosestStockedSlot(index, list1);
        if (closestStockedSlot != null)
        {
          list2[index].FocusNeighborTop = ((Node) closestStockedSlot).GetPath();
          goto label_23;
        }
      }
      list2[index].FocusNeighborTop = ((Node) list2[index]).GetPath();
label_23:
      if (list3.Count > 0)
      {
        Control closestStockedSlot = this.GetClosestStockedSlot(index, list3);
        if (closestStockedSlot != null)
        {
          list2[index].FocusNeighborBottom = ((Node) closestStockedSlot).GetPath();
          continue;
        }
      }
      list2[index].FocusNeighborBottom = ((Node) list2[index]).GetPath();
    }
  }

  public void BlockInput()
  {
    this._isInputBlocked = true;
    this._inputBlocker.MouseFilter = (Control.MouseFilterEnum) 0L;
    NHotkeyManager.Instance.AddBlockingScreen((Node) this._inputBlocker);
    if (!this._backButton.IsEnabled)
      return;
    this._backButton.Disable();
  }

  public void UnblockInput()
  {
    this._isInputBlocked = false;
    this._inputBlocker.MouseFilter = (Control.MouseFilterEnum) 2L;
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this._inputBlocker);
    if (this._backButton.IsEnabled)
      return;
    this._backButton.Enable();
  }

  private Control? GetClosestStockedSlot(int idx, List<NMerchantSlot> row)
  {
    NMerchantSlot closestStockedSlot = row[Math.Min(idx, row.Count - 1)];
    if (closestStockedSlot.Entry.IsStocked)
      return (Control) closestStockedSlot;
    int num = row.IndexOf(closestStockedSlot);
    int index1 = num - 1;
    int index2 = num + 1;
    while (index1 >= 0 || index2 < row.Count)
    {
      if (index2 < row.Count)
      {
        if (row[index2].Entry.IsStocked)
          return (Control) row[index2];
        ++index2;
      }
      if (index1 >= 0)
      {
        if (row[index1].Entry.IsStocked)
          return (Control) row[index1];
        --index1;
      }
    }
    return (Control) null;
  }

  private void OnActiveScreenUpdated()
  {
    this.UpdateControllerNavEnabled<NMerchantInventory>();
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
    {
      if (this._characterCardContainer != null && NControllerManager.Instance.IsUsingController && this._inventoryTween != null && this._inventoryTween.IsRunning())
      {
        float num = 80f - this._slotsContainer.Position.Y;
        this.MerchantHand.PointAtTarget((Control) ((Node) this._characterCardContainer).GetChild<NMerchantCard>(0, false), Vector2.op_Multiply(Vector2.Down, num));
      }
      if (this._isInputBlocked)
        return;
      this._backButton.Enable();
    }
    else
      this._backButton.Disable();
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      if (this._isInputBlocked)
        return this._inputBlocker;
      NMerchantSlot lastSlot = this._lastSlot;
      if (lastSlot != null)
      {
        MerchantEntry entry = lastSlot.Entry;
        if (entry != null && entry.IsStocked)
          return (Control) this._lastSlot;
      }
      return (Control) this.GetAllSlots().FirstOrDefault<NMerchantSlot>((Func<NMerchantSlot, bool>) (s => s.Entry.IsStocked));
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NMerchantInventory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.SubscribeToEntries, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.OnCardRemovalUsed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.UpdateHorizontalNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.UpdateVerticalNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.BlockInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.UnblockInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantInventory.MethodName.OnActiveScreenUpdated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.SubscribeToEntries) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SubscribeToEntries();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.OnCardRemovalUsed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCardRemovalUsed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.UpdateNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.UpdateHorizontalNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateHorizontalNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.UpdateVerticalNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVerticalNavigation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.BlockInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BlockInput();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantInventory.MethodName.UnblockInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UnblockInput();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantInventory.MethodName.OnActiveScreenUpdated) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnActiveScreenUpdated();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantInventory.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantInventory.MethodName._EnterTree) || StringName.op_Equality(ref method, NMerchantInventory.MethodName._ExitTree) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.Open) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.SubscribeToEntries) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.Close) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.OnCardRemovalUsed) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.UpdateNavigation) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.UpdateHorizontalNavigation) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.UpdateVerticalNavigation) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.BlockInput) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.UnblockInput) || StringName.op_Equality(ref method, NMerchantInventory.MethodName.OnActiveScreenUpdated) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName.IsOpen))
    {
      this.IsOpen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName.MerchantHand))
    {
      this.MerchantHand = VariantUtils.ConvertTo<NMerchantHand>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._characterCardContainer))
    {
      this._characterCardContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._colorlessCardContainer))
    {
      this._colorlessCardContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._relicContainer))
    {
      this._relicContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._potionContainer))
    {
      this._potionContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._cardRemovalNode))
    {
      this._cardRemovalNode = VariantUtils.ConvertTo<NMerchantCardRemoval>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._merchantDialogue))
    {
      this._merchantDialogue = VariantUtils.ConvertTo<NMerchantDialogue>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._inventoryTween))
    {
      this._inventoryTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._slotsContainer))
    {
      this._slotsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._inputBlocker))
    {
      this._inputBlocker = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._isInputBlocked))
    {
      this._isInputBlocked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantInventory.PropertyName._lastSlot))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._lastSlot = VariantUtils.ConvertTo<NMerchantSlot>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName.IsOpen))
    {
      ref godot_variant local = ref value;
      bool isOpen = this.IsOpen;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isOpen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName.MerchantHand))
    {
      ref godot_variant local = ref value;
      NMerchantHand merchantHand = this.MerchantHand;
      godot_variant from = VariantUtils.CreateFrom<NMerchantHand>(ref merchantHand);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._characterCardContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterCardContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._colorlessCardContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._colorlessCardContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._relicContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._potionContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._cardRemovalNode))
    {
      value = VariantUtils.CreateFrom<NMerchantCardRemoval>(ref this._cardRemovalNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._merchantDialogue))
    {
      value = VariantUtils.CreateFrom<NMerchantDialogue>(ref this._merchantDialogue);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._inventoryTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._inventoryTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._slotsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._slotsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._inputBlocker))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._inputBlocker);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantInventory.PropertyName._isInputBlocked))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isInputBlocked);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantInventory.PropertyName._lastSlot))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NMerchantSlot>(ref this._lastSlot);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._characterCardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._colorlessCardContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._relicContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._potionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._cardRemovalNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._merchantDialogue, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._inventoryTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._slotsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._inputBlocker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantInventory.PropertyName._isInputBlocked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName._lastSlot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantInventory.PropertyName.IsOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName.MerchantHand, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantInventory.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName isOpen1 = NMerchantInventory.PropertyName.IsOpen;
    bool isOpen2 = this.IsOpen;
    Variant variant1 = Variant.From<bool>(ref isOpen2);
    serializationInfo1.AddProperty(isOpen1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName merchantHand1 = NMerchantInventory.PropertyName.MerchantHand;
    NMerchantHand merchantHand2 = this.MerchantHand;
    Variant variant2 = Variant.From<NMerchantHand>(ref merchantHand2);
    serializationInfo2.AddProperty(merchantHand1, variant2);
    info.AddProperty(NMerchantInventory.PropertyName._characterCardContainer, Variant.From<Control>(ref this._characterCardContainer));
    info.AddProperty(NMerchantInventory.PropertyName._colorlessCardContainer, Variant.From<Control>(ref this._colorlessCardContainer));
    info.AddProperty(NMerchantInventory.PropertyName._relicContainer, Variant.From<Control>(ref this._relicContainer));
    info.AddProperty(NMerchantInventory.PropertyName._potionContainer, Variant.From<Control>(ref this._potionContainer));
    info.AddProperty(NMerchantInventory.PropertyName._cardRemovalNode, Variant.From<NMerchantCardRemoval>(ref this._cardRemovalNode));
    info.AddProperty(NMerchantInventory.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NMerchantInventory.PropertyName._merchantDialogue, Variant.From<NMerchantDialogue>(ref this._merchantDialogue));
    info.AddProperty(NMerchantInventory.PropertyName._inventoryTween, Variant.From<Tween>(ref this._inventoryTween));
    info.AddProperty(NMerchantInventory.PropertyName._slotsContainer, Variant.From<Control>(ref this._slotsContainer));
    info.AddProperty(NMerchantInventory.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
    info.AddProperty(NMerchantInventory.PropertyName._inputBlocker, Variant.From<Control>(ref this._inputBlocker));
    info.AddProperty(NMerchantInventory.PropertyName._isInputBlocked, Variant.From<bool>(ref this._isInputBlocked));
    info.AddProperty(NMerchantInventory.PropertyName._lastSlot, Variant.From<NMerchantSlot>(ref this._lastSlot));
    info.AddSignalEventDelegate(NMerchantInventory.SignalName.InventoryClosed, (Delegate) this.backing_InventoryClosed);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantInventory.PropertyName.IsOpen, ref variant1))
      this.IsOpen = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantInventory.PropertyName.MerchantHand, ref variant2))
      this.MerchantHand = ((Variant) ref variant2).As<NMerchantHand>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._characterCardContainer, ref variant3))
      this._characterCardContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._colorlessCardContainer, ref variant4))
      this._colorlessCardContainer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._relicContainer, ref variant5))
      this._relicContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._potionContainer, ref variant6))
      this._potionContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._cardRemovalNode, ref variant7))
      this._cardRemovalNode = ((Variant) ref variant7).As<NMerchantCardRemoval>();
    Variant variant8;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._backButton, ref variant8))
      this._backButton = ((Variant) ref variant8).As<NBackButton>();
    Variant variant9;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._merchantDialogue, ref variant9))
      this._merchantDialogue = ((Variant) ref variant9).As<NMerchantDialogue>();
    Variant variant10;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._inventoryTween, ref variant10))
      this._inventoryTween = ((Variant) ref variant10).As<Tween>();
    Variant variant11;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._slotsContainer, ref variant11))
      this._slotsContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._backstop, ref variant12))
      this._backstop = ((Variant) ref variant12).As<ColorRect>();
    Variant variant13;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._inputBlocker, ref variant13))
      this._inputBlocker = ((Variant) ref variant13).As<Control>();
    Variant variant14;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._isInputBlocked, ref variant14))
      this._isInputBlocked = ((Variant) ref variant14).As<bool>();
    Variant variant15;
    if (info.TryGetProperty(NMerchantInventory.PropertyName._lastSlot, ref variant15))
      this._lastSlot = ((Variant) ref variant15).As<NMerchantSlot>();
    NMerchantInventory.InventoryClosedEventHandler closedEventHandler;
    if (!info.TryGetSignalEventDelegate<NMerchantInventory.InventoryClosedEventHandler>(NMerchantInventory.SignalName.InventoryClosed, ref closedEventHandler))
      return;
    this.backing_InventoryClosed = closedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMerchantInventory.SignalName.InventoryClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NMerchantInventory.InventoryClosedEventHandler InventoryClosed
  {
    add => this.backing_InventoryClosed += value;
    remove => this.backing_InventoryClosed -= value;
  }

  protected void EmitSignalInventoryClosed()
  {
    ((GodotObject) this).EmitSignal(NMerchantInventory.SignalName.InventoryClosed, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NMerchantInventory.SignalName.InventoryClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMerchantInventory.InventoryClosedEventHandler backingInventoryClosed = this.backing_InventoryClosed;
      if (backingInventoryClosed == null)
        return;
      backingInventoryClosed();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NMerchantInventory.SignalName.InventoryClosed) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void InventoryClosedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public static readonly StringName SubscribeToEntries = StringName.op_Implicit(nameof (SubscribeToEntries));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName OnCardRemovalUsed = StringName.op_Implicit(nameof (OnCardRemovalUsed));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
    public static readonly StringName UpdateHorizontalNavigation = StringName.op_Implicit(nameof (UpdateHorizontalNavigation));
    public static readonly StringName UpdateVerticalNavigation = StringName.op_Implicit(nameof (UpdateVerticalNavigation));
    public static readonly StringName BlockInput = StringName.op_Implicit(nameof (BlockInput));
    public static readonly StringName UnblockInput = StringName.op_Implicit(nameof (UnblockInput));
    public static readonly StringName OnActiveScreenUpdated = StringName.op_Implicit(nameof (OnActiveScreenUpdated));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
    public static readonly StringName MerchantHand = StringName.op_Implicit(nameof (MerchantHand));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _characterCardContainer = StringName.op_Implicit(nameof (_characterCardContainer));
    public static readonly StringName _colorlessCardContainer = StringName.op_Implicit(nameof (_colorlessCardContainer));
    public static readonly StringName _relicContainer = StringName.op_Implicit(nameof (_relicContainer));
    public static readonly StringName _potionContainer = StringName.op_Implicit(nameof (_potionContainer));
    public static readonly StringName _cardRemovalNode = StringName.op_Implicit(nameof (_cardRemovalNode));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _merchantDialogue = StringName.op_Implicit(nameof (_merchantDialogue));
    public static readonly StringName _inventoryTween = StringName.op_Implicit(nameof (_inventoryTween));
    public static readonly StringName _slotsContainer = StringName.op_Implicit(nameof (_slotsContainer));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _inputBlocker = StringName.op_Implicit(nameof (_inputBlocker));
    public static readonly StringName _isInputBlocked = StringName.op_Implicit(nameof (_isInputBlocked));
    public static readonly StringName _lastSlot = StringName.op_Implicit(nameof (_lastSlot));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName InventoryClosed = StringName.op_Implicit(nameof (InventoryClosed));
  }
}
