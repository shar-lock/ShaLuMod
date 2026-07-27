// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Pael
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Pael : AncientEventModel
{
  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get
    {
      return this.OptionPool1.Concat<EventOption>((IEnumerable<EventOption>) this.OptionPool2).Concat<EventOption>((IEnumerable<EventOption>) this.OptionPool3).Concat<EventOption>((IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[4]
      {
        this.PaelsClawOption,
        this.PaelsToothOption,
        this.PaelsLegionOption,
        this.PaelsGrowthOption
      }));
    }
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
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(4)
      }
    });
    string key2 = AncientEventModel.CharKey<Silent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key2] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[1]{ "" })
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
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(4)
      }
    });
    string key4 = AncientEventModel.CharKey<Necrobinder>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key4] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
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
    string key5 = AncientEventModel.CharKey<Regent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key5] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
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
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary2 = dictionary1;
    ancientDialogueSet2.CharacterDialogues = dictionary2;
    // ISSUE: object of a compiler-generated type is created
    ancientDialogueSet1.AgnosticDialogues = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[2]
    {
      new AncientDialogue(new string[1]{ "" }),
      new AncientDialogue(new string[1]{ "" })
    });
    return ancientDialogueSet1;
  }

  public override Color ButtonColor => new Color(0.03f, 0.08f, 0.0f, 0.75f);

  public override Color DialogueColor => new Color("332C29");

  private EventOption PaelsClawOption => this.RelicOption<PaelsClaw>();

  private EventOption PaelsToothOption => this.RelicOption<PaelsTooth>();

  private EventOption PaelsGrowthOption => this.RelicOption<PaelsGrowth>();

  private EventOption PaelsLegionOption => this.RelicOption<PaelsLegion>();

  private IEnumerable<EventOption> OptionPool1
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
      {
        this.RelicOption<PaelsFlesh>(),
        this.RelicOption<PaelsHorn>(),
        this.RelicOption<PaelsTears>()
      });
    }
  }

  private List<EventOption> OptionPool2
  {
    get
    {
      int capacity = 1;
      List<EventOption> optionPool2 = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(optionPool2, capacity);
      CollectionsMarshal.AsSpan<EventOption>(optionPool2)[0] = this.RelicOption<PaelsWing>();
      return optionPool2;
    }
  }

  private List<EventOption> OptionPool3
  {
    get
    {
      int capacity = 2;
      List<EventOption> optionPool3 = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(optionPool3, capacity);
      Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(optionPool3);
      int num1 = 0;
      span[num1] = this.RelicOption<PaelsEye>();
      int num2 = num1 + 1;
      span[num2] = this.RelicOption<PaelsBlood>();
      return optionPool3;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    EventOption eventOption1 = this.Rng.NextItem<EventOption>(this.OptionPool1);
    List<EventOption> list1 = this.OptionPool2.ToList<EventOption>();
    IReadOnlyList<CardModel> cards = this.Owner.Deck.Cards;
    if (cards.Count<CardModel>((Func<CardModel, bool>) (c => ModelDb.Enchantment<Goopy>().CanEnchant(c))) >= 3)
      list1.Add(this.PaelsClawOption);
    if (cards.Count<CardModel>((Func<CardModel, bool>) (c => c.IsRemovable)) >= 5)
      list1.Add(this.PaelsToothOption);
    list1.AddRange((IEnumerable<EventOption>) list1);
    list1.Add(this.PaelsGrowthOption);
    EventOption eventOption2 = this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) list1);
    List<EventOption> list2 = this.OptionPool3.ToList<EventOption>();
    if (!this.Owner.HasEventPet())
      list2.Add(this.PaelsLegionOption);
    EventOption eventOption3 = this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) list2);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      eventOption1,
      eventOption2,
      eventOption3
    });
  }
}
