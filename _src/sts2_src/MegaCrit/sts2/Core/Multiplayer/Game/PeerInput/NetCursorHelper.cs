// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.NetCursorHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.TestSupport;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;

public static class NetCursorHelper
{
  public static readonly QuantizeParams quantizeParams = new QuantizeParams(-3f, 3f, 16 /*0x10*/);

  public static Vector2 GetNormalizedPosition(Vector2 mouseScreenPos, Control? rootNode)
  {
    if (rootNode == null)
    {
      if (TestMode.IsOn)
        return mouseScreenPos;
      throw new InvalidOperationException("Root node should only be null in tests!");
    }
    Vector2 vector2_1 = Transform2D.op_Multiply(((CanvasItem) rootNode).GetGlobalTransformWithCanvas(), mouseScreenPos);
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(960f, 540f);
    return Vector2.op_Division(Vector2.op_Subtraction(vector2_1, Vector2.op_Division(rootNode.Size, 2f)), vector2_2);
  }

  public static Vector2 GetControlSpacePosition(Vector2 normalizedCursorPosition, Control? rootNode)
  {
    if (rootNode == null)
    {
      if (TestMode.IsOn)
        return normalizedCursorPosition;
      throw new InvalidOperationException("Root node should only be null in tests!");
    }
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(960f, 540f);
    return Vector2.op_Addition(Vector2.op_Multiply(normalizedCursorPosition, vector2), Vector2.op_Division(rootNode.Size, 2f));
  }
}
