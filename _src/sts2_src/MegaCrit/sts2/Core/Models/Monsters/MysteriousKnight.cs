// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.MysteriousKnight
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public class MysteriousKnight : FlailKnight
{
  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 6M, this.Creature, (CardModel) null);
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 6M, this.Creature, (CardModel) null);
  }
}
