// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.INetActionSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.DevConsole;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public static class INetActionSubtypes
{
  private static readonly Type _t0 = typeof (NetConsoleCmdGameAction);
  private static readonly Type _t1 = typeof (NetDiscardPotionGameAction);
  private static readonly Type _t2 = typeof (NetEndPlayerTurnAction);
  private static readonly Type _t3 = typeof (NetMoveToMapCoordAction);
  private static readonly Type _t4 = typeof (NetPickRelicAction);
  private static readonly Type _t5 = typeof (NetPlayCardAction);
  private static readonly Type _t6 = typeof (NetReadyToBeginEnemyTurnAction);
  private static readonly Type _t7 = typeof (NetUndoEndPlayerTurnAction);
  private static readonly Type _t8 = typeof (NetUsePotionAction);
  private static readonly Type _t9 = typeof (NetVoteForMapCoordAction);
  private static readonly Type _t10 = typeof (NetVoteToMoveToNextActAction);
  private static readonly Type[] _subtypes = new Type[11]
  {
    INetActionSubtypes._t0,
    INetActionSubtypes._t1,
    INetActionSubtypes._t2,
    INetActionSubtypes._t3,
    INetActionSubtypes._t4,
    INetActionSubtypes._t5,
    INetActionSubtypes._t6,
    INetActionSubtypes._t7,
    INetActionSubtypes._t8,
    INetActionSubtypes._t9,
    INetActionSubtypes._t10
  };

  public static int Count => 11;

  public static IReadOnlyList<Type> All => (IReadOnlyList<Type>) INetActionSubtypes._subtypes;

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  public static Type Get(int i) => INetActionSubtypes._subtypes[i];
}
