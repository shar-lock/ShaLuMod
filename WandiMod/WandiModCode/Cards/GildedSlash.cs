using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / BlockVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard（基类，自带 [Pool] 自动入池）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 金焰斩 / Gilded Slash（普通 · 攻击）
/// 造成 9 点伤害，获得 3 点格挡。升级：12 伤害，4 格挡。
/// —— 万敌 Mod 的「最小可跑示例」，演示完整链路：定义变量 → OnPlay → 调用 CommonActions。
/// </summary>
public class GildedSlash : WandiModCard
{
    public GildedSlash() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    // ① 基础数值（真相源）。WithUpgrade 设定升级后的数值，逻辑代码无需改动。
    //    DamageVar/BlockVar 会被 CommonActions 自动读取。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9, ValueProp.Move).WithUpgrade(12),
        new BlockVar(3, ValueProp.Move).WithUpgrade(4),
    ];

    // ② 打出时触发。签名固定：(PlayerChoiceContext choiceContext, CardPlay cardPlay)。
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 造成伤害：自动读 DamageVar，按 TargetType=AnyEnemy 选定目标，附带攻击 VFX。
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // 获得格挡：自动读 BlockVar，加给卡牌拥有者（Owner）。
        await CommonActions.CardBlock(this, cardPlay);
    }
}
