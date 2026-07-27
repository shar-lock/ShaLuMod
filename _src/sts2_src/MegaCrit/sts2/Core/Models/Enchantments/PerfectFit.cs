// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.PerfectFit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class PerfectFit : EnchantmentModel
{
  public override bool HasExtraCardText => true;

  public override void ModifyShuffleOrder(
    Player player,
    List<CardModel> cards,
    bool isInitialShuffle)
  {
    if (isInitialShuffle || !cards.Contains(this.Card))
      return;
    cards.Remove(this.Card);
    cards.Insert(0, this.Card);
  }
}
