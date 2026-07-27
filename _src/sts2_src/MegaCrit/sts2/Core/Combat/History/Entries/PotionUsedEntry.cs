// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.PotionUsedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class PotionUsedEntry : CombatHistoryEntry
{
  public PotionModel Potion { get; }

  public Creature? Target { get; }

  public override string Description
  {
    get
    {
      StringBuilder stringBuilder1 = new StringBuilder($"{this.Actor.Player.Character.Id.Entry} drank {this.Potion.Id.Entry}");
      if (this.Target != null)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        StringBuilder stringBuilder3 = stringBuilder2;
        StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 1, stringBuilder2);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" targeting ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this.Target.Monster.Id.Entry);
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
        stringBuilder3.Append(ref local);
      }
      return stringBuilder1.ToString();
    }
  }

  public PotionUsedEntry(
    PotionModel potion,
    Creature? target,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(potion.Owner.Creature, roundNumber, currentSide, history, players)
  {
    this.Potion = potion;
    this.Target = target;
  }
}
