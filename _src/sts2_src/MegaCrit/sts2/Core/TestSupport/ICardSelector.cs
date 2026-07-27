// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.TestSupport.ICardSelector
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.TestSupport;

public interface ICardSelector
{
  Task<IEnumerable<CardModel>> GetSelectedCards(
    IEnumerable<CardModel> options,
    int minSelect,
    int maxSelect);

  CardRewardSelection GetSelectedCardReward(
    IReadOnlyList<CardCreationResult> options,
    IReadOnlyList<CardRewardAlternative> alternatives);
}
