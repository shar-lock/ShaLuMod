using BaseLib.Extensions;                           // WithUpgrade / WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd / PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / RepeatVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 连刺 / Repeated Thrust（普通 · 攻击）
/// 造成 3 伤害 ×2；每段消耗 1 层【血仇】额外 +3 伤害（无血仇则不加）。升级：4 伤害 ×2。
/// —— 血仇消耗型多段：手动逐段结算（每段按当时血仇余量决定是否 +3），ValueProp.Move 吃力量。
/// </summary>
public class RepeatedThrust : WandiModCard
{
    public RepeatedThrust() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3, ValueProp.Move).WithUpgradeTo(4),  // 每段基础伤害 3→4
        new RepeatVar(2),                                  // 固定 2 段（升级不变）
        new IntVar("BloodBonus", 3),                       // 每段消耗 1 血仇的额外伤害（固定 3）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[连刺] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        int hits = DynamicVars.Repeat.IntValue;
        decimal baseDmg = DynamicVars.Damage.BaseValue;
        int bonus = DynamicVars["BloodBonus"].IntValue;
        int consumed = 0;

        // 逐段结算：每段若还有血仇，消耗 1 层并 +bonus
        for (int i = 0; i < hits; i++)
        {
            decimal dmg = baseDmg;
            var vengeance = creature.GetPower<VengeancePower>();
            if (vengeance != null && vengeance.Amount > 0)
            {
                await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -1, creature, null);
                dmg += bonus;
                consumed++;
            }
            await DamageCmd.Attack(dmg)
                .FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithValueProp(ValueProp.Move)
                .Execute(choiceContext);
        }

        if (consumed > 0)
            MainFile.Logger.Info($"[连刺] 打出 {hits} 段，消耗 {consumed} 血仇增伤");
    }
}
