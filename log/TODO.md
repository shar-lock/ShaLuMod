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
