// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.OstyCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class OstyCmd
{
  public static async Task<SummonResult> Summon(
    PlayerChoiceContext choiceContext,
    Player summoner,
    Decimal amount,
    AbstractModel? source)
  {
    ICombatState combatState = summoner.Creature.CombatState;
    amount = Hook.ModifySummonAmount(combatState, summoner, amount, source);
    if (amount == 0M)
      return new SummonResult(summoner.Osty, 0M);
    if (CombatManager.Instance.IsInProgress)
      SfxCmd.Play("event:/sfx/characters/necrobinder/necrobinder_summon");
    Creature osty = combatState.Allies.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.Monster is Osty && c.PetOwner == summoner));
    if (summoner.IsOstyAlive)
    {
      await CreatureCmd.GainMaxHp(summoner.Osty, amount);
    }
    else
    {
      bool isReviving = osty != null;
      if (isReviving)
      {
        if (osty.IsAlive)
          throw new InvalidOperationException("We shouldn't make it here if Osty is still alive!");
        summoner.PlayerCombatState.AddPetInternal(osty);
      }
      else
      {
        osty = await PlayerCmd.AddPet<Osty>(summoner);
        NCreature ostyNode = NCombatRoom.Instance?.GetCreatureNode(osty);
        if (ostyNode != null && source is CardModel)
        {
          ((CanvasItem) ostyNode).Modulate = Colors.Transparent;
          ((Node) ostyNode).CreateTween().TweenProperty((GodotObject) ostyNode, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.34999999403953552).SetDelay(0.10000000149011612);
          ostyNode.StartReviveAnim();
        }
        DieForYouPower dieForYouPower = await PowerCmd.Apply<DieForYouPower>(choiceContext, osty, 1M, (Creature) null, (CardModel) null);
        ostyNode?.TrackBlockStatus(summoner.Creature);
        ostyNode = (NCreature) null;
      }
      Decimal num = await CreatureCmd.SetMaxHp(osty, amount);
      await CreatureCmd.Heal(osty, amount, isReviving);
      if (isReviving)
        await Hook.AfterOstyRevived(combatState, osty);
    }
    if (osty != null)
      NCombatRoom.Instance?.GetCreatureNode(osty)?.OstyScaleToSize((float) osty.MaxHp, 0.75);
    CombatManager.Instance.History.Summoned(combatState, (int) amount, summoner);
    await Hook.AfterSummon(combatState, choiceContext, summoner, amount);
    return new SummonResult(summoner.Osty, amount);
  }
}
