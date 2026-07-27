// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.HatchRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
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

public class HatchRestSiteOption(Player owner) : RestSiteOption(owner)
{
  public override string OptionId => "HATCH";

  public override async Task<bool> OnSelect()
  {
    Byrdpip byrdpip = await RelicCmd.Obtain<Byrdpip>(this.Owner);
    return true;
  }

  public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack");
    return Task.CompletedTask;
  }

  public override Task DoRemotePostSelectVfx()
  {
    SfxCmd.Play("event:/sfx/byrdpip/byrdpip_attack");
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
    NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<Byrdpip>());
    if (child == null)
      return Task.CompletedTask;
    if (parent != null)
      ((Node) parent).AddChildSafely((Node) child);
    child.Position = Vector2.Zero;
    return Task.CompletedTask;
  }
}
