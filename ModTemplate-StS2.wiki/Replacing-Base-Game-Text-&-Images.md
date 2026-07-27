> [!NOTE]
> Before following any guide on this page, create a template by following the steps in [Setup](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup) first. If you _only_ want to replace assets, use the `Empty Slay the Spire 2 Mod` template. Make sure to follow the section about [publishing](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild).


# Folder Layout
Before we begin, I'd like to make a note about folder layout, as the layout in this guide is different from how you would normally layout mods.

A typical mod layout looks like this:
```
ModName
├── ModName
│   ├── localization
│   │   ├── cards.json
│   │   └── ...
│   └── images
│       ├── card_portraits
│       └── ...
├── ModNameCode
│   ├── MainFile.cs
│   └── ...
├── ModName.csproj
├── ModName.json
├── ModName.sln
└── ...
```
Pay close attention to the nested `ModName` folders. In this setup, files inside of `localization` are merged with base game localization files, and files inside of `images` are ignored by the base game (your mod can still load them).

If you instead format your project like this (no nested `ModName` folders):
```
ModName
├── localization
│   ├── cards.json
│   └── ...
├── images
│   ├── card_portraits
│   └── ...
├── ModNameCode
│   ├── MainFile.cs
│   └── ...
├── ModName.csproj
├── ModName.json
├── ModName.sln
└── ...
```
Then files inside of `localization` or `images` will replace those from the base game. This is almost never what you want for `localization`, but placing files in the `images` folder can be used to replace assets in the base game, which is what we'll be doing in this guide.

# Replacing Static Images
Most images in Slay the Spire 2 are stored in 1 of 2 ways - either as individual pngs, or in an atlas. Some images have large and small variants, and some images have separate outline variants. 

In this guide, I will cover replacing relic images, as this encompasses all of those variants. Other static images (cards, potions, enchantments) can be replaced using the same steps. Characters, Enemies, and a couple other things are animated with [Spine](https://github.com/Alchyr/ModTemplate-StS2/wiki/Replacing-Base-Game-Text-%26-Images/#a-note-about-spine) and can't be replaced as easily.

The first step is to find where in the game files the relic images are located. You can do this by looking through the game's extracted assets (obtained by following the steps on the [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) page). 

Each relic has 3 different images. For akabeko,
- large image is at `images/relics/akabeko.png`
- small image is at `images/atlases/relic_atlas.sprites/akabeko.tres`
- small outline image is at `images/atlases/relic_outline_atlas.sprites/akabeko.tres`

Those last 2 are rectangular regions of atlases (located at `images/atlases/relic_atlas.png` and `images/atlases/relic_outline_atlas.png`)

So, in order to replace a relic's image in-game, we need to replace all 3 of these.

## Large Relic Images
I'll start with the large image, as that's the easiest. For assets that aren't stored in an atlas (e.g. enchantments), this would be the only step you need to do.

Start by opening up your project in the godot editor
  - If you have Rider open, you can click this button in the top right (If it doesn't appear, make sure your godot path in `Directory.Build.props` is correct)
  <img hspace="40" width="226" height="109" alt="The Start Godot Editor button in the top-right of Rider" src="https://github.com/user-attachments/assets/fd03a954-3aad-4a1b-8ea6-a09eeb798e64"/>

  - Otherwise, run godot, click `Import` and select your project folder

Next, place your image at the exact same filepath as the one you want to replace. Akabeko is at `images/relics/akabeko.png`, so I'll place a png at that location. I determined the dimensions of this png (256x256) by looking at the size of `akabeko.png` in the extracted sts2 assets.

<img hspace="40" width="270" height="320" src="https://github.com/user-attachments/assets/2992745c-e36e-4a30-9d6e-bc4bb12304e4" />

If you [publish](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) your project now, the image will show up in-game:

<img hspace="40" height="400" alt="image" src="https://github.com/user-attachments/assets/96051c06-00e6-4a27-b685-ab37ffcd2cac" />

However, this only replaces the large relic image shown when inspecting a single relic, and not the small one shown in the compendium or relic bar.

## Small Relic Images
Small relic images are stored in an atlas, and they require more steps to replace than the large image. Cards and potions are also stored in atlases, so you would adapt these steps if you want to replace those.

The small image files in the game end in `.tres` and not `.png`. Here, `.tres` is the extension for Godot's resource format. Godot Resources can be used for all sorts of things, in this case we want to create a resource that points to a png.

Start by placing your image somewhere in your project folder (in `ModName/images` is a good place for them, but exactly where doesn't matter). I determined the size of this image by looking at the last 2 numbers in this line of the `akabeko.tres` file (in this case, 81x69):
```
region = Rect2(1616, 536, 81, 69)
``` 

<img hspace="40" width="236" height="342" alt="image" src="https://github.com/user-attachments/assets/09039443-cddc-4c55-97fd-65556803226f" />

Then, double click your image in the File System pane to open it up in the Inspector pane on the right. Find and copy the `Load Path` field

<img hspace="40" height="500" alt="image" src="https://github.com/user-attachments/assets/8d93e32e-a446-4eea-b181-205d5e04478a" />

Next, at the exact same filepath as the image you want to replace, right click > `Create New` > `Resource` > search for `CompressedTexture2D`. Akabeko is at `images/atlases/relic_atlas.sprites/akabeko.tres`, so I'll create it there.

<img hspace="40" width="284" height="402" alt="image" src="https://github.com/user-attachments/assets/1b184fc2-d7ff-4f34-97fa-d366a163d79c" />

Finally, double click on the resource (`.tres` file) you created in the previous step, and paste the path you copied earlier into its `Load Path` field

If you [publish](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) your project now, the small image will show up in-game:

<img width="247" height="141" alt="image" src="https://github.com/user-attachments/assets/0d69d144-da5f-44c1-b71f-c1c8dc9bfb19" />

It's a bit hard to tell in this image, but the outline is still that of akabeko (there's a black blob on the right side). 

The last step is to replace the outline image. The process for the outline image is exactly the same as replacing the small image - just use a different path. In akabeko's case, it would be `images/atlases/relic_outline_atlas.sprites/akabeko.tres` for the outline. To create your outline image, create an all-white version of your small relic image that's slightly larger.

<hr/>
<details>
<summary><h3>Old Replacing Card Images Guide</h3></summary>

- Open up your project folder in the godot editor
  - If using rider, you can click this button in the top right (If it doesn't appear, make sure your godot path in `Directory.Build.props` is correct)
<p>
  <img hspace="60" width="226" height="109" alt="The Start Godot Editor button in the top-right of Rider" src="https://github.com/user-attachments/assets/fd03a954-3aad-4a1b-8ea6-a09eeb798e64"/>
</p>

- Place your custom image(s) somewhere in your project folder (in `ModName/images` is a good place for them)
  - Image dimensions should be a multiple of 500x380 (e.g. 250x190 or 1000x760)

- Open your custom image in the File Editor pane of the godot editor and double click it to open it up in the inspector panel on the right. Find and copy the `Load Path` field
<p>
  <img hspace="30" width="472" height="456" alt="Godot editor inspector pane showing an image" src="https://github.com/user-attachments/assets/df14adab-594e-4b86-acba-3ee4d4d3cd6e" />
</p>

- Recreate the same folder structure as the base game card you want to replace the art of
  - Example: `images/atlases/card_atlas.sprites/ironclad/strike_ironclad.tres`
  - Look at [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) to see the base game folder structure for yourself

- In the godot editor, right click > `Create New` > `Resource` > select `CompressedTexture2D`
<p>
  <img hspace="30" width="641" height="280" alt="Godot editor showing the folder, then selecting Resource" src="https://github.com/user-attachments/assets/13da13c7-c28e-49bd-8f13-2b3dd2889284"/>
</p>

- Double click on the resource (`.tres` file) you created in a previous step, and paste the path you copied earlier into its `Load Path` field
<p>
  <img hspace="30" width="472" height="420" alt="Godot editor inspector pane showing an empty resource" src="https://github.com/user-attachments/assets/dc9793d4-38ce-4fae-aa0f-5dc9bbbed009" />
</p>


- Finally, [publish](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) your project, and your changes should appear in-game

</details>
<hr/>

# Replacing Text/Localization

Since mods are loaded after the base game, using the same localization keys as the base game will cause your mod to replace certain parts of base game localization during the merging process (using the first layout mentioned at the top of this page).

For example, creating a file at `ModName/localization/eng/cards.json` with the following contents will change the title and description of ironclad's starter strike:

```json
{
    "STRIKE_IRONCLAD.title": "Strike 2",
    "STRIKE_IRONCLAD.description": "Electric Boogaloo"
}
```
Make sure to [publish](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup#publishbuild) so that your changes show up in-game.

This pattern can be used to change any text in the base-game, look at [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) to be able to view all base game localization.

# A Note About Spine
StS2 uses Spine for Character, Enemy, and a couple of other animations. The full version of Spine costs [~$400](https://en.esotericsoftware.com/spine-purchase), so editing animations directly in Spine is too expensive for most modders. You do have other options, though. 

For recolors/reskins of existing models, You can edit pieces of the atlas. For Example, you could replace the file at `animations/characters/silent/silent.png` with this:

<img width="400" alt="translent" src="https://github.com/user-attachments/assets/1e10d393-24f5-49cc-8bbb-e9f5d10b0ead" />

This doesn't allow much freedom for changing the shape of each piece, though. 

If you want to create your own animation, as long as you can load it in the godot editor, you can load it in-game. You can look up godot tutorials for 2d skeleton animation, or even use a 3d model and use a viewport to render it in 2d. r2Nexus has a guide on [Animating StS2 models with Blender](https://github.com/r2Nexus/The-Engineer/wiki/Animating-StS2-models-with-Blender-%E2%80%90-Introduction).

For replacing existing models, you'll likely have to [Harmony Patch](https://harmony.pardeike.net/articles/patching.html) to get the game to load your animation. For creating your own character, look at the [Baselib Wiki](https://alchyr.github.io/BaseLib-Wiki/docs/scenes/creature-visuals.html).
