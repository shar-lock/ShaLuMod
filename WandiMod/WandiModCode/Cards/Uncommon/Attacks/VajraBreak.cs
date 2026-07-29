using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd（易伤）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // VulnerablePower（原生易伤）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 金刚破 / Vajra Break（罕见 · 攻击）
/// 造成 12 点伤害，给予目标 2 层易伤。升级：16 伤害（易伤层数不变）。
/// —— 攻击 + 单体减益件：先攻击再施加易伤，确保本次伤害不依赖刚挂的易伤
///    （易伤收益留给后续打击；与 Heartpiercer 互补：本卡「挂易伤」，Heartpiercer「吃易伤」）。
/// </summary>
public class VajraBreak : WandiModCard
{
    public VajraBreak() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12, ValueProp.Move).WithUpgrade(16),
        new IntVar("Vulnerable", 2),                // 易伤层数（升级不变）
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[金刚破] OnPlay 时目标为空，效果未触发");
            return;
        }
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[金刚破] OnPlay 时 Owner.Creature 为空，易伤未施加");
            return;
        }

        // ① 造成伤害（按攻击前的易伤状态结算）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 给予目标易伤（收益留给后续打击）
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target,
            DynamicVars["Vulnerable"].IntValue, Owner.Creature, this);
    }
}
