本页解释了如何通过设置初始项目来开始杀戮尖塔 2（StS2）的模组开发。杀戮尖塔 2 使用 [Godot 游戏引擎](https://godotengine.org/) 以 C# 编写，具有极高的可模组化程度。下面 Setup 下的前几个步骤包含你需要安装的内容以及从哪里开始。

请注意，杀戮尖塔 2 目前处于抢先体验阶段。由于游戏经常更新，可能会出现破坏模组的变化。在此期间创建的任何模组都需要在主游戏每次发生破坏性变更后进行更新，而且模组开发整体上可能有些不稳定。

本指南仍在编写中；鉴于 StS 2 模组开发还如此之新，我们正在积极努力让开始模组开发变得更加容易。

# Setup

* [初始设置](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup)
* [模组开发基础](https://github.com/Alchyr/ModTemplate-StS2/wiki/Modding-Basics)
* [反编译](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)
* [提取资源与文本](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text)
* [测试与调试](https://github.com/Alchyr/ModTemplate-StS2/wiki/Testing-and-Debugging)

# 添加内容D:\mod\ModTemplate-StS2\doc

* [添加卡牌](https://github.com/Alchyr/ModTemplate-StS2/wiki/Adding-Cards)
* [添加先古之民](https://github.com/Alchyr/ModTemplate-StS2/wiki/Adding-Ancients)
* [常用命令手册](https://github.com/Alchyr/ModTemplate-StS2/wiki/Common-Commands-Cookbook)
* 这里未列出的任何内容，请查看 BaseLib（[文档](https://alchyr.github.io/BaseLib-Wiki/docs/models/)、[代码](https://github.com/Alchyr/BaseLib-StS2/tree/master/Abstracts)）

# 更多模组开发

* [着色器](https://github.com/Alchyr/ModTemplate-StS2/wiki/Shaders)
* [替换原版游戏的文本和图片](https://github.com/Alchyr/ModTemplate-StS2/wiki/Replacing-Base-Game-Text-&-Images)
* [用 Blender 为 StS2 模型制作动画](https://github.com/r2Nexus/The-Engineer/wiki/Animating-StS2-models-with-Blender-%E2%80%90-Introduction)

# 其他资源

* [BaseLib 文档](https://alchyr.github.io/BaseLib-Wiki/)
* [Harmony 文档](https://harmony.pardeike.net/articles/intro.html)
* [Godot 入门指南](https://docs.godotengine.org/en/stable/getting_started/step_by_step/index.html)（其中 GDScript 的内容相关性不高，但对概念的概述很好）
* [如何阅读堆栈跟踪](https://stackoverflow.com/questions/3988788/what-is-a-stack-trace-and-how-can-i-use-it-to-debug-my-application-errors)
* [Godot BBCode 文档](https://docs.godotengine.org/en/4.5/tutorials/ui/bbcode_in_richtextlabel.html)（用于本地化）

# Rider 插件

* [本地化插件（Localization Plugin）](https://github.com/lamali292/STS2-Rider-Localization-Plugin)

# 获取帮助

* [Slay the Spire Discord](https://discord.com/invite/SlayTheSpire) 上的 #sts2-modding 频道是寻求帮助的最佳地点

---

# 故障排除

如果你缺少 BaseLib 的功能，请检查你项目的 NuGet 依赖，看看是否错过了更新（在 Rider 中按 alt+shift+7 打开）。

<img width="921" height="171" alt="图片" src="https://github.com/user-attachments/assets/2edd1184-efaf-4275-af38-ee1c29642145" />

选择一个过期的依赖，然后点击带圆圈的箭头按钮来更新。

<img width="887" height="181" alt="图片" src="https://github.com/user-attachments/assets/dfee751a-8b33-4f2b-ac51-8475a76737fc" />

建议你也保持分析器为最新版本。
