// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.Badge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public abstract class Badge
{
  protected readonly SerializableRun _run;
  protected readonly bool _won;
  protected readonly SerializablePlayer _localPlayer;

  public string Id { get; }

  public abstract BadgeRarity Rarity { get; }

  public bool RequiresWin { get; }

  public bool MultiplayerOnly { get; }

  public Texture2D BadgeBase
  {
    get
    {
      Texture2D texture2D;
      switch (this.Rarity)
      {
        case BadgeRarity.Bronze:
          texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_bronze.png"));
          break;
        case BadgeRarity.Silver:
          texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_silver.png"));
          break;
        case BadgeRarity.Gold:
          texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_gold.png"));
          break;
        default:
          texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/power_atlas.sprites/missing_power.tres"));
          break;
      }
      return texture2D;
    }
  }

  public Texture2D BadgeIcon
  {
    get
    {
      if (ResourceLoader.Exists(this.IconPath, ""))
        return PreloadManager.Cache.GetTexture2D(this.IconPath);
      Log.Error($"Badge Icon: {this.IconPath} doesn't exist :(");
      return PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("debug/placeholder_64.png"));
    }
  }

  private string IconPath
  {
    get => ImageHelper.GetImagePath($"ui/game_over_screen/badge_{this.Id.ToLowerInvariant()}.png");
  }

  protected Badge(
    SerializableRun run,
    bool won,
    ulong playerId,
    string id,
    bool requiresWin,
    bool multiplayerOnly)
  {
    this._run = run;
    this._won = won;
    this._localPlayer = this._run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) playerId));
    this.Id = id;
    this.RequiresWin = requiresWin;
    this.MultiplayerOnly = multiplayerOnly;
  }

  public abstract bool IsObtained();

  public SerializableBadge ToSerializable()
  {
    return new SerializableBadge()
    {
      Id = this.Id,
      Rarity = this.Rarity
    };
  }
}
