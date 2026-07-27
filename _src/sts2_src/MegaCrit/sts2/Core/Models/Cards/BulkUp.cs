// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.BulkUp
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

public sealed class BulkUp : CardModel
{
  private const string _orbSlotVar = "OrbSlots";

  public BulkUp()
    : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>()
      });
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        new DynamicVar("OrbSlots", 1M),
        (DynamicVar) new PowerVar<StrengthPower>(2M),
        (DynamicVar) new PowerVar<DexterityPower>(2M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
    OrbCmd.RemoveSlots(this.Owner, this.DynamicVars["OrbSlots"].IntValue);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner.Creature, this.DynamicVars.Strength.BaseValue, this.Owner.Creature, (CardModel) this);
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>(choiceContext, this.Owner.Creature, this.DynamicVars.Dexterity.BaseValue, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Strength.UpgradeValueBy(1M);
    this.DynamicVars.Dexterity.UpgradeValueBy(1M);
  }
}
