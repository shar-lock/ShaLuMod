// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantCardRemoval
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantCardRemoval.cs")]
public class NMerchantCardRemoval : NMerchantSlot
{
  private const string _locTable = "merchant_room";
  private Sprite2D _removalVisual;
  private AnimationPlayer _animator;
  private Control _costContainer;
  private bool _isUnavailable;
  private MerchantCardRemovalEntry _removalEntry;

  private LocString Title => new LocString("merchant_room", "MERCHANT.cardRemovalService.title");

  private LocString Description
  {
    get => new LocString("merchant_room", "MERCHANT.cardRemovalService.description");
  }

  public override MerchantEntry Entry => (MerchantEntry) this._removalEntry;

  protected override CanvasItem Visual => (CanvasItem) this._removalVisual;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._removalVisual = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Visual"));
    this._animator = ((Node) this).GetNode<AnimationPlayer>(NodePath.op_Implicit("%Animation"));
    this._costContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Cost"));
  }

  public void FillSlot(MerchantCardRemovalEntry removalEntry)
  {
    this._removalEntry = removalEntry;
    this._removalEntry.EntryUpdated += new Action(((NMerchantSlot) this).UpdateVisual);
    this._removalEntry.PurchaseFailed += new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    this._removalEntry.PurchaseCompleted += new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
    if (!Hook.ShouldAllowMerchantCardRemoval(this.Player.RunState, this.Player))
      this._removalEntry.SetUsed();
    this.UpdateVisual();
  }

  protected override void UpdateVisual()
  {
    base.UpdateVisual();
    if (this._isUnavailable)
      return;
    if (this._removalEntry.Used)
    {
      this._hitbox.MouseFilter = (Control.MouseFilterEnum) 2L;
      this._animator.CurrentAnimation = "Used";
      this._isUnavailable = true;
      this._animator.Play((StringName) null, -1.0, 1f, false);
      ((CanvasItem) this._costLabel).Visible = false;
      ((CanvasItem) this._costContainer).Visible = false;
      this.FocusMode = (Control.FocusModeEnum) 0L;
    }
    else
    {
      this.MouseFilter = (Control.MouseFilterEnum) 0L;
      ((CanvasItem) this._costLabel).Visible = true;
      this._costLabel.SetTextAutoSize(this._removalEntry.Cost.ToString());
      ((CanvasItem) this._costLabel).Modulate = this._removalEntry.EnoughGold ? StsColors.cream : StsColors.red;
      ((CanvasItem) this._costContainer).Visible = true;
      this.FocusMode = (Control.FocusModeEnum) 2L;
    }
    this.ClearHoverTip();
  }

  protected override async Task OnTryPurchase(MerchantInventory? inventory)
  {
    int num = await this._removalEntry.OnTryPurchaseWrapper(inventory, false, true) ? 1 : 0;
  }

  protected void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
  {
    this.TriggerMerchantHandToPointHere();
    this.UpdateVisual();
  }

  public void OnCardRemovalUsed()
  {
    this._removalEntry.SetUsed();
    this.UpdateVisual();
  }

  protected override void CreateHoverTip()
  {
    LocString title = this.Title;
    LocString description = this.Description;
    description.Add("Amount", (Decimal) MerchantCardRemovalEntry.PriceIncrease);
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(title, description));
    if (andShow == null)
      return;
    andShow.GlobalPosition = this.GlobalPosition;
    double x = (double) this.GlobalPosition.X;
    Rect2 visibleRect = ((Node) this).GetViewport().GetVisibleRect();
    double num = (double) ((Rect2) ref visibleRect).Size.X * 0.5;
    if (x > num)
    {
      andShow.SetAlignment((Control) this, HoverTipAlignment.Left);
      NHoverTipSet nhoverTipSet = andShow;
      nhoverTipSet.GlobalPosition = Vector2.op_Subtraction(nhoverTipSet.GlobalPosition, Vector2.op_Multiply(Vector2.op_Multiply(this.Size, 0.5f), this.Scale));
    }
    else
    {
      NHoverTipSet nhoverTipSet = andShow;
      nhoverTipSet.GlobalPosition = Vector2.op_Addition(nhoverTipSet.GlobalPosition, Vector2.op_Addition(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, this.Size.X), 0.5f), this.Scale), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Up, this.Size.Y), 0.5f), this.Scale)));
    }
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._removalEntry.EntryUpdated -= new Action(((NMerchantSlot) this).UpdateVisual);
    this._removalEntry.PurchaseFailed -= new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    this._removalEntry.PurchaseCompleted -= new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMerchantCardRemoval.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCardRemoval.MethodName.UpdateVisual, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCardRemoval.MethodName.OnCardRemovalUsed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCardRemoval.MethodName.CreateHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantCardRemoval.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName.UpdateVisual) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisual();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName.OnCardRemovalUsed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCardRemovalUsed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName.CreateHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName.UpdateVisual) || StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName.OnCardRemovalUsed) || StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName.CreateHoverTip) || StringName.op_Equality(ref method, NMerchantCardRemoval.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._removalVisual))
    {
      this._removalVisual = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._animator))
    {
      this._animator = VariantUtils.ConvertTo<AnimationPlayer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._costContainer))
    {
      this._costContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._isUnavailable))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isUnavailable = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName.Visual))
    {
      ref godot_variant local = ref value;
      CanvasItem visual = this.Visual;
      godot_variant from = VariantUtils.CreateFrom<CanvasItem>(ref visual);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._removalVisual))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._removalVisual);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._animator))
    {
      value = VariantUtils.CreateFrom<AnimationPlayer>(ref this._animator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._costContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._costContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantCardRemoval.PropertyName._isUnavailable))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isUnavailable);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMerchantCardRemoval.PropertyName._removalVisual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCardRemoval.PropertyName._animator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCardRemoval.PropertyName._costContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantCardRemoval.PropertyName._isUnavailable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantCardRemoval.PropertyName.Visual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMerchantCardRemoval.PropertyName._removalVisual, Variant.From<Sprite2D>(ref this._removalVisual));
    info.AddProperty(NMerchantCardRemoval.PropertyName._animator, Variant.From<AnimationPlayer>(ref this._animator));
    info.AddProperty(NMerchantCardRemoval.PropertyName._costContainer, Variant.From<Control>(ref this._costContainer));
    info.AddProperty(NMerchantCardRemoval.PropertyName._isUnavailable, Variant.From<bool>(ref this._isUnavailable));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._removalVisual, ref variant1))
      this._removalVisual = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._animator, ref variant2))
      this._animator = ((Variant) ref variant2).As<AnimationPlayer>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantCardRemoval.PropertyName._costContainer, ref variant3))
      this._costContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NMerchantCardRemoval.PropertyName._isUnavailable, ref variant4))
      return;
    this._isUnavailable = ((Variant) ref variant4).As<bool>();
  }

  public new class MethodName : NMerchantSlot.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName UpdateVisual = StringName.op_Implicit(nameof (UpdateVisual));
    public static readonly StringName OnCardRemovalUsed = StringName.op_Implicit(nameof (OnCardRemovalUsed));
    public new static readonly StringName CreateHoverTip = StringName.op_Implicit(nameof (CreateHoverTip));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NMerchantSlot.PropertyName
  {
    public new static readonly StringName Visual = StringName.op_Implicit(nameof (Visual));
    public static readonly StringName _removalVisual = StringName.op_Implicit(nameof (_removalVisual));
    public static readonly StringName _animator = StringName.op_Implicit(nameof (_animator));
    public static readonly StringName _costContainer = StringName.op_Implicit(nameof (_costContainer));
    public static readonly StringName _isUnavailable = StringName.op_Implicit(nameof (_isUnavailable));
  }

  public new class SignalName : NMerchantSlot.SignalName
  {
  }
}
