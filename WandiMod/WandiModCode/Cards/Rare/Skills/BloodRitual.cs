using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血祭仪典 / Blood Ritual（稀有 · 技能 · 消耗）。
/// 失 3 血，抽 3 牌，获得 3 血仇 / 抽 5。
/// </summary>
public class BloodRitual : WandiModCard
{
    public BloodRitual() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(3),
        new IntVar("Draw", 3).WithUpgradeTo(5),
        new IntVar("Vengeance", 3),
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [WandiModKeywords.Vengeance, CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        await CreatureCmd.Damage(choiceContext, c, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
        await VengeancePower.Grant(choiceContext, c, DynamicVars["Vengeance"].IntValue, this);
        MainFile.Logger.Info($"[血祭仪典] 失 {DynamicVars.HpLoss.BaseValue} 血，抽 {DynamicVars["Draw"].IntValue}，+{DynamicVars["Vengeance"].IntValue} 血仇");
    }
}
