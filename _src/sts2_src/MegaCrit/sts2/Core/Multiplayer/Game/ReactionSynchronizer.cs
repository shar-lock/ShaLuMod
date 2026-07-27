// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.ReactionSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;
using MegaCrit.Sts2.Core.Nodes.Reaction;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class ReactionSynchronizer : IDisposable
{
  private readonly NReactionContainer _container;

  public INetGameService NetService { get; }

  public ReactionSynchronizer(INetGameService netService, NReactionContainer container)
  {
    this.NetService = netService;
    this._container = container;
    this.NetService.RegisterMessageHandler<ReactionMessage>(new MessageHandlerDelegate<ReactionMessage>(this.HandleReactionMessage));
  }

  public void Dispose()
  {
    this.NetService.UnregisterMessageHandler<ReactionMessage>(new MessageHandlerDelegate<ReactionMessage>(this.HandleReactionMessage));
  }

  public void SendLocalReaction(ReactionType type, Vector2 mouseScreenPos)
  {
    this.NetService.SendMessage<ReactionMessage>(new ReactionMessage()
    {
      type = type,
      normalizedPosition = NetCursorHelper.GetNormalizedPosition(mouseScreenPos, (Control) this._container)
    });
  }

  private void HandleReactionMessage(ReactionMessage message, ulong senderId)
  {
    this._container.DoRemoteReaction(message.type, NetCursorHelper.GetControlSpacePosition(message.normalizedPosition, (Control) this._container));
  }
}
