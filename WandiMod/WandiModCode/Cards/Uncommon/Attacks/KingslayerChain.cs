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
/// 弑王枪·连突 / Kingslayer Chain（罕见 · 攻击）
/// 造成 5 伤害 ×2；每段消耗 1 层【血仇】额外 +5 伤害（无血仇则不加）。升级：6 伤害 ×2，每段 +6。
/// —— 连刺（RepeatedThrust）的罕见加强版：更高单段基数 + 更高血仇增伤，2 费标杆多段。
/// 手动逐段结算（每段按当时血仇余量决定是否消耗 +5），ValueProp.Move 吃力量加成。
/// </summary>
public class KingslayerChain : WandiModCard
{
    public KingslayerChain() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move).WithUpgradeTo(6),  // 每段基础伤害 5→6
        new RepeatVar(2),                                  // 固定 2 段（升级不变）
        new IntVar("BloodBonus", 5).WithUpgradeTo(6),        // 每段消耗 1 血仇的额外伤害 5→6
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[弑王枪·连突] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        int hits = DynamicVars.Repeat.IntValue;
        decimal baseDmg = DynamicVars.Damage.BaseValue;
        int bonus = DynamicVars["BloodBonus"].IntValue;
        int consumed = 0;

        // 逐段结算：每段若还有血仇，消耗 1 层并 +bonus（多段消耗型，参考 RepeatedThrust）
        for (int i = 0; i < hits; i++)
        {
            decimal dmg = baseDmg;
            var vengeance = creature.GetPower<VengeancePower>();
            // 保底1层：Amount>=2 才允许消耗（Amount==1 是地板层，耗掉会把血仇 Power 移除）
            if (vengeance != null && vengeance.Amount >= 2)
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
            MainFile.Logger.Info($"[弑王枪·连突] 打出 {hits} 段，消耗 {consumed} 血仇增伤");
    }
}
