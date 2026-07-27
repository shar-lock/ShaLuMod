# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repository is

This repo distributes **`dotnet new` templates** for Slay the Spire 2 mods. It is not itself a mod, and it does not build against the game. The root `Alchyr.Sts2.Templates.csproj` simply packs everything under `content/` into a NuGet template package (`Alchyr.Sts2.Templates`). The three templates are consumed by mod authors via `dotnet new install Alchyr.Sts2.Templates`.

Do not try to `dotnet build` the templates in place — they reference `sts2.dll` / `0Harmony.dll` from a local Slay the Spire 2 install and will only compile once instantiated by `dotnet new` on a developer's machine.

## Commands

- `dotnet pack` (run in repo root) — produces the `Alchyr.Sts2.Templates.<version>.nupkg` template package.
- `dotnet new install <path-to-nupkg>` — install a locally packed template for testing.
- `dotnet new install Alchyr.Sts2.Templates` — install the published template from NuGet.
- `dotnet new alchyrsts2mod --name MyMod -o MyMod --ModAuthor Me` — instantiate the empty template (similar for `alchyrsts2contentmod`, `alchyrsts2charmod`). See each template's `.template.config/template.json` for available symbols.

There are no tests and no lint step in this repo.

## Repository layout

- `Alchyr.Sts2.Templates.csproj` — NuGet pack project only. `PackageType=Template`, packs `content/**/*`.
- `content/ModTemplate/` — empty mod template (short name `alchyrsts2mod`).
- `content/ContentModTemplate/` — content mod template with cards/relics/powers (short name `alchyrsts2contentmod`).
- `content/CharacterModTemplate/` — full character mod template with character + pools + UI assets (short name `alchyrsts2charmod`).
- `ModTemplate-StS2.wiki/` — checked-in wiki content (mirrors the GitHub wiki). Read these for authoritative workflows: `Setup.md`, `Modding-Basics.md`, `Common-Commands-Cookbook.md`, `Testing-and-Debugging.md`, `Decompiling.md`.

The three templates are near-identical in structure; the content/character variants add domain-specific base classes and asset folders. Changes that apply to all three (e.g. csproj targets, path discovery, Directory.Build.props) usually need to be made in triplicate.

## Template mechanics — read before editing template files

Each template uses `sourceName` for whole-word renaming when instantiated:

| Template | `sourceName` | Becomes |
|---|---|---|
| ModTemplate | `ModTemplate` | user's project name |
| ContentModTemplate | `ContentMod` | user's project name |
| CharacterModTemplate | `CharMod` | user's project name |

When editing files under `content/`, every occurrence of the `sourceName` (in namespaces, class names, folder names, file names, `.json` manifest `id`, the `ModId` constant in `MainFile.cs`, etc.) will be substituted at instantiation time. Keep this consistent — do not introduce hardcoded references to one template's name inside a different template, and do not break the `Id.Entry.RemovePrefix()` convention (the sourceName is used as the prefix stripped to derive asset filenames).

Symbols declared in `template.json` and their replacement tokens:
- `{ModAuthor}` — required parameter, replaces into manifest `author`.
- `{PublicizeSts}` — bool, replaces into the csproj `<ItemGroup Condition="{PublicizeSts}">` controlling `Krafs.Publicizer`.
- `{NullableChecks}` — `enable`/`disable`, replaces into `<Nullable>`.

## How a generated mod project builds (architecture to know before touching csproj/props)

The build pipeline lives entirely inside each template's `.csproj` + `Sts2PathDiscovery.props` + `Directory.Build.props`. It is engineered to run against the developer's local Steam install of Slay the Spire 2.

1. **Path discovery** (`Sts2PathDiscovery.props`, imported first): auto-detects `Sts2Path` and `Sts2DataDir` per OS. Windows uses the Steam registry key for app `2868840` plus default `C:/Program Files (x86)/Steam/steamapps`. Linux/macOS use `~/.local/share/Steam/steamapps` / `~/Library/Application Support/Steam/steamapps` (macOS data lives inside `SlayTheSpire2.app/Contents/Resources`). If detection fails, the user sets `<Sts2Path>` in `Directory.Build.props`. `CheckDependencyPaths` target errors out if `$(Sts2DataDir)` is missing.
2. **Game references**: `sts2.dll` and `0Harmony.dll` are referenced directly from `$(Sts2DataDir)` with `<Private>false</Private>` — they must not be copied into the mod output.
3. **Publicizer** (`Krafs.Publicizer`, conditional on `{PublicizeSts}`): exposes private/protected non-virtual members of `sts2` at compile time. Off by default; enabling can break on game updates.
4. **BaseLib + analyzer**: `Alchyr.Sts2.BaseLib` (the community modding library all templates depend on) and `Alchyr.Sts2.ModAnalyzers` (provides the localization code-fix). The `<AdditionalFiles Include="*/localization/**/*.json"/>` entry feeds the analyzer.
5. **`UpdateDependencyVersions` target**: reads `.godot/mono/temp/obj/project.assets.json` after restore, extracts the resolved `Alchyr.Sts2.BaseLib` version, and rewrites the `min_version` for the `BaseLib` dependency in the mod manifest `.json`. This is why the manifest's BaseLib version should not be hand-edited — it is regenerated.
6. **`CopyToModsFolderOnBuild` target**: after every build, copies the `.dll`, `.pdb`, and manifest `.json` to `$(ModsPath)$(ProjectName)/`. This is what makes `Build` (hammer button) sufficient for code-only iteration.
7. **`GodotPublish` target**: on `Publish`, runs MegaDot headless (`--headless --export-pack "BasicExport"`) to produce the `.pck` containing assets/localization, then copies it to the mods folder. Requires `<GodotPath>` in `Directory.Build.props` to point at `MegaDot_v4.5.1-stable_mono_win64.exe` (or equivalent). The Godot version must not be newer than what shipped with the game, or the `.pck` won't load.

Two asset conventions matter when editing template code:
- Images live under `<ModName>/images/{card_portraits,powers,relics,charui}/` with optional `big/` siblings. The `StringExtensions.cs` in each template (`ImagePath`, `CardImagePath`, `BigCardImagePath`, `PowerImagePath`, `RelicImagePath`, `CharacterUiPath`, etc.) builds `res://<ModId>/images/...` paths and **falls back to a placeholder** (`card.png`, `power.png`, `relic.png`) when the named file is missing — so partial assets still work.
- Localization lives in `<ModName>/localization/eng/*.json` (`cards`, `relics`, `powers`, `ancients`, `characters`, `card_keywords`, `static_hover_tips`). Empty arrays are valid. The analyzer surfaces "Generate localization" code-fixes for new model classes.

## Code conventions inside the templates

- Entry point is `MainFile.cs` — a `partial class MainFile : Node` decorated with `[ModInitializer(nameof(Initialize))]`. `Initialize` creates a `Harmony(ModId)` and calls `harmony.PatchAll()`. The commented-out `ScriptManagerBridge.LookupScriptsInAssembly` line is intentional — uncomment only when the mod defines Godot scripts attached to scenes.
- `ModId` constant in `MainFile` is used both for Harmony ID and for `res://` resource paths (character/content templates expose `ResPath = $"res://{ModId}"`).
- `Logger` is `MegaCrit.Sts2.Core.Logging.Logger` (not Godot's `GD.Print`). Always log through `MainFile.Logger`.
- Content classes inherit a per-mod base (`CharModCard`, `ContentModCard`, etc.), which itself inherits the BaseLib `Custom*Model`. Per-mod bases are decorated with `[Pool(typeof(<Mod>CardPool))]` (or RelicPool) so subclasses don't repeat it, and they centralize image-path wiring via the `StringExtensions`.
- Character template's `CharMod` model inherits `PlaceholderCharacterModel`, which fills in base-game placeholders for any asset the developer hasn't overridden. Starting deck / relics / pools are concrete examples meant to be replaced.

## Mod manifest format (the `<ModName>.json`)

Fields: `id` (do not change — drives file loading), `name`, `author`, `description`, `version`, `min_game_version` (templates ship `0.107.0`), `has_pck`, `has_dll`, `dependencies` (templates declare `{"id": "BaseLib", "min_version": "3.3.0"}` — but see `UpdateDependencyVersions` above; the `min_version` is auto-synced to the resolved BaseLib), `affects_gameplay`. Cosmetic-only mods should set `affects_gameplay: false` (skips the multiplayer mod-check; setting it wrong causes desyncs).

## Reference: wiki pointers

When a task is about *using* these templates (rather than editing them), defer to `ModTemplate-StS2.wiki/`:
- `Setup.md` — full end-user setup, including the `Put solution and project in same directory` Rider requirement and the Godot path troubleshooting.
- `Common-Commands-Cookbook.md` — canonical patterns for `CanonicalVars`, `DynamicVars`, and the `*Cmd` builders (`DamageCmd`, `PowerCmd`, `CardCmd`, `CardPileCmd`, `CardSelectCmd`). Use this when implementing model behavior.
- `Modding-Basics.md` — manifest fields, mod file layout, where files go, branch/version policy.
- `Testing-and-Debugging.md` — dev console (open with any of `` ~ `` `` ` `` `*` `'` `Shift+8`), log locations (`%appdata%/SlayTheSpire2/logs/godot.log` on Windows), multiplayer local testing (`steam_appid.txt` containing `2868840`, `-fastmp host_standard` / `-fastmp join`), attaching Rider/VS debugger.
