using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 蓄血 / Blood Charge（罕见 · 技能）
/// 获得 2 层【血仇】。升级：3 层。
/// —— 0 费纯血仇启动件：单卡把血仇推到 2，立刻放大后续自伤牌的收益（每层 +2% 失血放大）。
/// </summary>
public class BloodCharge : WandiModCard
{
    public BloodCharge() : base(
        cost: 0,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Vengeance", 2).WithUpgradeTo(3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Owner.Creature 战斗外 / 异常时序下可能为 null——VengeancePower.Grant 内部有空值守卫并记错误日志，直接透传
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
