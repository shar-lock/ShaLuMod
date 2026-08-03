# 开发记录 2026-08-03

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
对照 `game_source` 补全 `spec/assets.md`「原版角色 UI 调研」（路径/PNG·Spine·场景/分辨率）；本机 `spec/*` 工作区改动尚未提交。
