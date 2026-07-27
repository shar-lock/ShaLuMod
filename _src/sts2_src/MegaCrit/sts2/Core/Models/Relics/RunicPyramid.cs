// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RunicPyramid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RunicPyramid : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShouldFlush(Player player) => player != this.Owner;
}
