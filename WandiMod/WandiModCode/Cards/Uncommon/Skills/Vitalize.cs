using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）/ PowerCmd（易伤）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // VulnerablePower（原生易伤）
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 强身 / Vitalize（罕见 · 技能 · 消耗）
/// 回复 12 点生命值，给予目标 1 层易伤。升级：15 生命值，2 层易伤。
/// —— 回血 + 减益组合件。消耗防重复刷血。
/// </summary>
public class Vitalize : WandiModCard
{
    public Vitalize() : base(
        cost: 2,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Heal", 12).WithUpgradeTo(15),
        new IntVar("Vulnerable", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[强身] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        // ① 回复自身生命
        await CreatureCmd.Heal(creature, DynamicVars["Heal"].IntValue);
        // ② 给予目标易伤
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,
            DynamicVars["Vulnerable"].IntValue, creature, this);

        MainFile.Logger.Info($"[强身] 回 {DynamicVars["Heal"].IntValue} 血，给予目标 {DynamicVars["Vulnerable"].IntValue} 易伤");
    }
}
