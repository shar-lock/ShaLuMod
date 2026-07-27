// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.Glutton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class Glutton(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "GLUTTON", true, false)
{
  public override BadgeRarity Rarity
  {
    get
    {
      int num = this._localPlayer.MaxHp - SaveUtil.CharacterOrDeprecated(this._localPlayer.CharacterId).StartingHp;
      return num < 30 ? (num >= 15 ? BadgeRarity.Bronze : BadgeRarity.None) : (num >= 50 ? BadgeRarity.Gold : BadgeRarity.Silver);
    }
  }

  public override bool IsObtained() => this.Rarity != 0;
}
