// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.DenseVegetation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class DenseVegetation : EventModel
{
  public override bool IsShared => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new GoldVar(0),
        (DynamicVar) new HealVar(0M),
        (DynamicVar) new HpLossVar(8M)
      });
    }
  }

  public override void CalculateVars()
  {
    this.DynamicVars.Gold.BaseValue = (Decimal) this.Rng.NextInt(61, 100);
    this.DynamicVars.Heal.BaseValue = this.Owner != null ? HealRestSiteOption.GetHealAmount(this.Owner) : 0M;
  }

  public override bool IsAllowed(IRunState runState)
  {
    if (runState.Players.Count == 1)
      return true;
    foreach (Player player in (IEnumerable<Player>) runState.Players)
    {
      if ((Decimal) player.Creature.CurrentHp <= this.DynamicVars.HpLoss.BaseValue)
        return false;
    }
    return true;
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.TrudgeOn), "DENSE_VEGETATION.pages.INITIAL.options.TRUDGE_ON", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars.HpLoss.BaseValue),
      new EventOption((EventModel) this, new Func<Task>(this.Rest), "DENSE_VEGETATION.pages.INITIAL.options.REST", Array.Empty<IHoverTip>())
    });
  }

  private async Task TrudgeOn()
  {
    Control container = NEventRoom.Instance?.VfxContainer;
    if (LocalContext.IsMe(this.Owner) && container != null)
    {
      for (int i = 0; i < 3; ++i)
      {
        Vector2 vector2_1;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_1).\u002Ector(container.Size.X * 0.25f, container.Size.Y * 0.6f);
        Vector2 vector2_2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_2).\u002Ector(Rng.Chaotic.NextFloat(-100f, 100f), Rng.Chaotic.NextFloat(-200f, 200f));
        Node2D node2D1 = VfxCmd.PlayNonCombatVfx((Godot.Node) container, Vector2.op_Addition(vector2_1, vector2_2), "vfx/vfx_attack_slash");
        Node2D node2D2 = VfxCmd.PlayNonCombatVfx((Godot.Node) container, Vector2.op_Addition(vector2_1, vector2_2), "vfx/events/dense_vegetation_slice_vfx");
        NDebugAudioManager.Instance.Play("slash_attack.mp3", 0.8f, PitchVariance.Medium);
        node2D1.RotationDegrees = (float) (-(double) Rng.Chaotic.NextFloat() * 180.0);
        node2D2.RotationDegrees = (float) (-(double) Rng.Chaotic.NextFloat() * 180.0);
        await Cmd.CustomScaledWait(0.2f, 0.4f);
      }
    }
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("DENSE_VEGETATION.pages.TRUDGE_ON.description"));
    container = (Control) null;
  }

  private async Task Rest()
  {
    await PlayerCmd.MimicRestSiteHeal(this.Owner, false);
    if (LocalContext.IsMe(this.Owner))
    {
      int restHandle = NDebugAudioManager.Instance.Play("sleep.tres", 0.8f);
      await Cmd.CustomScaledWait(0.7f, 1.5f);
      NDebugAudioManager.Instance.Stop(restHandle);
      NDebugAudioManager.Instance.Play("hiss.mp3", 0.8f, PitchVariance.Large);
      NGame.Instance.ScreenRumble(ShakeStrength.Medium, ShakeDuration.Normal, RumbleStyle.Rumble);
    }
    // ISSUE: object of a compiler-generated type is created
    this.SetEventState(this.L10NLookup("DENSE_VEGETATION.pages.REST.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlySingleElementList<EventOption>(new EventOption((EventModel) this, new Func<Task>(this.Fight), "DENSE_VEGETATION.pages.REST.options.FIGHT", Array.Empty<IHoverTip>())));
  }

  private Task Fight()
  {
    this.EnterCombatWithoutExitingEvent<DenseVegetationEventEncounter>((IReadOnlyList<Reward>) Array.Empty<Reward>(), false);
    return Task.CompletedTask;
  }
}
