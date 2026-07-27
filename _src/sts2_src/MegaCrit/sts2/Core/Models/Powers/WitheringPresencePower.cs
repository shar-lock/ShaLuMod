// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.WitheringPresencePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class WitheringPresencePower : PowerModel
{
  private const int _baseCardsLeft = 6;
  private const string _cardsLeftKey = "CardsLeft";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.DynamicVars["CardsLeft"].IntValue;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("CardsLeft", 6M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      Wither mutable = (Wither) ModelDb.Card<Wither>().ToMutable();
      if (this.Owner.Monster is Aeonglass monster)
        monster.MatchWitherToUpgradeCount(mutable);
      List<IHoverTip> items = new List<IHoverTip>();
      items.Add(HoverTipFactory.FromCard((CardModel) mutable));
      items.AddRange(ModelDb.Card<Wither>().HoverTips);
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
    }
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Target.Player)
      return;
    --this.DynamicVars["CardsLeft"].BaseValue;
    this.InvokeDisplayAmountChanged();
    if (this.DynamicVars["CardsLeft"].IntValue > 0)
      return;
    await Cmd.Wait(0.5f);
    await CardPileCmd.AddToCombatAndPreview<Wither>(cardPlay.Card.Owner.Creature, PileType.Hand, 1, (Player) null);
    this.Flash();
    this.DynamicVars["CardsLeft"].BaseValue = 6M;
    this.InvokeDisplayAmountChanged();
  }
}
