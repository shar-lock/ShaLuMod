using BaseLib.Abstracts;                        // ITomeCard
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 弑神登神 / Godslayer Ascension（先古 · 能力 · 3/2费）。
/// 经先古之民「达弗」的尘封魔典随机给予。实现 ITomeCard 接口。
/// 回合开始+2血仇；攻击牌额外造成等同于血仇层数的伤害（不消耗）；攻击造成伤害回复 2% 最大生命。
/// </summary>
public class GodslayerAscension : WandiModCard, ITomeCard
{
    public GodslayerAscension() : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self) { }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        // amount=1：PowerCmd.Apply 在 amount==0 时直接 bail 不附着（PowerCmd.cs:84），标记/形态型 Power 须传 1（对齐原生 Barricade）
        => await PowerCmd.Apply<GodslayerAscensionPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
}
