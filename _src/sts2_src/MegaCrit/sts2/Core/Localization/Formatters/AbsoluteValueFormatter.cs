// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.AbsoluteValueFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using SmartFormat.Core.Extensions;
using System;
using System.Globalization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class AbsoluteValueFormatter : IFormatter
{
  public string Name
  {
    get => "abs";
    set => throw new NotImplementedException();
  }

  public bool CanAutoDetect { get; set; }

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    string str1;
    switch (formattingInfo.CurrentValue)
    {
      case Decimal num1:
        str1 = Math.Abs(num1).ToString((IFormatProvider) AbsoluteValueFormatter.Culture);
        break;
      case double num2:
        str1 = Math.Abs(num2).ToString((IFormatProvider) AbsoluteValueFormatter.Culture);
        break;
      case float num3:
        str1 = Math.Abs(num3).ToString((IFormatProvider) AbsoluteValueFormatter.Culture);
        break;
      case int num4:
        str1 = Math.Abs(num4).ToString();
        break;
      case long num5:
        str1 = Math.Abs(num5).ToString();
        break;
      case short num6:
        str1 = Math.Abs(num6).ToString();
        break;
      default:
        str1 = (string) null;
        break;
    }
    string str2 = str1;
    if (str2 == null)
      return false;
    formattingInfo.Write(str2);
    return true;
  }

  private static CultureInfo Culture => LocManager.Instance.CultureInfo;
}
