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
/// 连刺 / Repeated Thrust（普通 · 攻击）。两段伤，可耗1层血仇提升伤害 / 6伤+3。
/// 血仇真实层数保底1层：耗血仇须以 Amount>=2 判定（消耗后剩余≥1，Power 永不消失）；
/// 且整牌只耗一次、增益作用于两段。
/// 多段必须走单次 AttackCommand.WithHitCount——活力(Vigor) 绑定整次 AttackCommand，
/// for 循环多次 Execute 会让活力只加到第一段。
/// </summary>
public class RepeatedThrust : WandiModCard
{
    public RepeatedThrust() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(5, ValueProp.Move).WithUpgradeTo(6), new IntVar("BloodBonus", 3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var c = Owner.Creature;
        int bonus = 0;
        var blood = c.GetPower<VengeancePower>();
        if (blood?.Amount >= 2)
        {
            // 保底1层：Amount>=2 才允许消耗（PowerCmd.Apply 负值减层；PowerModel.Amount 只读不可直写）
            await PowerCmd.Apply<VengeancePower>(choiceContext, c, -1, c, null);
            bonus = DynamicVars["BloodBonus"].IntValue;
            MainFile.Logger.Info($"[连刺] 耗1血仇 → 每段+{bonus}（剩余血仇={blood.Amount}）");
        }
        decimal hit = DynamicVars.Damage.BaseValue + bonus;
        // 单次 AttackCommand + WithHitCount(2)：活力覆盖两段
        await DamageCmd.Attack(hit)
            .WithHitCount(2)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
    }
}
