// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RavenousPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Monsters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RavenousPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.Static(StaticHoverTip.Stun)
      });
    }
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature target,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || target == this.Owner || target.Side != this.Owner.Side || this.Owner.IsDead)
      return;
    this.Flash();
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous");
    await CreatureCmd.TriggerAnim(this.Owner, "DevourStartTrigger", 0.5f);
    ((CorpseSlug) this.Owner.Monster).IsRavenous = true;
    await CreatureCmd.Stun(this.Owner, new Func<IReadOnlyList<Creature>, Task>(this.StunnedMove));
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) this.Amount, this.Owner, (CardModel) null);
  }

  private async Task StunnedMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/corpse_slugs/corpse_slugs_ravenous_up_double");
    await CreatureCmd.TriggerAnim(this.Owner, "DevourEndkTrigger", 0.5f);
    ((CorpseSlug) this.Owner.Monster).IsRavenous = false;
  }
}
