using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // SpiritOfKingPower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 王之意志 / Spirit of King（罕见 · 能力）
/// [荡平万邦]的伤害额外提高 20%。升级：25%，且获得固有（起手必摸到）。
/// —— 放大终结技（荡平万邦由血仇≥8 生成）。
/// 百分比随升级态传入 Power 的 Amount（20→25），由 SpiritOfKingPower.ModifyDamageMultiplicative 读取。
/// </summary>
public class SpiritOfKing : WandiModCard
{
    public SpiritOfKing() : base(
        cost: 1,
        type: CardType.Power,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("BonusPct", 20).WithUpgradeTo(25),  // 荡平万邦伤害额外提升百分比
    ];

    // 设计「无/固有」：基础无固有，升级后获得固有（起手必摸到）；荡平万邦词条始终保留（悬停可见其说明）
    public override IEnumerable<CardKeyword> CanonicalKeywords => IsUpgraded
        ? [CardKeyword.Innate, WandiModKeywords.ConquerAllLands]
        : [WandiModKeywords.ConquerAllLands];

    /// <summary>悬停提示：荡平万邦卡牌预览（描述提到荡平万邦，玩家可实时查看其效果）。</summary>
    protected override IEnumerable<MegaCrit.Sts2.Core.HoverTips.IHoverTip> ExtraHoverTips =>
        [MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromCard<ConquerAllLands>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[王之意志] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }
        // Amount = 伤害提升百分比（20/25），SpiritOfKingPower 据此返回 1 + Amount/100
        int pct = DynamicVars["BonusPct"].IntValue;
        await PowerCmd.Apply<SpiritOfKingPower>(choiceContext, Owner.Creature, pct, Owner.Creature, this);
        MainFile.Logger.Info($"[王之意志] 授予王之意志：荡平万邦伤害 +{pct}%");
    }
}
