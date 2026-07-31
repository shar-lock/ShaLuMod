using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 圣血洗礼 / Holy Blood Baptism（稀有 · 技能 · 消耗）。失6血，移除自身全部减益 + 12纷争 / 失6+回5。
/// 移除减益：遍历 Creature.Powers，Type==Debuff → PowerCmd.Remove（参考 Misery 的 debuff 筛选）。
/// </summary>
public class HolyBloodBaptism : WandiModCard
{
    public HolyBloodBaptism() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(6),
        new IntVar("Strife", 12),
        new IntVar("Heal", 0).WithUpgradeTo(5),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        await CreatureCmd.Damage(choiceContext, c, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // 移除自身全部减益——遍历 Powers，Type==Debuff → Remove（参考 Misery 的 debuff 筛选）
        var debuffs = c.Powers.Where(p => p.Type == PowerType.Debuff).ToList();  // ToList 快照避免遍历中修改
        foreach (var debuff in debuffs)
            await PowerCmd.Remove(debuff);
        await StrifePower.Grant(choiceContext, c, DynamicVars["Strife"].IntValue, c, this);
        int heal = DynamicVars["Heal"].IntValue;
        if (heal > 0) await CreatureCmd.Heal(c, heal);
        MainFile.Logger.Info($"[圣血洗礼] 失血 + 移除 {debuffs.Count} 个减益 + {DynamicVars["Strife"].IntValue} 纷争");
    }
}
