using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // BloodthirstFormPower / VengeancePower（关键词）

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 嗜血形态 / Bloodthirst Form（罕见 · 能力）
/// HP ≤ 50% 时，攻击造成伤害获得 1 层【血仇】（每回合 ≤ 3 次）。升级：≤ 4 次。
/// —— 压血暴怒件：与战意持久（主动压血）联动，低血时攻击快速产血仇。
/// 每回合上限随升级态传入 Power 的 Amount（3→4），由 BloodthirstFormPower 读取。
/// </summary>
public class BloodthirstForm : WandiModCard
{
    public BloodthirstForm() : base(
        cost: 1,
        type: CardType.Power,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Cap", 3).WithUpgradeTo(4),  // 每回合通过本能力产血仇的次数上限
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[嗜血形态] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }
        // Amount = 每回合产血仇上限（3/4），BloodthirstFormPower 据此在 AfterDamageReceived 计数封顶
        int cap = DynamicVars["Cap"].IntValue;
        await PowerCmd.Apply<BloodthirstFormPower>(choiceContext, Owner.Creature, cap, Owner.Creature, this);
        MainFile.Logger.Info($"[嗜血形态] 授予嗜血形态：HP≤50% 攻击产血仇，每回合上限 {cap}");
    }
}
