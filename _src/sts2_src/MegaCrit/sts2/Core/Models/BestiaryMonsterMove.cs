// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.BestiaryMonsterMove
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public struct BestiaryMonsterMove
{
  private static readonly LocString _attackMoveName = new LocString("bestiary", "ACTION_NAME.attack");
  private static readonly LocString _castMoveName = new LocString("bestiary", "ACTION_NAME.cast");
  private static readonly LocString _dieMoveName = new LocString("bestiary", "ACTION_NAME.die");
  private static readonly LocString _hurtMoveName = new LocString("bestiary", "ACTION_NAME.hurt");
  private static readonly LocString _reviveMoveName = new LocString("bestiary", "ACTION_NAME.revive");
  private static readonly LocString _stunMoveName = new LocString("bestiary", "ACTION_NAME.stun");
  public string displayName;
  public string? animId;
  public string? sfx;
  public string? stateId;
  public Func<IReadOnlyList<Creature>, Task>? nonStateMove;
  public Func<Task>? action;
  public bool stopSfxLoops;

  public static BestiaryMonsterMove FromAnim(string animId, string? sfx)
  {
    BestiaryMonsterMove bestiaryMonsterMove = new BestiaryMonsterMove();
    bestiaryMonsterMove.animId = animId;
    string str1 = animId;
    string str2;
    if (!animId.StartsWith("attack"))
    {
      switch (str1)
      {
        case "cast":
          str2 = BestiaryMonsterMove._castMoveName.GetRawText();
          break;
        case "die":
          str2 = BestiaryMonsterMove._dieMoveName.GetRawText();
          break;
        case "hurt":
          str2 = BestiaryMonsterMove._hurtMoveName.GetRawText();
          break;
        case "revive":
          str2 = BestiaryMonsterMove._reviveMoveName.GetRawText();
          break;
        case "stun":
          str2 = BestiaryMonsterMove._stunMoveName.GetRawText();
          break;
        default:
          str2 = animId;
          break;
      }
    }
    else
      str2 = BestiaryMonsterMove._attackMoveName.GetRawText();
    bestiaryMonsterMove.displayName = str2;
    bestiaryMonsterMove.sfx = sfx;
    return bestiaryMonsterMove;
  }

  public BestiaryMonsterMove StopOtherSfx()
  {
    this.stopSfxLoops = true;
    return this;
  }

  public static BestiaryMonsterMove FromAnim(LocString moveName, string animId, string? sfx)
  {
    return new BestiaryMonsterMove()
    {
      animId = animId,
      displayName = moveName.GetRawText(),
      sfx = sfx
    };
  }

  public static BestiaryMonsterMove FromState(LocString moveName, string stateId)
  {
    return new BestiaryMonsterMove()
    {
      displayName = moveName.GetRawText(),
      stateId = stateId
    };
  }

  public static BestiaryMonsterMove FromState(string stateId)
  {
    return new BestiaryMonsterMove()
    {
      displayName = stateId,
      stateId = stateId
    };
  }

  public static BestiaryMonsterMove FromNonStateMove(
    LocString moveName,
    Func<IReadOnlyList<Creature>, Task> nonStateMove)
  {
    return new BestiaryMonsterMove()
    {
      displayName = moveName.GetRawText(),
      nonStateMove = nonStateMove
    };
  }

  public static BestiaryMonsterMove FromAction(LocString moveName, Func<Task> action)
  {
    return new BestiaryMonsterMove()
    {
      displayName = moveName.GetRawText(),
      action = action
    };
  }

  public static BestiaryMonsterMove FromStun(Func<Task> action)
  {
    return BestiaryMonsterMove.FromAction(new LocString("monsters", "GENERIC.moves.STUNNED.title"), action);
  }
}
