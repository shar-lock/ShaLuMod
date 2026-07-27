// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.HighlightDifferencesFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class HighlightDifferencesFormatter : IFormatter
{
  public string Name
  {
    get => "diff";
    set => throw new NotImplementedException();
  }

  public bool CanAutoDetect { get; set; }

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    if (!(formattingInfo.CurrentValue is DynamicVar currentValue))
      return false;
    formattingInfo.Write(currentValue.ToHighlightedString(false));
    return true;
  }
}
