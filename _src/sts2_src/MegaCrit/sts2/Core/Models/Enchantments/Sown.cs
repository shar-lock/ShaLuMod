// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Sown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Sown : EnchantmentModel
{
  public override bool HasExtraCardText => true;

  public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
  {
    if (this.Status != EnchantmentStatus.Normal)
      return;
    this.Status = EnchantmentStatus.Disabled;
    await PlayerCmd.GainEnergy((Decimal) this.Amount, this.Card.Owner);
  }
}
