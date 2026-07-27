// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocValidator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using SmartFormat.Core.Parsing;
using SmartFormat.Core.Settings;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public static class LocValidator
{
  public static bool ValidateFormatString(string text, out string? errorMessage)
  {
    try
    {
      new Parser((SmartSettings) null).ParseFormat(text);
      errorMessage = (string) null;
      return true;
    }
    catch (ParsingErrors ex)
    {
      errorMessage = ((Exception) ex).Message;
      return false;
    }
  }
}
