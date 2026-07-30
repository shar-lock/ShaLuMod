using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // WeakPower
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 断筋 / Hamstring（普通 · 攻击）
/// 造成 5 ���伤害，给予目标 1 层虚弱。升级：8 伤，2 层虚弱。
/// —— 攻击+减益工具件，与穿心枪（罕见·目标有易伤+5）形成上下位配合。
/// </summary>
public class Hamstring : WandiModCard
{
    public Hamstring() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5, ValueProp.Move).WithUpgradeTo(8),
        new IntVar("Weak", 1).WithUpgradeTo(2),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars["Weak"].IntValue, Owner.Creature, this);
    }
}
