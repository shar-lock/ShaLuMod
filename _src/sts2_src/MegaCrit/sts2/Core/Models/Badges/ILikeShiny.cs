// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.ILikeShiny
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class ILikeShiny(SerializableRun run, bool won, ulong playerId) : Badge(run, won, playerId, "ILIKESHINY", false, false)
{
  private const int _relicRequirement = 25;

  public override BadgeRarity Rarity => BadgeRarity.Bronze;

  public override bool IsObtained() => this._localPlayer.Relics.Count >= 25;
}
