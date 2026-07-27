# 使用开发者控制台
在启用模组的情况下，你可以按 `~`、`` ` ``、`*`、`'` 或 `Shift+8` 中的任意一个键来打开/关闭开发者控制台。在开发者控制台中输入 `help` 可以查看命令列表，或输入 `help commandname` 查看某个具体命令的帮助（例如 `help card`）。

开发者控制台允许你通过立即生成大多数类型的内容来快速测试，并且还有许多其他实用的命令。

# 查看游戏日志

如果游戏在测试你的模组时崩溃、卡死、挂起或出现其他异常行为，你应该检查游戏日志中是否有任何错误。

BaseLib 在开发者控制台中添加了一个 `showlog` 命令，可以立即打开一个日志窗口，该窗口会一直保持打开直到你退出游戏。

日志也会保存到文件中。如果游戏仍在运行，你可以在开发者控制台中运行 `open logs`，从而在日志目录处打开你的文件管理器。否则，你也可以自己导航到那里。它通常位于以下位置之一：
* Windows：`%appdata%/SlayTheSpire2/logs`
* MacOS：`~/Library/Application Support/SlayTheSpire2/logs`
* Linux：`~/.local/share/SlayTheSpire2/logs`

此目录下的 `godot.log` 包含最近的日志。

如果你正在开发模组，那么在出现问题时能够立即查看日志会非常有用。在游戏主菜单中，进入 `Mod Configuration` -> `BaseLib`，勾选 "Open log window on startup"。这样每次运行游戏时都会自动打开日志窗口。

看到日志后，你应该查找任何提到错误的行。它们通常会引用你模组中的某些内容，但也不总是如此。

你可以参考这篇关于[如何阅读堆栈跟踪](https://stackoverflow.com/questions/3988788/what-is-a-stack-trace-and-how-can-i-use-it-to-debug-my-application-errors)的指南，尝试理解你所发现的错误堆栈跟踪。

## 记录你自己的信息
如果你希望模组向日志文件中添加信息，可以使用 `MainFile` 中的 `Logger` 字段来完成（模板默认提供了此项），例如 `MainFile.Logger.Info("Some useful information.")`。如果你的代码复杂或容易出错，这会很有帮助；记录代码执行步骤的细节有助于缩小问题范围。

# 本地测试多人模式

## 初始设置
在你的 sts2 目录中创建一个名为 `steam_appid.txt` 的文件，内容为 `2868840`。这样无需通过 Steam 即可启动游戏。

## 启动实例

- **主机**：使用 `-fastmp host_standard` 命令行参数启动游戏。

- **客户端**：使用 `-fastmp join` 参数启动游戏。稍作延迟后，它会加入你的另一个游戏实例。

### 与 3 名及以上玩家测试

要与超过 2 名玩家进行测试，你还需要为额外的客户端传入 `-clientId` 参数。默认 clientId 为 1000。例如，要让第 3 名玩家加入，使用参数 `-fastmp join -clientId 1001`；第 4 名玩家使用 `-clientId 1002`。

## 脚本化

下面是用于运行多个游戏实例的示例脚本。

### Windows

将以下内容保存为 `.ps1` 文件，必要时调整 `ExePath` 和 `TotalPlayers`。双击文件管理器中的文件或在 PowerShell 命令行中运行它。
```pwsh
$ExePath = "SlayTheSpire2.exe"
$TotalPlayers = 4

Start-Process $ExePath -ArgumentList "-fastmp host_standard"

$NumClients = $TotalPlayers - 2

for ($i = 0; $i -le $NumClients; $i++) {
    $cid = 1000 + $i
    Start-Process $ExePath -ArgumentList "-fastmp join -clientId $cid"
}
```

### Linux/Mac

将以下内容保存为 `.sh` 文件，必要时调整 `ExePath` 和 `TotalPlayers`。在同一文件夹中打开终端并运行 `chmod +x <filename>` 以使其可执行。从终端运行：`./<filename>`。

例如，如果你将文件命名为 `sts2mp.sh`，则可以在同一文件夹的终端中运行 `./sts2mp.sh` 来执行它。
```bash
#!/usr/bin/env bash

ExePath="./SlayTheSpire2"
TotalPlayers=4

"$ExePath" -fastmp host_standard &

NumClients=$((TotalPlayers - 1))

for ((i = 0; i < NumClients; i++)); do
    cid=$((1000 + i))
    "$ExePath" -fastmp join -clientId "$cid" &
done

wait
```

# 附加调试器

### 初始设置

在你的 sts2 目录中创建一个名为 `steam_appid.txt` 的文件，内容为 `2868840`。这样无需通过 Steam 即可启动游戏。

### 复制 `pdb`

为了让断点正常工作，你需要将 `pdb` 复制到模组 `dll` 所在的文件夹中。你可以手动完成，或者将以下内容添加到 `.csproj` 中 `CopyToModsFolderOnBuild` Target 的末尾（假设你正在使用 ModTemplate）
```xml
<Copy SourceFiles="$(TargetDir)$(TargetName).pdb" DestinationFolder="$(ModsPath)$(MSBuildProjectName)/" />
```

### 创建启动配置文件

在 Rider 中，进入 Run / Debug Configuration > Edit Configurations。点击 Add New Configuration (+) > .Net Executable，并填写 Executable 和 Working Directory。然后点击 OK。

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/riderconfig.png)


<details>
<summary><h4>Visual Studio</h4></summary>
在 Visual Studio 中，进入 Menu > Debug > Debug Properties，填写 Executable 和 Working Directory。

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/visualstudioconfig.png)

</details>

### 启动游戏
至此，你应该能够通过你创建的配置来启动 sts2 了。

### 启用外部源码调试

这将允许你步入来自其他模组或 sts2 本身的代码。

在 Rider 中，进入 Settings > Build, Execution, Deployment > Debugger > .Net Languages，启用 Enable external source debug。

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/enableExternalRider.png)

<details>
<summary><h4>Visual Studio</h4></summary>
在 Visual Studio 中，进入 Menu > Tools > Options > Debugging > General，禁用 Enable Just My Code。

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/enableExternalVS.png)
</details>
