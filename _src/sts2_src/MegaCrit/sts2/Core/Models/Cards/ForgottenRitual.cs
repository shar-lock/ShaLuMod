// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.ForgottenRitual
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class ForgottenRitual : CardModel
{
  public ForgottenRitual()
    : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
  {
  }

  protected override bool ShouldGlowGoldInternal => this.WasCardExhaustedThisTurn;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(3));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        this.EnergyHoverTip,
        HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
      });
    }
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!this.WasCardExhaustedThisTurn)
      return;
    NGroundFireVfx child = NGroundFireVfx.Create(this.Owner.Creature, VfxColor.Purple);
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    await PlayerCmd.GainEnergy((Decimal) this.DynamicVars.Energy.IntValue, this.Owner);
  }

  protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1M);

  private bool WasCardExhaustedThisTurn
  {
    get
    {
      return CombatManager.Instance.History.Entries.OfType<CardExhaustedEntry>().Any<CardExhaustedEntry>((Func<CardExhaustedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.Actor == this.Owner.Creature));
    }
  }
}
