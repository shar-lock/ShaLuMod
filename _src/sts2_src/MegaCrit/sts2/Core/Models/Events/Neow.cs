// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Neow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Neow : AncientEventModel
{
  private const string _cursedChoiceDoneDescriptionOverride = "NEOW.pages.DONE.CURSED.description";
  private const string _positiveChoiceDoneDescriptionOverride = "NEOW.pages.DONE.POSITIVE.description";
  private const string _sfxSleepy = "event:/sfx/npcs/neow/neow_sleepy";
  private const string _sfxWelcome = "event:/sfx/npcs/neow/neow_welcome";
  private const string _sfxCurious = "event:/sfx/npcs/neow/neow_curious";
  private List<EventOption>? _modifierOptions;

  public override string AmbientBgm => "event:/sfx/ambience/act1_neow";

  public override Color ButtonColor => new Color(0.0f, 0.1f, 0.2f, 0.5f);

  public override Color DialogueColor => new Color("28454F");

  public override LocString InitialDescription
  {
    get
    {
      Player owner = this.Owner;
      return (owner != null ? (owner.RunState.Modifiers.Count <= 0 ? 1 : 0) : 0) != 0 ? base.InitialDescription : this.L10NLookup(this.Id.Entry + ".EVENT.description");
    }
  }

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get
    {
      List<EventOption> items = new List<EventOption>();
      items.AddRange(this.CurseOptions);
      items.AddRange(this.PositiveOptions);
      items.Add(this.LavaRockOption);
      items.Add(this.NeowsTalismanOption);
      items.Add(this.NutritiousOysterOption);
      items.Add(this.PomanderOption);
      items.Add(this.SmallCapsuleOption);
      items.Add(this.StoneHumidifierOption);
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyList<EventOption>(items);
    }
  }

  private IEnumerable<EventOption> PositiveOptions
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[14]
      {
        this.RelicOption<ArcaneScroll>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<BoomingConch>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<FishingRod>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<GoldenPearl>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<Kaleidoscope>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<LeadPaperweight>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<LostCoffer>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<MassiveScroll>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<NeowsTorment>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<NewLeaf>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<PhialHolster>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<PreciseScissors>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<ScrollBoxes>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<WingedBoots>(customDonePage: "NEOW.pages.DONE.POSITIVE.description")
      });
    }
  }

  private EventOption LavaRockOption
  {
    get => this.RelicOption<LavaRock>(customDonePage: "NEOW.pages.DONE.POSITIVE.description");
  }

  private EventOption NeowsTalismanOption
  {
    get => this.RelicOption<NeowsTalisman>(customDonePage: "NEOW.pages.DONE.POSITIVE.description");
  }

  private EventOption NutritiousOysterOption
  {
    get
    {
      return this.RelicOption<NutritiousOyster>(customDonePage: "NEOW.pages.DONE.POSITIVE.description");
    }
  }

  private EventOption PomanderOption
  {
    get => this.RelicOption<Pomander>(customDonePage: "NEOW.pages.DONE.POSITIVE.description");
  }

  private EventOption SmallCapsuleOption
  {
    get => this.RelicOption<SmallCapsule>(customDonePage: "NEOW.pages.DONE.POSITIVE.description");
  }

  private EventOption StoneHumidifierOption
  {
    get
    {
      return this.RelicOption<StoneHumidifier>(customDonePage: "NEOW.pages.DONE.POSITIVE.description");
    }
  }

  private IEnumerable<EventOption> CurseOptions
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[10]
      {
        this.RelicOption<CursedPearl>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<DowsingRod>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<HeftyTablet>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<LargeCapsule>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<LeafyPoultice>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<NeowsBones>(customDonePage: "NEOW.pages.DONE.POSITIVE.description"),
        this.RelicOption<NeowsSacrifice>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<PrecariousShears>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<SilkenTress>(customDonePage: "NEOW.pages.DONE.CURSED.description"),
        this.RelicOption<SilverCrucible>(customDonePage: "NEOW.pages.DONE.CURSED.description")
      });
    }
  }

  protected override AncientDialogueSet DefineDialogues()
  {
    AncientDialogueSet ancientDialogueSet1 = new AncientDialogueSet();
    ancientDialogueSet1.FirstVisitEverDialogue = new AncientDialogue(new string[1]
    {
      "event:/sfx/npcs/neow/neow_welcome"
    });
    AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet1;
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary1 = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
    string key1 = AncientEventModel.CharKey<Ironclad>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key1] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_welcome"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_curious"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/neow/neow_sleepy",
        "event:/sfx/npcs/neow/neow_sleepy",
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key2 = AncientEventModel.CharKey<Silent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key2] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_curious"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/neow/neow_sleepy",
        "",
        ""
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key3 = AncientEventModel.CharKey<Defect>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key3] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_curious"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/neow/neow_sleepy",
        "",
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key4 = AncientEventModel.CharKey<Necrobinder>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key4] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/neow/neow_welcome",
        "",
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[2]
      {
        "",
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key5 = AncientEventModel.CharKey<Regent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key5] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[2]
      {
        "",
        "event:/sfx/npcs/neow/neow_sleepy"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_curious"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "",
        "event:/sfx/npcs/neow/neow_sleepy",
        ""
      })
      {
        VisitIndex = new int?(4)
      }
    });
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary2 = dictionary1;
    ancientDialogueSet2.CharacterDialogues = dictionary2;
    // ISSUE: object of a compiler-generated type is created
    ancientDialogueSet1.AgnosticDialogues = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[5]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_welcome"
      }),
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_welcome"
      }),
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_welcome"
      }),
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/neow/neow_welcome"
      }),
      new AncientDialogue(new string[1]{ "" })
    });
    return ancientDialogueSet1;
  }

  private List<EventOption> ModifierOptions
  {
    get
    {
      this.AssertMutable();
      if (this._modifierOptions == null)
        this._modifierOptions = new List<EventOption>();
      return this._modifierOptions;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    if (this.Owner.RunState.Modifiers.Count <= 0)
    {
      List<EventOption> list1 = this.CurseOptions.ToList<EventOption>();
      list1.RemoveAll((Predicate<EventOption>) (r =>
      {
        RelicModel relic = r.Relic;
        return relic != null && !relic.IsAllowedAtNeow(this.Owner);
      }));
      EventOption eventOption = this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) list1);
      List<EventOption> list2 = this.PositiveOptions.ToList<EventOption>();
      if (eventOption.Relic is CursedPearl)
        list2.RemoveAll((Predicate<EventOption>) (o => o.Relic is GoldenPearl));
      if (eventOption.Relic is HeftyTablet)
        list2.RemoveAll((Predicate<EventOption>) (o => o.Relic is ArcaneScroll));
      if (eventOption.Relic is LeafyPoultice)
        list2.RemoveAll((Predicate<EventOption>) (o => o.Relic is NewLeaf));
      if (eventOption.Relic is PrecariousShears)
        list2.RemoveAll((Predicate<EventOption>) (o => o.Relic is PreciseScissors));
      if (eventOption.Relic is NeowsSacrifice)
      {
        list2.RemoveAll((Predicate<EventOption>) (o => o.Relic is PhialHolster));
        list2.RemoveAll((Predicate<EventOption>) (o => o.Relic is LostCoffer));
      }
      if (!(eventOption.Relic is LargeCapsule))
      {
        if (this.Rng.NextBool())
          list2.Add(this.LavaRockOption);
        else
          list2.Add(this.SmallCapsuleOption);
      }
      if (this.Rng.NextBool())
        list2.Add(this.NutritiousOysterOption);
      else
        list2.Add(this.StoneHumidifierOption);
      if (this.Rng.NextBool())
        list2.Add(this.NeowsTalismanOption);
      else
        list2.Add(this.PomanderOption);
      list2.RemoveAll((Predicate<EventOption>) (r =>
      {
        RelicModel relic = r.Relic;
        return relic != null && !relic.IsAllowedAtNeow(this.Owner);
      }));
      List<EventOption> items = new List<EventOption>();
      items.AddRange(list2.ToList<EventOption>().UnstableShuffle<EventOption>(this.Rng).Take<EventOption>(2));
      items.Add(eventOption);
      // ISSUE: object of a compiler-generated type is created
      return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyList<EventOption>(items);
    }
    foreach (ModifierModel modifier in (IEnumerable<ModifierModel>) this.Owner.RunState.Modifiers)
    {
      Func<Task> neowOption = modifier.GenerateNeowOption((EventModel) this);
      if (neowOption != null)
      {
        int optionIndex = this.ModifierOptions.Count;
        this.ModifierOptions.Add(new EventOption((EventModel) this, (Func<Task>) (() => this.OnModifierOptionSelected(neowOption, optionIndex)), modifier.NeowOptionTitle, modifier.NeowOptionDescription, modifier.Id.Entry, (IEnumerable<IHoverTip>) modifier.HoverTips.ToArray<IHoverTip>()));
      }
    }
    // ISSUE: object of a compiler-generated type is created
    return this.ModifierOptions.Count > 0 ? (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.ModifierOptions[0]) : (IReadOnlyList<EventOption>) Array.Empty<EventOption>();
  }

  private async Task OnModifierOptionSelected(Func<Task> modifierFunc, int index)
  {
    await modifierFunc();
    if (index + 1 >= this.ModifierOptions.Count)
    {
      this.SetEventFinished(this.L10NLookup(this.Id.Entry + ".pages.DONE.description"));
    }
    else
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.InitialDescription, (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.ModifierOptions[index + 1]));
    }
  }
}
