// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BigBang
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class BigBang : CardModel
{
  public BigBang()
    : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[4]
      {
        (DynamicVar) new CardsVar(1),
        (DynamicVar) new EnergyVar(1),
        (DynamicVar) new StarsVar(1),
        (DynamicVar) new ForgeVar(5)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      List<IHoverTip> items = new List<IHoverTip>();
      items.Add(this.EnergyHoverTip);
      items.AddRange(HoverTipFactory.FromForge());
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
    await PlayerCmd.GainStars(this.DynamicVars.Stars.BaseValue, this.Owner);
    await PlayerCmd.GainEnergy((Decimal) this.DynamicVars.Energy.IntValue, this.Owner);
    IEnumerable<SovereignBlade> sovereignBlades = await ForgeCmd.Forge((Decimal) this.DynamicVars.Forge.IntValue, this.Owner, (AbstractModel) this);
  }

  protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Innate);
}
