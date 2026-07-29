using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions（CardAttack）
using MegaCrit.Sts2.Core.Commands;                  // CardCmd（消耗）/ CardPileCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / PileType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 背水一战 / Last Stand（罕见 · 攻击）
/// 造成 8 伤害；消耗手牌中所有非攻击牌。升级：10 伤害。
/// —— 燃烧手牌换一击：把技能/诅咒/状态牌当燃料清掉，留攻击牌继续输出。
/// 手牌遍历走 PileType.Hand.GetPile(Owner).Cards（参考原生 BulletTime / Armaments）。
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
        new DamageVar(8, ValueProp.Move).WithUpgradeTo(10),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ① 标准攻击（DamageVar 走 CommonActions.CardAttack，自动吃力量/血仇放大）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 消耗手牌中所有非攻击牌——PileType.Hand.GetPile(Owner).Cards（参考原生 BulletTime/Armaments）
        var toExhaust = PileType.Hand.GetPile(Owner).Cards
            .Where(c => c.Type != CardType.Attack)
            .ToList();

        foreach (var c in toExhaust)
            await CardCmd.Exhaust(choiceContext, c);

        MainFile.Logger.Info($"[背水一战] 打出 {DynamicVars.Damage.BaseValue} 伤，消耗 {toExhaust.Count} 张非攻击牌");
    }
}
