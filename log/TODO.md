# 待办（TODO）

> 运行时确认 / 未完成项。**完成后标 ✅**。

## 08/03 跟进另一台机器（b33ad0f…8cbbea6）审查

跨机 3 commit：`b33ad0f`（11 卡+1 遗物调整）→ `c4ef59e`（诛天焚骨/此乃天谴 CalculatedDamage）→ `8cbbea6`（诛天焚骨时序 P0）。

### 审查已修（本机）
| # | 问题 | 处理 |
|---|---|---|
| 1 | 力敌万邦 **powers.json** 仍写旧机制（+2 血仇+抽牌） | eng+zhs 改为「回合开始 +{Amount} 血仇」 |
| 2 | 血仇圣坛代码 5%、**relics.json 仍 2%** | eng+zhs → 5%；日志 ×2%→×5% |
| 3 | 此乃天谴回血用预读 Calculate，格挡后偏高 | 改按 Attack 实伤 UnblockedDamage×50% |

### 待游戏内回归（本批）
- 诛天焚骨：卡面预览 = 实打（先读伤害再吞噬血仇）
- 灾厄之矛 / 命途斩 / 此乃天谴：卡面 `{CalculatedDamage}` 与实打一致
- 力敌万邦：打出 +4、回合开始 +4；悬停 Power 文案正确；**不再抽牌**
- 焚血：自伤卡 → 额外 +1 血仇（可多次）
- 沾血枪尖：获血仇 → 全体敌 1 伤（单次 Apply +N 只触发 1 次 AoE）
- 荡平万邦：无虚无；血仇圣坛击杀回 5% MaxHp
- 涅槃 1 费耗 2 血仇换 3/4 能；毁灭裁决无条件 +4 血仇

### 备注（非必须）
- `Indomitable` 类注释已同步 10/12%；`ConquerAllLands` 文件头仍写「虚无」——仅注释陈旧
- 角色 UI 调研已写入 `spec/assets.md`（本机未提交的 spec 改动另计）

## 07/31 跟进另一台机器大改（纷争稀有化 + 全卡对齐）审查修复

| # | 问题 | 修复 |
|---|---|---|
| 1 | Frenzy / TurnTheTide 缺 `using DynamicVar` → 本机无法编译 | 补 Localization.DynamicVars |
| 2 | CalamitySpear / BloodForBlood 缺 `BaseLib.Extensions`（WithValueProp） | 补 using |
| 3 | BodyguardPower 无 powers.json 条目 | eng+zhs 补 BODYGUARD_POWER |
| 4 | 灼血击用真实 Amount>4（显示层 off-by-one） | 改 DisplayAmount>4 |
| 5 | 浴血带冠自伤也触发「受到攻击」 | dealer==null/Owner 排除 |
| 6 | 饮血枪仍挂血仇关键词 | 去掉 CanonicalKeywords |

### 待游戏内回归（08/01）
- **王者之佑**：联机仅万敌失 1 血，队友只拿格挡
- **狂化 / 王之意志**：升级后卡面出现固有；狂化升级预览不再「无效果」
- **血仇主宰**：升级后失去虚无
- **潘多拉魔盒**：替换全部 Basic 打击+御敌（不替换血祭之枪/残影）
- **荡平万邦生成**：不再扣 5% 血；状态栏出现「荡平进度」0～7
- **复仇心**：费用 1；**庇护**：10%/15% MissingHp 纷争
- 御敌/坚壁/守誓等：纷争→回血数值与手感
- 灼血击：显示血仇>4 才抽牌
- 浴血带冠：联机受击给队友纷争；自伤不触发；敌方回合末消失
- 弑王枪·连突：BloodCost 门控 + 活力覆盖两段
- 金色裁决：每打出一次敌 MaxHp% +5%
- **金焰斩**：14/18 伤 + 回血 3；**裂伤**：6/9 伤 + 弃牌回手

## 全卡牌开发状态

| 批次 | 数量 | 状态 |
|---|---|---|
| 机制卡 | 1 | ✅ |
| 基本卡 | 4 | ✅ |
| 普通卡 | 20 | ✅ |
| 罕见卡 | 38 | ✅ |
| 稀有卡 | 27 | ✅ |
| 先古卡 | 2 | ✅ |
| **合计** | **92** | **全部完成** |

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
| ~~起手牌组打击位用原版 `Cards.Strike`~~ | ✅ 误报澄清（07/30 二轮审查）：嵌套命名空间解析规则下 `Cards.Strike` 本就解析为 `WandiMod.WandiModCode.Cards.Strike`（嵌套命名空间优先于 using），起手已是专属打击；已显式化引用消除歧义。游戏内抽一局确认牌框为血红即可彻底关闭 |

### ~~设计稿↔代码不一致~~ ✅（07/30 二轮审查发现，用户决定代码对齐设计稿）
| 卡 | 处理 |
|---|---|
| 巨灵之躯 TitanBody | ✅ 5/8 → 15/18 |
| 横扫 Sweep | ✅ 8/12 → 7/10 |
| 血潮 BloodTideSurge | ✅ 8/12 → 9/13 |
| 净血 BloodCleanse | ✅ 重做为「选择消耗一张手牌 + 7/13 纷争」（Scavenge 范式），本地化 eng/zhs 同步 |

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

## 07/30 游戏内实测 9 项修复（全部完成，构建 ✅）

| # | 问题 | 修复 |
|---|---|---|
| 1 | 初始遗物描述 | BloodOfTheKinslayer 加 IntVar("Charges") + setter 同步 BaseValue（WingedBoots 范式）+ ExtraHoverTips（血仇词条 + 荡平万邦预览）；描述 {Charges} 实时显示剩余免死 |
| 2 | 图鉴无万敌遗物 + 缺商店遗物 | WandiModRelicPool.SeenByDefault=true；新增商店遗物「深红契印 CrimsonSigil」（战斗开始获得 1 张荡平万邦，持有不灭王血→升级版；Shop 档只进商店、机制对标罕见；v4 经济向/v5 卖血产层方案均已迭代） |
| 3 | cards.json 文本规范化 | eng+zhs 全量重写：HP→生命值、×/+ 数学符号→文字、硬编码数值→DynamicVar 占位 |
| 4 | 血仇局内描述 | powers.json 重写（失血叠层/+2% 每层/7 层触发消耗 4 层 + 生成荡平万邦）；VengeancePower.ExtraHoverTips 加荡平万邦预览；新增 ConquerAllLands 关键词（WandiModKeywords + card_keywords.json） |
| 5 | 誓约之枪与血祭之枪重复 | 重做：1费 9 伤；有血仇（Amount>=2）额外 +3 / 12+4（奖励持有血仇，不再产血仇） |
| 6 | 守誓/连刺耗血仇降为 0 | 消耗判定统一 Amount>=2（保底 1 层地板）；守誓 BloodBonus 3→4；连刺重做（DamageVar 5→6、整牌只耗 1 层、两段都吃 +3）；弑王枪·连突同步 Amount>=2 |
| 7 | 焚天 UI 无伤害显示 | 重做 CalculatedDamageVar（CalculationBase 12→18 + ExtraDamage 3→4 × 力量），卡面实时显示总伤 |
| 8 | X 费牌无能量图标 | 金色壁垒/弑神枪·无尽：cost -2→0 + HasEnergyCostX override + ResolveEnergyXValue()（对齐原版 Whirlwind；DynamicVars.Energy 未声明会 KeyNotFound） |
| 9 | 欧洛巴斯事件卡死 | BloodriteStrike.GetTranscendenceTransformedCard 返回规范模型（原版流程自行处理升级态；返回 mutable 会 AssertCanonical 崩） |

附带修复：灼血击 blood==0→<=1、饮血枪 >0→>=2（地板层语义）；浴血奋战倍率语义改为 Amount=50/100 返回 1+Amount/100（总伤 ×1.5/×2，走 BloodbathPower.ModifyDamageMultiplicative，与旧版只放大已损生命分量/卡面显示歧义双 bug 一并修）；LastingFocus 30/40→20/30、BloodResonance 1/2→2/3（对齐用户设计稿改动）；文档三件同步（起手 5 打击+3 御敌、初始 80 血、商店遗物、誓约之枪/守誓新数值）。

### 待游戏内回归
- 欧洛巴斯事件：血祭之枪→血祭·诛王枪替换（升级态应保留）
- X 费双卡：左上角能量图标 + 结算段数/纷争
- 焚天/荡平万邦：卡面动态伤害显示与实打一致
- 浴血奋战单体 ×1.5/×2（总伤）+ 卡面文案 50%/100%
- 深红契印：商店出现、战斗开始手牌 +1 荡平万邦；持有不灭王血时为升级版
- 卡牌文本能量图标：涅槃/湮灭之枪/蓄能突涌/黄金之瓮描述中的 text_energy.png 内联渲染（中英文档同步）
- 坚韧：打出后正常弹出消耗选牌界面（07/30 修复：SelectionScreenPrompt 缺本地化抛 InvalidOperationException → 改坚毅升级版同款内置 CardSelectorPrefs.ExhaustSelectionPrompt），消耗 1 牌 +2/3 血仇
- 暴风连击 + 多段活力：持活力时每一段都加伤（WithHitCount / AttackContext）；血仇读真实层数保底1段/1层结算，描述已写「至少1段/至少按1层」
- 连刺 / 弑王枪·连突 / 弑神枪·无尽 / 血海狂涛：同上多段活力修复（血海狂涛顺手修了 Repeat 未传入导致只打1段的隐患）
- 弑亲血脉 / 不灭王血：免死充能 4→2（实测 4 次过高；觉醒继承同值；描述走 {Charges} 自动同步）
- 遗物图鉴：万敌遗物全彩可见（含未拾取）

## M2 遗留

| 问题 | 说明 | 状态 |
|---|---|---|
| ~~弑亲血脉充能角标~~ | ~~counter API~~ | ✅ ShowCounter + DisplayAmount + InvokeDisplayAmountChanged |

## 其他

| 问题 | 说明 |
|---|---|
| ~~14+ Power 类本地化~~ | ✅ 07/30 补齐 12 个新 Power（eng+zhs） |
| 本机 images 目录缺失 | .gitignore 忽略 png → **发布约定（07/30 定）：只有本机（含 images/ 素材）执行 dotnet publish，另一台机器只写代码不发布**，否则 pck 丢全部图片素材 |
| FatalThrustPower STS003 | 继承原生 TemporaryStrengthPower 无 ID 前缀，警告容忍（显示走 OriginModel 卡牌） |
| ~~血仇读层算伤卡的层数口径~~ | ✅ 已决定保留：狂怒/噬仇/暴风连击/噬魂/诛天焚骨仍读真实 Amount（不砍数值），隐藏的保底 1 层对这些伤害卡仍算数，作为兜底机制 |
