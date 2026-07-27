// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TouchOfOrobas
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TouchOfOrobas : RelicModel
{
  private const string _starterRelicKey = "StarterRelic";
  private const string _upgradedRelicKey = "UpgradedRelic";
  private ModelId? _starterRelic;
  private ModelId? _upgradedRelic;
  private List<IHoverTip> _extraHoverTips = new List<IHoverTip>();

  public override RelicRarity Rarity => RelicRarity.Ancient;

  private static Dictionary<ModelId, RelicModel> RefinementUpgrades
  {
    get
    {
      return new Dictionary<ModelId, RelicModel>()
      {
        {
          ModelDb.Relic<BurningBlood>().Id,
          (RelicModel) ModelDb.Relic<BlackBlood>()
        },
        {
          ModelDb.Relic<RingOfTheSnake>().Id,
          (RelicModel) ModelDb.Relic<RingOfTheDrake>()
        },
        {
          ModelDb.Relic<DivineRight>().Id,
          (RelicModel) ModelDb.Relic<DivineDestiny>()
        },
        {
          ModelDb.Relic<BoundPhylactery>().Id,
          (RelicModel) ModelDb.Relic<PhylacteryUnbound>()
        },
        {
          ModelDb.Relic<CrackedCore>().Id,
          (RelicModel) ModelDb.Relic<InfusedCore>()
        }
      };
    }
  }

  [SavedProperty]
  public ModelId? StarterRelic
  {
    get => this._starterRelic;
    set
    {
      this.AssertMutable();
      this._starterRelic = !(this._starterRelic != (ModelId) null) ? value : throw new InvalidOperationException("Recursive Core setup called twice!");
      if (!(this._starterRelic != (ModelId) null))
        return;
      RelicModel relicModel = SaveUtil.RelicOrDeprecated(this._starterRelic);
      this._extraHoverTips.AddRange(relicModel.HoverTips);
      ((StringVar) this.DynamicVars[nameof (StarterRelic)]).StringValue = relicModel.Title.GetFormattedText();
    }
  }

  [SavedProperty]
  public ModelId? UpgradedRelic
  {
    get => this._upgradedRelic;
    set
    {
      this.AssertMutable();
      this._upgradedRelic = !(this._upgradedRelic != (ModelId) null) ? value : throw new InvalidOperationException("Recursive Core setup called twice!");
      if (!(this._upgradedRelic != (ModelId) null))
        return;
      RelicModel relicModel = SaveUtil.RelicOrDeprecated(this._upgradedRelic);
      this._extraHoverTips.AddRange(relicModel.HoverTips);
      ((StringVar) this.DynamicVars[nameof (UpgradedRelic)]).StringValue = relicModel.Title.GetFormattedText();
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) this._extraHoverTips;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("StarterRelic"),
        (DynamicVar) new StringVar("UpgradedRelic")
      });
    }
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this._extraHoverTips = new List<IHoverTip>();
  }

  private RelicModel? GetStarterRelic(Player p)
  {
    return p.Relics.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r.Rarity == RelicRarity.Starter));
  }

  public RelicModel GetUpgradedStarterRelic(RelicModel starterRelic)
  {
    RelicModel relicModel;
    return TouchOfOrobas.RefinementUpgrades.TryGetValue(starterRelic.Id, out relicModel) ? relicModel : ModelDb.Relic<Circlet>().ToMutable();
  }

  public bool SetupForPlayer(Player player)
  {
    this.AssertMutable();
    RelicModel starterRelic = this.GetStarterRelic(player);
    if (starterRelic == null)
      return false;
    this.StarterRelic = starterRelic.Id;
    this.UpgradedRelic = this.GetUpgradedStarterRelic(starterRelic).Id;
    return true;
  }

  public void SetupForTests(ModelId starterRelic, ModelId upgradedRelic)
  {
    this.AssertMutable();
    this.StarterRelic = starterRelic;
    this.UpgradedRelic = upgradedRelic;
  }

  public override async Task AfterObtained()
  {
    ModelId id1 = this.StarterRelic;
    if ((object) id1 == null)
      id1 = this.Owner.Relics.First<RelicModel>((Func<RelicModel, bool>) (r => r.Rarity == RelicRarity.Starter)).Id;
    RelicModel relicById = this.Owner.GetRelicById(id1);
    ModelId id2 = this.UpgradedRelic;
    if ((object) id2 == null)
      id2 = this.GetUpgradedStarterRelic(relicById).Id;
    RelicModel mutable = ModelDb.GetById<RelicModel>(id2).ToMutable();
    RelicModel relicModel = await RelicCmd.Replace(relicById, mutable);
  }
}
