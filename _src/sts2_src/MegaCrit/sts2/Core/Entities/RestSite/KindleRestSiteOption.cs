// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.KindleRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
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

public class KindleRestSiteOption(Player owner) : RestSiteOption(owner)
{
  public override string OptionId => "KINDLE";

  public override LocString Description
  {
    get
    {
      LocString description = base.Description;
      description.Add("RelicName", ModelDb.Relic<PumpkinCandle>().Title.GetFormattedText());
      description.Add("RekindleAmount", 5M);
      return description;
    }
  }

  public override Task<bool> OnSelect()
  {
    this.Owner.GetRelic<PumpkinCandle>()?.Rekindle();
    return Task.FromResult<bool>(true);
  }

  public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    this.PlayKindleVfx();
    return Task.CompletedTask;
  }

  public override Task DoRemotePostSelectVfx()
  {
    this.PlayKindleVfx();
    return Task.CompletedTask;
  }

  private void PlayKindleVfx()
  {
    SfxCmd.Play("event:/sfx/characters/attack_fire");
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
    parent?.Shake();
    NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<PumpkinCandle>());
    if (child == null)
      return;
    if (parent != null)
      ((Node) parent).AddChildSafely((Node) child);
    child.Position = Vector2.Zero;
  }
}
