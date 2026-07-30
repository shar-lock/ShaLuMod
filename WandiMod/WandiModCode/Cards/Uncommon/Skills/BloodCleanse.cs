using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd / PowerCmd
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 净血 / Blood Cleanse（罕见 · 技能）
/// 移除自身 1 个减益，获得 6 点【纷争】。升级：额外回复 3 生命（纷争提至 9）。
/// —— 解控 + 防御：清一个负面效果的同时补纷争（临时上限）扛伤，升级添续航。
/// </summary>
public class BloodCleanse : WandiModCard
{
    public BloodCleanse() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 6).WithUpgradeTo(9),
        new IntVar("Heal", 0).WithUpgradeTo(3),   // 基础不回血，升级后才回 3
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[净血] OnPlay 时 Owner.Creature 为空，效果未触发");
            return;
        }

        // 移除自身 1 个减益——遍历 Powers，取第一个 Debuff → Remove（参考 Misery）
        var debuff = creature.Powers.FirstOrDefault(p => p.Type == PowerType.Debuff);
        if (debuff != null)
        {
            await PowerCmd.Remove(debuff);
            MainFile.Logger.Info($"[净血] 移除减益：{debuff.GetType().Name}");
        }

        // 纷争
        await StrifePower.Grant(choiceContext, creature, DynamicVars["Strife"].IntValue, creature, this);

        // 升级后才有的回血
        int heal = DynamicVars["Heal"].IntValue;
        if (heal > 0)
        {
            await CreatureCmd.Heal(creature, heal);
        }
        MainFile.Logger.Info($"[净血] 获得 {DynamicVars["Strife"].IntValue} 纷争" + (heal > 0 ? $"，回 {heal} 血" : ""));
    }
}
