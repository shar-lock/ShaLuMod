using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（自伤走 Damage + Unblockable|Unpowered）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // HpLossVar / DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血祭 / Blood Rite（普通 · 攻击）
/// 失去 3 点生命，造成 12 点伤害，获得 1 层【血仇】。升级：16 伤害。
/// —— 自伤在前、攻击在后（参考原版 Hemokinesis）：自伤经 VengeancePower.AfterDamageReceived
///    自动 +1 血仇，新层数能放大本张牌的伤害；卡面「获得 1 血仇」是额外的手动授予。
/// </summary>
public class BloodRite : WandiModCard
{
    public BloodRite() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(3),
        new DamageVar(12, ValueProp.Move).WithUpgradeTo(16),
        new IntVar("Vengeance", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ① 自伤（原版 Hemokinesis 写法）：Unblockable → 全额计入 UnblockedDamage → 触发血仇叠层钩子
        await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // ② 攻击（能吃到 ① 新产的那层血仇的 +2% 放大）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        // ③ 卡面效果：额外获得 1 层血仇
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
