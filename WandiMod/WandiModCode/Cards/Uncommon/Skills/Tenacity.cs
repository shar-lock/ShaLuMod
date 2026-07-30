using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.CardSelection;             // CardSelectorPrefs
using MegaCrit.Sts2.Core.Commands;                  // CardCmd / CardSelectCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 坚韧 / Tenacity（罕见 · 技能）
/// 消耗手牌中的 1 张牌，获得 2 层【血仇】。升级：3 层。
/// —— 手牌管理 + 血仇启动：用多余手牌换血仇层数。
/// 卡牌选择消耗参考原生坚毅(TrueGrit)升级版：CardSelectCmd.FromHand + 内置 ExhaustSelectionPrompt
/// （Purity 式的 this.SelectionScreenPrompt 需要每张卡自带 .selectionScreenPrompt 本地化，
/// 缺失会在出牌时直接抛 InvalidOperationException）。
/// </summary>
public class Tenacity : WandiModCard
{
    public Tenacity() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Vengeance", 2).WithUpgradeTo(3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 让玩家从手牌中选 1 张牌消耗——坚毅(TrueGrit)升级版同款内置 ExhaustSelectionPrompt
        var prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1);
        var selected = (await CardSelectCmd.FromHand(choiceContext, Owner, prefs, null, this)).FirstOrDefault();
        if (selected != null)
            await CardCmd.Exhaust(choiceContext, selected);

        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);

        MainFile.Logger.Info($"[坚韧] 消耗手牌={selected?.Title ?? "无（空手牌）"}，获得 {DynamicVars["Vengeance"].IntValue} 血仇");
    }
}
