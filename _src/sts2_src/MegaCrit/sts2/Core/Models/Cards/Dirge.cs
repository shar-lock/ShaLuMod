// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Dirge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Dirge : CardModel
{
  public Dirge()
    : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override bool HasEnergyCostX => true;

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
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new SummonVar(3M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.SummonDynamic, (DynamicVar) this.DynamicVars.Summon),
        HoverTipFactory.FromCard<Soul>(this.IsUpgraded)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    int xValue = this.ResolveEnergyXValue();
    for (int i = 0; i < xValue; ++i)
    {
      SummonResult summonResult = await OstyCmd.Summon(choiceContext, this.Owner, this.DynamicVars.Summon.BaseValue, (AbstractModel) this);
    }
    List<Soul> list = Soul.Create(this.Owner, xValue, this.CombatState).ToList<Soul>();
    if (this.IsUpgraded)
    {
      foreach (CardModel card in list)
        CardCmd.Upgrade(card);
    }
    CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) list, PileType.Draw, this.Owner, CardPilePosition.Random));
  }

  protected override void OnUpgrade() => this.DynamicVars.Summon.UpgradeValueBy(1M);
}
