// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.NSimpleCardsViewScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens;

[ScriptPath("res://src/Core/Nodes/Screens/NSimpleCardsViewScreen.cs")]
public class NSimpleCardsViewScreen : NCardsViewScreen
{
  private NButton _confirmButton;
  private List<CardPileAddResult> _cardResults;

  private static string ScenePath => SceneHelper.GetScenePath("screens/simple_cards_view_screen");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NSimpleCardsViewScreen.ScenePath);
    }
  }

  public override NetScreenType ScreenType => NetScreenType.SimpleCardsView;

  public override void _Ready()
  {
    this._confirmButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("ConfirmButton"));
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ViewUpgradesLabel")).SetTextAutoSize(new LocString("gameplay_ui", "VIEW_UPGRADES").GetFormattedText());
    this.ConnectSignals();
    NCardGrid grid = this._grid;
    List<CardModel> cards = this._cards;
    int capacity = 1;
    List<SortingOrders> sortingPriority = new List<SortingOrders>(capacity);
    CollectionsMarshal.SetCount<SortingOrders>(sortingPriority, capacity);
    CollectionsMarshal.AsSpan<SortingOrders>(sortingPriority)[0] = SortingOrders.Ascending;
    grid.SetCards((IReadOnlyList<CardModel>) cards, PileType.Deck, sortingPriority);
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this._backButton.Disable();
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(((NCardsViewScreen) this).OnReturnButtonPressed)), 0U);
    this._confirmButton.Enable();
    ((GodotObject) this._grid).Connect(NCardGrid.SignalName.HolderPressed, Callable.From<NCardHolder>((Action<NCardHolder>) (h => this.ShowCardDetail(h.CardModel))), 0U);
  }

  public static NCardsViewScreen? ShowScreen(List<CardPileAddResult> cards, LocString infoText)
  {
    if (TestMode.IsOn)
      return (NCardsViewScreen) null;
    NSimpleCardsViewScreen screen = PreloadManager.Cache.GetScene(NSimpleCardsViewScreen.ScenePath).Instantiate<NSimpleCardsViewScreen>((PackedScene.GenEditState) 0L);
    screen._cards = cards.Select<CardPileAddResult, CardModel>((Func<CardPileAddResult, CardModel>) (c => c.cardAdded)).ToList<CardModel>();
    screen._cardResults = cards;
    screen._infoText = infoText;
    NDebugAudioManager.Instance?.Play("map_open.mp3");
    NCapstoneContainer.Instance.Open((ICapstoneScreen) screen);
    return (NCardsViewScreen) screen;
  }

  protected override void OnInspectCardHidden()
  {
  }

  public override void AfterCapstoneOpened()
  {
    base.AfterCapstoneOpened();
    TaskHelper.RunSafely(this.FlashRelicsOnModifiedCards());
  }

  private async Task FlashRelicsOnModifiedCards()
  {
    double num = (double) await ((Node) this).AwaitProcessFrame();
    foreach (CardPileAddResult cardResult in this._cardResults)
    {
      CardPileAddResult result = cardResult;
      NGridCardHolder ngridCardHolder = this._grid.CurrentlyDisplayedCardHolders.FirstOrDefault<NGridCardHolder>((Func<NGridCardHolder, bool>) (h => h.CardModel == result.cardAdded));
      if (ngridCardHolder != null && result.modifyingModels != null && result.modifyingModels.Count != 0)
      {
        foreach (RelicModel relic in result.modifyingModels.OfType<RelicModel>())
        {
          relic.Flash();
          ngridCardHolder.CardNode?.FlashRelicOnCard(relic);
        }
      }
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NSimpleCardsViewScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardsViewScreen.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardsViewScreen.MethodName.OnInspectCardHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardsViewScreen.MethodName.AfterCapstoneOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName.OnInspectCardHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnInspectCardHidden();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName.AfterCapstoneOpened) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AfterCapstoneOpened();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName._Ready) || StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName.OnInspectCardHidden) || StringName.op_Equality(ref method, NSimpleCardsViewScreen.MethodName.AfterCapstoneOpened) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NSimpleCardsViewScreen.PropertyName._confirmButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._confirmButton = VariantUtils.ConvertTo<NButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSimpleCardsViewScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NSimpleCardsViewScreen.PropertyName._confirmButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NButton>(ref this._confirmButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSimpleCardsViewScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSimpleCardsViewScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSimpleCardsViewScreen.PropertyName._confirmButton, Variant.From<NButton>(ref this._confirmButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NSimpleCardsViewScreen.PropertyName._confirmButton, ref variant))
      return;
    this._confirmButton = ((Variant) ref variant).As<NButton>();
  }

  public new class MethodName : NCardsViewScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public new static readonly StringName OnInspectCardHidden = StringName.op_Implicit(nameof (OnInspectCardHidden));
    public new static readonly StringName AfterCapstoneOpened = StringName.op_Implicit(nameof (AfterCapstoneOpened));
  }

  public new class PropertyName : NCardsViewScreen.PropertyName
  {
    public new static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
  }

  public new class SignalName : NCardsViewScreen.SignalName
  {
  }
}
