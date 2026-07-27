// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.HoverTips.IHoverTip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.HoverTips;

public interface IHoverTip
{
  private static readonly string _summonStaticId = HoverTipFactory.Static(StaticHoverTip.SummonStatic).Id;
  private static readonly string _summonDynamicId = HoverTipFactory.Static(StaticHoverTip.SummonDynamic, (DynamicVar) new SummonVar(0M)).Id;

  string Id { get; }

  bool IsSmart { get; }

  bool IsDebuff { get; }

  bool IsInstanced { get; }

  AbstractModel? CanonicalModel { get; }

  static IEnumerable<IHoverTip> RemoveDupes(IEnumerable<IHoverTip> tips)
  {
    List<IHoverTip> source = new List<IHoverTip>();
    foreach (IHoverTip tip1 in tips)
    {
      IHoverTip hoverTip = tip1;
      if (string.IsNullOrEmpty(hoverTip.Id))
      {
        source.Add(hoverTip);
      }
      else
      {
        IHoverTip hoverTip1 = source.FirstOrDefault<IHoverTip>((Func<IHoverTip, bool>) (tip => tip.Id == hoverTip.Id && !tip.IsInstanced));
        if (hoverTip1 == null)
          source.Add(hoverTip);
        else if (!hoverTip1.IsSmart || hoverTip.IsSmart)
        {
          source.Remove(hoverTip1);
          source.Add(hoverTip);
        }
      }
    }
    if (source.Any<IHoverTip>((Func<IHoverTip, bool>) (tip => tip.Id == IHoverTip._summonStaticId)))
      source.RemoveAll((Predicate<IHoverTip>) (tip => tip.Id == IHoverTip._summonDynamicId));
    return (IEnumerable<IHoverTip>) source;
  }
}
