// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TeaMaster
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TeaMaster : EventModel
{
  private const string _boneTeaCostKey = "BoneTeaCost";
  private const string _emberTeaCostKey = "EmberTeaCost";
  private const int _boneTeaCost = 50;
  private const int _emberTeaCost = 150;

  public override bool IsAllowed(IRunState runState)
  {
    return runState.CurrentActIndex < 2 && runState.Players.All<Player>((Func<Player, bool>) (p => p.Gold >= 150));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[5]
      {
        new DynamicVar("BoneTeaCost", 50M),
        new DynamicVar("EmberTeaCost", 150M),
        (DynamicVar) new StringVar("BoneTeaDescription", ModelDb.Relic<MegaCrit.Sts2.Core.Models.Relics.BoneTea>().DynamicDescription.GetFormattedText()),
        (DynamicVar) new StringVar("EmberTeaDescription", ModelDb.Relic<MegaCrit.Sts2.Core.Models.Relics.EmberTea>().DynamicDescription.GetFormattedText()),
        (DynamicVar) new StringVar("TeaOfDiscourtesyDescription", ModelDb.Relic<MegaCrit.Sts2.Core.Models.Relics.TeaOfDiscourtesy>().DynamicDescription.GetFormattedText())
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    if ((Decimal) this.Owner.Gold >= this.DynamicVars["BoneTeaCost"].BaseValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.BoneTea), "TEA_MASTER.pages.INITIAL.options.BONE_TEA", HoverTipFactory.FromRelicExcludingItself<MegaCrit.Sts2.Core.Models.Relics.BoneTea>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "TEA_MASTER.pages.INITIAL.options.BONE_TEA_LOCKED", Array.Empty<IHoverTip>()));
    if ((Decimal) this.Owner.Gold >= this.DynamicVars["EmberTeaCost"].BaseValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.EmberTea), "TEA_MASTER.pages.INITIAL.options.EMBER_TEA", HoverTipFactory.FromRelicExcludingItself<MegaCrit.Sts2.Core.Models.Relics.EmberTea>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "TEA_MASTER.pages.INITIAL.options.EMBER_TEA_LOCKED", Array.Empty<IHoverTip>()));
    initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.TeaOfDiscourtesy), "TEA_MASTER.pages.INITIAL.options.TEA_OF_DISCOURTESY", HoverTipFactory.FromRelicExcludingItself<MegaCrit.Sts2.Core.Models.Relics.TeaOfDiscourtesy>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task BoneTea()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["BoneTeaCost"].BaseValue, this.Owner, GoldLossType.Spent);
    MegaCrit.Sts2.Core.Models.Relics.BoneTea boneTea = await RelicCmd.Obtain<MegaCrit.Sts2.Core.Models.Relics.BoneTea>(this.Owner);
    this.SetEventFinished(this.L10NLookup("TEA_MASTER.pages.DONE.description"));
  }

  private async Task EmberTea()
  {
    await PlayerCmd.LoseGold(this.DynamicVars["EmberTeaCost"].BaseValue, this.Owner, GoldLossType.Spent);
    MegaCrit.Sts2.Core.Models.Relics.EmberTea emberTea = await RelicCmd.Obtain<MegaCrit.Sts2.Core.Models.Relics.EmberTea>(this.Owner);
    this.SetEventFinished(this.L10NLookup("TEA_MASTER.pages.DONE.description"));
  }

  private async Task TeaOfDiscourtesy()
  {
    MegaCrit.Sts2.Core.Models.Relics.TeaOfDiscourtesy teaOfDiscourtesy = await RelicCmd.Obtain<MegaCrit.Sts2.Core.Models.Relics.TeaOfDiscourtesy>(this.Owner);
    this.SetEventFinished(this.L10NLookup("TEA_MASTER.pages.TEA_OF_DISCOURTESY.description"));
  }
}
