using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 御敌 / Hold the Line（起手 · 技能，原「防御」位）
/// 获得 5 点【纷争】。升级：8 点。
/// —— 万敌没有格挡：他抬临时生命上限硬扛伤害（机制详见 StrifePower）。
/// </summary>
public class HoldTheLine : WandiModCard
{
    public HoldTheLine() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Basic,
        target: TargetType.Self)
    {
    }

    // 纷争数值（真相源）。IntVar 名称 "Strife" 对应本地化占位符 {Strife:diff()}；
    // WithUpgrade 设定升级后的数值，逻辑代码无需改动（读到的就是升级后值）。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 4).WithUpgrade(6),
    ];

    // 关键词词条（tooltip 文案见 card_keywords.json 的 WANDIMOD-STRIFE）
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Owner 是 Player（不是 Creature），生物实体要取 Owner.Creature。
        // 战斗外 / 异常时序下可能为 null——打不出效果就记错误日志并中断，避免空引用炸战斗
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[御敌] OnPlay 时 Owner.Creature 为空（不在战斗中？），纷争未授予");
            return;
        }

        int amount = DynamicVars["Strife"].IntValue;
        MainFile.Logger.Debug($"[御敌] 打出，请求 {amount} 点纷争");
        await StrifePower.Grant(choiceContext, Owner.Creature, amount, Owner.Creature, this);
    }
}
