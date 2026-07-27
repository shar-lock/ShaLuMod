// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.AbstractRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public abstract class AbstractRoom
{
  public abstract RoomType RoomType { get; }

  public abstract ModelId? ModelId { get; }

  public virtual bool IsPreFinished => false;

  public int? Id { get; private set; }

  public Task Enter(IRunState? runState, bool isRestoringRoomStackBase)
  {
    this.Id = runState?.GetAndIncrementNextRoomId();
    return this.EnterInternal(runState, isRestoringRoomStackBase);
  }

  public abstract Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase);

  public abstract Task Exit(IRunState? runState);

  public abstract Task Resume(AbstractRoom exitedRoom, IRunState? runState);

  public bool IsVictoryRoom
  {
    get => this is EventRoom eventRoom && eventRoom.CanonicalEvent is TheArchitect;
  }

  public virtual SerializableRoom ToSerializable()
  {
    return new SerializableRoom()
    {
      RoomType = this.RoomType
    };
  }

  public static AbstractRoom? FromSerializable(
    SerializableRoom? serializableRoom,
    IRunState? runState)
  {
    if (serializableRoom == null)
      return (AbstractRoom) null;
    switch (serializableRoom.RoomType)
    {
      case RoomType.Monster:
      case RoomType.Elite:
      case RoomType.Boss:
        return (AbstractRoom) CombatRoom.FromSerializable(serializableRoom, runState);
      case RoomType.Event:
        return (AbstractRoom) new EventRoom(serializableRoom);
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
