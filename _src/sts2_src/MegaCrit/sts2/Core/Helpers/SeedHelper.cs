// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.SeedHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class SeedHelper
{
  private const string _characters = "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
  public const int seedDefaultLength = 12;

  public static string GetRandomSeed(Rng? rng = null, int length = 12)
  {
    if (rng == null)
      rng = Rng.Chaotic;
    string text;
    do
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < length; ++index)
        stringBuilder.Append(rng.NextItem<char>((IEnumerable<char>) "0123456789ABCDEFGHJKLMNPQRSTUVWXYZ"));
      text = stringBuilder.ToString();
    }
    while (BadWordChecker.ContainsBadWord(text));
    return text;
  }

  public static string CanonicalizeSeed(string seed)
  {
    seed = seed.ToUpperInvariant();
    seed = seed.Replace('O', '0');
    seed = seed.Replace('I', '1');
    seed = seed.Trim();
    return seed;
  }
}
