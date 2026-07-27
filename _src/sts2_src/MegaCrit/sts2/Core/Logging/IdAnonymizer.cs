// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Logging.IdAnonymizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Logging;

public static class IdAnonymizer
{
  private static readonly Dictionary<ulong, ulong> _idToAnonymized = new Dictionary<ulong, ulong>();

  public static ulong Anonymize(ulong id)
  {
    ulong num;
    if (!IdAnonymizer._idToAnonymized.TryGetValue(id, out num))
    {
      num = (ulong) Rng.Chaotic.NextUnsignedInt();
      IdAnonymizer._idToAnonymized[id] = num;
    }
    return num;
  }
}
