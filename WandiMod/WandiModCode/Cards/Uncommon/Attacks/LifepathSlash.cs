using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 命途斩 / Lifepath Slash（罕见 · 攻击）
/// 造成「当前最大生命」20% 的伤害。升级：30%。
/// —— MaxHp 缩放型：万敌走 MaxHp 上限流派（堆 MaxHp 遗物、不灭王血觉醒等）的爆发件。
/// 走 OnPlay 手动 DamageCmd（缩放值在打出时按当前 MaxHp 动态计算，不走 DamageVar 以免卡面显示异常）。
/// 与血仇/纷争无直接关联，故不挂 WandiModKeywords。
/// </summary>
public class LifepathSlash : WandiModCard
{
    public LifepathSlash() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("MaxHpPct", 20).WithUpgrade(30),  // 最大生命百分比 20→30（运算时 /100）
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null)
        {
            MainFile.Logger.Error("[命途斩] OnPlay 时 Owner.Creature 或目标为空，效果未触发");
            return;
        }

        decimal pct = DynamicVars["MaxHpPct"].IntValue / 100m;  // 0.20 / 0.30
        decimal dmg = creature.MaxHp * pct;

        await DamageCmd.Attack(dmg)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        MainFile.Logger.Info($"[命途斩] MaxHp {creature.MaxHp} × {pct} = {dmg} 伤害");
    }
}
