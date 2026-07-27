// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.NemesisPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class NemesisPower : PowerModel
{
  private bool _shouldApplyIntangible;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    this._shouldApplyIntangible = !this._shouldApplyIntangible;
    if (this._shouldApplyIntangible)
    {
      this.Flash();
      IntangiblePower intangiblePower = await PowerCmd.Apply<IntangiblePower>(choiceContext, this.Owner, 1M, this.Owner, (CardModel) null);
    }
    else
    {
      if (!this.Owner.HasPower<IntangiblePower>())
        return;
      await PowerCmd.Remove((PowerModel) this.Owner.GetPower<IntangiblePower>());
    }
  }
}
