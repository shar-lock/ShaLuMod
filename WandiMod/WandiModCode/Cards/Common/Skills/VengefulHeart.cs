using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // VulnerablePower（原生易伤）
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 复仇心 / Vengeful Heart（普通 · 技能）
/// 失去 1 层【血仇】，对所有敌人施加 1 层易伤。升级：2 层易伤。费用 1。
/// —— 血仇·减益：把血仇转化成群体易伤（AoE 易伤）。血仇不足则不放易伤。
/// </summary>
public class VengefulHeart : WandiModCard
{
    public VengefulHeart() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("BloodCost", 1),                  // 需消耗的血仇（固定 1）
        new IntVar("Vulnerable", 1).WithUpgradeTo(2),  // 施加的易伤层数 1→2
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null)
        {
            MainFile.Logger.Error("[复仇心] OnPlay 时 Owner.Creature 或 CombatState 为空，效果未触发");
            return;
        }

        int cost = DynamicVars["BloodCost"].IntValue;
        int have = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        if (have < cost)
        {
            MainFile.Logger.Warn($"[复仇心] 血仇不足（{have}/{cost}），未施加易伤");
            return;
        }

        // 消耗血仇 + 对所有敌人施加易伤
        await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -cost, creature, null);
        int amount = DynamicVars["Vulnerable"].IntValue;
        foreach (var enemy in CombatState.HittableEnemies)
            await PowerCmd.Apply<VulnerablePower>(choiceContext, enemy, amount, creature, this);

        MainFile.Logger.Info($"[复仇心] 消耗 {cost} 血仇，对全体敌人施加 {amount} 层易伤");
    }
}
