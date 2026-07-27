// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.MoveToMapCoordAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class MoveToMapCoordAction : GameAction
{
  private readonly Player _player;
  private readonly MapCoord _destination;

  public override ulong OwnerId => this._player.NetId;

  public override GameActionType ActionType => GameActionType.NonCombat;

  public MoveToMapCoordAction(Player player, MapCoord destination)
  {
    this._player = player;
    this._destination = destination;
  }

  protected override async Task ExecuteAction()
  {
    if (TestMode.IsOn)
    {
      await RunManager.Instance.FadeOut();
      await RunManager.Instance.EnterMapCoord(this._destination);
      TaskHelper.RunSafely(RunManager.Instance.FadeIn());
    }
    else
      await NMapScreen.Instance.TravelToMapCoord(this._destination);
  }

  public override INetAction ToNetAction()
  {
    return (INetAction) new NetMoveToMapCoordAction()
    {
      destination = this._destination
    };
  }

  public override string ToString()
  {
    return $"{nameof (MoveToMapCoordAction)} {this._player.NetId} {this._destination}";
  }
}
