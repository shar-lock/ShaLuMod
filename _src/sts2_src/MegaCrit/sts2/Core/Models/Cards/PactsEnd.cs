// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.PactsEnd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class PactsEnd : CardModel
{
  public PactsEnd()
    : base(0, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(17M, ValueProp.Move),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  protected override bool ShouldGlowGoldInternal => this.CanDealDamage;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!this.CanDealDamage)
      return;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithAttackerAnim(Ironclad.GetHeavyAnimIfApplicable(this.Owner.Character), Ironclad.GetHeavyAttackDelayIfApplicable(this.Owner.Character)).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").WithHitVfxSpawnedAtBase().Execute(choiceContext);
  }

  private bool CanDealDamage
  {
    get
    {
      return CardPile.GetCards(this.Owner, PileType.Exhaust).Count<CardModel>() >= this.DynamicVars.Cards.IntValue;
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(6M);
}
