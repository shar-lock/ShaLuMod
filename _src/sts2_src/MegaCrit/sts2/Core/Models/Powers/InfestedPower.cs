// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.InfestedPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class InfestedPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature target,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    List<Creature> wrigglers;
    if (wasRemovalPrevented)
      wrigglers = (List<Creature>) null;
    else if (this.Owner != target)
    {
      wrigglers = (List<Creature>) null;
    }
    else
    {
      wrigglers = new List<Creature>();
      for (int i = 0; i < 4; ++i)
      {
        Wriggler mutable = (Wriggler) ModelDb.Monster<Wriggler>().ToMutable();
        mutable.StartStunned = true;
        Creature creature = await CreatureCmd.Add((MonsterModel) mutable, this.CombatState, this.Owner.Side, PhrogParasiteElite.GetWrigglerSlotName(i));
        creature.SetNodeVisible(false);
        wrigglers.Add(creature);
      }
      TaskHelper.RunSafely(InfestedPower.RevealWrigglersAfterDeathAnim(wrigglers, deathAnimLength));
      wrigglers = (List<Creature>) null;
    }
  }

  private static async Task RevealWrigglersAfterDeathAnim(
    List<Creature> wrigglers,
    float deathAnimLength)
  {
    await Cmd.CustomScaledWait(deathAnimLength, deathAnimLength);
    if (TestMode.IsOff)
      NRunMusicController.Instance.TriggerEliteSecondPhase();
    foreach (Creature wriggler in wrigglers)
      wriggler.SetNodeVisible(true);
  }

  public override bool ShouldStopCombatFromEnding() => true;
}
