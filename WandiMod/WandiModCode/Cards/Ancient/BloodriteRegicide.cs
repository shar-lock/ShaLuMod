using BaseLib.Extensions;                       // RemovePrefix
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 血祭·诛王枪 / Bloodrite Regicide（先古 · 攻击 · 1费 · 保留）。
/// 经先古之民「欧洛巴斯」的古老牙齿将血祭之枪替换为本卡。
/// 造成 10 伤；获得 2 血仇，并额外造成「当前最大生命值」15% 的伤害 / 14伤、3血仇、20%。
/// 使用 CalculatedDamageVar（参考荡平万邦）实现卡面实时显示含动态加成的总伤。
/// </summary>
public class BloodriteRegicide : WandiModCard
{
    public BloodriteRegicide() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(10m).WithUpgradeTo(14),
        new ExtraDamageVar(15m).WithUpgradeTo(20),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
            card.Owner.Creature == null ? 0m : card.Owner.Creature.MaxHp / 100m),
        new IntVar("Vengeance", 2).WithUpgradeTo(3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        // ① 造成 CalculatedDamage（基础+当前最大生命%，卡面实时显示同源）
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        // ② 获得血仇
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
        MainFile.Logger.Info($"[血祭·诛王枪] 打出 {DynamicVars.CalculatedDamage.Calculate(null)} 伤+{DynamicVars["Vengeance"].IntValue} 血仇");
    }
}
