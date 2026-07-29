using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 死地后生 / Turn The Tide（罕见 · 技能）
/// HP ≤ 30%：获得 18 点【纷争】并回复 4 生命；否则只获得 6 纷争。升级：24 纷争 + 回 6 / 9 纷争。
/// —— 绝境翻盘件：低血量时一次性大额纷争（临时上限）+ 回血保命；健康时仅小额纷争垫防。
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
        new IntVar("StrifeHigh", 18).WithUpgradeTo(24),  // HP≤30% 时的大额纷争
        new IntVar("StrifeLow", 6).WithUpgradeTo(9),     // HP>30% 时的小额纷争
        new IntVar("Heal", 4).WithUpgradeTo(6),          // HP≤30% 时的回血
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[死地后生] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // HP ≤ 30% 触发「死地」分支：大额纷争 + 回血；否则只给小额纷争
        bool critical = creature.CurrentHp <= creature.MaxHp * 0.3m;
        if (critical)
        {
            await StrifePower.Grant(choiceContext, creature, DynamicVars["StrifeHigh"].IntValue, creature, this);
            int heal = DynamicVars["Heal"].IntValue;
            await CreatureCmd.Heal(creature, heal);
            MainFile.Logger.Info($"[死地后生] HP≤30%（{creature.CurrentHp}/{creature.MaxHp}），获得 {DynamicVars["StrifeHigh"].IntValue} 纷争，回 {heal} 血");
        }
        else
        {
            await StrifePower.Grant(choiceContext, creature, DynamicVars["StrifeLow"].IntValue, creature, this);
            MainFile.Logger.Info($"[死地后生] HP>30%（{creature.CurrentHp}/{creature.MaxHp}），获得 {DynamicVars["StrifeLow"].IntValue} 纷争");
        }
    }
}
