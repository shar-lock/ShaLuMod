## 启动选项
你可以在 Steam 中通过传入 `-nomods` 启动选项来在不加载任何模组的情况下启动游戏
- 在 Steam 中右键点击该游戏 > `Properties...`
- 在 `Launch Options` 框中输入 `-nomods`
<img width="850" alt="nomods" src="https://github.com/user-attachments/assets/58a84811-a3b0-44a2-a6ed-98dbaa218c74" />

<hr/>

如果你想轻松地在模组版与原版之间切换，而不必每次都编辑启动选项，请继续阅读
## 独立的 Steam 条目
1. 在你的 sts2 安装目录中创建一个名为 `steam_appid.txt` 的文件，内容为 `2868840`
2. 在 Steam 左下角点击 `Add a Game` > `Add a non-Steam Game...`
3. 在弹出的窗口中，点击左下角的 `Browse...`
4. 导航到你的 sts2 安装目录并选择 `SlayTheSpire2.exe`（Windows）或 `SlayTheSpire2`（Linux）。<br/>
如果你不知道它在哪里，请在 Steam 中右键点击该游戏，进入 `Manage` > `Browse local files`
5. 点击右下角的 `Add Selected Programs`。你的 Steam 库中应会添加一个标题为 `SlayTheSpire2.exe` 或 `SlayTheSpire2` 的游戏，且没有缩略图
6.（可选）右键点击新添加的游戏，进入 `Properties...`，自定义标题和缩略图

现在，你可以按照本页顶部的说明，将 `-nomods` 启动选项添加到其中一个游戏实例，而另一个实例则可用于加载模组
