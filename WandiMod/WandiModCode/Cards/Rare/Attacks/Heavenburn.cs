using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>焚天 / Heavenburn（稀有 · 攻击 · 全体）。失8血，造成「力量×3+12」全体伤 / ×4+18。</summary>
public class Heavenburn : WandiModCard
{
    public Heavenburn() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(8),
        new IntVar("StrMultiplier", 3).WithUpgradeTo(4),
        new IntVar("BaseDamage", 12).WithUpgradeTo(18),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        if (c == null || CombatState == null) return;
        // 自伤
        await CreatureCmd.Damage(choiceContext, c, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // 力量缩放 AoE
        int str = c.GetPower<StrengthPower>()?.Amount ?? 0;
        decimal dmg = str * DynamicVars["StrMultiplier"].IntValue + DynamicVars["BaseDamage"].IntValue;
        await DamageCmd.Attack(dmg).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).WithValueProp(ValueProp.Move).Execute(choiceContext);
        MainFile.Logger.Info($"[焚天] 失血+全体 {dmg}（力量 {str}×{DynamicVars["StrMultiplier"].IntValue}+{DynamicVars["BaseDamage"].IntValue}）");
    }
}
