using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 金焰斩 / Gilded Slash（普通 · 攻击）
/// 造成 14 点伤害，回复 3 点生命值。升级：18 伤害（回血不变）。
/// —— 攻防一体：高伤攻击顺手回血。
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

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12, ValueProp.Move).WithUpgradeTo(15),
        new IntVar("Heal", 3),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[金焰斩] OnPlay 时 Owner.Creature 为空（不在战斗中？），回血未生效");
            return;
        }
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["Heal"].IntValue);
    }
}
