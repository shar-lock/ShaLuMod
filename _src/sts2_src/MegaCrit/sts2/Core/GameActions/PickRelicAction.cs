// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.PickRelicAction
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

public class PickRelicAction : GameAction
{
  private readonly Player _player;
  private readonly int? _relicIndex;

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType => GameActionType.NonCombat;

  public TreasureRoomRelicSynchronizer? TestSynchronizer { get; set; }

  public PickRelicAction(Player player, int? relicIndex)
  {
    this._player = player;
    this._relicIndex = relicIndex;
  }

  protected override Task ExecuteAction()
  {
    (this.TestSynchronizer ?? RunManager.Instance.TreasureRoomRelicSynchronizer).OnPicked(this._player, this._relicIndex);
    return Task.CompletedTask;
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetPickRelicAction()
    {
      relicIndex = this._relicIndex
    };
  }

  public override string ToString()
  {
    return $"{"NetPickRelicAction"} for player {this._player.NetId} index {this._relicIndex}";
  }
}
