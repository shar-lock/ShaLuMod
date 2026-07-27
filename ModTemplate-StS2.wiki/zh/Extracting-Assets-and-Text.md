杀戮尖塔 2（StS2）使用 Godot 游戏引擎，因此如果你想查找基础游戏的资源和文本，就需要使用 [GDRE Tools](https://github.com/GDRETools/gdsdecomp)。
* 从 Github 下载 GDRE Tools（在 [releases 页面](https://github.com/GDRETools/gdsdecomp/releases)；最新的发布版本即可；展开 Assets 部分，下载与你操作系统匹配的版本）
* 将 GDRE Tools 解压到一个文件夹并运行 .exe
* 在 RE Tools 菜单中，选择 Recover Project
* 从杀戮尖塔 2 的安装目录中打开 `SlayTheSpire2.pck`
* 等待它处理项目

现在你可以浏览游戏的所有资源了。文本可以在 `localization` 文件夹下找到。其他资源（图片、场景、声音等）分布在各种文件夹中；命名大多比较合理。游戏的代码也在这里，位于 `src/Core` 下，不过用反编译器查看会更方便；参见 [Decompiling](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)。

如果你希望以后更方便地引用这些资源，可以考虑将 pck 的内容解压到你自己的文件夹中。
