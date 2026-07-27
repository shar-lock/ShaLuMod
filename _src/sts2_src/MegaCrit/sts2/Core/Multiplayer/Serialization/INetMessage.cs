// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.INetMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.SourceGeneration;

#nullable disable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

[GenerateSubtypes]
public interface INetMessage : IPacketSerializable
{
  bool ShouldBroadcast { get; }

  NetTransferMode Mode { get; }

  LogLevel LogLevel { get; }

  bool ShouldBuffer { get; }
}
