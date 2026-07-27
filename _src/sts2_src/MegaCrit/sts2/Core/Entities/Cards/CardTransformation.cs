// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardTransformation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public readonly struct CardTransformation
{
  public CardModel Original { get; }

  public CardModel? Replacement { get; }

  public IEnumerable<CardModel>? ReplacementOptions { get; }

  public bool IsInCombat { get; }

  public CardTransformation(CardModel original)
  {
    CardTransformation.AssertTransformable(original);
    this.Original = original;
    this.ReplacementOptions = (IEnumerable<CardModel>) null;
    this.Replacement = (CardModel) null;
    this.IsInCombat = original.CombatState != null;
  }

  public CardTransformation(CardModel original, IEnumerable<CardModel> options)
  {
    CardTransformation.AssertTransformable(original);
    this.Original = original;
    this.ReplacementOptions = options;
    this.Replacement = (CardModel) null;
    this.IsInCombat = original.CombatState != null;
  }

  public CardTransformation(CardModel original, CardModel replacement)
  {
    CardTransformation.AssertTransformable(original);
    this.Original = original;
    this.Replacement = replacement;
    this.ReplacementOptions = (IEnumerable<CardModel>) null;
    this.IsInCombat = original.CombatState != null;
  }

  public CardModel? GetReplacement(Rng? rng)
  {
    if (this.Replacement != null)
      return this.Replacement;
    if (rng == null)
      throw new ArgumentException("RNG must be passed when replacement options is set!");
    if (!this.Original.IsTransformable)
      return (CardModel) null;
    return this.ReplacementOptions == null ? CardFactory.CreateRandomCardForTransform(this.Original, this.IsInCombat, rng) : CardFactory.CreateRandomCardForTransform(this.Original, this.ReplacementOptions, this.IsInCombat, rng);
  }

  public IEnumerable<CardTransformation> Yield()
  {
    yield return this;
  }

  private static void AssertTransformable(CardModel card)
  {
    if (!card.IsTransformable)
      throw new InvalidOperationException("Non-removable cards cannot be transformed!");
  }
}
