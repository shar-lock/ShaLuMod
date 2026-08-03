# 开发记录 2026-08-03

---

### 王者归来对齐设计稿（未提交）

- 阈值 30%→**50%**；爆发纷争 24/32→**12/16**；否则 8/12 不变；力量 3/4、回血 8/12 不变
- 补 **Exhaust**；HP≤50% 金边高亮；eng+zhs 文案同步

---

### 游戏测试 Bug 修复（未提交）

7 项局内反馈修复 + 审查，`dotnet build` 通过（0 error）。

1. **血仇 / 荡平进度独立**：生成荡平万邦迁至 `ConquerProgressPower`（只计获得量，满 7 生成）；`VengeancePower` 仅成长消耗 4；去掉 `SyncProgress`
2. **动态伤卡局外不显 0**：BodySlam 式 `{InCombat:…CalculatedDamage…}`；覆盖灾厄之矛、命途斩、万死无悔、噬仇、荡平万邦、焚天、诛王枪、诛天焚骨、此乃天谴
3. **狂战之心**：去掉攻击自伤，仅 +1 血仇
4. **血祭仪典**：失 3 / 抽 3 / +3 血仇（升级抽 5）+ Exhaust
5. **遗物稀有度对调**：焚血勋章 Rare、血仇圣坛 Uncommon（文档表已同步）
6. **战意持久**：卡面标注「效果不叠加」
7. **灼血击**：`DisplayAmount > 4` 金边高亮

同步：`spec/core-mechanics.md`、`doc` 卡牌/遗物表、powers/card_keywords 文案。

---

### 跟进另一台机器进度（审查 `b33ad0f`…`8cbbea6`）

远程 `origin/wandi` 已含 3 个新提交（相对本机上次同步的 `e0dc2a7`）：

| Commit | 内容 |
|---|---|
| `b33ad0f` | 11 卡 + 沾血枪尖机制翻转；灾厄之矛/命途斩转 CalculatedDamageVar |
| `c4ef59e` | 诛天焚骨 / 此乃天谴转 CalculatedDamageVar；血仇圣坛 2%→5% |
| `8cbbea6` | **P0**：诛天焚骨先读 CalculatedDamage 再吞噬血仇（避免实打×1） |

机制向摘要：力敌万邦去抽牌改产层；焚血改额外血仇；庇护/诛王枪改 MaxHp 缩放；荡平去虚无；毁灭裁决无条件产血仇等。

### 审查修复（本机）
1. **力敌万邦** `powers.json` eng+zhs 与代码脱节（仍写旧抽牌）→ 同步为回合开始 +{Amount} 血仇  
2. **血仇圣坛** relics 文案仍 2%、代码已 5% → 文案+日志同步  
3. **此乃天谴** 回血改为实际 UnblockedDamage 的 50%（对齐饮血反击）

### 角色 UI 调研（同日，另线）
对照 `game_source` 补全 `spec/assets.md`「原版角色 UI 调研」（路径/PNG·Spine·场景/分辨率）；已随 `f44c56c` 提交。

---

### 远程拉取审查（`69931ae`…`99429dd`）

| Commit | 内容 |
|---|---|
| `69931ae` | 沾血枪尖→随机单体3伤；金焰斩/连突/狂怒/坚壁数值下调 |
| `99429dd` | Harmony 注入建筑师对话 + `GetArchitectAttackVfx`；ancients/banter 文案 |

审查修复：
1. 沾血枪尖 `Random.Shared` → `RunState.Rng.CombatTargets.NextItem`（防联机不同步）
2. eng `card_keywords` 去掉荡平误留的 Retain
