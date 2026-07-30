using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（自伤走 Damage + Unblockable|Unpowered）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // HpLossVar / DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血潮 / Blood Tide Surge（罕见 · 攻击 · AoE）
/// 失去 3 点生命，对所有敌人造成 9 点伤害，获得 1 层【血仇】。升级：13 伤害。
/// —— BloodRite 的 AoE 升级版：自伤在前（产 1 层血仇，经 AfterDamageReceived 钩子放大本张全体伤害），
///    再全体攻击，最后额外授予 1 层血仇（卡面效果）。
/// </summary>
public class BloodTideSurge : WandiModCard
{
    public BloodTideSurge() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(3),
        new DamageVar(9, ValueProp.Move).WithUpgradeTo(13),
        new IntVar("Vengeance", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[血潮] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // ① 自伤（Unblockable → 全额计入 UnblockedDamage → 触发血仇叠层钩子，产 1 层血仇放大本张全体伤害）
        await CreatureCmd.Damage(choiceContext, creature, DynamicVars.HpLoss.BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // ② 全体攻击（TargetType.AllEnemies 自动全体；能吃到 ① 新产的那层血仇的 +2% 放大）
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        // ③ 卡面效果：额外获得 1 层血仇
        await VengeancePower.Grant(choiceContext, creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
