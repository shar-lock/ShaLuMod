using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // StrengthPower（原生力量）
using WandiMod.WandiModCode.Powers;                 // VengeancePower（关键词）

namespace WandiMod.WandiModCode.Cards;

// 联机

/// <summary>
/// 同仇敌忾 / Shared Fury（罕见 · 攻击 · 联机）
/// 所有队友 +2 力量（本场）。升级：+3 力量。
/// —— 联机专属团队增益：给所有队友（含自己）叠力量，提升全队输出。
///
/// 实现说明���
///   - 队友枚举走 CombatState.GetTeammatesOf（Agent 确认的多人 API；Player 上无 Allies 属性）。
///   - 含自己：GetTeammatesOf 返回同侧所有生物，过滤 IsAlive &amp;&amp; IsPlayer 后含自己（与原生 Rally 一致）。
///   - 设计稿列在攻击表但未写伤害，按设计实现为纯增益（不造成伤害）。
///   - 联机专属：MultiplayerConstraint = MultiplayerOnly（仅联机掉落/可用，参考原生 Rally/OneForAll）。
/// </summary>
public class SharedFury : WandiModCard
{
    public SharedFury() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    // 联机专属
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strength", 2).WithUpgrade(3),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null)
        {
            MainFile.Logger.Error("[同仇敌忾] OnPlay 时 Owner.Creature 或 CombatState 为空，效果未触发");
            return;
        }

        int str = DynamicVars["Strength"].IntValue;
        // 枚举所有队友（同侧、存活、玩家身份；含自己）——参考原生 Rally / OneForAll 的多人迭代写法
        var teammates = CombatState.GetTeammatesOf(creature)
            .Where(c => c != null && c.IsAlive && c.IsPlayer)
            .ToList();

        foreach (var ally in teammates)
            await PowerCmd.Apply<StrengthPower>(choiceContext, ally, str, creature, this);

        MainFile.Logger.Info($"[同仇敌忾] 给 {teammates.Count} 名队友（含自己）各 +{str} 力量");
    }
}
