// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.HoverTips.HoverTipExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.HoverTips;

public static class HoverTipExtensions
{
  public static void MegaTryAddingTip(this ICollection<IHoverTip> tips, IHoverTip tip)
  {
    IHoverTip hoverTip = tips.FirstOrDefault<IHoverTip>((Func<IHoverTip, bool>) (t => t.Id == tip.Id));
    if (hoverTip != null && !hoverTip.IsInstanced)
    {
      if (hoverTip.IsSmart || !tip.IsSmart)
        return;
      tips.Remove(hoverTip);
      tips.Add(tip);
    }
    else
      tips.Add(tip);
  }
}
