// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.AssetSets
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Orbs;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rewards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.PotionLab;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.RelicCollection;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

public static class AssetSets
{
  public static IReadOnlySet<string> MainMenuEssentials { get; }

  public static IReadOnlySet<string> IntroLogoAssets { get; }

  public static IReadOnlySet<string> CommonAssets { get; }

  public static IReadOnlySet<string> MainMenuSet { get; }

  public static IReadOnlySet<string> RunSet { get; set; }

  public static IReadOnlySet<string> Act { get; set; }

  private static IEnumerable<string> CardMaterialPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[16 /*0x10*/]
      {
        "res://materials/cards/banners/card_banner_common_mat.tres",
        "res://materials/cards/banners/card_banner_uncommon_mat.tres",
        "res://materials/cards/banners/card_banner_rare_mat.tres",
        "res://materials/cards/banners/card_banner_curse_mat.tres",
        "res://materials/cards/banners/card_banner_status_mat.tres",
        "res://materials/cards/banners/card_banner_event_mat.tres",
        "res://materials/cards/banners/card_banner_quest_mat.tres",
        "res://materials/cards/banners/card_banner_ancient_mat.tres",
        "res://materials/cards/frames/card_frame_red_mat.tres",
        "res://materials/cards/frames/card_frame_green_mat.tres",
        "res://materials/cards/frames/card_frame_blue_mat.tres",
        "res://materials/cards/frames/card_frame_pink_mat.tres",
        "res://materials/cards/frames/card_frame_orange_mat.tres",
        "res://materials/cards/frames/card_frame_colorless_mat.tres",
        "res://materials/cards/frames/card_frame_curse_mat.tres",
        "res://materials/cards/frames/card_frame_quest_mat.tres"
      });
    }
  }

  static AssetSets()
  {
    List<string> items1 = new List<string>();
    items1.AddRange(NMainMenu.AssetPaths);
    items1.AddRange(NTransition.AssetPaths);
    items1.AddRange((IEnumerable<string>) NSettingsScreen.AssetPaths);
    items1.Add("res://shaders/hsv.gdshader");
    items1.Add("res://shaders/dark_blur.gdshader");
    // ISSUE: object of a compiler-generated type is created
    AssetSets.MainMenuEssentials = (IReadOnlySet<string>) new HashSet<string>((IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items1));
    List<string> items2 = new List<string>();
    items2.AddRange(NTransition.AssetPaths);
    items2.AddRange((IEnumerable<string>) NLogoAnimation.AssetPaths);
    // ISSUE: object of a compiler-generated type is created
    AssetSets.IntroLogoAssets = (IReadOnlySet<string>) new HashSet<string>((IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items2));
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: object of a compiler-generated type is created
    AssetSets.CommonAssets = (IReadOnlySet<string>) new HashSet<string>(((IEnumerable<IEnumerable<string>>) new IEnumerable<string>[99]
    {
      NActBanner.AssetPaths,
      NActHistoryEntry.AssetPaths,
      NMultiplayerVoteContainer.AssetPaths,
      NAncientMapPoint.AssetPaths,
      NNormalMapPoint.AssetPaths,
      NAncientNameBanner.AssetPaths,
      NBossMapPoint.AssetPaths,
      (IEnumerable<string>) NInspectCardScreen.AssetPaths,
      (IEnumerable<string>) NInspectRelicScreen.AssetPaths,
      (IEnumerable<string>) NSettingsScreen.AssetPaths,
      (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDevConsole.assetPath),
      NCardPileScreen.AssetPaths,
      NCardRewardSelectionScreen.AssetPaths,
      NChooseABundleSelectionScreen.AssetPaths,
      NChooseACardSelectionScreen.AssetPaths,
      NCombatPileCardSelectScreen.AssetPaths,
      NCombatStartBanner.AssetPaths,
      NCreature.AssetPaths,
      (IEnumerable<string>) NDailyRunLeaderboard.AssetPaths,
      NDeckCardSelectScreen.AssetPaths,
      NDeckEnchantSelectScreen.AssetPaths,
      NDeckHistoryEntry.AssetPaths,
      NDeckUpgradeSelectScreen.AssetPaths,
      NEndTurnButton.AssetPaths,
      NEnemyTurnBanner.AssetPaths,
      NEnergyCounter.AssetPaths,
      NEventOptionButton.AssetPaths,
      NEventRoom.AssetPaths,
      NHandCardHolder.AssetPaths,
      NHoverTipSet.AssetPaths,
      NIntent.AssetPaths,
      NLinkedRewardSet.AssetPaths,
      NMapScreen.AssetPaths,
      NOrb.AssetPaths,
      NOrbManager.AssetPaths,
      NPlayerTurnBanner.AssetPaths,
      NPotion.AssetPaths,
      NPotionHolder.AssetPaths,
      NPotionPopup.AssetPaths,
      NPower.AssetPaths,
      NRelic.AssetPaths,
      NMultiplayerPlayerState.AssetPaths,
      NRestSiteButton.AssetPaths,
      NRestSiteRoom.AssetPaths,
      NRewardButton.AssetPaths,
      NRewardsScreen.AssetPaths,
      NMapPointHistoryEntry.AssetPaths,
      NRun.AssetPaths,
      NSelectedHandCardHolder.AssetPaths,
      NSimpleCardSelectScreen.AssetPaths,
      NSimpleCardsViewScreen.AssetPaths,
      NSpeechBubbleVfx.AssetPaths,
      NStarCounter.AssetPaths,
      NErrorPopup.AssetPaths,
      NTargetingArrow.AssetPaths,
      NThoughtBubbleVfx.AssetPaths,
      NVerticalPopup.AssetPaths,
      NBlockBrokenVfx.AssetPaths,
      NBlockSparkVfx.AssetPaths,
      NCardBundle.AssetPaths,
      NHorizontalLinesVfx.AssetPaths,
      NCardFlyPowerVfx.AssetPaths,
      NCardFlyShuffleVfx.AssetPaths,
      NCardFlyVfx.AssetPaths,
      NCardRareGlow.AssetPaths,
      NCardSmithVfx.AssetPaths,
      NCardTransformVfx.AssetPaths,
      NCardUncommonGlow.AssetPaths,
      NCardUpgradeVfx.AssetPaths,
      NDamageBlockedVfx.AssetPaths,
      NDamageNumVfx.AssetPaths,
      NDeckViewScreen.AssetPaths,
      NDoomVfx.AssetPaths,
      NGainEpochVfx.AssetPaths,
      NGridCardHolder.AssetPaths,
      NHitSparkVfx.AssetPaths,
      NMapCircleVfx.AssetPaths,
      NMapNodeSelectVfx.AssetPaths,
      NMonsterDeathVfx.AssetPaths,
      NPowerAppliedVfx.AssetPaths,
      NPowerFlashVfx.AssetPaths,
      NPowerRemovedVfx.AssetPaths,
      NPowerUpVfx.AssetPaths,
      NPreviewCardHolder.AssetPaths,
      NRelicFlashVfx.AssetPaths,
      NSmokyVignetteVfx.AssetPaths,
      NStunnedVfx.AssetPaths,
      NUiFlashVfx.AssetPaths,
      VfxCmd.AssetPaths,
      ControllerConfig.AllAssetPaths,
      NTransition.AssetPaths,
      ((IEnumerable<RewardType>) Enum.GetValues<RewardType>()).SelectMany<RewardType, string>((Func<RewardType, IEnumerable<string>>) (t => t.GetAssetPaths())),
      (IEnumerable<string>) TmpSfx.assetPaths,
      NPowerAppliedBuffVfx.AssetPaths,
      NPowerAppliedDebuffVfx.AssetPaths,
      PlayerFullscreenHealVfx.AssetPaths,
      NFireSmokePuffVfx.AssetPaths,
      NThinSliceVfx.AssetPaths,
      (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://materials/vfx/hsv.tres")
    }).SelectMany<IEnumerable<string>, string>((Func<IEnumerable<string>, IEnumerable<string>>) (s => s)).Concat<string>(AssetSets.CardMaterialPaths));
    AssetSets.MainMenuSet = (IReadOnlySet<string>) new HashSet<string>(((IEnumerable<IEnumerable<string>>) new IEnumerable<string>[23]
    {
      NCharacterSelectScreen.AssetPaths,
      NMainMenu.AssetPaths,
      NAbandonRunConfirmPopup.AssetPaths,
      NGenericPopup.AssetPaths,
      NMultiplayerWarningPopup.AssetPaths,
      NCard.AssetPaths,
      NMultiplayerLoadGameScreen.AssetPaths,
      (IEnumerable<string>) NBestiary.AssetPaths,
      (IEnumerable<string>) NRelicCollection.AssetPaths,
      (IEnumerable<string>) NPotionLab.AssetPaths,
      (IEnumerable<string>) NCardLibrary.AssetPaths,
      (IEnumerable<string>) NRunHistory.AssetPaths,
      (IEnumerable<string>) NStatsScreen.AssetPaths,
      (IEnumerable<string>) NTimelineScreen.AssetPaths,
      (IEnumerable<string>) NDailyRunScreen.AssetPaths,
      (IEnumerable<string>) NDailyRunLoadScreen.AssetPaths,
      NCustomRunScreen.AssetPaths,
      NCustomRunLoadScreen.AssetPaths,
      (IEnumerable<string>) NModdingScreen.AssetPaths,
      NBestiaryEntry.AssetPaths,
      (IEnumerable<string>) NProfileScreen.AssetPaths,
      NAchievementsGrid.AssetPaths,
      ModelDb.AllCharacters.SelectMany<CharacterModel, string>((Func<CharacterModel, IEnumerable<string>>) (character => character.AssetPathsCharacterSelect))
    }).SelectMany<IEnumerable<string>, string>((Func<IEnumerable<string>, IEnumerable<string>>) (s => s)));
    // ISSUE: reference to a compiler-generated field
    AssetSets.\u003CRunSet\u003Ek__BackingField = (IReadOnlySet<string>) null;
    // ISSUE: reference to a compiler-generated field
    AssetSets.\u003CAct\u003Ek__BackingField = (IReadOnlySet<string>) null;
  }
}
