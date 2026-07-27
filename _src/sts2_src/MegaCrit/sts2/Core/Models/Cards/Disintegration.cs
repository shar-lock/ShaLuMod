// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Disintegration
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Disintegration : CardModel, KnowledgeDemon.IChoosable
{
  public Disintegration()
    : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  public override bool CanBeGeneratedInCombat => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<DisintegrationPower>(6M));
    }
  }

  public async Task OnChosen()
  {
    DisintegrationPower disintegrationPower = await PowerCmd.Apply<DisintegrationPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars["DisintegrationPower"].BaseValue, this.Owner.Creature, (CardModel) this);
  }
}
