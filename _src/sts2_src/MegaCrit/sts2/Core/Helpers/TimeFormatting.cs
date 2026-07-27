// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.TimeFormatting
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class TimeFormatting
{
  public static string Format(float time)
  {
    TimeSpan timeSpan = TimeSpan.FromSeconds((double) time);
    int totalHours = (int) timeSpan.TotalHours;
    LocString locString;
    if (totalHours > 0)
    {
      locString = new LocString("main_menu_ui", "RUN_TIME_FORMAT_HOURS");
      locString.Add("Hours", totalHours.ToString());
    }
    else
      locString = new LocString("main_menu_ui", "RUN_TIME_FORMAT");
    locString.Add("Seconds", timeSpan.Seconds.ToString("00"));
    locString.Add("Minutes", timeSpan.Minutes.ToString("00"));
    return locString.GetFormattedText();
  }
}
