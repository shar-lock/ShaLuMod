using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 死地后生 / Turn The Tide（罕见 · 技能）
/// HP ≤ 50%：回复 10 生命值；否则回复 6 生命值。升级：15 / 8。
/// —— 绝境翻盘件：低血量时大额回血保命；健康时仅小额回血。
/// </summary>
public class TurnTheTide : WandiModCard
{
    public TurnTheTide() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("HealHigh", 10).WithUpgradeTo(15),  // HP≤50% 时的大额回血
        new IntVar("HealLow", 6).WithUpgradeTo(8),     // HP>50% 时的小额回血
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[死地后生] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // HP ≤ 50% 触发「死地」分支：大额回血；否则小额回血
        bool critical = creature.CurrentHp <= creature.MaxHp * 0.5m;
        int heal = critical ? DynamicVars["HealHigh"].IntValue : DynamicVars["HealLow"].IntValue;
        await CreatureCmd.Heal(creature, heal);
        MainFile.Logger.Info($"[死地后生] HP{(critical ? "≤" : ">")}50%（{creature.CurrentHp}/{creature.MaxHp}），回 {heal} 血");
    }
}
