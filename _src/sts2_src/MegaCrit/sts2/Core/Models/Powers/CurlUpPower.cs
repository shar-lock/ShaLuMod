// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CurlUpPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CurlUpPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  protected override object InitInternalData() => (object) new CurlUpPower.Data();

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult _,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner || !props.IsPoweredAttack() || cardSource == null || this.GetInternalData<CurlUpPower.Data>().playedCard != null && cardSource != this.GetInternalData<CurlUpPower.Data>().playedCard)
      return Task.CompletedTask;
    this.GetInternalData<CurlUpPower.Data>().playedCard = cardSource;
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card != this.GetInternalData<CurlUpPower.Data>().playedCard)
      return;
    this.GetInternalData<CurlUpPower.Data>().playedCard = (CardModel) null;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/giant_louse/giant_louse_curl");
    await CreatureCmd.TriggerAnim(this.Owner, "Curl", 0.25f);
    Decimal num = await CreatureCmd.GainBlock(this.Owner, (Decimal) this.Amount, ValueProp.Unpowered, (CardPlay) null);
    if (this.Owner.Monster is LouseProgenitor monster)
      monster.Curled = true;
    await PowerCmd.Remove((PowerModel) this);
  }

  private class Data
  {
    public CardModel? playedCard;
  }
}
