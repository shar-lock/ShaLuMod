// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.CommandLineHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Collections;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class CommandLineHelper
{
  private static readonly Dictionary<string, string?> _args = new Dictionary<string, string>();

  static CommandLineHelper()
  {
    string[] cmdlineArgs = OS.GetCmdlineArgs();
    for (int index = 0; index < cmdlineArgs.Length; ++index)
    {
      string str1 = cmdlineArgs[index].TrimStart('-');
      string str2 = str1;
      string str3 = (string) null;
      int length = str1.IndexOf('=');
      if (length > 0)
      {
        str2 = str1.Substring(0, length);
        str3 = str1.Substring(length + 1);
      }
      else if (index + 1 < cmdlineArgs.Length && !cmdlineArgs[index + 1].StartsWith('-') && !cmdlineArgs[index + 1].StartsWith('+'))
      {
        str3 = cmdlineArgs[index + 1];
        ++index;
      }
      CommandLineHelper._args[str2] = str3;
    }
  }

  public static bool HasArg(string key) => CommandLineHelper._args.ContainsKey(key);

  public static bool TryGetValue(string key, out string? value)
  {
    return CommandLineHelper._args.TryGetValue(key, ref value);
  }

  public static string? GetValue(string key)
  {
    string str;
    return !CommandLineHelper.TryGetValue(key, out str) ? (string) null : str;
  }
}
