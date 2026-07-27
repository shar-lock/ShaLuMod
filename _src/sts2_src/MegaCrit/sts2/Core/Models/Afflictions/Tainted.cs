// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Afflictions.Tainted
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Afflictions;

public sealed class Tainted : AfflictionModel
{
  public override bool IsStackable => true;

  public override bool HasExtraCardText => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromPowerWithPowerHoverTips<TaintedPower>(new int?(this.Amount));
  }

  public override bool CanAfflictCardType(CardType cardType) => cardType == CardType.Skill;
}
