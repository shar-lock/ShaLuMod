using MegaCrit.Sts2.Core.GameActions.Multiplayer; // ThrowingPlayerChoiceContext
using MegaCrit.Sts2.Core.Commands;             // PowerCmd / CreatureCmd
using MegaCrit.Sts2.Core.Entities.Creatures;   // Creature
using MegaCrit.Sts2.Core.Entities.Relics;      // RelicRarity / RelicStatus
using MegaCrit.Sts2.Core.HoverTips;            // IHoverTip / HoverTipFactory（悬停提示：血仇词条 + 荡平万邦预览）
using MegaCrit.Sts2.Core.Localization.DynamicVars; // DynamicVar / IntVar（描述变量：{Charges} 实时显示剩余免死次数）
using MegaCrit.Sts2.Core.Models;               // ModelDb / RelicModel
using MegaCrit.Sts2.Core.Saves.Runs;           // [SavedProperty]（充能存档持久化）
using WandiMod.WandiModCode.Cards;              // ConquerAllLands / WandiModKeywords（悬停提示用）
using WandiMod.WandiModCode.Powers;             // VengeancePower

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 弑亲血脉 / Blood of the Kinslayer —— 万敌起手遗物（角色灵魂）。
/// 合并「血仇引擎」与「2 次免死」二为一：
///   ① 战斗开始赋予万敌「血仇」Power（BeforeCombatStart）；
///   ② 受到致命伤害时免死，回复至 30% 最大生命，消耗 1 次充能（共 2 次）。
/// 血仇的「失血叠层 / +2%/层伤害放大」逻辑在 VengeancePower 内；
/// 免死逻辑套用原生遗物「蜥蜴尾巴 / LizardTail」（ShouldDieLate + AfterPreventingDeath）。
/// 觉醒版「不灭王血」经先古之民欧洛巴斯的「欧洛巴斯之触」替换（GetUpgradeReplacement）——继承本类充能数。
/// 必须继承 WandiModRelic 以获得 [Pool]：0.109 起所有遗物模型都强制要求 PoolAttribute，
/// 缺失会在游戏启动注册模型时直接致命错误。不进奖励池是靠 Rarity=Starter 保证的，不是靠不标 Pool。
/// 平衡：4 次实测过高（≈全程几乎不死），压到 2 次保留卖血容错、避免无脑强。
/// </summary>
public class BloodOfTheKinslayer : WandiModRelic
{
    // 起手稀有度（不掉落于普通奖励）
    public override RelicRarity Rarity => RelicRarity.Starter;

    /// <summary>
    /// 描述变量：{Charges} 让遗物描述里的免死次数实时跟随剩余充能（参考原版 WingedBoots 的
    /// DynamicVars["Rooms"].BaseValue 同步写法——遗物描述经 DynamicVars.AddTo 智能格式化）。
    /// </summary>
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Charges", 2)];

    /// <summary>
    /// 免死充能次数（剩 0 则免死失效）。初始 2 次（觉醒版不灭王血继承同值）。
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
            DynamicVars["Charges"].BaseValue = value;  // 同步描述变量 → 悬停描述实时显示剩余次数
            if (IsUsedUp)
                Status = RelicStatus.Disabled;
            InvokeDisplayAmountChanged();  // 刷新角标（参考 PenNib.UpdateDisplay）
        }
    }
    private int _charges = 2;

    /// <summary>
    /// 悬停提示：血仇关键词词条（玩家可在遗物描述里点「血仇」看机制）+ 荡平万邦卡牌预览。
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WandiModKeywords.Vengeance),
        HoverTipFactory.FromCard<ConquerAllLands>(),
    ];

    // 遗物角标计数器：显示剩余免死次数（参考 PenNib.ShowCounter + DisplayAmount）
    public override bool ShowCounter => !IsUsedUp;
    public override int DisplayAmount => Charges;

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
    /// 先古（觉醒）替换入口：欧洛巴斯之触拾起时，游戏据此把本遗物替换为「不灭王血」。
    /// （BaseLib 的 StarterUpgradePatches 已让 CustomRelicModel 走此方法，无需自建先古之民。）
    /// </summary>
    public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<UndyingRoyalBlood>();
}
