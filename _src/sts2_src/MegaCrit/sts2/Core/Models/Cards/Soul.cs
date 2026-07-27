// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Soul
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Soul : CardModel
{
  public Soul()
    : base(0, CardType.Skill, CardRarity.Token, TargetType.Self)
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
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  public static async Task<IEnumerable<Soul>> CreateInHand(
    Player owner,
    int amount,
    ICombatState combatState,
    Player? creator = null)
  {
    IEnumerable<Soul> souls = Soul.Create(owner, amount, combatState);
    IReadOnlyList<CardPileAddResult> combat = await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) souls, PileType.Hand, creator ?? owner);
    IEnumerable<Soul> inHand = souls;
    souls = (IEnumerable<Soul>) null;
    return inHand;
  }

  public static IEnumerable<Soul> Create(Player owner, int amount, ICombatState combatState)
  {
    List<Soul> soulList = new List<Soul>();
    for (int index = 0; index < amount; ++index)
      soulList.Add(combatState.CreateCard<Soul>(owner));
    return (IEnumerable<Soul>) soulList;
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, this.DynamicVars.Cards.BaseValue, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}
