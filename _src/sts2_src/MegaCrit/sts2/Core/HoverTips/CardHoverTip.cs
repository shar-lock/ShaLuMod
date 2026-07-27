// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.HoverTips.CardHoverTip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;

#nullable enable
namespace MegaCrit.Sts2.Core.HoverTips;

public class CardHoverTip : IHoverTip
{
  public CardModel Card { get; }

  public string Id { get; }

  public bool IsSmart => true;

  public bool IsDebuff => false;

  public bool IsInstanced => false;

  public AbstractModel CanonicalModel => (AbstractModel) this.Card.CanonicalInstance;

  public CardHoverTip(CardModel card)
  {
    this.Card = card.IsMutable ? card : card.ToMutable();
    this.Id = this.Card.Id.ToString();
    if (!card.IsUpgraded)
      return;
    this.Id = this.Id + "+";
  }
}
