// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ThunderPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ThunderPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.Evoke),
        HoverTipFactory.FromOrb<LightningOrb>()
      });
    }
  }

  public override async Task AfterOrbEvoked(
    PlayerChoiceContext choiceContext,
    OrbModel orb,
    IEnumerable<Creature> targets)
  {
    List<Creature> livingTargets;
    if (orb.Owner != this.Owner.Player)
      livingTargets = (List<Creature>) null;
    else if (!(orb is LightningOrb))
    {
      livingTargets = (List<Creature>) null;
    }
    else
    {
      livingTargets = targets.Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).ToList<Creature>();
      this.Flash();
      SfxCmd.Play("slash_attack.mp3");
      VfxCmd.PlayOnCreatureCenters((IEnumerable<Creature>) livingTargets, "vfx/vfx_attack_slash");
      await CreatureCmd.TriggerAnim(orb.Owner.Creature, "Attack", this.Owner.Player.Character.AttackAnimDelay);
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) livingTargets, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner, (CardModel) null, (CardPlay) null);
      livingTargets = (List<Creature>) null;
    }
  }
}
