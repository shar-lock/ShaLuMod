using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 狂怒 / Fury（罕见 · 攻击 · 消耗）
/// 造成「当前【血仇】层数 × 3」的伤害。升级：倍率 ×4。
/// —— 噬仇的 0 费消耗版：免费高爆发的代价是打出即消（不能反复刷）。
/// 配合血仇囤积（自伤件、嗜血、起手遗物）做一波流收尾。
/// 同样不走 DamageVar（动态值），伤害在 OnPlay 里算。
/// </summary>
public class Fury : WandiModCard
{
    public Fury() : base(
        cost: 0,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Multiplier", 2).WithUpgradeTo(3),  // 血仇 → 伤害 倍率 2→3
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[狂怒] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        int blood = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        // 真实层数含保底 1；无 Power 时伤害落空（不凭空造 1 层）
        if (blood <= 0)
        {
            MainFile.Logger.Warn($"[狂怒] 无血仇 Power（blood={blood}），伤害落空");
            return;
        }
        int multiplier = DynamicVars["Multiplier"].IntValue;
        decimal dmg = blood * multiplier;

        await DamageCmd.Attack(dmg)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        MainFile.Logger.Info($"[狂怒] 血仇 {blood} × {multiplier} = {dmg} 伤害（消耗）");
    }
}
