using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（自伤）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // HpLossVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Powers;                 // StrifePower / VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 鲜血护盾 / Blood Aegis（罕见 · 技能）
/// 失去 4 点生命，获得 10 点【纷争】和 1 层【血仇】。升级：15 纷争（失血与血仇不变）。
/// —— 自伤双资源件：一次自伤同时喂大额纷争（临时上限）+ 血仇引擎。
///    自伤经 VengeancePower.AfterDamageReceived 自动 +1 血仇，卡面再授 1 层，实际共 +2。
/// </summary>
public class BloodAegis : WandiModCard
{
    public BloodAegis() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(4),
        new IntVar("Strife", 10).WithUpgrade(15),
        new IntVar("Vengeance", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[鲜血护盾] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 自伤：Unblockable|Unpowered|Move → 全额计入失血 → 触发血仇自动 +1
        await CreatureCmd.Damage(choiceContext, creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // ② 纷争（临时上限）
        await StrifePower.Grant(choiceContext, creature, DynamicVars["Strife"].IntValue, creature, this);
        // ③ 卡面血仇（加上 ① 自伤自动 +1，实际共 +2）
        await VengeancePower.Grant(choiceContext, creature, DynamicVars["Vengeance"].IntValue, this);

        MainFile.Logger.Info($"[鲜血护盾] 失 {DynamicVars.HpLoss.BaseValue} 血，获得 {DynamicVars["Strife"].IntValue} 纷争 + {DynamicVars["Vengeance"].IntValue} 血仇");
    }
}
