// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCmdGameAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Debug;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public class ConsoleCmdGameAction : GameAction
{
  public override ulong OwnerId => this.Player.NetId;

  public override GameActionType ActionType
  {
    get => !this.InCombat ? GameActionType.Any : GameActionType.CombatPlayPhaseOnly;
  }

  public Player Player { get; }

  public string Cmd { get; }

  public bool InCombat { get; }

  public ConsoleCmdGameAction(Player player, string cmd, bool inCombat)
  {
    this.Player = player;
    this.Cmd = cmd;
    this.InCombat = inCombat;
  }

  protected override async Task ExecuteAction()
  {
    await NDevConsole.Instance.ProcessNetCommand(this.Player, this.Cmd);
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetConsoleCmdGameAction()
    {
      cmd = this.Cmd,
      inCombat = this.InCombat
    };
  }

  public override string ToString()
  {
    return $"{nameof (ConsoleCmdGameAction)} player {this.Player.NetId} cmd {this.Cmd} InCombat {this.InCombat}";
  }
}
