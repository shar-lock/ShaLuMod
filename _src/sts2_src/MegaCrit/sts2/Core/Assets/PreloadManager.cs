// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Assets.PreloadManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Assets;

public static class PreloadManager
{
  public static AssetCache Cache { get; } = new AssetCache();

  public static bool Enabled { get; set; } = true;

  public static async Task LoadLogoAnimation()
  {
    await (await PreloadManager.LoadAssetSets("IntroLogo", (IEnumerable<string>) AssetSets.IntroLogoAssets)).WaitForCompletion();
  }

  public static async Task LoadMainMenuEssentials()
  {
    if (TestMode.IsOn)
      return;
    await (await PreloadManager.LoadAssetSets("MainMenuEssentials", (IEnumerable<string>) AssetSets.MainMenuEssentials)).WaitForCompletion();
  }

  public static async Task LoadCommonAndMainMenuAssets()
  {
    PreloadManager.Cache.UnloadMissedCacheAssets();
    AssetLoadingSession assetLoadingSession = await PreloadManager.LoadAssetSets("Common", (IEnumerable<string>) AssetSets.CommonAssets, (IEnumerable<string>) AssetSets.MainMenuSet);
  }

  public static async Task LoadMainMenuAssets()
  {
    if (TestMode.IsOn)
      return;
    await (await PreloadManager.LoadAssetSets("MainMenu", (IEnumerable<string>) AssetSets.MainMenuSet)).WaitForCompletion();
  }

  public static async Task LoadRunAssets(IEnumerable<CharacterModel> characters)
  {
    if (TestMode.IsOn)
      return;
    List<CharacterModel> list = characters.ToList<CharacterModel>();
    bool isMultiplayer = RunManager.Instance.NetService.Type.IsMultiplayer();
    AssetSets.RunSet = (IReadOnlySet<string>) new HashSet<string>(PreloadManager.GetRunAssetPaths((IEnumerable<CharacterModel>) list, isMultiplayer));
    await (await PreloadManager.LoadAssetSets("characters=" + string.Join<string>(',', list.Select<CharacterModel, string>((Func<CharacterModel, string>) (c => c.Id.Entry))), (IEnumerable<string>) AssetSets.CommonAssets, (IEnumerable<string>) AssetSets.RunSet)).WaitForCompletion();
    GC.Collect();
  }

  public static async Task LoadActAssets(ActModel act)
  {
    if (TestMode.IsOn)
      return;
    AssetSets.Act = (IReadOnlySet<string>) new HashSet<string>(act.AssetPaths);
    await (await PreloadManager.LoadAssetSets("Act=" + act.Id.Entry, (IEnumerable<string>) AssetSets.CommonAssets, (IEnumerable<string>) AssetSets.RunSet, (IEnumerable<string>) AssetSets.Act)).WaitForCompletion();
    GC.Collect();
  }

  public static async Task LoadRoomEventAssets(EventModel eventModel, IRunState runState)
  {
    await PreloadManager.LoadRoomAssets("Event Room", eventModel.GetAssetPaths(runState));
  }

  public static async Task LoadRoomCombatAssets(EncounterModel encounter, IRunState runState)
  {
    await PreloadManager.LoadRoomAssets("Combat Room", PreloadManager.GetCombatAssetPaths(encounter, runState));
  }

  public static async Task LoadRoomTreasureAssets(ActModel actModel)
  {
    List<string> items = new List<string>();
    items.Add(actModel.ChestSpineResourcePath);
    items.AddRange(NTreasureRoom.AssetPaths);
    // ISSUE: object of a compiler-generated type is created
    await PreloadManager.LoadRoomAssets("Treasure Room", (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items));
  }

  public static async Task LoadRoomMerchantAssets()
  {
    await PreloadManager.LoadRoomAssets("Merchant Room", NMerchantRoom.AssetPaths);
  }

  public static async Task LoadRoomRestSite(
    ActModel actModel,
    IEnumerable<RestSiteOption> restSiteOptions)
  {
    List<string> items = new List<string>();
    items.Add(actModel.RestSiteBackgroundPath);
    items.AddRange(restSiteOptions.SelectMany<RestSiteOption, string>((Func<RestSiteOption, IEnumerable<string>>) (s => s.AssetPaths)));
    // ISSUE: object of a compiler-generated type is created
    await PreloadManager.LoadRoomAssets("RestSite Room", (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items));
  }

  private static async Task LoadRoomAssets(string roomName, IEnumerable<string> additionalAssets)
  {
    if (TestMode.IsOn)
      return;
    PreloadManager.Cache.UnloadMissedCacheAssets();
    HashSet<string> stringSet1 = new HashSet<string>();
    foreach (string additionalAsset in additionalAssets)
      stringSet1.Add(additionalAsset);
    HashSet<string> stringSet2 = stringSet1;
    await (await PreloadManager.LoadAssetSets(roomName, (IEnumerable<string>) AssetSets.CommonAssets, (IEnumerable<string>) AssetSets.RunSet, (IEnumerable<string>) AssetSets.Act, (IEnumerable<string>) stringSet2)).WaitForCompletion();
    GC.Collect();
  }

  private static async Task<AssetLoadingSession> LoadAssetSets(
    string name,
    params IEnumerable<string>[] assetSets)
  {
    HashSet<string> stringSet1 = new HashSet<string>();
    foreach (string str in ((IEnumerable<IEnumerable<string>>) assetSets).SelectMany<IEnumerable<string>, string>((Func<IEnumerable<string>, IEnumerable<string>>) (set => set)))
      stringSet1.Add(str);
    HashSet<string> stringSet2 = stringSet1;
    IReadOnlySet<string> loadedCacheAssets = PreloadManager.Cache.GetLoadedCacheAssets();
    IEnumerable<string> assetsToUnloadSet = ((IEnumerable<string>) loadedCacheAssets).Except<string>((IEnumerable<string>) stringSet2);
    IEnumerable<string> needLoaded = stringSet2.Except<string>((IEnumerable<string>) loadedCacheAssets);
    PreloadManager.Cache.UnloadAssets(assetsToUnloadSet);
    await Task.Yield();
    AssetLoadingSession assetLoadingSession = PreloadManager.Enabled ? PreloadManager.LoadAssets(needLoaded, name) : AssetLoadingSession.Empty();
    needLoaded = (IEnumerable<string>) null;
    return assetLoadingSession;
  }

  private static AssetLoadingSession LoadAssets(IEnumerable<string> assetPaths, string name)
  {
    AssetLoadingSession session = PreloadManager.Cache.CreateSession(name, assetPaths);
    TaskHelper.RunSafely((Task) NAssetLoader.Instance.LoadInTheBackground(session));
    return session;
  }

  private static IEnumerable<string> GetRunAssetPaths(
    IEnumerable<CharacterModel> characters,
    bool isMultiplayer)
  {
    IEnumerable<CharacterModel> list = (IEnumerable<CharacterModel>) characters.ToList<CharacterModel>();
    IEnumerable<CardModel> source = ModelDb.AllSharedCardPools.SelectMany<CardPoolModel, CardModel>((Func<CardPoolModel, IEnumerable<CardModel>>) (pool => pool.AllCards));
    if (!isMultiplayer)
      source = source.Where<CardModel>((Func<CardModel, bool>) (card => card.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly));
    return ((IEnumerable<IEnumerable<string>>) new IEnumerable<string>[10]
    {
      source.SelectMany<CardModel, string>((Func<CardModel, IEnumerable<string>>) (card => card.RunAssetPaths)),
      list.SelectMany<CharacterModel, string>((Func<CharacterModel, IEnumerable<string>>) (c => c.CardPool.AllCards.SelectMany<CardModel, string>((Func<CardModel, IEnumerable<string>>) (card => card.RunAssetPaths)))),
      list.SelectMany<CharacterModel, string>((Func<CharacterModel, IEnumerable<string>>) (c => c.AssetPaths)),
      NCard.AssetPaths,
      NMapRoom.AssetPaths,
      NChooseACardSelectionScreen.AssetPaths,
      NGameOverScreen.AssetPaths,
      NRelicInventoryHolder.AssetPaths,
      ModelDb.DebugEnchantments.Select<EnchantmentModel, string>((Func<EnchantmentModel, string>) (e => e.IconPath)).Where<string>((Func<string, bool>) (p => p != EnchantmentModel.MissingIconPath)),
      ModelDb.AllCardPools.Select<CardPoolModel, string>((Func<CardPoolModel, string>) (p => p.EnergyIconPath))
    }).SelectMany<IEnumerable<string>, string>((Func<IEnumerable<string>, IEnumerable<string>>) (s => s));
  }

  private static IEnumerable<string> GetCombatAssetPaths(
    EncounterModel encounter,
    IRunState runState)
  {
    if (TestMode.IsOn)
      return (IEnumerable<string>) Array.Empty<string>();
    return ((IEnumerable<IEnumerable<string>>) new IEnumerable<string>[2]
    {
      NCombatRoom.AssetPaths,
      encounter.GetAssetPaths(runState)
    }).SelectMany<IEnumerable<string>, string>((Func<IEnumerable<string>, IEnumerable<string>>) (s => s));
  }
}
