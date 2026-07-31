using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）/ PowerCmd（消耗血仇）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 守誓 / Oathguard（普通 · 技能）
/// 回复 4 点生命值；消耗 1 层【血仇】额外回复 3 生命（无血仇则不加）。升级：6 生命值。
/// —— 血仇·回血联动：基础回血 + 用血仇换额外回血。没血仇时只给基础，不白送。
/// —— 保底1层：血仇真实层数>=2 才允许消耗（Amount==1 是地板层，耗掉会把血仇 Power 移除）。
/// </summary>
public class Oathguard : WandiModCard
{
    public Oathguard() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Heal", 4).WithUpgradeTo(6),
        new IntVar("BloodBonus", 3),   // 消耗 1 血仇换的额外回血（固定，升级不变）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[守誓] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 基础回血
        await CreatureCmd.Heal(creature, DynamicVars["Heal"].IntValue);

        // ② 消耗 1 血仇 → 额外回血（无血仇则跳过；保底1层：Amount>=2 才允许消耗，消耗后剩余≥1，血仇 Power 不会被移除）
        var vengeance = creature.GetPower<VengeancePower>();
        if (vengeance != null && vengeance.Amount >= 2)
        {
            await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -1, creature, null);
            await CreatureCmd.Heal(creature, DynamicVars["BloodBonus"].IntValue);
            MainFile.Logger.Info("[守誓] 消耗 1 血仇，额外回血");
        }
    }
}
