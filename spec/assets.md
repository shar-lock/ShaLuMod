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
└── charui/            ← 角色界面（图标/选择/能量等）
    ├── character_icon_wandi.png
    ├── char_select_wandi.png
    └── ...
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

### 角色界面

在 `WandiMod.cs` 中手动覆写路径（不走 RemovePrefix 机制，直接写字符串）：

| 属性 | 目录 | 文件名（硬编码在 WandiMod.cs） |
|---|---|---|
| `CustomIconTexturePath` | `images/charui/` | `character_icon_wandi.png` |
| `CustomCharacterSelectIconPath` | | `char_select_wandi.png` |
| `CustomCharacterSelectLockedIconPath` | | `char_select_wandi_locked.png` |
| `CustomMapMarkerPath` | | `map_marker_wandi.png` |

- `CharacterUiPath()` **没有回退机制**——文件缺失直接空白

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

# 角色主题配置（卡牌边框 / 能量 / 角色模型）

> 调研对象：万敌在游戏里「卡牌背景色、能量颜色、能量背景色、战斗角色模型」都显示成战士（Ironclad）的根因与配置方法。

## 根因：PlaceholderCharacterModel 回退到战士

`WandiMod : PlaceholderCharacterModel`（BaseLib）。`PlaceholderCharacterModel` 把**所有未 override 的 `Custom*Path` 属性**指向 `PlaceholderID = "ironclad"`（见 [PlaceholderCharacterModel.cs](../BaseLib-StS2/Abstracts/PlaceholderCharacterModel.cs)）。

→ **任何不覆写的主题属性 = 战士的资产**。万敌目前只覆写了图标/选角图等少量项，能量计数器、战斗角色模型等仍是战士。

## 总览：主题属性归属与当前状态

| 主题元素 | 配置位置（文件 / 属性） | 当前万敌 |
|---|---|---|
| **卡牌边框/背景色** | `WandiModCardPool` 的 `H`/`S`/`V`（HSV 着色） | ✅ 血红（H=0/S=1/V=0.6） |
| **卡牌能量图标**（牌面右下 + 文本 `{E}`） | `WandiModCardPool` 的 `BigEnergyIconPath`/`TextEnergyIconPath` | ⚠️ 路径已设，PNG 待补 |
| **战斗能量球 + 背景面板**（右上角） | `WandiMod.CustomEnergyCounterPath`（场景）或 `CustomEnergyCounter`（legacy） | ❌ 未覆写 → 战士（需场景/PNG） |
| **战斗角色模型**（战斗内人物） | `WandiMod.CustomVisualPath`（`creature_visuals` 场景） | ❌ 未覆写 → 战士 |
| 能量数字描边色 | `WandiMod.EnergyLabelOutlineColor` | ✅ 血红 B71C1C |
| 角色名颜色 | `WandiMod.NameColor` | ✅ 血红 B71C1C |
| 卡牌出牌轨迹 / 地图标 / 选角背景 / 音效 / 篝火 / 商人 / 猜拳手势 | `PlaceholderCharacterModel` 各 `Custom*Path` | 多为战士 |

## 1. 卡牌边框 / 背景色 — `WandiModCardPool : CustomCardPoolModel`

卡牌的彩色边框由**卡池**的着色器材质决定（不是单张卡）。三选一：

| 方式 | 属性 | 说明 |
|---|---|---|
| **HSV 着色**（最简，推荐） | `H` / `S` / `V`（0–1，或 `ShaderColor` 派生） | 对基础边框图（`card_frame_red`）做 HSV 位移。万敌想要虚数金 → 调 `H` 到金黄区（约 0.12），`S`/`V` 适度。**作用在已着色图上，需实验调参**（见现有注释） |
| 自定义边框图 | `CustomFrame(CustomCardModel)` → `Texture2D` | 返回自定义 PNG（如 `images/cards/frame.png`），完全替换边框美术 |
| 自定义着色器 | `CardFrameMaterialPath` | 指向自己的 `.tres` ShaderMaterial（高级，一般不用） |

附带：`DeckEntryCardColor`（牌组列表里小卡牌图标的着色）。

> 实现：`CustomCardPoolMaterialPatch`（[CustomCardPoolModel.cs:90](../BaseLib-StS2/Abstracts/CustomCardPoolModel.cs#L90)）用 `ShaderUtils.GenerateHsv(H,S,V)` 自动生成材质。

**万敌现状**：[WandiModCardPool.cs:19-21](../WandiMod/WandiModCode/Character/WandiModCardPool.cs#L19) `H=S=V=1f` → 等于不着色。改成金色 hue 即可（纯代码，无需图片）。

## 2. 卡牌能量图标（牌面右下角 + 卡牌文本里的能量符号）

| 属性 | 目录 | 文件名 |
|---|---|---|
| `BigEnergyIconPath`（牌面大图标） | `images/charui/` | `big_energy.png` |
| `TextEnergyIconPath`（文本 `{E}` 小图标） | `images/charui/` | `text_energy.png` |

- 或用 `EnergyColorName` 指向 `images/atlases/ui_atlas.sprites/card/energy_{name}.tres`（高级）。
- 走 [CustomEnergyIconPatches.cs](../BaseLib-StS2/Patches/UI/CustomEnergyIconPatches.cs) 的 Harmony 补丁分发。

**万敌现状**：路径已在 WandiModCardPool 设好，但 `charui/big_energy.png` / `text_energy.png` 缺失 → 回退默认。补两张 PNG + Publish 即可。

## 3. 战斗能量球 + 背景面板（右上角能量区） — `WandiMod`

这是「能量的颜色 + 能量的背景色」的真正出处。两种 API：

### 方式 A：`CustomEnergyCounterPath`（完整自定义场景，推荐，能改背景）

override 返回一个 Godot 场景路径（`.tscn`）：
```csharp
public override string CustomEnergyCounterPath =>
    SceneHelper.GetScenePath("combat/energy_counters/wandi_energy_counter");
```
- BaseLib 把**标准 Godot 节点**（Control / Label / TextureRect / Node2D / GpuParticles2D）运行时自动转成 `NEnergyCounter`——无需手写 C# 脚本（见 [CustomCharacterModel.cs:74-77,209](../BaseLib-StS2/Abstracts/CustomCharacterModel.cs#L74)）。
- 场景内含**能量球 + 背景面板**，完全自定义（颜色/图片/形状全控）。
- 制作：在 MegaDot/Godot 编辑器里搭一个 `.tscn`，Publish 打包。

### 方式 B：`CustomEnergyCounter`（legacy，纯代码，但不能改背景）

```csharp
public override CustomEnergyCounter? CustomEnergyCounter =>
    new(pathFunc: energy => "charui/energy.png".ImagePath(),
        outlineColor: new Color("D4AF37"),
        burstColor: new Color("D4AF37"));
```
- `CustomEnergyCounter(Func<int,string> pathFunc, Color outlineColor, Color burstColor)`（[CustomCharacterModel.cs:213](../BaseLib-StS2/Abstracts/CustomCharacterModel.cs#L213)）。
- **底层仍用 `ironclad_energy_counter` 场景做底**（[CustomCharacterModel.cs:511](../BaseLib-StS2/Abstracts/CustomCharacterModel.cs#L511)）→ 只换图标/描边色/爆发色，**背景面板仍是战士的**。

> 结论：**只改能量球颜色/图标** → 方式 B（快）；**要改背景面板** → 必须方式 A（自定义场景）。

附带：`EnergyLabelOutlineColor`（能量数字的描边色，CharacterModel 上，可单独 override）。

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
| 立绘 | **透明 PNG** | 约 240×280 起步（Bounds 默认），更大更清晰（工厂按图×1.1 自适应） | 角色半身/全身立绘，**透明背景**；放 `images/charui/wandi_combat.png` |
| 受击框 Bounds | 场景 `Control` | 默认 240×280 | 决定点击/选区；场景方式可调，PNG 方式按图自动 |
| 动画（可选） | `AnimationPlayer` 或 Spine `.skel`+`.atlas`+`.png` | — | 受击/攻击/死亡/待机；仅方式 B/C 支持 |

### 参考原生 / 覆盖范围

- 想看战士立绘的结构与尺寸作参照：用 wiki [Extracting-Assets-and-Text](../ModTemplate-StS2.wiki/Extracting-Assets-and-Text.md) 提取 `creature_visuals/ironclad` 场景与贴图。
- **覆盖范围差异**：方式 B（注册场景路径）能透明覆盖所有 `Instantiate<NCreatureVisuals>` 路径——含玩家战斗、**怪物图鉴（Bestiary）、游戏结束界面（GameOverScreen）**；方式 A（`CreateCustomVisuals`）只覆盖玩家战斗内的 `CreateVisuals()`。

> ⚠️ 场景/图片改动后**必须 Publish**（Build 只编译 .dll，不打包资源）。方式 A 仅需立绘 PNG + Publish + 改 `WandiMod.cs`（Build）。

## 5. 其他仍在用战士的主题项（按需覆写）

`PlaceholderCharacterModel` 默认指向 ironclad 的属性（[源码](../BaseLib-StS2/Abstracts/PlaceholderCharacterModel.cs)）：

| 属性 | 用途 | ironclad 路径 |
|---|---|---|
| `CustomTrailPath` | 出牌轨迹特效 | `vfx/card_trail_ironclad` |
| `CustomMapMarkerPath` | 地图角色标记 | `packed/map/icons/map_marker_ironclad.png` |
| `CustomCharacterSelectBg` | 选角界面背景 | `screens/char_select/char_select_bg_ironclad` |
| `CustomCharacterSelectTransitionPath` | 选角转场材质 | `materials/transitions/ironclad_transition_mat.tres` |
| `CharacterSelectSfx` / `CharacterTransitionSfx` / `CustomAttackSfx` 等 | 音效 | `event:/sfx/characters/ironclad/...` |
| `CustomRestSiteAnimPath` / `CustomMerchantAnimPath` | 篝火/商人动画 | `rest_site/characters/ironclad_*` / `merchant/characters/ironclad_*` |
| `CustomArm*TexturePath`（×4） | 联机猜拳手势 | `ui/hands/multiplayer_hand_ironclad_*.png` |

覆写方式：在 `WandiMod.cs` override 对应属性，指向自己的资源路径（多数走 `SceneHelper.GetScenePath(...)` 或 `ImageHelper.GetImagePath(...)`）。

## 快速「去战士化」优先级清单

| 状态 | 改动 | 类型 | 工作量 |
|---|---|---|---|
| ✅ 已完成 | `WandiModCardPool` 的 `H`/`S`/`V` 改血红（H=0/S=1/V=0.6） | 纯代码 | Build 即生效 |
| ✅ 已完成 | `WandiMod.NameColor` + `EnergyLabelOutlineColor` 改血红 B71C1C | 纯代码 | Build 即生效 |
| ✅ 已完成 | `WandiModCardPool.DeckEntryCardColor`（牌组小图标）改血红 | 纯代码 | Build 即生效 |
| 🔴 高 | 补 `charui/big_energy.png` + `text_energy.png`（卡牌能量图标） | 2 张 PNG + Publish | 小 |
| 🟡 中 | `CustomEnergyCounter` legacy 换能量球颜色 | ⚠️ **需配 PNG**（pathFunc 按图层重建能量球，缺图→球空白；非纯代码） | 中 |
| 🟠 大 | `CustomEnergyCounterPath` 自定义场景（换能量背景面板） | Godot 场景 + Publish | 中 |
| 🟡 中 | 战斗角色模型（[详见 §4](#4-战斗角色模型--customvisualpath--createcustomvisuals)） | 方式 A 单张 PNG（静态，小）/ 方式 B 场景+动画（大） | 小～大 |

> 当前主题色统一为**血红色**（`B71C1C`，HSV: H=0/S=1/V=0.6）。HSV 是近似值，**需游戏内微调**（V 调深浅）。
>
> ⚠️ 代码色（NameColor/EnergyLabelOutlineColor/HSV/DeckEntryCardColor）**Build 即生效**；图片/场景改动必须 **Publish**。见上文「Build vs Publish」。

