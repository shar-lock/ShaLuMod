using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>浴血奋战 / Bloodbath（稀有 · 能力）。若荡平万邦只攻击到一个敌人则伤害×1.5/×2。</summary>
public class Bloodbath : WandiModCard
{
    public Bloodbath() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    // 提升百分比（50=伤害×1.5，100=伤害×2）：卡面 {Multiplier:diff()}% 直接显示 50%/100%
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Multiplier", 50).WithUpgradeTo(100)];
    // 荡平万邦词条（悬停卡牌可见荡平万邦词条说明）
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.ConquerAllLands];
    /// <summary>悬停提示：荡平万邦卡牌预览（描述提到荡平万邦，玩家可实时查看其效果）。</summary>
    protected override IEnumerable<MegaCrit.Sts2.Core.HoverTips.IHoverTip> ExtraHoverTips =>
        [MegaCrit.Sts2.Core.HoverTips.HoverTipFactory.FromCard<ConquerAllLands>()];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<BloodbathPower>(choiceContext, Owner.Creature, DynamicVars["Multiplier"].IntValue, Owner.Creature, this);
}
