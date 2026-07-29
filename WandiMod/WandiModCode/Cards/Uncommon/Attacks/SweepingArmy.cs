using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // PowerCmd���虚弱）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.Models.Powers;             // WeakPower（原生虚弱）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 横扫千军 / Sweeping Army（罕见 · 攻击 · AoE）
/// 对所有敌人造成 10 点伤害，并施加 1 层虚弱。升级：14 伤害，2 层虚弱。
/// —— AoE 减益件：CommonActions.CardAttack 按 TargetType.AllEnemies 自动全体攻击，
///    再对 CombatState.HittableEnemies 逐一施加虚弱（参考 VengefulHeart 的群体施减益写法）。
/// </summary>
public class SweepingArmy : WandiModCard
{
    public SweepingArmy() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10, ValueProp.Move).WithUpgradeTo(14),
        new IntVar("Weak", 1).WithUpgradeTo(2),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null || Owner.Creature == null)
        {
            MainFile.Logger.Error("[横扫千军] OnPlay 时 CombatState 或 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 全体攻击（读 DamageVar；TargetType.AllEnemies 决定选牌 UI 无需点目标）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 对所有可命中敌人施加虚弱（PowerCmd.Apply：target=敌人，source=Owner.Creature）
        int weak = DynamicVars["Weak"].IntValue;
        foreach (var enemy in CombatState.HittableEnemies)
            await PowerCmd.Apply<WeakPower>(choiceContext, enemy, weak, Owner.Creature, this);

        MainFile.Logger.Info($"[横扫千军] 全体攻击 + 对所有敌人施加 {weak} 层虚弱");
    }
}
