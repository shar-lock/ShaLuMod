// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.SpineNodeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class SpineNodeExtensions
{
  private const int _spineReadyWarnThresholdFrames = 600;

  public static void RunWhenSpineReady(
    this Node host,
    MegaSprite sprite,
    Action<MegaAnimationState> onReady)
  {
    TaskHelper.RunSafely(SpineNodeExtensions.WaitForSpineReady(host, sprite, onReady));
  }

  private static async Task WaitForSpineReady(
    Node host,
    MegaSprite sprite,
    Action<MegaAnimationState> onReady)
  {
    int framesWaited = 0;
    while (GodotObject.IsInstanceValid(sprite.BoundObject) && !sprite.IsAnimationStateReady())
    {
      double num = (double) await host.AwaitProcessFrame();
      if (++framesWaited == 600)
        Log.Warn($"{host.Name}: still waiting for a SpineSprite skeleton after {framesWaited} " + "frames; its animation will not start until the skeleton loads (possible asset load failure).");
    }
    if (!GodotObject.IsInstanceValid(sprite.BoundObject))
      return;
    onReady(sprite.GetAnimationState());
  }
}
