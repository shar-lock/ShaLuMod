// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Buffers;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
internal static class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities
{
  internal static readonly TimeSpan s_defaultTimeout = !(AppContext.GetData("REGEX_DEFAULT_MATCH_TIMEOUT") is TimeSpan data) ? Regex.InfiniteMatchTimeout : data;
  internal static readonly bool s_hasTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout != Regex.InfiniteMatchTimeout;
  internal static readonly SearchValues<char> s_asciiLettersAndDigits = SearchValues.Create(string.op_Implicit("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"));
  internal static readonly SearchValues<char> s_ascii_FF03FEFFFF8700000000 = SearchValues.Create(string.op_Implicit("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ_"));
  internal static readonly SearchValues<string> s_indexOfString_76561_Ordinal;
  internal static readonly SearchValues<char> s_whitespace;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsBoundary(ReadOnlySpan<char> inputSpan, int index)
  {
    int num = index - 1;
    return ((uint) num >= (uint) inputSpan.Length ? 0 : (IsBoundaryWordChar(inputSpan[num]) ? 1 : 0)) != ((uint) index >= (uint) inputSpan.Length ? 0 : (IsBoundaryWordChar(inputSpan[index]) ? 1 : 0));

    static bool IsBoundaryWordChar(char ch)
    {
      return \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.IsWordChar(ch) || ch == '\u200C' | ch == '\u200D';
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static unsafe bool IsWordChar(char ch)
  {
    // ISSUE: reference to a compiler-generated field
    ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.\u0033C17DA885F7916F14BECDDD5A559E146795A03758C28EA455EC8E9E803B90531, 16 /*0x10*/);
    int num = (int) ch >> 3;
    return (uint) num >= (uint) readOnlySpan.Length ? (262463 & 1 << CharUnicodeInfo.GetUnicodeCategory(ch)) != 0 : ((uint) readOnlySpan[num] & (uint) (1 << ((int) ch & 7))) > 0U;
  }

  static \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities()
  {
    string str = "76561";
    \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_indexOfString_76561_Ordinal = SearchValues.Create(new ReadOnlySpan<string>(ref str), StringComparison.Ordinal);
    \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_whitespace = SearchValues.Create(string.op_Implicit("\t\n\v\f\r \u0085             \u2028\u2029  　"));
  }
}
