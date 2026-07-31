using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 狂化 Power：每当你获得血仇，抽 1 张牌（无上限）。
/// 钩子 AfterPowerAmountChanged：监听 VengeancePower 的正向变化 → 抽 1。
/// </summary>
public class FrenzyPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态型，不叠加

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        // 仅当血仇正向增加时抽 1（消耗/负向变化不抽）
        if (power is not VengeancePower || amount <= 0) return;
        await CardPileCmd.Draw(ctx, 1, Owner.Player);
    }
}
