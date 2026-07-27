// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.ICardScope
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public interface ICardScope
{
  T CreateCard<T>(Player owner) where T : CardModel;

  CardModel CreateCard(CardModel canonicalCard, Player owner);

  CardModel CloneCard(CardModel mutableCard);

  void AddCard(CardModel mutableCard, Player owner);

  void RemoveCard(CardModel card);

  static ICardScope DebugOnlyGet(CardScope scope)
  {
    if (scope == CardScope.Run)
      return (ICardScope) RunManager.Instance.DebugOnlyGetState();
    if (scope == CardScope.Combat)
      return (ICardScope) CombatManager.Instance.DebugOnlyGetState();
    throw new ArgumentOutOfRangeException(nameof (scope), (object) scope, (string) null);
  }
}
