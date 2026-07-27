// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantRelic
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
using MegaCrit.Sts2.Core.Nodes.Relics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantRelic.cs")]
public class NMerchantRelic : NMerchantSlot
{
  [Export]
  private NRelic.IconSize _iconSize;
  private Control _relicHolder;
  private NRelic? _relicNode;
  private MerchantRelicEntry _relicEntry;
  private RelicModel? _relic;
  private Vector2 _relicNodePosition;

  public override MerchantEntry Entry => (MerchantEntry) this._relicEntry;

  protected override CanvasItem Visual => (CanvasItem) this._relicHolder;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._relicHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RelicHolder"));
  }

  public void FillSlot(MerchantRelicEntry relicEntry)
  {
    this._relicEntry = relicEntry;
    this._relic = relicEntry.Model;
    relicEntry.EntryUpdated += new Action(((NMerchantSlot) this).UpdateVisual);
    relicEntry.PurchaseFailed += new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    relicEntry.PurchaseCompleted += new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
    this.UpdateVisual();
  }

  protected override void UpdateVisual()
  {
    base.UpdateVisual();
    if (this._relicEntry.Model == null)
    {
      ((CanvasItem) this).Visible = false;
      this.MouseFilter = (Control.MouseFilterEnum) 2L;
      if (this._relicNode != null)
      {
        ((Node) this._relicNode).QueueFreeSafely();
        this._relicNode = (NRelic) null;
      }
      this.ClearHoverTip();
    }
    else
    {
      if (this._relicNode != null && this._relicNode.Model != this._relicEntry.Model)
      {
        ((Node) this._relicNode).QueueFreeSafely();
        this._relicNode = (NRelic) null;
      }
      if (this._relicNode == null)
      {
        this._relicNode = NRelic.Create(this._relicEntry.Model, this._iconSize);
        ((Node) this._relicHolder).AddChildSafely((Node) this._relicNode);
        if (this._iconSize == NRelic.IconSize.Large)
        {
          this._relicNode.Size = new Vector2(128f, 128f);
          TextureRect icon = this._relicNode.Icon;
          ((Control) icon).Position = Vector2.op_Subtraction(((Control) icon).Position, new Vector2(0.0f, ((Control) this._costLabel).Size.Y));
        }
        this.Hitbox.Size = ((Control) this._relicNode.Icon).Size;
        this.Hitbox.Scale = this._relicHolder.Scale;
        this.Hitbox.GlobalPosition = ((Control) this._relicNode.Icon).GlobalPosition;
      }
      this._relicNodePosition = ((Control) this._relicNode.Icon).GlobalPosition;
      this._costLabel.SetTextAutoSize(this._relicEntry.Cost.ToString());
      ((CanvasItem) this._costLabel).Modulate = this._relicEntry.EnoughGold ? StsColors.cream : StsColors.red;
    }
  }

  protected override async Task OnTryPurchase(MerchantInventory? inventory)
  {
    int num = await this._relicEntry.OnTryPurchaseWrapper(inventory) ? 1 : 0;
  }

  private void OnSuccessfulPurchase(PurchaseStatus _, MerchantEntry __)
  {
    this.TriggerMerchantHandToPointHere();
    NRun.Instance?.GlobalUi.RelicInventory.AnimateRelic(this._relic, new Vector2?(this._relicNodePosition));
    this.UpdateVisual();
    this._relic = this._relicEntry.Model;
  }

  protected override void CreateHoverTip()
  {
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, this._relicNode.Model.HoverTips);
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

  protected override void OnPreview()
  {
    this.ClearHoverTip();
    int capacity = 1;
    List<RelicModel> relicModelList = new List<RelicModel>(capacity);
    CollectionsMarshal.SetCount<RelicModel>(relicModelList, capacity);
    CollectionsMarshal.AsSpan<RelicModel>(relicModelList)[0] = this._relicNode.Model;
    List<RelicModel> relics = relicModelList;
    NGame.Instance.GetInspectRelicScreen().Open((IReadOnlyList<RelicModel>) relics, this._relicNode.Model);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._relicEntry.EntryUpdated -= new Action(((NMerchantSlot) this).UpdateVisual);
    this._relicEntry.PurchaseFailed -= new Action<PurchaseStatus>(((NMerchantSlot) this).OnPurchaseFailed);
    this._relicEntry.PurchaseCompleted -= new Action<PurchaseStatus, MerchantEntry>(this.OnSuccessfulPurchase);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMerchantRelic.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRelic.MethodName.UpdateVisual, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRelic.MethodName.CreateHoverTip, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRelic.MethodName.OnPreview, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantRelic.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantRelic.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRelic.MethodName.UpdateVisual) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisual();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRelic.MethodName.CreateHoverTip) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTip();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantRelic.MethodName.OnPreview) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPreview();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantRelic.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantRelic.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantRelic.MethodName.UpdateVisual) || StringName.op_Equality(ref method, NMerchantRelic.MethodName.CreateHoverTip) || StringName.op_Equality(ref method, NMerchantRelic.MethodName.OnPreview) || StringName.op_Equality(ref method, NMerchantRelic.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName._iconSize))
    {
      this._iconSize = VariantUtils.ConvertTo<NRelic.IconSize>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName._relicHolder))
    {
      this._relicHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName._relicNode))
    {
      this._relicNode = VariantUtils.ConvertTo<NRelic>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantRelic.PropertyName._relicNodePosition))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._relicNodePosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName.Visual))
    {
      ref godot_variant local = ref value;
      CanvasItem visual = this.Visual;
      godot_variant from = VariantUtils.CreateFrom<CanvasItem>(ref visual);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName._iconSize))
    {
      value = VariantUtils.CreateFrom<NRelic.IconSize>(ref this._iconSize);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName._relicHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._relicHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantRelic.PropertyName._relicNode))
    {
      value = VariantUtils.CreateFrom<NRelic>(ref this._relicNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantRelic.PropertyName._relicNodePosition))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._relicNodePosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMerchantRelic.PropertyName._iconSize, (PropertyHint) 2L, "Small,Large", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NMerchantRelic.PropertyName._relicHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRelic.PropertyName._relicNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantRelic.PropertyName.Visual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMerchantRelic.PropertyName._relicNodePosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMerchantRelic.PropertyName._iconSize, Variant.From<NRelic.IconSize>(ref this._iconSize));
    info.AddProperty(NMerchantRelic.PropertyName._relicHolder, Variant.From<Control>(ref this._relicHolder));
    info.AddProperty(NMerchantRelic.PropertyName._relicNode, Variant.From<NRelic>(ref this._relicNode));
    info.AddProperty(NMerchantRelic.PropertyName._relicNodePosition, Variant.From<Vector2>(ref this._relicNodePosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantRelic.PropertyName._iconSize, ref variant1))
      this._iconSize = ((Variant) ref variant1).As<NRelic.IconSize>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantRelic.PropertyName._relicHolder, ref variant2))
      this._relicHolder = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantRelic.PropertyName._relicNode, ref variant3))
      this._relicNode = ((Variant) ref variant3).As<NRelic>();
    Variant variant4;
    if (!info.TryGetProperty(NMerchantRelic.PropertyName._relicNodePosition, ref variant4))
      return;
    this._relicNodePosition = ((Variant) ref variant4).As<Vector2>();
  }

  public new class MethodName : NMerchantSlot.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName UpdateVisual = StringName.op_Implicit(nameof (UpdateVisual));
    public new static readonly StringName CreateHoverTip = StringName.op_Implicit(nameof (CreateHoverTip));
    public new static readonly StringName OnPreview = StringName.op_Implicit(nameof (OnPreview));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NMerchantSlot.PropertyName
  {
    public new static readonly StringName Visual = StringName.op_Implicit(nameof (Visual));
    public static readonly StringName _iconSize = StringName.op_Implicit(nameof (_iconSize));
    public static readonly StringName _relicHolder = StringName.op_Implicit(nameof (_relicHolder));
    public static readonly StringName _relicNode = StringName.op_Implicit(nameof (_relicNode));
    public static readonly StringName _relicNodePosition = StringName.op_Implicit(nameof (_relicNodePosition));
  }

  public new class SignalName : NMerchantSlot.SignalName
  {
  }
}
