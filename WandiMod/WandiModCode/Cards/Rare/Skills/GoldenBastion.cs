using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;                 // GoldenBastionNextTurnPower / StrifePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>金色壁垒 / Golden Bastion（稀有 · 技能 · X费）。获得 X×6 纷争；本回合 +X 力量 / X×8。</summary>
public class GoldenBastion : WandiModCard
{
    public GoldenBastion() : base(-2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("StrifePerEnergy", 6).WithUpgradeTo(8)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        int energy = DynamicVars.Energy.IntValue;
        int perEnergy = DynamicVars["StrifePerEnergy"].IntValue;
        await StrifePower.Grant(choiceContext, c, energy * perEnergy, c, this);
        // 下回合 +X 力量——GoldenBastionNextTurnPower（AfterSideTurnStart 触发后自毁，参考 DrawCardsNextTurnPower）
        await PowerCmd.Apply<GoldenBastionNextTurnPower>(choiceContext, c, energy, c, this);
        MainFile.Logger.Info($"[金色壁垒] X={energy}，纷争 {energy * perEnergy}，下回合 +{energy} 力量");
    }
}
