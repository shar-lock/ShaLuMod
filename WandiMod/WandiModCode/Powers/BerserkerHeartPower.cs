using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（自伤）
using MegaCrit.Sts2.Core.Entities.Cards;           // CardPlay / CardType
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;  // PlayerChoiceContext
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 狂战之心 / Berserker Heart（万敌 · 能力 Power）。
/// 机制：每打出一张攻击牌：失 1 血，获得 1 血仇。
///   - 钩子 AfterCardPlayed：万敌本人打出攻击牌时，自伤 1 点。
///   - 「获得 1 血仇」由自伤自动触发——VengeancePower.AfterDamageReceived 会在万敌掉血时 +1 层。
///     （起手遗物「弑亲血脉」在战斗开始赋予血仇 Power，故此 Power 存活期间血仇引擎常驻。）
///     因此本 Power 只负责自伤，不重复调用 VengeancePower.Grant（避免双倍）。
/// 参考 RupturePower.AfterCardPlayed、原生 EnragePower.AfterCardPlayed（同钩子按卡牌类型触发）。
/// 注意：自伤用 Unblockable|Unpowered|Move——全额计入 UnblockedDamage，触发血仇叠层钩子。
/// </summary>
public class BerserkerHeartPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态类，不叠加

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay?.Card;
        if (card == null)
            return;
        // 仅万敌本人打出的攻击牌触发；战斗外/异常时序 Owner 可能为空
        if (Owner == null || card.Owner?.Creature != Owner)
            return;
        if (card.Type != CardType.Attack)
            return;
        // 仅在万敌这一侧的回合触发（敌人侧打牌不触发；CombatState.CurrentSide 参考 RupturePower）
        if (CombatState == null || CombatState.CurrentSide != Owner.Side)
            return;

        // 自伤 1 点：Unblockable|Unpowered|Move → 全额失血 → 触发 VengeancePower 自动 +1 血仇
        await CreatureCmd.Damage(choiceContext, Owner, 1m,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, card, cardPlay);

        MainFile.Logger.Info($"[狂战之心] 打出攻击牌 {card.Id.Entry}：自伤 1（自动 +1 血仇）");
    }
}
