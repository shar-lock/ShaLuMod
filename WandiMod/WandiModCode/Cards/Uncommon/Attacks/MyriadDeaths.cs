using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 万死无悔 / Myriad Deaths（罕见 · 攻击 · 全体 · 消耗）⭐ 灵魂卡
/// 对所有敌人造成「已损失生命」40% 的伤害。升级：50%。
/// —— 致敬星穹铁道 Mydei（万敌）战技：残血越深，爆发越猛的全体 AoE 收尾。
/// MissingHp 缩放参考 Token 卡「荡平万邦」的逐敌手动 DamageCmd 写法；
/// 由于全体伤害对每个敌人相同，逐敌打以触发各自受击钩子（与荡平万邦一致）。
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
        new IntVar("MissingHpPct", 40).WithUpgradeTo(50),  // 已损失生命百分比 40→50（运算时 /100）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[万死无悔] OnPlay 时 Owner.Creature 为空（不在战斗中？），效果未触发");
            return;
        }
        if (CombatState == null)
        {
            MainFile.Logger.Error("[万死无悔] CombatState 为空，无法取敌方列表，效果未触发");
            return;
        }

        decimal pct = DynamicVars["MissingHpPct"].IntValue / 100m;    // 0.40 / 0.50
        decimal missingHp = creature.MaxHp - creature.CurrentHp;      // 已损失生命
        decimal dmg = missingHp * pct;

        var enemies = CombatState.HittableEnemies.ToList();
        if (enemies.Count == 0)
        {
            MainFile.Logger.Warn("[万死无悔] 没有可命中的敌人，伤害落空");
            return;
        }

        // 逐敌结算（与荡平万邦一致）：每个敌人单独打一发，触发各自受击钩子
        foreach (var enemy in enemies)
        {
            await DamageCmd.Attack(dmg)
                .FromCard(this, cardPlay)
                .Targeting(enemy)
                .WithValueProp(ValueProp.Move)
                .Execute(choiceContext);
        }

        MainFile.Logger.Info($"[万死无悔] 已损失生命 {missingHp} × {pct} = {dmg}，命中 {enemies.Count} 个敌人");
    }
}
