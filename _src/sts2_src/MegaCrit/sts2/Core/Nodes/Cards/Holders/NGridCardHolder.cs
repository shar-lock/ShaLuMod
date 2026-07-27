// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.Holders.NGridCardHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Pooling;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders;

[ScriptPath("res://src/Core/Nodes/Cards/Holders/NGridCardHolder.cs")]
public class NGridCardHolder : NCardHolder, IPoolable
{
  private CardModel _baseCard;
  private CardModel? _upgradedCard;
  private bool _isPreviewingUpgrade;

  private static string ScenePath => SceneHelper.GetScenePath("cards/holders/grid_card_holder");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NGridCardHolder.ScenePath);
    }
  }

  public NCardLibraryStats? CardLibraryStats { get; private set; }

  public override CardModel CardModel => this._baseCard;

  public override bool IsShowingUpgradedCard
  {
    get => this._isPreviewingUpgrade || base.IsShowingUpgradedCard;
  }

  public static void InitPool() => NodePool.Init<NGridCardHolder>(NGridCardHolder.ScenePath, 30);

  public static NGridCardHolder? Create(NCard cardNode)
  {
    if (TestMode.IsOn)
      return (NGridCardHolder) null;
    NGridCardHolder ngridCardHolder = NodePool.Get<NGridCardHolder>();
    ngridCardHolder.SetCard(cardNode);
    ngridCardHolder.UpdateCardModel();
    ngridCardHolder.UpdateName();
    ngridCardHolder.Scale = ngridCardHolder.SmallScale;
    return ngridCardHolder;
  }

  private void UpdateCardModel()
  {
    CardModel model = this.CardNode.Model;
    this._baseCard = model;
    if (!model.IsUpgradable)
      return;
    this._upgradedCard = (CardModel) model.MutableClone();
    this._upgradedCard.UpgradeInternal();
    if (!((Node) this).IsNodeReady())
      return;
    bool previewingUpgrade = this._isPreviewingUpgrade;
    this._isPreviewingUpgrade = false;
    this.SetIsPreviewingUpgrade(previewingUpgrade);
  }

  public void OnInstantiated()
  {
  }

  public override void _Ready()
  {
    bool previewingUpgrade = this._isPreviewingUpgrade;
    this._isPreviewingUpgrade = false;
    this.SetIsPreviewingUpgrade(previewingUpgrade);
    this.ConnectSignals();
  }

  public void EnsureCardLibraryStatsExists()
  {
    if (this.CardLibraryStats != null)
      return;
    this.CardLibraryStats = NCardLibraryStats.Create();
    ((Node) this).AddChildSafely((Node) this.CardLibraryStats);
  }

  protected override void OnCardReassigned()
  {
    this.UpdateCardModel();
    this.UpdateName();
  }

  protected override void SetCard(NCard node)
  {
    base.SetCard(node);
    if (this.CardLibraryStats == null)
      return;
    ((Node) this).MoveChildSafely((Node) this.CardLibraryStats, ((Node) this).GetChildCount(false) - 1);
  }

  private void UpdateName()
  {
    ((Node) this).Name = StringName.op_Implicit($"GridCardHolder-{this.CardNode.Model.Id}");
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    ((CanvasItem) this).MoveToFrontSafely();
  }

  public void SetIsPreviewingUpgrade(bool showUpgradePreview)
  {
    if (!((CanvasItem) this).Visible)
      return;
    if (!this._baseCard.IsUpgradable & showUpgradePreview)
      throw new InvalidExpressionException($"{this._baseCard.Id} is not upgradable.");
    if (this._isPreviewingUpgrade == showUpgradePreview)
      return;
    if (showUpgradePreview && this._upgradedCard != null)
    {
      this.CardNode.Model = this._upgradedCard;
      this.CardNode.ShowUpgradePreview();
    }
    else
    {
      this.CardNode.Model = this._baseCard;
      this.CardNode.UpdateVisuals(this.CardNode.DisplayingPile, CardPreviewMode.Normal);
    }
    this._isPreviewingUpgrade = showUpgradePreview;
  }

  public override void _ExitTree()
  {
    if (((Node) this).IsAncestorOf((Node) this.CardNode))
    {
      NCard cardNode = this.CardNode;
      if (cardNode != null)
        ((Node) cardNode).QueueFreeSafely();
    }
    this.CardNode = (NCard) null;
  }

  public void OnReturnedFromPool()
  {
    if (!((Node) this).IsNodeReady())
      return;
    this.Position = Vector2.Zero;
    this.Rotation = 0.0f;
    this.Scale = Vector2.One;
    ((CanvasItem) this).Modulate = Colors.White;
    ((CanvasItem) this).Visible = true;
    this.SetClickable(true);
    this.Hitbox.MouseDefaultCursorShape = (Control.CursorShape) 0L;
    this._isPreviewingUpgrade = false;
    if (this.CardLibraryStats == null)
      return;
    ((CanvasItem) this.CardLibraryStats).Visible = false;
    ((CanvasItem) this.CardLibraryStats).Modulate = Colors.White;
  }

  public void OnFreedToPool()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NGridCardHolder.MethodName.InitPool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.UpdateCardModel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.OnInstantiated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.EnsureCardLibraryStatsExists, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.OnCardReassigned, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.SetCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.UpdateName, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.SetIsPreviewingUpgrade, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showUpgradePreview"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.OnReturnedFromPool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGridCardHolder.MethodName.OnFreedToPool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.InitPool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGridCardHolder.InitPool();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NGridCardHolder ngridCardHolder = NGridCardHolder.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NGridCardHolder>(ref ngridCardHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.UpdateCardModel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateCardModel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnInstantiated) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnInstantiated();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.EnsureCardLibraryStatsExists) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnsureCardLibraryStatsExists();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnCardReassigned) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnCardReassigned();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.SetCard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCard(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.UpdateName) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateName();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.SetIsPreviewingUpgrade) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsPreviewingUpgrade(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnReturnedFromPool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnReturnedFromPool();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnFreedToPool) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnFreedToPool();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.InitPool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGridCardHolder.InitPool();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGridCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NGridCardHolder ngridCardHolder = NGridCardHolder.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NGridCardHolder>(ref ngridCardHolder);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGridCardHolder.MethodName.InitPool) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.Create) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.UpdateCardModel) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnInstantiated) || StringName.op_Equality(ref method, NGridCardHolder.MethodName._Ready) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.EnsureCardLibraryStatsExists) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnCardReassigned) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.SetCard) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.UpdateName) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.SetIsPreviewingUpgrade) || StringName.op_Equality(ref method, NGridCardHolder.MethodName._ExitTree) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnReturnedFromPool) || StringName.op_Equality(ref method, NGridCardHolder.MethodName.OnFreedToPool) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGridCardHolder.PropertyName.CardLibraryStats))
    {
      this.CardLibraryStats = VariantUtils.ConvertTo<NCardLibraryStats>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGridCardHolder.PropertyName._isPreviewingUpgrade))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isPreviewingUpgrade = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGridCardHolder.PropertyName.CardLibraryStats))
    {
      ref godot_variant local = ref value;
      NCardLibraryStats cardLibraryStats = this.CardLibraryStats;
      godot_variant from = VariantUtils.CreateFrom<NCardLibraryStats>(ref cardLibraryStats);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGridCardHolder.PropertyName.IsShowingUpgradedCard))
    {
      ref godot_variant local = ref value;
      bool showingUpgradedCard = this.IsShowingUpgradedCard;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showingUpgradedCard);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NGridCardHolder.PropertyName._isPreviewingUpgrade))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isPreviewingUpgrade);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGridCardHolder.PropertyName.CardLibraryStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NGridCardHolder.PropertyName._isPreviewingUpgrade, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NGridCardHolder.PropertyName.IsShowingUpgradedCard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName cardLibraryStats1 = NGridCardHolder.PropertyName.CardLibraryStats;
    NCardLibraryStats cardLibraryStats2 = this.CardLibraryStats;
    Variant variant = Variant.From<NCardLibraryStats>(ref cardLibraryStats2);
    serializationInfo.AddProperty(cardLibraryStats1, variant);
    info.AddProperty(NGridCardHolder.PropertyName._isPreviewingUpgrade, Variant.From<bool>(ref this._isPreviewingUpgrade));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGridCardHolder.PropertyName.CardLibraryStats, ref variant1))
      this.CardLibraryStats = ((Variant) ref variant1).As<NCardLibraryStats>();
    Variant variant2;
    if (!info.TryGetProperty(NGridCardHolder.PropertyName._isPreviewingUpgrade, ref variant2))
      return;
    this._isPreviewingUpgrade = ((Variant) ref variant2).As<bool>();
  }

  public new class MethodName : NCardHolder.MethodName
  {
    public static readonly StringName InitPool = StringName.op_Implicit(nameof (InitPool));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName UpdateCardModel = StringName.op_Implicit(nameof (UpdateCardModel));
    public static readonly StringName OnInstantiated = StringName.op_Implicit(nameof (OnInstantiated));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName EnsureCardLibraryStatsExists = StringName.op_Implicit(nameof (EnsureCardLibraryStatsExists));
    public new static readonly StringName OnCardReassigned = StringName.op_Implicit(nameof (OnCardReassigned));
    public new static readonly StringName SetCard = StringName.op_Implicit(nameof (SetCard));
    public static readonly StringName UpdateName = StringName.op_Implicit(nameof (UpdateName));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName SetIsPreviewingUpgrade = StringName.op_Implicit(nameof (SetIsPreviewingUpgrade));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnReturnedFromPool = StringName.op_Implicit(nameof (OnReturnedFromPool));
    public static readonly StringName OnFreedToPool = StringName.op_Implicit(nameof (OnFreedToPool));
  }

  public new class PropertyName : NCardHolder.PropertyName
  {
    public static readonly StringName CardLibraryStats = StringName.op_Implicit(nameof (CardLibraryStats));
    public new static readonly StringName IsShowingUpgradedCard = StringName.op_Implicit(nameof (IsShowingUpgradedCard));
    public static readonly StringName _isPreviewingUpgrade = StringName.op_Implicit(nameof (_isPreviewingUpgrade));
  }

  public new class SignalName : NCardHolder.SignalName
  {
  }
}
