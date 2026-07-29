using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower / VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 蓄势 / Stance（普通 · 技能）
/// 获得 4 点【纷争】和 1 层【血仇】。升级：6 纷争。
/// —— 0 费双资源启动件：纷争扛伤、血仇备爆。
/// </summary>
public class Stance : WandiModCard
{
    public Stance() : base(
        cost: 0,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 4).WithUpgrade(6),
        new IntVar("Vengeance", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Owner.Creature 战斗外 / 异常时序下可能为 null——StrifePower.Grant 与 VengeancePower.Grant
        // 内部均有空值守卫并记错误日志，这里直接透传即可
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
