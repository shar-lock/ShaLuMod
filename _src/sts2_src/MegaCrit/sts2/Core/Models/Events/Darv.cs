// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Darv
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Darv : AncientEventModel
{
  private const string _sfxExcited = "event:/sfx/npcs/darv/darv_excited";
  private const string _sfxOuttaTheWay = "event:/sfx/npcs/darv/darv_outta_the_way";
  private const string _sfxFear = "event:/sfx/npcs/darv/darv_fear";
  private const string _sfxPain = "event:/sfx/npcs/darv/darv_pain";
  private const string _sfxEndeared = "event:/sfx/npcs/darv/darv_endeared";
  private const string _sfxIntroduction = "event:/sfx/npcs/darv/darv_introduction";
  private static readonly List<Darv.ValidRelicSet> _validRelicSets;

  public override Color ButtonColor => new Color(0.06f, 0.0f, 0.08f, 0.5f);

  public override Color DialogueColor => new Color("512E66");

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get
    {
      return Darv._validRelicSets.SelectMany<Darv.ValidRelicSet, RelicModel>((Func<Darv.ValidRelicSet, IEnumerable<RelicModel>>) (s => (IEnumerable<RelicModel>) s.relics)).Select<RelicModel, EventOption>((Func<RelicModel, EventOption>) (r => this.RelicOption(r.ToMutable()))).Concat<EventOption>((IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.RelicOption<DustyTome>()));
    }
  }

  protected override AncientDialogueSet DefineDialogues()
  {
    AncientDialogueSet ancientDialogueSet1 = new AncientDialogueSet();
    ancientDialogueSet1.FirstVisitEverDialogue = new AncientDialogue(new string[1]
    {
      "event:/sfx/npcs/darv/darv_introduction"
    });
    AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet1;
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary1 = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
    string key1 = AncientEventModel.CharKey<Ironclad>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key1] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_introduction"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_endeared"
      })
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
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_introduction"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_excited"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/darv/darv_pain",
        "",
        "event:/sfx/npcs/darv/darv_outta_the_way"
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key3 = AncientEventModel.CharKey<Defect>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key3] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[2]
      {
        "event:/sfx/npcs/darv/darv_introduction",
        ""
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_endeared"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/darv/darv_fear",
        "",
        "event:/sfx/npcs/darv/darv_fear"
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
        "event:/sfx/npcs/darv/darv_introduction",
        "",
        "event:/sfx/npcs/darv/darv_excited"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_endeared"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/darv/darv_excited",
        "",
        "event:/sfx/npcs/darv/darv_fear"
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key5 = AncientEventModel.CharKey<Regent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key5] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/darv/darv_introduction",
        "",
        "event:/sfx/npcs/darv/darv_excited"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_introduction"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/darv/darv_excited",
        "",
        "event:/sfx/npcs/darv/darv_pain"
      })
      {
        VisitIndex = new int?(4)
      }
    });
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary2 = dictionary1;
    ancientDialogueSet2.CharacterDialogues = dictionary2;
    // ISSUE: object of a compiler-generated type is created
    ancientDialogueSet1.AgnosticDialogues = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[2]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_excited"
      }),
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/darv/darv_outta_the_way"
      })
    });
    return ancientDialogueSet1;
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> source = Darv._validRelicSets.Where<Darv.ValidRelicSet>((Func<Darv.ValidRelicSet, bool>) (rs => rs.filter(this.Owner))).Select<Darv.ValidRelicSet, EventOption>((Func<Darv.ValidRelicSet, EventOption>) (rs => this.RelicOption(this.Rng.NextItem<RelicModel>((IEnumerable<RelicModel>) rs.relics).ToMutable()))).ToList<EventOption>().UnstableShuffle<EventOption>(this.Rng);
    List<EventOption> list;
    if (this.Rng.NextBool())
    {
      list = source.Take<EventOption>(2).ToList<EventOption>();
      DustyTome mutable = (DustyTome) ModelDb.Relic<DustyTome>().ToMutable();
      if (this.Owner != null)
        mutable.SetupForPlayer(this.Owner);
      list.Add(this.RelicOption((RelicModel) mutable));
    }
    else
      list = source.Take<EventOption>(3).ToList<EventOption>();
    return (IReadOnlyList<EventOption>) list;
  }

  static Darv()
  {
    int capacity = 11;
    List<Darv.ValidRelicSet> validRelicSetList = new List<Darv.ValidRelicSet>(capacity);
    CollectionsMarshal.SetCount<Darv.ValidRelicSet>(validRelicSetList, capacity);
    Span<Darv.ValidRelicSet> span = CollectionsMarshal.AsSpan<Darv.ValidRelicSet>(validRelicSetList);
    int num1 = 0;
    span[num1] = new Darv.ValidRelicSet(new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<Astrolabe>()
    });
    int num2 = num1 + 1;
    span[num2] = new Darv.ValidRelicSet(new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<BlackStar>()
    });
    int num3 = num2 + 1;
    span[num3] = new Darv.ValidRelicSet(new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<CallingBell>()
    });
    int num4 = num3 + 1;
    span[num4] = new Darv.ValidRelicSet(new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<EmptyCage>()
    });
    int num5 = num4 + 1;
    span[num5] = new Darv.ValidRelicSet((Func<Player, bool>) (owner => !owner.RunState.Modifiers.Any<ModifierModel>((Func<ModifierModel, bool>) (m => m.ClearsPlayerDeck))), new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<PandorasBox>()
    });
    int num6 = num5 + 1;
    span[num6] = new Darv.ValidRelicSet(new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<RunicPyramid>()
    });
    int num7 = num6 + 1;
    span[num7] = new Darv.ValidRelicSet(new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<SneckoEye>()
    });
    int num8 = num7 + 1;
    span[num8] = new Darv.ValidRelicSet((Func<Player, bool>) (owner => owner.RunState.CurrentActIndex == 1), new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<Ectoplasm>()
    });
    int num9 = num8 + 1;
    span[num9] = new Darv.ValidRelicSet((Func<Player, bool>) (owner => owner.RunState.CurrentActIndex == 1), new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<Sozu>()
    });
    int num10 = num9 + 1;
    span[num10] = new Darv.ValidRelicSet((Func<Player, bool>) (owner => owner.RunState.CurrentActIndex >= 1), new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<PhilosophersStone>()
    });
    int num11 = num10 + 1;
    span[num11] = new Darv.ValidRelicSet((Func<Player, bool>) (owner => owner.RunState.CurrentActIndex >= 1), new RelicModel[1]
    {
      (RelicModel) ModelDb.Relic<VelvetChoker>()
    });
    Darv._validRelicSets = validRelicSetList;
  }

  private struct ValidRelicSet
  {
    public readonly Func<Player, bool> filter;
    public readonly RelicModel[] relics;

    public ValidRelicSet(Func<Player, bool> filter, RelicModel[] relics)
    {
      this.filter = filter;
      this.relics = relics;
    }

    public ValidRelicSet(RelicModel[] relics)
    {
      this.filter = (Func<Player, bool>) (_ => true);
      this.relics = relics;
    }
  }
}
