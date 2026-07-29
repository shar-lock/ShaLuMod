using BaseLib.Extensions;                           // WithUpgrade / WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 暴风连击 / Storm Combo（罕见 · 攻击）
/// 造成 4 伤害 ×（当前【血仇】层数）。升级：段数 +1（即 ×（血仇 + 1））。
/// —— 动态段数血仇转化：把积攒的血仇一次性倾泻成多段小伤（不消耗血仇，仅读取层数）。
/// 命中数取决于打出时的血仇层数；无血仇且未升级时为 0 段（仅消耗费用，伤害落空）。
/// 注意：多段每段都会吃一次力量加成（ValueProp.Move），高力量下放大明显。
/// </summary>
public class StormCombo : WandiModCard
{
    public StormCombo() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4, ValueProp.Move),               // 每段基础伤害 4（升级不变）
        new IntVar("BloodHitBonus", 0).WithUpgrade(1),  // 段数 = 血仇 + 此值（0→1）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[暴风连击] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        int blood = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        int bonus = DynamicVars["BloodHitBonus"].IntValue;
        int hits = Math.Max(0, blood + bonus);
        decimal dmg = DynamicVars.Damage.BaseValue;

        if (hits == 0)
        {
            MainFile.Logger.Warn($"[暴风连击] 当前血仇 {blood} + bonus {bonus} = 0 段，伤害落空");
            return;
        }

        for (int i = 0; i < hits; i++)
        {
            await DamageCmd.Attack(dmg)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithValueProp(ValueProp.Move)
                .Execute(choiceContext);
        }

        MainFile.Logger.Info($"[暴风连击] 当前血仇 {blood}（+bonus {bonus}），打出 {hits} 段 {dmg} 伤害");
    }
}
