using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 金色裁决 / Golden Judgment（稀有 · 攻击）。
/// 造成 10 伤害 + 敌人生命上限 10%；本场战斗每打出一次，敌人生命上限占比 +5%。升级：15%。
/// —— 玩家 MaxHP 流的终结件：敌人血越多越痛，且越打越痛（每打出一次百分比 +5%）。
/// 计数用战斗实例字段（新战新实例→自动重置；存档重载会重置计数，可接受）。
/// </summary>
public class GoldenJudgment : WandiModCard
{
    public GoldenJudgment() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

    // 战斗内打出次数（每次 OnPlay 自增；新战斗创建新卡实例→自动归零）
    private int _timesPlayed = 0;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("FlatDamage", 10),                       // 固定 10 伤（升级不变）
        new IntVar("EnemyMaxHpPct", 10).WithUpgradeTo(15),  // 敌人生命上限占比 10→15（每打出一次再 +5%）
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        _timesPlayed++;
        int flat = DynamicVars["FlatDamage"].IntValue;
        int basePct = DynamicVars["EnemyMaxHpPct"].IntValue;
        int bonusPct = (_timesPlayed - 1) * 5;  // 每打出一次 +5%（首次 +0）
        decimal enemyMaxHp = cardPlay.Target.MaxHp;
        decimal dmg = flat + enemyMaxHp * (basePct + bonusPct) / 100m;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[金色裁决] 第 {_timesPlayed} 次打出：{flat} + 敌MaxHp {enemyMaxHp}×{basePct + bonusPct}% = {dmg} 伤");
    }
}
