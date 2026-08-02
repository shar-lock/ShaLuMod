# 开发记录 2026-08-01

---

### 六项 Bug / 设计同步修复

#### 1. 王者之佑 KingsBlessing
- **问题**：全队（含队友）各失 1 血。
- **修复**：格挡仍给全体队友；`CreatureCmd.Damage` 仅打 `Owner.Creature`。
- 本地化改为「你失去…」。

#### 2. 狂化 Frenzy 升级固有无效
- **根因**：`CanonicalKeywords => IsUpgraded ? [Innate] : []` 无效——`LocalKeywords` 首次访问时缓存 CanonicalKeywords，升级后不重读。
- **修复**：原版范式 `OnUpgrade() => AddKeyword(CardKeyword.Innate)`。
- 顺带同修：王之意志（加固有）、血仇主宰（升级 `RemoveKeyword(Ethereal)`）。

#### 3. 潘多拉魔盒不替换打击/御敌
- **调研**：`PandorasBox.AfterObtained` 过滤 `c.IsBasicStrikeOrDefend`，判定为 `Rarity==Basic && (CardTag.Strike || CardTag.Defend)`。
- **根因**：万敌 `Strike` / `HoldTheLine` 未挂 `CanonicalTags`。
- **修复**：Strike → `CardTag.Strike`；HoldTheLine → `CardTag.Defend`（对齐原版 StrikeIronclad / DefendIronclad）。血祭之枪等特色起手不挂标签，不被替换。

#### 4. 生成荡平万邦不再扣 5% 血
- `VengeancePower` 去掉触发时的 `CreatureCmd.Damage`；powers / card_keywords 文案同步。

#### 5. 荡平进度独立计数器
- 新 Power `ConquerProgressPower`：状态栏右下角数字 = 距下次生成的进度 0～7。
- 初版误用 `Single`（NPower 只对 Counter 渲染数字）→ 已改为 **Counter**；Amount 固定 1 防进度 0 被移除，DisplayAmount 显示真实进度。去掉卡牌悬停预览。
- 触发消耗：目标 4 层；不足时只减到保底真实 1 层（玩家可见 0）。
- 战斗开始由弑亲血脉与血仇一并赋予；`VengeancePower` 层数变化后 `SyncProgress`。

#### 6. 卡牌设计文档数值同步
| 卡 | 变更 |
|---|---|
| 复仇心 VengefulHeart | 费用 0 → **1** |
| 庇护 Sanctuary | MissingHp% 8/10 → **10/15** |
| 裂伤 / 金焰斩 | 已对齐设计稿（6/9 弃牌回手；14/18+回血3），本次无再改 |

### 部署
`dotnet build` ✅ + `dotnet publish` ✅

---

### 遗物图标全量注册（源目录 `D:\shaluMod\image\relic`）
6 件遗物主图已按 snake_case 写入 `images/relics/` + `big/`。源目录无 outline：用 PIL 按 alpha 生成白色剪影 `{name}_outline.png`（StS 轮廓配套）。`WandiModRelic` 路径约定无需改代码。`dotnet publish` 重新 import/打 pck。

### 地图画笔颜色 → 血红
调研：`CharacterModel.MapDrawingColor`（默认黑）→ `NMapDrawings` 赋给 `Line2D.DefaultColor`。原版铁甲 `CB282B`。万敌 override 为既有主色 `B71C1C`。

### Power 图标全量替换（源目录 `D:\shaluMod\image\power`）

将 23 张最新 Power icon 统一改名为 `Id.Entry.RemovePrefix().ToLowerInvariant()` 规则（snake_case），覆盖写入：
- `WandiMod/images/powers/{name}.png`
- `WandiMod/images/powers/big/{name}.png`

含此前缺失的 13 张（荡平进度/狂化/力敌万邦等）+ 替换原有 10 张。无需改代码——`WandiModPower` 已按文件名自动加载。`dotnet publish` 触发 Godot 重新 import 并打进 pck。
