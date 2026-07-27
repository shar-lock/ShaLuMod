// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.DumpConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class DumpConsoleCmd : AbstractConsoleCmd
{
  public override string CmdName => "dump";

  public override string Args => "";

  public override string Description => "Dumps Model ID database to console & logs.";

  public override bool IsNetworked => false;

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    Log.Info(ModelIdSerializationCache.Dump());
    return new CmdResult(true, "Model ID database dumped to console & logs");
  }
}
