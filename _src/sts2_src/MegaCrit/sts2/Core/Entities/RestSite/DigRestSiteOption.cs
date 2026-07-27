// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.DigRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public class DigRestSiteOption(Player owner) : RestSiteOption(owner)
{
  public override string OptionId => "DIG";

  public override async Task<bool> OnSelect()
  {
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    return true;
  }

  public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    NDebugAudioManager.Instance.Play("sts_sfx_shovel_v1.mp3", variance: PitchVariance.Small);
    return Task.CompletedTask;
  }

  public override Task DoRemotePostSelectVfx()
  {
    NDebugAudioManager.Instance?.Play("sts_sfx_shovel_v1.mp3", 0.5f, PitchVariance.Small);
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
    parent?.Shake();
    NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<Shovel>());
    if (child == null)
      return Task.CompletedTask;
    if (parent != null)
      ((Node) parent).AddChildSafely((Node) child);
    child.Position = Vector2.Zero;
    return Task.CompletedTask;
  }
}
