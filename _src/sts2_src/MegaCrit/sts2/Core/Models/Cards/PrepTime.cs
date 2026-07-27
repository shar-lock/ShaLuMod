// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.PrepTime
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class PrepTime : CardModel
{
  public PrepTime()
    : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<PrepTimePower>(4M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<VigorPower>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    PrepTimePower prepTimePower = await PowerCmd.Apply<PrepTimePower>(choiceContext, this.Owner.Creature, this.DynamicVars["PrepTimePower"].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars["PrepTimePower"].UpgradeValueBy(2M);
}
