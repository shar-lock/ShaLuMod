// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.DynamicVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.TextEffects;
using SmartFormat.Core.Extensions;
using System;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class DynamicVar : IConvertible
{
  protected AbstractModel? _owner;
  private Decimal _baseValue;
  private Decimal _enchantedValue;
  private Decimal _previewValue;

  public string Name { get; }

  public Decimal BaseValue
  {
    get => this._baseValue;
    set
    {
      this._baseValue = Math.Min(value, 999999999M);
      this.ResetToBase();
    }
  }

  public Decimal EnchantedValue
  {
    get => this._enchantedValue;
    set => this._enchantedValue = Math.Min(value, 999999999M);
  }

  public Decimal PreviewValue
  {
    get => this._previewValue;
    set => this._previewValue = Math.Min(value, 999999999M);
  }

  public bool WasJustUpgraded { get; protected set; }

  public int IntValue => (int) this.BaseValue;

  public DynamicVar(string name, Decimal baseValue)
  {
    this.Name = name;
    this.BaseValue = baseValue;
    this.ResetToBase();
  }

  public void ResetToBase()
  {
    this.EnchantedValue = this.BaseValue;
    this.PreviewValue = this.BaseValue;
  }

  public virtual void SetOwner(AbstractModel owner) => this._owner = owner;

  public virtual void UpdateCardPreview(
    CardModel card,
    CardPreviewMode previewMode,
    Creature? target,
    bool runGlobalHooks)
  {
  }

  public void UpgradeValueBy(Decimal addend)
  {
    this.BaseValue += addend;
    this.WasJustUpgraded = true;
  }

  public void FinalizeUpgrade() => this.WasJustUpgraded = false;

  public DynamicVar Clone()
  {
    DynamicVar dynamicVar = (DynamicVar) this.MemberwiseClone();
    dynamicVar.ResetToBase();
    return dynamicVar;
  }

  public string ToHighlightedString(bool inverse)
  {
    int previewValue = (int) this.PreviewValue;
    int enchantedValue = (int) this.EnchantedValue;
    int baseComparison = !this.WasJustUpgraded ? (!inverse ? previewValue.CompareTo(enchantedValue) : enchantedValue.CompareTo(previewValue)) : 1;
    return StsTextUtilities.HighlightChangeText(previewValue.ToString((IFormatProvider) CultureInfo.InvariantCulture), baseComparison);
  }

  public override string ToString() => this.IntValue.ToString();

  public object GetSourceValue(ISelectorInfo selector)
  {
    return (object) this.GetBaseValueForIConvertible();
  }

  public TypeCode GetTypeCode() => TypeCode.Object;

  public bool ToBoolean(IFormatProvider? provider)
  {
    throw new InvalidCastException($"Cannot convert {this.BaseValue} to Boolean");
  }

  public byte ToByte(IFormatProvider? provider)
  {
    return Convert.ToByte((object) this.GetBaseValueForIConvertible(), provider);
  }

  public char ToChar(IFormatProvider? provider)
  {
    throw new InvalidCastException($"Cannot convert {this.BaseValue} to Char");
  }

  public DateTime ToDateTime(IFormatProvider? provider)
  {
    throw new InvalidCastException($"Cannot convert {this.BaseValue} to DateTime");
  }

  public Decimal ToDecimal(IFormatProvider? provider) => this.GetBaseValueForIConvertible();

  public double ToDouble(IFormatProvider? provider)
  {
    return Convert.ToDouble((object) this.GetBaseValueForIConvertible(), provider);
  }

  public short ToInt16(IFormatProvider? provider)
  {
    return Convert.ToInt16((object) this.GetBaseValueForIConvertible(), provider);
  }

  public int ToInt32(IFormatProvider? provider)
  {
    return Convert.ToInt32((object) this.GetBaseValueForIConvertible(), provider);
  }

  public long ToInt64(IFormatProvider? provider)
  {
    return Convert.ToInt64((object) this.GetBaseValueForIConvertible(), provider);
  }

  public sbyte ToSByte(IFormatProvider? provider)
  {
    return Convert.ToSByte((object) this.GetBaseValueForIConvertible(), provider);
  }

  public float ToSingle(IFormatProvider? provider)
  {
    return Convert.ToSingle((object) this.GetBaseValueForIConvertible(), provider);
  }

  public string ToString(IFormatProvider? provider)
  {
    return this.GetBaseValueForIConvertible().ToString(provider);
  }

  public object ToType(Type conversionType, IFormatProvider? provider)
  {
    return Convert.ChangeType((object) this.GetBaseValueForIConvertible(), conversionType, provider);
  }

  public ushort ToUInt16(IFormatProvider? provider)
  {
    return Convert.ToUInt16((object) this.GetBaseValueForIConvertible(), provider);
  }

  public uint ToUInt32(IFormatProvider? provider)
  {
    return Convert.ToUInt32((object) this.GetBaseValueForIConvertible(), provider);
  }

  public ulong ToUInt64(IFormatProvider? provider)
  {
    return Convert.ToUInt64((object) this.GetBaseValueForIConvertible(), provider);
  }

  protected virtual Decimal GetBaseValueForIConvertible() => this.BaseValue;
}
