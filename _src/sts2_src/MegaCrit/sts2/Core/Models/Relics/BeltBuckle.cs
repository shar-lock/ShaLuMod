// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BeltBuckle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BeltBuckle : RelicModel
{
  private bool _dexterityApplied;

  public override RelicRarity Rarity => RelicRarity.Shop;

  private bool DexterityApplied
  {
    get => this._dexterityApplied;
    set
    {
      this.AssertMutable();
      this._dexterityApplied = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<DexterityPower>(2M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<DexterityPower>());
    }
  }

  public override async Task AfterObtained()
  {
    if (!CombatManager.Instance.IsInProgress || this.Owner.Potions.Any<PotionModel>())
      return;
    await this.ApplyDexterity();
  }

  public override async Task BeforeCombatStart()
  {
    this.DexterityApplied = false;
    this.RefreshStatus();
    if (this.Owner.Potions.Any<PotionModel>())
      return;
    await this.ApplyDexterity();
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.RefreshStatus();
    return Task.CompletedTask;
  }

  public override async Task AfterPotionProcured(PotionModel potion)
  {
    this.RefreshStatus();
    if (!CombatManager.Instance.IsInProgress || !this.Owner.Potions.Any<PotionModel>())
      return;
    await this.RemoveDexterity();
  }

  public override async Task AfterPotionDiscarded(PotionModel potion)
  {
    this.RefreshStatus();
    if (!CombatManager.Instance.IsInProgress || this.Owner.Potions.Any<PotionModel>())
      return;
    await this.ApplyDexterity();
  }

  public override async Task AfterPotionUsed(PotionModel potion, Creature? target)
  {
    this.RefreshStatus();
    if (!CombatManager.Instance.IsInProgress || this.Owner.Potions.Any<PotionModel>())
      return;
    await this.ApplyDexterity();
  }

  public override Task AfterCombatVictory(CombatRoom room)
  {
    this.DexterityApplied = false;
    this.RefreshStatus();
    return Task.CompletedTask;
  }

  private async Task ApplyDexterity()
  {
    if (this.DexterityApplied)
      return;
    this.DexterityApplied = true;
    this.Flash();
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.Dexterity.BaseValue, (Creature) null, (CardModel) null);
  }

  private async Task RemoveDexterity()
  {
    if (!this.DexterityApplied)
      return;
    this.DexterityApplied = false;
    this.Flash();
    DexterityPower dexterityPower = await PowerCmd.Apply<DexterityPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, -this.DynamicVars.Dexterity.BaseValue, (Creature) null, (CardModel) null);
  }

  private void RefreshStatus()
  {
    if (CombatManager.Instance.IsInProgress && !this.Owner.Potions.Any<PotionModel>())
      this.Status = RelicStatus.Active;
    else
      this.Status = RelicStatus.Normal;
  }
}
