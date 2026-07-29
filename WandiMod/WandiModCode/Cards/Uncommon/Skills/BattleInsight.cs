using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using System.Linq;                                  // HittableEnemies.Count()
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 战场洞察 / Battle Insight（罕见 · 技能）
/// 每存在一个敌人，获得 1 层【血仇】。升级：每个敌人 2 层。
/// —— 群战启动件：敌人越多血仇越厚（ AoE 战、多精英战时一张拉开血仇差）。
///    设计口径：血仇加在万敌自己身上（血仇是失血计数器，不是敌方 debuff）。
/// </summary>
public class BattleInsight : WandiModCard
{
    public BattleInsight() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("VengeancePerEnemy", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null)
        {
            MainFile.Logger.Error("[战场洞察] OnPlay 时 Owner.Creature 或 CombatState 为空，效果未触发");
            return;
        }

        int perEnemy = DynamicVars["VengeancePerEnemy"].IntValue;
        int enemyCount = CombatState.HittableEnemies.Count();   // HittableEnemies 为 IEnumerable，走 Linq.Count()
        if (enemyCount == 0)
        {
            MainFile.Logger.Warn("[战场洞察] 没有可命中的敌人，血仇未触发");
            return;
        }

        // 总血仇 = 每敌层数 × 敌人数量（一次性授予，等效于逐敌 Grant 但更高效）
        int total = perEnemy * enemyCount;
        await VengeancePower.Grant(choiceContext, creature, total, this);
        MainFile.Logger.Info($"[战场洞察] {enemyCount} 个敌人 × {perEnemy} = 获得 {total} 血仇");
    }
}
