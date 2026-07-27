// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.ScrollHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class ScrollHelper
{
  private const float _scrollAmount = 40f;
  private const float _panScrollSpeed = 50f;
  public const float dragLerpSpeed = 15f;
  public const float snapThreshold = 0.5f;
  public const float bounceBackStrength = 12f;

  public static float GetDragForScrollEvent(InputEvent inputEvent)
  {
    float dragForScrollEvent;
    switch (inputEvent)
    {
      case InputEventMouseButton eventMouseButton:
        MouseButton buttonIndex = eventMouseButton.ButtonIndex;
        if (buttonIndex != 4L)
        {
          if (buttonIndex == 5L)
          {
            dragForScrollEvent = -40f;
            break;
          }
          goto default;
        }
        dragForScrollEvent = 40f;
        break;
      case InputEventPanGesture inputEventPanGesture:
        dragForScrollEvent = (float) (-(double) inputEventPanGesture.Delta.Y * 50.0);
        break;
      default:
        dragForScrollEvent = 0.0f;
        break;
    }
    return dragForScrollEvent;
  }
}
