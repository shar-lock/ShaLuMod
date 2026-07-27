// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.WraithForm
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

public sealed class WraithForm : CardModel
{
  public WraithForm()
    : base(3, CardType.Power, CardRarity.Ancient, TargetType.Self)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<IntangiblePower>(2M),
        (DynamicVar) new PowerVar<WraithFormPower>(1M)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<IntangiblePower>(),
        HoverTipFactory.FromPower<DexterityPower>()
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
    IntangiblePower intangiblePower = await PowerCmd.Apply<IntangiblePower>(choiceContext, this.Owner.Creature, this.DynamicVars["IntangiblePower"].BaseValue, this.Owner.Creature, (CardModel) this);
    WraithFormPower wraithFormPower = await PowerCmd.Apply<WraithFormPower>(choiceContext, this.Owner.Creature, this.DynamicVars["WraithFormPower"].BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars["IntangiblePower"].UpgradeValueBy(1M);
}
