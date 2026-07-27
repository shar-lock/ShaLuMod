// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BoundPhylactery
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BoundPhylactery : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Starter;

  public override bool SpawnsPets => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new SummonVar(1M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.SummonDynamic, (DynamicVar) this.DynamicVars.Summon));
    }
  }

  public override async Task BeforeCombatStart() => await this.SummonPet();

  public override async Task AfterEnergyResetLate(Player player)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber == 1)
      return;
    await this.SummonPet();
  }

  private async Task SummonPet()
  {
    SummonResult summonResult = await OstyCmd.Summon((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, this.DynamicVars.Summon.BaseValue, (AbstractModel) this);
  }
}
