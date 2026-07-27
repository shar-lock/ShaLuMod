// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Connection.ClientConnectionFailedException
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Connection;

public class ClientConnectionFailedException : Exception
{
  public NetErrorInfo info;

  public ClientConnectionFailedException(string message, NetErrorInfo info)
    : base(message)
  {
    this.info = info;
  }
}
