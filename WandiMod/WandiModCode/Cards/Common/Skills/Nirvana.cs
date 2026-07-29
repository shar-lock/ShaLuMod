using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd / PlayerCmd（获得能量）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 涅槃 / Nirvana（普通 · 技能）
/// 失去 2 层【血仇】，获得 2 点能量。升级：失去 1 层血仇（能量仍为 2）。
/// —— 续航（回费用）：用血仇换能量。血仇不足则无法转化（不打折）。
/// </summary>
public class Nirvana : WandiModCard
{
    public Nirvana() : base(
        cost: 0,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("BloodCost", 2).WithUpgradeTo(1),  // 需消耗的血仇层数 2→1
        new IntVar("Energy", 2),                     // 获得的能量（固定 2）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[涅槃] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        int cost = DynamicVars["BloodCost"].IntValue;
        int have = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        if (have < cost)
        {
            // 血仇不足：不转化、不耗层（设计上要求足额血仇）
            MainFile.Logger.Warn($"[涅槃] 血仇不足（{have}/{cost}），未获得能量");
            return;
        }

        // 消耗血仇 + 获得能量
        await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -cost, creature, null);
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
        MainFile.Logger.Info($"[涅槃] 消耗 {cost} 血仇，获得 {DynamicVars["Energy"].IntValue} 能量");
    }
}
