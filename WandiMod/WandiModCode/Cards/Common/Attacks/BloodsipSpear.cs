using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 饮血枪 / Bloodsip Spear（普通 · 攻击）
/// 造成 7 伤害；若本战斗失去过生命（血仇 > 0）则回复 2 生命。升级：10 伤害，回 3。
/// —— 血仇·续航：用「血仇>0」作为「失过血」的判定口径（血仇是失血计数器，简洁且贴主题）。
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
        new DamageVar(7, ValueProp.Move).WithUpgrade(10),
        new IntVar("Heal", 2).WithUpgrade(3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ① 造成伤害（读 DamageVar）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 血仇 > 0 = 本战斗失过血 → 回血
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[饮血枪] OnPlay 时 Owner.Creature 为空，回血未触发");
            return;
        }
        if (creature.GetPower<VengeancePower>() is { Amount: > 0 })
        {
            int heal = DynamicVars["Heal"].IntValue;
            await CreatureCmd.Heal(creature, heal);
            MainFile.Logger.Info($"[饮血枪] 血仇>0，回复 {heal} 生命");
        }
    }
}
