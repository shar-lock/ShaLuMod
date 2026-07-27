// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.NetConsoleCmdGameAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public struct NetConsoleCmdGameAction : INetAction, IPacketSerializable
{
  public string cmd;
  public bool inCombat;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.cmd);
    writer.WriteBool(this.inCombat);
  }

  public void Deserialize(PacketReader reader)
  {
    this.cmd = reader.ReadString();
    this.inCombat = reader.ReadBool();
  }

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new ConsoleCmdGameAction(player, this.cmd, this.inCombat);
  }

  public override string ToString()
  {
    return $"{nameof (NetConsoleCmdGameAction)} cmd {this.cmd} inCombat {this.inCombat}";
  }
}
