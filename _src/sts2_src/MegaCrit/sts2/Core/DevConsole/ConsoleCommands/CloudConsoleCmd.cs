// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.CloudConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class CloudConsoleCmd : AbstractConsoleCmd
{
  private static bool _confirmed;

  public override string CmdName => "cloud";

  public override string Args => "delete";

  public override string Description
  {
    get => "Deletes all save files from Steam Cloud, if you are running on Steam";
  }

  public override bool IsNetworked => false;

  public override bool DebugOnly => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1 || args[0] != "delete")
      return new CmdResult(false, "No argument specified.\n" + this.Args);
    if (!CloudConsoleCmd._confirmed)
    {
      CloudConsoleCmd._confirmed = true;
      return new CmdResult(false, "Run this command again to confirm you want to delete all your Steam cloud saves. The game will quit.");
    }
    CloudConsoleCmd.DeleteCloudSaves();
    return new CmdResult(true, "Steam cloud saves deleted.");
  }

  public static void DeleteCloudSaves()
  {
    if (!SteamInitializer.Initialized)
      throw new InvalidOperationException("Steam not initialized");
    for (int index = SteamRemoteStorage.GetFileCount() - 1; index >= 0; --index)
    {
      int num;
      string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(index, ref num);
      Log.Info($"Deleting {fileNameAndSize} from Steam cloud ({num} bytes)");
      SteamRemoteStorage.FileDelete(fileNameAndSize);
    }
    ((SceneTree) Engine.GetMainLoop()).Quit(0);
  }
}
