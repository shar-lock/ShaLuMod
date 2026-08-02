using BaseLib.Extensions;                           // WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 灾厄之矛 / Calamity Spear（稀有 · 攻击 · 消耗）。
/// 造成「当前最大生命」40%/45% 的伤害（按含纷争的当前上限结算，吃纷争放大）。
/// </summary>
public class CalamitySpear : WandiModCard
{
    public CalamitySpear() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("MaxHpPct", 40).WithUpgradeTo(45)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        // 当前最大生命（含纷争临时上限）× 百分比——纷争越高，伤害越高
        decimal dmg = c.MaxHp * DynamicVars["MaxHpPct"].IntValue / 100m;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[灾厄之矛] MaxHp={c.MaxHp} ×{DynamicVars["MaxHpPct"].IntValue}% = {dmg} 伤");
    }
}
