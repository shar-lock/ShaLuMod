// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.Cmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class Cmd
{
  public static Task Wait(float seconds, bool ignoreCombatEnd = false)
  {
    return Cmd.Wait(seconds, new CancellationToken(), ignoreCombatEnd);
  }

  public static async Task Wait(float seconds, CancellationToken cancelToken, bool ignoreCombatEnd = false)
  {
    if (NonInteractiveMode.IsActive || (double) seconds <= 0.0 || NGame.Instance != null && (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant || !ignoreCombatEnd && CombatManager.Instance.IsEnding))
      return;
    await Cmd.WaitInternal(((SceneTree) Engine.GetMainLoop()).CreateTimer((double) seconds, true, false, false), cancelToken);
  }

  private static async Task WaitInternal(SceneTreeTimer timer, CancellationToken cancellationToken)
  {
    await ((GodotObject) timer).ToSignal((GodotObject) timer, SceneTreeTimer.SignalName.Timeout).ToTask().WaitAsync(cancellationToken);
  }

  public static async Task CustomScaledWait(
    float fastSeconds,
    float standardSeconds,
    bool ignoreCombatEnd = false,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (NonInteractiveMode.IsActive || SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant || !ignoreCombatEnd && CombatManager.Instance.IsEnding)
      return;
    switch (SaveManager.Instance.PrefsSave.FastMode)
    {
      case FastModeType.Normal:
        await Cmd.Wait(standardSeconds, cancellationToken, ignoreCombatEnd);
        break;
      case FastModeType.Fast:
        await Cmd.Wait(fastSeconds, cancellationToken, ignoreCombatEnd);
        break;
      case FastModeType.Instant:
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
