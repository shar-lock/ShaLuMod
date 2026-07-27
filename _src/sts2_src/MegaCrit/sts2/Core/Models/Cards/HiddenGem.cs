// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.HiddenGem
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
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class HiddenGem : CardModel
{
  private const string _replayKey = "Replay";

  public HiddenGem()
    : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new IntVar("Replay", 2M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayStatic));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    List<CardModel> list1 = PileType.Draw.GetPile(this.Owner).Cards.ToList<CardModel>();
    if (list1.Count == 0)
      return;
    List<CardModel> list2 = list1.Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      bool flag1 = !c.Keywords.Contains(CardKeyword.Unplayable);
      if (flag1)
      {
        bool flag2;
        switch (c.Type)
        {
          case CardType.Curse:
          case CardType.Quest:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = !flag2;
      }
      return flag1 && c.GetEnchantedReplayCount() < 1;
    })).ToList<CardModel>();
    List<CardModel> list3 = list2.Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      bool flag;
      switch (c.Type)
      {
        case CardType.Attack:
        case CardType.Skill:
        case CardType.Power:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    })).ToList<CardModel>();
    CardModel card = this.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(list3.Count == 0 ? (IEnumerable<CardModel>) list2 : (IEnumerable<CardModel>) list3);
    if (card == null)
      return;
    card.BaseReplayCount += this.DynamicVars["Replay"].IntValue;
    CardCmd.Preview(card);
  }

  protected override void OnUpgrade() => this.DynamicVars["Replay"].UpgradeValueBy(1M);
}
