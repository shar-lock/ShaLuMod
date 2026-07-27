// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.MapRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class MapRoom : AbstractRoom
{
  public override RoomType RoomType => RoomType.Map;

  public override ModelId? ModelId => (ModelId) null;

  public override Task EnterInternal(IRunState? runState, bool isRestoringRoomStackBase)
  {
    if (isRestoringRoomStackBase)
      throw new InvalidOperationException("MapRoom does not support room stack reconstruction.");
    if (TestMode.IsOn)
      return Task.CompletedTask;
    NRun.Instance.SetCurrentRoom((Control) NMapRoom.Create(runState?.Act ?? (ActModel) ModelDb.Act<Overgrowth>(), runState != null ? runState.CurrentActIndex : 0));
    return Task.CompletedTask;
  }

  public override Task Exit(IRunState? runState) => Task.CompletedTask;

  public override Task Resume(AbstractRoom _, IRunState? runState)
  {
    throw new NotImplementedException();
  }
}
