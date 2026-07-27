// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.ShowIfUpgradedFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using SmartFormat.Core.Parsing;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class ShowIfUpgradedFormatter : IFormatter
{
  public string Name
  {
    get => "show";
    set => throw new NotSupportedException("Setting the 'Names' property is not supported.");
  }

  public bool CanAutoDetect { get; set; }

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    if (!(formattingInfo.CurrentValue is IfUpgradedVar currentValue))
      return false;
    IList<Format> formatList = formattingInfo.Format?.Split('|');
    if (formatList == null)
      throw new LocException($"Format expression must contain at least 1 option. format={formattingInfo.Format}.");
    Format format1 = formatList.Count <= 2 ? formatList[0] : throw new LocException($"Format expression cannot contain more than 2 options. num_of_options={formatList.Count} format={formattingInfo.Format}.");
    Format format2 = formatList.Count > 1 ? formatList[1] : (Format) null;
    switch (currentValue.upgradeDisplay)
    {
      case UpgradeDisplay.Normal:
        formattingInfo.FormatAsChild(format2, formattingInfo.CurrentValue);
        break;
      case UpgradeDisplay.Upgraded:
        formattingInfo.FormatAsChild(format1, formattingInfo.CurrentValue);
        break;
      case UpgradeDisplay.UpgradePreview:
        formattingInfo.Write("[green]");
        formattingInfo.FormatAsChild(format1, formattingInfo.CurrentValue);
        formattingInfo.Write("[/green]");
        break;
      default:
        throw new ArgumentOutOfRangeException("upgradeDisplay", $"Unexpected value: {currentValue.upgradeDisplay}");
    }
    return true;
  }
}
