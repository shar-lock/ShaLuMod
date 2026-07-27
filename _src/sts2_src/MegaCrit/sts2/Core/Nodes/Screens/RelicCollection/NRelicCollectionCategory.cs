// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection.NRelicCollectionCategory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;

[ScriptPath("res://src/Core/Nodes/Screens/RelicCollection/NRelicCollectionCategory.cs")]
public class NRelicCollectionCategory : VBoxContainer
{
  public static readonly string scenePath = SceneHelper.GetScenePath("screens/relic_collection/relic_collection_subcategory");
  private static readonly List<RelicModel> _relicModelCache = new List<RelicModel>();
  private NRelicCollection _collection;
  private MegaRichTextLabel _headerLabel;
  private GridContainer _relicsContainer;
  private readonly List<NRelicCollectionCategory> _subCategories = new List<NRelicCollectionCategory>();
  private Control _spacer;
  private TextureRect _icon;

  private NRelicCollectionCategory CreateForSubcategory()
  {
    return PreloadManager.Cache.GetScene(NRelicCollectionCategory.scenePath).Instantiate<NRelicCollectionCategory>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._headerLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Header"));
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._relicsContainer = ((Node) this).GetNode<GridContainer>(NodePath.op_Implicit("%RelicsContainer"));
    this._spacer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Spacer"));
    ((CanvasItem) this._icon).Visible = false;
  }

  public void LoadRelics(
    RelicRarity relicRarity,
    NRelicCollection collection,
    LocString header,
    HashSet<RelicModel> seenRelics,
    UnlockState unlockState,
    HashSet<RelicModel> allUnlockedRelics)
  {
    this._subCategories.Clear();
    this._headerLabel.Text = header.GetFormattedText();
    this._collection = collection;
    NRelicCollectionCategory._relicModelCache.Clear();
    NRelicCollectionCategory._relicModelCache.AddRange(ModelDb.AllRelics.Where<RelicModel>((Func<RelicModel, bool>) (relic => relic.Rarity == relicRarity)));
    if (relicRarity == RelicRarity.Starter)
    {
      List<RelicModel> list = ModelDb.AllCharacters.SelectMany<CharacterModel, RelicModel>((Func<CharacterModel, IEnumerable<RelicModel>>) (c => (IEnumerable<RelicModel>) c.StartingRelics)).ToList<RelicModel>();
      NRelicCollectionCategory forSubcategory1 = this.CreateForSubcategory();
      this._subCategories.Add(forSubcategory1);
      ((Node) this).AddChildSafely((Node) forSubcategory1);
      ((Node) this).MoveChildSafely((Node) forSubcategory1, ((Node) this._headerLabel).GetIndex(false) + 1);
      ((CanvasItem) forSubcategory1._spacer).Visible = false;
      ((CanvasItem) forSubcategory1._headerLabel).Visible = false;
      forSubcategory1.LoadSubcategory(this._collection, (LocString) null, (IEnumerable<RelicModel>) list, seenRelics, allUnlockedRelics);
      IEnumerable<RelicModel> relics = list.Select<RelicModel, RelicModel>((Func<RelicModel, RelicModel>) (r => ModelDb.Relic<TouchOfOrobas>().GetUpgradedStarterRelic(r)));
      NRelicCollectionCategory forSubcategory2 = this.CreateForSubcategory();
      this._subCategories.Add(forSubcategory2);
      ((Node) this).AddChildSafely((Node) forSubcategory2);
      ((Node) this).MoveChildSafely((Node) forSubcategory2, ((Node) this._headerLabel).GetIndex(false) + 2);
      ((CanvasItem) forSubcategory2._headerLabel).Visible = false;
      forSubcategory2.LoadSubcategory(this._collection, (LocString) null, relics, seenRelics, allUnlockedRelics);
    }
    else if (relicRarity == RelicRarity.Ancient)
    {
      List<ActModel> list1 = ModelDb.Acts.ToList<ActModel>();
      List<AncientEventModel> list2 = list1.Select<ActModel, IEnumerable<AncientEventModel>>((Func<ActModel, IEnumerable<AncientEventModel>>) (a => a.AllAncients)).SelectMany<IEnumerable<AncientEventModel>, AncientEventModel>((Func<IEnumerable<AncientEventModel>, IEnumerable<AncientEventModel>>) (a => a)).Concat<AncientEventModel>(ModelDb.AllSharedAncients).Distinct<AncientEventModel>().ToList<AncientEventModel>();
      HashSet<AncientEventModel> hashSet = list1.Select<ActModel, IEnumerable<AncientEventModel>>((Func<ActModel, IEnumerable<AncientEventModel>>) (a => a.GetUnlockedAncients(unlockState))).SelectMany<IEnumerable<AncientEventModel>, AncientEventModel>((Func<IEnumerable<AncientEventModel>, IEnumerable<AncientEventModel>>) (a => a)).Concat<AncientEventModel>(unlockState.SharedAncients).Distinct<AncientEventModel>().ToHashSet<AncientEventModel>();
      IReadOnlyDictionary<ModelId, AncientStats> ancientStats = SaveManager.Instance.Progress.AncientStats;
      LocString locString = new LocString("relic_collection", "UNKNOWN_ANCIENT");
      for (int index = 0; index < list2.Count; ++index)
      {
        AncientEventModel ancientEventModel = list2[index];
        if (hashSet.Contains(ancientEventModel))
        {
          NRelicCollectionCategory forSubcategory = this.CreateForSubcategory();
          this._subCategories.Add(forSubcategory);
          ((Node) this).AddChildSafely((Node) forSubcategory);
          ((Node) this).MoveChildSafely((Node) forSubcategory, ((Node) this._headerLabel).GetIndex(false) + index + 1);
          RelicModel[] array = ancientEventModel.AllPossibleOptions.Select<EventOption, RelicModel>((Func<EventOption, RelicModel>) (o => o.Relic?.CanonicalInstance)).OfType<RelicModel>().Intersect<RelicModel>((IEnumerable<RelicModel>) NRelicCollectionCategory._relicModelCache).OrderBy<RelicModel, string>((Func<RelicModel, string>) (r => r.Title.GetFormattedText()), (IComparer<string>) LocManager.Instance.StringComparer).ToArray<RelicModel>();
          bool flag = ancientStats.ContainsKey(ancientEventModel.Id) || ((IEnumerable<RelicModel>) array).Any<RelicModel>((Func<RelicModel, bool>) (r => seenRelics.Contains(r)));
          LocString header1 = new LocString("relic_collection", "ANCIENT_SUBCATEGORY");
          header1.Add("Ancient", flag ? ancientEventModel.Title : locString);
          forSubcategory.LoadSubcategory(this._collection, header1, (IEnumerable<RelicModel>) array, seenRelics, allUnlockedRelics);
          forSubcategory.LoadIcon(ancientEventModel.RunHistoryIcon);
        }
      }
    }
    else
    {
      List<RelicModel> relicModelList1 = new List<RelicModel>();
      List<RelicModel> relicModelList2 = new List<RelicModel>();
      foreach (RelicPoolModel characterRelicPool in ModelDb.AllCharacterRelicPools)
      {
        foreach (RelicModel relicModel in NRelicCollectionCategory._relicModelCache)
        {
          if (characterRelicPool.AllRelicIds.Contains(relicModel.Id))
            relicModelList1.Add(relicModel);
        }
      }
      foreach (RelicModel relicModel in NRelicCollectionCategory._relicModelCache)
      {
        if (!relicModelList1.Contains(relicModel))
          relicModelList2.Add(relicModel);
      }
      relicModelList2.Sort((Comparison<RelicModel>) ((p1, p2) => LocManager.Instance.StringComparer.Compare(p1.Title.GetFormattedText(), p2.Title.GetFormattedText())));
      this.LoadRelicNodes(relicModelList2.Concat<RelicModel>((IEnumerable<RelicModel>) relicModelList1), seenRelics, allUnlockedRelics);
      this._collection.AddRelics((IEnumerable<RelicModel>) relicModelList2);
      this._collection.AddRelics((IEnumerable<RelicModel>) relicModelList1);
    }
  }

  private void LoadSubcategory(
    NRelicCollection collection,
    LocString? header,
    IEnumerable<RelicModel> relics,
    HashSet<RelicModel> seenRelics,
    HashSet<RelicModel> unlockedRelics)
  {
    this._headerLabel.Text = header?.GetFormattedText() ?? "";
    this._collection = collection;
    this._collection.AddRelics(relics);
    this.LoadRelicNodes(relics, seenRelics, unlockedRelics);
  }

  private void LoadIcon(Texture2D tex)
  {
    this._icon.Texture = tex;
    ((CanvasItem) this._icon).Visible = true;
  }

  private void LoadRelicNodes(
    IEnumerable<RelicModel> relics,
    HashSet<RelicModel> seenRelics,
    HashSet<RelicModel> unlockedRelics)
  {
    foreach (Node child in ((Node) this._relicsContainer).GetChildren(false))
      child.QueueFreeSafely();
    foreach (RelicModel relic in relics)
    {
      ModelVisibility visibility = !unlockedRelics.Contains(relic) ? ModelVisibility.Locked : (!seenRelics.Contains(relic) ? ModelVisibility.NotSeen : ModelVisibility.Visible);
      NRelicCollectionEntry child = NRelicCollectionEntry.Create(relic, visibility);
      ((Node) this._relicsContainer).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Released, Callable.From<NRelicCollectionEntry>(new Action<NRelicCollectionEntry>(this.OnRelicEntryPressed)), 0U);
    }
  }

  public void ClearRelics()
  {
    foreach (Node child in ((Node) this._relicsContainer).GetChildren(false))
      child.QueueFreeSafely();
    foreach (Node node in ((IEnumerable) ((Node) this).GetChildren(false)).OfType<NRelicCollectionCategory>())
      node.QueueFreeSafely();
  }

  private void OnRelicEntryPressed(NRelicCollectionEntry entry)
  {
    NGame.Instance.GetInspectRelicScreen().Open(this._collection.Relics, entry.relic);
    this._collection.SetLastFocusedRelic(entry);
  }

  public Control? DefaultFocusedControl
  {
    get
    {
      if (this._subCategories.Any<NRelicCollectionCategory>())
        return this._subCategories.First<NRelicCollectionCategory>().DefaultFocusedControl;
      return GodotObject.IsInstanceValid((GodotObject) this._relicsContainer) && ((Node) this._relicsContainer).GetChildCount(false) > 0 ? ((Node) this._relicsContainer).GetChild<Control>(0, false) : (Control) null;
    }
  }

  public List<IReadOnlyList<Control>> GetGridItems()
  {
    List<IReadOnlyList<Control>> gridItems = new List<IReadOnlyList<Control>>();
    if (this._subCategories.Any<NRelicCollectionCategory>())
    {
      foreach (NRelicCollectionCategory subCategory in this._subCategories)
        gridItems.AddRange((IEnumerable<IReadOnlyList<Control>>) subCategory.GetGridItems());
    }
    else
    {
      for (int count = 0; count < ((Node) this._relicsContainer).GetChildren(false).Count; count += this._relicsContainer.Columns)
        gridItems.Add((IReadOnlyList<Control>) ((IEnumerable) ((Node) this._relicsContainer).GetChildren(false)).OfType<Control>().Skip<Control>(count).Take<Control>(this._relicsContainer.Columns).ToList<Control>());
    }
    return gridItems;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NRelicCollectionCategory.MethodName.CreateForSubcategory, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("VBoxContainer"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionCategory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionCategory.MethodName.LoadIcon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRelicCollectionCategory.MethodName.ClearRelics, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollectionCategory.MethodName.OnRelicEntryPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.CreateForSubcategory) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRelicCollectionCategory forSubcategory = this.CreateForSubcategory();
      ret = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref forSubcategory);
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.LoadIcon) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.LoadIcon(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.ClearRelics) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearRelics();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.OnRelicEntryPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnRelicEntryPressed(VariantUtils.ConvertTo<NRelicCollectionEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.CreateForSubcategory) || StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName._Ready) || StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.LoadIcon) || StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.ClearRelics) || StringName.op_Equality(ref method, NRelicCollectionCategory.MethodName.OnRelicEntryPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._collection))
    {
      this._collection = VariantUtils.ConvertTo<NRelicCollection>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._headerLabel))
    {
      this._headerLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._relicsContainer))
    {
      this._relicsContainer = VariantUtils.ConvertTo<GridContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._spacer))
    {
      this._spacer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._icon))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._collection))
    {
      value = VariantUtils.CreateFrom<NRelicCollection>(ref this._collection);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._headerLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._headerLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._relicsContainer))
    {
      value = VariantUtils.CreateFrom<GridContainer>(ref this._relicsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._spacer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._spacer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicCollectionCategory.PropertyName._icon))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionCategory.PropertyName._collection, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionCategory.PropertyName._headerLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionCategory.PropertyName._relicsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionCategory.PropertyName._spacer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionCategory.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollectionCategory.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRelicCollectionCategory.PropertyName._collection, Variant.From<NRelicCollection>(ref this._collection));
    info.AddProperty(NRelicCollectionCategory.PropertyName._headerLabel, Variant.From<MegaRichTextLabel>(ref this._headerLabel));
    info.AddProperty(NRelicCollectionCategory.PropertyName._relicsContainer, Variant.From<GridContainer>(ref this._relicsContainer));
    info.AddProperty(NRelicCollectionCategory.PropertyName._spacer, Variant.From<Control>(ref this._spacer));
    info.AddProperty(NRelicCollectionCategory.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._collection, ref variant1))
      this._collection = ((Variant) ref variant1).As<NRelicCollection>();
    Variant variant2;
    if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._headerLabel, ref variant2))
      this._headerLabel = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._relicsContainer, ref variant3))
      this._relicsContainer = ((Variant) ref variant3).As<GridContainer>();
    Variant variant4;
    if (info.TryGetProperty(NRelicCollectionCategory.PropertyName._spacer, ref variant4))
      this._spacer = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (!info.TryGetProperty(NRelicCollectionCategory.PropertyName._icon, ref variant5))
      return;
    this._icon = ((Variant) ref variant5).As<TextureRect>();
  }

  public class MethodName : VBoxContainer.MethodName
  {
    public static readonly StringName CreateForSubcategory = StringName.op_Implicit(nameof (CreateForSubcategory));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName LoadIcon = StringName.op_Implicit(nameof (LoadIcon));
    public static readonly StringName ClearRelics = StringName.op_Implicit(nameof (ClearRelics));
    public static readonly StringName OnRelicEntryPressed = StringName.op_Implicit(nameof (OnRelicEntryPressed));
  }

  public class PropertyName : VBoxContainer.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _collection = StringName.op_Implicit(nameof (_collection));
    public static readonly StringName _headerLabel = StringName.op_Implicit(nameof (_headerLabel));
    public static readonly StringName _relicsContainer = StringName.op_Implicit(nameof (_relicsContainer));
    public static readonly StringName _spacer = StringName.op_Implicit(nameof (_spacer));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
  }

  public class SignalName : VBoxContainer.SignalName
  {
  }
}
