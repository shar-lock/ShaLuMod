using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 弑神枪·无尽 / Godslayer Endless（稀有 · 攻击 · X费 · 保留）。X段，每段=血仇真实层数+bonus / +2。
/// X 费写法对齐原版 Whirlwind/Skewer：构造费传 0 + override HasEnergyCostX（左上角渲染能量图标 X），
/// 段数用 ResolveEnergyXValue() 取（DynamicVars.Energy 未声明会 KeyNotFound）。
/// 血仇读真实 Amount（保底 1 层 → 显示 0 时每段仍按 1 结算）。
/// 多段必须走单次 AttackCommand.WithHitCount——活力绑定整次 AttackCommand，循环 Execute 会让活力只加第一段。
/// </summary>
public class GodslayerEndless : WandiModCard
{
    public GodslayerEndless() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override bool HasEnergyCostX => true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("BonusPerHit", 0).WithUpgradeTo(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        int blood = c.GetPower<VengeancePower>()?.Amount ?? 0;
        // 保底按 1 层结算（显示 0 = 真实 1；无 Power 时也按 1）
        int effective = Math.Max(1, blood);
        int bonus = DynamicVars["BonusPerHit"].IntValue;
        decimal perHit = effective + bonus;
        int hits = ResolveEnergyXValue();
        if (hits <= 0)
        {
            MainFile.Logger.Info($"[弑神枪·无尽] X=0，未出伤（血仇真实 {blood}）");
            return;
        }
        await DamageCmd.Attack(perHit)
            .WithHitCount(hits)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        MainFile.Logger.Info($"[弑神枪·无尽] {hits} 段 × {perHit}（血仇有效 {effective}+{bonus}，真实 {blood}）");
    }
}
