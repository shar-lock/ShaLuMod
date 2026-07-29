# 开发记录

> **规范：每次开发完毕后更新本文件**（AI 必须做）。格式：日期 → 做了什么 → commit hash。
> AI 开发前先读 [TODO.md](TODO.md)（待办）和 [spec/](../spec/)（开发约束）。

---

## 2026-07-29

### M2 核心机制 + 关键词高亮 + spec 规范（`531ea10`）
- 血仇 Power（VengeancePower）：失血叠层 + +2%/层攻击伤害放大 + 满 7 触发荡平万邦
- 弑亲血脉遗物（BloodOfTheKinslayer）：战斗开始赋予血仇 + 4 次免死（套 LizardTail）
- 不灭王血觉醒遗物（UndyingRoyalBlood）：每回合 +1 血仇 + GetUpgradeReplacement
- 荡平万邦机制卡（ConquerAllLands）：0 费 AoE + MissingHp% + 敌 MaxHp%
- 关键词高亮改内联：AutoKeywordPosition.None + [gold] 包裹
- spec/ 开发约束（7 个 md）+ CLAUDE.md 声明必读

### M3 普通卡 + 罕见卡 + 目录重组（`9dcaa12`）
- 8 张普通卡（打击/连刺/绝命突/饮血枪/守誓/涅槃/嗜血/复仇心）+ 起手 Strike 接线
- 38 张罕见卡（17 攻击 + 15 技能 + 6 能力 + 3 联机）+ 7 个独立 Power 类
- 卡牌目录按 稀有度/类型 重组（Basic/Common/Uncommon/Token），.uid 同步
- 全部本地化（zhs+eng，关键词 [gold] 高亮）
- spec/assets.md 图片注册规范
- 9 项运行时 TODO 记录到 log/TODO.md

### 此前（用户另一台机器）
- M1 脚手架（工程创建 + 角色模型 + 占位立绘）
- 纷争（Strife）机�����临时最大生命上限 + Grant + AfterCombatEnd 还原
- M2 普通卡批次（12 张：御敌/蓄势/坚壁/誓约之枪 等）
- 构建链路打通（Rider + .NET SDK + MegaDot）
