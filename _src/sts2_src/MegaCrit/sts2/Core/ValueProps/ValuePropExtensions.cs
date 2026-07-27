// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ValueProps.ValuePropExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable disable
namespace MegaCrit.Sts2.Core.ValueProps;

public static class ValuePropExtensions
{
  public static bool IsPoweredAttack(this ValueProp props)
  {
    return props.HasFlag((Enum) ValueProp.Move) && !props.HasFlag((Enum) ValueProp.Unpowered);
  }

  public static bool IsPoweredCardOrMonsterMoveBlock(this ValueProp props)
  {
    return props.HasFlag((Enum) ValueProp.Move) && !props.HasFlag((Enum) ValueProp.Unpowered);
  }

  public static bool IsCardOrMonsterMove(this ValueProp props)
  {
    return props.HasFlag((Enum) ValueProp.Move);
  }
}
