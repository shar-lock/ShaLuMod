// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Fasten
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
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Fasten : CardModel
{
  private const string _extraBlockKey = "ExtraBlock";

  public Fasten()
    : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      CardModel card = (CardModel) null;
      if (this.IsMutable && this.Owner != null)
        card = this.Owner.Character.CardPool.GetUnlockedCards(this.Owner.UnlockState, this.Owner.RunState.CardMultiplayerConstraint).First<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Defend)));
      if (card == null)
        card = (CardModel) ModelDb.Card<DefendIronclad>();
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromCard(card)
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("ExtraBlock", 4M));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    FastenPower fastenPower = await PowerCmd.Apply<FastenPower>(choiceContext, this.Owner.Creature, this.DynamicVars["ExtraBlock"].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars["ExtraBlock"].UpgradeValueBy(2M);
}
