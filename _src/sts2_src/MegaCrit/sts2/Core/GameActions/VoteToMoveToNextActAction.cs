// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.VoteToMoveToNextActAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Runs;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class VoteToMoveToNextActAction : GameAction
{
  private readonly Player _player;

  public int CurrentActIndex { get; }

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType => GameActionType.NonCombat;

  public VoteToMoveToNextActAction(Player player, int currentActIndex)
  {
    this._player = player;
    this.CurrentActIndex = currentActIndex;
  }

  protected override Task ExecuteAction()
  {
    RunManager.Instance.ActChangeSynchronizer.OnPlayerReady(this._player, this.CurrentActIndex);
    return Task.CompletedTask;
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetVoteToMoveToNextActAction()
    {
      currentActIndex = this.CurrentActIndex
    };
  }

  public override string ToString()
  {
    return $"{nameof (VoteToMoveToNextActAction)} {this._player.NetId} act {this.CurrentActIndex}";
  }
}
