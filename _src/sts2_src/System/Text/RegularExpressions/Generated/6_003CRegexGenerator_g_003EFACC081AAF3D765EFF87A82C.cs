// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
[SkipLocalsInit]
internal sealed class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6 : 
  Regex
{
  internal static readonly \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6 Instance = new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6();

  private \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6()
  {
    this.pattern = "\\s{2,}";
    this.roptions = RegexOptions.None;
    Regex.ValidateMatchTimeout(\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
    this.internalMatchTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
    this.factory = (RegexRunnerFactory) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6.RunnerFactory();
    this.capsize = 1;
  }

  private sealed class RunnerFactory : RegexRunnerFactory
  {
    protected override RegexRunner CreateInstance()
    {
      return (RegexRunner) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__ConsecutiveSpaces_6.RunnerFactory.Runner();
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
        if (runtextpos <= inputSpan.Length - 2)
        {
          ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(runtextpos);
          int num1;
          for (int index = 0; index < readOnlySpan.Length - 1; index = num1 + 1)
          {
            int num2 = MemoryExtensions.IndexOfAny<char>(readOnlySpan.Slice(index), \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_whitespace);
            if (num2 >= 0)
            {
              num1 = index + num2;
              if ((uint) (num1 + 1) < (uint) readOnlySpan.Length)
              {
                if (char.IsWhiteSpace(readOnlySpan[num1 + 1]))
                {
                  this.runtextpos = runtextpos + num1;
                  return true;
                }
              }
              else
                break;
            }
            else
              break;
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
        int num = 0;
        while ((uint) num < (uint) readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[num]))
          ++num;
        if (num < 2)
          return false;
        readOnlySpan.Slice(num);
        int end = runtextpos + num;
        this.runtextpos = end;
        this.Capture(0, start, end);
        return true;
      }
    }
  }
}
