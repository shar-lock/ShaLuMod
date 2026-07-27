// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.JungleMazeAdventure
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class JungleMazeAdventure : EventModel
{
  private const string _soloGoldKey = "SoloGold";
  private const string _soloHpKey = "SoloHp";
  private const string _joinForcesGoldKey = "JoinForcesGold";
  private static readonly List<(string, string)> _fx;

  public override bool IsShared => true;

  public override bool IsAllowed(IRunState runState)
  {
    if (runState.Players.Count == 1)
      return true;
    foreach (Player player in (IEnumerable<Player>) runState.Players)
    {
      if ((Decimal) player.Creature.CurrentHp <= this.DynamicVars["SoloHp"].BaseValue)
        return false;
    }
    return true;
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.DontNeedHelp), "JUNGLE_MAZE_ADVENTURE.pages.INITIAL.options.SOLO_QUEST", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars["SoloHp"].BaseValue),
      new EventOption((EventModel) this, new Func<Task>(this.SafetyInNumbers), "JUNGLE_MAZE_ADVENTURE.pages.INITIAL.options.JOIN_FORCES", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        new DynamicVar("SoloGold", 150M),
        (DynamicVar) new DamageVar("SoloHp", 18M, ValueProp.Unblockable | ValueProp.Unpowered),
        new DynamicVar("JoinForcesGold", 50M)
      });
    }
  }

  public override void CalculateVars()
  {
    this.DynamicVars["SoloGold"].BaseValue += (Decimal) this.Rng.NextFloat(-15f, 15f);
    this.DynamicVars["JoinForcesGold"].BaseValue += (Decimal) this.Rng.NextFloat(-15f, 15f);
  }

  private async Task DontNeedHelp()
  {
    List<(string, string)> shuffledFx = JungleMazeAdventure._fx.ToList<(string, string)>().StableShuffle<(string, string)>(this.Rng);
    for (int i = 0; i < 3; ++i)
    {
      Control vfxContainer = NEventRoom.Instance?.VfxContainer;
      if (LocalContext.IsMe(this.Owner) && vfxContainer != null)
      {
        VfxCmd.PlayNonCombatVfx((Godot.Node) vfxContainer, new Vector2(vfxContainer.Size.X * 0.25f, vfxContainer.Size.Y * 0.5f), shuffledFx[i].Item1);
        NDebugAudioManager.Instance.Play(shuffledFx[i].Item2);
      }
      if (i < 2)
        await Cmd.CustomScaledWait(0.25f, 0.5f);
    }
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars["SoloHp"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
    await PlayerCmd.GainGold(this.DynamicVars["SoloGold"].BaseValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("JUNGLE_MAZE_ADVENTURE.pages.SOLO_QUEST.description"));
    shuffledFx = (List<(string, string)>) null;
  }

  private async Task SafetyInNumbers()
  {
    NDebugAudioManager.Instance.Play("hey.mp3");
    await Cmd.CustomScaledWait(0.0f, 0.2f);
    await PlayerCmd.GainGold(this.DynamicVars["JoinForcesGold"].BaseValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("JUNGLE_MAZE_ADVENTURE.pages.JOIN_FORCES.description"));
  }

  static JungleMazeAdventure()
  {
    int capacity = 3;
    List<(string, string)> valueTupleList = new List<(string, string)>(capacity);
    CollectionsMarshal.SetCount<(string, string)>(valueTupleList, capacity);
    Span<(string, string)> span = CollectionsMarshal.AsSpan<(string, string)>(valueTupleList);
    int num1 = 0;
    span[num1] = ("vfx/vfx_attack_blunt", "blunt_attack.mp3");
    int num2 = num1 + 1;
    span[num2] = ("vfx/vfx_attack_slash", "slash_attack.mp3");
    int num3 = num2 + 1;
    span[num3] = ("vfx/vfx_heavy_blunt", "heavy_attack.mp3");
    JungleMazeAdventure._fx = valueTupleList;
  }
}
