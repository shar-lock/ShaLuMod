# 核心机制：血仇 + 纷争

万敌双核 Power。所有钩子已反编译确认（`_src/sts2_src/`）。

## 血仇（Vengeance）— `VengeancePower`（Buff + Counter）
万敌的核心计数器 + 伤害引擎，由起手遗物「弑亲血脉」战斗开始赋予。

| 机制 | 钩子 | 参考原生 | 要点 |
|---|---|---|---|
| 失血叠层 | `AfterDamageReceived(ctx,target,result,props,dealer,cardSource)` | RupturePower | `target==Owner && result.UnblockedDamage>0` → `PowerCmd.Apply<VengeancePower>(ctx,Owner,1,...)`；每事件 +1 |
| 伤害放大 | `ModifyDamageMultiplicative(target,amount,props,dealer,cardSource,cardPlay)` | WeakPower | `dealer==Owner && props.IsPoweredAttack()` → `1m + 0.02m*Max(0,Amount-1)`（按**有效层=真实−1**，保底 1 层无增伤）|
| 显示数字 | `DisplayAmount`（override） | — | `Max(0, Amount-1)`：1层→0、2层→1、8层→7。1层=结构性储备（维持 Power 存活），不显示数字、0% 增伤 |
| 成长触发 | `AfterPowerAmountChanged(ctx,power,amount,applier,cardSource)` | OutbreakPower | `power==this && amount>0 && Amount>=_lastTriggerBase+7` → **消耗 4 层** + 生成荡平万邦（**不再扣 5% 血**）；基准 `_lastTriggerBase` 抬高 → 触发点 8→11→14...，血仇净成长 +3/轮 |
| 荡平进度 | `ConquerProgressPower`（独立 Buff + **Counter**） | NPower 仅 Counter 渲染右下角数字 | 战斗开始与血仇一并赋予；Amount 固定 1（防进度 0 移除），`DisplayAmount = clamp(血仇−基准,0,7)` |

| 卡牌授予入口 | `static Grant(ctx,target,amount,cardSource)` | — | 卡面「获得 X 血仇」走这个；带守卫 + 日志 |

> **玩家可见模型**：状态栏显示 = 真实层数 − 1（override `DisplayAmount`）。真实 1 层（储备）→ 显示为空 + 0% 增伤；满 8 层触发时玩家看到的是「7 层」。原生 Counter 型 Power 在 `DisplayAmount=0` 会显示 "0"，故加 Harmony 补丁 `VengeancePowerDisplayPatch`（Postfix on `NPower.RefreshAmount`，血仇显示值≤0 时清空标签）实现真正空白。
>
> **成长触发要点**：触发条件是「累积到 `_lastTriggerBase + 7`」（非固定阈值）。消耗目标 4 层：`consume = Min(4, Max(0, Amount-1))`——不够 4 层时只减到保底真实 1 层（玩家可见 0），绝不归零移除 Power。消耗用负 `PowerCmd.Apply`（amount<0 被 `amount>0` 过滤，避免递归）；**不清空**——消耗后把 `_lastTriggerBase` 更新为当前值，下次需再累积 7 层 → 触发点逐次抬高。**不再扣 5% 当前血**。生成荡平万邦用 `CardPileCmd.AddGeneratedCardToCombat`；觉醒时 `CardCmd.Upgrade`。独立 Power「荡平进度」（Counter + DisplayAmount）在状态栏右下角同步进度数字。
>
> **血仇消耗卡门控**（复仇心 / 涅槃 等带 `BloodCost` 变量的卡）：在 `WandiModCard` 基类 override `IsPlayable` / `ShouldGlowGoldInternal`——血仇不足（`GetPowerAmount<VengeancePower>() <= BloodCost`，即消耗后跌破保底 1 层）→ 灰显不可打出（不白花能量）；充足 → 金边高亮（GrandFinale 范式）。
>
> **不会归零**：触发留 ≥4 层、消耗卡由 IsPlayable 门控保底 ≥1，故 `Amount` 始终 ≥1，不会触发 `ShouldRemoveDueToAmount` 自动移除（移除会让掉血叠层钩子失效）。

## 纷争（Strife）— `StrifePower`（Buff + Counter）
万敌的**临时最大生命上限**（替代格挡的防御机制）。层数 = 临时上限总量。

- `static Grant(ctx, target, amount, applier, cardSource)`（唯一入口）：
  1. **clamp**：积累上限 = 基础最大生命（当前上限 − 已有纷争），即上限最多翻倍。
  2. `CreatureCmd.SetMaxHp(target, MaxHp+gain)` 抬上限（**不用 GainMaxHp**——会污染 `MaxHpGained` 永久统计）。
  3. `CreatureCmd.Heal(target, gain)` 等量回血（**必须先抬后回**，反了会被旧上限截断）。
  4. `PowerCmd.Apply<StrifePower>` 叠层展示。
- `AfterCombatEnd(room)`：`SetMaxHp(Owner, MaxHp - Amount)` 还原（永久提升保留），当前血超出则截断。

> **设计原则**：临时 vs 永久最大生命严格分通道（`SetMaxHp` vs `GainMaxHp`）。
