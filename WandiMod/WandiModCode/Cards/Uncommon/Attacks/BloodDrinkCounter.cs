using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using WandiMod.WandiModCode.Powers;                 // BloodDrinkCounterPower
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 饮血反击 / Blood Drink Counter（罕见 · 攻击）
/// 造成 8 点伤害，回复造成伤害的 30%。升级：11 伤害，40% 吸血。
/// —— 吸血件：攻击后按「基础伤害 × 吸血比例」回血。
///    简化口径：以 DynamicVars.Damage.BaseValue 为回血基数，不考虑格挡减免 / 易伤放大 / 力量加成。
/// // TODO: 精确吸血需读 AttackCommand.Results 的实际 DamageResult（参考 Feed.cs / RuinSpear.cs 的 WasTargetKilled 取值方式）。
/// </summary>
public class BloodDrinkCounter : WandiModCard
{
    public BloodDrinkCounter() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move).WithUpgradeTo(11),
        new IntVar("LifestealPct", 30).WithUpgradeTo(40),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[饮血反击] OnPlay 时 Owner.Creature 为空，吸血未触发");
            return;
        }

        // ① 施加精确吸血 Power（AfterAttack 汇总 UnblockedDamage → 按比例回血）
        await PowerCmd.Apply<BloodDrinkCounterPower>(choiceContext, creature, DynamicVars["LifestealPct"].IntValue, creature, this);

        // ② 造成伤害（Power 会在攻击结算后自动回血）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        MainFile.Logger.Info($"[饮血反击] 打出 {DynamicVars.Damage.BaseValue} 伤（吸血 {DynamicVars["LifestealPct"].IntValue}% 由 Power 精确结算）");
    }
}
