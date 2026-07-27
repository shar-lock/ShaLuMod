// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.ThinkCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Vfx;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class ThinkCmd
{
  private const double _defaultTimePerCharacter = 0.08;
  private const double _minTimeToDisplay = 1.5;

  public static void Play(LocString line, Creature speaker, double secondsToDisplay = -1.0)
  {
    string formattedText = line.GetFormattedText();
    if (secondsToDisplay < 0.0)
      secondsToDisplay = (double) formattedText.Length * 0.08;
    if (secondsToDisplay < 1.5)
      secondsToDisplay = 1.5;
    NThoughtBubbleVfx child = NThoughtBubbleVfx.Create(formattedText, speaker, new double?(secondsToDisplay));
    if (child == null)
      return;
    Control vfxContainer = speaker.GetVfxContainer();
    if (vfxContainer == null)
      return;
    ((Node) vfxContainer).AddChildSafely((Node) child);
  }
}
