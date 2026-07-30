using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.CardSelection;             // CardSelectorPrefs
using MegaCrit.Sts2.Core.Commands;                 // CardSelectCmd / CardCmd
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 净血 / Blood Cleanse（罕见 · 技能）
/// 选择消耗一张手牌，获得 7 点【纷争】。升级：13 纷争。
/// —— 消耗流转防御：烧掉冗余手牌（诅咒/状态牌最佳）换大额临时上限。选牌消耗参考原版 Scavenge。
/// </summary>
public class BloodCleanse : WandiModCard
{
    public BloodCleanse() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 7).WithUpgradeTo(13),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[净血] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // 选择消耗一张手牌（Scavenge 范式：ExhaustSelectionPrompt 强制选 1 张；手牌为空时返回 null，跳过消耗）
        var card = (await CardSelectCmd.FromHand(choiceContext, Owner,
            new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), null, this)).FirstOrDefault();
        if (card != null)
        {
            await CardCmd.Exhaust(choiceContext, card);
            MainFile.Logger.Info($"[净血] 消耗手牌：{card.GetType().Name}");
        }

        await StrifePower.Grant(choiceContext, creature, DynamicVars["Strife"].IntValue, creature, this);
        MainFile.Logger.Info($"[净血] 获得 {DynamicVars["Strife"].IntValue} 纷争");
    }
}
