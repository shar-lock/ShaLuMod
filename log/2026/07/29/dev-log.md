# 开发记录 2026-07-29

> **规范**：每次开发完毕后，在 `log/YYYY/MM/DD/dev-log.md` 追加记录。
> AI 开发前先读 [TODO.md](../../TODO.md) 和最近一次 dev-log。

---

### M2 核心机制 + 关键词高亮 + spec 规范（`531ea10`）
- 血仇 Power（VengeancePower）：失血叠层 + +2%/层攻击伤害放大 + 满 8 触发荡平万邦
- 弑亲血脉遗物（BloodOfTheKinslayer）：战斗开始赋予血仇 + 4 次免死（套 LizardTail）
- 不灭王血觉醒遗物（UndyingRoyalBlood）：每回合 +1 血仇 + GetUpgradeReplacement
- 荡平万邦机制卡（ConquerAllLands）：0 费 AoE + MissingHp%
- 关键词高亮改内联：AutoKeywordPosition.None + [gold] 包裹
- spec/ 开发约束（7 个 md）+ CLAUDE.md 声明必读

### M3 普通卡 + 罕见卡 + 目录重组（`9dcaa12`）
- 8 张普通卡 + 38 张罕见卡（17 攻击 + 15 技能 + 6 能力 + 3 联机）+ 7 个独立 Power 类
- 卡牌目录按 稀有度/类型 重组（Basic/Common/Uncommon/Token），.uid 同步
- 全部本地化（zhs+eng，关键词 [gold] 高亮）
- spec/assets.md 图片注册规范

### 修复 6 个罕见卡 TODO + log 规范（`dbf5c04`）
- FatalThrust: FatalThrustPower : TemporaryStrengthPower
- LastStand/BloodPact: PileType.Hand.GetPile(Owner).Cards
- Tenacity: CardSelectCmd.FromHand + CardCmd.Exhaust
- SharedFury/KingsBlessing: 已正确实现 GetTeammatesOf

### 用户另一台机器提交（`7c1b72e`）
- 血仇 8 层保底防移除 + 荡平万邦 CalculatedDamageVar 重订
- WithUpgradeTo 全库整改（WithUpgrade 增量→目标值）
- 10 个 Power 图标落地
- 编译修复 + [Pool(TokenCardPool)] + 觉醒版荡平万邦

### 此前（用户另一台机器）
- M1 脚手架（工程创建 + 角色模型 + 占位立绘）
- 纷争（Strife）机制：临时最大生命上限 + Grant + AfterCombatEnd 还原
- M2 普通卡批次（12 张：御敌/蓄势/坚壁/誓约之枪 等）
- 构建链路打通（Rider + .NET SDK + MegaDot）
