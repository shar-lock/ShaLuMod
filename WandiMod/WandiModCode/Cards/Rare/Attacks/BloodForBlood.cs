using MegaCrit.Sts2.Core.Commands;                  // DamageCmd / CreatureCmd
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 🔗 此乃天谴 / Blood for Blood（稀有 · 攻击 · 联机）。
/// 造成「所有队友生命上限总和」25% 的伤害；你回复一半。升级：33%。
/// —— 联机承伤反击：队友血池越厚，反击越猛，自身回血续航。
/// </summary>
public class BloodForBlood : WandiModCard
{
    public BloodForBlood() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Pct", 25).WithUpgradeTo(33)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || CombatState == null) return;
        var c = Owner.Creature;
        // 所有队友（含自己）生命上限总和 × Pct
        decimal totalMaxHp = 0;
        foreach (var ally in CombatState.GetTeammatesOf(c).Where(t => t != null && t.IsAlive && t.IsPlayer))
            totalMaxHp += ally.MaxHp;
        decimal dmg = totalMaxHp * DynamicVars["Pct"].IntValue / 100m;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        await CreatureCmd.Heal(c, dmg / 2m);  // 回复一半
        MainFile.Logger.Info($"[此乃天谴] 队友生命上限总和 {totalMaxHp} ×{DynamicVars["Pct"].IntValue}% = {dmg} 伤，回血 {dmg/2m}");
    }
}
