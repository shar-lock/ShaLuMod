// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
[SkipLocalsInit]
internal sealed class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0 : 
  Regex
{
  internal static readonly \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0 Instance = new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0();

  private \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0()
  {
    this.pattern = "([A-Za-z0-9]|\\G(?!^))([A-Z])";
    this.roptions = RegexOptions.None;
    Regex.ValidateMatchTimeout(\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
    this.internalMatchTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
    this.factory = (RegexRunnerFactory) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0.RunnerFactory();
    this.capsize = 3;
  }

  private sealed class RunnerFactory : RegexRunnerFactory
  {
    protected override RegexRunner CreateInstance()
    {
      return (RegexRunner) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__CamelCaseRegex_0.RunnerFactory.Runner();
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
        if ((uint) runtextpos < (uint) inputSpan.Length)
        {
          int num = MemoryExtensions.IndexOfAny<char>(inputSpan.Slice(runtextpos), \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_asciiLettersAndDigits);
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
        int start1 = runtextpos;
        ReadOnlySpan<char> readOnlySpan1 = inputSpan.Slice(runtextpos);
        int start2 = runtextpos;
        int num1 = runtextpos;
        int capturePosition = this.Crawlpos();
        int num2;
        int end1;
        ReadOnlySpan<char> readOnlySpan2;
        if (!readOnlySpan1.IsEmpty && char.IsAsciiLetterOrDigit(readOnlySpan1[0]))
        {
          num2 = 0;
          end1 = runtextpos + 1;
          readOnlySpan2 = inputSpan.Slice(end1);
          goto label_13;
        }
label_2:
        int num3 = num1;
        ReadOnlySpan<char> readOnlySpan3 = inputSpan.Slice(num3);
        UncaptureUntil(capturePosition);
        if (num3 != this.runtextstart)
        {
          UncaptureUntil(0);
          return false;
        }
        int num4 = num3;
        if (\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
          this.CheckTimeout();
        if (num3 == 0)
        {
          UncaptureUntil(0);
          return false;
        }
        end1 = num4;
        readOnlySpan2 = inputSpan.Slice(end1);
        num2 = 1;
label_13:
        this.Capture(1, start2, end1);
        int start3 = end1;
        if (readOnlySpan2.IsEmpty || !char.IsAsciiLetterUpper(readOnlySpan2[0]))
        {
          if (\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
            this.CheckTimeout();
          switch (num2)
          {
            case 0:
              goto label_2;
            case 1:
              UncaptureUntil(0);
              return false;
            default:
              goto label_13;
          }
        }
        else
        {
          int end2 = end1 + 1;
          readOnlySpan3 = inputSpan.Slice(end2);
          this.Capture(2, start3, end2);
          this.runtextpos = end2;
          this.Capture(0, start1, end2);
          return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void UncaptureUntil(int capturePosition)
        {
          while (this.Crawlpos() > capturePosition)
            this.Uncapture();
        }
      }
    }
  }
}
