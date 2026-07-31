using MegaCrit.Sts2.Core.Combat;                   // CombatSide
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd（移除自身）
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature / DamageResult
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Models;                    // CardModel
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 浴血带冠 Power（万敌 · 联机单回合增益）。
/// 本回合内，每当万敌受到攻击并掉血，所有其他队友获得 Amount 层【纷争】。
/// 敌方回合结束后移除自身（单回合，参考 GuardianPactPower）。
/// Amount = 每次受击给队友的纷争层数（5/8，由卡牌传入）。
/// </summary>
public class BodyguardPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 单回合增益，不叠加

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // 仅万敌本人实际掉血时触发（给其他队友纷争）
        if (target != Owner || result.UnblockedDamage <= 0) return;
        if (CombatState == null) return;

        int strife = Amount;
        // 所有其他队友（同侧、玩家、存活、不含自己）
        foreach (var ally in CombatState.GetTeammatesOf(Owner).Where(t => t != null && t.IsAlive && t.IsPlayer && t != Owner))
            await StrifePower.Grant(choiceContext, ally, strife, Owner, cardSource);

        MainFile.Logger.Info($"[浴血带冠] 万敌受击 → 给其他队友各 {strife} 纷争");
    }

    /// <summary>敌方回合结束后移除自身（保护覆盖敌方出手阶段，单回合）。</summary>
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == Owner.Side) return;
        await PowerCmd.Remove(this);
        MainFile.Logger.Info("[浴血带冠] 敌方回合结束，移除浴血带冠");
    }
}
