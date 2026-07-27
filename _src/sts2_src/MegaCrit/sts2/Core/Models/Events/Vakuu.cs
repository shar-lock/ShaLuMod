// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Vakuu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Vakuu : AncientEventModel
{
  private const string _visitsKey = "Visits";

  public override Color ButtonColor => new Color(0.05f, 0.06f, 0.12f, 0.8f);

  public override Color DialogueColor => new Color("3C1931");

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Visits", 0M));
    }
  }

  public override void CalculateVars()
  {
    base.CalculateVars();
    if (!LocalContext.IsMe(this.Owner))
      return;
    AncientStats valueOrDefault = CollectionExtensions.GetValueOrDefault<ModelId, AncientStats>(SaveManager.Instance.Progress.AncientStats, this.Id);
    this.DynamicVars["Visits"].BaseValue = (Decimal) ((valueOrDefault != null ? valueOrDefault.GetVisitsAs(this.Owner.Character.Id) : 0) + 1);
  }

  protected override AncientDialogueSet DefineDialogues()
  {
    AncientDialogueSet ancientDialogueSet1 = new AncientDialogueSet();
    ancientDialogueSet1.FirstVisitEverDialogue = new AncientDialogue(new string[1]
    {
      ""
    });
    AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet1;
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary1 = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
    string key1 = AncientEventModel.CharKey<Ironclad>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key1] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(4)
      }
    });
    string key2 = AncientEventModel.CharKey<Silent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key2] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(4)
      }
    });
    string key3 = AncientEventModel.CharKey<Defect>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key3] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(4)
      }
    });
    string key4 = AncientEventModel.CharKey<Necrobinder>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key4] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[2]{ "", "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(4)
      }
    });
    string key5 = AncientEventModel.CharKey<Regent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key5] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(4)
      }
    });
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary2 = dictionary1;
    ancientDialogueSet2.CharacterDialogues = dictionary2;
    // ISSUE: object of a compiler-generated type is created
    ancientDialogueSet1.AgnosticDialogues = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]{ "" }),
      new AncientDialogue(new string[1]{ "" }),
      new AncientDialogue(new string[1]{ "" })
    });
    return ancientDialogueSet1;
  }

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get => this.Pool1.Concat<EventOption>(this.Pool2).Concat<EventOption>(this.Pool3);
  }

  private IEnumerable<EventOption> Pool1
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
      {
        this.RelicOption<BloodSoakedRose>(),
        this.RelicOption<WhisperingEarring>(),
        this.RelicOption<Fiddle>()
      });
    }
  }

  private IEnumerable<EventOption> Pool2
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
      {
        this.RelicOption<PreservedFog>(),
        this.RelicOption<SereTalon>().ThatDecreasesMaxHp(9M),
        this.RelicOption<DistinguishedCape>()
      });
    }
  }

  private IEnumerable<EventOption> Pool3
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[4]
      {
        this.RelicOption<ChoicesParadox>(),
        this.RelicOption<MusicBox>(),
        this.RelicOption<LordsParasol>(),
        this.RelicOption<JeweledMask>()
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> list1 = this.Pool1.ToList<EventOption>();
    List<EventOption> list2 = this.Pool2.ToList<EventOption>();
    List<EventOption> list3 = this.Pool3.ToList<EventOption>();
    list1.UnstableShuffle<EventOption>(this.Rng);
    list2.UnstableShuffle<EventOption>(this.Rng);
    list3.UnstableShuffle<EventOption>(this.Rng);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      list1[0],
      list2[0],
      list3[0]
    });
  }
}
