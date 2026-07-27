// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.HungryForMushrooms
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class HungryForMushrooms : EventModel
{
  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      this.RelicOption<MegaCrit.Sts2.Core.Models.Relics.BigMushroom>(new Func<Task>(this.BigMushroom)),
      this.RelicOption<MegaCrit.Sts2.Core.Models.Relics.FragrantMushroom>(new Func<Task>(this.FragrantMushroom)).ThatDoesDamage(15M)
    });
  }

  private async Task BigMushroom()
  {
    MegaCrit.Sts2.Core.Models.Relics.BigMushroom bigMushroom = await RelicCmd.Obtain<MegaCrit.Sts2.Core.Models.Relics.BigMushroom>(this.Owner);
    this.SetEventFinished(this.L10NLookup("HUNGRY_FOR_MUSHROOMS.pages.BIG_MUSHROOM.description"));
  }

  private async Task FragrantMushroom()
  {
    MegaCrit.Sts2.Core.Models.Relics.FragrantMushroom fragrantMushroom = await RelicCmd.Obtain<MegaCrit.Sts2.Core.Models.Relics.FragrantMushroom>(this.Owner);
    this.SetEventFinished(this.L10NLookup("HUNGRY_FOR_MUSHROOMS.pages.FRAGRANT_MUSHROOM.description"));
  }
}
