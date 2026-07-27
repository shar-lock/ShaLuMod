// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetScreenTypeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public static class NetScreenTypeExtensions
{
  public static Texture2D? GetLocationIcon(this NetScreenType screenType)
  {
    string str;
    switch (screenType)
    {
      case NetScreenType.None:
        str = (string) null;
        break;
      case NetScreenType.Room:
        str = (string) null;
        break;
      case NetScreenType.Map:
        str = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_map.tres";
        break;
      case NetScreenType.Settings:
        str = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_settings.tres";
        break;
      case NetScreenType.Compendium:
        str = "res://images/atlases/ui_atlas.sprites/compendium.tres";
        break;
      case NetScreenType.DeckView:
        str = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_deck.tres";
        break;
      case NetScreenType.CardPile:
        str = "res://images/packed/combat_ui/discard_pile.png";
        break;
      case NetScreenType.SimpleCardsView:
        str = "res://images/ui/reward_screen/reward_icon_card.png";
        break;
      case NetScreenType.CardSelection:
        str = "res://images/ui/reward_screen/reward_icon_card.png";
        break;
      case NetScreenType.GameOver:
        str = (string) null;
        break;
      case NetScreenType.PauseMenu:
        str = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_settings.tres";
        break;
      case NetScreenType.Rewards:
        str = "res://images/ui/reward_screen/reward_icon_money.png";
        break;
      case NetScreenType.Feedback:
        str = "res://images/atlases/ui_atlas.sprites/top_bar/top_bar_settings.tres";
        break;
      case NetScreenType.SharedRelicPicking:
        str = "res://images/ui/reward_screen/reward_icon_shared_relic.png";
        break;
      case NetScreenType.RemotePlayerExpandedState:
        str = (string) null;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (screenType), (object) screenType, (string) null);
    }
    string path = str;
    return path == null ? (Texture2D) null : PreloadManager.Cache.GetTexture2D(path);
  }
}
