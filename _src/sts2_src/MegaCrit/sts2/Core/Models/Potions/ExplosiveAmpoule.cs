// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Potions.ExplosiveAmpoule
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Potions;

public sealed class ExplosiveAmpoule : PotionModel
{
  public override PotionRarity Rarity => PotionRarity.Common;

  public override PotionUsage Usage => PotionUsage.CombatOnly;

  public override TargetType TargetType => TargetType.AllEnemies;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(10M, ValueProp.Unpowered));
    }
  }

  protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    Creature player = this.Owner.Creature;
    DamageVar damage = this.DynamicVars.Damage;
    IReadOnlyList<Creature> targets = player.CombatState.HittableEnemies;
    foreach (Creature target1 in (IEnumerable<Creature>) targets)
    {
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NFireSmokePuffVfx.Create(target1));
    }
    await Cmd.CustomScaledWait(0.2f, 0.3f);
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) targets, damage.BaseValue, damage.Props, player, (CardModel) null, (CardPlay) null);
    player = (Creature) null;
    damage = (DamageVar) null;
    targets = (IReadOnlyList<Creature>) null;
  }
}
