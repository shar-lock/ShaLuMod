using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 荡平进度 / Conquer Progress —— 独立计数器，状态栏右下角数字展示进度（与血仇同款 Counter 渲染）。
/// 显示数字 = 自上次触发基准起已累积的血仇层数（0～7）；满 7 时由 VengeancePower 消耗并生成卡牌后归零。
/// StackType 必须是 Counter：NPower.RefreshAmount 仅对 Counter 写 DisplayAmount，Single 永远空白。
/// Amount 固定为 1（维持实例、避免进度 0 时被 ShouldRemoveDueToAmount 移除）；真实进度走 DisplayAmount。
/// 由 BloodOfTheKinslayer 战斗开始赋予；VengeancePower 在层数变化后 SyncProgress。
/// </summary>
public class ConquerProgressPower : WandiModPower
{
    /// <summary>生成一张荡平万邦所需累积的血仇层数（与 VengeancePower.GainPerTrigger 一致）。</summary>
    public const int Threshold = 7;

    private int _progress;

    public override PowerType Type => PowerType.Buff;
    // 必须 Counter：原生 NPower 仅对 Counter 渲染右下角数字（Single 标签恒为空）
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>状态栏右下角数字 = 当前进度（0～7）。</summary>
    public override int DisplayAmount => _progress;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Threshold", Threshold),
        new IntVar("Progress", 0),
    ];

    /// <summary>
    /// 按血仇真实层数与触发基准同步进度显示。
    /// progress = clamp(血仇层数 − 上次触发基准, 0, Threshold)。
    /// </summary>
    public void SyncProgress(int vengeanceAmount, int lastTriggerBase)
    {
        int next = Math.Clamp(vengeanceAmount - lastTriggerBase, 0, Threshold);
        if (next == _progress)
            return;
        _progress = next;
        DynamicVars["Progress"].BaseValue = next;
        InvokeDisplayAmountChanged();
    }
}
