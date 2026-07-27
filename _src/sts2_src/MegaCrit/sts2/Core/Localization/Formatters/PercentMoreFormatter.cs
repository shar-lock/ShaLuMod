// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.PercentMoreFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using System;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class PercentMoreFormatter : IFormatter
{
  public string Name
  {
    get => "percentMore";
    set => throw new NotImplementedException();
  }

  public bool CanAutoDetect { get; set; }

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    Decimal baseValue;
    if (formattingInfo.CurrentValue is DynamicVar currentValue)
    {
      baseValue = currentValue.BaseValue;
    }
    else
    {
      try
      {
        baseValue = Convert.ToDecimal(formattingInfo.CurrentValue);
      }
      catch (FormatException ex)
      {
        return false;
      }
      catch (InvalidCastException ex)
      {
        return false;
      }
    }
    int int32 = Convert.ToInt32((baseValue - 1M) * 100M);
    formattingInfo.Write(int32.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    return true;
  }
}
