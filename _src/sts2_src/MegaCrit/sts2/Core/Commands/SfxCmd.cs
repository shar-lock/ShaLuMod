// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.SfxCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Audio;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class SfxCmd
{
  public static void Play(string sfx, float volume = 1f)
  {
    if (NonInteractiveMode.IsActive || CombatManager.Instance.IsEnding)
      return;
    NAudioManager.Instance.PlayOneShot(sfx, volume);
  }

  public static void Play(string sfx, string param, float val, float volume = 1f)
  {
    if (NonInteractiveMode.IsActive || CombatManager.Instance.IsEnding)
      return;
    NAudioManager instance = NAudioManager.Instance;
    string path = sfx;
    Dictionary<string, float> parameters = new Dictionary<string, float>();
    parameters.Add(param, val);
    double volume1 = (double) volume;
    instance.PlayOneShot(path, parameters, (float) volume1);
  }

  public static void PlayLoop(string sfx, bool usesLoopParam = true)
  {
    if (NonInteractiveMode.IsActive)
      return;
    NAudioManager.Instance.PlayLoop(sfx, usesLoopParam);
  }

  public static void PlayLoop(Creature creature, string sfx)
  {
    if (NonInteractiveMode.IsActive)
      return;
    creature.GetCreatureNode()?.StartSfxLoop(sfx);
  }

  public static void PlayLoop(
    Creature creature,
    string sfx,
    string loopParam,
    float loopStopValue)
  {
    if (NonInteractiveMode.IsActive)
      return;
    creature.GetCreatureNode()?.StartSfxLoop(sfx, loopParam, loopStopValue);
  }

  public static void StopLoop(string sfx)
  {
    if (NonInteractiveMode.IsActive)
      return;
    NAudioManager.Instance.StopLoop(sfx);
  }

  public static void StopLoop(Creature creature, string sfx)
  {
    if (NonInteractiveMode.IsActive)
      return;
    creature.GetCreatureNode()?.StopSfxLoop(sfx);
  }

  public static void SetParam(string sfx, string param, float value)
  {
    if (NonInteractiveMode.IsActive)
      return;
    NAudioManager.Instance.SetParam(sfx, param, value);
  }

  public static void PlayDamage(MonsterModel? monster, int damageAmount)
  {
    if (NonInteractiveMode.IsActive || CombatManager.Instance.IsEnding || monster == null)
      return;
    NAudioManager.Instance.PlayOneShot(monster.TakeDamageSfx, new Dictionary<string, float>()
    {
      {
        "EnemyImpact_Intensity",
        2f
      }
    });
  }

  public static void PlayDeath(MonsterModel? monster)
  {
    if (NonInteractiveMode.IsActive || monster == null)
      return;
    NAudioManager.Instance.PlayOneShot(monster.DeathSfx);
  }

  public static void PlayDeath(Player player)
  {
    if (NonInteractiveMode.IsActive)
      return;
    NAudioManager.Instance.PlayOneShot(player.Character.DeathSfx);
  }

  public static void PlayCardSwooshSfx(CardPile currentPile, CardPile? prevPile = null)
  {
    if (currentPile.Type == PileType.Draw)
      SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_into_draw");
    else if (currentPile.Type == PileType.Discard)
    {
      if (prevPile != null && prevPile.Type == PileType.Play)
        SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_play_into_discard");
      else
        SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_into_discard");
    }
    else
      SfxCmd.Play("event:/sfx/ui/cards/card_movement_B_into_deck");
  }
}
