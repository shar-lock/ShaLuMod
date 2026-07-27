// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVarSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class DynamicVarSet : 
  IReadOnlyDictionary<string, DynamicVar>,
  IEnumerable<KeyValuePair<string, DynamicVar>>,
  IEnumerable,
  IReadOnlyCollection<KeyValuePair<string, DynamicVar>>
{
  private readonly Dictionary<string, DynamicVar> _vars = new Dictionary<string, DynamicVar>();

  public IEnumerator<KeyValuePair<string, DynamicVar>> GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<string, DynamicVar>>) this._vars.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  public int Count => this._vars.Count;

  public bool ContainsKey(string key) => this._vars.ContainsKey(key);

  public bool TryGetValue(string key, [MaybeNullWhen(false)] out DynamicVar value)
  {
    return this._vars.TryGetValue(key, out value);
  }

  public DynamicVar this[string key] => this._vars[key];

  public IEnumerable<string> Keys => (IEnumerable<string>) this._vars.Keys;

  public IEnumerable<DynamicVar> Values => (IEnumerable<DynamicVar>) this._vars.Values;

  public BlockVar Block => (BlockVar) this._vars[nameof (Block)];

  public CalculatedBlockVar CalculatedBlock
  {
    get => (CalculatedBlockVar) this._vars[nameof (CalculatedBlock)];
  }

  public CalculatedDamageVar CalculatedDamage
  {
    get => (CalculatedDamageVar) this._vars[nameof (CalculatedDamage)];
  }

  public CalculationBaseVar CalculationBase
  {
    get => (CalculationBaseVar) this._vars[nameof (CalculationBase)];
  }

  public CalculationExtraVar CalculationExtra
  {
    get => (CalculationExtraVar) this._vars[nameof (CalculationExtra)];
  }

  public CardsVar Cards => (CardsVar) this._vars[nameof (Cards)];

  public DamageVar Damage => (DamageVar) this._vars[nameof (Damage)];

  public PowerVar<DexterityPower> Dexterity
  {
    get => (PowerVar<DexterityPower>) this._vars["DexterityPower"];
  }

  public PowerVar<DoomPower> Doom => (PowerVar<DoomPower>) this._vars["DoomPower"];

  public EnergyVar Energy => (EnergyVar) this._vars[nameof (Energy)];

  public ExtraDamageVar ExtraDamage => (ExtraDamageVar) this._vars[nameof (ExtraDamage)];

  public ForgeVar Forge => (ForgeVar) this._vars[nameof (Forge)];

  public GoldVar Gold => (GoldVar) this._vars[nameof (Gold)];

  public HealVar Heal => (HealVar) this._vars[nameof (Heal)];

  public HpLossVar HpLoss => (HpLossVar) this._vars[nameof (HpLoss)];

  public MaxHpVar MaxHp => (MaxHpVar) this._vars[nameof (MaxHp)];

  public OstyDamageVar OstyDamage => (OstyDamageVar) this._vars[nameof (OstyDamage)];

  public PowerVar<PoisonPower> Poison => (PowerVar<PoisonPower>) this._vars["PoisonPower"];

  public RepeatVar Repeat => (RepeatVar) this._vars[nameof (Repeat)];

  public StarsVar Stars => (StarsVar) this._vars[nameof (Stars)];

  public PowerVar<StrengthPower> Strength => (PowerVar<StrengthPower>) this._vars["StrengthPower"];

  public SummonVar Summon => (SummonVar) this._vars[nameof (Summon)];

  public PowerVar<VulnerablePower> Vulnerable
  {
    get => (PowerVar<VulnerablePower>) this._vars["VulnerablePower"];
  }

  public PowerVar<WeakPower> Weak => (PowerVar<WeakPower>) this._vars["WeakPower"];

  public DynamicVarSet(IEnumerable<DynamicVar> vars)
  {
    foreach (DynamicVar var in vars)
    {
      if (this._vars.ContainsKey(var.Name))
        throw new ArgumentException(StringHelper.CompactText($"DynamicVarSet contains duplicate key '{var.Name}'. If you're using two of the same variable\ntype (like 2 BlockVars for Halt), please specify a name for each one instead of using the default."));
      this._vars[var.Name] = var;
    }
  }

  public void InitializeWithOwner(AbstractModel model)
  {
    foreach (DynamicVar dynamicVar in this.Values)
      dynamicVar.SetOwner(model);
  }

  public void AddTo(LocString str)
  {
    foreach (KeyValuePair<string, DynamicVar> keyValuePair in this)
      str.Add(keyValuePair.Value);
  }

  public void ClearPreview()
  {
    foreach (DynamicVar dynamicVar in this.Values)
      dynamicVar.ResetToBase();
  }

  public void FinalizeUpgrade()
  {
    foreach (DynamicVar dynamicVar in this.Values)
      dynamicVar.FinalizeUpgrade();
  }

  public void RecalculateForUpgradeOrEnchant()
  {
    foreach (CalculatedVar calculatedVar in this.Values.OfType<CalculatedVar>())
      calculatedVar.RecalculateForUpgradeOrEnchant();
  }

  public DynamicVarSet Clone(AbstractModel model)
  {
    DynamicVarSet dynamicVarSet = new DynamicVarSet(this.Values.Select<DynamicVar, DynamicVar>((Func<DynamicVar, DynamicVar>) (v => v.Clone())));
    dynamicVarSet.InitializeWithOwner(model);
    return dynamicVarSet;
  }
}
