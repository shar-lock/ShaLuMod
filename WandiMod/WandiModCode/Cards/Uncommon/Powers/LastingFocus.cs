using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd / CreatureCmd（设定当前生命）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // LastingFocusPower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 战意持久 / Lasting Focus（罕见 · 能力）
/// 将你的生命值减少至生命上限的 50%，受到的伤害降低 20%。升级：减伤 30%。
/// —— 主动压血换减伤：与嗜血形态（HP≤50% 触发）天然契合，压血即激活暴怒通道。
/// 生命设定在卡牌 OnPlay 完成（持有 choiceContext）；减伤由 LastingFocusPower.ModifyDamageMultiplicative 处理。
/// </summary>
public class LastingFocus : WandiModCard
{
    public LastingFocus() : base(
        cost: 1,
        type: CardType.Power,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("ReducePct", 20).WithUpgradeTo(30),  // 受到伤害降低百分比 20→30
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[战意持久] OnPlay 时 Owner.Creature 为空，能力未授予");
            return;
        }

        // ① 生命减少至上限的 50%（向下取整）。SetCurrentHp 见 StrifePower 同 API。
        //    设定生命不视为「失血」（不走 Damage 管线），故不触发 VengeancePower 的失血叠层。
        int target = (int)(creature.MaxHp * 0.5m);
        if (creature.CurrentHp > target)
            await CreatureCmd.SetCurrentHp(creature, target);

        // ② 授予减伤 Power（Amount=20/30，LastingFocusPower 返回 1 - Amount/100）
        int pct = DynamicVars["ReducePct"].IntValue;
        await PowerCmd.Apply<LastingFocusPower>(choiceContext, creature, pct, creature, this);

        MainFile.Logger.Info($"[战意持久] 生命降至 {target}（上限50%），授予减伤 {pct}%");
    }
}
