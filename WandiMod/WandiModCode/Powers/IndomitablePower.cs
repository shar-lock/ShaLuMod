using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（回血）
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.Rooms;                    // CombatRoom

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 不屈 / Indomitable（万敌 · 能力 Power）。
/// 机制：战斗结束时回复生命上限 5% 的生命值（升级 7%）。
///   - 钩子 AfterCombatEnd：回复 Owner.MaxHp * Amount / 100（Amount = 5 或 7，由卡牌按升级态传入）。
/// 参考 StrifePower.AfterCombatEnd（同钩子做上限还原）。
/// 注意：AfterCombatEnd 在胜负都触发；CreatureCmd.Heal 修改 Creature.CurrentHp。
///   // TODO: 运行时确认——战斗结束后回血是否持久化到地图（Creature 是否跨战斗延续）；
///   //       若不持久化，需改走 run 级回血 API（参考原生「燃烧之血」类遗物的战后回血机制）。
/// </summary>
public class IndomitablePower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态类，不叠加

    public override async Task AfterCombatEnd(CombatRoom room)
    {
        if (Owner == null)
        {
            MainFile.Logger.Error("[不屈] AfterCombatEnd 时 Owner 为空，无法回血");
            return;
        }

        // 回复生命上限的 Amount%（向下取整，至少 1）
        decimal heal = Math.Max(1m, Owner.MaxHp * Amount / 100m);
        Flash();
        await CreatureCmd.Heal(Owner, heal);

        MainFile.Logger.Info($"[不屈] 战斗结束回复生命：上限 {Owner.MaxHp} 的 {Amount}% = {heal}");
    }
}
