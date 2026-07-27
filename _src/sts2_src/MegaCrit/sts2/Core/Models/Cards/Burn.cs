// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Burn
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Burn : CardModel
{
  public Burn()
    : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(2M, ValueProp.Unpowered | ValueProp.Move));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  public override bool HasTurnEndInHandEffect => true;

  protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

  protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
  {
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance != null)
      ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NGroundFireVfx.Create(this.Owner.Creature));
    SfxCmd.Play("event:/sfx/characters/attack_fire");
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.Damage, (CardModel) this, (CardPlay) null);
  }
}
