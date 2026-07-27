// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.VoteForMapCoordAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Runs;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class VoteForMapCoordAction : GameAction
{
  private readonly Player _player;
  private readonly MapLocation _source;
  private readonly MapVote? _destination;

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType => GameActionType.Any;

  public VoteForMapCoordAction(Player player, MapLocation source, MapVote? destination)
  {
    this._player = player;
    this._source = source;
    this._destination = destination;
  }

  protected override Task ExecuteAction()
  {
    RunManager.Instance.MapSelectionSynchronizer.PlayerVotedForMapCoord(this._player, this._source, this._destination);
    return Task.CompletedTask;
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetVoteForMapCoordAction()
    {
      source = this._source,
      destination = this._destination
    };
  }

  public override string ToString()
  {
    return $"{nameof (VoteForMapCoordAction)} {this._player.NetId} {this._source}->{this._destination}";
  }
}
