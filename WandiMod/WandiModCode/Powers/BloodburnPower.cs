using MegaCrit.Sts2.Core.GameActions.Multiplayer;  // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 焚血 / Bloodburn（万敌 · 能力 Power）。
/// 因自身卡牌失血时，额外获得 1 层【血仇】。
///   - 钩子 AfterDamageReceived：万敌本人因自己的卡牌失血 → +1 血仇（叠加在 VengeancePower 失血叠层之上）。
///   - 无每回合次数限制（每次自身卡牌失血都触发）。
/// </summary>
public class BloodburnPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0) return;
        // 仅「自身卡牌」失血：来源卡牌存在且属于万敌本人
        if (cardSource == null || cardSource.Owner?.Creature != Owner) return;

        Flash();
        await VengeancePower.Grant(choiceContext, Owner, 1, cardSource);
        MainFile.Logger.Info("[焚血] 自身卡牌失血 → 额外 +1 血仇");
    }
}
