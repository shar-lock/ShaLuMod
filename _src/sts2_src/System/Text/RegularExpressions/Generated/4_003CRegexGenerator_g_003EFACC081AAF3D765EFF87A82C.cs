// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
[SkipLocalsInit]
internal sealed class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4 : 
  Regex
{
  internal static readonly \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4 Instance = new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4();

  private \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4()
  {
    this.pattern = "\\b76561\\d{12}\\b";
    this.roptions = RegexOptions.None;
    Regex.ValidateMatchTimeout(\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
    this.internalMatchTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
    this.factory = (RegexRunnerFactory) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4.RunnerFactory();
    this.capsize = 1;
  }

  private sealed class RunnerFactory : RegexRunnerFactory
  {
    protected override RegexRunner CreateInstance()
    {
      return (RegexRunner) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SteamIdRegex_4.RunnerFactory.Runner();
    }

    private sealed class Runner : RegexRunner
    {
      protected override void Scan(ReadOnlySpan<char> inputSpan)
      {
        while (this.TryFindNextPossibleStartingPosition(inputSpan) && !this.TryMatchAtCurrentPosition(inputSpan) && this.runtextpos != inputSpan.Length)
        {
          ++this.runtextpos;
          if (\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
            this.CheckTimeout();
        }
      }

      private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
      {
        int runtextpos = this.runtextpos;
        if (runtextpos <= inputSpan.Length - 17)
        {
          int num = MemoryExtensions.IndexOfAny(inputSpan.Slice(runtextpos), \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_indexOfString_76561_Ordinal);
          if (num >= 0)
          {
            this.runtextpos = runtextpos + num;
            return true;
          }
        }
        this.runtextpos = inputSpan.Length;
        return false;
      }

      private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
      {
        int runtextpos = this.runtextpos;
        int start = runtextpos;
        ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(runtextpos);
        if (!\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.IsBoundary(inputSpan, runtextpos) || (uint) readOnlySpan.Length < 17U || !MemoryExtensions.StartsWith(readOnlySpan, string.op_Implicit("76561"), StringComparison.OrdinalIgnoreCase) || !char.IsDigit(readOnlySpan[5]) || !char.IsDigit(readOnlySpan[6]) || !char.IsDigit(readOnlySpan[7]) || !char.IsDigit(readOnlySpan[8]) || !char.IsDigit(readOnlySpan[9]) || !char.IsDigit(readOnlySpan[10]) || !char.IsDigit(readOnlySpan[11]) || !char.IsDigit(readOnlySpan[12]) || !char.IsDigit(readOnlySpan[13]) || !char.IsDigit(readOnlySpan[14]) || !char.IsDigit(readOnlySpan[15]) || !char.IsDigit(readOnlySpan[16 /*0x10*/]) || !\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.IsBoundary(inputSpan, runtextpos + 17))
          return false;
        int end = runtextpos + 17;
        this.runtextpos = end;
        this.Capture(0, start, end);
        return true;
      }
    }
  }
}
