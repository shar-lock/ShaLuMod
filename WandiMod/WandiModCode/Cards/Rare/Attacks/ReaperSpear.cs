using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 绝命枪 / Reaper Spear（稀有 · 攻击）。8伤；HP≤50% 额外+7 / 12+8。
/// 设计稿是「HP≤50%费用变0」——StS2 无单卡动态降费 API（TryModifyEnergyCostInCombatLate 需持久 Power，
/// 且作用于所有攻击牌而非指定卡）。接受简化：固定 1 费 + 残血加伤。
/// </summary>
public class ReaperSpear : WandiModCard
{
    public ReaperSpear() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10, ValueProp.Move).WithUpgradeTo(12),
        new IntVar("Bonus", 7).WithUpgradeTo(8),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        decimal dmg = DynamicVars.Damage.BaseValue;
        bool lowHp = c.CurrentHp <= c.MaxHp * 0.5m;
        if (lowHp) dmg += DynamicVars["Bonus"].IntValue;
        // AttackCommand.DamageProps 默认即 ValueProp.Move，无需（也无 API）再设置
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
        if (lowHp) MainFile.Logger.Info($"[绝命枪] HP≤50%，总伤 {dmg}");
    }
}
