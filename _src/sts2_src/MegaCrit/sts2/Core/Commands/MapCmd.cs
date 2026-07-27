// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.MapCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class MapCmd
{
  public static void SetBossEncounter(IRunState runState, EncounterModel boss)
  {
    runState.Act.SetBossEncounter(boss);
    if (!TestMode.IsOff)
      return;
    NRun.Instance.GlobalUi.TopBar.BossIcon.RefreshBossIcon();
    NMapScreen.Instance?.SetMap(runState.Map, runState.Rng.Seed, false);
  }
}
