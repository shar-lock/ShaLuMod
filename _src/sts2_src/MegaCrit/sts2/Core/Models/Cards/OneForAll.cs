// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.OneForAll
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class OneForAll : CardModel
{
  public OneForAll()
    : base(1, CardType.Power, CardRarity.Rare, TargetType.AllAllies)
  {
  }

  public override CardMultiplayerConstraint MultiplayerConstraint
  {
    get => CardMultiplayerConstraint.MultiplayerOnly;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<OneForAllPower>(3M));
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ICombatState combatState = this.CombatState;
    foreach (Player player in (IEnumerable<Player>) ((combatState != null ? (object) combatState.Players : (object) null) ?? (object) Array.Empty<Player>()))
    {
      await CreatureCmd.TriggerAnim(player.Creature, "PowerUp", player.Character.PowerUpAnimDelay);
      OneForAllPower oneForAllPower = await PowerCmd.Apply<OneForAllPower>(choiceContext, player.Creature, this.DynamicVars["OneForAllPower"].BaseValue, this.Owner.Creature, (CardModel) this);
    }
  }

  protected override void OnUpgrade() => this.DynamicVars["OneForAllPower"].UpgradeValueBy(1M);
}
