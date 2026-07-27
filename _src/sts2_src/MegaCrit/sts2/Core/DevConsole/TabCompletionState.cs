// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.TabCompletionState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public class TabCompletionState
{
  public int SelectionIndex { get; set; } = -1;

  public List<string> CompletionCandidates { get; } = new List<string>();

  public bool InSelectionMode { get; set; }

  public bool ProgrammaticTextChange { get; set; }

  public CompletionResult? LastCompletionResult { get; set; }

  public void Reset()
  {
    this.InSelectionMode = false;
    this.SelectionIndex = -1;
    this.CompletionCandidates.Clear();
    this.LastCompletionResult = (CompletionResult) null;
  }
}
