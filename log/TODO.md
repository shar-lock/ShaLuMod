# 待办（TODO）

> 运行时确认 / 未完成项。**完成后标 ✅**。

## 全卡牌开发状态

| 批次 | 数量 | 状态 |
|---|---|---|
| 机制卡 | 1 | ✅ |
| 普通卡 | 20 | ✅ |
| 罕见卡 | 38 | ✅（3 项可接受简化） |
| 稀有卡 | 27 | ✅ |
| 先古卡 | 2 | ✅ |
| **合计** | **88** | **全部完成** |

## 运行时待确认

| 文件 | 问题 | 状态 |
|---|---|---|
| ReaperSpear | 设计稿「HP≤50%费用变0」简化为固定1费+加伤 | 可接受简化 |
| GoldenBastion | 设计稿「下回合+X力量」简化为本回合力量 | // TODO 标注 |
| HolyBloodBaptism | 移除自身全部减益 API | // TODO 标注 |
| Bodyguard | 追踪本回合失血→给队友纷争 | 简化为直接给纷争 |
| BloodDrinkCounter | 精确吸血 | 简化为基础伤害×% |
| BloodCleanse | 移除减益 | 简化为只给纷争 |
| GuardianPact | 联机伤害转移 | 需自定义 Power |
| 浴血奋战 Bloodbath | 荡平万邦单体×1.5 | 需改 ConquerAllLands.OnPlay 加检查 |
| 血仇主宰 VengeanceDominion | 各消耗血仇卡需加不消耗检查 | // TODO 标注 |

## M2 遗留

| 问题 | 说明 |
|---|---|
| 弑亲血脉充能角标 | counter 显示 API 待确认 |

## 其他

| 问题 | 说明 |
|---|---|
| 10+ 新 Power 类本地化 | powers.json 需补条目（Rider Generate） |
| 本机 images 目录缺失 | .gitignore 忽略 png |
