# 开发记录 2026-07-30

---

### M3 稀有卡 + 先古卡全量实装

- 27 张稀有卡（10 攻击 + 9 技能 + 8 能力 + 2 联机）
- 2 张先古卡：弑神登神（ITomeCard）+ 血祭·诛王枪（BloodriteStrike ITranscendenceCard）
- 10 个新 Power 类
- 全部本地化（zhs+eng cards.json，176 条）
- BloodriteStrike 修改：加 ITranscendenceCard → GetTranscendenceTransformedCard
- 9 项简化/TODO 标注（运行时确认后修复）
- **全部 88 张卡牌代码开发完成**（commit `cafe518`）

---

### 本机跟进：拉取 M3 后修复编译 + 乱码 + 逻辑漏洞

**编码乱码修复（另一台机器编辑器 GBK 污染）**
- 6 文件被写入烘焙的 U+FFFD：BloodDrink / ReaperSpear / SharedFury / SweepingArmy / TitanBody 注释 + zhs/cards.json 力敌万邦行（→ 游戏内会显示 ``）。
- 已按行号重建文本，全仓 WandiMod/doc/spec/log 复扫 0 残留。
- **预防：另一台机器编辑器务必将默认编码设为 UTF-8**（VS Code `files.encoding`，Rider File Encodings）。

**编译修复（HEAD 在本机 0 错误通过，此前 11 个错误）**
- 3 个 Power 全限定名写错命名空间：`GameActions.Multiplayer.DamageResult` → `DamageResult`（正确命名空间 `Entities.Creatures`），补 `using Models`（CardModel）。
- RivalAllLandsPower：`Combat.CattleSide` 不存在 → `CombatSide`。
- BloodriteStrike：缺 `using MegaCrit.Sts2.Core.Commands`（CardCmd）。
- DoomVerdict：击杀判定误用 —— `Execute` 返回 `AttackCommand` 本体，正确写法 `.Results.SelectMany(r => r).Any(r => r.WasTargetKilled)`（原生 Feed/KnockoutBlow 范式）。
- ThroneOfBonescorchingHeaven / ReaperSpear：`AttackCommand` 无 `WithValueProp` API，`DamageProps` 默认即 `ValueProp.Move`，删除多余调用。
- 补齐 12 个新 Power 的 powers.json 本地化（eng+zhs 各 12 条，此前 STS001 阻断构建）。

**逻辑漏洞修复**
- FrenzyPower：`_drawnThisTurn` 从不重置 → 首回合后狂化永久失效。补 AfterSideTurnStart 重置（代码里留有 TODO）。
- DeathDenialPower：免死既不消耗也不移除 → 打出即永久免死。补：免死触发后 Remove 自身；下一次我方回合开始过期移除（保护窗口=本回合剩余+敌方阶段）。

**部署**：`dotnet publish` 已通过，dll+pck 已落地游戏。

---

### 血仇核心机制优化（2 项）

**① 血仇消耗卡条件打出（GrandFinale 式）**
- 固定血仇消耗卡只有 **复仇心 VengefulHeart**、**涅槃 Nirvana** 两张（声明 `new IntVar("BloodCost", N)`）。
- 在 `WandiModCard` 基类统一接管（参考原生 PactsEnd/GrandFinale）：
  - `IsPlayable`：血仇不足（消耗后跌破保底 1 层，即 `blood <= BloodCost`）→ 灰显不可打出（不会白花能量）；充足 → 可打出。
  - `ShouldGlowGoldInternal`：血仇充足时金边高亮（仅战斗内，避免牌库误亮）。
  - 条件 = `GetPowerAmount<VengeancePower>() > BloodCost`（留 1 层保底）。
- 两张卡自身零改动（基类通过 `HasBloodCost` 自动识别 BloodCost 变量）。
- 决策点：采用 GrandFinale 式（灰显不可打出）而非 PactsEnd 式（始终可打出+fizzle），因为血仇卡有 1-2 费，灰显更省心。

**② 血仇 UI 显示 = 真实层数 - 1**
- `VengeancePower.DisplayAmount => Max(0, Amount-1)`：1 层（保底）→ 显示 0、2 层→显示 1、8 层→显示 7。
- `ModifyDamageMultiplicative` 改用有效层数（Amount-1）：1 层 = 0% 增伤（保底层无效果），与显示值一致。
- 触发阈值不变（真实 8 层 = 显示 7 层触发荡平万邦，符合「7 层触发」）。
- **1 层显示空白**：原生 NPower.RefreshAmount 对 Counter 型写死显示 `DisplayAmount.ToString()`（=0 时显示 "0"）。加 `VengeancePowerDisplayPatch`（Harmony Postfix on NPower.RefreshAmount），血仇显示值≤0 时清空标签 → 真正「不展示任何数字」。

**读血仇层数算伤卡（已决定，不改）**：狂怒/噬仇/暴风连击/噬魂/诛天焚骨的王座仍读真实 Amount（不砍数值）。隐藏的保底 1 层对这些伤害卡仍算数——作为兜底机制保留（显示值 Amount-1 与它们结算用的真实 Amount 差 1，是有意为之）。

---

### 血仇触发机制：成长型（消耗 4 + 抬高基准线，非固定阈值）

- **设计**：原本「满 8 清空到保底 1」（振荡，无成长）。改为**成长型**——每累积 7 层消耗 4 层（不清空），触发基准线逐次抬高 → 血仇逐轮成长 +3，越打越高增伤。
- **成长循环**（玩家视角 / 真实层）：触发点 8→11→14...（视角 7→10→13...），消耗后留 4→7→10...（视角 3→6→9...），每次再累积 7 层（含 5% 自伤反馈 +1）。例：8 层触发 → 消耗 4 → 4 层 → 自伤反馈 +1 → 5 层；之后每次受击 +1，到 11 层再次触发 → 消耗 4 → 7 层 → 反馈 → 8 层；以此类推。
- **代码**（`VengeancePower`）：删除固定 `TriggerThreshold`/`FloorAfterTrigger`，改为 `GainPerTrigger=7` + `ConsumeOnTrigger=4` + 战斗实例字段 `_lastTriggerBase=1`。触发条件 `Amount >= _lastTriggerBase + GainPerTrigger`；触发后 `_lastTriggerBase = Amount`（消耗后的值）。日志输出消耗/当前/下次基准。
- **为何用实例字段而非 [SavedProperty]**：基准线是战斗内、每战重置的语义（新战 Power 重建 → 重置 1）；[SavedProperty] 偏 run 级、可能跨战残留。且 `AfterPowerAmountChanged` 只在层数变化时触发、存档恢复不触发，重载不会误触发。
- **顺带修复**：`VengeancePower.cs` 第27行「能力」的「力」字是 GBK 污染的 3×U+FFFD，已用 Python 代码点级替换修复。
- **安全性**：触发留 ≥ 基准+3（远高于 0）、消耗卡由 IsPlayable 门控保底 ≥1，Amount 始终 ≥1，不会触发 ShouldRemoveDueToAmount 自动移除。
- **文档同步**：`spec/core-mechanics.md`（成长触发行 + 要点）、`doc/万敌Mod-设计方案.md` 4.3（成长循环 + 显示说明）。其余文档只提「血仇≥8 生成荡平万邦」（首触发阈值未变），无需改。


---

### 全面代码审计 + P0 修复 + 文档同步

**3 路并行审计**（本地化 / 注册生命周期 / 设计一致性）+ 直接核查，结论：
- ✅ 本地化完整（92 卡 / 21 Power / 5 遗物，eng+zhs 齐全）、[Pool] 全覆盖、联机卡 5 张、先古卡接口、Power 归零风险（除下方 P0）、起手接线、卡牌 1:1 覆盖。

**P0 修复**：5 张能力卡 `PowerCmd.Apply<...>(..., 0, ...)` → `1`。
- 根因：`PowerCmd.Apply` 在 `amount==0` 时直接 bail 不附着（PowerCmd.cs:84），整张卡 no-op。
- 涉及：弑神登神 / 力敌万邦 / 血仇主宰 / 毁灭意志 / 死亡拒绝。对齐原生 Barricade（标记/形态型 Power 传 1）。

**文档全面同步**（文档 ↔ 代码一致）：
- 力敌万邦：遵从卡牌设计稿（回合+2血仇+抽牌），移除遗物/设计方案里虚构的「+25% 强化态」。
- 遗物设计.md：旧触发（积累8减7保底1）→ 成长型（累积7消耗4）；对局外掉血桥接标注「未实装」；StrifePower `entryMaxHp` → 动态 clamp（`MaxHp−2×当前层`）；counter API `SetCounter/ChangeCounter` → `ShowCounter+DisplayAmount+InvokeDisplayAmountChanged`；「待核实项」→「已确认项」。
- 设计方案.md §五草案表：万死无悔 20%→40/50、灾厄之矛 50%→70/80、御敌 5→4/6 纷争；力敌万邦去强化态。
- powers.json：删冗余死键 `WANDIMOD-BLOOD_DRINK_COUNTER_POWER`（eng+zhs 各 3 键）。

**遗留**（写入 TODO）：P0 起手 Strike 接线；P1 简化（GodslayerAscension per-hit 回血、对局外掉血）；P2 待验证（Indomitable 战后回血持久化、GuardianPact 联机时序、2 Harmony 补丁更新回归）。

---

### 本机审查：拉取 8 个新提交后的复核与补漏

**审查结论**：血仇成长型触发/显示补丁/BloodCost 门控/死亡拒绝重做/饮血反击简化/4 新普通卡/3 新遗物/P0 修复/主题配色 —— 设计与实现均核对通过（Harmony 补丁目标 `NPower.RefreshAmount`/`_amountLabel`/`CreatureCmd.Heal(creature,amount)` 已对反编译源码逐一验证）。

**补漏 4 处（另一台机器再次未构建即提交）**：
- `BloodiedSpearhead.cs` 缺 `using MegaCrit.Sts2.Core.Models`（CardModel）——编译错误。
- 3 件新遗物缺 `.flavor` 风味文本（STS001 阻断构建）——eng+zhs 补齐。
- `powers.json` DEATH_DENIAL_POWER 文案残留旧「免死」语义（重做后应为吸血）——eng+zhs 更新。
- `BloodDrinkCounterPower.cs.uid` 孤儿文件（.cs 已删）——删除。

**部署**：修复后 `dotnet publish` 通过，已落地游戏。

---

### 全面审查（二轮）+ 设计稿对齐修复

**审查范围**：全代码（92 卡 / 5 遗物 / 21 Power）× 三份设计文档 × TODO × 素材目录。

**新发现与处理**：
1. **4 张卡设计稿↔代码不一致**（另一台机器在 36332fd/9004b9e 只改了 doc 没改 code）——用户决定代码对齐设计稿：
   - 巨灵之躯 5/8 → **15/18** 纷争；横扫 8/12 → **7/10**；血潮 8/12 → **9/13**
   - 净血重做：移除减益+纷争 → **选择消耗一张手牌 + 7/13 纷争**（Scavenge 范式：`CardSelectCmd.FromHand(ExhaustSelectionPrompt)` + `CardCmd.Exhaust`），关键词加 Exhaust，本地化 eng/zhs 同步
2. **起手 Strike「未接线」P0 系误报**：嵌套命名空间解析规则下 `Cards.Strike` 本就指向 `WandiMod.WandiModCode.Cards.Strike`（嵌套命名空间优先于 using）。已显式化引用消除歧义，TODO 关闭。游戏内确认血红牌框即可彻底归档。
3. **双机素材隐患**：images/ 被 gitignore，另一台机器无素材，在那边 publish 会丢全部图片。**发布约定：只有本机执行 `dotnet publish`，另一台机器只写代码**（已记入 TODO）。
4. TODO.md 卡牌总数 88 → **92**（补基本卡 4 行）。

**结论**：代码层无大规模待开发项；剩余为 P1 机制补充（弑神登神 per-hit 回血、弑亲血脉对局外掉血桥接）+ 游戏内测试 + 美术素材（92 卡立绘 / 11 Power 图标 / 5 遗物图 / 战斗形象 / 能量计数器）。

**部署**：`dotnet publish` 通过（0 错误，仅 3 CS8604 + 1 已知 STS003 警告），dll/pck 已落地游戏（19:37）。
