// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.DeprecatedPotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class DeprecatedPotion : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.None;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AnyEnemy;
}
