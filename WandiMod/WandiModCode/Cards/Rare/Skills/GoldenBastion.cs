using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>金色壁垒 / Golden Bastion（稀有 · 技能 · X费）。获得 X×6 纷争；本回合 +X 力量 / X×8。</summary>
public class GoldenBastion : WandiModCard
{
    public GoldenBastion() : base(-2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("StrifePerEnergy", 6).WithUpgradeTo(8)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        int energy = DynamicVars.Energy.IntValue;
        int perEnergy = DynamicVars["StrifePerEnergy"].IntValue;
        await StrifePower.Grant(choiceContext, c, energy * perEnergy, c, this);
        // TODO: 设计稿写「下回合 +X 力量」——原生下回合力量走 NextTurnPower 或类似机制；
        // 先给本回合力量 + TODO 标注
        await PowerCmd.Apply<StrengthPower>(choiceContext, c, energy, c, null);
        MainFile.Logger.Info($"[金色壁垒] X={energy}，纷争 {energy * perEnergy}，力量 +{energy}（// TODO: 应为下回合）");
    }
}
