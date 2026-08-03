using BaseLib.Extensions;                           // WithValueProp / WithUpgradeTo
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd / CreatureCmd
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // CalculatedDamageVar 等
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 🔗 此乃天谴 / Blood for Blood（稀有 · 攻击 · 联机）。
/// 造成「所有队友生命上限总和」10%/15% 的伤害；回复造成伤害50%的生命值。
/// 卡面实时显示总伤（CalculatedDamageVar）。
/// </summary>
public class BloodForBlood : WandiModCard
{
    public BloodForBlood() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Pct", 10).WithUpgradeTo(15),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(10m).WithUpgradeTo(15),
        // multiplier = 所有队友 MaxHp 总和 / 100
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) => {
            if (card.Owner?.Creature == null || card.CombatState == null) return 0m;
            decimal totalMaxHp = 0;
            foreach (var ally in card.CombatState.GetTeammatesOf(card.Owner.Creature)
                        .Where(t => t != null && t.IsAlive && t.IsPlayer))
                totalMaxHp += ally.MaxHp;
            return totalMaxHp / 100m;
        }),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || CombatState == null) return;
        var c = Owner.Creature;
        // CalculatedDamageVar 结算（卡面预览与攻击同源）；回血按实际 UnblockedDamage（对齐饮血反击）
        var executed = await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move).Execute(choiceContext);
        decimal dealt = 0;
        foreach (var results in executed.Results)
            foreach (var r in results)
                dealt += r.UnblockedDamage;
        if (dealt > 0)
            await CreatureCmd.Heal(c, dealt / 2m);
        MainFile.Logger.Info($"[此乃天谴] 实伤={dealt}，回血 {dealt / 2m}");
    }
}
