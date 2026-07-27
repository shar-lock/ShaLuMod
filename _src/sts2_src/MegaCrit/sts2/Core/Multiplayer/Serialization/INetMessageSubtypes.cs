// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.INetMessageSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Messages;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class INetMessageSubtypes
{
  private static readonly Type _t0 = typeof (ActionEnqueuedMessage);
  private static readonly Type _t1 = typeof (CardRemovedMessage);
  private static readonly Type _t2 = typeof (ChecksumDataMessage);
  private static readonly Type _t3 = typeof (StateDivergenceMessage);
  private static readonly Type _t4 = typeof (CrystalSphereRewardsMessage);
  private static readonly Type _t5 = typeof (ClearMapDrawingsMessage);
  private static readonly Type _t6 = typeof (EndTurnPingMessage);
  private static readonly Type _t7 = typeof (MapDrawingMessage);
  private static readonly Type _t8 = typeof (MapDrawingModeChangedMessage);
  private static readonly Type _t9 = typeof (MapPingMessage);
  private static readonly Type _t10 = typeof (ReactionMessage);
  private static readonly Type _t11 = typeof (RestSiteOptionHoveredMessage);
  private static readonly Type _t12 = typeof (HookActionEnqueuedMessage);
  private static readonly Type _t13 = typeof (MerchantCardRemovalMessage);
  private static readonly Type _t14 = typeof (PlayerChoiceMessage);
  private static readonly Type _t15 = typeof (RequestEnqueueActionMessage);
  private static readonly Type _t16 = typeof (RequestEnqueueHookActionMessage);
  private static readonly Type _t17 = typeof (RequestResumeActionAfterPlayerChoiceMessage);
  private static readonly Type _t18 = typeof (ResumeActionAfterPlayerChoiceMessage);
  private static readonly Type _t19 = typeof (RunAbandonedMessage);
  private static readonly Type _t20 = typeof (GoldLostMessage);
  private static readonly Type _t21 = typeof (OptionIndexChosenMessage);
  private static readonly Type _t22 = typeof (PeerInputMessage);
  private static readonly Type _t23 = typeof (RestSiteSkippedMessage);
  private static readonly Type _t24 = typeof (RewardObtainedMessage);
  private static readonly Type _t25 = typeof (RewardSelectedMessage);
  private static readonly Type _t26 = typeof (RewardSetSkippedMessage);
  private static readonly Type _t27 = typeof (SharedEventOptionChosenMessage);
  private static readonly Type _t28 = typeof (VotedForSharedEventOptionMessage);
  private static readonly Type _t29 = typeof (SyncPlayerDataMessage);
  private static readonly Type _t30 = typeof (SyncRngMessage);
  private static readonly Type _t31 = typeof (TreasureChestOpenedMessage);
  private static readonly Type _t32 = typeof (HeartbeatRequestMessage);
  private static readonly Type _t33 = typeof (HeartbeatResponseMessage);
  private static readonly Type _t34 = typeof (ClientLoadJoinRequestMessage);
  private static readonly Type _t35 = typeof (ClientLoadJoinResponseMessage);
  private static readonly Type _t36 = typeof (ClientLobbyJoinRequestMessage);
  private static readonly Type _t37 = typeof (ClientLobbyJoinResponseMessage);
  private static readonly Type _t38 = typeof (ClientRejoinRequestMessage);
  private static readonly Type _t39 = typeof (ClientRejoinResponseMessage);
  private static readonly Type _t40 = typeof (InitialGameInfoMessage);
  private static readonly Type _t41 = typeof (LobbyAscensionChangedMessage);
  private static readonly Type _t42 = typeof (LobbyBeginLoadedRunMessage);
  private static readonly Type _t43 = typeof (LobbyBeginRunMessage);
  private static readonly Type _t44 = typeof (LobbyModifiersChangedMessage);
  private static readonly Type _t45 = typeof (LobbyPlayerChangedCharacterMessage);
  private static readonly Type _t46 = typeof (LobbyPlayerSetReadyMessage);
  private static readonly Type _t47 = typeof (LobbySeedChangedMessage);
  private static readonly Type _t48 = typeof (PlayerJoinedMessage);
  private static readonly Type _t49 = typeof (PlayerLeftMessage);
  private static readonly Type _t50 = typeof (PlayerReconnectedMessage);
  private static readonly Type _t51 = typeof (PlayerRejoinedMessage);
  private static readonly Type[] _subtypes = new Type[52]
  {
    INetMessageSubtypes._t0,
    INetMessageSubtypes._t1,
    INetMessageSubtypes._t2,
    INetMessageSubtypes._t3,
    INetMessageSubtypes._t4,
    INetMessageSubtypes._t5,
    INetMessageSubtypes._t6,
    INetMessageSubtypes._t7,
    INetMessageSubtypes._t8,
    INetMessageSubtypes._t9,
    INetMessageSubtypes._t10,
    INetMessageSubtypes._t11,
    INetMessageSubtypes._t12,
    INetMessageSubtypes._t13,
    INetMessageSubtypes._t14,
    INetMessageSubtypes._t15,
    INetMessageSubtypes._t16,
    INetMessageSubtypes._t17,
    INetMessageSubtypes._t18,
    INetMessageSubtypes._t19,
    INetMessageSubtypes._t20,
    INetMessageSubtypes._t21,
    INetMessageSubtypes._t22,
    INetMessageSubtypes._t23,
    INetMessageSubtypes._t24,
    INetMessageSubtypes._t25,
    INetMessageSubtypes._t26,
    INetMessageSubtypes._t27,
    INetMessageSubtypes._t28,
    INetMessageSubtypes._t29,
    INetMessageSubtypes._t30,
    INetMessageSubtypes._t31,
    INetMessageSubtypes._t32,
    INetMessageSubtypes._t33,
    INetMessageSubtypes._t34,
    INetMessageSubtypes._t35,
    INetMessageSubtypes._t36,
    INetMessageSubtypes._t37,
    INetMessageSubtypes._t38,
    INetMessageSubtypes._t39,
    INetMessageSubtypes._t40,
    INetMessageSubtypes._t41,
    INetMessageSubtypes._t42,
    INetMessageSubtypes._t43,
    INetMessageSubtypes._t44,
    INetMessageSubtypes._t45,
    INetMessageSubtypes._t46,
    INetMessageSubtypes._t47,
    INetMessageSubtypes._t48,
    INetMessageSubtypes._t49,
    INetMessageSubtypes._t50,
    INetMessageSubtypes._t51
  };

  public static int Count => 52;

  public static IReadOnlyList<Type> All => (IReadOnlyList<Type>) INetMessageSubtypes._subtypes;

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  public static Type Get(int i) => INetMessageSubtypes._subtypes[i];
}
