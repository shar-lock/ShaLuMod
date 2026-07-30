using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>🔗以血还血 / Blood for Blood（稀有 · 攻击 · 联机）。造成「队友本场所失总生命」50%/75% 的伤害；你回复一半。</summary>
public class BloodForBlood : WandiModCard
{
    public BloodForBlood() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Pct", 50).WithUpgradeTo(75)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null || CombatState == null) return;
        var c = Owner.Creature;
        // 统计队友本场所失总生命 = sum(MaxHp - CurrentHp) for each teammate
        decimal totalLost = 0;
        foreach (var ally in CombatState.GetTeammatesOf(c).Where(t => t != null && t.IsAlive && t.IsPlayer))
            totalLost += ally.MaxHp - ally.CurrentHp;
        decimal dmg = totalLost * DynamicVars["Pct"].IntValue / 100m;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        await CreatureCmd.Heal(c, dmg / 2m);
        MainFile.Logger.Info($"[以血还血] 队友总失血 {totalLost} → {dmg} 伤，回血 {dmg/2m}");
    }
}
