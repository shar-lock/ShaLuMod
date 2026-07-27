// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.MultiplayerDebugUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer;

public static class MultiplayerDebugUtil
{
  private static IEnumerable<BigInteger> Chunk(IReadOnlyList<byte> source, int size)
  {
    return source.Select<byte, byte[]>((Func<byte, int, byte[]>) ((_, index) => source.Skip<byte>(size * index).Take<byte>(size).ToArray<byte>())).TakeWhile<byte[]>((Func<byte[], bool>) (bucket => bucket.Length != 0)).Select<byte[], BigInteger>((Func<byte[], BigInteger>) (e => new BigInteger(e)));
  }

  private static byte ReplaceControlCharacterWithDot(byte character)
  {
    return character >= (byte) 31 /*0x1F*/ && character < (byte) 127 /*0x7F*/ ? character : (byte) 46;
  }

  private static byte[] ReplaceControlCharactersWithDots(byte[] characters)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    return ((IEnumerable<byte>) characters).Select<byte, byte>(MultiplayerDebugUtil.\u003C\u003EO.\u003C0\u003E__ReplaceControlCharacterWithDot ?? (MultiplayerDebugUtil.\u003C\u003EO.\u003C0\u003E__ReplaceControlCharacterWithDot = new Func<byte, byte>(MultiplayerDebugUtil.ReplaceControlCharacterWithDot))).ToArray<byte>();
  }

  public static string FormatAsHex(ReadOnlySpan<byte> data, int lineWidth = 16 /*0x10*/, int byteWidth = 1)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    byte[] array;
    for (int index = 0; index < data.Length; index += array.Length)
    {
      array = data.Slice(index, Math.Min(lineWidth * byteWidth, data.Length - index)).ToArray();
      string str1 = string.Join(" ", MultiplayerDebugUtil.Chunk((IReadOnlyList<byte>) array, byteWidth).Select<BigInteger, string>((Func<BigInteger, string>) (v => v.ToString("X" + (byteWidth * 2).ToString(), (IFormatProvider) CultureInfo.InvariantCulture))));
      string str2 = str1 + new string(' ', lineWidth * (byteWidth * 2 + 1) - 1 - str1.Length);
      string str3 = Encoding.ASCII.GetString(MultiplayerDebugUtil.ReplaceControlCharactersWithDots(array));
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(3, 3, stringBuilder2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(index, "X4");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str3);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.Append(ref local);
    }
    return stringBuilder1.ToString();
  }

  public static byte[] StringToByteArray(string hex)
  {
    byte[] byteArray = hex.Length % 2 != 1 ? new byte[hex.Length >> 1] : throw new Exception("The binary key cannot have an odd number of digits");
    for (int index = 0; index < hex.Length >> 1; ++index)
      byteArray[index] = (byte) ((MultiplayerDebugUtil.GetHexVal(hex[index << 1]) << 4) + MultiplayerDebugUtil.GetHexVal(hex[(index << 1) + 1]));
    return byteArray;
  }

  private static int GetHexVal(char hex)
  {
    int num = (int) hex;
    return num - (num < 58 ? 48 /*0x30*/ : (num < 97 ? 55 : 87));
  }
}
