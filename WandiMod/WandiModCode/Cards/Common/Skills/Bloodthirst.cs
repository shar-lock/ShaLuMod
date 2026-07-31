using MegaCrit.Sts2.Core.Commands;                  // PowerCmd（力量）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // StrengthPower（原生力量）
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 嗜血 / Bloodthirst（普通 · 技能 · 消耗）
/// 获得 1 层力量。升级：2 力量。
/// —— 纯增益件（消耗保证一次性，防止反复刷力量）。
/// </summary>
public class Bloodthirst : WandiModCard
{
    public Bloodthirst() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strength", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[嗜血] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }
        await PowerCmd.Apply<StrengthPower>(choiceContext, creature, DynamicVars["Strength"].IntValue, creature, null);
    }
}
