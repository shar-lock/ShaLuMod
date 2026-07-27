// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LoomingFruit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LoomingFruit : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override string IconBaseName
  {
    get => !LoomingFruit.HasCornucopia() ? base.IconBaseName + "_2" : base.IconBaseName;
  }

  private static bool HasCornucopia()
  {
    string uniqueId = SaveManager.Instance.Progress.UniqueId;
    if (string.IsNullOrEmpty(uniqueId))
      return false;
    string str = uniqueId;
    return (int) str[str.Length - 1] % 2 == 0;
  }

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new MaxHpVar(31M));
    }
  }

  public override async Task AfterObtained()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
  }
}
