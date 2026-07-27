// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Ascension.AscensionManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Ascension;

public class AscensionManager
{
  public const int maxAscensionAllowed = 10;
  private readonly int _level;

  public AscensionManager(int level) => this._level = level;

  public AscensionManager(AscensionLevel level) => this._level = (int) level;

  public bool HasLevel(AscensionLevel level) => (AscensionLevel) this._level >= level;

  public void ApplyEffectsTo(Player player)
  {
    if (this.HasLevel(AscensionLevel.TightBelt))
      player.SubtractFromMaxPotionCount(1);
    if (!this.HasLevel(AscensionLevel.AscendersBane))
      return;
    AscendersBane card = player.RunState.CreateCard<AscendersBane>(player);
    card.FloorAddedToDeck = new int?(1);
    player.Deck.AddInternal((CardModel) card, silent: true);
  }
}
