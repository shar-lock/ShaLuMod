using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.CardSelection;             // CardSelectorPrefs
using MegaCrit.Sts2.Core.Commands;                  // CardSelectCmd / CardPileCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / PileType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 裂伤 / Lacerate（普通 · 攻击）
/// 造成 6 点伤害，选择弃牌堆 1 张牌放回手牌。升级：9 伤。
/// —— 运转件：参考 Necrobinder 的 Graveblast（FromCombatPile 弃牌堆 → CardPileCmd.Add 入手牌）。
/// 选择提示走本卡 .selectionScreenPrompt 本地化（缺失会在出牌时抛 InvalidOperationException）。
/// </summary>
public class Lacerate : WandiModCard
{
    public Lacerate() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(6, ValueProp.Move).WithUpgradeTo(9),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // 弃牌堆为空时 FromCombatPile 直接返回空序列，不弹选牌 UI
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var card = (await CardSelectCmd.FromCombatPile(
            choiceContext, PileType.Discard.GetPile(Owner), Owner, prefs)).FirstOrDefault();
        if (card == null)
        {
            MainFile.Logger.Info("[裂伤] 弃牌堆为空或未选择，跳过回手");
            return;
        }

        await CardPileCmd.Add(card, PileType.Hand);
        MainFile.Logger.Info($"[裂伤] 从弃牌堆回手：{card.Title}");
    }
}
