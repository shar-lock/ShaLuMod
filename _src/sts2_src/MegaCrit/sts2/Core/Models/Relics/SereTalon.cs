// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SereTalon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SereTalon : RelicModel
{
  public const int maxHpLoss = 9;
  private const string _wishesKey = "Wishes";

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new HpLossVar(9M),
        new DynamicVar("Wishes", 3M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<Wish>();
  }

  public override async Task AfterObtained()
  {
    await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, false);
    List<CardPileAddResult> wishResults = new List<CardPileAddResult>();
    for (int i = 0; i < this.DynamicVars["Wishes"].IntValue; ++i)
      wishResults.Add(await CardPileCmd.Add(this.Owner.RunState.CreateCard((CardModel) ModelDb.Card<Wish>(), this.Owner), PileType.Deck));
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) wishResults, 2f);
    await Cmd.Wait(0.75f);
    wishResults = (List<CardPileAddResult>) null;
  }
}
