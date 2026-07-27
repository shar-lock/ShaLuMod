// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Ancients;

public class AncientDialogue
{
  private const string _locTable = "ancients";

  public IReadOnlyList<AncientDialogueLine> Lines { get; }

  public bool IsRepeating { get; set; }

  public int? VisitIndex { get; init; }

  public ArchitectAttackers StartAttackers { get; init; }

  public ArchitectAttackers EndAttackers { get; init; }

  public AncientDialogue(params string[] sfxPaths)
  {
    this.Lines = sfxPaths.Length != 0 ? (IReadOnlyList<AncientDialogueLine>) ((IEnumerable<string>) sfxPaths).Select<string, AncientDialogueLine>((Func<string, AncientDialogueLine>) (sfx => new AncientDialogueLine(sfx))).ToList<AncientDialogueLine>() : throw new ArgumentException("Requires at least 1 SFX path", nameof (sfxPaths));
  }

  public void PopulateLines(string ancientEntry, string charEntry, int dialogueIndex)
  {
    bool flag1 = AncientDialogue.HasRepeatingSuffix($"{ancientEntry}.talk.{charEntry}.{dialogueIndex}-0");
    if (flag1)
      this.IsRepeating = true;
    string str1 = flag1 ? "r" : "";
    for (int index = 0; index < this.Lines.Count; ++index)
    {
      string baseKey = $"{ancientEntry}.talk.{charEntry}.{dialogueIndex}-{index}";
      bool flag2 = AncientDialogue.HasRepeatingSuffix(baseKey);
      if (flag1 && !flag2)
        throw new InvalidOperationException($"Dialogue {ancientEntry}.talk.{charEntry}.{dialogueIndex}: line 0 has 'r' suffix but line {index} does not.");
      if (!flag1 & flag2)
        throw new InvalidOperationException($"Dialogue {ancientEntry}.talk.{charEntry}.{dialogueIndex}: line 0 has no 'r' suffix but line {index} does.");
      string str2 = baseKey + str1;
      string str3 = str2 + ".ancient";
      string locEntryKey = str2 + ".char";
      if (LocString.Exists("ancients", str3))
      {
        this.Lines[index].LineText = new LocString("ancients", str3);
        this.Lines[index].Speaker = AncientDialogueSpeaker.Ancient;
      }
      else
      {
        this.Lines[index].LineText = new LocString("ancients", locEntryKey);
        this.Lines[index].Speaker = AncientDialogueSpeaker.Character;
      }
    }
  }

  private static bool HasRepeatingSuffix(string baseKey)
  {
    return LocString.Exists("ancients", baseKey + "r.ancient") || LocString.Exists("ancients", baseKey + "r.char");
  }
}
