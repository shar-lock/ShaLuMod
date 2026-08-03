using BaseLib.Extensions;                           // WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 命途斩 / Lifepath Slash（罕见 · 攻击）
/// 造成「当前最大生命」20%/25% 的伤害。卡面文案用 MaxHpPct；实战预览/结算走 CalculatedDamageVar。
/// </summary>
public class LifepathSlash : WandiModCard
{
    public LifepathSlash() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("MaxHpPct", 20).WithUpgradeTo(25),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(20m).WithUpgradeTo(25),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
            card.Owner.Creature == null ? 0m : card.Owner.Creature.MaxHp / 100m),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[命途斩] MaxHp={Owner.Creature.MaxHp} → {DynamicVars.CalculatedDamage.Calculate(null)} 伤");
    }
}
