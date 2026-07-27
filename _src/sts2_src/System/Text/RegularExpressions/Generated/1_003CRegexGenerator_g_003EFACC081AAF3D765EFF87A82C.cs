// Decompiled with JetBrains decompiler
// Type: System.Text.RegularExpressions.Generated.<RegexGenerator_g>FACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "9.0.12.31616")]
[SkipLocalsInit]
internal sealed class \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1 : 
  Regex
{
  internal static readonly \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1 Instance = new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1();

  private \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1()
  {
    this.pattern = "(.*?)_([a-zA-Z0-9])";
    this.roptions = RegexOptions.None;
    Regex.ValidateMatchTimeout(\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout);
    this.internalMatchTimeout = \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_defaultTimeout;
    this.factory = (RegexRunnerFactory) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1.RunnerFactory();
    this.capsize = 3;
  }

  private sealed class RunnerFactory : RegexRunnerFactory
  {
    protected override RegexRunner CreateInstance()
    {
      return (RegexRunner) new \u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__SnakeCaseRegex_1.RunnerFactory.Runner();
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
          for (int index = 0; index < readOnlySpan.Length; ++index)
          {
            if (readOnlySpan[index] != '\n')
            {
              this.runtextpos = runtextpos + index;
              return true;
            }
          }
        }
        this.runtextpos = inputSpan.Length;
        return false;
      }

      private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
      {
        int end1 = this.runtextpos;
        int start1 = end1;
        ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(end1);
        int start2 = end1;
        int num1 = end1;
        int num2;
        int start3;
        while (true)
        {
          int capturePosition = this.Crawlpos();
          this.Capture(1, start2, end1);
          if (!readOnlySpan.IsEmpty && readOnlySpan[0] == '_')
            goto label_9;
label_1:
          UncaptureUntil(capturePosition);
          if (\u003CRegexGenerator_g\u003EFACC081AAF3D765EFF87A82C4FBB77F6FD3EA759AA2D03D993988F88E97CC0B5B__Utilities.s_hasTimeout)
            this.CheckTimeout();
          int num3 = num1;
          readOnlySpan = inputSpan.Slice(num3);
          if (!readOnlySpan.IsEmpty && readOnlySpan[0] != '\n')
          {
            int num4 = num3 + 1;
            readOnlySpan = inputSpan.Slice(num4);
            int num5 = MemoryExtensions.IndexOfAny<char>(readOnlySpan, '\n', '_');
            if ((uint) num5 < (uint) readOnlySpan.Length && readOnlySpan[num5] != '\n')
            {
              end1 = num4 + num5;
              readOnlySpan = inputSpan.Slice(end1);
              num1 = end1;
              continue;
            }
            goto label_6;
          }
          break;
label_9:
          num2 = end1 + 1;
          readOnlySpan = inputSpan.Slice(num2);
          start3 = num2;
          if (readOnlySpan.IsEmpty || !char.IsAsciiLetterOrDigit(readOnlySpan[0]))
            goto label_1;
          goto label_10;
        }
        UncaptureUntil(0);
        return false;
label_6:
        UncaptureUntil(0);
        return false;
label_10:
        int end2 = num2 + 1;
        readOnlySpan = inputSpan.Slice(end2);
        this.Capture(2, start3, end2);
        this.runtextpos = end2;
        this.Capture(0, start1, end2);
        return true;

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
