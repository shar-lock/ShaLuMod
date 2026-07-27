// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.DeprecatedAncientEvent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Ancients;
using MegaCrit.Sts2.Core.Events;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class DeprecatedAncientEvent : AncientEventModel
{
  protected override AncientDialogueSet DefineDialogues() => throw new NotImplementedException();

  public override IEnumerable<EventOption> AllPossibleOptions
  {
    get => (IEnumerable<EventOption>) Array.Empty<EventOption>();
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    return (IReadOnlyList<EventOption>) Array.Empty<EventOption>();
  }
}
