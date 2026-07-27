> [!IMPORTANT]
> 熟悉 C# 和面向对象编程对于想要为杀戮尖塔 2（StS2）制作模组的人来说非常有帮助。如果你是编程新手，或者刚接触 C#，建议先找一些其他资源学习 C#，例如 [Codeacademy 的课程](https://www.codecademy.com/learn/learn-c-sharp) 或 [Essential C#](https://essentialcsharp.com/home) 之类的文字教程。

这些步骤将帮助你着手开发一个基于 [BaseLib](https://github.com/Alchyr/BaseLib-StS2) 的项目，BaseLib 是一个社区开发的库，能够帮助简化模组开发流程。

1. 安装一个 C# IDE。[Rider](https://www.jetbrains.com/rider/) 或 [Visual Studio](https://visualstudio.microsoft.com/) 都是常见的选择。**推荐使用 Rider**，因为 Godot 需要 .sln 文件，而 Visual Studio 正在逐步弃用这类文件，这会让生成使用它的项目变得更困难。本教程将主要在 Rider 的语境下进行讲解。
2. 下载依赖

- 下载最新版的 [MegaDot](https://megadot.megacrit.com/)（Mega Crit 定制版的 Godot）
  - 作为备选方案，你也可以使用与最新 MegaDot 版本完全一致的 [Godot .NET](https://godotengine.org/download/archive/) 版本
- 安装 [.NET SDK](https://dotnet.microsoft.com/en-us/download)（9.0 或更高版本）
- 在 [Steam 创意工坊订阅 BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127)。这会把它放到你的 Steam 安装目录下的一个文件夹中，路径为 `Steam/steamapps/workshop/content/2868840/3737335127/BaseLib`。
  - 如果出于某些原因这行不通，你可以[从 GitHub 下载 BaseLib 的最新 release](https://github.com/Alchyr/BaseLib-StS2/releases/)，并把它放到你的杀戮尖塔 2 模组文件夹中。（如果你不确定在哪里，请参考[这个页面](https://github.com/Alchyr/ModTemplate-StS2/wiki/Modding-Basics#mod-files-and-where-they-go)。）你可以下载 `.zip` 并解压文件，或者分别下载 `.dll`、`.pck` 和 `.json`。（这种设置方式更复杂，而且意味着你不会自动获取 BaseLib 的新版本。）

3. 在你的 IDE 中打开一个终端（在 Rider 中：先创建一个新的解决方案作为占位，然后根据你的键位绑定按 `Alt+F12` 或 ``Ctrl+` ``，或者点击左下角的 Terminal 按钮 ），运行命令 `dotnet new install Alchyr.Sts2.Templates` 来从 NuGet（.NET 内置的包管理器）安装模板。如果没有打开终端的选项，请创建一个新的解决方案让它显示出来。你也可以在任何可以使用 dotnet sdk 的命令行中运行此命令。

* 或者，你可以从 GitHub 下载本项目，并按照[这里](https://www.jetbrains.com/help/rider/Install_custom_project_templates.html#create-custom-project-template)从第 7 步开始的内容，将其作为模板添加到 Rider 中。

4. 转到 `File` > `New Solution`，使用三个内置模板之一创建解决方案：

- **Slay the Spire 2 Character** - 用于创建新角色的模板
- **Slay the Spire 2 Content** - 用于添加新内容（卡牌、遗物、药水等）的模板
- **Empty Slay the Spire 2 Mod** - 一个最简模板，适用于其他任何需求

<img hspace="40" width="902" height="693" alt="图片" src="https://github.com/user-attachments/assets/13fc965c-18bc-4ffd-9d7f-ed34dbc3b35d" />

创建模板时：

- 项目名不能包含任何空格
- 格式必须为 `.sln`
- **勾选 `Put solution and project in same directory` 复选框**
- 展开 "Advanced Settings"（在 Rider 中）来调整作者以及其他一些选项。

5. 打开 `Directory.Build.props` 文件。找到 `<GodotPath>` 属性；如果你的 MegaDot 安装位置不在默认路径，把它改成可执行文件所在的路径（如果这里设置错误，下面第 8 步中的发布将无法工作）。不要在路径外加引号。
   此外，如果杀戮尖塔 2 的路径没有被自动找到，也是在这个文件中调整。可以尝试按 `Build` 按钮（顶部工具栏上的锤子图标，或者通过左下角的 Build 菜单）。

<img hspace="40" width="231" height="104" alt="图片" src="https://github.com/user-attachments/assets/55f4916e-7324-4152-b0fe-217d646b8404" align="left"/>

如果找不到杀戮尖塔 2，你会得到类似这样的错误 `Error  :  Slay the Spire 2 data not found at path '?/steamapps/common/Slay the Spire 2/data_sts2_windows_x86_64'`。要修复此问题，请取消 `<Sts2Path>` 行的注释，并将其设置为你安装 StS 2 的目录。

如果你使用的是角色模板，并且遇到了与本地化相关的错误，那么项目其实已经正确设置好了。

6. 你可以在模组的清单 `.json` 中修改模组的显示名称。该文件将与项目同名。不要修改 id；它决定了游戏会尝试加载的文件名。其他值可以根据你正在制作的模组进行修改；详情请参见[模组清单文档](https://github.com/Alchyr/ModTemplate-StS2/wiki/Modding-Basics#mod-manifest)。
7. 如果你使用的是角色模板，你需要为角色生成本地化。在 `YourModCode/Character/YourMod.cs` 中打开角色类。它应该会有两个这样的错误；一个是 "character" 本地化的，一个是 "ancient" 本地化的。

<img hspace="40" width="1099" height="132" alt="图片" src="https://github.com/user-attachments/assets/1766bd33-d989-478b-a13d-ee55d76423b8" />

对每一个，按 alt+enter 并选择 "Generate localization"，然后把生成的文本移动到对应的文件中（`localization/eng/characters.json` 和 `localization/eng/ancients.json`）。

<img width="488" height="83" alt="图片" src="https://github.com/user-attachments/assets/af612cea-8e18-4006-9b45-46e126a382a3" />

卡牌、遗物、先古之民以及其他内容也可以用类似的方式生成本地化。你可以在[这里](https://github.com/Alchyr/StS2ModAnalyzers/blob/master/ModAnalyzers/ModAnalyzers/LocalizationAnalyzer.cs#L28)找到支持的类型完整列表。

8. 要让你的模组的本地化、图片以及其他任何非代码改动生效，你必须发布（publish）你的模组（而不是仅仅构建）。要设置发布，请在左侧栏中右键点击项目，选择 `Publish`。在 Rider 中，选择 `Local folder`。发布选项可以保持默认。发布会编译你的模组 `.dll`，生成一个包含你模组资源的 `.pck`，并将模组文件（`.dll`、`.pck` 和 `.json`）复制到杀戮尖塔 2 的模组文件夹。如果在 `Directory.Build.props` 中 Godot 的路径设置不正确，尝试发布会报错。

<img width="536" height="329" alt="图片" src="https://github.com/user-attachments/assets/1acda21b-8361-4d8b-809f-b95158951b72" align="right" />

由于通过 Godot 生成 `.pck` 的过程比较慢，发布速度会有些慢。如果你只修改了代码文件，可以改用 `Build`，点击顶部工具栏上的锤子按钮。这只会编译包含你代码的 `.dll` 并将其复制到模组文件夹。

如果系统上的 dotnet 没有正确设置，可能会出现发布问题；可以在 GodotPublish 命令开头加上 `DOTNET_ROOT=~/.dotnet`（替换为 dotnet 所在的有效路径）来绕过。

> [!WARNING]
> 每次做任何非代码改动（本地化、图片、场景等）后，你都必须发布才能让它们生效。

## 运行游戏

在构建或发布之后，你应该能够运行游戏，并在模组列表中看到你的模组（在 Settings -> Mod Settings 下）。

## 后续步骤

要开发一个模组，你还需要更多信息。以下是你应该从哪里开始的内容。

* 请注意 [Modding Basics](https://github.com/Alchyr/ModTemplate-StS2/wiki/Modding-Basics) 中所有通用的有用信息
* 学习如何[反编译游戏](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)以及[提取游戏资源和文本](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text)；看看原版游戏是怎么做事的是最好的学习方法之一
* 了解[测试和调试](https://github.com/Alchyr/ModTemplate-StS2/wiki/Testing-and-Debugging)的工作方式，这样你就能检查你的代码是否工作、理解错误并修复问题
* 通读[从主页链接的其他参考资料](https://github.com/Alchyr/ModTemplate-StS2/wiki#adding-content)，涵盖了特定类型的内容、外部资源等等

## 故障排除

- 错误：**`[MSB4236] The SDK 'Godot.NET.Sdk/4.5.1' specified could not be found.`**
  在终端中运行以下命令

```
dotnet nuget add source https://api.nuget.org/v3/index.json
```

- 错误：**`[MSB4236] The SDK 'Microsoft.NET.SDK.WorkloadAutoImportPropsLocator' specified could not be found.`**
  转到 `File` > `Settings`，搜索 `toolset`，然后将 `MSBuild version` 改为一个提到 rider 的版本

<img width="1000" alt="change-msbuild" src="https://github.com/user-attachments/assets/c25178d7-ac6a-4f0d-b988-f588f0355700" />
