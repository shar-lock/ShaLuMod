# 图片资源注册规范

> ⚠️ **`*.png` 和 `*.import` 被 `.gitignore` 忽略——图片不入版本控制，每个开发者本地维护。**
> 新机器 clone 后需要自行创建 `images/` 目录 + 占位图，否则游戏内显示空白。

## 核心逻辑：类名 → 路径

所有图片路径由代码自动生成，**你不需要手动注册图片**——只需把 PNG 放到正确的目录、用正确的文件名。

```
类名 PascalCase → Id.Entry.RemovePrefix().ToLowerInvariant() → 文件名
例：BloodRite → "WandiMod:BloodRite" → RemovePrefix → "BloodRite" → ToLowerInvariant → "bloodrite"
→ 文件名 = bloodrite.png（全小写，无下划线，无分隔符）
```

> **注意**：是类名直接小写（`bloodrite.png`），**不是** snake_case（`blood_rite.png`）。

## 目录结构

```
WandiMod/WandiMod/images/          ← 磁盘根目录（= Godot res://WandiMod/images/）
├── card_portraits/
│   ├── big/           ← 卡牌大图（必须，1000×760）
│   │   ├── card.png             （占位回退图）
│   │   └── bloodrite.png        （BloodRite 的大图）
│   ├── bloodrite.png            （卡牌小图，可选 250×190，不放则游戏自动缩放大图）
│   ├── card.png                 （占位回退图）
│   └── beta/
│       └── bloodrite.png        （Beta 美术，可选）
├── powers/
│   ├── big/           ← 能力大图（可选）
│   │   ├── power.png
│   │   └── vengeancepower.png
│   ├── vengeancepower.png       ← Power 图标
│   └── power.png                （占位回退图）
├── relics/
│   ├── big/           ← 遗物大图（可选）
│   │   ├── relic.png
│   │   └── bloodofthekinslayer.png
│   ├── bloodofthekinslayer.png           ← 遗物图标
│   ├── bloodofthekinslayer_outline.png   ← 遗物轮廓（必须配套）
│   ├── relic.png                          （占位回退）
│   └── relic_outline.png                  （占位回退）
└── charui/            ← 角色 UI PNG（规格见「原版角色 UI 调研」）
    ├── character_icon_wandi.png      （建议 88×88）
    ├── char_select_wandi.png         （132×195）
    ├── char_select_wandi_locked.png
    ├── char_select_art_wandi.png     （选角静态大图，非原版 Spine）
    ├── map_marker_wandi.png          （建议 49×64 竖图）
    ├── big_energy.png / text_energy.png
    └── （下一步）wandi_combat.png、能量球 layer 等
```

## 各类型详解

### 卡牌图片

| 属性（WandiModCard 基类自动设置） | 目录 | 尺寸 | 必须？ |
|---|---|---|---|
| `CustomPortraitPath`（大图，卡牌详情/战斗） | `images/card_portraits/big/{name}.png` | 1000×760（或 500×380） | ✅ 必须 |
| `PortraitPath`（小图，手牌/牌堆） | `images/card_portraits/{name}.png` | 250×190 | 可选（缺失则缩放大图） |
| `BetaPortraitPath`（Beta 美术） | `images/card_portraits/beta/{name}.png` | 250×190 | 可选 |

- 回退：找不到 → `card.png`（也找不到则空白）
- **只需要放 big/ 下的一张大图就能用**

### Power 图标

| 属性（WandiModPower 基类自动设置） | 目录 | 尺寸 | 必须？ |
|---|---|---|---|
| `CustomPackedIconPath`（小图标，显示在角色身上） | `images/powers/{name}.png` | 64×64 或 84×84 | ✅ |
| `CustomBigIconPath`（大图标，Power 列表/悬停） | `images/powers/big/{name}.png` | 128×128（可用至 256×256） | 可选 |

- 回退：找不到 → `power.png`
- `{name}` = 类名转 snake_case 小写（`Id.Entry.RemovePrefix().ToLowerInvariant()`），如 `VengeancePower` → `vengeance_power.png`
- **能力卡（Card）有卡牌图片（card_portraits），Power 类有 Power 图标（powers）——两套不同的图片**
- **尺寸不是硬性要求**：图标由游戏 `NPower` 场景的 `TextureRect` 按固定大小缩放渲染（已核实源码），
  大于标注尺寸的正方形素材可直接用（GPU 线性过滤缩到 64px，画质良好，推荐 128/256 边长做超采样）；
  小于 64px 会放大模糊，不要用。**必须 1:1 正方形**（非方图会被压扁或留边）。
  省事做法：一张 256×256 同时放 `powers/` 和 `powers/big/` 两处。

### 遗物图标

| 属性（WandiModRelic 基类自动设置） | 目录 | 尺寸 | 必须？ |
|---|---|---|---|
| `PackedIconPath`（小图标，遗物栏） | `images/relics/{name}.png` | 128×128 | ✅ |
| `PackedIconOutlinePath`（轮廓） | `images/relics/{name}_outline.png` | 128×128 | ✅ |
| `BigIconPath`（大图标，遗物详情） | `images/relics/big/{name}.png` | 256×256 | 可选 |

- 回退：找不到 → `relic.png` / `relic_outline.png`
- **图标和轮廓必须成对**（`xxx.png` + `xxx_outline.png`）

### 角色界面（charui）

在 `WandiMod.cs` 中手动覆写路径（不走 RemovePrefix 机制，直接写字符串）。  
**原版路径 / 分辨率 / 文件类型的权威对照见下文「原版角色 UI 调研」。**

| 属性（BaseLib） | 万敌文件（`images/charui/`） | 原版对应分辨率 | 本地现状（约） |
|---|---|---|---|
| `CustomIconTexturePath` | `character_icon_wandi.png` | **88×88** PNG | 510×510（过大可缩放，建议方图） |
| `CustomIconOutlineTexturePath` | （未覆写，可补 `character_icon_wandi_outline.png`） | **88×88** PNG | ❌ |
| `CustomCharacterSelectIconPath` | `char_select_wandi.png` | **132×195** PNG | 600×847（比例接近，UI 缩到约 88×130） |
| `CustomCharacterSelectLockedIconPath` | `char_select_wandi_locked.png` | **132×195** PNG | ✅ 132×195 |
| `CustomMapMarkerPath` | `map_marker_wandi.png` | **49×64** PNG | 510×510（⚠️ 方图 vs 竖图，建议重导出） |
| `BigEnergyIconPath`（卡池） | `big_energy.png` | 原版卡面能量 **74×74** | 134×134 ✅ |
| `TextEnergyIconPath`（卡池） | `text_energy.png` | 文本能量 **24×24** | ✅ 24×24 |
| 选角大图（自定义场景用） | `char_select_art_wandi.png` | 原版为 **Spine**（非整张 PNG） | 1067×600（静态替代） |

- `CharacterUiPath()` **没有回退机制**——文件缺失直接空白
- 改图后必须 **Publish**

## 回退链（StringExtensions.cs）

```csharp
// 以卡牌为例（其他类型同理）
path = "res://WandiMod/images/card_portraits/big/bloodrite.png";
if (ResourceLoader.Exists(path)) return path;       // ✅ 找到 → 用自定义图
MainFile.Logger.Info("Could not find...");          // ⚠️ 日志记录
return "res://WandiMod/images/card_portraits/big/card.png";  // 回退到占位图
// 占位图也找不到 → ResourceLoader 返回 null → 游戏显示空白
```

## Build vs Publish

| 操作 | 图片生效？ |
|---|---|
| Build（锤子） | ❌ 只编译 .dll，不打包图片 |
| **Publish** | ✅ MegaDot headless 打包所有资源（含图片）到 .pck |

**加新图后必须 Publish**，否则游戏看不到图片。

## 快速操作：给一张新卡加图片

1. 画一张 1000×760 的 PNG（或 500×380 也行，会放大）
2. 命名 = 类名全小写，如 `BloodRite` → `bloodrite.png`
3. 放到 `WandiMod/WandiMod/images/card_portraits/big/`
4. Publish
5. 完成——不需要改任何代码

## 排查

- **日志里出现 `Could not find card image path: ...`** → 文件名或路径不对，检查是否全小写、是否在 `big/` 目录
- **游戏内显示空白** → 占位图（card.png）也不存在，需要创建占位图或正确图片
- **Publish 后仍然看不到** → 确认图片在 `WandiMod/WandiMod/images/` 下（不是 `WandiMod/images/`）
- **新机器 clone 后没有 images 目录** → 正常（png 被 gitignore），需要手动创建目录结构 + 占位图

---

# 原版角色 UI 调研（`D:\shaluMod\game_source`）

> 权威来源：`CharacterModel.cs` 路径约定 + 各角色实际资源尺寸（以 Ironclad 为样板，Silent/Defect/Regent/Necrobinder 同规格）。  
> 调研日期：2026-08-03。

## 原版怎么挂资源

原版角色 **不靠一张「角色 UI 合集」**，而是按用途拆到多处；路径由角色 `Id.Entry`（如 `ironclad`）拼出来：

| 用途 | 类型 | 原版路径模式（`{id}` = ironclad 等） |
|---|---|---|
| 顶栏 / 存档小头像贴图 | PNG | `images/ui/top_panel/character_icon_{id}.png` + `_outline.png` |
| 顶栏图标场景 | `.tscn`（TextureRect 包一层） | `scenes/ui/character_icons/{id}_icon.tscn` |
| 选角按钮立绘（解锁/锁定） | PNG | `images/packed/character_select/char_select_{id}.png`（+ `_locked`） |
| 选角大背景 | **Spine 场景** `.tscn` + `.skel/.atlas/.png` | `scenes/screens/char_select/char_select_bg_{id}.tscn` → `animations/character_select/{id}/` |
| 选角转场擦除图 | PNG + ShaderMaterial `.tres` | `images/ui/transitions/{id}_transition.png` + `materials/transitions/{id}_transition_mat.tres` |
| 地图标记 | PNG | `images/packed/map/icons/map_marker_{id}.png` |
| 卡面能量（牌面右下） | PNG（图集切片） | `images/atlases/ui_atlas.sprites/card/energy_{id}.png`（运行时走 `.tres`） |
| 文本内能量符号 | PNG | `images/packed/sprite_fonts/{id}_energy_icon.png` |
| 战斗能量球 | `.tscn` + 多层 PNG + VFX 场景 | `scenes/combat/energy_counters/{id}_energy_counter.tscn` + `images/ui/combat/energy_counters/{id}/{id}_orb_layer_*.png` |
| 战斗角色 | **Spine** `.tscn` | `scenes/creature_visuals/{id}.tscn` → `animations/characters/{id}/` |
| 出牌轨迹 | `.tscn`（粒子/渐变，非单图） | `scenes/vfx/card_trail_{id}.tscn` |
| 篝火 / 商人 | Spine `.tscn` | `scenes/rest_site/characters/{id}_rest_site.tscn` / `merchant/characters/{id}_merchant.tscn` |
| 联机猜拳手 | PNG ×4 | `images/ui/hands/multiplayer_hand_{id}_{point,rock,paper,scissors}.png` |
| 音效 | FMOD event | `event:/sfx/characters/{id}/...` |

Mod 侧通过 BaseLib `CustomCharacterModel` / `PlaceholderCharacterModel` 覆写同名语义属性；未覆写则回退战士。

## 原版分辨率与格式一览（实测）

| 资源 | 格式 | 分辨率 / 规格 | 备注 |
|---|---|---|---|
| `character_icon_{id}.png` | PNG（透明） | **88×88** | 场景 `*_icon.tscn` 仅包 TextureRect；UI 再缩放 |
| `character_icon_{id}_outline.png` | PNG | **88×88** | 联机地图等描边 |
| `char_select_{id}.png` / `_locked` | PNG（透明） | **132×195**（约 2:3 竖图） | 按钮内 Icon 区约 **88×130**，`expand` 缩放 |
| 选角背景 | Spine（`.skel` + `.atlas` + 贴图） | 贴图例：ironclad 选角 sheet **3716×2428** | 场景根 Control 参考布局约 1920×1200 量级；**不是**单张立绘 PNG |
| `{id}_transition.png` | PNG（灰度遮罩） | **2560×1200** | 给 wipe shader 的 `transitionTex` |
| `map_marker_{id}.png` | PNG（透明） | **49×64**（竖图） | 图集另有 46×60 切片；以 packed 为准 |
| 卡面 `energy_{id}` | PNG | **74×74** | 对应 BaseLib `BigEnergyIconPath` |
| 文本 `{id}_energy_icon` | PNG | **24×24** | 对应 BaseLib `TextEnergyIconPath` |
| 能量球 `*_orb_layer_1..5` | PNG（透明） | **256×256** 每层 | 计数器 Control **128×128**，纹理拉伸；Layer2/3 旋转 |
| 能量球场景 | `.tscn` + `NEnergyCounter.cs` | 根 Control 128×128 | 含 `%Layers` / `%RotationLayers` / Label / 前后 VFX |
| 战斗角色 | Spine | Bounds 约 **242×278**（ironclad：±121 × −278..0）；Spine 缩放约 0.28 | 含 `%Visuals` / `Bounds` / `CenterPos` / `IntentPos` |
| 猜拳手 ×4 | PNG | **422×1200** | point / rock / paper / scissors |
| 时间线 epoch 像 | PNG | 例 **833×533** | 非角色 UI 刚需 |

文件类型结论：

- **静态 UI（头像/选角钮/地图标/能量图标）→ PNG**
- **会动的角色表现（选角大背景、战斗、篝火、商人）→ Spine（`.skel` + `.atlas` + `.png`）+ `.tscn`**
- **战斗能量球 → 多层 PNG + 专用 `.tscn`（不是一张球图了事）**
- **转场 → 宽屏遮罩 PNG + `.tres` ShaderMaterial**

## 万敌 charui 对照（本地 `WandiMod/WandiMod/images/charui`）

| 文件 | 本地尺寸 | vs 原版 | 建议 |
|---|---|---|---|
| `text_energy.png` | 24×24 | ✅ 对齐 | 保持 |
| `big_energy.png` | 134×134 | 原版 74×74 | 可用（偏大超采样 OK）；要严对齐可改 74×74 |
| `char_select_wandi_locked.png` | 132×195 | ✅ | 保持 |
| `char_select_wandi.png` | 600×847 | 比例≈原版 | 可用；追求锐利可导出 **132×195** 或 2× **264×390** |
| `character_icon_wandi.png` | 510×510 | 原版 88×88 | 建议导出 **88×88**（或 176×176）方图，减少顶栏模糊/留白 |
| `map_marker_wandi.png` | 510×510 | 原版 **49×64 竖图** | ⚠️ **重导出竖图**（49×64 或 98×128） |
| `char_select_art_wandi.png` | 1067×600 | 原版 Spine | 已接 `char_select_bg_wandimod.tscn` 作静态替代，可接受 |

---

# 角色主题配置（卡牌边框 / 能量 / 角色模型）

> 万敌「像战士」的根因与覆写方法；尺寸规格以上一节原版调研为准。

## 根因：PlaceholderCharacterModel 回退到战士

`WandiMod : PlaceholderCharacterModel`（BaseLib）。`PlaceholderCharacterModel` 把**所有未 override 的 `Custom*Path` 属性**指向 `PlaceholderID = "ironclad"`（见 [PlaceholderCharacterModel.cs](../BaseLib-StS2/Abstracts/PlaceholderCharacterModel.cs)）。

→ **任何不覆写的主题属性 = 战士的资产**。图标/选角/地图标/选角背景路径已覆写；能量计数器、战斗模型、篝火/商人等仍默认战士。

## 总览：主题属性归属与当前状态

| 主题元素 | 配置位置（文件 / 属性） | 当前万敌 |
|---|---|---|
| **卡牌边框/背景色** | `WandiModCardPool` 的 `H`/`S`/`V`（HSV 着色） | ✅ 血红（H=0/S=1/V=0.6） |
| **卡牌能量图标**（牌面右下 + 文本 `{E}`） | `WandiModCardPool` 的 `BigEnergyIconPath`/`TextEnergyIconPath` | ✅ PNG 已就位（Publish 后生效） |
| **选角钮 / 锁定 / 顶栏头像 / 地图标** | `WandiMod` 各 `Custom*Path` → `charui/` | ✅ 路径+PNG 已有（地图标建议改竖图） |
| **选角大背景** | `CustomCharacterSelectBg` → 静态 `.tscn` + `char_select_art_wandi.png` | ✅ 静态替代（非 Spine） |
| **战斗能量球** | `CustomEnergyCounterPath` / `CustomEnergyCounter` | ❌ 未覆写 → 战士 |
| **战斗角色模型** | `CustomVisualPath` / `CreateCustomVisuals()` | ❌ 未覆写 → 战士 |
| 能量数字描边色 | `WandiMod.EnergyLabelOutlineColor` | ✅ 血红 B71C1C |
| 角色名颜色 | `WandiMod.NameColor` | ✅ 血红 B71C1C |
| 出牌轨迹 / 转场 / 音效 / 篝火 / 商人 / 猜拳手 | 各 `Custom*Path` | ❌ 多为战士 |

## 1. 卡牌边框 / 背景色 — `WandiModCardPool : CustomCardPoolModel`

卡牌的彩色边框由**卡池**的着色器材质决定（不是单张卡）。三选一：

| 方式 | 属性 | 说明 |
|---|---|---|
| **HSV 着色**（最简，推荐） | `H` / `S` / `V`（0–1，或 `ShaderColor` 派生） | 对基础边框图（`card_frame_red`）做 HSV 位移。万敌想要虚数金 → 调 `H` 到金黄区（约 0.12），`S`/`V` 适度。**作用在已着色图上，需实验调参**（见现有注释） |
| 自定义边框图 | `CustomFrame(CustomCardModel)` → `Texture2D` | 返回自定义 PNG（如 `images/cards/frame.png`），完全替换边框美术 |
| 自定义着色器 | `CardFrameMaterialPath` | 指向自己的 `.tres` ShaderMaterial（高级，一般不用） |

附带：`DeckEntryCardColor`（牌组列表里小卡牌图标的着色）。

> 实现：`CustomCardPoolMaterialPatch`（[CustomCardPoolModel.cs:90](../BaseLib-StS2/Abstracts/CustomCardPoolModel.cs#L90)）用 `ShaderUtils.GenerateHsv(H,S,V)` 自动生成材质。

**万敌现状**：[WandiModCardPool.cs](../WandiMod/WandiModCode/Character/WandiModCardPool.cs) `H=0/S=1/V=0.6` 血红（纯代码，无需图片）。

## 2. 卡牌能量图标（牌面右下角 + 卡牌文本里的能量符号）

| 属性 | 目录 | 文件名 | 原版规格 |
|---|---|---|---|
| `BigEnergyIconPath`（牌面大图标） | `images/charui/` | `big_energy.png` | 74×74 PNG |
| `TextEnergyIconPath`（文本 `{E}` 小图标） | `images/charui/` | `text_energy.png` | 24×24 PNG |

**卡牌/遗物描述文本内嵌能量图标的写法**：直接在描述字符串里写 BBCode
`[img]res://WandiMod/images/charui/text_energy.png[/img]`（数字照常走 `{Energy:diff()}` 等变量）。
不建议用原版 `{Var:energyIcons()}` 格式化器——BaseLib 的 `CustomEnergyIconPatches.TextIconPatch`
会把整个占位输出替换成单张 `[img]`（连数量数字一起吞掉），「获得 2 点能量」会丢失数量信息。
（万敌已用例：涅槃 / 湮灭之枪 / 蓄能突涌 / 黄金之瓮。）

- 原版用 `EnergyColorName` → `atlases/ui_atlas.sprites/card/energy_{name}.png`（74×74）；Mod 用两张独立 PNG 更简单。
- 走 [CustomEnergyIconPatches.cs](../BaseLib-StS2/Patches/UI/CustomEnergyIconPatches.cs) 的 Harmony 补丁分发。

**万敌现状**：两张 PNG 已在 `charui/`（Publish 后生效）。

## 3. 战斗能量球（右上角） — `WandiMod`

原版不是「一张能量球图」，而是：

1. 场景 `scenes/combat/energy_counters/{id}_energy_counter.tscn`（根 Control **128×128**，脚本 `NEnergyCounter`）
2. 最多 **5 层** `*_orb_layer_*.png`，每层 **256×256**（Layer2/3 在 `%RotationLayers` 里旋转）
3. 前后粒子 VFX 子场景（`scenes/vfx/energy/{id}/...`）
4. 共用材质 `materials/ui/energy_orb_dark.tres`；数字描边色走 `EnergyLabelOutlineColor`

### 方式 A：`CustomEnergyCounterPath`（完整自定义场景，推荐）

```csharp
public override string CustomEnergyCounterPath =>
    "res://WandiMod/scenes/combat/energy_counters/wandi_energy_counter.tscn";
```
- BaseLib 把标准 Godot 节点运行时转成 `NEnergyCounter`（见 [CustomCharacterModel.cs](../BaseLib-StS2/Abstracts/CustomCharacterModel.cs)）。
- 制作：复制原版 `ironclad_energy_counter.tscn` 结构 → 替换图层 PNG / 粒子色 → Publish。
- **资源清单**：`wandi_orb_layer_1..5.png`（256×256，可先做 3 层）+ `.tscn`。

### 方式 B：`CustomEnergyCounter`（legacy）

```csharp
public override CustomEnergyCounter? CustomEnergyCounter =>
    new(pathFunc: energy => "charui/energy.png".ImagePath(),
        outlineColor: new Color("B71C1C"),
        burstColor: new Color("B71C1C"));
```
- 底层仍挂 `ironclad_energy_counter` 场景，只换贴图/描边/爆发色；**缺 pathFunc 指向的 PNG 会空白**。

> **只改色/单图** → B；**完整血红能量球** → A（对照原版多层）。

附带：`EnergyLabelOutlineColor`（已血红）。

## 4. 战斗角色模型 — `CustomVisualPath` / `CreateCustomVisuals()`

战斗里的角色立绘来自 `creature_visuals/{id}` 场景，实例化为 `NCreatureVisuals` 节点（[CharacterModel.cs:120-122](../_src/sts2_src/MegaCrit/sts2/Core/Models/CharacterModel.cs#L120)）。`PlaceholderCharacterModel` 默认指向 `creature_visuals/ironclad` → 战士模型。

### NCreatureVisuals 需要的子节点（[NCreatureVisualsFactory](../BaseLib-StS2/Utils/NodeFactories/NCreatureVisualsFactory.cs)）

| 节点 | 类型 | 必须？ | 作用 |
|---|---|---|---|
| `%Visuals` | Node2D / Sprite2D（`unique_name_in_owner=true`） | ✅ 必须 | 角色立绘/动画本体 |
| `Bounds` | Control | 可选（缺省 240×280） | **受击框/点击区**（敌方选目标、鼠标点选） |
| `%CenterPos` / `IntentPos` / `%OrbPos` / `%TalkPos` | Marker2D | 可选 | 锚点（意图图标/宝珠/对话气泡位置），缺则按 Bounds 自动生成 |
| `%PhobiaModeVisuals` | Node2D | 可选 | 恐惧模式视觉（玩家角色一般不用） |

> 缺失的 `Bounds` / 锚点节点由工厂按 Bounds 尺寸**自动补齐**；只有 `%Visuals` 必须自己提供。

### 三种配置方式（由简到繁）

#### 方式 A：单张立绘 PNG（最简，静态立绘，推荐先上）

BaseLib 工厂能从一张 Texture2D 直接生成完整 `NCreatureVisuals`（自动建 `Bounds`=图×1.1 + `Sprite2D` 居中，见 [工厂 CreateBareFromResource(Texture2D)](../BaseLib-StS2/Utils/NodeFactories/NCreatureVisualsFactory.cs#L24)）。在 `WandiMod.cs` override：

```csharp
using BaseLib.Utils.NodeFactories;   // NodeFactory<NCreatureVisuals>
using Godot;

public override NCreatureVisuals? CreateCustomVisuals()
{
    // 单张立绘 → 自动生成战斗视觉（静态）。Bounds 按图片尺寸×1.1 自适应。
    var tex = ResourceLoader.Load<Texture2D>("charui/wandi_combat.png".ImagePath());
    return NodeFactory<NCreatureVisuals>.CreateFromResource(tex);
}
```

- 只需**一张透明 PNG**（角色立绘），无需 Godot 场景、无需动画。
- 代价：**静态**（无受击/攻击/死亡动画），但能立刻去掉"战士脸"。

#### 方式 B：自定义场景（.tscn，支持动画，最灵活）

override `CustomVisualPath` 指向自己的场景，`CreateCustomVisuals()` 返回 null（走默认 `Instantiate<NCreatureVisuals>()` + BaseLib 自动转换）：

```csharp
public override string? CustomVisualPath => "res://WandiMod/scenes/creature_visuals/wandi.tscn";
public override NCreatureVisuals? CreateCustomVisuals() => null;  // 走场景 + 自动转换
```

在 MegaDot/Godot 编辑器里搭 `.tscn`（根节点 `Node2D`）：
- 子节点 `%Visuals`（`Sprite2D`，或 `AnimationPlayer`+`Sprite2D`）放立绘/动画。
- 可选子节点 `Bounds`（`Control`）精确调整受击框。
- 其余锚点（`CenterPos` 等）缺省自动补。

⚠️ 工厂**不自动建 AnimationPlayer**——动画要在场景里自己加（`AnimationPlayer` 或 Spine `.skel`+`.atlas`+`.png`），见 [auto_conversion.md「Known Limitations」](../BaseLib-StS2/docs/auto_conversion.md)。

#### 方式 C：全代码构建（`CreateCustomVisuals` 返回自建节点）

完整程序化控制。可 `NodeFactory<NCreatureVisuals>.CreateFromScene(scenePath)` 从场景建，或方式 A 的 `CreateFromResource(texture)`，再自行微调 Bounds / 锚点 / 动画状态。

### 需要的资源

| 资源 | 类型 | 尺寸 | 说明 |
|---|---|---|---|
| 立绘（方式 A） | **透明 PNG** | 建议高度 ≥280；宽高比接近原版 Bounds **约 242×278** | 放 `images/charui/wandi_combat.png`；工厂 Bounds≈图×1.1 |
| 受击框 Bounds | 场景 `Control` | ironclad 约 242×278 | 点击/选区 |
| 动画（方式 B） | Spine `.skel`+`.atlas`+`.png` 或 AnimationPlayer | 原版战斗 sheet 多页（例 ironclad.png 1000×269 等） | 受击/攻击/死亡/待机 |

### 参考原生 / 覆盖范围

- 对照：`game_source/scenes/creature_visuals/ironclad.tscn` + `animations/characters/ironclad/`。
- **覆盖范围差异**：方式 B（注册场景路径）覆盖战斗 / 图鉴 / GameOver；方式 A（`CreateCustomVisuals`）主要覆盖战斗内 `CreateVisuals()`。

> ⚠️ 场景/图片改动后**必须 Publish**。方式 A：立绘 PNG + 改 `WandiMod.cs` + Publish。

## 5. 其他仍在用战士的主题项（按需覆写）

| 属性 | 用途 | 原版类型与规格 |
|---|---|---|
| `CustomTrailPath` | 出牌轨迹 | `.tscn` 粒子场景（非单 PNG） |
| `CustomCharacterSelectTransitionPath` | 选角转场 | **2560×1200** 遮罩 PNG + `.tres` |
| `CharacterSelectSfx` / `CharacterTransitionSfx` / `CustomAttackSfx` 等 | 音效 | FMOD event |
| `CustomRestSiteAnimPath` / `CustomMerchantAnimPath` | 篝火/商人 | Spine `.tscn` |
| `CustomArm*TexturePath`（×4） | 联机猜拳 | PNG **422×1200** ×4 |
| `CustomIconOutlineTexturePath` | 头像描边 | PNG **88×88** |

覆写：在 `WandiMod.cs` 指向自己的 `res://WandiMod/...` 资源。

## 快速「去战士化」优先级清单

| 状态 | 改动 | 类型 | 工作量 |
|---|---|---|---|
| ✅ | 主题色 / HSV / 描边 / 地图画线 | 纯代码 | — |
| ✅ | charui：能量图标、选角钮、头像、地图标、选角静态大图 | PNG + 场景 | 需 Publish；地图标建议改竖图 |
| 🔴 下一步 | 战斗静态立绘 `wandi_combat.png` + `CreateCustomVisuals` | 1 张透明 PNG + 代码 | 小 |
| 🟠 | 能量球：复制 ironclad 计数器场景 + `orb_layer` 256×256 ×3～5 | PNG + `.tscn` | 中 |
| 🟡 | 修正 `map_marker` 为 49×64 竖图；头像压到 88×88 | 重导出 PNG | 小 |
| 🟡 | 选角转场 2560×1200 + mat | PNG + `.tres` | 中 |
| 🟣 后期 | Spine 战斗/篝火/商人、猜拳手、出牌轨迹、SFX | Spine/场景/音频 | 大 |

> 主题色：**血红** `B71C1C`（HSV 约 H=0/S=1/V=0.6，需游戏内微调）。  
> 代码色 Build 即生效；图片/场景必须 **Publish**。

