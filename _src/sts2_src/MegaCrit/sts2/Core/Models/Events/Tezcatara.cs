// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Tezcatara
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public class Tezcatara : AncientEventModel
{
  public override IEnumerable<CharacterModel> AnyCharacterDialogueBlacklist
  {
    get
    {
      return (IEnumerable<CharacterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CharacterModel>((CharacterModel) ModelDb.Character<Defect>());
    }
  }

  public override Color ButtonColor => new Color(0.08f, 0.04f, 0.0f, 0.75f);

  public override Color DialogueColor => new Color("33251E");

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
      List<EventOption>[] source = new List<EventOption>[4]
      {
        this.OptionPool1,
        this.OptionPool2,
        this.OptionPool3,
        null
      };
      int capacity = 1;
      List<EventOption> eventOptionList = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(eventOptionList, capacity);
      CollectionsMarshal.AsSpan<EventOption>(eventOptionList)[0] = this.NutritiousSoupOption;
      source[3] = eventOptionList;
      return ((IEnumerable<List<EventOption>>) source).SelectMany<List<EventOption>, EventOption>((Func<List<EventOption>, IEnumerable<EventOption>>) (x => (IEnumerable<EventOption>) x));
    }
  }

  private EventOption NutritiousSoupOption => this.RelicOption<NutritiousSoup>();

  private List<EventOption> OptionPool1
  {
    get
    {
      int capacity = 2;
      List<EventOption> optionPool1 = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(optionPool1, capacity);
      Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(optionPool1);
      int num1 = 0;
      span[num1] = this.RelicOption<VeryHotCocoa>();
      int num2 = num1 + 1;
      span[num2] = this.RelicOption<YummyCookie>();
      return optionPool1;
    }
  }

  private List<EventOption> OptionPool2
  {
    get
    {
      int capacity = 3;
      List<EventOption> optionPool2 = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(optionPool2, capacity);
      Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(optionPool2);
      int num1 = 0;
      span[num1] = this.RelicOption<BiiigHug>();
      int num2 = num1 + 1;
      span[num2] = this.RelicOption<Storybook>();
      int num3 = num2 + 1;
      span[num3] = this.RelicOption<ToastyMittens>();
      return optionPool2;
    }
  }

  private List<EventOption> OptionPool3
  {
    get
    {
      int capacity = 4;
      List<EventOption> optionPool3 = new List<EventOption>(capacity);
      CollectionsMarshal.SetCount<EventOption>(optionPool3, capacity);
      Span<EventOption> span = CollectionsMarshal.AsSpan<EventOption>(optionPool3);
      int num1 = 0;
      span[num1] = this.RelicOption<GoldenCompass>();
      int num2 = num1 + 1;
      span[num2] = this.RelicOption<PumpkinCandle>();
      int num3 = num2 + 1;
      span[num3] = this.RelicOption<ToyBox>();
      int num4 = num3 + 1;
      span[num4] = this.RelicOption<SealOfGold>();
      return optionPool3;
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> list = this.OptionPool1.ToList<EventOption>();
    if (this.Owner.Deck.Cards.Any<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Strike) && c.Rarity == CardRarity.Basic)))
      list.Add(this.NutritiousSoupOption);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) list),
      this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) this.OptionPool2),
      this.Rng.NextItem<EventOption>((IEnumerable<EventOption>) this.OptionPool3)
    });
  }
}
