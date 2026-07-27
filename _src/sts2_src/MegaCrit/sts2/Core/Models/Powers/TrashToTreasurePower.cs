// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.TrashToTreasurePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class TrashToTreasurePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterCardGeneratedForCombat(CardModel card, Player? creator)
  {
    if (card.Type != CardType.Status || creator == null || creator.Creature != this.Owner)
      return;
    this.Flash();
    for (int i = 0; i < this.Amount; ++i)
      await OrbCmd.Channel((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), OrbModel.GetRandomOrb(this.Owner.Player.RunState.Rng.CombatOrbGeneration).ToMutable(), this.Owner.Player);
  }
}
