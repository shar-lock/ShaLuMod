using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 坚韧 / Tenacity（罕见 · 技能）
/// 消耗手牌中的 1 张牌，获得 2 层【血仇】。升级：3 层。
/// —— 手牌管理 + 血仇启动：用多余手牌换血仇层数（参考原版 Second Wind 的「消耗手牌换收益」模型）。
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
        new IntVar("Vengeance", 2).WithUpgrade(3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 让玩家从手牌中选 1 张牌消耗（弹窗选择，参考 CardSelectCmd 的手牌筛选器）
        // TODO: 运行时确认 CardSelectCmd 的手牌消��� API（筛选器 / 回调 / 是否需 await 玩家选择）
        MainFile.Logger.Warn("[坚韧] 手牌消耗 API 未实现，跳过消耗（仅给血仇）");

        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
