// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.EnergyIconsFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using SmartFormat.Core.Extensions;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class EnergyIconsFormatter : IFormatter
{
  private const string _fallbackIconPrefix = "colorless";

  public string Name
  {
    get => "energyIcons";
    set => throw new NotImplementedException();
  }

  public bool CanAutoDetect { get; set; }

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    string str1 = (string) null;
    int result;
    switch (formattingInfo.CurrentValue)
    {
      case EnergyVar energyVar:
        result = Convert.ToInt32(energyVar.PreviewValue);
        if (!string.IsNullOrEmpty(energyVar.ColorPrefix))
        {
          str1 = energyVar.ColorPrefix;
          break;
        }
        break;
      case CalculatedVar calculatedVar:
        result = Convert.ToInt32(calculatedVar.Calculate((Creature) null));
        break;
      case Decimal num1:
        result = (int) num1;
        break;
      case int num2:
        result = num2;
        break;
      case string str2:
        if (!int.TryParse(formattingInfo.FormatterOptions, out result))
          return false;
        str1 = str2;
        break;
      default:
        throw new LocException($"Unknown value='{formattingInfo.CurrentValue}' type={formattingInfo.CurrentValue?.GetType()}");
    }
    if (string.IsNullOrEmpty(str1) || str1 == "colorless")
      str1 = RunManager.Instance.GetLocalCharacterEnergyIconPrefix();
    if (str1 == null)
    {
      Log.Warn("No energy prefix found for EnergyIconsFormatter! Using colorless as a fallback.");
      if (str1 == null)
        str1 = "colorless";
    }
    string element = $"[img]res://images/packed/sprite_fonts/{str1}_energy_icon.png[/img]";
    string str3;
    if (result > 0 && result < 4)
      str3 = string.Concat(Enumerable.Repeat<string>(element, result));
    else if (formattingInfo.CurrentValue is DynamicVar currentValue)
      str3 = currentValue.ToHighlightedString(false) + element;
    else
      str3 = $"{result}{element}";
    formattingInfo.Write(str3);
    return true;
  }
}
