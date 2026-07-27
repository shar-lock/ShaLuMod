# Using the dev console
With mods enabled, you can open/close the dev console by pressing any of `~`, `` ` ``, `*`, `'`, or `Shift+8`. In the dev console, type `help` to see the list of commands, or `help commandname` to see help for a specific command (e.g. `help card`).

The dev console allows you to quickly test most types of content by immediately spawning it and has a variety of other useful commands too.

# Reading the game logs

If the game crashes, freezes, hangs, or otherwise behaves strangely while testing your mod, you should check the game's logs for any errors.

Baselib adds a `showlog` command to the dev console that immediately opens a log window, and it will stay open until you quit the game.

Logs are also saved to a file. If the game is still open, you can run `open logs` in the dev console to open your file explorer at the logs directory. Otherwise, you can navigate there yourself. It will normally be at one of the following locations:
* Windows: `%appdata%/SlayTheSpire2/logs`
* MacOS: `~/Library/Application Support/SlayTheSpire2/logs`
* Linux: `~/.local/share/SlayTheSpire2/logs`

`godot.log` in this directory contains the most recent logs.

If you're  developing a mod, it's useful to be able to look at the logs immediately when things go wrong. While on the main menu of the game, go to `Mod Configuration` -> `BaseLib` and check "Open log window on startup". This makes the log window open automatically whenever you run the game.

Once you can see the logs, you should check for any lines that mention errors. They will usually reference something in your mod, although not always.

You can refer to this guide on [Reading a stack trace](https://stackoverflow.com/questions/3988788/what-is-a-stack-trace-and-how-can-i-use-it-to-debug-my-application-errors) to try and understand stack traces of errors you find.

## Logging your own information
If you want your mod to add information to the log file, you can do this using the `Logger` field in your `MainFile` (the template comes with this by default), e.g. `MainFile.Logger.Info("Some useful information.")`. This can be helpful if you have complex or error-prone code; logging details of the steps your code is taking can help narrow down issues.

# Testing Multiplayer Locally

## Initial Setup
Create a file named `steam_appid.txt` in your sts2 directory with the contents `2868840`. This allows launching the game without going through Steam.

## Launching Instances

- **Host**: Launch the game with the `-fastmp host_standard` command line argument. 

- **Client**: Launch the game with the `-fastmp join` argument. After a brief delay, it will join your other game.

### Testing with 3+ Players

To test with more than 2 players, you will additionally need to pass the `-clientId` argument for the additional clients. The default clientId is 1000. For example, to have a 3rd player join use the arguments `-fastmp join -clientId 1001`. and `-clientId 1002` for the 4th player.

## Scripting

Below are example scripts for running multiple instances of the game.

### Windows

Save the following as a `.ps1` file, adjusting `ExePath` and `TotalPlayers` if necessary. Run it by double clicking in file explorer or from a powershell command line.
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

Save the following as a `.sh` file, adjusting `ExePath` and `TotalPlayers` if necessary. Open a terminal in the same folder and run `chmod +x <filename>` to make it executable. Run it from a terminal: `./<filename>`.

For example, if you named the file `sts2mp.sh`, you would run `./sts2mp.sh` from a terminal in the same folder to run it
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

# Attaching a Debugger

### Initial Setup

Create a file named `steam_appid.txt` in your sts2 directory with the contents `2868840`. This allows launching the game without going through Steam.

### Copying the `pdb`

In order for breakpoints to work correctly, you need to copy the `pdb` to the folder where your mod's `dll` is. You can do it manually, or add the following to the end of your `CopyToModsFolderOnBuild` Target in your `.csproj` (assuming you're using the ModTemplate)
```xml
<Copy SourceFiles="$(TargetDir)$(TargetName).pdb" DestinationFolder="$(ModsPath)$(MSBuildProjectName)/" />
```

### Creating a Launch Profile 

In Rider, go to Run / Debug Configuration > Edit Configurations. Click Add New Configuration (+) > .Net Executable, and fill out the Executable and Working Directory. Then click OK.

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/riderconfig.png)


<details>
<summary><h4>Visual Studio</h4></summary>
In Visual Studio, go to Menu > Debug > Debug Properties and fill out the Executable and Working Directory.

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/visualstudioconfig.png)

</details>

### Launching the Game
At this point, you should be able to launch sts2 from the configuration you've created.

### Enabling External Source Debugging

This will allow you to step into code from other mods or sts2 itself.

In Rider, go to Settings > Build, Execution, Deployment > Debugger > .Net Languages, and enable Enable external source debug

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/enableExternalRider.png)

<details>
<summary><h4>Visual Studio</h4></summary>
In Visual Studio, go into Menu > Tools > Options > Debugging > General, and Disable Enable Just My Code

![](https://github.com/pikcube/Setting-Up-the-Slay-the-Spire-2-Debugger/blob/main/enableExternalVS.png)
</details>