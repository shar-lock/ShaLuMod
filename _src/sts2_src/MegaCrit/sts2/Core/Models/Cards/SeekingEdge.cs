// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.SeekingEdge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class SeekingEdge : CardModel
{
  public SeekingEdge()
    : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new ForgeVar(7));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "PowerUp", this.Owner.Character.PowerUpAnimDelay);
    SeekingEdgePower seekingEdgePower = await PowerCmd.Apply<SeekingEdgePower>(choiceContext, this.Owner.Creature, 1M, this.Owner.Creature, (CardModel) this);
    IEnumerable<SovereignBlade> sovereignBlades = await ForgeCmd.Forge((Decimal) this.DynamicVars.Forge.IntValue, this.Owner, (AbstractModel) this);
  }

  protected override void OnUpgrade() => this.DynamicVars.Forge.UpgradeValueBy(4M);
}
