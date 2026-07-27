// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Cleanse
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Cleanse : CardModel
{
  public Cleanse()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
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
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, Necrobinder.GetSummonAnimIfApplicable(this.Owner.Character), Necrobinder.GetSummonDelayIfApplicable(this.Owner.Character));
    SummonResult summonResult = await OstyCmd.Summon(choiceContext, this.Owner, this.DynamicVars.Summon.BaseValue, (AbstractModel) this);
    CardModel card = (await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(this.Owner), this.Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1))).FirstOrDefault<CardModel>();
    if (card == null)
      return;
    await CardCmd.Exhaust(choiceContext, card);
  }

  protected override void OnUpgrade() => this.DynamicVars.Summon.UpgradeValueBy(2M);
}
