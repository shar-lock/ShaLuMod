using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // GuardianPactPower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

// 联机

/// <summary>
/// 庇护盟约 / Guardian Pact（罕见 · 技能 · 联机）
/// 本回合队友受到的伤害减半，你多承受 50% 的伤害。升级：队友减伤升至 75%。
/// —— 联机专属坦克件：替队友分担火力（简化为乘法减伤 + 自身增伤）。
///
/// 实现说明：
///   - 队友减伤百分比（50/75）随升级态传入 GuardianPactPower.Amount；
///     自身 +50% 惩罚固定（见 GuardianPactPower）。
///   - 详见 GuardianPactPower 的简化策略与 TODO（分数转移 / 回合移除时序待运行时确认）。
/// </summary>
public class GuardianPact : WandiModCard
{
    public GuardianPact() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    // 联机专属
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("AllyReduce", 50).WithUpgradeTo(75),  // 队友减伤百分比
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[庇护盟约] OnPlay 时 Owner.Creature 为空，效果未授予");
            return;
        }
        // 授予庇护 Power（Amount=50/75；自身惩罚固定 +50% 见 GuardianPactPower）
        int reduce = DynamicVars["AllyReduce"].IntValue;
        await PowerCmd.Apply<GuardianPactPower>(choiceContext, Owner.Creature, reduce, Owner.Creature, this);
        MainFile.Logger.Info($"[庇护盟约] 授予庇护：队友 -{reduce}% 伤害，自身 +50% 伤害（本回合）");
    }
}
