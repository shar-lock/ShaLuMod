using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>浴血奋战 / Bloodbath（稀有 · 能力）。若荡平万邦只攻击到一个敌人则伤害×1.5/×2。</summary>
public class Bloodbath : WandiModCard
{
    public Bloodbath() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Multiplier", 150).WithUpgradeTo(200)]; // 整数百分比：150=1.5×
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<BloodbathPower>(choiceContext, Owner.Creature, DynamicVars["Multiplier"].IntValue, Owner.Creature, this);
}
