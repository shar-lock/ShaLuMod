// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Logging.LogSanitizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

#nullable enable
namespace MegaCrit.Sts2.Core.Logging;

public static class LogSanitizer
{
  private static readonly string _homeReplacement = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "%USERPROFILE%" : "~";

  [GeneratedRegex("\\b76561\\d{12}\\b")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex SteamIdRegex()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4.Instance;
  }

  public static string Sanitize(string text)
  {
    string folderPath = Environment.GetFolderPath((Environment.SpecialFolder) 40);
    if (!string.IsNullOrEmpty(folderPath))
    {
      text = text.Replace(folderPath, LogSanitizer._homeReplacement);
      string oldValue = folderPath.Replace('\\', '/');
      if (oldValue != folderPath)
        text = text.Replace(oldValue, LogSanitizer._homeReplacement);
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    text = LogSanitizer.SteamIdRegex().Replace(text, LogSanitizer.\u003C\u003EO.\u003C0\u003E__ReplaceSteamId ?? (LogSanitizer.\u003C\u003EO.\u003C0\u003E__ReplaceSteamId = new MatchEvaluator(LogSanitizer.ReplaceSteamId)));
    return text;
  }

  public static string ReplaceSteamId(Match m)
  {
    return "A" + IdAnonymizer.Anonymize(ulong.Parse(m.Value)).ToString();
  }
}
