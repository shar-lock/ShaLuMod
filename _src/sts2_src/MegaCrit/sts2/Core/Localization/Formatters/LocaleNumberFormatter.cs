// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.LocaleNumberFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using SmartFormat.Core.Extensions;
using SmartFormat.Core.Parsing;
using System;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class LocaleNumberFormatter : IFormatter
{
  public string Name
  {
    get => "n";
    set => throw new NotImplementedException();
  }

  public bool CanAutoDetect { get; set; } = true;

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    bool flag;
    switch (formattingInfo.CurrentValue)
    {
      case int _:
      case uint _:
      case long _:
      case ulong _:
      case Decimal _:
      case float _:
      case double _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return false;
    string format = string.IsNullOrEmpty(((FormatItem) formattingInfo.Format)?.RawText) ? "N0" : ((FormatItem) formattingInfo.Format).RawText;
    string str = ((IFormattable) formattingInfo.CurrentValue).ToString(format, (IFormatProvider) LocaleNumberFormatter.Culture);
    formattingInfo.Write(str);
    return true;
  }

  private static CultureInfo Culture => LocManager.Instance.CultureInfo;
}
