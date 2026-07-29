using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 噬仇 / Vengeance Devour（罕见 · 攻击）
/// 造成「当前【血仇】层数 × 3」的伤害（不消耗血仇）。升级：倍率 ×4。
/// —— 血仇直接一次性转化伤害：高伤但不走多段，力量只放大一次（区别于暴风连击的多段小伤）。
/// 伤害在 OnPlay 里动态计算，故不用 DamageVar（DamageVar 是卡面显示真相源，动态值会让卡面显示异常）。
/// </summary>
public class VengeanceDevour : WandiModCard
{
    public VengeanceDevour() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Multiplier", 3).WithUpgrade(4),  // 血仇 → 伤害 倍率 3→4
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[噬仇] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        int blood = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        int multiplier = DynamicVars["Multiplier"].IntValue;
        decimal dmg = blood * multiplier;

        if (dmg <= 0)
        {
            MainFile.Logger.Warn($"[噬仇] 血仇为 0（blood={blood}），伤害落空");
            return;
        }

        await DamageCmd.Attack(dmg)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        MainFile.Logger.Info($"[噬仇] 血仇 {blood} × {multiplier} = {dmg} 伤害");
    }
}
