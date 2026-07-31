# 开发记录 2026-07-31

---

### 全卡牌数值对齐设计稿（用户改设计稿 → 代码+本地化跟进）

用户修改了 `doc/万敌Mod-卡牌设计.md`（新值已随近期提交进 HEAD），本机跟进代码与本地化。约 25 张卡改值、其中 ~12 张机制重做。

**主题转变**：多张防御牌从「纷争（临时上限）」改为「直接回血」——御敌/坚壁/守誓/备战/死地后生/净血。纷争流仍由 蓄势/凝血/鲜血护盾/庇护/巨灵之躯/金色壁垒/绝对防御/王者归来/生命之泉/血色共鸣 承载。

#### 普通/基本卡
- **御敌** HoldTheLine：纷争 4/6 → 回血 4/5（起手防御变回血件）
- **坚壁** Bulwark：纷争 10/12 + 血仇 → 回血 8/12 + 血仇
- **守誓** Oathguard：纷争 5/+4 → 回血 4/+3（消耗1血仇）
- **嗜血** Bloodthirst：0费 自伤+2血仇+1/2力量 → **1费** 1/2力量 + **消耗**（去掉自伤与血仇）
- **饮血** BloodDrink：回血 6/9 → 5/6
- **备战** Preparations：纷争 4/6 + 抽牌 → 回血 3/5 + 抽牌
- **蓄势** Stance / **凝血** Coagulate：加 **消耗** 关键词（数值不变）
- **残影突袭** AfterimageStrike：自伤 2 → 1

#### 机制卡
- **荡平万邦** ConquerAllLands：MissingHp 25/35% → 15/20%；词条 **保留→虚无**

#### 罕见卡
- **弑王枪·连突** KingslayerChain：5/6×2（每段消耗1血仇+5/6）→ **11/15×2 + 纯消耗1血仇**（BloodCost 门控：血仇≤1 不可打出、充足金边高亮；无增伤加成；多段走 AttackContext 活力覆盖）
- **背水一战** LastStand：8 → 7 伤
- **灼血击** BloodburnStrike：触发条件「血仇≤1（无血仇）」→「血仇>4」
- **致命突刺** FatalThrust：14 → 9 伤
- **血潮** BloodTideSurge：9/13 → 10/14 伤
- **命途斩** LifepathSlash：MaxHp 20/30% → 20/25%
- **死地后生** TurnTheTide：纷争重做 → 回血；阈值 30%→50%；HP≤50%回10/15、否则回6/8
- **庇护** Sanctuary：MissingHp 25/30% → 8/10%；加消耗
- **强身** Vitalize：MissingHp%回血 → **回血12/15 + 给目标1/2易伤**（目标改 AnyEnemy，费用1→2）
- **净血** BloodCleanse：消耗手牌+纷争 7/13 → +回血 5/7
- **鲜血护盾** BloodAegis：失血 4→1、纷争 10/15→9/12；加消耗
- **巨灵之躯** TitanBody：纷争 15/18 → 12/16（回退之前 b4a1d92 的改动）
- **血契** BloodPact：失「手牌数」血（动态）→ 固定失 3 血
- **庇护盟约** GuardianPact（联机）：队友减伤 50/75% → **固定减半**；自身增伤固定50% → 50%/45%（Amount 语义翻转）
- **王者之佑** KingsBlessing（联机）：格挡 14/20 → 16/22；失血 5 → 1

#### 稀有卡
- **灾厄之矛** CalamitySpear：MissingHp 70/80% → **当前最大生命 40/45%**（改 MaxHp 缩放，吃纷争放大；设计稿流派标签同步 MissingHp→MaxHp）
- **绝命枪** ReaperSpear：8/10伤+残血加伤 → **14/15伤 + HP≤50%抽2/3**
- **金色壁垒** GoldenBastion：X×6/8 → X×7/9 纷争
- **血祭仪典** BloodRitual：失血 8 → 5
- **圣血洗礼** HolyBloodBaptism：加消耗
- **绝对防御** AbsoluteGuard：MaxHp 15/20% → 8/10%
- **焚天** Heavenburn：失血 8 → 5
- **血色共鸣** BloodResonance：纷争 2/3 → 1/2（回退之前"1/2→2/3"的改动）
- **金色裁决** GoldenJudgment：自身MaxHp 25/35% → **10伤 + 敌人MaxHp 10/15%（每打出一次+5%，战斗实例字段计数）**
- **淤血带冠** Bodyguard（原「替天行道」改名，联机）：效果 = 本回合失血 → 给所有队友纷争（MissingHp 近似，每失10血给 {StrifePerAlly} 纷争，5/8）。先前���度改成格挡，按设计改回纷争。
- **狂化** Frenzy：去掉每回合抽牌上限（≤2/3 → 无上限）；升级获得 **固有**
- **血仇主宰** VengeanceDominion：基础虚无，升级 **失去虚无**（IsUpgraded 切换词条）
- **王之意志** SpiritOfKing：固有改为 **升级固有**（基础无）

#### 先古卡
- **血祭·诛王枪** BloodriteRegicide：MissingHp 20/30% → 10/15%

### 本地化同步
- cards.json（eng+zhs）：16 张机制/变量名变更卡 description 重写（纷争→生命值、变量名 Strife→Heal 等）；纯数值卡走 `{Var:diff()}` 占位符自动更新，无需手改。
- powers.json（eng+zhs）：FrenzyPower（去上限描述）、GuardianPactPower（改语义）。

### 代码审查 + 修复
- `IsUpgraded` 切换词条（Frenzy/VengeanceDominion/SpiritOfKing）——属性已确认（CardModel.cs:722）。
- BloodburnStrike.cs 1 处 GBK 污染（「高」→3×U+FFFD）已修。
- 全仓 U+FFFD 复扫归零。

### 待用户确认的解读点
- ~~弑王枪·连突~~：已确认 = 纯消耗1血仇（BloodCost 门控，血仇≤1 不可打出），无加成。数值 11/15。
- ~~Bodyguard~~：已确认 = 改名「淤血带冠」，效果改回失血→队友纷争（5/8，MissingHp 近似）。

### 部署
未编译（本机无 sts2.dll）；代码基于设计稿 + 既有 API 范式编写。用户本机 Build + Publish 验证。

---

### 三张卡二次调整（设计稿细化）

- **饮血枪** BloodsipSpear：去掉「失血判断」条件（原"本回合失过血才回"），改为**直接回血**（7伤回2 / 10伤回4）。彻底消除 per-turn 歧义。
- **浴血带冠** Bodyguard（原"淤血带冠"，错别字 淤→浴）：从一次性技能**重做为单回合能力**。新建 `BodyguardPower`——万敌受击（AfterDamageReceived，实际掉血）时给所有其他队友 5/8 纷争；敌方回合末自毁（GuardianPactPower 同范式）。
- **此乃天谴** BloodForBlood（原"以血还血"）：伤害从「队友 MissingHp 总和 50/75%」改为「**所有队友生命上限总和 25/33%**」，回血一半不变。
- 本地化 eng+zhs 同步（标题改名 + 描述重写）。BodyguardPower 1 处"攻"字 GBK 污染已修。
- 英文名按设计稿保留（Bodyguard / Blood for Blood）；若需同步英文待用户确认。
