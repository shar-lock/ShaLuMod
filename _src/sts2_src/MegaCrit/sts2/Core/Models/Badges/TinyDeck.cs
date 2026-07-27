// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.TinyDeck
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class TinyDeck(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "TINY_DECK", true, false)
{
  public override BadgeRarity Rarity
  {
    get
    {
      int count = this._localPlayer.Deck.Count;
      return count > 10 ? (count <= 20 ? BadgeRarity.Bronze : BadgeRarity.None) : (count <= 5 ? BadgeRarity.Gold : BadgeRarity.Silver);
    }
  }

  public override bool IsObtained() => this.Rarity != 0;
}
