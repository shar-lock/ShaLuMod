using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 暴风连击 / Storm Combo（罕见 · 攻击）
/// 造成 4 点伤害，段数 = 当前【血仇】真实层数（保底至少 1 段）。升级：伤害 4→5。
/// —— 动态段数血仇转化：把积攒的血仇一次性倾泻成多段小伤（不消耗血仇，仅读取层数）。
/// 血仇 Power 保底 1 层（显示 0）：真实 Amount 始终 ≥1 → 至少打 1 段，与「看到 0 层仍有 1 层效果」的数值设计一致。
/// 多段必须走单次 AttackCommand.WithHitCount（剑回旋镖 / 烈火同款）——
/// 活力(Vigor) 在 BeforeAttack 绑定整次 AttackCommand，AfterAttack 才消耗；
/// 若 for 循环多次 Execute，活力只会加到第一段并被立刻清掉。
/// </summary>
public class StormCombo : WandiModCard
{
    public StormCombo() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(4, ValueProp.Move).WithUpgradeTo(5),  // 每段基础伤害 4→5（升级加伤不加段）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[暴风连击] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        // 真实层数（含保底 1）；无 Power 时也至少打 1 段
        int blood = creature.GetPower<VengeancePower>()?.Amount ?? 0;
        int hits = Math.Max(1, blood);
        decimal dmg = DynamicVars.Damage.BaseValue;

        // 单次 AttackCommand + WithHitCount：活力/力量等 BeforeAttack 绑定覆盖每一段
        await DamageCmd.Attack(dmg)
            .WithHitCount(hits)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);

        MainFile.Logger.Info($"[暴风连击] 血仇真实 {blood}（显示 {Math.Max(0, blood - 1)}），打出 {hits} 段 {dmg} 伤害");
    }
}
