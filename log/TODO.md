# 待办（TODO）

> 运行时确认 / 未完成项。**完成后标 ✅**。

## 全卡牌开发状态

| 批次 | 数量 | 状态 |
|---|---|---|
| 机制卡 | 1 | ✅ |
| 普通卡 | 20 | ✅ |
| 罕见卡 | 38 | ✅ |
| 稀有卡 | 27 | ✅ |
| 先古卡 | 2 | ✅ |
| **合计** | **88** | **全部完成** |

## TODO 机制修复状态（9 项 → 7✅ + 2 接受简化）

| 文件 | 问题 | 状态 |
|---|---|---|
| VengeanceDominionPower | 阻止血仇消耗 | ✅ TryModifyPowerAmountReceived 全局拦截（ArtifactPower 范式） |
| BloodOfTheKinslayer | 充能角标 | ✅ ShowCounter + DisplayAmount + InvokeDisplayAmountChanged（PenNib 范式） |
| HolyBloodBaptism | 移除全部减益 | ✅ Creature.Powers.Where(Debuff) + PowerCmd.Remove（Misery 范式） |
| BloodCleanse | 移除 1 个减益 | ✅ FirstOrDefault(Debuff) + PowerCmd.Remove |
| GoldenBastion | 下回合力量 | ✅ GoldenBastionNextTurnPower（AfterSideTurnStart + Remove，DrawCardsNextTurnPower 范式） |
| BloodDrinkCounter | 精确吸血 | ✅ BloodDrinkCounterPower（AfterAttack 汇总 UnblockedDamage，SuckPower 范式） |
| ConquerAllLands | 浴血奋战单体加成 | ✅ WithMultiplier lambda 检查 BloodbathPower + enemyCount==1 |
| GuardianPactPower | 联机伤害转移 | ✅ 已确认正确（ModifyDamageMultiplicative 全局分发），清理 TODO 注释 |
| ReaperSpear | HP≤50% 费用变 0 | 接受简化：StS2 无单卡动态降费 API，固定 1 费 + 残血加伤 |
| Bodyguard | 追踪本回合失血 | 接受简化：用 MissingHp 近似（无原生 per-turn API） |

## 代码审查发现（07/30 全面审计）

### P0 — Bug / 未接线
| 项 | 状态 |
|---|---|
| ~~5 张能力卡 amount=0 授予 Power 不附着（弑神登神/力敌万邦/血仇主宰/毁灭意志/死亡拒绝）~~ | ✅ 已修复（0→1；PowerCmd.Apply 在 amount==0 时 bail 不附着） |
| 起手牌组打击位用原版 `Cards.Strike`，万敌专属 [Strike.cs](../WandiMod/WandiModCode/Cards/Basic/Attacks/Strike.cs) 未接线 | ⏸ 待修：改 `ModelDb.Card<WandiMod.WandiModCode.Cards.Strike>()` |

### P1 — 简化（偏离设计稿，能跑；9-项表里的 ReaperSpear/Bodyguard 不重复）
| 卡/Power | ��计 | 现状 |
|---|---|---|
| 弑神登神 GodslayerAscension | 攻击回血（每次攻击） | 每次命中回 2% MaxHp（AfterDamageReceived 是 per-hit，多段回多次） |
| 弑亲血脉 对局外掉血桥接 | 遗物暂存→下场战斗转化（上限 3） | 未实装（采用「忽略」兜底，核心循环不受影响） |

### P2 — 待运行时验证
| 项 | 说明 |
|---|---|
| IndomitablePower 战后回血 | AfterCombatEnd 的 Heal 是否持久化到地图（推测能——Strife 同钩子 SetMaxHp 还原疑似正常） |
| GuardianPactPower 联机转移 | 伤害转移 Side/IsPlayer 判定 + AfterSideTurnEnd 移除时序；`-fastmp` 联机测 |
| 2 个 Harmony 补丁 | CreatureCmd.Heal（焚血勋章）/ NPower.RefreshAmount（血仇显示），游戏更新后回归 |
| BloodOfTheKinslayer 开局血仇 | 试 Apply 传 0 是否建立实例（现传 1 保底，可接受现状） |

### 文档同步 ✅（07/30 已完成）
- 遗物设计.md：旧触发→成长型、移除「+25% 强化态」、对局外掉血标注未实装、StrifePower 动态 clamp、待核实→已确认
- 设计方案.md：力敌万邦去强化态、§五草案 3 处数值（万死无悔 40/50、灾厄之矛 70/80、御敌 4/6）
- powers.json：删冗余 BLOOD_DRINK_COUNTER_POWER 死键

## M2 遗留

| 问题 | 说明 | 状态 |
|---|---|---|
| ~~弑亲血脉充能角标~~ | ~~counter API~~ | ✅ ShowCounter + DisplayAmount + InvokeDisplayAmountChanged |

## 其他

| 问题 | 说明 |
|---|---|
| ~~14+ Power 类本地化~~ | ✅ 07/30 补齐 12 个新 Power（eng+zhs） |
| 本机 images 目录缺失 | .gitignore 忽略 png |
| FatalThrustPower STS003 | 继承原生 TemporaryStrengthPower 无 ID 前缀，警告容忍（显示走 OriginModel 卡牌） |
| ~~血仇读层算伤卡的层数口径~~ | ✅ 已决定保留：狂怒/噬仇/暴风连击/噬魂/诛天焚骨仍读真实 Amount（不砍数值），隐藏的保底 1 层对这些伤害卡仍算数，作为兜底机制 |
