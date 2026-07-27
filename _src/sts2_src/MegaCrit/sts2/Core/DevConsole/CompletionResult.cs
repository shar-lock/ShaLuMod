// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.CompletionResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public class CompletionResult
{
  public List<string> Candidates { get; set; } = new List<string>();

  public string CommonPrefix { get; set; } = string.Empty;

  public bool HasMultipleMatches => this.Candidates.Count > 1;

  public CompletionType Type { get; set; }

  public string CommandPrefix { get; set; } = "";

  public int ArgumentIndex { get; set; } = -1;

  public string ArgumentContext { get; set; } = "";
}
