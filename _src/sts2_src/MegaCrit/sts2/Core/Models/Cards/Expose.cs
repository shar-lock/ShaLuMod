// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Expose
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Expose : CardModel
{
  private const string _powerKey = "Power";

  public Expose()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Power", 2M));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[3]
      {
        HoverTipFactory.FromPower<VulnerablePower>(),
        HoverTipFactory.FromPower<ArtifactPower>(),
        HoverTipFactory.Static(StaticHoverTip.Block)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    VfxCmd.PlayOnCreatureCenter(this.Owner.Creature, "vfx/vfx_flying_slash");
    int amount = this.DynamicVars["Power"].IntValue;
    await CreatureCmd.LoseBlock(choiceContext, cardPlay.Target, (Decimal) cardPlay.Target.Block, this.Owner.Creature);
    if (cardPlay.Target.HasPower<ArtifactPower>())
      await PowerCmd.Remove<ArtifactPower>(cardPlay.Target);
    VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, (Decimal) amount, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars["Power"].UpgradeValueBy(1M);
}
