// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Soulbound
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Soulbound : CardModel
{
  public Soulbound()
    : base(1, CardType.Power, CardRarity.Uncommon, TargetType.AnyAlly)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromCard<Soul>());
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
    await CreatureCmd.TriggerAnim(cardPlay.Target, "PowerUp", cardPlay.Target.Player.Character.PowerUpAnimDelay);
    SoulboundPower soulboundPower = await PowerCmd.Apply<SoulboundPower>(choiceContext, cardPlay.Target, 1M, this.Owner.Creature, (CardModel) this);
  }

  protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Innate);
}
