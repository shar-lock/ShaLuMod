// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Timeline.EpochComparer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Timeline;

public class EpochComparer : IComparer<SerializableEpoch>
{
  public int Compare(SerializableEpoch? x, SerializableEpoch? y)
  {
    EpochModel epochModel1 = EpochModel.Get(x.Id);
    EpochModel epochModel2 = EpochModel.Get(y.Id);
    int num = epochModel1.Era.CompareTo((object) epochModel2.Era);
    return num != 0 ? num : epochModel1.EraPosition.CompareTo(epochModel2.EraPosition);
  }
}
