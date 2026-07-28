using BaseLib.Abstracts;                       // CustomRelicModel
using MegaCrit.Sts2.Core.CardSelection;        // ThrowingPlayerChoiceContext
using MegaCrit.Sts2.Core.Commands;             // PowerCmd / CreatureCmd
using MegaCrit.Sts2.Core.Entities.Creatures;   // Creature
using MegaCrit.Sts2.Core.Entities.Relics;      // RelicRarity / RelicStatus
using MegaCrit.Sts2.Core.Models;               // ModelDb / RelicModel
using MegaCrit.Sts2.Core.Saves.Runs;           // [SavedProperty]（充能存档持久化）
using WandiMod.WandiModCode.Extensions;         // 资源路径扩展方法
using WandiMod.WandiModCode.Powers;             // VengeancePower

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 弑亲血脉 / Blood of the Kinslayer —— 万敌起手遗物（角色灵魂）。
/// 合并「血仇引擎」与「4 次免死」二为一：
///   ① 战斗开始赋予万敌「血仇」Power（BeforeCombatStart）；
///   ② 受到致命伤害时免死，回复至 30% 最大生命，消耗 1 次充能（共 4 次）。
/// 血仇的「失血叠层 / +2%/层伤害放大」逻辑在 VengeancePower 内；
/// 免死逻辑套用原生遗物「蜥蜴尾巴 / LizardTail」（ShouldDieLate + AfterPreventingDeath）。
/// 觉醒版「不灭王血」经先古之民欧洛巴斯的「���洛巴斯之触」替换（GetUpgradeReplacement）。
/// 注意：起手遗物不进普通遗物池，故直接继承 CustomRelicModel（不标 [Pool]）。
/// </summary>
public class BloodOfTheKinslayer : CustomRelicModel
{
    // —— 资源图标路径（类名小写；缺图自动回退 relic.png 占位）——
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();

    // 起手稀有度（不掉落于普通奖励）
    public override RelicRarity Rarity => RelicRarity.Starter;

    /// <summary>
    /// 免死充能次数（剩 0 则免死失效）。初始 4 次，致敬星铁天赋「4 次免死」。
    /// [SavedProperty] 保证充能跨存档持久化。参考 LizardTail 的 [SavedProperty] bool WasUsed（这里用 int）。
    /// </summary>
    [SavedProperty]
    public int Charges
    {
        get => _charges;
        set
        {
            AssertMutable();
            _charges = value;
            // 充能耗尽 → 遗物置灰（参考 LizardTail.WasUsed 设置 Status）
            if (IsUsedUp)
                Status = RelicStatus.Disabled;
            // TODO: 运行时确认数字计数器显示 API（SetCounter/ChangeCounter?），把 Charges 显示在遗物角标上
        }
    }
    private int _charges = 4;

    /// <summary>充能是否耗尽（用于置灰/计数归零判定，参考 LizardTail.IsUsedUp）。</summary>
    public override bool IsUsedUp => Charges <= 0;

    /// <summary>
    /// 战斗开始：赋予万敌「血仇」Power。参考原生 BeltBuckle.BeforeCombatStart + PowerCmd.Apply。
    /// 传 1 层以确保 Power 实例建立——
    /// // TODO: 运行时确认 PowerCmd.Apply 传 amount=0 是否建立实例；若能，可改 0（设计意图是开局 0 层、靠失血累积）。
    /// </summary>
    public override async Task BeforeCombatStart()
    {
        await PowerCmd.Apply<VengeancePower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            Owner.Creature,
            null);
    }

    /// <summary>
    /// 免死判定：返回 true = 「该生物照常死亡」，false = 「拦截这次死亡」。
    /// 仅拦截万敌本人的致死、且还有充能时。参考 LizardTail.ShouldDieLate。
    /// </summary>
    public override bool ShouldDieLate(Creature creature)
        => creature != Owner.Creature || Charges <= 0;

    /// <summary>
    /// 免死生效后：闪烁、扣 1 充能、回复 30% 最大生命。参考 LizardTail.AfterPreventingDeath。
    /// （回复量 = 30% 最大生命；从濒死状态约等于「回到 30%」。）
    /// </summary>
    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        Charges--;
        await CreatureCmd.Heal(creature, Math.Max(1m, creature.MaxHp * 0.30m));
    }

    /// <summary>
    /// 先古（觉���）替换入口：欧洛巴斯之触拾起时，游戏据此把本遗物替换为「不灭王血」。
    /// （BaseLib 的 StarterUpgradePatches 已让 CustomRelicModel 走此方法，无需自建先古之民。）
    /// </summary>
    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<UndyingRoyalBlood>();
}
