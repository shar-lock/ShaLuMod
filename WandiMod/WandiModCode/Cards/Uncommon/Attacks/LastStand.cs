using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions（CardAttack）
using MegaCrit.Sts2.Core.Commands;                  // CardCmd（消耗手牌）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 背水一战 / Last Stand（罕见 · 攻击）
/// 造成 8 伤害；消耗手牌中所有非攻击牌。升级：10 伤害。
/// —— 燃烧手牌换一击：把技能/诅咒/状态牌当燃料清掉，留攻击牌继续输出。
/// 注意：本牌自身是攻击牌，不会被自己的消耗效果命中；已结算中的本牌不在遍历集合内。
/// TODO: 手牌遍历 API（Owner.Player.Hand.Cards 字段名 / 集合类型）运行时确认。
/// </summary>
public class LastStand : WandiModCard
{
    public LastStand() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move).WithUpgrade(10),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ① 标准攻击（DamageVar 走 CommonActions.CardAttack，自动吃力量/血仇放大）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 消耗手牌中所有非攻击牌
        // TODO: 手牌 API 运行时确认——Owner.Player.Hand.Cards 字段名 / 集合类型待核对
        var hand = Owner.Player?.Hand?.Cards;
        if (hand == null)
        {
            MainFile.Logger.Warn("[背水一战] 取不到手牌（Owner.Player.Hand.Cards 为空），跳过消耗");
            return;
        }

        // 复制一份再遍历：CardCmd.Exhaust 会修改手牌集合，避免迭代中修改集合
        var toExhaust = hand.Where(c => c.Type != CardType.Attack).ToList();
        foreach (var c in toExhaust)
        {
            await CardCmd.Exhaust(choiceContext, c);
        }

        MainFile.Logger.Info($"[背水一战] 打出 {DynamicVars.Damage.BaseValue} 伤，消耗 {toExhaust.Count} 张非攻击牌");
    }
}
