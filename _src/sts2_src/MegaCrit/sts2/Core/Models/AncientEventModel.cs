// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.AncientEventModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class AncientEventModel : EventModel
{
  private AncientDialogueSet? _dialogueSet;
  private List<EventOption>? _generatedOptions;
  private string? _customDonePage;
  private string? _debugOption;

  public override string LocTable => "ancients";

  public LocString Epithet => this.L10NLookup(this.Id.Entry + ".epithet");

  public AncientDialogueSet DialogueSet
  {
    get
    {
      if (this._dialogueSet == null)
      {
        this._dialogueSet = this.DefineDialogues();
        this._dialogueSet.PopulateLocKeys(this.Id.Entry);
      }
      return this._dialogueSet;
    }
  }

  protected abstract AncientDialogueSet DefineDialogues();

  protected static string CharKey<T>() where T : CharacterModel => ModelDb.Character<T>().Id.Entry;

  public virtual IEnumerable<CharacterModel> AnyCharacterDialogueBlacklist
  {
    get => (IEnumerable<CharacterModel>) Array.Empty<CharacterModel>();
  }

  public override Color ButtonColor => new Color(0.0f, 0.0f, 0.0f, 0.35f);

  public virtual Color DialogueColor { get; } = new Color("28454f");

  private string? CustomDonePage
  {
    get => this._customDonePage;
    set
    {
      this.AssertMutable();
      this._customDonePage = value;
    }
  }

  public string? DebugOption
  {
    get => this._debugOption;
    set
    {
      this.AssertMutable();
      this._debugOption = value;
    }
  }

  private List<EventOption>? GeneratedOptions
  {
    get => this._generatedOptions;
    set
    {
      this.AssertMutable();
      this._generatedOptions = value;
    }
  }

  public override EventLayoutType LayoutType => EventLayoutType.Ancient;

  private string MapIconPath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/map/ancients/ancient_node_{this.Id.Entry.ToLowerInvariant()}.png");
    }
  }

  private string MapIconOutlinePath
  {
    get
    {
      return ImageHelper.GetImagePath($"packed/map/ancients/ancient_node_{this.Id.Entry.ToLowerInvariant()}_outline.png");
    }
  }

  public Texture2D MapIcon
  {
    get => (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.MapIconPath);
  }

  public Texture2D MapIconOutline
  {
    get => (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.MapIconOutlinePath);
  }

  public IEnumerable<string> MapNodeAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        this.MapIconPath,
        this.MapIconOutlinePath
      });
    }
  }

  public virtual string AmbientBgm => "";

  public bool HasAmbientBgm => this.AmbientBgm != "";

  public Texture2D RunHistoryIcon
  {
    get
    {
      return (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(ImageHelper.GetRoomIconPath(MapPointType.Ancient, RoomType.Event, this.Id));
    }
  }

  public Texture2D RunHistoryIconOutline
  {
    get => (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(this.RunHistoryIconOutlinePath);
  }

  private string RunHistoryIconOutlinePath
  {
    get
    {
      return ImageHelper.GetImagePath($"ui/run_history/{this.Id.Entry.ToLowerInvariant()}_outline.png");
    }
  }

  public int HealedAmount { get; private set; }

  public abstract IEnumerable<EventOption> AllPossibleOptions { get; }

  protected virtual Color EventButtonColor { get; set; } = new Color("00000059");

  public override LocString InitialDescription
  {
    get
    {
      return !RunManager.Instance.IsInProgress || Hook.ShouldAllowAncient(this.Owner.RunState, this.Owner, this) ? base.InitialDescription : new LocString("relics", "WAX_CHOKER.blockMessage");
    }
  }

  protected override async Task BeforeEventStarted(bool isPreFinished)
  {
    if (isPreFinished)
      return;
    if (this is Neow)
      this.Owner.Creature.SetCurrentHpInternal(0M);
    int oldHp = this.Owner.Creature.CurrentHp;
    Decimal amount = (Decimal) (this.Owner.Creature.MaxHp - this.Owner.Creature.CurrentHp);
    if (RunManager.Instance.HasAscension(AscensionLevel.WearyTraveler))
      amount *= 0.8M;
    await CreatureCmd.Heal(this.Owner.Creature, amount, false);
    if (NRun.Instance != null && this is Neow)
      TaskHelper.RunSafely(NRun.Instance.GlobalUi.TopBar.Hp.LerpAtNeow());
    this.HealedAmount = this.Owner.Creature.CurrentHp - oldHp;
  }

  protected sealed override IReadOnlyList<EventOption> GenerateInitialOptionsWrapper()
  {
    if (!Hook.ShouldAllowAncient(this.Owner.RunState, this.Owner, this))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, AncientEventModel.\u003C\u003EO.\u003C0\u003E__Proceed ?? (AncientEventModel.\u003C\u003EO.\u003C0\u003E__Proceed = new Func<Task>(NEventRoom.Proceed)), "PROCEED", false, true, Array.Empty<IHoverTip>()));
    }
    this.GeneratedOptions = this.GenerateInitialOptions().ToList<EventOption>();
    if (this.DebugOption != null)
    {
      this.GeneratedOptions.RemoveAt(0);
      this.GeneratedOptions.Insert(0, this.AllPossibleOptions.First<EventOption>((Func<EventOption, bool>) (c => c.TextKey.Contains(this.DebugOption))));
    }
    this.ReplaceNullOptions(this.GeneratedOptions);
    return (IReadOnlyList<EventOption>) this.GeneratedOptions;
  }

  protected override void SetInitialEventState(bool isPreFinished)
  {
    IReadOnlyList<EventOption> initialOptionsWrapper = this.GenerateInitialOptionsWrapper();
    if (initialOptionsWrapper.Count == 0 | isPreFinished)
      this.StartPreFinished();
    else
      this.SetEventState(this.InitialDescription, (IEnumerable<EventOption>) initialOptionsWrapper);
  }

  private void UpdateRunHistory()
  {
    if (!RunManager.Instance.IsInProgress)
      return;
    foreach (EventOption generatedOption in this.GeneratedOptions)
    {
      AncientChoiceHistoryEntry choiceHistoryEntry = new AncientChoiceHistoryEntry(generatedOption.Title, generatedOption.WasChosen);
      this.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(this.Owner.NetId).AncientChoices.Add(choiceHistoryEntry);
    }
  }

  public void StartPreFinished()
  {
    if (this.CustomDonePage == null)
      this.SetEventFinished(this.L10NLookup(this.Id.Entry + ".pages.DONE.description"));
    else
      this.SetEventFinished(this.L10NLookup(this.CustomDonePage));
  }

  protected void Done()
  {
    this.UpdateRunHistory();
    if (this.CustomDonePage == null)
      this.SetEventFinished(this.L10NLookup(this.Id.Entry + ".pages.DONE.description"));
    else
      this.SetEventFinished(this.L10NLookup(this.CustomDonePage));
  }

  protected EventOption RelicOption<T>(string pageName = "INITIAL", string? customDonePage = null) where T : RelicModel
  {
    return this.RelicOption(ModelDb.Relic<T>().ToMutable(), pageName);
  }

  protected EventOption RelicOption(RelicModel relic, string pageName = "INITIAL", string? customDonePage = null)
  {
    return this.RelicOption(relic, new Func<Task>(OnChosen), pageName);

    async Task OnChosen()
    {
      RelicModel relicModel = await RelicCmd.Obtain(relic, this.Owner);
      this.CustomDonePage = customDonePage;
      this.Done();
    }
  }
}
