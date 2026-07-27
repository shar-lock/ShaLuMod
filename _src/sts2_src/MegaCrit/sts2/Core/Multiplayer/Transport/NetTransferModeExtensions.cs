// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.NetTransferModeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport;

public static class NetTransferModeExtensions
{
  public static int ToChannelId(this NetTransferMode mode)
  {
    if (mode == NetTransferMode.Unreliable)
      return 1;
    if (mode == NetTransferMode.Reliable)
      return 0;
    throw new ArgumentOutOfRangeException(nameof (mode), (object) mode, (string) null);
  }
}
