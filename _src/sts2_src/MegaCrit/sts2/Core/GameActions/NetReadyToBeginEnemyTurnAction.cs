// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetReadyToBeginEnemyTurnAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct NetReadyToBeginEnemyTurnAction : INetAction, IPacketSerializable
{
  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new ReadyToBeginEnemyTurnAction(player);
  }

  public void Serialize(PacketWriter serializer)
  {
  }

  public void Deserialize(PacketReader deserializer)
  {
  }

  public override string ToString() => nameof (NetReadyToBeginEnemyTurnAction);
}
