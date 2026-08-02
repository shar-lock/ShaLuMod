using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 狂化 / Frenzy（稀有 · 能力）。每当你获得血仇，抽 1 张牌。升级：固有（开局在手）。
/// —— 升级固有必须走 OnUpgrade → AddKeyword（原版 MachineLearning / InfiniteBlades 范式）。
/// CanonicalKeywords 里写 IsUpgraded ? Innate 无效：LocalKeywords 首次访问时缓存 CanonicalKeywords，
/// 升级后不会重读该属性，卡面/预览会显示「升级无效果」。
/// </summary>
public class Frenzy : WandiModCard
{
    public Frenzy() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

    protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        // amount=1：PowerCmd.Apply 需非 0 才附着（形态型 Power 传 1，对齐 Barricade）；FrenzyPower 不用 Amount 做上限
        => await PowerCmd.Apply<FrenzyPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
}
