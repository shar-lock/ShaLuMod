// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.TalkCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Text.RegularExpressions;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class TalkCmd
{
  public static NSpeechBubbleVfx? Play(
    LocString line,
    Creature speaker,
    VfxColor vfxColor,
    VfxDuration duration = VfxDuration.Custom)
  {
    if (speaker.IsDead)
      return (NSpeechBubbleVfx) null;
    string formattedText = line.GetFormattedText();
    double num;
    if (duration == VfxDuration.Custom)
    {
      num = (double) TalkCmd.GetRawCharCount(formattedText) * (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.1 : 0.12);
    }
    else
    {
      num = TalkCmd.GetDuration(duration);
      if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
        num -= 0.5;
    }
    double secondsToDisplay = Math.Max(0.5, num);
    NSpeechBubbleVfx child = NSpeechBubbleVfx.Create(formattedText, speaker, secondsToDisplay, vfxColor);
    if (child != null)
    {
      Control vfxContainer = speaker.GetVfxContainer();
      if (vfxContainer != null)
        ((Node) vfxContainer).AddChildSafely((Node) child);
    }
    return child;
  }

  private static double GetDuration(VfxDuration duration)
  {
    double duration1;
    switch (duration)
    {
      case VfxDuration.None:
        duration1 = 0.0;
        break;
      case VfxDuration.VeryShort:
        duration1 = 1.0;
        break;
      case VfxDuration.Short:
        duration1 = 1.5;
        break;
      case VfxDuration.Standard:
        duration1 = 1.75;
        break;
      case VfxDuration.Long:
        duration1 = 2.25;
        break;
      case VfxDuration.VeryLong:
        duration1 = 3.0;
        break;
      case VfxDuration.Forever:
        duration1 = 999999999.0;
        break;
      default:
        duration1 = 0.0;
        break;
    }
    return duration1;
  }

  private static int GetRawCharCount(string bbcodeText)
  {
    return Regex.Replace(bbcodeText, "\\[/?[^\\]]+\\]", "").Replace("\n", "").Replace("\r", "").Replace(" ", "").Length;
  }
}
