# 万敌（Mydei）角色 Mod 设计方案

> **项目**：Slay the Spire 2 角色模组 · 参考崩坏：星穹铁道「万敌」  
> **分支**：`wandi`（血仇 + 免死 双核流派；其余机制流派留作其他分支实验）  
> **技术基线**：BaseLib `3.3.0` / `min_game_version 0.107.0` / Godot(MegaDot) `4.5`  
> **状态**：方向已定稿，进入 M1 脚手架阶段

---

## 一、项目概述

把星铁「万敌」（毁灭命途 · 虚数属性 · 五星）的战斗风味移植为 StS2 的原创角色模组。核心体验：**血量即资源**——通过烧血叠「血仇」计数，换取爆发伤害；靠天赋级「免死」兜底，敢于刀尖舔血。

> ⚠️ 数值均为**待平衡的起点**，需进游戏后用 dev console 迭代。

---

## 二、技术基线（先要了解的约束）

| 主题 | 现状 / 约束 |
|---|---|
| **角色动画** | StS2 角色动画用 **Spine**，**无原生 Live2D**。自定义角色视觉走 BaseLib [`NCreatureVisuals`](https://alchyr.github.io/BaseLib-Wiki/docs/scenes/creature-visuals.html)：静态 PNG / Godot `AnimationPlayer` / Spine 三选一。 |
| **工程位置** | 本仓库是**模板分发仓库**（`content/` 下的三个模板会被打包成 NuGet 包）。万敌 mod 在仓库根的**独立工程目录 `WandiMod/`** 开发，不污染模板。 |
| **构建/测试** | 模板代码**不能在本仓库直接 `dotnet build`**——需本机 StS2 安装 + BaseLib 还原。本仓库只产出代码/资产，**构建与游戏内测试由本机完成**。 |
| **实现栈** | 卡牌 `CustomCardModel` / 能力 `CustomPowerModel` / 遗物 `CustomRelicModel`，行为用流式 Command API（`DamageCmd` / `PowerCmd` / `CardCmd` / `CardPileCmd` / `CardSelectCmd`）。 |
| **本地化** | `WandiMod/localization/eng/*.json`（`cards`/`relics`/`powers`/`ancients`/`characters`/`card_keywords`/`static_hover_tips`）。 |
| **美术** | `WandiMod/images/{card_portraits,powers,relics,charui}/`，卡面 `1000×760`（全艺术 `606×852`），缺失自动回退占位图。 |

---

## 三、设计支柱（万敌 → StS2）

| 万敌原作机制 | StS2 落地方向 |
|---|---|
| 毁灭命途 / 烧血输出 | 自伤换爆发的攻击牌（HP 即资源） |
| 生命值倍率伤害 | 伤害/格挡按「最大生命」或「已损生命」缩放 |
| 【血仇】充能状态 | **核心计数型 Power**：损血/受击叠层，达阈值进入强化态 |
| 天赋 4 次免死 | **起手遗物**：锁血免死 N 次（角色安全网） |
| 扩散（单体 + 邻位） | StS2 无邻位 →「单体重击 + 全体溅射」或多段近似 |
| 终结技「诛天焚骨的王座」 | 稀有大牌：吞层数打巨额伤害 + 回血 |
| 虚数 / 金色 | gdshader 金色能量 VFX + 枪击命中特效 |

---

## 四、核心机制：血仇 + 免死 双核

### 4.1 血仇（Vengeance）—— 计数型 Power

- `PowerType = Buff`，`PowerStackType = Counter`
- **触发**：每当「失去生命或受击」→ `+1` 层
- **效果**：层数同时是①伤害放大器、②部分卡牌的资源（如连突消耗层数增伤）
- **血仇态**：层数 ≥ 阈值时进入强化态（额外力量/吸血），致敬星铁「血仇状态」
- **实现要点**：需钩住「受击/损血」事件触发叠层（反编译确认正确钩子，可能落在 `AfterPowerAmountChanged` 之外的伤害回调）；命令走 `PowerCmd.Apply / ModifyAmount`

### 4.2 弑亲血脉 —— 起手遗物（4 次免死）

- **效果**：受到致命伤害时，改为回复到 `30%` 最大生命，消耗 1 次充能（共 4 次）
- **定位**：整套烧血玩法的兜底，也是角色灵魂
- **实现要点**：钩住伤害结算/死亡事件，是**高级项**（可能需 Harmony），**M2 单独做可行性验证**，失败则降级为「每场战斗首次免死」等简化版

> 这两个机制让万敌**玩起来像万敌**（毁灭 + 不死），而不是又一个数值怪。

---

## 五、卡牌设计（稀有度 / 种类 / 数值草案）

> StS2 稀有度：Basic / Common / Uncommon / Rare。数值为起点，待平衡。

| 卡牌 | 稀有度 | 类型 | 费用 | 方向（草案） |
|---|---|---|---|---|
| 横扫突击（替代 Strike） | Basic | 攻击 | 1 | 6 伤；血仇态下多打 1 次目标 |
| 御阵（替代 Defend） | Basic | 技能 | 1 | 5 格挡；本回合若损过血 +2 格挡 |
| **血祭** | Common | 攻击 | 1 | 自伤 3，造成 9 伤（烧血基础牌） |
| 溅血枪 | Common | 攻击 | 1 | 5 伤单体 + 2 伤溅射全体（扩散近似） |
| 蓄仇 | Common | 技能 | 0 | 获得 2 层血仇（已损血则抽 1 牌） |
| **万死无悔** ⭐ | Uncommon | 攻击 | 2 | 消耗当前生命 50%（封顶），打「最大生命×0.4」+溅射（致敬战技） |
| 涅槃 | Uncommon | 技能 | 1 | 回复 8 生命；耗尽时再回 4 |
| **血仇（形态牌）** | Uncommon | 能力 | 2 | 获得 Power：受击/回合开始叠血仇；≥阈值进强化态 |
| 弑王枪·连突 | Uncommon | 攻击 | 2 | 3 段 ×4 伤，每段消耗 1 层血仇 +2 伤 |
| **诛天焚骨的王座** ⭐ | Rare | 攻击 | 3 | 吞噬所有血仇，每层 +8 伤打全体，回复造成伤害的 20% 生命（终结技） |
| 不死王权 | Rare | 能力 | 3 | Power：每回合首次致死免死并回血（遗物外的第二层保险） |
| 灾厄之矛 | Rare | 攻击 | 2 | 耗尽，伤害 =「已损失生命」的一半（越残血越强） |

**起手牌组**：5 × 横扫突击 + 4 × 御阵 + 1 × 血祭。  
**起手遗物**：弑亲血脉（免死 ×4）。

---

## 六、能力 / 遗物

- **Powers**：`血仇`（计数）、`不死王权`（每回合免死）
- **Relics**：`弑亲血脉`（起手，4 次免死）；稀有遗物待定（候选：击杀回血/叠血仇、低血量加力量）

---

## 七、视觉 / 动画 / VFX（分三层）

| 层级 | 方案 | 工作量 | 说明 |
|---|---|---|---|
| **L1 MVP** | 静态立绘 PNG | 低 | `CreateFromResource`，1 张图即可进战斗。**当前阶段先行** |
| **L2 动起来** | Godot `AnimationPlayer` 序列帧 / 2D 骨骼 | 中 | 自动接入 `idle/attack/hurt/die/cast`。最贴「Live2D 感」且零成本的路线 |
| **L3 官方级** | Spine（自制 / Harmony 换皮） | 高 | 编辑器约 $400；远期可选目标 |

**VFX**：金色能量用 `gdshader`（`canvas_item` + `ShaderMaterial`），命中走 `WithHitFx("vfx/...")`，终结技做全屏着色（`CanvasLayer` + `ColorRect`）。

---

## 八、平衡原则

**高风险高收益**：血量即资源，越残血越强，爆发极高；容错由「免死」撑。避免做成无脑强——每次烧血都应是**有代价的决策**。

---

## 九、落地里程碑

1. **M1 脚手架**：`WandiMod/` 工程（复制 CharacterModTemplate 并改名 `CharMod → WandiMod`），改清单/角色类/占位立绘，确保能被游戏加载出空角色。
2. **M2 核心机制**：实现「血仇」Power +「弑亲血脉」遗物（**重点验证免死钩子可行性**）。
3. **M3 卡牌组**：Basic → Common → Uncommon → Rare 逐档实装 + 本地化（`cards.json`）。
4. **M4 视觉**：立绘替换 →（可选）序列帧动画 → VFX shader。
5. **M5 平衡/测试**：dev console 实测、数值迭代、（可选）联机 `-fastmp` 测试。

---

## 十、决策记录（本次方向定稿）

| 维度 | 定案 |
|---|---|
| 核心机制 | 血仇（计数 Power）+ 弑亲血脉（起手遗物·4 次免死）双核 |
| 平衡风格 | 高风险高收益 |
| 动画 | L1 静态立绘 PNG 先行 |
| 美术 | 占位图起步（模板自带 + 角色立绘占位） |
| 分支策略 | `wandi` 做血仇+免死；其他流派留作其他分支 |

---

## 十一、参考资料

- 万敌角色资料：[BWIKI 万敌](https://wiki.biligame.com/sr/%E4%B8%87%E6%95%8C) · [游民星空技能介绍](https://www.gamersky.com/handbook/202501/1871341.shtml) · [TapTap 攻略](https://www.taptap.cn/app/224267/strategy/entity-collection/267470)
- BaseLib 角色视觉：[Creature Visuals](https://alchyr.github.io/BaseLib-Wiki/docs/scenes/creature-visuals.html)
- 模板工作流：`ModTemplate-StS2.wiki/`（Setup / Modding-Basics / Common-Commands-Cookbook / Adding-Cards / Shaders / Testing-and-Debugging）
