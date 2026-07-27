// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.ThrowingPlayerChoiceContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class ThrowingPlayerChoiceContext : PlayerChoiceContext
{
  public override ulong? OwnerId => new ulong?();

  public override Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
  {
    throw new NotImplementedException();
  }

  public override Task SignalPlayerChoiceEnded() => throw new NotImplementedException();
}
