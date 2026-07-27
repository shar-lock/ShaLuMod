// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NSimpleCardSelectScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NSimpleCardSelectScreen.cs")]
public sealed class NSimpleCardSelectScreen : NCardGridSelectionScreen
{
  private Control _bottomTextContainer;
  private MegaRichTextLabel _infoLabel;
  private NConfirmButton _confirmButton;
  private NCombatPilesContainer _combatPiles;
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private readonly HashSet<CardModel> _selectedCards = new HashSet<CardModel>();
  private CardSelectorPrefs _prefs;
  private List<CardCreationResult>? _cardResults;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_selection/simple_card_select_screen");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NSimpleCardSelectScreen.ScenePath);
    }
  }

  public static NSimpleCardSelectScreen Create(
    IReadOnlyList<CardModel> cards,
    CardSelectorPrefs prefs)
  {
    NSimpleCardSelectScreen cardSelectScreen = PreloadManager.Cache.GetScene(NSimpleCardSelectScreen.ScenePath).Instantiate<NSimpleCardSelectScreen>((PackedScene.GenEditState) 0L);
    List<CardModel> list = cards.ToList<CardModel>();
    if (prefs.Comparison != null)
      list.Sort(prefs.Comparison);
    ((Node) cardSelectScreen).Name = StringName.op_Implicit(nameof (NSimpleCardSelectScreen));
    cardSelectScreen._cards = (IReadOnlyList<CardModel>) list;
    cardSelectScreen._cardResults = (List<CardCreationResult>) null;
    cardSelectScreen._prefs = prefs;
    return cardSelectScreen;
  }

  public static NSimpleCardSelectScreen Create(
    IReadOnlyList<CardCreationResult> cards,
    CardSelectorPrefs prefs)
  {
    NSimpleCardSelectScreen cardSelectScreen = PreloadManager.Cache.GetScene(NSimpleCardSelectScreen.ScenePath).Instantiate<NSimpleCardSelectScreen>((PackedScene.GenEditState) 0L);
    List<CardCreationResult> list = cards.ToList<CardCreationResult>();
    if (prefs.Comparison != null)
      list.Sort((Comparison<CardCreationResult>) ((c1, c2) => prefs.Comparison(c1.Card, c2.Card)));
    ((Node) cardSelectScreen).Name = StringName.op_Implicit(nameof (NSimpleCardSelectScreen));
    cardSelectScreen._cardResults = list;
    cardSelectScreen._cards = (IReadOnlyList<CardModel>) cardSelectScreen._cardResults.Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (r => r.Card)).ToList<CardModel>();
    cardSelectScreen._prefs = prefs;
    return cardSelectScreen;
  }

  public override void _Ready()
  {
    this.ConnectSignalsAndInitGrid();
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%Confirm"));
    this._bottomTextContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BottomText"));
    this._infoLabel = ((Node) this._bottomTextContainer).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BottomLabel"));
    this._infoLabel.Text = this._prefs.Prompt.GetFormattedText();
    if (this._prefs.MinSelect == 0)
      this._confirmButton.Enable();
    else
      this._confirmButton.Disable();
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.CompleteSelection())), 0U);
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree()
  {
    base._ExitTree();
    this._cts.Cancel();
  }

  protected override void ConnectSignalsAndInitGrid()
  {
    base.ConnectSignalsAndInitGrid();
    this._combatPiles = ((Node) this).GetNode<NCombatPilesContainer>(NodePath.op_Implicit("%CombatPiles"));
    if (CombatManager.Instance.IsInProgress)
      this._combatPiles.Initialize(this._cards.First<CardModel>().Owner);
    this._combatPiles.Disable();
    ((CanvasItem) this._combatPiles).SetVisible(false);
    ((GodotObject) this._peekButton).Connect(NPeekButton.SignalName.Toggled, Callable.From<NPeekButton>((Action<NPeekButton>) (_ =>
    {
      if (this._peekButton.IsPeeking)
      {
        this._combatPiles.Enable();
        ((CanvasItem) this._combatPiles).SetVisible(true);
      }
      else
      {
        this._combatPiles.Disable();
        ((CanvasItem) this._combatPiles).SetVisible(false);
      }
    })), 0U);
  }

  protected override IEnumerable<Control> PeekButtonTargets
  {
    get
    {
      return (IEnumerable<Control>) new \u003C\u003Ez__ReadOnlySingleElementList<Control>(this._bottomTextContainer);
    }
  }

  public override void AfterOverlayOpened()
  {
    base.AfterOverlayOpened();
    TaskHelper.RunSafely(this.FlashRelicsOnModifiedCards());
  }

  private async Task FlashRelicsOnModifiedCards()
  {
    if (this._cardResults == null)
      return;
    double num = (double) await ((Node) this).AwaitProcessFrame(this._cts.Token);
    foreach (CardCreationResult cardResult in this._cardResults)
    {
      CardCreationResult result = cardResult;
      NGridCardHolder ngridCardHolder = this._grid.CurrentlyDisplayedCardHolders.FirstOrDefault<NGridCardHolder>((Func<NGridCardHolder, bool>) (h => h.CardModel == result.Card));
      if (ngridCardHolder != null && result.HasBeenModified)
      {
        foreach (RelicModel modifyingRelic in result.ModifyingRelics)
        {
          modifyingRelic.Flash();
          ngridCardHolder.CardNode?.FlashRelicOnCard(modifyingRelic);
        }
      }
    }
  }

  protected override void OnCardClicked(CardModel card)
  {
    if (this._selectedCards.Contains(card))
    {
      this._grid.UnhighlightCard(card);
      this._selectedCards.Remove(card);
    }
    else
    {
      if (this._selectedCards.Count < this._prefs.MaxSelect)
      {
        this._grid.HighlightCard(card);
        this._selectedCards.Add(card);
      }
      if (!this._prefs.RequireManualConfirmation)
        this.CheckIfSelectionComplete();
    }
    if (this._selectedCards.Count >= this._prefs.MinSelect && this._prefs.RequireManualConfirmation)
      this._confirmButton.Enable();
    else
      this._confirmButton.Disable();
  }

  private void CheckIfSelectionComplete()
  {
    if (this._selectedCards.Count < this._prefs.MaxSelect)
      return;
    this.CompleteSelection();
  }

  private void CompleteSelection()
  {
    this._completionSource.SetResult((IEnumerable<CardModel>) this._selectedCards);
    NOverlayStack.Instance.Remove((IOverlayScreen) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NSimpleCardSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardSelectScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardSelectScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardSelectScreen.MethodName.ConnectSignalsAndInitGrid, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardSelectScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardSelectScreen.MethodName.CheckIfSelectionComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSimpleCardSelectScreen.MethodName.CompleteSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.ConnectSignalsAndInitGrid) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignalsAndInitGrid();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.CheckIfSelectionComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckIfSelectionComplete();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.CompleteSelection) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.CompleteSelection();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.ConnectSignalsAndInitGrid) || StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.CheckIfSelectionComplete) || StringName.op_Equality(ref method, NSimpleCardSelectScreen.MethodName.CompleteSelection) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._bottomTextContainer))
    {
      this._bottomTextContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._infoLabel))
    {
      this._infoLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._combatPiles))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._combatPiles = VariantUtils.ConvertTo<NCombatPilesContainer>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._bottomTextContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bottomTextContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._infoLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._infoLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSimpleCardSelectScreen.PropertyName._combatPiles))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NCombatPilesContainer>(ref this._combatPiles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSimpleCardSelectScreen.PropertyName._bottomTextContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSimpleCardSelectScreen.PropertyName._infoLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSimpleCardSelectScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSimpleCardSelectScreen.PropertyName._combatPiles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSimpleCardSelectScreen.PropertyName._bottomTextContainer, Variant.From<Control>(ref this._bottomTextContainer));
    info.AddProperty(NSimpleCardSelectScreen.PropertyName._infoLabel, Variant.From<MegaRichTextLabel>(ref this._infoLabel));
    info.AddProperty(NSimpleCardSelectScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NSimpleCardSelectScreen.PropertyName._combatPiles, Variant.From<NCombatPilesContainer>(ref this._combatPiles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._bottomTextContainer, ref variant1))
      this._bottomTextContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._infoLabel, ref variant2))
      this._infoLabel = ((Variant) ref variant2).As<MegaRichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._confirmButton, ref variant3))
      this._confirmButton = ((Variant) ref variant3).As<NConfirmButton>();
    Variant variant4;
    if (!info.TryGetProperty(NSimpleCardSelectScreen.PropertyName._combatPiles, ref variant4))
      return;
    this._combatPiles = ((Variant) ref variant4).As<NCombatPilesContainer>();
  }

  public new class MethodName : NCardGridSelectionScreen.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName ConnectSignalsAndInitGrid = StringName.op_Implicit(nameof (ConnectSignalsAndInitGrid));
    public new static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName CheckIfSelectionComplete = StringName.op_Implicit(nameof (CheckIfSelectionComplete));
    public static readonly StringName CompleteSelection = StringName.op_Implicit(nameof (CompleteSelection));
  }

  public new class PropertyName : NCardGridSelectionScreen.PropertyName
  {
    public static readonly StringName _bottomTextContainer = StringName.op_Implicit(nameof (_bottomTextContainer));
    public static readonly StringName _infoLabel = StringName.op_Implicit(nameof (_infoLabel));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public static readonly StringName _combatPiles = StringName.op_Implicit(nameof (_combatPiles));
  }

  public new class SignalName : NCardGridSelectionScreen.SignalName
  {
  }
}
