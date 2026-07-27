// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantPotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Potions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantPotion.cs")]
public class NMerchantPotion : NMerchantSlot
{
  private Control _potionHolder;
  private NPotion? _potionNode;
  private MerchantPotionEntry _potionEntry;
  private PotionModel? _potion;
  private Vector2 _potionNodePosition;

  public override MerchantEntry Entry => (MerchantEntry) this._potionEntry;

  protected override CanvasItem Visual => (CanvasItem) this._potionHolder;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._potionHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PotionHolder"));
  }

  public void FillSlot(MerchantPotionEntry potionEntry)
  {
    this._potionEntry = potionEntry;
    this._potion = potionEntry.Model;
    this._potionEntry.EntryUpdated += new Action(((NMerchantSlot) this).UpdateVisual);
    this._potionEntry.PurchaseFailed += new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    this._potionEntry.PurchaseCompleted += new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
    this.UpdateVisual();
  }

  protected override void UpdateVisual()
  {
    base.UpdateVisual();
    if (this._potionEntry.Model == null)
    {
      ((CanvasItem) this).Visible = false;
      this.MouseFilter = (Control.MouseFilterEnum) 2L;
      if (this._potionNode != null)
      {
        ((Node) this._potionNode).QueueFreeSafely();
        this._potionNode = (NPotion) null;
      }
      this.ClearHoverTip();
    }
    else
    {
      if (this._potionNode != null && this._potionNode.Model != this._potionEntry.Model)
      {
        ((Node) this._potionNode).QueueFreeSafely();
        this._potionNode = (NPotion) null;
      }
      if (this._potionNode == null)
      {
        this._potionNode = NPotion.Create(this._potionEntry.Model);
        ((Node) this._potionHolder).AddChildSafely((Node) this._potionNode);
        this._potionNode.Position = Vector2.Zero;
      }
      this._potionNodePosition = this._potionNode.GlobalPosition;
      this._costLabel.SetTextAutoSize(this._potionEntry.Cost.ToString());
      ((CanvasItem) this._costLabel).Modulate = this._potionEntry.EnoughGold ? StsColors.cream : StsColors.red;
    }
  }

  protected override async Task OnTryPurchase(MerchantInventory? inventory)
  {
    int num = await this._potionEntry.OnTryPurchaseWrapper(inventory) ? 1 : 0;
  }

  protected void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
  {
    this.TriggerMerchantHandToPointHere();
    NRun.Instance?.GlobalUi.TopBar.PotionContainer.AnimatePotion(this._potion, new Vector2?(this._potionNodePosition));
    this.UpdateVisual();
    this._potion = this._potionEntry.Model;
  }

  protected override void CreateHoverTip()
  {
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, this._potionNode.Model.HoverTips);
    andShow?.SetGlobalPosition(this.GlobalPosition, false);
    if (andShow == null)
      return;
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
      andShow.SetAlignment((Control) this, HoverTipAlignment.Right);
      NHoverTipSet nhoverTipSet = andShow;
      nhoverTipSet.GlobalPosition = Vector2.op_Addition(nhoverTipSet.GlobalPosition, Vector2.op_Addition(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Right, this.Size.X), 0.5f), this.Scale), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Up, this.Size.Y), 0.5f), this.Scale)));
    }
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._potionEntry.EntryUpdated -= new Action(((NMerchantSlot) this).UpdateVisual);
    this._potionEntry.PurchaseFailed -= new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    this._potionEntry.PurchaseCompleted -= new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NMerchantPotion.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantPotion.MethodName.UpdateVisual, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantPotion.MethodName.CreateHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantPotion.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantPotion.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantPotion.MethodName.UpdateVisual) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisual();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantPotion.MethodName.CreateHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantPotion.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantPotion.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantPotion.MethodName.UpdateVisual) || StringName.op_Equality(ref method, NMerchantPotion.MethodName.CreateHoverTip) || StringName.op_Equality(ref method, NMerchantPotion.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantPotion.PropertyName._potionHolder))
    {
      this._potionHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantPotion.PropertyName._potionNode))
    {
      this._potionNode = VariantUtils.ConvertTo<NPotion>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantPotion.PropertyName._potionNodePosition))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._potionNodePosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantPotion.PropertyName.Visual))
    {
      ref godot_variant local = ref value;
      CanvasItem visual = this.Visual;
      godot_variant from = VariantUtils.CreateFrom<CanvasItem>(ref visual);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantPotion.PropertyName._potionHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantPotion.PropertyName._potionNode))
    {
      value = VariantUtils.CreateFrom<NPotion>(ref this._potionNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantPotion.PropertyName._potionNodePosition))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._potionNodePosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMerchantPotion.PropertyName._potionHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantPotion.PropertyName._potionNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantPotion.PropertyName.Visual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMerchantPotion.PropertyName._potionNodePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMerchantPotion.PropertyName._potionHolder, Variant.From<Control>(ref this._potionHolder));
    info.AddProperty(NMerchantPotion.PropertyName._potionNode, Variant.From<NPotion>(ref this._potionNode));
    info.AddProperty(NMerchantPotion.PropertyName._potionNodePosition, Variant.From<Vector2>(ref this._potionNodePosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantPotion.PropertyName._potionHolder, ref variant1))
      this._potionHolder = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantPotion.PropertyName._potionNode, ref variant2))
      this._potionNode = ((Variant) ref variant2).As<NPotion>();
    Variant variant3;
    if (!info.TryGetProperty(NMerchantPotion.PropertyName._potionNodePosition, ref variant3))
      return;
    this._potionNodePosition = ((Variant) ref variant3).As<Vector2>();
  }

  public new class MethodName : NMerchantSlot.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName UpdateVisual = StringName.op_Implicit(nameof (UpdateVisual));
    public new static readonly StringName CreateHoverTip = StringName.op_Implicit(nameof (CreateHoverTip));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NMerchantSlot.PropertyName
  {
    public new static readonly StringName Visual = StringName.op_Implicit(nameof (Visual));
    public static readonly StringName _potionHolder = StringName.op_Implicit(nameof (_potionHolder));
    public static readonly StringName _potionNode = StringName.op_Implicit(nameof (_potionNode));
    public static readonly StringName _potionNodePosition = StringName.op_Implicit(nameof (_potionNodePosition));
  }

  public new class SignalName : NMerchantSlot.SignalName
  {
  }
}
