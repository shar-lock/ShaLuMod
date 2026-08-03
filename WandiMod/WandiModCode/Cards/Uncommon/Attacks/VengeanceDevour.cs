using BaseLib.Extensions;                           // WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // CalculatedDamageVar 等
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 噬仇 / Vengeance Devour（罕见 · 攻击）
/// 造成「当前【血仇】层数 × 3/4」的伤害（不消耗血仇）。
/// 卡面文案用 Multiplier；战斗中 InCombat 行 + OnPlay 走 CalculatedDamageVar。
/// </summary>
public class VengeanceDevour : WandiModCard
{
    public VengeanceDevour() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Multiplier", 3).WithUpgradeTo(4),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
        {
            if (card.Owner.Creature == null) return 0m;
            int blood = card.Owner.Creature.GetPower<VengeancePower>()?.Amount ?? 0;
            if (blood <= 0) return 0m;
            return blood * card.DynamicVars["Multiplier"].IntValue;
        }),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[噬仇] OnPlay 时目标为空，效果未触发");
            return;
        }

        decimal dmg = DynamicVars.CalculatedDamage.Calculate(null);
        if (dmg <= 0)
        {
            MainFile.Logger.Warn("[噬仇] 无血仇，伤害落空");
            return;
        }

        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        MainFile.Logger.Info($"[噬仇] {dmg} 伤害");
    }
}
