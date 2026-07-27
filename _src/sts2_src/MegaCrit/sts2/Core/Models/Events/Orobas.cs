// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Orobas
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Orobas : AncientEventModel
{
  private const float _prismaticOdds = 0.3333333f;

  public override Color ButtonColor => new Color(0.05f, 0.0f, 0.1f, 0.35f);

  public override Color DialogueColor => new Color("5C5F7A");

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
      new AncientDialogue(new string[2]{ "", "" })
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
      new AncientDialogue(new string[3]{ "", "", "" })
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
    ancientDialogueSet1.AgnosticDialogues = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[2]
    {
      new AncientDialogue(new string[1]{ "" }),
      new AncientDialogue(new string[1]{ "" })
    });
    return ancientDialogueSet1;
  }

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get
    {
      return ((IEnumerable<IEnumerable<EventOption>>) new IEnumerable<EventOption>[5]
      {
        this.OptionPool1,
        this.OptionPool2,
        this.OptionPool3,
        this.SeaGlassOptions,
        (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.PrismaticGemOption)
      }).SelectMany<IEnumerable<EventOption>, EventOption>((Func<IEnumerable<EventOption>, IEnumerable<EventOption>>) (x => x));
    }
  }

  private IEnumerable<EventOption> OptionPool1
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
      {
        this.RelicOption<ElectricShrymp>(),
        this.RelicOption<GlassEye>()
      });
    }
  }

  private IEnumerable<EventOption> OptionPool2
  {
    get
    {
      return (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[4]
      {
        this.RelicOption<AlchemicalCoffer>(),
        this.RelicOption<Driftwood>(),
        this.RelicOption<RadiantPearl>(),
        this.RelicOption<SandCastle>()
      });
    }
  }

  private IEnumerable<EventOption> SeaGlassOptions
  {
    get
    {
      List<EventOption> seaGlassOptions = new List<EventOption>();
      foreach (CharacterModel allCharacter in ModelDb.AllCharacters)
      {
        SeaGlass mutable = (SeaGlass) ModelDb.Relic<SeaGlass>().ToMutable();
        mutable.CharacterId = allCharacter.Id;
        seaGlassOptions.Add(this.RelicOption((RelicModel) mutable));
      }
      return (IEnumerable<EventOption>) seaGlassOptions;
    }
  }

  private EventOption PrismaticGemOption => this.RelicOption<PrismaticGem>();

  private IEnumerable<EventOption> OptionPool3
  {
    get
    {
      List<EventOption> optionPool3 = new List<EventOption>();
      TouchOfOrobas mutable1 = (TouchOfOrobas) ModelDb.Relic<TouchOfOrobas>().ToMutable();
      if (this.Owner != null)
      {
        if (mutable1.SetupForPlayer(this.Owner))
          optionPool3.Add(this.RelicOption((RelicModel) mutable1));
      }
      else
        optionPool3.Add(this.RelicOption((RelicModel) mutable1));
      ArchaicTooth mutable2 = (ArchaicTooth) ModelDb.Relic<ArchaicTooth>().ToMutable();
      if (this.Owner != null)
      {
        if (mutable2.SetupForPlayer(this.Owner))
          optionPool3.Add(this.RelicOption((RelicModel) mutable2));
      }
      else
        optionPool3.Add(this.RelicOption((RelicModel) mutable2));
      if (optionPool3.Count == 0)
        optionPool3.Add(new EventOption((EventModel) this, (Func<Task>) null, "OROBAS.pages.INITIAL.options.OPTION_POOL_3_LOCKED", Array.Empty<IHoverTip>()));
      return (IEnumerable<EventOption>) optionPool3;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    CharacterModel character = this.Owner.Character;
    CharacterModel characterModel = this.Rng.NextItem<CharacterModel>(this.Owner.UnlockState.Characters.Where<CharacterModel>((Func<CharacterModel, bool>) (c => c.Id != character.Id))) ?? character;
    List<EventOption> list = this.OptionPool1.ToList<EventOption>();
    EventOption eventOption;
    if ((double) this.Rng.NextFloat() < 0.33333331346511841)
    {
      eventOption = this.PrismaticGemOption;
    }
    else
    {
      SeaGlass mutable = (SeaGlass) ModelDb.Relic<SeaGlass>().ToMutable();
      mutable.CharacterId = characterModel.Id;
      eventOption = this.RelicOption((RelicModel) mutable);
    }
    list.Add(eventOption);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) list),
      this.Rng.NextItem<EventOption>(this.OptionPool2),
      this.Rng.NextItem<EventOption>(this.OptionPool3)
    });
  }
}
