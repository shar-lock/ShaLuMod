using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 死亡拒绝 / Death Denial（稀有 · 技能 · 消耗）
/// 回复下一张攻击牌造成未被格挡的伤害的血量。升级：费用 2→1。
/// —— 临时能力：打出一个「吸血buff」，下一张攻击牌（含多段/AoE）结算后，
///    按实际造成的 UnblockedDamage 总量回血，然后能力消失。
///    参考原生 Unrelenting（无情猛攻）的「下一张攻击牌」临时能力范式。
/// </summary>
public class DeathDenial : WandiModCard
{
    public DeathDenial() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 施加临时能力：下一张攻击牌造成伤害后等量回血，然后自毁
        await PowerCmd.Apply<DeathDenialPower>(choiceContext, Owner.Creature, 0, Owner.Creature, this);
        MainFile.Logger.Info("[死亡拒绝] 施加临时能力：下一张攻击牌全额吸血");
    }
}
