// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
[SkipLocalsInit]
internal sealed class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3 : 
  Regex
{
  internal static readonly \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3 Instance = new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3();

  private \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3()
  {
    this.pattern = "[^A-Z0-9_]";
    this.roptions = RegexOptions.None;
    Regex.ValidateMatchTimeout(\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
    this.internalMatchTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
    this.factory = (RegexRunnerFactory) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3.RunnerFactory();
    this.capsize = 1;
  }

  private sealed class RunnerFactory : RegexRunnerFactory
  {
    protected override RegexRunner CreateInstance()
    {
      return (RegexRunner) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SpecialCharRegex_3.RunnerFactory.Runner();
    }

    private sealed class Runner : RegexRunner
    {
      protected override void Scan(ReadOnlySpan<char> inputSpan)
      {
        if (!this.TryFindNextPossibleStartingPosition(inputSpan))
          return;
        int runtextpos = this.runtextpos;
        int end = this.runtextpos = runtextpos + 1;
        this.Capture(0, runtextpos, end);
      }

      private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
      {
        int runtextpos = this.runtextpos;
        if ((uint) runtextpos < (uint) inputSpan.Length)
        {
          int num = MemoryExtensions.IndexOfAnyExcept<char>(inputSpan.Slice(runtextpos), \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_ascii_FF03FEFFFF8700000000);
          if (num >= 0)
          {
            this.runtextpos = runtextpos + num;
            return true;
          }
        }
        this.runtextpos = inputSpan.Length;
        return false;
      }
    }
  }
}
