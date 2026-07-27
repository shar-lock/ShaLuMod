// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.DoubleSnecko
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class DoubleSnecko(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "DOUBLE_SNECKO", false, false)
{
  public override BadgeRarity Rarity => BadgeRarity.Bronze;

  public override bool IsObtained()
  {
    return this._localPlayer.Relics.Any<SerializableRelic>((Func<SerializableRelic, bool>) (r => r.Id == ModelDb.Relic<SneckoEye>().Id)) && this._localPlayer.Relics.Any<SerializableRelic>((Func<SerializableRelic, bool>) (r => r.Id == ModelDb.Relic<FakeSneckoEye>().Id));
  }
}
