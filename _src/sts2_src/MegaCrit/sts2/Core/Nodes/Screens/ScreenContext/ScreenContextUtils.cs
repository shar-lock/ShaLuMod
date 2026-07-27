// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext.ScreenContextUtils
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;

public static class ScreenContextUtils
{
  public static void UpdateControllerNavEnabled<T>(this T screenContext) where T : Control, IScreenContext
  {
    if (ActiveScreenContext.Instance.IsCurrent((IScreenContext) screenContext))
      screenContext.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 2L;
    else
      screenContext.FocusBehaviorRecursive = (Control.FocusBehaviorRecursiveEnum) 1L;
  }
}
