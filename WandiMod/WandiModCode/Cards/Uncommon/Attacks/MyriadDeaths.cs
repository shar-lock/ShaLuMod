using BaseLib.Extensions;                           // WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // CalculatedDamageVar 等
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 万死无悔 / Myriad Deaths（罕见 · 攻击 · 全体 · 消耗）⭐ 灵魂卡
/// 对所有敌人造成「已损失生命」40%/50% 的伤害。
/// 卡面文案用 MissingHpPct；战斗中 InCombat 行 + OnPlay 走 CalculatedDamageVar。
/// </summary>
public class MyriadDeaths : WandiModCard
{
    public MyriadDeaths() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("MissingHpPct", 40).WithUpgradeTo(50),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(40m).WithUpgradeTo(50),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
        {
            if (card.Owner.Creature == null) return 0m;
            return (card.Owner.Creature.MaxHp - card.Owner.Creature.CurrentHp) / 100m;
        }),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null)
        {
            MainFile.Logger.Error("[万死无悔] CombatState 为空，无法取敌方列表，效果未触发");
            return;
        }

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        MainFile.Logger.Info($"[万死无悔] 全体 {DynamicVars.CalculatedDamage.Calculate(null)} 伤");
    }
}
