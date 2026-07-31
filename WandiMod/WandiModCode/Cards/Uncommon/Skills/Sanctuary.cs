using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 庇护 / Sanctuary（罕见 · 技能 · 消耗）
/// 获得「已损失生命」8% 的【纷争】。升级：10%。
/// —— 越伤越稳：失血越多纷争越高（临时上限等比扩张）。满血时为 0 纷争（不白送）。消耗防重复刷。
/// </summary>
public class Sanctuary : WandiModCard
{
    public Sanctuary() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("MissingHpPct", 8).WithUpgradeTo(10),   // 已损失生命的百分比（整数，运算时 /100）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[庇护] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        decimal missingHp = creature.MaxHp - creature.CurrentHp;             // 已损失生命
        decimal pct = DynamicVars["MissingHpPct"].IntValue / 100m;           // 0.25 / 0.30
        // 纷争层数向下取整（至少 0：满血时不给）
        int strife = (int)Math.Max(0m, Math.Floor(missingHp * pct));
        if (strife > 0)
        {
            await StrifePower.Grant(choiceContext, creature, strife, creature, this);
        }
        MainFile.Logger.Info($"[庇护] 已损失生命 {missingHp} × {pct} = {strife} 纷争");
    }
}
