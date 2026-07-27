// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.CrushUnder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class CrushUnder : CardModel
{
  private const string _strengthLossKey = "StrengthLoss";

  public CrushUnder()
    : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(7M, ValueProp.Move),
        new DynamicVar("StrengthLoss", 1M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.AttackAnimDelay);
    IReadOnlyList<Creature> enemies = this.CombatState.HittableEnemies;
    foreach (Creature target in (IEnumerable<Creature>) enemies)
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NSpikeSplashVfx.Create(target));
    }
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).TargetingAllOpponents(this.CombatState).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "blunt_attack.mp3").WithHitVfxSpawnedAtBase().Execute(choiceContext);
    IReadOnlyList<CrushUnderPower> crushUnderPowerList = await PowerCmd.Apply<CrushUnderPower>(choiceContext, (IEnumerable<Creature>) enemies, this.DynamicVars["StrengthLoss"].BaseValue, this.Owner.Creature, (CardModel) this);
    enemies = (IReadOnlyList<Creature>) null;
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(1M);
    this.DynamicVars["StrengthLoss"].UpgradeValueBy(1M);
  }
}
