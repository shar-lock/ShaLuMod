using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // IndomitablePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 不屈 / Indomitable（罕见 · 能力）
/// 战斗结束时回复生命上限 5% 的生命值。升级：7%。
/// —— 续航件：每场战斗结束小额回血，长线作战。
/// 百分比随升级态传入 Power 的 Amount（5→7），由 IndomitablePower.AfterCombatEnd 读取。
/// </summary>
public class Indomitable : WandiModCard
{
    public Indomitable() : base(
        cost: 1,
        type: CardType.Power,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("HealPct", 5).WithUpgradeTo(7),  // 战后回血占生命上限的百分比
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[不屈] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }
        // Amount = 回血百分比（5/7），IndomitablePower 据此在 AfterCombatEnd 回 MaxHp*Amount/100
        int pct = DynamicVars["HealPct"].IntValue;
        await PowerCmd.Apply<IndomitablePower>(choiceContext, Owner.Creature, pct, Owner.Creature, this);
        MainFile.Logger.Info($"[不屈] 授予不屈：战斗结束回复生命上限 {pct}%");
    }
}
