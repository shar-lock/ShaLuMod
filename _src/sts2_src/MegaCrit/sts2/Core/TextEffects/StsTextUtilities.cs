// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.TextEffects.StsTextUtilities
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.TextEffects;

public static class StsTextUtilities
{
  private const string _creamColorCode = "#FFF6E2";
  private const string _buffPopColorCode = "#77ff67";
  private const string _debuffPopColorCode = "#ff6563";
  private const string _normalPopColor = "yellow";

  public static string HighlightChangeText(string text, int baseComparison)
  {
    StringBuilder stringBuilder1 = new StringBuilder(text);
    if (baseComparison == 0)
      return stringBuilder1.ToString();
    string str = baseComparison > 0 ? "green" : "red";
    stringBuilder1.Insert(0, $"[{str}]");
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(3, 1, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[/");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("]");
    ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
    stringBuilder3.Append(ref local);
    return stringBuilder1.ToString();
  }
}
