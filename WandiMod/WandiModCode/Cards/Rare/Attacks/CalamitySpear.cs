using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // CalculatedDamageVar 等
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 灾厄之矛 / Calamity Spear（稀有 · 攻击 · 消耗）。
/// 造成「当前最大生命」40%/60% 的伤害（含纷争上限）。卡面实时显示总伤。
/// CalculatedDamageVar = 0 + ExtraDamage(40→60) × MaxHp/100，与 OnPlay 结算同源。
/// </summary>
public class CalamitySpear : WandiModCard
{
    public CalamitySpear() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(0m),
        new ExtraDamageVar(40m).WithUpgradeTo(60),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
            card.Owner.Creature == null ? 0m : card.Owner.Creature.MaxHp / 100m),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[灾厄之矛] MaxHp={Owner.Creature.MaxHp} → {DynamicVars.CalculatedDamage.Calculate(null)} 伤");
    }
}
