using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 绝命枪 / Reaper Spear（稀有 · 攻击）。8伤；HP≤50% 额外+7 / 12+8。
/// // TODO: 设计稿写「HP≤50%费用变0」——费用是静态的无法动态降，简化为固定 1 费+残血加伤。
/// </summary>
public class ReaperSpear : WandiModCard
{
    public ReaperSpear() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move).WithUpgradeTo(12),
        new IntVar("Bonus", 7).WithUpgradeTo(8),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        decimal dmg = DynamicVars.Damage.BaseValue;
        bool lowHp = c.CurrentHp <= c.MaxHp * 0.5m;
        if (lowHp) dmg += DynamicVars["Bonus"].IntValue;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        if (lowHp) MainFile.Logger.Info($"[绝命枪] HP≤50%，总伤 {dmg}");
    }
}
