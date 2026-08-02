using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardTag
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 打击 / Strike（起手 · 攻击 · Basic）
/// 造成 6 点伤害。升级：9 伤害。
/// —— 万敌专属 Strike。CanonicalTags 含 CardTag.Strike，供潘多拉魔盒等 IsBasicStrikeOrDefend 识别替换。
/// </summary>
public class Strike : WandiModCard
{
    public Strike() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Basic,
        target: TargetType.AnyEnemy)
    {
    }

    // 原版各角色 Strike 均挂 Strike 标签；潘多拉魔盒过滤 IsBasicStrikeOrDefend 依赖此标签
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move).WithUpgradeTo(9),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
}
