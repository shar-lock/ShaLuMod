// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.ActionTargetExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable disable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public static class ActionTargetExtensions
{
  public static bool IsSingleTarget(this TargetType targetType)
  {
    bool flag;
    switch (targetType)
    {
      case TargetType.Self:
      case TargetType.AnyEnemy:
      case TargetType.AnyPlayer:
      case TargetType.AnyAlly:
      case TargetType.TargetedNoCreature:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }
}
