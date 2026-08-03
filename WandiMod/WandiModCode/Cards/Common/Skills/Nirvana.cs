using MegaCrit.Sts2.Core.Commands;                  // PowerCmd / PlayerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 涅槃 / Nirvana（普通 · 技能）
/// 失去 2 层【血仇】，获得 3 点能量。升级：获得 4 点能量。
/// —— 续航（回费用）：用血仇换大量能量。BloodCost 门控：血仇≤1 不可打出。
/// </summary>
public class Nirvana : WandiModCard
{
    public Nirvana() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("BloodCost", 2),                     // 固定消耗 2 血仇（升级不变）
        new IntVar("Energy", 3).WithUpgradeTo(4),       // 获得能量 3→4
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null) { MainFile.Logger.Error("[涅槃] Owner.Creature 为空"); return; }
        int cost = DynamicVars["BloodCost"].IntValue;
        await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -cost, creature, null);
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
        MainFile.Logger.Info($"[涅槃] 消耗 {cost} 血仇，获得 {DynamicVars["Energy"].IntValue} 能量");
    }
}
