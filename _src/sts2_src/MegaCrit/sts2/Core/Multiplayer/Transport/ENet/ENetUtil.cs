// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;

public static class ENetUtil
{
  public static NetTransferMode ModeFromFlags(int flags)
  {
    if (((long) flags & 1L) > 0L)
      return NetTransferMode.Reliable;
    if (((long) flags & 8L) > 0L)
      return NetTransferMode.Unreliable;
    throw new ArgumentOutOfRangeException($"Flags {flags} cannot be mapped to NetTransferMode!");
  }

  public static int FlagsFromMode(NetTransferMode mode)
  {
    if (mode == NetTransferMode.Unreliable)
      return 8;
    if (mode == NetTransferMode.Reliable)
      return 1;
    throw new ArgumentOutOfRangeException(nameof (mode), (object) mode, (string) null);
  }
}
