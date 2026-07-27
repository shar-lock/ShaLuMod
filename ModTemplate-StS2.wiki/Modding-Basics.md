Developing a mod for Slay the Spire 2 requires knowledge of modding utilities, how to adapt existing code, and how mods work under the hood.

This page covers a variety of topics, roughly ordered by how soon you need to know them when modding (understanding BaseLib is needed immediately, uploading your mod comes much later).

# BaseLib
[BaseLib](https://github.com/Alchyr/BaseLib-StS2) is a community-developed library that helps the modding process. This guide is built with the assumption that you are using BaseLib, and it's included in the [project templates](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup).

BaseLib provides APIs for configuring mods, adding content, and doing many other common things mods want to do, plus a framework for avoiding conflicts between mods. Documentation for it can be found on the [BaseLib Wiki](https://alchyr.github.io/BaseLib-Wiki/). The [Features List](https://alchyr.github.io/BaseLib-Wiki/docs/Features.html) is a good place to start.

If there's something you're looking to do that isn't covered on this wiki, check BaseLib. This guides here only cover a subset of what BaseLib does.

The best way to get BaseLib is to [subscribe to it on the Steam workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127).

# Learning how to make things
Once you've set up your project, the next challenge is making the content you want. Almost all content in the game exists as a `Model` (e.g. `CardModel`, `RelicModel`) and hooks into shared lifecycle actions like `OnPlay`. All actions are made up of a series of commands (e.g. `DamageCmd.Attack`, `PowerCmd.Apply<StrengthPower>`). It is these commands which give models their behaviour. Always try to use commands over directly manipulating data.

Most models have an inheriting class in BaseLib (e.g. `CustomCardModel`) with extra support; you generally want to inherit these and override the relevant properties.

To implement a specific action (e.g. apply 3 Weak, heal when entering a room):
1. Think about what base game content does something similar; something almost always exists. 
   1. It may help to look through a list of of the game's cards/relics/etc. or search through the game's text files.
   1. Think creatively about base game code that might be like what you want
2. Check the code the base game uses for it (see [Decompiling](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)) and adapt that code for your own purposes (e.g. if you wanted to add strength gain for a card, you'd look for a card that gives strength and find out what commands that card calls).
   1. Alternatively, check if another mod has done something similar, or if BaseLib has built-in support for what you want to do.

If you're wondering how to implement a type of Model (e.g. card, relic, ancient):
* Check for a guide on this wiki
* Check if BaseLib has a `Custom*Model` class (e.g. CustomRelicModel) and look at its documentation
  * Generally you want to make a class that inherits the Custom*Model and override various properties
  * If you're using the Character mod template or the Content mod template, you should inherit the classes named after your mod instead (for example, if your mod is called `FoobarMod` and you're making a card, you should inherit `FoobarModCard`)
* Check how the base game does it (see [Decompiling](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling))

The guides for adding content cover some areas, but StS 2 has far too much to explain how to do everything. Creating a successful mod requires ongoing learning and adaptation.

If you're trying to make something with Godot, the [Godot docs](https://docs.godotengine.org/en/stable/) or other Godot guides are a good place to go.

# Turning mods on and off
Mods can be turned on and off by going to Settings -> Mod Settings. If you disable all mods, you'll be back to playing unmodded. You can also play without mods by launching the game with the `-nomods` flag (with Steam, right-click the game, select Properties, and under General there's Launch Options).

Note that you have separate save files for playing with and without mods. 

# Mod files and where they go
Mods are loaded from two places: one for mods you've subscribed to on the Steam workshop, one for mods you've manually downloaded or are developing yourself.

## Steam workshop
The Steam workshop automatically downloads and updates mods you're subscribed to. You can find the mods downloaded from the workshop in a directory under your Steam installation, `Steam/steamapps/workshop/content/2868840/`. Each mod is in a folder corresponding to its ID from the Steam workshop (found in the URL; e.g., BaseLib's workshop page URL is `https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127`, so the ID for it is `3737335127`).

## Local mods
Mods are also loaded from the `mods` directory, which is either in your StS 2 install directory (Windows/Linux) or in `SlayTheSpire2.app/Contents/MacOS` (Mac). Mods built using the templates from this guide will automatically be copied to the right location. People installing mods manually will need to create the `mods` folder in the right place and then copy over mods.

## Mod files
Mods consist of up to three files:
* `ModName.json`, the manifest, which declares basic information about the mod
* `ModName.dll`, which contains code changes (mods that have no code changes may not have this)
* `ModName.pck`, which contains text and assets (mods with no new text/assets may not have this)

While they can be placed anywhere in the `mods` folder as long as they are together, for organization's sake it's best if each mod is placed in an individual folder named after it. To send your mod to someone or upload it somewhere, send/upload this folder.

## Mod manifest
The manifest file provides basic metadata for each mod, created from the JSON file of the same name in your project. The template creates the file for you, but you'll need to fill things in (and update the version number when releasing new versions or if anything else changes).

It has the following format:
```
{
  "id": "ModName", //The ID, used to identify the mod and avoid conflicts with other mods
  "name": "Test Mod", //The name of the mod, displayed in the mod list
  "author": "me", //The author, also displayed
  "description": "A test mod for this guide.", //A brief description, also displayed
  "version": "v0.0.1", //Version number of the mod, also displayed
  "has_pck": true, //Whether to load a pck for the mod 
  "has_dll": true, //Whether to load a DLL for the mod 
  // Note: min_game_version and the use of min_version for dependencies are new as of game version 0.105 (the May 7th 2026 beta branch)
  "min_game_version": "0.105.0", //The minimum version of the game required for the mod
  "dependencies":  [{"id": "BaseLib", "min_version": "3.1.2"}], //List of mods that this mod depends on and the minimum version required for each
  "affects_gameplay": true //Whether the mod affects gameplay; purely cosmetic or informational mods that don't affect game logic should set this to false 
}
```

Mods with `affects_gameplay` set to `false` will not be checked when connecting to a multiplayer lobby, meaning they can be used when other people in the lobby do not have them. Setting this incorrectly will cause desyncs.

# Version updates and the beta branch
Currently, Slay the Spire 2 is in Early Access, with both a main branch (updated around once a month) and a beta branch (updated every week or two). These updates often break BaseLib and/or individual mods.

BaseLib will be updated for beta branch changes (it may take a day or so); if you're getting weird errors after the game is updated, this is most likely the cause.

You can switch branches in Steam by right-clicking Slay the Spire 2 and going to Properties -> Game Versions and Betas.

Individual mods may or may not need updates of their own. Generally, if a mod uses BaseLib's APIs, it will work fine once BaseLib is updated. If a mod has its own patches, it may break and need updates.

Each update has a version number, but right now, there's no automatic way of checking what branch or version a mod supports.

# Uploading and sharing your mod
The [Slay the Spire 2 Steam workshop](https://steamcommunity.com/app/2868840/workshop/) is the place to share and find mods.

For uploading mods, Megacrit have provided a [mod uploader on GitHub.](https://github.com/megacrit/sts2-mod-uploader). See the instructions there.

Previously, people used the following places to share mods:
* The [Repository of early StS2 mods](https://discord.com/channels/309399445785673728/1480486428843446383/1480486428843446383) on the StS Discord
* The [Slay the Spire 2 section on NexusMods](https://www.nexusmods.com/games/slaythespire2)

When uploading your mod, you should upload the directory named after your mod, that contains your mod files (see the section on mod files above).