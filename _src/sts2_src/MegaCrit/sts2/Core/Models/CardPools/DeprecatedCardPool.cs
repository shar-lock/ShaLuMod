// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.CardPools.DeprecatedCardPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Cards;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.CardPools;

public sealed class DeprecatedCardPool : CardPoolModel
{
  public override string Title => "token";

  public override string EnergyColorName => "colorless";

  public override string CardFrameMaterialPath => "card_frame_colorless";

  public override Color DeckEntryCardColor => Colors.White;

  public override bool IsColorless => true;

  protected override CardModel[] GenerateAllCards()
  {
    return new CardModel[1]
    {
      (CardModel) ModelDb.Card<DeprecatedCard>()
    };
  }
}
