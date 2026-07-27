// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogueSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Ancients;

public class AncientDialogueSet
{
  public required AncientDialogue? FirstVisitEverDialogue { get; init; }

  public required Dictionary<string, IReadOnlyList<AncientDialogue>> CharacterDialogues { get; init; }

  public required IReadOnlyList<AncientDialogue> AgnosticDialogues { get; init; } = (IReadOnlyList<AncientDialogue>) Array.Empty<AncientDialogue>();

  public IEnumerable<AncientDialogue> GetAllDialogues()
  {
    if (this.FirstVisitEverDialogue != null)
      yield return this.FirstVisitEverDialogue;
    foreach (IEnumerable<AncientDialogue> ancientDialogues in this.CharacterDialogues.Values)
    {
      IEnumerator<AncientDialogue> enumerator = ancientDialogues.GetEnumerator();
      while (enumerator.MoveNext())
        yield return enumerator.Current;
      enumerator = (IEnumerator<AncientDialogue>) null;
    }
    foreach (AncientDialogue agnosticDialogue in (IEnumerable<AncientDialogue>) this.AgnosticDialogues)
      yield return agnosticDialogue;
  }

  public IEnumerable<AncientDialogue> GetValidDialogues(
    ModelId characterId,
    int charVisits,
    int totalVisits,
    bool allowAnyCharacterDialogues)
  {
    if (totalVisits == 0 && this.FirstVisitEverDialogue != null)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<AncientDialogue>) new \u003C\u003Ez__ReadOnlySingleElementList<AncientDialogue>(this.FirstVisitEverDialogue);
    }
    IReadOnlyList<AncientDialogue> source = (IReadOnlyList<AncientDialogue>) null;
    IReadOnlyList<AncientDialogue> ancientDialogueList;
    if (this.CharacterDialogues.TryGetValue(characterId.Entry, out ancientDialogueList))
    {
      source = ancientDialogueList;
      List<AncientDialogue> list = source.Where<AncientDialogue>((Func<AncientDialogue, bool>) (d =>
      {
        int? visitIndex = d.VisitIndex;
        int num = charVisits;
        return visitIndex.GetValueOrDefault() == num & visitIndex.HasValue;
      })).ToList<AncientDialogue>();
      if (list.Count > 0)
        return (IEnumerable<AncientDialogue>) list;
    }
    if (allowAnyCharacterDialogues)
    {
      List<AncientDialogue> list = this.AgnosticDialogues.Where<AncientDialogue>((Func<AncientDialogue, bool>) (d =>
      {
        int? visitIndex = d.VisitIndex;
        int num = charVisits;
        return visitIndex.GetValueOrDefault() == num & visitIndex.HasValue;
      })).ToList<AncientDialogue>();
      if (list.Count > 0)
        return (IEnumerable<AncientDialogue>) list;
    }
    List<AncientDialogue> destination = new List<AncientDialogue>();
    if (source != null)
      AncientDialogueSet.AddRepeatingDialogues((IEnumerable<AncientDialogue>) source, destination, charVisits);
    if (allowAnyCharacterDialogues)
      AncientDialogueSet.AddRepeatingDialogues((IEnumerable<AncientDialogue>) this.AgnosticDialogues, destination, charVisits);
    return (IEnumerable<AncientDialogue>) destination;
  }

  public void PopulateLocKeys(string ancientEntry)
  {
    this.FirstVisitEverDialogue?.PopulateLines(ancientEntry, "firstVisitEver", 0);
    foreach (KeyValuePair<string, IReadOnlyList<AncientDialogue>> characterDialogue in this.CharacterDialogues)
    {
      string str;
      IReadOnlyList<AncientDialogue> ancientDialogueList1;
      characterDialogue.Deconstruct(ref str, ref ancientDialogueList1);
      string charEntry = str;
      IReadOnlyList<AncientDialogue> ancientDialogueList2 = ancientDialogueList1;
      for (int index = 0; index < ancientDialogueList2.Count; ++index)
        ancientDialogueList2[index].PopulateLines(ancientEntry, charEntry, index);
    }
    for (int index = 0; index < this.AgnosticDialogues.Count; ++index)
      this.AgnosticDialogues[index].PopulateLines(ancientEntry, "ANY", index);
    foreach (AncientDialogue allDialogue in this.GetAllDialogues())
    {
      for (int index = 0; index < allDialogue.Lines.Count - 1; ++index)
      {
        AncientDialogueLine line = allDialogue.Lines[index];
        string locEntryKey1 = line.LineText.LocEntryKey;
        string locEntryKey2 = locEntryKey1.Substring(0, locEntryKey1.LastIndexOf('.')) + ".next";
        line.NextButtonText = new LocString("ancients", locEntryKey2);
      }
    }
  }

  private static void AddRepeatingDialogues(
    IEnumerable<AncientDialogue> source,
    List<AncientDialogue> destination,
    int charVisits)
  {
    foreach (AncientDialogue ancientDialogue in source)
    {
      if (ancientDialogue.IsRepeating)
      {
        int? visitIndex = ancientDialogue.VisitIndex;
        if (visitIndex.HasValue)
        {
          int num = charVisits;
          visitIndex = ancientDialogue.VisitIndex;
          int valueOrDefault = visitIndex.GetValueOrDefault();
          if (num < valueOrDefault & visitIndex.HasValue)
            continue;
        }
        destination.Add(ancientDialogue);
      }
    }
  }
}
