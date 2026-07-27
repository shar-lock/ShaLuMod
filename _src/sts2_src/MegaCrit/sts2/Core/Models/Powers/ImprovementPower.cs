// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ImprovementPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ImprovementPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Task AfterCombatEnd(CombatRoom room)
  {
    List<CardModel> list = PileType.Deck.GetPile(this.Owner.Player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>();
    for (int index = 0; index < this.Amount && list.Count != 0; ++index)
    {
      CardModel card = this.Owner.Player.RunState.Rng.CombatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) list);
      list.Remove(card);
      CardCmd.Upgrade(card);
    }
    return Task.CompletedTask;
  }
}
