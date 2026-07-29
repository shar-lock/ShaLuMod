# 核心机制：血仇 + 纷争

万敌双核 Power。所有钩子已反编译确认（`_src/sts2_src/`）。

## 血仇（Vengeance）— `VengeancePower`（Buff + Counter）
万敌的核心计数器 + 伤害引擎，由起手遗物「弑亲血脉」战斗开始赋予。

| 机制 | 钩子 | 参考原生 | 要点 |
|---|---|---|---|
| 失血叠层 | `AfterDamageReceived(ctx,target,result,props,dealer,cardSource)` | RupturePower | `target==Owner && result.UnblockedDamage>0` → `PowerCmd.Apply<VengeancePower>(ctx,Owner,1,...)`；每事件 +1 |
| 伤害放大 | `ModifyDamageMultiplicative(target,amount,props,dealer,cardSource,cardPlay)` | WeakPower | `dealer==Owner && props.IsPoweredAttack()` → `1m + 0.02m*Amount` |
| 满 7 触发 | `AfterPowerAmountChanged(ctx,power,amount,applier,cardSource)` | OutbreakPower | `power==this && amount>0 && Amount>=7` → 消耗 7 层 + 扣 5% 当前血 + 生成荡平万邦 |
| 卡牌授予入口 | `static Grant(ctx,target,amount,cardSource)` | — | 卡面「获得 X 血仇」走这个；带守卫 + 日志 |

> **满 7 触发要点**：消耗 7 用 `PowerCmd.Apply<-7>`（amount<0 会被 `amount>0` 过滤，避免递归）；扣血用 `CreatureCmd.Damage(self, Unblockable|Unpowered|Move)`（会触发失血叠层 +1，设计内可接受）；生成荡平万邦用 `CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, player)`；觉醒时（`Owner.Player.GetRelic<UndyingRoyalBlood>()!=null`）给升级版（升级 API 待确认）。

## 纷争（Strife）— `StrifePower`（Buff + Counter）
万敌的**临时最大生命上限**（替代格挡的防御机制）。层数 = 临时上限总量。

- `static Grant(ctx, target, amount, applier, cardSource)`（唯一入口）：
  1. **clamp**：积累上限 = 基础最大生命（当前上限 − 已有纷争），即上限最多翻倍。
  2. `CreatureCmd.SetMaxHp(target, MaxHp+gain)` 抬上限（**不用 GainMaxHp**——会污染 `MaxHpGained` 永久统计）。
  3. `CreatureCmd.Heal(target, gain)` 等量回血（**必须先抬后回**，反了会被旧上限截断）。
  4. `PowerCmd.Apply<StrifePower>` 叠层展示。
- `AfterCombatEnd(room)`：`SetMaxHp(Owner, MaxHp - Amount)` 还原（永久提升保留），当前血超出则截断。

> **设计原则**：临时 vs 永久最大生命严格分通道（`SetMaxHp` vs `GainMaxHp`）。
