// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.SmithRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public sealed class SmithRestSiteOption(Player owner) : RestSiteOption(owner)
{
  private IEnumerable<CardModel>? _selection;

  public override string OptionId => "SMITH";

  public override IEnumerable<string> AssetPaths
  {
    get => base.AssetPaths.Concat<string>(NCardSmithVfx.AssetPaths);
  }

  public int SmithCount { get; set; } = 1;

  public override LocString Description
  {
    get
    {
      LocString description;
      if (this.IsEnabled)
      {
        description = new LocString("rest_site_ui", $"OPTION_{this.OptionId}.description");
        description.Add("Count", (Decimal) this.SmithCount);
      }
      else
        description = new LocString("rest_site_ui", $"OPTION_{this.OptionId}.descriptionDisabled");
      return description;
    }
  }

  public override bool IsEnabled => this.Owner.Deck.UpgradableCardCount != 0;

  public override async Task<bool> OnSelect()
  {
    this._selection = await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, this.SmithCount)
    {
      Cancelable = true,
      RequireManualConfirmation = true
    });
    if (!this._selection.Any<CardModel>())
      return false;
    foreach (CardModel card in this._selection)
      CardCmd.Upgrade(card, CardPreviewStyle.None);
    await Hook.AfterRestSiteSmith(this.Owner.RunState, this.Owner);
    return true;
  }

  public override async Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    NRun instance = NRun.Instance;
    if (instance != null)
      ((Node) instance.GlobalUi.CardPreviewContainer).AddChildSafely((Node) NCardSmithVfx.Create((IEnumerable<CardModel>) this._selection.ToArray<CardModel>()));
    await Cmd.CustomScaledWait(1f, 2f, cancellationToken: ct);
  }

  public override Task DoRemotePostSelectVfx()
  {
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    NRestSiteCharacter parent = instance != null ? instance.Characters.First<NRestSiteCharacter>((Func<NRestSiteCharacter, bool>) (c => c.Player == this.Owner)) : (NRestSiteCharacter) null;
    NCardSmithVfx child = NCardSmithVfx.Create();
    if (child == null)
      return Task.CompletedTask;
    if (parent != null)
      ((Node) parent).AddChildSafely((Node) child);
    child.Position = Vector2.Zero;
    return Task.CompletedTask;
  }
}
