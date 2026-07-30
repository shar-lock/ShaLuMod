using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>金色裁决 / Golden Judgment（稀有 · 攻击）。造成「当前最大生命」25%/35% 的伤害。</summary>
public class GoldenJudgment : WandiModCard
{
    public GoldenJudgment() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("MaxHpPct", 25).WithUpgradeTo(35)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        decimal dmg = c.MaxHp * DynamicVars["MaxHpPct"].IntValue / 100m;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[金色裁决] MaxHp={c.MaxHp} → {dmg} 伤");
    }
}
