> [!NOTE]
> 在按照本页任何指南操作之前，请先按照 [Setup](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup) 中的步骤创建一个模板。如果你 _只想_ 替换资源，请使用 `Empty Slay the Spire 2 Mod` 模板。务必阅读关于 [发布](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) 的章节。


# 文件夹布局
在开始之前，我想先说明一下文件夹布局，因为本指南中的布局与通常的模组布局方式不同。

典型的模组布局如下：
```
ModName
├── ModName
│   ├── localization
│   │   ├── cards.json
│   │   └── ...
│   └── images
│       ├── card_portraits
│       └── ...
├── ModNameCode
│   ├── MainFile.cs
│   └── ...
├── ModName.csproj
├── ModName.json
├── ModName.sln
└── ...
```
请特别注意嵌套的 `ModName` 文件夹。在这种结构下，`localization` 中的文件会与基础游戏的本地化文件合并，而 `images` 中的文件会被基础游戏忽略（你的模组仍然可以加载它们）。

如果你改成把项目格式化成这样（没有嵌套的 `ModName` 文件夹）：
```
ModName
├── localization
│   ├── cards.json
│   └── ...
├── images
│   ├── card_portraits
│   └── ...
├── ModNameCode
│   ├── MainFile.cs
│   └── ...
├── ModName.csproj
├── ModName.json
├── ModName.sln
└── ...
```
那么 `localization` 或 `images` 中的文件将替换基础游戏中对应的文件。对于 `localization` 来说，这几乎绝不会是你想要的结果，但把文件放在 `images` 文件夹中可以用来替换基础游戏中的资源，这正是本指南要做的。

# 替换静态图片
杀戮尖塔 2（StS2）中的大多数图片以两种方式之一存储——要么作为独立的 png 文件，要么放在图集中。有些图片有大小两种变体，有些图片还有单独的轮廓变体。

在本指南中，我将介绍如何替换遗物图片，因为这涵盖了所有这些变体。其他静态图片（卡牌、药水、附魔）可以用相同的步骤替换。角色、敌人和其他少数东西使用 [Spine](https://github.com/Alchyr/ModTemplate-StS2/wiki/Replacing-Base-Game-Text-%26-Images/#a-note-about-spine) 制作动画，无法这样轻松地替换。

第一步是找出遗物图片在游戏文件中的位置。你可以通过浏览游戏的解包资源来完成（按照 [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) 页面上的步骤获取这些资源）。

每件遗物有 3 张不同的图片。对于 akabeko：
- 大图位于 `images/relics/akabeko.png`
- 小图位于 `images/atlases/relic_atlas.sprites/akabeko.tres`
- 小轮廓图位于 `images/atlases/relic_outline_atlas.sprites/akabeko.tres`

后两者是图集中的矩形区域（图集分别位于 `images/atlases/relic_atlas.png` 和 `images/atlases/relic_outline_atlas.png`）

所以，要在游戏中替换一件遗物的图片，我们需要把这 3 个都替换掉。

## 大尺寸遗物图片
我先从大图开始，因为这是最容易的。对于没有存储在图集中的资源（例如附魔），这会是你唯一需要做的步骤。

首先，在 godot 编辑器中打开你的项目
  - 如果你已经打开了 Rider，可以点击右上角的这个按钮（如果没有出现，请确认 `Directory.Build.props` 中的 godot 路径是否正确）
  <img hspace="40" width="226" height="109" alt="Rider 右上角的 Start Godot Editor 按钮" src="https://github.com/user-attachments/assets/fd03a954-3aad-4a1b-8ea6-a09eeb798e64"/>

  - 否则，运行 godot，点击 `Import` 并选择你的项目文件夹

接下来，把你的图片放到与要替换的图片完全相同的路径下。Akabeko 位于 `images/relics/akabeko.png`，所以我会把一个 png 放到该位置。我是通过查看解包后的 sts2 资源中 `akabeko.png` 的尺寸来确定这个 png 的尺寸（256x256）的。

<img hspace="40" width="270" height="320" src="https://github.com/user-attachments/assets/2992745c-e36e-4a30-9d6e-bc4bb12304e4" />

如果你现在就 [发布](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) 项目，这张图片就会出现在游戏中：

<img hspace="40" height="400" alt="图片" src="https://github.com/user-attachments/assets/96051c06-00e6-4a27-b685-ab37ffcd2cac" />

不过，这只会替换查看单件遗物时显示的大尺寸遗物图片，并不会替换图鉴或遗物栏中显示的小图。

## 小尺寸遗物图片
小尺寸遗物图片存储在图集中，所需的替换步骤比大图更多。卡牌和药水也存储在图集中，因此如果要替换它们，你需要对这些步骤稍作调整。

游戏中的小图文件以 `.tres` 而不是 `.png` 结尾。这里，`.tres` 是 Godot 资源格式的扩展名。Godot 资源可以用于各种用途，这里我们要创建一个指向某个 png 的资源。

首先，把你的图片放在项目文件夹中的某个位置（放在 `ModName/images` 下是个不错的选择，具体放在哪里并不重要）。我是通过查看 `akabeko.tres` 文件中这一行的最后两个数字（本例中为 81x69）来确定这张图片的尺寸的：
```
region = Rect2(1616, 536, 81, 69)
``` 

<img hspace="40" width="236" height="342" alt="图片" src="https://github.com/user-attachments/assets/09039443-cddc-4c55-97fd-65556803226f" />

然后，在 File System（文件系统）面板中双击你的图片，以便在右侧的 Inspector（检查器）面板中打开它。找到并复制 `Load Path` 字段

<img hspace="40" height="500" alt="图片" src="https://github.com/user-attachments/assets/8d93e32e-a446-4eea-b181-205d5e04478a" />

接下来，在与要替换的图片完全相同的路径下，右键点击 > `Create New` > `Resource` > 搜索 `CompressedTexture2D`。Akabeko 位于 `images/atlases/relic_atlas.sprites/akabeko.tres`，所以我会在那里创建它。

<img hspace="40" width="284" height="402" alt="图片" src="https://github.com/user-attachments/assets/1b184fc2-d7ff-4f34-97fa-d366a163d79c" />

最后，双击你在上一步中创建的资源（`.tres` 文件），将之前复制的路径粘贴到它的 `Load Path` 字段中

如果你现在就 [发布](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) 项目，这张小图就会出现在游戏中：

<img width="247" height="141" alt="图片" src="https://github.com/user-attachments/assets/0d69d144-da5f-44c1-b71f-c1c8dc9bfb19" />

从这张图里有点难看出来，但轮廓仍然是 akabeko 的轮廓（右侧有一团黑色块）。

最后一步是替换轮廓图。轮廓图的替换过程与替换小图完全相同——只是使用不同的路径。对于 akabeko，轮廓图位于 `images/atlases/relic_outline_atlas.sprites/akabeko.tres`。要制作轮廓图，请创建一个比小尺寸遗物图略大的全白版本。

<hr/>
<details>
<summary><h3>旧的替换卡牌图片指南</h3></summary>

- 在 godot 编辑器中打开你的项目文件夹
  - 如果你使用 Rider，可以点击右上角的这个按钮（如果没有出现，请确认 `Directory.Build.props` 中的 godot 路径是否正确）
<p>
  <img hspace="60" width="226" height="109" alt="Rider 右上角的 Start Godot Editor 按钮" src="https://github.com/user-attachments/assets/fd03a954-3aad-4a1b-8ea6-a09eeb798e64"/>
</p>

- 把你的自定义图片放在项目文件夹中的某个位置（放在 `ModName/images` 下是个不错的选择）
  - 图片尺寸应为 500x380 的倍数（例如 250x190 或 1000x760）

- 在 godot 编辑器的 File Editor（文件编辑器）面板中打开你的自定义图片，并双击它以便在右侧的 Inspector（检查器）面板中打开它。找到并复制 `Load Path` 字段
<p>
  <img hspace="30" width="472" height="456" alt="Godot 编辑器检查器面板中显示的一张图片" src="https://github.com/user-attachments/assets/df14adab-594e-4b86-acba-3ee4d4d3cd6e" />
</p>

- 重建与你要替换其原画的基础游戏卡牌相同的文件夹结构
  - 示例：`images/atlases/card_atlas.sprites/ironclad/strike_ironclad.tres`
  - 查看 [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) 以便自行了解基础游戏的文件夹结构

- 在 godot 编辑器中，右键点击 > `Create New` > `Resource` > 选择 `CompressedTexture2D`
<p>
  <img hspace="30" width="641" height="280" alt="Godot 编辑器中显示文件夹，然后选择 Resource" src="https://github.com/user-attachments/assets/13da13c7-c28e-49bd-8f13-2b3dd2889284"/>
</p>

- 双击你在上一步中创建的资源（`.tres` 文件），将之前复制的路径粘贴到它的 `Load Path` 字段中
<p>
  <img hspace="30" width="472" height="420" alt="Godot 编辑器检查器面板中显示的一个空资源" src="https://github.com/user-attachments/assets/dc9793d4-38ce-4fae-aa0f-5dc9bbbed009" />
</p>


- 最后，[发布](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) 你的项目，你的修改就会出现在游戏中

</details>
<hr/>

# 替换文本/本地化

由于模组是在基础游戏之后加载的，使用与基础游戏相同的本地化键会让你的模组在合并过程中替换掉基础游戏本地化的某些部分（使用本页顶部提到的第一种布局）。

例如，在 `ModName/localization/eng/cards.json` 创建一个包含以下内容的文件，将会改变 ironclad 的起手卡 strike 的标题和描述：

```json
{
    "STRIKE_IRONCLAD.title": "Strike 2",
    "STRIKE_IRONCLAD.description": "Electric Boogaloo"
}
```
记得 [发布](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild)，这样你的修改才会出现在游戏中。

这种模式可以用来修改基础游戏中的任何文本，请查看 [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) 以便查看所有基础游戏的本地化内容。

# 关于 Spine 的说明
StS2 使用 Spine 来制作角色、敌人和其他少数动画。Spine 完整版的价格约为 [400 美元](https://en.esotericsoftware.com/spine-purchase)，因此对大多数模组开发者来说，直接在 Spine 中编辑动画太过昂贵。不过你也有其他选择。

对于现有模型的重涂色/换肤，你可以编辑图集中的各个部分。例如，你可以用下面这张图替换 `animations/characters/silent/silent.png` 文件：

<img width="400" alt="translent" src="https://github.com/user-attachments/assets/1e10d393-24f5-49cc-8bbb-e9f5d10b0ead" />

不过这种方式在改变每个部分的形状上没有多少自由度。

如果你想创建自己的动画，只要能在 godot 编辑器中加载，就可以在游戏中加载。你可以搜索 godot 的 2D 骨骼动画教程，甚至可以使用 3D 模型并通过视口将其渲染为 2D。r2Nexus 有一篇关于 [使用 Blender 为 StS2 模型制作动画](https://github.com/r2Nexus/The-Engineer/wiki/Animating-StS2-models-with-Blender-%E2%80%90-Introduction) 的指南。

要替换现有模型，你很可能需要使用 [Harmony 补丁](https://harmony.pardeike.net/articles/patching.html) 才能让游戏加载你的动画。要创建自己的角色，请查看 [BaseLib Wiki](https://alchyr.github.io/BaseLib-Wiki/docs/scenes/creature-visuals.html)。
