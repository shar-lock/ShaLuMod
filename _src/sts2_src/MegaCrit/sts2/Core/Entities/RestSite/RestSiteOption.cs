// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.RestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public abstract class RestSiteOption
{
  public static Func<Player, List<RestSiteOption>>? generateForTests;

  public abstract string OptionId { get; }

  protected Player Owner { get; }

  public LocString Title => new LocString("rest_site_ui", $"OPTION_{this.OptionId}.name");

  public virtual LocString Description
  {
    get => new LocString("rest_site_ui", $"OPTION_{this.OptionId}.description");
  }

  private string IconPath
  {
    get => ImageHelper.GetImagePath($"ui/rest_site/option_{this.OptionId.ToLowerInvariant()}.png");
  }

  public Texture2D Icon => PreloadManager.Cache.GetTexture2D(this.IconPath);

  public virtual IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(this.IconPath);
    }
  }

  public virtual bool IsEnabled => true;

  protected RestSiteOption(Player owner) => this.Owner = owner;

  public static List<RestSiteOption> Generate(Player player)
  {
    if (TestMode.IsOn && RestSiteOption.generateForTests != null)
      return RestSiteOption.generateForTests(player);
    int capacity = 2;
    List<RestSiteOption> restSiteOptionList = new List<RestSiteOption>(capacity);
    CollectionsMarshal.SetCount<RestSiteOption>(restSiteOptionList, capacity);
    Span<RestSiteOption> span = CollectionsMarshal.AsSpan<RestSiteOption>(restSiteOptionList);
    int num1 = 0;
    span[num1] = (RestSiteOption) new HealRestSiteOption(player);
    int num2 = num1 + 1;
    span[num2] = (RestSiteOption) new SmithRestSiteOption(player);
    List<RestSiteOption> options = restSiteOptionList;
    if (player.RunState.Players.Count > 1)
      options.Add((RestSiteOption) new MendRestSiteOption(player));
    Hook.ModifyRestSiteOptions(player.RunState, player, (ICollection<RestSiteOption>) options);
    return options;
  }

  public abstract Task<bool> OnSelect();

  public virtual Task DoLocalPostSelectVfx(CancellationToken ct = default (CancellationToken))
  {
    return Task.CompletedTask;
  }

  public virtual Task DoRemotePostSelectVfx() => Task.CompletedTask;

  public override bool Equals(object? obj)
  {
    RestSiteOption restSiteOption = obj as RestSiteOption;
    return (object) restSiteOption != null && this.OptionId == restSiteOption.OptionId && this.Owner == restSiteOption.Owner;
  }

  public override int GetHashCode() => (this.OptionId, this.Owner).GetHashCode();

  public static bool operator ==(RestSiteOption? left, RestSiteOption? right)
  {
    if ((object) left == (object) right)
      return true;
    if ((object) left != null)
      return left.Equals((object) right);
    return (object) left == null && (object) right == null;
  }

  public static bool operator !=(RestSiteOption? left, RestSiteOption? right) => !(left == right);
}
