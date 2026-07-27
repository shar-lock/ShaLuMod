// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.DamageLeader
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class DamageLeader(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "DAMAGE_LEADER", false, true)
{
  public override BadgeRarity Rarity => BadgeRarity.Bronze;

  public override bool IsObtained()
  {
    if (this._run.MapPointHistory.Count < 1 || this._run.MapPointHistory[0].Count < 5)
      return false;
    SerializablePlayer serializablePlayer = (SerializablePlayer) null;
    foreach (SerializablePlayer player in this._run.Players)
    {
      if (serializablePlayer == null || player.ExtraFields.DamageDealt > serializablePlayer.ExtraFields.DamageDealt)
        serializablePlayer = player;
    }
    ulong? netId1 = serializablePlayer?.NetId;
    ulong netId2 = this._localPlayer.NetId;
    return (long) netId1.GetValueOrDefault() == (long) netId2 & netId1.HasValue;
  }
}
