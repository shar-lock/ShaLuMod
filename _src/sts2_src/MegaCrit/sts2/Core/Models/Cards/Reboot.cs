// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Reboot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Reboot : CardModel
{
  public Reboot()
    : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(4));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    foreach (CardModel card in PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>())
    {
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Draw);
    }
    await CardPileCmd.Shuffle(choiceContext, this.Owner);
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(2M);
}
