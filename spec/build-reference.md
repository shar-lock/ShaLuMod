# 构建与 API 参考

## 构建
- **本仓库不能就地 `dotnet build`**——WandiMod 引用 `sts2.dll` / `0Harmony.dll`（游戏程序集），需本机装了 StS2。
- 命令：`dotnet build WandiMod/WandiMod.csproj -c Debug`（编 dll + 拷到 mods/）。
- StS2 路径自动探测（Steam 注册表 app `2868840`）；探测失败则在 `WandiMod/Directory.Build.props` 取消注释 `<Sts2Path>` 手填。
- `GodotPath`（Directory.Build.props）= MegaDot exe 路径，仅 **Publish**（出 `.pck`）用。
- 只改代码 → Build；改本地化 / 图片 / 场景 → Publish。

## API 参考（三层，查证权威）
| 要查 | 位置 |
|---|---|
| 游戏钩子 / 方法签名（PowerModel / RelicModel / CardModel / Cmd） | `_src/sts2_src/`（反编译源，**查 API 的第一权威**） |
| BaseLib 封装（CommonActions、CustomXModel、Grant 范式） | `BaseLib-StS2/` |
| 原版卡数值 / JSON 字段 | `sts2_database/cards/*.json`（580 张，game 0.107.1） |

反编译源里找范例：直接 grep 类名（如 `RupturePower` / `WeakPower` / `LizardTail` / `Akabeko` / `OutbreakPower` / `Apparition` / `Soul`）。

## 两层 API
- **高层** `CommonActions.*`（`BaseLib-StS2/Utils/CommonActions.cs`）：`CardAttack` / `CardBlock` / `Apply` / `Draw` / `SelectCards`——自动读 Vars、处理 TargetType、VFX。**优先用**。
- **低层** `*Cmd` 流式（`DamageCmd` / `CreatureCmd` / `PowerCmd` / `CardPileCmd` / `CardSelectCmd`，见 wiki `Common-Commands-Cookbook.md`）——需手动控制时。
- ⚠️ 很多 `CommonActions.Apply` 旧重载 `[Obsolete]`，用带 `PlayerChoiceContext` 的版。

## 本地化
- 目录：`WandiMod/WandiMod/localization/{eng,zhs}/*.json`（zhs=简中，玩家主用）。
- 占位符 `{VarName:diff()}`；关键词高亮 `[gold]…[/gold]`（见 keyword-highlight.md）。
- 新 model 类用 Rider「Generate localization」代码修正生成骨架（Alt+Enter），再填文案。
- **改本地化后必须 Publish 出 `.pck` 才生效。**

## 反编译源注意（wiki `Decompiling.md`）
反编译代码有假象：`[OriginalAttributes(...)]`（Publicizer 痕迹，原码没有）、冗余强转 / `0.75M`、编译器生成类型 `<>z__...`、常量替成字符串——抄代码时简化掉。
