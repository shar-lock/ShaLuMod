// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
[SkipLocalsInit]
internal sealed class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7 : 
  Regex
{
  internal static readonly \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7 Instance = new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7();

  private \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7()
  {
    this.pattern = "<.*?>";
    this.roptions = RegexOptions.None;
    Regex.ValidateMatchTimeout(\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
    this.internalMatchTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
    this.factory = (RegexRunnerFactory) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7.RunnerFactory();
    this.capsize = 1;
  }

  private sealed class RunnerFactory : RegexRunnerFactory
  {
    protected override RegexRunner CreateInstance()
    {
      return (RegexRunner) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__HtmlTags_7.RunnerFactory.Runner();
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
          int num = MemoryExtensions.IndexOf<char>(inputSpan.Slice(runtextpos), '<');
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
        ReadOnlySpan<char> readOnlySpan1 = inputSpan.Slice(runtextpos);
        if (readOnlySpan1.IsEmpty || readOnlySpan1[0] != '<')
          return false;
        int num1 = runtextpos + 1;
        ReadOnlySpan<char> readOnlySpan2 = inputSpan.Slice(num1);
        int num2 = num1;
        while (readOnlySpan2.IsEmpty || readOnlySpan2[0] != '>')
        {
          if (\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
            this.CheckTimeout();
          int num3 = num2;
          readOnlySpan2 = inputSpan.Slice(num3);
          if (readOnlySpan2.IsEmpty || readOnlySpan2[0] == '\n')
            return false;
          int num4 = num3 + 1;
          readOnlySpan2 = inputSpan.Slice(num4);
          int num5 = MemoryExtensions.IndexOfAny<char>(readOnlySpan2, '\n', '>');
          if ((uint) num5 >= (uint) readOnlySpan2.Length || readOnlySpan2[num5] == '\n')
            return false;
          num1 = num4 + num5;
          readOnlySpan2 = inputSpan.Slice(num1);
          num2 = num1;
        }
        int end = num1 + 1;
        this.runtextpos = end;
        this.Capture(0, start, end);
        return true;
      }
    }
  }
}
