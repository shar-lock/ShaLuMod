// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Tanx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Tanx : AncientEventModel
{
  private const string _sfxLaugh = "event:/sfx/npcs/tanx/tanx_laugh";
  private const string _sfxRoar = "event:/sfx/npcs/tanx/tanx_roar";
  private const string _sfxCuriosity = "event:/sfx/npcs/tanx/tanx_curiosity";
  private const int _triBoomerangCount = 3;

  public override Color ButtonColor => new Color(0.05f, 0.02f, 0.0f, 0.5f);

  public override Color DialogueColor => new Color("731717");

  protected override AncientDialogueSet DefineDialogues()
  {
    AncientDialogueSet ancientDialogueSet1 = new AncientDialogueSet();
    ancientDialogueSet1.FirstVisitEverDialogue = new AncientDialogue(new string[1]
    {
      "event:/sfx/npcs/tanx/tanx_laugh"
    });
    AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet1;
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary1 = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
    string key1 = AncientEventModel.CharKey<Ironclad>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key1] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_roar"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_laugh"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/tanx/tanx_roar",
        "",
        "event:/sfx/npcs/tanx/tanx_laugh"
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
        "event:/sfx/npcs/tanx/tanx_roar"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_curiosity"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/tanx/tanx_roar",
        "",
        "event:/sfx/npcs/tanx/tanx_curiosity"
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
        "event:/sfx/npcs/tanx/tanx_curiosity"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_laugh"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/tanx/tanx_roar",
        "",
        "event:/sfx/npcs/tanx/tanx_curiosity"
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
        "event:/sfx/npcs/tanx/tanx_roar",
        "",
        "event:/sfx/npcs/tanx/tanx_curiosity"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_roar"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/tanx/tanx_roar",
        "",
        "event:/sfx/npcs/tanx/tanx_laugh"
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
        "event:/sfx/npcs/tanx/tanx_curiosity",
        "",
        "event:/sfx/npcs/tanx/tanx_laugh"
      })
      {
        VisitIndex = new int?(0)
      },
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_roar"
      })
      {
        VisitIndex = new int?(1)
      },
      new AncientDialogue(new string[3]
      {
        "event:/sfx/npcs/tanx/tanx_roar",
        "",
        "event:/sfx/npcs/tanx/tanx_roar"
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
        "event:/sfx/npcs/tanx/tanx_roar"
      }),
      new AncientDialogue(new string[1]
      {
        "event:/sfx/npcs/tanx/tanx_laugh"
      })
    });
    return ancientDialogueSet1;
  }

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get => this.BaseOptionPool.Append<EventOption>(this.TriBoomerangOption);
  }

  private IEnumerable<EventOption> BaseOptionPool
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[9]
      {
        this.RelicOption<Claws>(),
        this.RelicOption<Crossbow>(),
        this.RelicOption<IronClub>(),
        this.RelicOption<MeatCleaver>(),
        this.RelicOption<Sai>(),
        this.RelicOption<SpikedGauntlets>(),
        this.RelicOption<TanxsWhistle>(),
        this.RelicOption<ThrowingAxe>(),
        this.RelicOption<WarHammer>()
      });
    }
  }

  private EventOption TriBoomerangOption => this.RelicOption<TriBoomerang>();

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> list = this.BaseOptionPool.ToList<EventOption>();
    if (this.Owner.Deck.Cards.Count<CardModel>((Func<CardModel, bool>) (c => ModelDb.Enchantment<Instinct>().CanEnchant(c))) >= 3)
      list.Add(this.TriBoomerangOption);
    return (IReadOnlyList<EventOption>) list.UnstableShuffle<EventOption>(this.Rng).Take<EventOption>(3).ToList<EventOption>();
  }
}
