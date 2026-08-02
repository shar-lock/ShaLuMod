using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // SpiritOfKingPower
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 王之意志 / Spirit of King（罕见 · 能力）
/// [荡平万邦]的伤害额外提高 20%。升级：25%，且获得固有（起手必摸到）。
/// —— 升级固有走 OnUpgrade → AddKeyword（LocalKeywords 缓存，不可用 IsUpgraded 切换 CanonicalKeywords）。
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

    // 荡平万邦词条始终保留；固有在 OnUpgrade 追加
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.ConquerAllLands];

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);

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
        int pct = DynamicVars["BonusPct"].IntValue;
        await PowerCmd.Apply<SpiritOfKingPower>(choiceContext, Owner.Creature, pct, Owner.Creature, this);
        MainFile.Logger.Info($"[王之意志] 授予王之意志：荡平万邦伤害 +{pct}%");
    }
}
