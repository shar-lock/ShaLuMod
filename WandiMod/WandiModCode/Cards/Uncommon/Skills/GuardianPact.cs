using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // GuardianPactPower

namespace WandiMod.WandiModCode.Cards;

// 联机

/// <summary>
/// 庇护盟约 / Guardian Pact（罕见 · 技能 · 联机）
/// 本回合队友受到的伤害减半，你多承受 50% 的伤害。升级：自身增伤降至 45%。
/// —— 联机专属坦克件：替队友分担火力（队友固定减半，自身承受更多）。
///
/// 实现说明：
///   - 自身增伤百分比（50/45）随升级态传入 GuardianPactPower.Amount；
///     队友减伤固定 50%（减半，见 GuardianPactPower）。
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
        new IntVar("SelfPenalty", 50).WithUpgradeTo(45),  // 自身增伤百分比 50→45（升级降低惩罚）
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[庇护盟约] OnPlay 时 Owner.Creature 为空，效果未授予");
            return;
        }
        // 授予庇护 Power（Amount=自身增伤百分比 50/45；队友固定减半见 GuardianPactPower）
        int penalty = DynamicVars["SelfPenalty"].IntValue;
        await PowerCmd.Apply<GuardianPactPower>(choiceContext, Owner.Creature, penalty, Owner.Creature, this);
        MainFile.Logger.Info($"[庇护盟约] 授予庇护：队友受伤减半，自身 +{penalty}% 伤害（本回合）");
    }
}
