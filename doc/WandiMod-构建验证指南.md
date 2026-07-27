# WandiMod 构建验证指南

> **用途**：把 `WandiMod/` 工程从「代码」变成「游戏里能加载的角色」的完整步骤。
> **平台**：Windows · Rider（推荐）· StS2 Steam 版。
> **配套文档**：[万敌Mod-设计方案.md](万敌Mod-设计方案.md) · 官方 [Setup.md](../ModTemplate-StS2.wiki/Setup.md) · [Testing-and-Debugging.md](../ModTemplate-StS2.wiki/Testing-and-Debugging.md)

---

## 〇、前置准备（只需做一次）

| 依赖 | 说明 |
|---|---|
| **Rider**（推荐）| [下载](https://www.jetbrains.com/rider/)。VS 也行，但 wiki 不推荐（Godot 要 `.sln`，VS 在淡化它） |
| **MegaDot 4.5.1** | Mega Crit 定制版 Godot，[下载](https://megadot.megacrit.com/)。版本不能比游戏新，否则 `.pck` 加载不了 |
| **.NET SDK 9.0+** | [下载](https://dotnet.microsoft.com/download) |
| **BaseLib** | [Steam Workshop 订阅](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127)（模板硬依赖）。订阅后会落在 `Steam/steamapps/workshop/content/2868840/3737335127/BaseLib` |

> 这套工程已自带 `WandiMod.sln`（与 `WandiMod.sln.DotSettings` 配对，加载卡牌文件模板用），无需再用 `dotnet new` 新建项目。

---

## 一、打开工程

Rider → `File` → `Open` → 选 **`WandiMod/WandiMod.sln`**。

> 用 `.sln`（而不是直接开 `.csproj`）的原因：这样 Rider 会自动加载旁边的 `WandiMod.sln.DotSettings`，里面带了「Custom Card」等文件模板，M3 加卡牌时能右键直接新建。

---

## 二、配路径（关键，决定能不能编译）

打开 `WandiMod/Directory.Build.props`，检查两项：

```xml
<!-- ① MegaDot 的 exe 实际路径（改成你自己的，用正斜杠，不要加引号） -->
<GodotPath>C:/megadot/MegaDot_v4.5.1-stable_mono_win64.exe</GodotPath>

<!-- ② StS2 一般能自动探测；若报错再取消注释并填你的安装目录 -->
<!-- <Sts2Path>D:/SteamLibrary/steamapps/common/Slay the Spire 2</Sts2Path> -->
```

- **`GodotPath`**：改成你 MegaDot exe 的真实路径（默认值是 `C:/megadot/...`，多数人要改）。
- **`Sts2Path`**：通常靠 Steam 注册表自动找到。**如果 Build 时报 `Slay the Spire 2 data not found at path '?/...'`**，就取消注释 `<Sts2Path>` 那行，填你的 StS2 安装目录。
- 编译产物会自动拷到 `$(Sts2Path)/mods/WandiMod/`（见 `Sts2PathDiscovery.props`）。

---

## 三、还原 NuGet

打开 `.sln` 后 Rider 一般会自动还原。若没有：右键 `WandiMod` 工程 → `Restore`。这会拉取 BaseLib、ModAnalyzers 等。

---

## 四、生成角色本地化（⚠️ 会出现 2 个报错，这是**正常的**）

打开 `WandiMod/WandiModCode/Character/WandiMod.cs`。你会看到 **2 个本地化报错**（一个 character、一个 ancient）。wiki 原话：*"如果你用角色模板并得到 localization 相关报错，说明工程配对了。"*

逐个处理：光标停在红色类名上 → 按 **`Alt+Enter`** → 选 **`Generate localization`** → 把生成的文本块剪切到对应文件：

- character 的 → `WandiMod/localization/eng/characters.json`
- ancient 的 → `WandiMod/localization/eng/ancients.json`

存盘后报错消失。

---

## 五、Build（编译 dll）

点顶部工具栏的 **🔨 锤子**（或 `Build` 菜单）。这会编译 `WandiMod.dll` 并自动拷到 `mods/WandiMod/`。**只改代码时用 Build 就够了，快。**

---

## 六、Publish（生成 .pck，首次必做）

⚠️ **首次、以及每次改了非代码资产（本地化、图片、场景）后，必须 Publish，否则游戏里看不到变化。**

右键左侧 `WandiMod` 工程 → `Publish` → 选 `Local folder` → 选项保持默认。这会：

1. 编译 `.dll`
2. 调用 MegaDot headless 打包出 `.pck`（含本地化/图片）
3. 把 `.dll` + `.pck` + `.json` 一起拷到 mods 文件夹

> 这步较慢（要过 Godot 打包），属正常。若报 dotnet 相关错，可在 GodotPublish 命令前加 `DOTNET_ROOT=<你的dotnet路径>` 绕过。

### Build vs Publish 速记

| 改了什么 | 用哪个 |
|---|---|
| 只改 `.cs` 代码 | **Build**（锤子，快） |
| 改了本地化 / 图片 / 场景 / 首次 | **Publish**（慢，但出 `.pck`） |

---

## 七、运行游戏验证

1. 启动 StS2 → **Settings → Mod Settings** → 启用 **WandiMod** 和 **BaseLib**。
2. 回主菜单 → **Mod Configuration → BaseLib** → 勾 **"Open log window on startup"**（方便看日志）。
3. **新游戏 → 角色选择** → 应能看到 **「Wandi (Mydei)」**（金色名字、占位立绘）。
4. 选中进战斗，确认：起手牌组（目前是 Strike/Defend 占位）、起手遗物（BurningBlood 占位）、HP=75。

---

## 八、出问题怎么查

- **Dev console**：进游戏后按 `` ~ `` 或 `` ` `` 或 `*` 或 `'` 或 `Shift+8` 打开；输 `help` 看命令，`showlog` 开日志窗，`open logs` 打开日志目录。
- **日志文件**：`%appdata%/SlayTheSpire2/logs/godot.log`。重点看提到 `WandiMod` 的 ERROR 行。
- **自己加日志**：用 `MainFile.Logger.Info("...")` 在关键代码处打点，便于定位。

---

## 九、常见报错速查

| 报错 | 解决 |
|---|---|
| `The SDK 'Godot.NET.Sdk/4.5.1' could not be found` | 终端跑 `dotnet nuget add source https://api.nuget.org/v3/index.json` |
| `... WorkloadAutoImportPropsLocator ... could not be found` | Rider → Settings 搜 `toolset`，把 MSBuild version 换成带 rider 字样的 |
| `Slay the Spire 2 data not found at path '?/...'` | 在 `Directory.Build.props` 取消注释并填 `<Sts2Path>` |
| 角色 localization 报错 | 正常，按上面**第四步**生成 |
| Publish 报 Godot 路径错 | 检查 `Directory.Build.props` 里的 `<GodotPath>` |
| 角色进战斗后是默认/占位立绘 | 预期行为：当前用 `PlaceholderCharacterModel`，真实立绘在 M4 替换 |

---

## 十、M1 验收标准

- [ ] Mod Settings 里能看到并启用 WandiMod
- [ ] 角色选择界面出现「Wandi (Mydei)」
- [ ] 能进战斗、有起手牌组和遗物（占位）
- [ ] 日志无 WandiMod 相关报错

达到这四点，M1 脚手架就算验证通过，可进入 **M2（血仇 Power + 弑亲血脉免死遗物）**。

---

## 附：调试进阶（可选）

需要打断点调试时：

1. 在 StS2 目录建 `steam_appid.txt`，内容 `2868840`（免 Steam 启动）。
2. 把 `.pdb` 拷到 mod 的 dll 同目录（或在 `.csproj` 的 `CopyToModsFolderOnBuild` Target 末尾加 `<Copy SourceFiles="$(TargetDir)$(TargetName).pdb" DestinationFolder="$(ModsPath)$(MSBuildProjectName)/" />`）。
3. Rider → Run/Debug Configurations → 新建 `.NET Executable`，填 StS2 exe 和工作目录。
4. 想单机测联机：`-fastmp host_standard` / `-fastmp join`（详见 [Testing-and-Debugging.md](../ModTemplate-StS2.wiki/Testing-and-Debugging.md)）。
