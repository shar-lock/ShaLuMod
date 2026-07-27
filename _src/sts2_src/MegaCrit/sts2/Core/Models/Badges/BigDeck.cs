// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.BigDeck
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class BigDeck(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "BIG_DECK", true, false)
{
  public override BadgeRarity Rarity
  {
    get
    {
      int count = this._localPlayer.Deck.Count;
      return count < 60 ? (count >= 40 ? BadgeRarity.Bronze : BadgeRarity.None) : (count >= 100 ? BadgeRarity.Gold : BadgeRarity.Silver);
    }
  }

  public override bool IsObtained() => this.Rarity != 0;
}
