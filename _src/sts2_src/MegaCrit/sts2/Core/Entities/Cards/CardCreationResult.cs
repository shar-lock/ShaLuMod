// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardCreationResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public class CardCreationResult
{
  public readonly CardModel originalCard;
  private CardModel? _modifiedCard;
  private readonly List<RelicModel> _modifyingRelics = new List<RelicModel>();

  public CardModel Card => this._modifiedCard ?? this.originalCard;

  public IEnumerable<RelicModel> ModifyingRelics => (IEnumerable<RelicModel>) this._modifyingRelics;

  public bool HasBeenModified => this._modifiedCard != null;

  public CardCreationResult(CardModel originalCard) => this.originalCard = originalCard;

  public void ModifyCard(CardModel card, RelicModel modifyingRelic)
  {
    this._modifiedCard = card;
    this._modifyingRelics.Add(modifyingRelic);
  }

  public void ModifyCard(CardModel card) => this._modifiedCard = card;
}
