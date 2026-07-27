// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.SweepingGaze
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public class SweepingGaze : CardModel
{
  public SweepingGaze()
    : base(0, CardType.Attack, CardRarity.Token, TargetType.RandomEnemy)
  {
  }

  protected override bool ShouldGlowRedInternal => this.Owner.IsOstyMissing;

  protected override HashSet<CardTag> CanonicalTags
  {
    get => new HashSet<CardTag>() { CardTag.OstyAttack };
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlyArray<CardKeyword>(new CardKeyword[2]
      {
        CardKeyword.Ethereal,
        CardKeyword.Exhaust
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new OstyDamageVar(10M, ValueProp.Move));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (Osty.CheckMissingWithAnim(this.Owner))
      return;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.OstyDamage.BaseValue).FromOsty(this.Owner.Osty, (CardModel) this, cardPlay).TargetingRandomOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.OstyDamage.UpgradeValueBy(5M);
}
