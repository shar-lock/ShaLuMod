using MegaCrit.Sts2.Core.Commands;                 // PowerCmd / CardPileCmd / CardCmd
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / PileType
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.HoverTips;                 // IHoverTip / HoverTipFactory
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.Models;                    // PowerModel
using WandiMod.WandiModCode.Cards;                  // ConquerAllLands
using WandiMod.WandiModCode.Relics;                 // UndyingRoyalBlood

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 荡平进度 / Conquer Progress —— 独立计数器（与血仇层数脱钩）。
/// 仅在获得血仇时累加进度；消耗血仇（打牌 / 血仇自身成长消耗）不影响本进度。
/// 玩家可见进度到 7 → 生成 1 张「荡平万邦」并扣减 7 点进度（可一次获得过量时连触发）。
/// StackType=Counter：NPower 渲染右下角数字。Amount 固定 1 防进度 0 被移除。
/// </summary>
public class ConquerProgressPower : WandiModPower
{
    public const int Threshold = 7;

    private int _progress;

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => _progress;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Threshold", Threshold),
        new IntVar("Progress", 0),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<ConquerAllLands>(),
    ];

    /// <summary>
    /// 监听血仇正向变化：按获得量累加进度；满 Threshold 则生成荡平万邦（不消耗血仇）。
    /// </summary>
    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power is not VengeancePower || amount <= 0)
            return;

        _progress += (int)amount;
        DynamicVars["Progress"].BaseValue = _progress;
        InvokeDisplayAmountChanged();

        while (_progress >= Threshold)
        {
            _progress -= Threshold;
            DynamicVars["Progress"].BaseValue = _progress;
            InvokeDisplayAmountChanged();
            await GenerateConquerCard(choiceContext);
        }
    }

    private async Task GenerateConquerCard(PlayerChoiceContext choiceContext)
    {
        if (CombatState == null || Owner.Player == null)
        {
            MainFile.Logger.Error("[荡平进度] 生成时 CombatState/Owner.Player 为空，荡平万邦未生成");
            return;
        }

        CardModel card = CombatState.CreateCard<ConquerAllLands>(Owner.Player);
        bool awakened = Owner.Player.GetRelic<UndyingRoyalBlood>() != null;
        if (awakened)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
        MainFile.Logger.Info($"[荡平进度] 满 {Threshold} → 生成荡平万邦（觉醒={awakened}），剩余进度 {_progress}");
    }
}
