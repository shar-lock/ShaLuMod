// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Players.ExtraPlayerFields
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Players;

public class ExtraPlayerFields
{
  public int CardShopRemovalsUsed { get; set; }

  public int WongoPoints { get; set; }

  public bool CccomboBadgeUnlocked { get; set; }

  public int DamageDealt { get; set; }

  public int DebuffsApplied { get; set; }

  public SerializableExtraPlayerFields ToSerializable()
  {
    return new SerializableExtraPlayerFields()
    {
      CardShopRemovalsUsed = this.CardShopRemovalsUsed,
      WongoPoints = this.WongoPoints,
      CccomboBadgeUnlocked = this.CccomboBadgeUnlocked,
      DamageDealt = this.DamageDealt,
      DebuffsApplied = this.DebuffsApplied
    };
  }

  public static ExtraPlayerFields FromSerializable(SerializableExtraPlayerFields save)
  {
    return new ExtraPlayerFields()
    {
      CardShopRemovalsUsed = save.CardShopRemovalsUsed,
      WongoPoints = save.WongoPoints,
      CccomboBadgeUnlocked = save.CccomboBadgeUnlocked,
      DamageDealt = save.DamageDealt,
      DebuffsApplied = save.DebuffsApplied
    };
  }
}
