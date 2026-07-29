using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 强身 / Vitalize（罕见 · 技能 · 消耗）
/// 回复「已损失生命」10%。升级：12%。
/// —— 续航件：按已损失生命等比回血（伤得越重回得越多）。消耗防重复刷血。
///    注意：满血时已损失生命为 0，回血量为 0（不白送，但卡仍消耗）。
/// </summary>
public class Vitalize : WandiModCard
{
    public Vitalize() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("MissingHpPct", 10).WithUpgradeTo(12),  // 已损失生命的百分比（整数，运算时 /100）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[强身] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        decimal missingHp = creature.MaxHp - creature.CurrentHp;     // 已损失生命
        decimal pct = DynamicVars["MissingHpPct"].IntValue / 100m;   // 0.10 / 0.12
        decimal heal = Math.Floor(missingHp * pct);                  // 向下取整（满血时为 0）
        if (heal > 0)
        {
            await CreatureCmd.Heal(creature, heal);
        }
        MainFile.Logger.Info($"[强身] 已损失生命 {missingHp} × {pct} = 回 {heal} 血");
    }
}
