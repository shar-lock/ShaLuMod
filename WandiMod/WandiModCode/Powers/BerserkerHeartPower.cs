using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd 不再需要自伤
using MegaCrit.Sts2.Core.Entities.Cards;           // CardPlay / CardType
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature（Owner）
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;  // PlayerChoiceContext
using MegaCrit.Sts2.Core.Models;                   // CardModel

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 狂战之心 / Berserker Heart（万敌 · 能力 Power）。
/// 每打出一张攻击牌：获得 1 层【血仇】（不自伤）。
/// </summary>
public class BerserkerHeartPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var card = cardPlay?.Card;
        if (card == null)
            return;
        if (Owner == null || card.Owner?.Creature != Owner)
            return;
        if (card.Type != CardType.Attack)
            return;
        if (CombatState == null || CombatState.CurrentSide != Owner.Side)
            return;

        await VengeancePower.Grant(choiceContext, Owner, 1, card);
        MainFile.Logger.Info($"[狂战之心] 打出攻击牌 {card.Id.Entry} → +1 血仇");
    }
}
