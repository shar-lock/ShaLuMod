using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（自伤）/ PowerCmd（力量）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // HpLossVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // StrengthPower（原生力量）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 嗜血 / Bloodthirst（普通 · 技能）
/// 失去 2 点生命，获得 1 层力量、2 层【血仇】。升级：2 力量。
/// —— 血仇·增益：自伤触发血仇引擎（自动 +1），卡面再授 2 层，叠加力量。
/// </summary>
public class Bloodthirst : WandiModCard
{
    public Bloodthirst() : base(
        cost: 0,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(2),
        new IntVar("Strength", 1).WithUpgrade(2),
        new IntVar("Vengeance", 2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[嗜血] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 自伤：Unblockable|Unpowered|Move → 全额计入失血 → 触发 VengeancePower.AfterDamageReceived 自动 +1 血仇
        await CreatureCmd.Damage(choiceContext, creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // ② 力量
        await PowerCmd.Apply<StrengthPower>(choiceContext, creature, DynamicVars["Strength"].IntValue, creature, null);
        // ③ 血仇（卡面 2 层；加上 ① 自伤自动 +1，实际共 +3）
        await VengeancePower.Grant(choiceContext, creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
