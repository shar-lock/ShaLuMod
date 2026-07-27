// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.StormOfSteel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class StormOfSteel : CardModel
{
  public StormOfSteel()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Shiv>(this.IsUpgraded));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    IEnumerable<CardModel> list = (IEnumerable<CardModel>) PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>();
    int handSize = list.Count<CardModel>();
    await CardCmd.Discard(choiceContext, list);
    await Cmd.CustomScaledWait(0.0f, 0.25f);
    IEnumerable<CardModel> inHand = await Shiv.CreateInHand(this.Owner, handSize, this.CombatState);
    if (!this.IsUpgraded)
      return;
    foreach (CardModel card in inHand)
      CardCmd.Upgrade(card);
  }
}
