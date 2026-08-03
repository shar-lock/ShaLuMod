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
/// 诛天焚骨的王座 / Throne of Bonescorching Heaven ⭐（稀有 · 攻击 · 全体）
/// 吞噬所有血仇，每层全体 8 伤；HP≤50% 每层额外+4。升级：10/+5。
/// 卡面实时显示总伤（CalculatedDamageVar）：DmgPerStack × blood + (HP≤50% ? BonusPerStack × blood : 0)
/// </summary>
public class ThroneOfBonescorchingHeaven : WandiModCard
{
    public ThroneOfBonescorchingHeaven() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("DmgPerStack", 8).WithUpgradeTo(10),
        new IntVar("BonusPerStack", 4).WithUpgradeTo(5),
        new CalculationBaseVar(0m),
        new ExtraDamageVar(1m),  // 哨兵值 1（乘数承担全部伤害计算）
        // CalculatedDamageVar = 0 + 1 × multiplier = multiplier = (DmgPerStack + 残血加成) × blood
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) => {
            if (card.Owner.Creature == null) return 0m;
            int blood = card.Owner.Creature.GetPower<VengeancePower>()?.Amount ?? 0;
            if (blood <= 0) return 0m;
            int perStack = card.DynamicVars["DmgPerStack"].IntValue;
            int bonus = card.DynamicVars["BonusPerStack"].IntValue;
            decimal mul = perStack;
            if (card.Owner.Creature.CurrentHp <= card.Owner.Creature.MaxHp * 0.5m)
                mul += bonus;
            return mul * blood;
        }),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null) { MainFile.Logger.Error("[诛天焚骨的王座] Creature/CombatState 为空"); return; }

        int blood = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        if (blood <= 0) { MainFile.Logger.Warn("[诛天焚骨的王座] 无血仇，效果落空"); return; }

        // 吞噬所有血仇（留 1 层防 Power 移除）
        await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -(blood - 1), creature, null);

        // 用 CalculatedDamageVar 结算（卡面预览与实打同源）
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay).TargetingAllOpponents(CombatState).Execute(choiceContext);

        MainFile.Logger.Info($"[诛天焚骨的王座] 吞噬 {blood} 血仇，全体 {DynamicVars.CalculatedDamage.Calculate(null)} 伤");
    }
}
