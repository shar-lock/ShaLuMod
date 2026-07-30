using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 誓约之枪 / Vow Spear（普通 · 攻击）
/// 造成 9 点伤害；若有【血仇】（显示层数>0）则额外 +3 伤害。升级：12 伤害，+4。
/// —— 重新设计：与初始卡血祭之枪（造伤+血仇）区分——本卡不产血仇，
///    而是「奖励已有血仇」的条件增伤件（1费普通强度：基础9=打击+3，条件+3=满额12）。
/// —— 「有血仇」以真实层数>=2 判定（保底1层=地板层不显示，显示>0 才算有血仇）。
/// </summary>
public class VowSpear : WandiModCard
{
    public VowSpear() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9, ValueProp.Move).WithUpgradeTo(12),
        new IntVar("BloodBonus", 3).WithUpgradeTo(4),   // 有血仇时的额外伤害 3→4
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[誓约之枪] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        // 血仇真实层数保底1层：显示层数>0 ⇔ Amount>=2
        bool hasVengeance = creature.GetPower<VengeancePower>() is { Amount: >= 2 };
        decimal dmg = DynamicVars.Damage.BaseValue + (hasVengeance ? DynamicVars["BloodBonus"].IntValue : 0);
        if (hasVengeance)
            MainFile.Logger.Info($"[誓约之枪] 有血仇 → 伤害 {dmg}（基础 {DynamicVars.Damage.BaseValue}+{DynamicVars["BloodBonus"].IntValue}）");

        await DamageCmd.Attack(dmg)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);
    }
}
