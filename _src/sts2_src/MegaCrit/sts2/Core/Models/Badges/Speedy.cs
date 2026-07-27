// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.Speedy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class Speedy(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "SPEEDY", true, false)
{
  private const int _winTimeGold = 1800;
  private const int _winTimeSilver = 2400;
  private const int _winTimeBronze = 3000;

  public override BadgeRarity Rarity
  {
    get
    {
      long winTime = this._run.WinTime;
      return winTime > 2400L ? (winTime <= 3000L ? BadgeRarity.Bronze : BadgeRarity.None) : (winTime <= 1800L ? BadgeRarity.Gold : BadgeRarity.Silver);
    }
  }

  public override bool IsObtained() => this.Rarity != 0;
}
