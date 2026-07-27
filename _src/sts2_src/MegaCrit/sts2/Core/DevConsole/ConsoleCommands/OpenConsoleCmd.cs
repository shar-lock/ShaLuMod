// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.OpenConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class OpenConsoleCmd : AbstractConsoleCmd
{
  private static readonly IReadOnlyCollection<string> _options = (IReadOnlyCollection<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[5]
  {
    "logs",
    "saves",
    "root",
    "build-logs",
    "loc-override"
  });

  public override string CmdName => "open";

  public override string Args => "logs|saves|root|build-logs|loc-override";

  public override string Description => "Opens a common path in the local OS file browser.";

  public override bool IsNetworked => false;

  public override bool DebugOnly => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, "No argument specified.\n" + this.Args);
    if (!OpenConsoleCmd._options.Contains<string>(args[0]))
      return new CmdResult(false, $"Argument '{args[0]}' unrecognized.\n{this.Args}");
    string userDataDir = OS.GetUserDataDir();
    if (userDataDir == null)
      return new CmdResult(false, "Unable to open the user data directory.");
    string dataDir = OS.GetDataDir();
    string str1;
    switch (args[0])
    {
      case "logs":
        str1 = Path.Combine(userDataDir, "logs");
        break;
      case "saves":
        str1 = ProjectSettings.GlobalizePath(SaveManager.Instance.GetProfileScopedPath("saves"));
        break;
      case "root":
        str1 = userDataDir;
        break;
      case "build-logs":
        str1 = Path.Combine(dataDir, "Godot", "mono", "build_logs");
        break;
      case "loc-override":
        str1 = ProjectSettings.GlobalizePath("user://localization_override");
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    string str2 = str1;
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      str2 = str2.Replace('/', '\\');
    Error error = OS.ShellShowInFileManager(str2, true);
    if (error == null)
      return new CmdResult(true, $"Opened '{str2}'");
    return new CmdResult(false, $"Error {error}: Cannot open OS file manager.");
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) OpenConsoleCmd._options, Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }
}
