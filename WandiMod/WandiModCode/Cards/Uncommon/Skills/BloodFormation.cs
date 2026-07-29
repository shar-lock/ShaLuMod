using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（自伤）/ PowerCmd（力量）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // HpLossVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // StrengthPower（原生力量）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血祭·阵 / Blood Formation（罕见 · 技能）
/// 失去 3 点生命，获得 2 层力量。升级：3 层力量。
/// —— 阵法血祭：用小额自伤换力量（参考原版 Demon Form 的力量价值，但一次性、更便宜）。
///    自伤经 VengeancePower.AfterDamageReceived 自动 +1 血仇，能放大后续自伤牌。
/// </summary>
public class BloodFormation : WandiModCard
{
    public BloodFormation() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(3),
        new IntVar("Strength", 2).WithUpgrade(3),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[血祭·阵] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 自伤：Unblockable|Unpowered|Move → 全额计入失血 → 触发血仇自动 +1
        await CreatureCmd.Damage(choiceContext, creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // ② 力量（直接走原生 StrengthPower）
        await PowerCmd.Apply<StrengthPower>(choiceContext, creature, DynamicVars["Strength"].IntValue, creature, null);

        MainFile.Logger.Info($"[血祭·阵] 失 {DynamicVars.HpLoss.BaseValue} 血，获得 {DynamicVars["Strength"].IntValue} 力量");
    }
}
