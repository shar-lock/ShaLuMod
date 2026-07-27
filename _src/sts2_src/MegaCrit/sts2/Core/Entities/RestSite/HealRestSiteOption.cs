// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.HealRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public sealed class HealRestSiteOption(Player owner) : RestSiteOption(owner)
{
  public override string OptionId => "HEAL";

  public override IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.AddRange(base.AssetPaths);
      items.AddRange(NRestSmokeVfx.AssetPaths);
      items.AddRange(NDesaturateTransitionVfx.AssetPaths);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  public static Decimal GetHealAmount(Player player)
  {
    return Hook.ModifyRestSiteHealAmount(player.RunState, player.Creature, HealRestSiteOption.GetBaseHealAmount(player.Creature));
  }

  public override LocString Description
  {
    get
    {
      LocString description = base.Description;
      HealVar healVar1 = new HealVar(HealRestSiteOption.GetBaseHealAmount(this.Owner.Creature));
      healVar1.PreviewValue = HealRestSiteOption.GetHealAmount(this.Owner);
      HealVar healVar2 = healVar1;
      description.Add("Character", this.Owner.Character.Id.Entry);
      description.Add((DynamicVar) healVar2);
      IReadOnlyList<LocString> source = Hook.ModifyExtraRestSiteHealText(this.Owner.RunState, this.Owner, (IReadOnlyList<LocString>) Array.Empty<LocString>());
      if (source.Any<LocString>())
        description.Add("ExtraText", "\n" + string.Join("\n", source.Select<LocString, string>((Func<LocString, string>) (s => s.GetFormattedText()))));
      else
        description.Add("ExtraText", string.Empty);
      return description;
    }
  }

  public override async Task<bool> OnSelect()
  {
    await HealRestSiteOption.ExecuteRestSiteHeal(this.Owner, false);
    return true;
  }

  public override async Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    HealRestSiteOption.PlayRestSiteHealSfx();
    NRestSiteRoom instance1 = NRestSiteRoom.Instance;
    if (instance1 != null)
      ((Node) instance1).AddChildSafely((Node) NRestSmokeVfx.Create());
    NRestSiteRoom instance2 = NRestSiteRoom.Instance;
    if (instance2 != null)
      ((Node) instance2).AddChildSafely((Node) NDesaturateTransitionVfx.Create());
    await Cmd.CustomScaledWait(1.5f, 2.5f, cancellationToken: ct);
  }

  public override Task DoRemotePostSelectVfx()
  {
    NDebugAudioManager.Instance?.Play("SOTE_SFX_SleepBlanket_v1.mp3", 0.5f, PitchVariance.Small);
    return Task.CompletedTask;
  }

  public static Decimal GetBaseHealAmount(Creature creature) => (Decimal) creature.MaxHp * 0.3M;

  public static void PlayRestSiteHealSfx()
  {
    NDebugAudioManager.Instance?.Play("sleep.tres");
    NDebugAudioManager.Instance?.Play("SOTE_SFX_SleepBlanket_v1.mp3", variance: PitchVariance.Small);
  }

  public static async Task ExecuteRestSiteHeal(Player player, bool isMimicked)
  {
    await CreatureCmd.Heal(player.Creature, HealRestSiteOption.GetHealAmount(player));
    await Hook.AfterRestSiteHeal(player.RunState, player, isMimicked);
    List<Reward> rewards = new List<Reward>();
    Hook.ModifyRestSiteHealRewards(player.RunState, player, rewards, isMimicked);
    await RewardsCmd.OfferCustom(player, rewards);
  }
}
