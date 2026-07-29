# 待办（TODO）

> 运行时确认 / 未完成项。**完成后标 ✅**。
> AI 每次开发前读这里，优先处理高优先项。

## 罕见卡运行时确认

| 文件 | 问题 | 状态 |
|---|---|---|
| ~~FatalThrust~~ | 临时降力量 | ✅ FatalThrustPower : TemporaryStrengthPower（参考 PiercingWail） |
| ~~LastStand~~ | 手牌遍历 | ✅ PileType.Hand.GetPile(Owner).Cards（参考 BulletTime） |
| ~~BloodPact~~ | 手牌数 | ✅ 同上 .Count（参考 Anointed） |
| ~~Tenacity~~ | 卡牌选择消耗 | ✅ CardSelectCmd.FromHand + CardCmd.Exhaust（参考 Purity） |
| ~~SharedFury~~ | 联机队友力量 | ✅ 已正确实现 GetTeammatesOf（参考 Rally） |
| ~~KingsBlessing~~ | 联机队友格挡 | ✅ 同上 + GainBlock（参考 Rally） |
| BloodDrinkCounter | 精确吸血 | 可接受简化（基础伤害×%） |
| BloodCleanse | 移除减益 | 原版无直接 API，建议简化（只给纷争） |
| GuardianPact | 联机伤害转移 | 需自定义 Power（伤害修改 hook） |

## M2 遗留

| 问题 | 文件 | 说明 |
|---|---|---|
| 觉醒版荡平万邦 | VengeancePower 7 层触发处 | 升级态卡牌获取 API 待确认（ToUpgraded?） |
| 弑亲血脉充能角标 | BloodOfTheKinslayer.cs | counter 显示 API（SetCounter/ChangeCounter）待确认 |

## M3 待开发

| 批次 | 数量 | 状态 |
|---|---|---|
| 普通卡 | 20 | 已完成 |
| 罕见卡 | 38 | 已完成（3 项可接受简化/复杂处理） |
| 稀有卡 | 27 | 未开始 |
| 先古卡 | 2 | 未开始 |

## 其他

| 问题 | 说明 |
|---|---|
| 8 个独立 Power 类本地化 | FatalThrustPower 等 8 个需 powers.json 条目（Rider Generate） |
| 本机 images 目录缺失 | .gitignore 忽略 png，需从其他机器拷贝或创建占位图 |
