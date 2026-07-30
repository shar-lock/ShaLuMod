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
