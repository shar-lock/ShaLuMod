using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CardPileCmd（抽牌）/ PlayerCmd（获得能量）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 战意凝聚 / Focus Surge（罕见 · 技能 · 消耗）
/// 获得 1 点能量，抽 1 张牌。升级：2 点能量（抽牌不变）。
/// —— 0 费过牌 + 费用补贴（参考原版 Focus / Adrenaline 的费用模型）。消耗防无限循环。
/// </summary>
public class FocusSurge : WandiModCard
{
    public FocusSurge() : base(
        cost: 0,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Energy", 1).WithUpgradeTo(2),
        new IntVar("Draw", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 获得能量（Owner 是 Player 不是 Creature）
        await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
        // 抽牌
        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);

        MainFile.Logger.Info($"[战意凝聚] 获得 {DynamicVars["Energy"].IntValue} 能量，抽 {DynamicVars["Draw"].IntValue} 牌");
    }
}
