// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.Honed
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class Honed(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "HONED", true, false)
{
  private const int _sameCardAmount = 5;

  public override BadgeRarity Rarity => BadgeRarity.Bronze;

  public override bool IsObtained()
  {
    return this._localPlayer.Deck.Where<SerializableCard>((Func<SerializableCard, bool>) (c => SaveUtil.CardOrDeprecated(c.Id).Rarity != CardRarity.Basic)).ToList<SerializableCard>().GroupBy<SerializableCard, ModelId>((Func<SerializableCard, ModelId>) (c => c.Id)).Any<IGrouping<ModelId, SerializableCard>>((Func<IGrouping<ModelId, SerializableCard>, bool>) (g => g.Count<SerializableCard>() >= 5));
  }
}
