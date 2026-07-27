本指南介绍如何为游戏添加新的先古之民（Ancient）。

添加一个先古之民需要：
- 一个用于你的先古之民的 `CustomAncientModel` 类，提供资源文件路径和遗物池
- 对应的本地化
- 一个用作先古之民事件背景的场景
- 一个用于运行历史和对话的图标
- 一个带轮廓线的地图图标

先古之民需要至少三件稀有度为 `RelicRarity.Ancient` 的遗物，并将其添加到 `EventRelicPool` 池中。[这里](https://alchyr.github.io/BaseLib-Wiki/docs/models/custom-relic.html)是 `CustomRelic` 的 BaseLib wiki 页面。


# 创建文件

首先，创建你新先古之民的类文件。它可以放在项目的任意文件夹下，你可以按自己喜欢的方式组织文件。

<img width="569" height="211" alt="图片" src="https://github.com/user-attachments/assets/b96a9370-0a3f-4d69-aac6-026c98f27c71" />

<p> </p>

然后，让你的新类继承 BaseLib 的 CustomAncientModel。这样做会因为缺少我们即将添加的信息而报错。它应该看起来像这样：

```C#
public class TestAncient : CustomAncientModel
{

}
```



# 选项池

这会决定你的先古之民提供哪些遗物。

在你的先古之民类中重写 `MakeOptionPools`，并提供一个选项池，如下所示：

```C#
protected override OptionPools MakeOptionPools => new OptionPools(
    [
        AncientOption<Nunchaku>(),
        AncientOption<Lantern>(),
        AncientOption<ArtOfWar>()
        //更多遗物选项
    ]
);
```

会从该池中随机选择三个选项作为你的先古之民提供的奖励。

你也可以选择性地为 OptionPools 构造函数提供最多 3 个数组，从而创建多个池。如果提供 2 个池，则会从第一个池中选择 2 个选项，从第二个池中选择 1 个。如果提供 3 个池，则每个选项会分别从不同的池中选择。基础游戏中的先古之民通常要么使用一个大池（Nonupeipe、Tanx），要么使用三个独立的池（Tezcatara、Orobas、Pael、Vakuu）。Darv 有两个池，一个包含他全部的遗物，另一个只包含 Dusty Tome（并带有逻辑，使其只在一半的情况下出现）。

# 设置先古之民的生成时机

要更改你的先古之民可以出现在哪一幕，请重写 `IsValidForAct` 方法（本例使其仅在第 2 幕出现）：
```C#
public override bool IsValidForAct(ActModel act)
{
    return act.ActNumber() == 2;
}
```

# 本地化

首先，创建 `ancients.json` 文件。它**必须**位于正确的文件夹中：`[YourModName]/localization/eng`（将 `eng` 改为你正在编写本地化所用的语言）。如有需要，请先创建这些文件夹。

<img width="521" height="306" alt="图片" src="https://github.com/user-attachments/assets/18f0ccba-77dd-4929-a964-5c8f7f04e2ec" />

<p> </p>

现在我们可以为先古之民添加本地化了。回到你的先古之民类文件，然后 `right click > Show context actions > Generate localization > any of the three options`

<img width="756" height="60" alt="图片" src="https://github.com/user-attachments/assets/beebd179-8cb5-43c5-ba15-c58144d1fa8f" />

<p> </p>

剪切生成的文本，将其粘贴到 `ancients.json` 的括号之间，然后保存文件。这应该会清除类文件中显示的错误。如果没有，请检查文件夹的名称和位置。

生成的本地化会为你的先古之民添加最少量的文本：名称、称号、首段对话（一行）以及通用的可重复对话（一行）。如需添加更多内容，请查阅 [BaseLib wiki](https://alchyr.github.io/BaseLib-Wiki/docs/extras/ancient-dialogue.html)。

# 资源

对于所有这些资源，使用 [GDRE Tools 提取基础游戏资源](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) 可能会有所帮助，这样你可以看到基础游戏中的先古之民是如何工作的。

## 地图图标

地图图标是一张 278x278 的图片。它必须附带一张同样大小的单独轮廓图（与遗物类似）。默认的名称和位置是 `images/packed/map/ancients/ancient_node_[modID]-[ancientclassname].png` 和 `ancient_node_[modID]-[ancientclassname]_outline.png`（全部小写）。

你也可以在先古之民类中使用 `CustomMapIconPath` 和 `CustomMapIconOutlinePath` 属性来提供其他路径。

## 运行历史图标

该图标将用于运行历史以及对话中。

这是一张 88x88 的图片，同样必须有一张单独的轮廓图。

默认的名称和位置是 `images/ui/run_history/[modID]-[ancientclassname].png` 和 `[modID]-[ancientclassname]_outline.png`（全部小写）。

你也可以在先古之民类中使用 `CustomRunHistoryIconPath` 和 `CustomRunHistoryIconOutlinePath` 属性来提供其他路径。


## 场景

你需要制作一个 Godot 场景，用作先古之民的背景。通常它只会包含一张背景图片（本指南也只介绍这一种情况），但 Godot 场景也允许添加特效、粒子和动画。

打开 Godot 或 Megadot，然后从你的模组文件夹中打开 `project.godot` 文件。现在你应该可以将你的模组作为 Godot 项目打开了。

创建一个新的 2d 场景，作为先古之民事件的背景。默认的路径和名称是 `scenes/events/background_scenes/[modID]-[ancientclassname].tscn`（全部小写）。你也可以在先古之民类中重写 `CustomScenePath` 属性来提供其他路径。

<img width="420" height="300" alt="图片" src="https://github.com/user-attachments/assets/ab84379d-c3aa-4470-a05c-957ac3804953" />
<img width="290" height="300" alt="图片" src="https://github.com/user-attachments/assets/c4b08891-aa82-4f18-8b8b-092e09b7e9c4" />

<p> </p>

将根节点更改为 Control 节点，并将其重命名为你的先古之民名称。

<img width="240" height="300" alt="图片" src="https://github.com/user-attachments/assets/28c3e3fa-95e3-4d6d-9cc6-7513c19e8d3c"/>
&nbsp; &nbsp; &nbsp;
<img width="380" height="300" alt="图片" src="https://github.com/user-attachments/assets/b86778e5-a376-445a-a18b-c9a42ecd0831" />

<p> </p>

为根节点添加一个子节点，类型为 `TextureRect`。

<img width="377" height="210" alt="图片" src="https://github.com/user-attachments/assets/51bc083a-1e49-4da2-821d-aeef5cb735aa" />

<p> </p>

将你选择的图片添加到模组的文件中（基础游戏的图片尺寸为 2560x1200），并将该图片选作子节点的纹理。

<img width="300" height="124" alt="图片" src="https://github.com/user-attachments/assets/409a3824-3820-4704-b3f7-23c94f07ac92" />
 
<p> </p>

为了让纹理对齐正确，请转到 `Project > Project Settings > Display > Window`，将视口宽度和高度分别改为 1920 和 1080。这会将场景中表示游戏窗口的蓝色矩形更改为游戏期望的尺寸。然后你可以通过拖动来使图片居中。

保存场景，你就完成了！按照本指南完成所有步骤后，你的先古之民应该能够正常生成并工作。
