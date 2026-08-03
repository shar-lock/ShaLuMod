using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 庇护 / Sanctuary（罕见 · 技能 · 消耗）
/// 获得「当前最大生命值」10% 的【纷争】。升级：15%。
/// —— MaxHp 缩放纷争：生命上限越高纷争越多（吃纷争放大）。消耗防重复刷。
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
        new IntVar("MaxHpPct", 10).WithUpgradeTo(15),   // 当前最大生命值的百分比（整数，运算时 /100）
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

        decimal maxHp = creature.MaxHp;                                      // 当前最大生命（含纷争）
        decimal pct = DynamicVars["MaxHpPct"].IntValue / 100m;               // 0.10 / 0.15
        // 纷争层数向下取整（至少 0）
        int strife = (int)Math.Max(0m, Math.Floor(maxHp * pct));
        if (strife > 0)
        {
            await StrifePower.Grant(choiceContext, creature, strife, creature, this);
        }
        MainFile.Logger.Info($"[庇护] MaxHp {maxHp} × {pct} = {strife} 纷争");
    }
}
