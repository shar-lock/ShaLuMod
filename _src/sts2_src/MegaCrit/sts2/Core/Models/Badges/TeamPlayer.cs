// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.TeamPlayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class TeamPlayer(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "TEAM_PLAYER", false, true)
{
  public override BadgeRarity Rarity => BadgeRarity.Silver;

  public override bool IsObtained()
  {
    int num = 0;
    foreach (SerializableCard serializableCard in this._localPlayer.Deck)
    {
      if (SaveUtil.CardOrDeprecated(serializableCard.Id).MultiplayerConstraint == CardMultiplayerConstraint.MultiplayerOnly)
        ++num;
    }
    return num >= 3;
  }
}
