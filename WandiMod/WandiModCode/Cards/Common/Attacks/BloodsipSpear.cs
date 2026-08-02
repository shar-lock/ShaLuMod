using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 饮血枪 / Bloodsip Spear（普通 · 攻击）
/// 造成 7 点伤害，回复 2 生命。升级：10 伤害，回复 4。
/// —— 攻击+续航：打人同时直接回血（无条件）。
/// </summary>
public class BloodsipSpear : WandiModCard
{
    public BloodsipSpear() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(7, ValueProp.Move).WithUpgradeTo(10),
        new IntVar("Heal", 2).WithUpgradeTo(4),
    ];

    // 已无血仇条件，不挂血仇关键词
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        // ① 造成伤害
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        // ② 直接回血（无条件）
        if (creature != null)
        {
            int heal = DynamicVars["Heal"].IntValue;
            await CreatureCmd.Heal(creature, heal);
            MainFile.Logger.Info($"[饮血枪] 造成伤害，回复 {heal} 生命");
        }
    }
}
