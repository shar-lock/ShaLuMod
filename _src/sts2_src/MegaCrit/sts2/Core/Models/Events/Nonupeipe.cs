// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Nonupeipe
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Nonupeipe : AncientEventModel
{
  private const string _sfxEeked = "event:/sfx/npcs/nonupeipe/nonupeipe_eeked";
  private const string _sfxWelcome = "event:/sfx/npcs/nonupeipe/nonupeipe_welcome";
  private const string _sfxGrossedOut = "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out";
  private const string _sfxGiggle = "event:/sfx/npcs/nonupeipe/nonupeipe_giggle";

  public override Color ButtonColor => new Color(0.0f, 0.1f, 0.16f, 0.75f);

  public override Color DialogueColor => new Color("0A494D");

  protected override AncientDialogueSet DefineDialogues()
  {
    AncientDialogueSet ancientDialogueSet1 = new AncientDialogueSet();
    ancientDialogueSet1.FirstVisitEverDialogue = new AncientDialogue(new string[1]
    {
      "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
    });
    AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet1;
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary1 = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
    string key1 = AncientEventModel.CharKey<Ironclad>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key1] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      })
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
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out",
        "",
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_giggle"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_eeked",
        "",
        "event:/sfx/npcs/nonupeipe/nonupeipe_eeked"
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
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[2]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_giggle",
        ""
      })
      {
        VisitIndex = new int?(4)
      }
    });
    string key4 = AncientEventModel.CharKey<Necrobinder>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key4] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[2]
      {
        "",
        "event:/sfx/npcs/nonupeipe/nonupeipe_eeked"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[2]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out",
        ""
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
        "event:/sfx/npcs/nonupeipe/nonupeipe_eeked"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[2]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_grossed_out",
        ""
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
        "event:/sfx/npcs/nonupeipe/nonupeipe_welcome"
      }),
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/nonupeipe/nonupeipe_eeked"
      })
    });
    return ancientDialogueSet1;
  }

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get
    {
      return this.OptionPool.Concat<EventOption>((IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.BeautifulBraceletEventOption));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  private IEnumerable<EventOption> OptionPool
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[9]
      {
        this.RelicOption<BlessedAntler>(),
        this.RelicOption<BrilliantScarf>(),
        this.RelicOption<DelicateFrond>(),
        this.RelicOption<DiamondDiadem>(),
        this.RelicOption<FurCoat>(),
        this.RelicOption<Glitter>(),
        this.RelicOption<JewelryBox>(),
        this.RelicOption<LoomingFruit>(),
        this.RelicOption<SignetRing>()
      });
    }
  }

  private EventOption BeautifulBraceletEventOption => this.RelicOption<BeautifulBracelet>();

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> list = this.OptionPool.ToList<EventOption>();
    if (this.Owner.Deck.Cards.Count<CardModel>(new Func<CardModel, bool>(((EnchantmentModel) ModelDb.Enchantment<Swift>()).CanEnchant)) >= 4)
      list.Add(this.BeautifulBraceletEventOption);
    list.UnstableShuffle<EventOption>(this.Rng);
    return (IReadOnlyList<EventOption>) list.Take<EventOption>(3).ToList<EventOption>();
  }

  protected override Color EventButtonColor => new Color("000000BF");
}
