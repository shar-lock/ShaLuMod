// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardTypeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public static class CardTypeExtensions
{
  public static LocString ToLocString(this CardType cardType)
  {
    LocString locString;
    switch (cardType)
    {
      case CardType.None:
        locString = new LocString("gameplay_ui", "CARD_TYPE.NONE");
        break;
      case CardType.Attack:
        locString = new LocString("gameplay_ui", "CARD_TYPE.ATTACK");
        break;
      case CardType.Skill:
        locString = new LocString("gameplay_ui", "CARD_TYPE.SKILL");
        break;
      case CardType.Power:
        locString = new LocString("gameplay_ui", "CARD_TYPE.POWER");
        break;
      case CardType.Status:
        locString = new LocString("gameplay_ui", "CARD_TYPE.STATUS");
        break;
      case CardType.Curse:
        locString = new LocString("gameplay_ui", "CARD_TYPE.CURSE");
        break;
      case CardType.Quest:
        locString = new LocString("gameplay_ui", "CARD_TYPE.QUEST");
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) cardType);
        break;
    }
    return locString;
  }
}
