using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 王者归来 / King's Return（稀有 · 技能 · 消耗）。
/// HP≤50%：12 纷争 + 3 力量 + 回 8；否则 8 纷争。升级：16+4+12 / 12。
/// </summary>
public class KingsReturn : WandiModCard
{
    public KingsReturn() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("StrifeHigh", 12).WithUpgradeTo(16),
        new IntVar("StrifeLow", 8).WithUpgradeTo(12),
        new IntVar("Strength", 3).WithUpgradeTo(4),
        new IntVar("Heal", 8).WithUpgradeTo(12),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [WandiModKeywords.Strife, CardKeyword.Exhaust];

    /// <summary>HP≤50% 时金边，提示爆发条件已满足。</summary>
    protected override bool ShouldGlowGoldInternal =>
        Owner?.Creature != null
        && Owner.Creature.CurrentHp <= Owner.Creature.MaxHp * 0.5m;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        if (c == null)
        {
            MainFile.Logger.Error("[王者归来] OnPlay 时 Owner.Creature 为空");
            return;
        }

        bool lowHp = c.CurrentHp <= c.MaxHp * 0.5m;
        if (lowHp)
        {
            await StrifePower.Grant(choiceContext, c, DynamicVars["StrifeHigh"].IntValue, c, this);
            await PowerCmd.Apply<StrengthPower>(choiceContext, c, DynamicVars["Strength"].IntValue, c, null);
            await CreatureCmd.Heal(c, DynamicVars["Heal"].IntValue);
            MainFile.Logger.Info($"[王者归来] HP≤50% 爆发：纷争+{DynamicVars["StrifeHigh"].IntValue} 力量+{DynamicVars["Strength"].IntValue} 回血{DynamicVars["Heal"].IntValue}");
        }
        else
        {
            await StrifePower.Grant(choiceContext, c, DynamicVars["StrifeLow"].IntValue, c, this);
            MainFile.Logger.Info($"[王者归来] HP>50%：纷争+{DynamicVars["StrifeLow"].IntValue}");
        }
    }
}
