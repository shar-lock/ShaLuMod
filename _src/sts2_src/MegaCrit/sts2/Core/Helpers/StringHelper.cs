// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.StringHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO.Hashing;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class StringHelper
{
  [ThreadStatic]
  private static byte[]? _stringHashCache;

  [GeneratedRegex("([A-Za-z0-9]|\\G(?!^))([A-Z])")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex CamelCaseRegex()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0.Instance;
  }

  [GeneratedRegex("(.*?)_([a-zA-Z0-9])")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex SnakeCaseRegex()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1.Instance;
  }

  [GeneratedRegex("\\s+")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex WhitespaceRegex()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__WhitespaceRegex_2.Instance;
  }

  [GeneratedRegex("[^A-Z0-9_]")]
  [GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
  private static Regex SpecialCharRegex()
  {
    return (Regex) \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3.Instance;
  }

  public static string SnakeCase(string txt)
  {
    return StringHelper.CamelCaseRegex().Replace(txt.Trim(), "$1_$2").ToLowerInvariant();
  }

  public static string Slugify(string txt)
  {
    string str = StringHelper.CamelCaseRegex().Replace(txt.Trim(), "$1_$2");
    string input = StringHelper.WhitespaceRegex().Replace(str.ToUpperInvariant(), "_");
    return StringHelper.SpecialCharRegex().Replace(input, "");
  }

  public static string Unslugify(string txt)
  {
    string str1 = StringHelper.SnakeCaseRegex().Replace(txt.Trim().ToLowerInvariant(), (MatchEvaluator) (match => match.Groups[1].ToString() + match.Groups[2].ToString().ToUpperInvariant()));
    char upperInvariant = char.ToUpperInvariant(str1[0]);
    ReadOnlySpan<char> readOnlySpan1 = new ReadOnlySpan<char>(ref upperInvariant);
    string str2 = str1;
    ReadOnlySpan<char> readOnlySpan2 = string.op_Implicit(str2.Substring(1, str2.Length - 1));
    return readOnlySpan1.ToString() + readOnlySpan2;
  }

  public static string CompactText(string text) => text.Trim();

  public static ulong GetDeterministicHashCode(string str)
  {
    if (StringHelper._stringHashCache == null)
      StringHelper._stringHashCache = new byte[1024 /*0x0400*/];
    int byteCount = Encoding.UTF8.GetByteCount(str);
    if (byteCount > StringHelper._stringHashCache.Length)
      StringHelper._stringHashCache = new byte[(int) Math.Round((double) byteCount * 1.5)];
    int bytes = Encoding.UTF8.GetBytes(string.op_Implicit(str), Span<byte>.op_Implicit(StringHelper._stringHashCache));
    return XxHash64.HashToUInt64(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(StringHelper._stringHashCache).Slice(0, bytes)), 0L);
  }

  public static int GetDeterministicHashCodeOld(string str)
  {
    int num1 = 352654597 /*0x15051505*/;
    int num2 = num1;
    for (int index = 0; index < str.Length; index += 2)
    {
      num1 = (num1 << 5) + num1 ^ (int) str[index];
      if (index != str.Length - 1)
        num2 = (num2 << 5) + num2 ^ (int) str[index + 1];
      else
        break;
    }
    return num1 + num2 * 1566083941;
  }

  public static string Radix(int value)
  {
    string language = SaveManager.Instance.SettingsSave.Language;
    if (language != null && language.Length == 3)
    {
      switch (language[0])
      {
        case 'b':
          if (language == "ben")
            goto label_19;
          goto label_20;
        case 'c':
          if (language == "cze")
            goto label_18;
          goto label_20;
        case 'd':
          if (language == "deu" || language == "dut")
            break;
          goto label_20;
        case 'f':
          if (language == "fin" || language == "fra")
            goto label_18;
          goto label_20;
        case 'g':
          if (language == "gre")
            break;
          goto label_20;
        case 'h':
          if (language == "hin")
            goto label_19;
          goto label_20;
        case 'i':
          if (language == "ind" || language == "ita")
            break;
          goto label_20;
        case 'm':
          if (language == "mal")
            break;
          goto label_20;
        case 'n':
          if (language == "nor")
            break;
          goto label_20;
        case 'p':
          switch (language)
          {
            case "por":
            case "ptb":
              break;
            case "pol":
              goto label_18;
            default:
              goto label_20;
          }
          break;
        case 'r':
          if (language == "rus")
            goto label_18;
          goto label_20;
        case 's':
          switch (language)
          {
            case "spa":
              break;
            case "swe":
              goto label_18;
            default:
              goto label_20;
          }
          break;
        case 't':
          if (language == "tur")
            break;
          goto label_20;
        case 'u':
          if (language == "ukr")
            goto label_18;
          goto label_20;
        case 'v':
          if (language == "vie")
            break;
          goto label_20;
        default:
          goto label_20;
      }
      return value.ToString("N0", (IFormatProvider) new CultureInfo("es-ES"));
label_18:
      return value.ToString("N0", (IFormatProvider) new CultureInfo("fr-FR"));
label_19:
      return value.ToString("N0", (IFormatProvider) new CultureInfo("hi-IN"));
    }
label_20:
    return value.ToString("N0", (IFormatProvider) new CultureInfo("en-US"));
  }

  public static LocString RatioFormat(int numerator, int denominator)
  {
    return StringHelper.RatioFormat(numerator.ToString(), denominator.ToString());
  }

  public static LocString RatioFormat(string numerator, string denominator)
  {
    LocString locString = new LocString("stats_screen", "RATIO_FORMAT");
    locString.Add("Numerator", numerator);
    locString.Add("Denominator", denominator);
    return locString;
  }

  public static string Capitalize(string input)
  {
    char upperInvariant = char.ToUpperInvariant(input[0]);
    ReadOnlySpan<char> readOnlySpan1 = new ReadOnlySpan<char>(ref upperInvariant);
    string str = input;
    ReadOnlySpan<char> readOnlySpan2 = string.op_Implicit(str.Substring(1, str.Length - 1));
    return readOnlySpan1.ToString() + readOnlySpan2;
  }

  public static string StripBbCode(this string text) => Regex.Replace(text, "\\[(.*?)\\]", "");

  public static string EscapeBbcodeTags(this string text) => text.Replace("[", "[lb]");
}
