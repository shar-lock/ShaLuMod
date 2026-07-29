using BaseLib.Extensions;                           // WithUpgrade / WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd（手动伤害，便于条件加成）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // VulnerablePower（读取目标易伤）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 穿心枪 / Heartpiercer（罕见 · 攻击）
/// 造成 9 点伤害；若目标有易伤，额外 +5 伤害。升级：12 伤害，+6。
/// —— 易伤联动件：手动算总伤（基础 + 易伤奖励），用 DamageCmd 单次结算。
///    判定口径同原版 Dominate/Dismantle：cardPlay.Target.GetPower&lt;VulnerablePower&gt;() != null。
///    与 VajraBreak（先攻后挂易伤）互补：本卡奖励「已挂易伤」的目标。
/// </summary>
public class Heartpiercer : WandiModCard
{
    public Heartpiercer() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9, ValueProp.Move).WithUpgrade(12),
        new IntVar("Bonus", 5).WithUpgrade(6),      // 目标有易伤时的额外伤害
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[穿心枪] OnPlay 时目标为空，效果未触发");
            return;
        }

        decimal dmg = DynamicVars.Damage.BaseValue;
        // 目标已有易伤 → 加额外伤害
        bool hasVuln = cardPlay.Target.GetPower<VulnerablePower>() != null;
        if (hasVuln)
            dmg += DynamicVars["Bonus"].IntValue;

        await DamageCmd.Attack(dmg)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        if (hasVuln)
            MainFile.Logger.Info($"[穿心枪] 目标已有易伤，触发奖励，总伤 {dmg}");
    }
}
