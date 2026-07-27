// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection.NRelicCollection
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;

[ScriptPath("res://src/Core/Nodes/Screens/RelicCollection/NRelicCollection.cs")]
public class NRelicCollection : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/relic_collection/relic_collection");
  private NScrollableContainer _screenContents;
  private NRelicCollectionCategory _starter;
  private NRelicCollectionCategory _common;
  private NRelicCollectionCategory _uncommon;
  private NRelicCollectionCategory _rare;
  private NRelicCollectionCategory _shop;
  private NRelicCollectionCategory _ancient;
  private NRelicCollectionCategory _event;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private Tween? _screenTween;
  private Task? _loadTask;
  private readonly List<RelicModel> _relics = new List<RelicModel>();

  public IReadOnlyList<RelicModel> Relics => (IReadOnlyList<RelicModel>) this._relics;

  public static string[] AssetPaths
  {
    get
    {
      return new string[4]
      {
        NRelicCollection._scenePath,
        NRelicCollectionEntry.scenePath,
        NRelicCollectionCategory.scenePath,
        NRelicCollectionEntry.lockedIconPath
      };
    }
  }

  public static NRelicCollection? Create()
  {
    return TestMode.IsOn ? (NRelicCollection) null : PreloadManager.Cache.GetScene(NRelicCollection._scenePath).Instantiate<NRelicCollection>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._screenContents = ((Node) this).GetNode<NScrollableContainer>(NodePath.op_Implicit("%ScreenContents"));
    this._starter = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Starter"));
    this._common = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Common"));
    this._uncommon = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Uncommon"));
    this._rare = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Rare"));
    this._shop = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Shop"));
    this._ancient = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Ancient"));
    this._event = ((Node) this).GetNode<NRelicCollectionCategory>(NodePath.op_Implicit("%Event"));
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree() => this._cts.Cancel();

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    this._relics.Clear();
    this._loadTask = TaskHelper.RunSafely(this.LoadRelics());
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    this._screenTween?.Kill();
    this.ClearRelics();
  }

  protected override void OnSubmenuShown()
  {
    base.OnSubmenuShown();
    TaskHelper.RunSafely(this.TweenAfterLoading());
  }

  private async Task TweenAfterLoading()
  {
    ((CanvasItem) this._screenContents).Modulate = new Color(1f, 1f, 1f, 0.0f);
    if (this._loadTask != null)
      await this._loadTask;
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween();
    this._screenTween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
  }

  private async Task LoadRelics()
  {
    ((CanvasItem) this._starter).Modulate = Colors.Transparent;
    ((CanvasItem) this._common).Modulate = Colors.Transparent;
    ((CanvasItem) this._uncommon).Modulate = Colors.Transparent;
    ((CanvasItem) this._rare).Modulate = Colors.Transparent;
    ((CanvasItem) this._shop).Modulate = Colors.Transparent;
    ((CanvasItem) this._ancient).Modulate = Colors.Transparent;
    ((CanvasItem) this._event).Modulate = Colors.Transparent;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    HashSet<RelicModel> hashSet1 = ((IEnumerable<ModelId>) SaveManager.Instance.Progress.DiscoveredRelics).Select<ModelId, RelicModel>(NRelicCollection.\u003C\u003EO.\u003C0\u003E__GetByIdOrNull ?? (NRelicCollection.\u003C\u003EO.\u003C0\u003E__GetByIdOrNull = new Func<ModelId, RelicModel>(ModelDb.GetByIdOrNull<RelicModel>))).OfType<RelicModel>().ToHashSet<RelicModel>();
    UnlockState stateFromProgress = SaveManager.Instance.GenerateUnlockStateFromProgress();
    HashSet<RelicModel> hashSet2 = stateFromProgress.Relics.ToHashSet<RelicModel>();
    this._starter.LoadRelics(RelicRarity.Starter, this, new LocString("relic_collection", "STARTER"), hashSet1, stateFromProgress, hashSet2);
    this._common.LoadRelics(RelicRarity.Common, this, new LocString("relic_collection", "COMMON"), hashSet1, stateFromProgress, hashSet2);
    this._uncommon.LoadRelics(RelicRarity.Uncommon, this, new LocString("relic_collection", "UNCOMMON"), hashSet1, stateFromProgress, hashSet2);
    this._rare.LoadRelics(RelicRarity.Rare, this, new LocString("relic_collection", "RARE"), hashSet1, stateFromProgress, hashSet2);
    this._shop.LoadRelics(RelicRarity.Shop, this, new LocString("relic_collection", "SHOP"), hashSet1, stateFromProgress, hashSet2);
    this._ancient.LoadRelics(RelicRarity.Ancient, this, new LocString("relic_collection", "ANCIENT"), hashSet1, stateFromProgress, hashSet2);
    this._event.LoadRelics(RelicRarity.Event, this, new LocString("relic_collection", "EVENT"), hashSet1, stateFromProgress, hashSet2);
    List<IReadOnlyList<Control>> controlListList = new List<IReadOnlyList<Control>>();
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._starter.GetGridItems());
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._common.GetGridItems());
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._uncommon.GetGridItems());
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._rare.GetGridItems());
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._shop.GetGridItems());
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._ancient.GetGridItems());
    controlListList.AddRange((IEnumerable<IReadOnlyList<Control>>) this._event.GetGridItems());
    for (int index1 = 0; index1 < controlListList.Count; ++index1)
    {
      for (int index2 = 0; index2 < controlListList[index1].Count; ++index2)
      {
        Control control1 = controlListList[index1][index2];
        Control control2 = control1;
        NodePath path;
        if (index2 <= 0)
        {
          IReadOnlyList<Control> controlList = controlListList[index1];
          path = ((Node) controlList[controlList.Count - 1]).GetPath();
        }
        else
          path = ((Node) controlListList[index1][index2 - 1]).GetPath();
        control2.FocusNeighborLeft = path;
        control1.FocusNeighborRight = index2 < controlListList[index1].Count - 1 ? ((Node) controlListList[index1][index2 + 1]).GetPath() : ((Node) controlListList[index1][0]).GetPath();
        control1.FocusNeighborTop = index1 <= 0 ? ((Node) controlListList[index1][index2]).GetPath() : (index2 < controlListList[index1 - 1].Count ? ((Node) controlListList[index1 - 1][index2]).GetPath() : ((Node) controlListList[index1 - 1][controlListList[index1 - 1].Count - 1]).GetPath());
        control1.FocusNeighborBottom = index1 >= controlListList.Count - 1 ? ((Node) controlListList[index1][index2]).GetPath() : (index2 < controlListList[index1 + 1].Count ? ((Node) controlListList[index1 + 1][index2]).GetPath() : ((Node) controlListList[index1 + 1][controlListList[index1 + 1].Count - 1]).GetPath());
      }
    }
    double num = (double) await ((Node) this).AwaitProcessFrame(this._cts.Token);
    ((CanvasItem) this._starter).Modulate = Colors.White;
    ((CanvasItem) this._common).Modulate = Colors.White;
    ((CanvasItem) this._uncommon).Modulate = Colors.White;
    ((CanvasItem) this._rare).Modulate = Colors.White;
    ((CanvasItem) this._shop).Modulate = Colors.White;
    ((CanvasItem) this._ancient).Modulate = Colors.White;
    ((CanvasItem) this._event).Modulate = Colors.White;
    this._screenContents.InstantlyScrollToTop();
    Control initialFocusedControl = this.InitialFocusedControl;
    if (initialFocusedControl == null)
      return;
    initialFocusedControl.TryGrabFocus();
  }

  public void AddRelics(IEnumerable<RelicModel> relics) => this._relics.AddRange(relics);

  private void ClearRelics()
  {
    this._starter.ClearRelics();
    this._common.ClearRelics();
    this._uncommon.ClearRelics();
    this._rare.ClearRelics();
    this._shop.ClearRelics();
    this._ancient.ClearRelics();
    this._event.ClearRelics();
    this._relics.Clear();
  }

  public void SetLastFocusedRelic(NRelicCollectionEntry relic)
  {
    this._lastFocusedControl = (Control) relic;
  }

  protected override Control? InitialFocusedControl => this._starter.DefaultFocusedControl;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NRelicCollection.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName.OnSubmenuShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName.ClearRelics, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRelicCollection.MethodName.SetLastFocusedRelic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("relic"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRelicCollection nrelicCollection = NRelicCollection.Create();
      ret = VariantUtils.CreateFrom<NRelicCollection>(ref nrelicCollection);
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName.OnSubmenuShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName.ClearRelics) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearRelics();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRelicCollection.MethodName.SetLastFocusedRelic) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetLastFocusedRelic(VariantUtils.ConvertTo<NRelicCollectionEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRelicCollection.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRelicCollection nrelicCollection = NRelicCollection.Create();
      ret = VariantUtils.CreateFrom<NRelicCollection>(ref nrelicCollection);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRelicCollection.MethodName.Create) || StringName.op_Equality(ref method, NRelicCollection.MethodName._Ready) || StringName.op_Equality(ref method, NRelicCollection.MethodName._EnterTree) || StringName.op_Equality(ref method, NRelicCollection.MethodName._ExitTree) || StringName.op_Equality(ref method, NRelicCollection.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NRelicCollection.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NRelicCollection.MethodName.OnSubmenuShown) || StringName.op_Equality(ref method, NRelicCollection.MethodName.ClearRelics) || StringName.op_Equality(ref method, NRelicCollection.MethodName.SetLastFocusedRelic) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._screenContents))
    {
      this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._starter))
    {
      this._starter = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._common))
    {
      this._common = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._uncommon))
    {
      this._uncommon = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._rare))
    {
      this._rare = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._shop))
    {
      this._shop = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._ancient))
    {
      this._ancient = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._event))
    {
      this._event = VariantUtils.ConvertTo<NRelicCollectionCategory>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicCollection.PropertyName._screenTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._screenContents))
    {
      value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._starter))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._starter);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._common))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._common);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._uncommon))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._uncommon);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._rare))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._rare);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._shop))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._shop);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._ancient))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._ancient);
      return true;
    }
    if (StringName.op_Equality(ref name, NRelicCollection.PropertyName._event))
    {
      value = VariantUtils.CreateFrom<NRelicCollectionCategory>(ref this._event);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRelicCollection.PropertyName._screenTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._screenContents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._starter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._common, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._uncommon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._rare, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._shop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._ancient, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._event, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName._screenTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRelicCollection.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRelicCollection.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
    info.AddProperty(NRelicCollection.PropertyName._starter, Variant.From<NRelicCollectionCategory>(ref this._starter));
    info.AddProperty(NRelicCollection.PropertyName._common, Variant.From<NRelicCollectionCategory>(ref this._common));
    info.AddProperty(NRelicCollection.PropertyName._uncommon, Variant.From<NRelicCollectionCategory>(ref this._uncommon));
    info.AddProperty(NRelicCollection.PropertyName._rare, Variant.From<NRelicCollectionCategory>(ref this._rare));
    info.AddProperty(NRelicCollection.PropertyName._shop, Variant.From<NRelicCollectionCategory>(ref this._shop));
    info.AddProperty(NRelicCollection.PropertyName._ancient, Variant.From<NRelicCollectionCategory>(ref this._ancient));
    info.AddProperty(NRelicCollection.PropertyName._event, Variant.From<NRelicCollectionCategory>(ref this._event));
    info.AddProperty(NRelicCollection.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRelicCollection.PropertyName._screenContents, ref variant1))
      this._screenContents = ((Variant) ref variant1).As<NScrollableContainer>();
    Variant variant2;
    if (info.TryGetProperty(NRelicCollection.PropertyName._starter, ref variant2))
      this._starter = ((Variant) ref variant2).As<NRelicCollectionCategory>();
    Variant variant3;
    if (info.TryGetProperty(NRelicCollection.PropertyName._common, ref variant3))
      this._common = ((Variant) ref variant3).As<NRelicCollectionCategory>();
    Variant variant4;
    if (info.TryGetProperty(NRelicCollection.PropertyName._uncommon, ref variant4))
      this._uncommon = ((Variant) ref variant4).As<NRelicCollectionCategory>();
    Variant variant5;
    if (info.TryGetProperty(NRelicCollection.PropertyName._rare, ref variant5))
      this._rare = ((Variant) ref variant5).As<NRelicCollectionCategory>();
    Variant variant6;
    if (info.TryGetProperty(NRelicCollection.PropertyName._shop, ref variant6))
      this._shop = ((Variant) ref variant6).As<NRelicCollectionCategory>();
    Variant variant7;
    if (info.TryGetProperty(NRelicCollection.PropertyName._ancient, ref variant7))
      this._ancient = ((Variant) ref variant7).As<NRelicCollectionCategory>();
    Variant variant8;
    if (info.TryGetProperty(NRelicCollection.PropertyName._event, ref variant8))
      this._event = ((Variant) ref variant8).As<NRelicCollectionCategory>();
    Variant variant9;
    if (!info.TryGetProperty(NRelicCollection.PropertyName._screenTween, ref variant9))
      return;
    this._screenTween = ((Variant) ref variant9).As<Tween>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public new static readonly StringName OnSubmenuShown = StringName.op_Implicit(nameof (OnSubmenuShown));
    public static readonly StringName ClearRelics = StringName.op_Implicit(nameof (ClearRelics));
    public static readonly StringName SetLastFocusedRelic = StringName.op_Implicit(nameof (SetLastFocusedRelic));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _screenContents = StringName.op_Implicit(nameof (_screenContents));
    public static readonly StringName _starter = StringName.op_Implicit(nameof (_starter));
    public static readonly StringName _common = StringName.op_Implicit(nameof (_common));
    public static readonly StringName _uncommon = StringName.op_Implicit(nameof (_uncommon));
    public static readonly StringName _rare = StringName.op_Implicit(nameof (_rare));
    public static readonly StringName _shop = StringName.op_Implicit(nameof (_shop));
    public static readonly StringName _ancient = StringName.op_Implicit(nameof (_ancient));
    public static readonly StringName _event = StringName.op_Implicit(nameof (_event));
    public static readonly StringName _screenTween = StringName.op_Implicit(nameof (_screenTween));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
