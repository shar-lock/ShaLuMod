// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TheArchitect
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TheArchitect : EventModel
{
  private const string _locTable = "ancients";
  private static readonly LocString _emptyLocString = new LocString("ancients", "PROCEED.description");
  private static readonly LocString _continueLocString = new LocString("ancients", "THE_ARCHITECT.CONTINUE");
  private static readonly LocString _respondLocString = new LocString("ancients", "THE_ARCHITECT.RESPOND");
  private AncientDialogue? _dialogue;
  private int _currentLineIndex;
  private NSpeechBubbleVfx? _speechBubble;
  private Creature? _architectCreature;
  private int _score;
  private AncientDialogueSet? _dialogueSet;

  public override EventLayoutType LayoutType => EventLayoutType.Combat;

  public override EncounterModel CanonicalEncounter
  {
    get => (EncounterModel) ModelDb.Encounter<TheArchitectEventEncounter>();
  }

  public override string LocTable => "ancients";

  private AncientDialogue? Dialogue
  {
    get => this._dialogue;
    set
    {
      this.AssertMutable();
      this._dialogue = value;
    }
  }

  private int CurrentLineIndex
  {
    get => this._currentLineIndex;
    set
    {
      this.AssertMutable();
      this._currentLineIndex = value;
    }
  }

  private NSpeechBubbleVfx? SpeechBubble
  {
    get => this._speechBubble;
    set
    {
      this.AssertMutable();
      this._speechBubble = value;
    }
  }

  private Creature? ArchitectCreature
  {
    get => this._architectCreature;
    set
    {
      this.AssertMutable();
      this._architectCreature = value;
    }
  }

  private int Score
  {
    get => this._score;
    set
    {
      this.AssertMutable();
      this._score = value;
    }
  }

  public override IEnumerable<LocString> GameInfoOptions
  {
    get => (IEnumerable<LocString>) Array.Empty<LocString>();
  }

  private bool IsOnLastLine
  {
    get => this.Dialogue == null || this.CurrentLineIndex >= this.Dialogue.Lines.Count - 1;
  }

  protected override void SetInitialEventState(bool isPreFinished)
  {
    this.SetEventState(TheArchitect._emptyLocString, (IEnumerable<EventOption>) this.GenerateInitialOptionsWrapper());
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    this.LoadDialogue();
    if (this.Dialogue == null || this.Dialogue.Lines.Count == 0)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.CreateProceedOption());
    }
    this.CurrentLineIndex = 0;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.CreateOptionForCurrentLine());
  }

  public override void OnRoomEnter()
  {
    StatsManager.RefreshGlobalStats();
    NCombatRoom instance = NCombatRoom.Instance;
    this.ArchitectCreature = instance != null ? instance.CreatureNodes.FirstOrDefault<NCreature>((Func<NCreature, bool>) (n => n.Entity.Side == CombatSide.Enemy))?.Entity : (Creature) null;
    this.Score = ScoreUtility.CalculateScore(this.Owner.RunState, true);
    if (LocalContext.IsMe(this.Owner))
    {
      if (this.ArchitectCreature != null)
        this.GetArchitectAnimationState()?.SetAnimation("_tracks/head_reading", trackId: 1);
      AncientDialogue dialogue = this.Dialogue;
      if (dialogue != null)
      {
        IReadOnlyList<AncientDialogueLine> lines = dialogue.Lines;
        if (lines != null && lines.Count > 0)
          this.ClearCurrentOptions();
      }
    }
    TaskHelper.RunSafely(this.PlayCurrentLine());
  }

  public void TriggerVictory()
  {
    if (!LocalContext.IsMe(this.Owner))
      return;
    NCombatRoom.Instance?.SetWaitingForOtherPlayersOverlayVisible(false);
  }

  public AncientDialogueSet DialogueSet
  {
    get
    {
      if (this._dialogueSet == null)
      {
        this._dialogueSet = TheArchitect.DefineDialogues();
        this._dialogueSet.PopulateLocKeys(this.Id.Entry);
      }
      return this._dialogueSet;
    }
  }

  private static string CharKey<T>() where T : CharacterModel => ModelDb.Character<T>().Id.Entry;

  private static AncientDialogueSet DefineDialogues()
  {
    AncientDialogueSet ancientDialogueSet1 = new AncientDialogueSet();
    ancientDialogueSet1.FirstVisitEverDialogue = (AncientDialogue) null;
    AncientDialogueSet ancientDialogueSet2 = ancientDialogueSet1;
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary1 = new Dictionary<string, IReadOnlyList<AncientDialogue>>();
    string key1 = TheArchitect.CharKey<Ironclad>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key1] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[2]{ "", "" })
      {
        VisitIndex = new int?(0),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(1),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(2),
        EndAttackers = ArchitectAttackers.Both
      }
    });
    string key2 = TheArchitect.CharKey<Silent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key2] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[4]
    {
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(0),
        StartAttackers = ArchitectAttackers.Player,
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(1),
        StartAttackers = ArchitectAttackers.Player,
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(2),
        StartAttackers = ArchitectAttackers.Player,
        EndAttackers = ArchitectAttackers.Architect
      },
      new AncientDialogue(new string[1]{ "" })
      {
        VisitIndex = new int?(3),
        StartAttackers = ArchitectAttackers.Player,
        EndAttackers = ArchitectAttackers.Architect
      }
    });
    string key3 = TheArchitect.CharKey<Defect>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key3] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(0),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(1),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(2),
        EndAttackers = ArchitectAttackers.Both
      }
    });
    string key4 = TheArchitect.CharKey<Necrobinder>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key4] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[4]
    {
      new AncientDialogue(new string[2]{ "", "" })
      {
        VisitIndex = new int?(0),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[2]{ "", "" })
      {
        VisitIndex = new int?(1),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[2]{ "", "" })
      {
        VisitIndex = new int?(2),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(3),
        EndAttackers = ArchitectAttackers.Both
      }
    });
    string key5 = TheArchitect.CharKey<Regent>();
    // ISSUE: object of a compiler-generated type is created
    dictionary1[key5] = (IReadOnlyList<AncientDialogue>) new \u003C\u003Ez__ReadOnlyArray<AncientDialogue>(new AncientDialogue[3]
    {
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(0),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(1),
        EndAttackers = ArchitectAttackers.Both
      },
      new AncientDialogue(new string[3]{ "", "", "" })
      {
        VisitIndex = new int?(2),
        EndAttackers = ArchitectAttackers.Both
      }
    });
    Dictionary<string, IReadOnlyList<AncientDialogue>> dictionary2 = dictionary1;
    ancientDialogueSet2.CharacterDialogues = dictionary2;
    ancientDialogueSet1.AgnosticDialogues = (IReadOnlyList<AncientDialogue>) Array.Empty<AncientDialogue>();
    return ancientDialogueSet1;
  }

  private void LoadDialogue()
  {
    CharacterStats statsForCharacter = SaveManager.Instance.Progress.GetStatsForCharacter(this.Owner.Character.Id);
    this.Dialogue = this.Rng.NextItem<AncientDialogue>((IEnumerable<AncientDialogue>) this.DialogueSet.GetValidDialogues(this.Owner.Character.Id, statsForCharacter != null ? statsForCharacter.TotalWins : 0, SaveManager.Instance.Progress.Wins, false).ToList<AncientDialogue>());
  }

  private EventOption CreateOptionForCurrentLine()
  {
    AncientDialogueLine line = this.Dialogue.Lines[this.CurrentLineIndex];
    EventOption optionForCurrentLine;
    if (this.IsOnLastLine)
      optionForCurrentLine = this.CreateProceedOption();
    else
      optionForCurrentLine = new EventOption((EventModel) this, new Func<Task>(this.AdvanceDialogue), line.NextButtonText == null ? (line.Speaker != AncientDialogueSpeaker.Ancient ? TheArchitect._continueLocString : TheArchitect._respondLocString) : line.NextButtonText, TheArchitect._emptyLocString, $"{this.Id.Entry}.dialogue.{this.CurrentLineIndex}", (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>()).ThatWontSaveToChoiceHistory();
    return optionForCurrentLine;
  }

  private EventOption CreateProceedOption()
  {
    return new EventOption((EventModel) this, new Func<Task>(this.WinRun), "PROCEED", false, false, Array.Empty<IHoverTip>()).ThatWontSaveToChoiceHistory();
  }

  private async Task AdvanceDialogue()
  {
    this.CurrentLineIndex++;
    IEnumerable<EventOption> eventOptions;
    if (this.CurrentLineIndex < this.Dialogue.Lines.Count)
    {
      await this.PlayCurrentLine();
      // ISSUE: object of a compiler-generated type is created
      eventOptions = (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.CreateOptionForCurrentLine());
    }
    else
    {
      // ISSUE: object of a compiler-generated type is created
      eventOptions = (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.CreateProceedOption());
    }
    this.SetEventState(TheArchitect._emptyLocString, eventOptions);
  }

  private async Task WinRun()
  {
    if (!LocalContext.IsMe(this.Owner))
      return;
    int num = await this.AnimPlayerAttackIfNecessary(this.Dialogue.EndAttackers) ? 1 : 0;
    await this.AnimArchitectAttackIfNecessary(this.Dialogue.EndAttackers);
    if (this.Owner.RunState.Players.Count > 1)
      NCombatRoom.Instance?.SetWaitingForOtherPlayersOverlayVisible(true);
    RunManager.Instance.ActChangeSynchronizer.SetLocalPlayerReady();
  }

  private async Task<bool> AnimPlayerAttackIfNecessary(ArchitectAttackers attackers)
  {
    bool flag;
    switch (attackers)
    {
      case ArchitectAttackers.Player:
      case ArchitectAttackers.Both:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag || this.ArchitectCreature == null)
      return false;
    List<string> shuffledVfx = this.Owner.Character.GetArchitectAttackVfx();
    this.Rng.Shuffle<string>((IList<string>) shuffledVfx);
    int[] damageParts = TheArchitect.DivideWildly(this.Score, shuffledVfx.Count, this.Rng);
    Control vfxContainer = NCombatRoom.Instance?.CombatVfxContainer;
    for (int i = 0; i < shuffledVfx.Count; ++i)
    {
      bool isFinalHit = i == shuffledVfx.Count - 1;
      await CreatureCmd.TriggerAnim(this.Owner.Creature, "Attack", 0.1f);
      Control parent1 = vfxContainer;
      if (parent1 != null)
        ((Godot.Node) parent1).AddChildSafely((Godot.Node) NDamageNumVfx.Create(this.ArchitectCreature, damageParts[i], false));
      Control parent2 = vfxContainer;
      if (parent2 != null)
        ((Godot.Node) parent2).AddChildSafely((Godot.Node) NHitSparkVfx.Create(this.ArchitectCreature, false));
      VfxCmd.PlayOnCreatureCenter(this.ArchitectCreature, shuffledVfx[i]);
      await CreatureCmd.TriggerAnim(this.ArchitectCreature, "Hit", 0.0f);
      if (isFinalHit)
        NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal);
      else
        NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short);
      if (!isFinalHit)
        await Cmd.Wait(0.1f);
    }
    await Cmd.Wait(2f);
    return true;
  }

  private static int[] DivideWildly(int total, int parts, Rng rng)
  {
    if (parts <= 0)
      return Array.Empty<int>();
    if (parts == 1)
      return new int[1]{ total };
    double[] source = new double[parts];
    int num1 = rng.NextInt(parts);
    int num2;
    do
    {
      num2 = rng.NextInt(parts);
    }
    while (num2 == num1);
    for (int index = 0; index < parts; ++index)
      source[index] = index != num1 ? (index != num2 ? (double) rng.NextFloat(0.7f, 1.3f) : (double) rng.NextFloat(0.1f, 0.5f)) : (double) rng.NextFloat(2f, 3f);
    double num3 = ((IEnumerable<double>) source).Sum();
    int[] numArray = new int[parts];
    int num4 = 0;
    for (int index = 0; index < parts - 1; ++index)
    {
      numArray[index] = Math.Max(1, (int) ((double) total * source[index] / num3));
      num4 += numArray[index];
    }
    numArray[parts - 1] = Math.Max(1, total - num4);
    return numArray;
  }

  private async Task AnimArchitectAttackIfNecessary(ArchitectAttackers attackers)
  {
    bool flag;
    switch (attackers)
    {
      case ArchitectAttackers.Architect:
      case ArchitectAttackers.Both:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag || this.ArchitectCreature == null)
      return;
    await CreatureCmd.TriggerAnim(this.ArchitectCreature, "Attack", 0.5f);
    VfxCmd.PlayOnCreature(this.Owner.Creature, "vfx/vfx_attack_lightning");
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Godot.Node) instance.CombatVfxContainer).AddChildSafely((Godot.Node) NFireBurstVfx.Create(this.Owner.Creature, 1f));
    await Cmd.Wait(0.5f);
  }

  private async Task PlayCurrentLine()
  {
    AncientDialogueLine line;
    Creature speaker;
    if (!LocalContext.IsMe(this.Owner))
    {
      line = (AncientDialogueLine) null;
      speaker = (Creature) null;
    }
    else
    {
      if (this.SpeechBubble != null)
      {
        TaskHelper.RunSafely(this.SpeechBubble.AnimOut());
        this.SpeechBubble = (NSpeechBubbleVfx) null;
      }
      if (this.Dialogue == null)
      {
        line = (AncientDialogueLine) null;
        speaker = (Creature) null;
      }
      else if (this.CurrentLineIndex >= this.Dialogue.Lines.Count)
      {
        line = (AncientDialogueLine) null;
        speaker = (Creature) null;
      }
      else
      {
        line = this.Dialogue.Lines[this.CurrentLineIndex];
        if (line.LineText == null)
        {
          line = (AncientDialogueLine) null;
          speaker = (Creature) null;
        }
        else
        {
          speaker = this.GetSpeaker(line.Speaker);
          if (speaker == null)
          {
            line = (AncientDialogueLine) null;
            speaker = (Creature) null;
          }
          else
          {
            if (this.CurrentLineIndex == 0)
            {
              if (this.Dialogue.StartAttackers != ArchitectAttackers.None || this.Dialogue.EndAttackers != ArchitectAttackers.None)
                await Cmd.Wait(0.75f);
              bool flag = await this.AnimPlayerAttackIfNecessary(this.Dialogue.StartAttackers);
              if (this.ArchitectCreature != null)
              {
                if (!flag)
                  await Cmd.Wait(1f);
                MegaAnimationState state = this.GetArchitectAnimationState();
                state?.SetAnimation("_tracks/head_stop_reading", false, 1);
                await Cmd.Wait(0.5f);
                state?.SetAnimation("_tracks/head_normal", trackId: 1);
                state = (MegaAnimationState) null;
              }
              await this.AnimArchitectAttackIfNecessary(this.Dialogue.StartAttackers);
            }
            this.ShowSpeechBubble(line, speaker);
            if (this.CurrentLineIndex != 0)
            {
              line = (AncientDialogueLine) null;
              speaker = (Creature) null;
            }
            else
            {
              // ISSUE: object of a compiler-generated type is created
              this.SetEventState(TheArchitect._emptyLocString, (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(this.CreateOptionForCurrentLine()));
              line = (AncientDialogueLine) null;
              speaker = (Creature) null;
            }
          }
        }
      }
    }
  }

  private void ShowSpeechBubble(AncientDialogueLine line, Creature speaker)
  {
    this.SpeechBubble = TalkCmd.Play(line.LineText, speaker, line.Speaker == AncientDialogueSpeaker.Ancient ? VfxColor.DarkGray : this.Owner.Character.SpeechBubbleColor, VfxDuration.Forever);
  }

  private Creature? GetSpeaker(AncientDialogueSpeaker speaker)
  {
    Creature speaker1;
    switch (speaker)
    {
      case AncientDialogueSpeaker.Ancient:
        speaker1 = this.ArchitectCreature;
        break;
      case AncientDialogueSpeaker.Character:
        speaker1 = this.Owner.Creature;
        break;
      default:
        speaker1 = (Creature) null;
        break;
    }
    return speaker1;
  }

  private MegaAnimationState? GetArchitectAnimationState()
  {
    return NCombatRoom.Instance?.GetCreatureNode(this.ArchitectCreature)?.SpineAnimation.GetAnimationState();
  }
}
