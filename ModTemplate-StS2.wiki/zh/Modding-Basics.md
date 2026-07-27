为杀戮尖塔 2（StS2）开发模组需要了解模组工具、如何改编现有代码，以及模组在底层是如何工作的。

本页涵盖了各种主题，大致按照你在模组开发时需要多快了解到它们来排序（理解 BaseLib 是立刻需要的，而上传你的模组则要晚得多）。

# BaseLib
[BaseLib](https://github.com/Alchyr/BaseLib-StS2) 是一个社区开发的库，能够帮助简化模组开发流程。本指南假设你在使用 BaseLib，它已经被包含在[项目模板](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup)中。

BaseLib 提供了用于配置模组、添加内容以及许多其他模组常做的事情的 API，还提供了一套避免模组之间冲突的框架。相关文档可以在 [BaseLib Wiki](https://alchyr.github.io/BaseLib-Wiki/) 上找到。[功能列表（Features List）](https://alchyr.github.io/BaseLib-Wiki/docs/Features.html) 是个不错的起点。

如果你想做的事情在本 wiki 上没有覆盖，请查看 BaseLib。这里的指南只覆盖了 BaseLib 功能的一个子集。

获取 BaseLib 的最佳方式是[在 Steam 创意工坊订阅它](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127)。

# 学习如何制作东西
一旦你设置好了项目，下一个挑战就是制作你想要的内容。游戏中几乎所有的内容都以 `Model`（例如 `CardModel`、`RelicModel`）的形式存在，并通过像 `OnPlay` 这样的共享生命周期动作接入。所有动作都由一系列命令（例如 `DamageCmd.Attack`、`PowerCmd.Apply<StrengthPower>`）组成。正是这些命令赋予了模型行为。始终应优先使用命令，而不是直接操作数据。

大多数模型在 BaseLib 中都有一个继承类（例如 `CustomCardModel`），提供了额外的支持；你通常会想要继承这些类并重写相关属性。

要实现一个具体的动作（例如施加 3 层虚弱、进入房间时治疗）：
1. 想想原版游戏里有什么内容做了类似的事；几乎总能找到。
   1. 浏览一下游戏的卡牌/遗物等列表，或者搜索游戏的文本文件，可能会有所帮助。
   1. 创造性地思考原版游戏代码中可能与你想做的相似的部分
2. 查看原版游戏用于此目的的代码（参见[反编译](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)），并将该代码改编以符合你自己的目的（例如，如果你想为一张卡牌添加力量增加，你可以去找一张给予力量的卡牌，并查看那张卡牌调用了哪些命令）。
   1. 或者，检查是否有其他模组做过类似的事情，或者 BaseLib 是否对你想做的事情有内置支持。

如果你想知道如何实现某一类 Model（例如卡牌、遗物、先古之民）：
* 查看本 wiki 上是否有相应的指南
* 检查 BaseLib 是否有 `Custom*Model` 类（例如 CustomRelicModel）并查看其文档
  * 通常你想要创建一个继承 Custom*Model 的类，并重写各种属性
  * 如果你使用的是角色模组模板或内容模组模板，你应该改为继承以你的模组命名的类（例如，如果你的模组叫 `FoobarMod`，而你在制作一张卡牌，你应该继承 `FoobarModCard`）
* 查看原版游戏是怎么做的（参见[反编译](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)）

添加内容的指南涵盖了一些领域，但 StS 2 有太多内容，不可能解释如何做所有事情。制作一个成功的模组需要持续的学习和改编。

如果你想用 Godot 制作东西，[Godot 文档](https://docs.godotengine.org/en/stable/) 或其他 Godot 指南是个不错的去处。

# 打开和关闭模组
可以通过转到 Settings -> Mod Settings 来打开和关闭模组。如果你禁用了所有模组，就会回到无模组游玩。你也可以通过使用 `-nomods` 标志启动游戏来不带模组游玩（在 Steam 中，右键点击游戏，选择 Properties，在 General 下有 Launch Options）。

请注意，带模组和不带模组游玩时，你的存档文件是分开的。

# 模组文件及其位置
模组从两个位置加载：一个用于你在 Steam 创意工坊订阅的模组，一个用于你手动下载或正在自己开发的模组。

## Steam 创意工坊
Steam 创意工坊会自动下载并更新你订阅的模组。你可以在你的 Steam 安装目录下的 `Steam/steamapps/workshop/content/2868840/` 目录中找到从创意工坊下载的模组。每个模组都位于一个以它在 Steam 创意工坊的 ID 命名的文件夹中（该 ID 可在 URL 中找到；例如，BaseLib 的创意工坊页面 URL 是 `https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127`，所以它的 ID 是 `3737335127`）。

## 本地模组
模组也会从 `mods` 目录加载，该目录要么在你的 StS 2 安装目录中（Windows/Linux），要么在 `SlayTheSpire2.app/Contents/MacOS` 中（Mac）。使用本指南中的模板构建的模组会自动复制到正确的位置。手动安装模组的人需要在正确的位置创建 `mods` 文件夹，然后把模组复制过去。

## 模组文件
模组最多由三个文件组成：
* `ModName.json`，即清单，用于声明模组的基本信息
* `ModName.dll`，包含代码改动（没有代码改动的模组可能没有这个文件）
* `ModName.pck`，包含文本和资源（没有新文本/资源的模组可能没有这个文件）

虽然只要放在一起，它们可以被放在 `mods` 文件夹中的任何位置，但为了便于组织，最好把每个模组放在以它命名的单独文件夹中。要把你的模组发送给别人或上传到某处，请发送/上传这个文件夹。

## 模组清单
清单文件为每个模组提供基本的元数据，它是从你项目中同名的 JSON 文件创建的。模板会为你创建该文件，但你需要填写内容（并且在发布新版本或有其他任何变化时更新版本号）。

它具有以下格式：
```
{
  "id": "ModName", //ID，用于标识模组并避免与其他模组冲突
  "name": "Test Mod", //模组名称，显示在模组列表中
  "author": "me", //作者，同样会显示
  "description": "A test mod for this guide.", //简要描述，同样会显示
  "version": "v0.0.1", //模组的版本号，同样会显示
  "has_pck": true, //是否为该模组加载 pck
  "has_dll": true, //是否为该模组加载 DLL
  // 注意：min_game_version 以及为依赖使用 min_version 是游戏版本 0.105（2026 年 5 月 7 日的 beta 分支）起新增的
  "min_game_version": "0.105.0", //该模组所需的最低游戏版本
  "dependencies":  [{"id": "BaseLib", "min_version": "3.1.2"}], //该模组所依赖的模组列表，以及每个依赖所需的最低版本
  "affects_gameplay": true //该模组是否影响游戏玩法；不影响游戏逻辑的纯外观或信息类模组应将其设置为 false
}
```

`affects_gameplay` 设置为 `false` 的模组在连接到多人游戏房间时不会被检查，这意味着当房间里的其他人没有这些模组时，仍然可以使用它们。错误地设置此项会导致不同步。

# 版本更新与 beta 分支
目前，杀戮尖塔 2 处于抢先体验阶段，既有主分支（大约每月更新一次），也有 beta 分支（每一两周更新一次）。这些更新经常会破坏 BaseLib 和/或个别模组。

BaseLib 会针对 beta 分支的变化进行更新（可能需要一天左右）；如果游戏更新后你遇到了奇怪的错误，这很可能是原因所在。

你可以在 Steam 中通过右键点击杀戮尖塔 2，然后转到 Properties -> Game Versions and Betas 来切换分支。

个别模组可能需要也可能不需要自己更新。通常情况下，如果一个模组使用的是 BaseLib 的 API，那么一旦 BaseLib 更新完成，它就能正常工作。如果一个模组有自己的补丁，它可能会失效并需要更新。

每次更新都有一个版本号，但目前还没有自动方式来检查一个模组支持哪个分支或哪个版本。

# 上传和分享你的模组
[杀戮尖塔 2 的 Steam 创意工坊](https://steamcommunity.com/app/2868840/workshop/) 是分享和查找模组的地方。

对于上传模组，Megacrit 提供了一个 [GitHub 上的模组上传工具](https://github.com/megacrit/sts2-mod-uploader)。请参阅那里的说明。

以前，人们使用以下地方分享模组：
* StS Discord 上的[早期 StS2 模组合集](https://discord.com/channels/309399445785673728/1480486428843446383/1480486428843446383)
* NexusMods 上的[杀戮尖塔 2 板块](https://www.nexusmods.com/games/slaythespire2)

上传模组时，你应该上传以你的模组命名的目录，其中包含你的模组文件（参见上文关于模组文件的小节）。
