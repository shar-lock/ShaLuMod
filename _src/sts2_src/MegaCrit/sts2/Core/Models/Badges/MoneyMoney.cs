// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.MoneyMoney
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class MoneyMoney(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "MONEY_MONEY", true, false)
{
  public override BadgeRarity Rarity
  {
    get
    {
      int gold = this._localPlayer.Gold;
      return gold < 400 ? (gold >= 200 ? BadgeRarity.Bronze : BadgeRarity.None) : (gold >= 600 ? BadgeRarity.Gold : BadgeRarity.Silver);
    }
  }

  public override bool IsObtained() => this.Rarity != 0;
}
