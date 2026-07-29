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

### 本机跟进：编译修复（另一台机器拉取后）
- 11 处 `DamageCmd...FromCard(this)` 缺必填参数 `cardPlay` → 全部补传（BaseLib 3.3.8 下 `FromCard(CardModel, CardPlay?)` 是可空但**必填**）
- ConquerAllLands 构造器命名参数 `cost:` 在 CustomCardModel 3.3.8 上不存在 → 改位置参数
- 补齐 8 个独立 Power 类 + 荡平万邦的 eng/zhs 本地化（STS001 清零，TODO「8 个独立 Power 类本地化」✅）
- `dotnet build` + `dotnet publish` 通过，已部署到游戏 mods 目录
- ⚠️ 根因：另一台机器提交前未做最终编译验证（两台机器 BaseLib 均为 3.3.8=最新版，非版本漂移）

### 本机跟进②：启动崩溃修复（ConquerAllLands 缺 [Pool]）
- 现象：游戏启动致命错误「ConquerAllLands must be marked with a PoolAttribute」
- 根因：spec/card-dev.md 曾误判「卡牌不像遗物那样强制 PoolAttribute」——原生 Token 卡（Soul 等）不经过 BaseLib 注册（由 `TokenCardPool.GenerateAllCards` 显式列出），自定义卡走 `CustomCardModel` 构造（autoAdd 默认 true）**强制** [Pool]；且游戏 `CardModel.Pool` 找不到池会抛 InvalidProgramException（手牌渲染必触达）
- 修复：`[Pool(typeof(TokenCardPool))]` 注入原版机制卡池（游戏原生 `ModHelper.AddModelToPool` 支持注入原版池；Token 池非奖励/商店取数源 + Token 稀有度不被掷骰 = 双保险不掉落）
- 同步修正 spec/card-dev.md「生成卡」一节（错误论断已更正为两层强制说明）
- publish 通过已部署；待游戏内验证：启动 → 血仇≥7 触发 → 荡平万邦在手牌正常渲染（无色卡框）

### 本机跟进⑦：WithUpgrade 增量语义踩坑 → 全库改 WithUpgradeTo
- 现象：打击升级 15 伤（6+9）、御敌升级 10 纷争（4+6）——升级值被**加**到基础值上
- 根因：BaseLib `WithUpgrade(x)` → `DynamicVar.UpgradeValueBy(x)` = `BaseValue += x`（**增量**，非目标值），全库 70 处按目标值写全踩坑
- 整改：新增扩展 **`WithUpgradeTo(x)`**（`Extensions/WandiDynamicVarExtensions.cs`，目标值语义，内部换算 x−基础值，支持降值升级如 2→1）；56 个卡牌文件 70 处 `WithUpgrade(` → `WithUpgradeTo(`（数值不动，注释 X→Y 仍成立）；批量补 `using WandiMod.WandiModCode.Extensions;`
- `EnergyCost.UpgradeBy(-1)` 是原版增量 API（降费），不在整改范围
- **spec/card-dev.md 已加醒目警告区块**（增量 vs 目标值 + 踩坑记录），后续新卡禁用 WithUpgrade
- build/publish 通过已部署；待游戏内抽查升级数值：打击 9、御敌 6 纷争、荡平万邦 20+35%、血祭·诛王枪 14/3血仇/30%

### 本机跟进⑥：9 个 Power 图标落地
- 素材来源 `D:\shaluMod\image`（9 张），按 `Id.Entry` snake_case 规则命名（`VengeancePower` → `vengeance_power.png`，已用本地化键名反向核实）复制到 `images/powers/` + `images/powers/big/` 两处（小图标 + 悬停大图共用同一张）
- 绑定方式：WandiModPower 基类 `CustomPackedIconPath`/`CustomBigIconPath` 自动按类名寻址，无需改代码
- 补：FatalThrustPower 素材后补（源文件名带尾随空格 `FatalThrustPower .png`），已复制为 `fatal_thrust_power.png` 两处并 publish——10 个 Power 图标全部到位
- publish 通过已部署；待游戏内验证：战斗中血仇/纷争图标、悬停大图

### 本机跟进⑤：荡平万邦数值重订 + CalculatedDamageVar 卡面实时总伤
- 按用户定稿：删「敌生命上限 5%」段，数值改 14 + 已损失生命 25%（升级 20 + 35%）——卡牌设计文档本就已是此数值，实现追平
- 伤害改 **CalculatedDamageVar**（原版 BodySlam/PerfectedStrike 写法）：`CalculationBase(14→20) + ExtraDamage(25→35) × multiplier(已损失生命/100)`；卡面伤害数字与描述 `{CalculatedDamage:diff()}` 实时显示含动态加成的总伤（指向/悬停敌人时刷新），且与 OnPlay 结算同源（`DamageCmd.Attack(DynamicVars.CalculatedDamage)`），显示与实打一致
- 伤害全体一致后回到单次 `TargetingAllOpponents` AoE（带 vfx/vfx_attack_slash 侧向全体特效），删除逐敌追加循环
- 注意：multiplier 必须静态 lambda（WithMultiplier 强制），经 card 参数读 Creature；战斗外 Owner.Creature 为 null → 0
- 本地化 eng/zhs 描述同步；设计方案文档起手表行同步；publish 通过已部署
- 待游戏内验证：①卡面/描述数字随当前已损失生命实时变化（升级后 20+35%）②打出为一次攻击动作全体同时结算

### 本机跟进④：血仇触发后无法再叠层 + 荡平万邦逐怪结算修复
- **Bug 1（血仇不再叠加）**：触发消耗 7 层使层数归 0 → 游戏 `ShouldRemoveDueToAmount` 自动移除能力 → `AfterDamageReceived` 掉血叠层钩子随能力消失失效。即使钩子里改 `SetAmount(0)` 也活不过外层 `ModifyAmount` 帧的移除检查（先查过 `ShouldRetainAsNegative`，新版源码已无此逃生门）
- **修复（用户定方案）**：阈值 7→**8 层触发、消耗 7 层、保底 1 层**（`TriggerThreshold=8`/`FloorAfterTrigger=1`），能力永不归零即永不被移除，稳态仍每 7 次失血一轮；文档（设计方案/卡牌设计/遗物设计）与代码注释同步改为 ≥8
- **Bug 2（挨个打）**：`AttackCommand` 一次命中只携带统一伤害值，无法按目标分别取值 → 拆两段：①「基础+已损失生命%」一次性 `TargetingAllOpponents` AoE（原版 Sow/CrashLanding 写法，一次攻击动画全体同时结算）；②「敌 MaxHp×5%」因敌而异，逐敌 `CreatureCmd.Damage` 追加（非攻击伤害无动画，仅跳数字），放 AoE 后取最新敌列表避免鞭尸
- publish 通过已部署；待游戏内验证：①血仇触发后（剩 1 层+自伤回 2 层）继续掉血可继续叠层、再次到 8 层可再触发 ②荡平万邦打多敌时一次攻击动作全体掉血 + 各敌追加一段上限百分比数字

### 本机跟进③：血仇≥7 不生成荡平万邦（CanonicalModelException）
- 现象：血仇到 7 层正常清空 + 扣血，但荡平万邦没进手牌，报「Canonical model of type ConquerAllLands used in incorrect place」
- 根因：`ModelDb.Card<T>()` 返回**规范（不可变）实例**，`CardPileCmd.AddGeneratedCardToCombat` 访问 `card.Owner` 触发 `AssertMutable` 异常
- 修复：改 `CombatState.CreateCard<ConquerAllLands>(Owner.Player)` 创建战斗内可变实例（原版 Turbo 造 Void 同款写法）；顺带用 `CardCmd.Upgrade` 落地觉醒版升级版荡平万邦（TODO「觉醒版荡平万邦」✅）
- publish 通过已部署；待游戏内验证：≥7 触发 → 手牌出现荡平万邦（不灭王血在场时为升级版）

### 此前（用户另一台机器）
- M1 脚手架（工程创建 + 角色模型 + 占位立绘）
- 纷争（Strife）机�����临时最大生命上限 + Grant + AfterCombatEnd 还原
- M2 普通卡批次（12 张：御敌/蓄势/坚壁/誓约之枪 等）
- 构建链路打通（Rider + .NET SDK + MegaDot）
