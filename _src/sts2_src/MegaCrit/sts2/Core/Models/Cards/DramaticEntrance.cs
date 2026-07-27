// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.DramaticEntrance
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class DramaticEntrance : CardModel
{
  public const string vfxPath = "vfx/vfx_dramatic_entrance_fullscreen";

  public DramaticEntrance()
    : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<string> ExtraRunAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(SceneHelper.GetScenePath("vfx/vfx_dramatic_entrance_fullscreen"));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(11M, ValueProp.Move));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlyArray<CardKeyword>(new CardKeyword[2]
      {
        CardKeyword.Exhaust,
        CardKeyword.Innate
      });
    }
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(4M);

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Attack", this.Owner.Character.AttackAnimDelay);
    VfxCmd.PlayFullScreenInCombat("vfx/vfx_dramatic_entrance_fullscreen", this.Owner.Creature);
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
  }
}
