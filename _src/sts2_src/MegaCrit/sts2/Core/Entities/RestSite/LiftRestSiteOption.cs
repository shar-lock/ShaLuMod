// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.LiftRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public class LiftRestSiteOption(Player owner) : RestSiteOption(owner)
{
  public override LocString Description
  {
    get
    {
      LocString description = base.Description;
      int variable = 3 - this.Owner.GetRelic<Girya>().TimesLifted;
      description.Add("LiftsLeft", (Decimal) variable);
      return description;
    }
  }

  public override string OptionId => "LIFT";

  public override async Task<bool> OnSelect()
  {
    ++this.Owner.GetRelic<Girya>().TimesLifted;
    return await Task.FromResult<bool>(true);
  }

  public override Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Short);
    return Task.CompletedTask;
  }

  public override Task DoRemotePostSelectVfx()
  {
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
    parent?.Shake();
    NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<Girya>());
    if (child == null)
      return Task.CompletedTask;
    if (parent != null)
      ((Node) parent).AddChildSafely((Node) child);
    child.Position = Vector2.Zero;
    return Task.CompletedTask;
  }
}
