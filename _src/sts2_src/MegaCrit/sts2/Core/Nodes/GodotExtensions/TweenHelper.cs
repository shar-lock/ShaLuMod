// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.TweenHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

public static class TweenHelper
{
  public static void FastForwardToCompletion(this Tween t) => t.CustomStep(999999999.0);

  public static async Task<bool> AwaitFinished(this Tween tween, Node owner)
  {
    return await TweenHelper.AwaitFinishedInternal(tween, owner) && GodotObject.IsInstanceValid((GodotObject) owner) && owner.IsInsideTree();
  }

  private static Task<bool> AwaitFinishedInternal(Tween tween, Node owner)
  {
    if (!tween.IsValid() || !tween.IsRunning())
      return Task.FromResult<bool>(true);
    TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>((TaskCreationOptions) 64 /*0x40*/);
    bool resolved = false;
    tween.Finished += new Action(OnFinished);
    owner.TreeExiting += new Action(OnExiting);
    if (!tween.IsValid() || !tween.IsRunning())
      OnFinished();
    else if (!owner.IsInsideTree())
      OnExiting();
    return tcs.Task;

    void OnFinished()
    {
      if (resolved)
        return;
      resolved = true;
      if (tween.IsValid())
        tween.Finished -= new Action(OnFinished);
      if (GodotObject.IsInstanceValid((GodotObject) owner))
        owner.TreeExiting -= new Action(OnExiting);
      tcs.TrySetResult(true);
    }

    void OnExiting()
    {
      if (resolved)
        return;
      resolved = true;
      if (tween.IsValid())
        tween.Finished -= new Action(OnFinished);
      if (GodotObject.IsInstanceValid((GodotObject) owner))
        owner.TreeExiting -= new Action(OnExiting);
      tcs.TrySetResult(false);
    }
  }

  public static Task AwaitFinished(this Tween tween, CancellationToken ct)
  {
    TaskCompletionSource tcs = new TaskCompletionSource();
    int unsubscribed = 0;
    CancellationTokenRegistration ctr = new CancellationTokenRegistration();
    tween.Finished += new Action(OnFinished);
    if (ct.CanBeCanceled)
      ctr = ct.Register((Action) (() =>
      {
        if (Interlocked.Exchange(ref unsubscribed, 1) == 0)
          tween.Finished -= new Action(OnFinished);
        tcs.TrySetCanceled(ct);
      }));
    return tcs.Task;

    void OnFinished()
    {
      if (Interlocked.Exchange(ref unsubscribed, 1) == 0)
        tween.Finished -= new Action(OnFinished);
      ctr.Dispose();
      tcs.TrySetResult();
    }
  }
}
