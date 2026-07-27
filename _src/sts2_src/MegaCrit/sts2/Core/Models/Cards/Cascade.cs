// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Cascade
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Cascade : CardModel
{
  public Cascade()
    : base(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override bool HasEnergyCostX => true;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int count = this.ResolveEnergyXValue();
    if (this.IsUpgraded)
      ++count;
    await CardPileCmd.AutoPlayFromDrawPile(choiceContext, this.Owner, count, CardPilePosition.Top, false);
  }
}
