using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）/ CardSelectCmd / CardCmd
using MegaCrit.Sts2.Core.CardSelection;             // CardSelectorPrefs
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 净血 / Blood Cleanse（罕见 · 技能）
/// 选择消耗一张手牌，回复 5 点生命值。升级：7 点。
/// —— 消耗换回血：烧掉冗余手牌（诅咒/状态牌最佳）换续航。选牌消耗参考原版 Scavenge。
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
        new IntVar("Heal", 5).WithUpgradeTo(7),
    ];

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

        await CreatureCmd.Heal(creature, DynamicVars["Heal"].IntValue);
        MainFile.Logger.Info($"[净血] 回复 {DynamicVars["Heal"].IntValue} 生命");
    }
}
