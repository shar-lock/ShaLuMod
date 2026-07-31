using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd / CardPileCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 绝命枪 / Reaper Spear（稀有 · 攻击）。
/// 造成 14 点伤害；HP≤50% 时抽 2 张牌。升级：15 伤害，抽 3 张。
/// </summary>
public class ReaperSpear : WandiModCard
{
    public ReaperSpear() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(14, ValueProp.Move).WithUpgradeTo(15),
        new IntVar("Draw", 2).WithUpgradeTo(3),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        // ① 造成伤害
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
        // ② HP≤50% 抽牌（残血续航）
        bool lowHp = c.CurrentHp <= c.MaxHp * 0.5m;
        if (lowHp)
        {
            int draw = DynamicVars["Draw"].IntValue;
            await CardPileCmd.Draw(choiceContext, draw, Owner);
            MainFile.Logger.Info($"[绝命枪] HP≤50%，抽 {draw} 张牌");
        }
    }
}
