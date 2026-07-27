This guide is for adding new Ancients to the game.

Adding an Ancient requires:
- a `CustomAncientModel` class for your ancient, providing asset file paths and the relic pool
- corresponding localization
- a scene for the background of the ancient event
- an icon for run history and dialogues
- a map icon with an outline

The ancient needs at least three relics with the `RelicRarity.Ancient` rarity, added to the `EventRelicPool` pool. [Here](https://alchyr.github.io/BaseLib-Wiki/docs/models/custom-relic.html) is the Baselib wiki page for `CustomRelic`.


# Creating the file

First up, create the class file of your new ancient. It can be within any folder of the project, you can organize your files how you like it.

<img width="569" height="211" alt="image" src="https://github.com/user-attachments/assets/b96a9370-0a3f-4d69-aac6-026c98f27c71" />

<p> </p>

Then, make your new class extend BaseLib's CustomAncientModel. Doing this will show errors for missing information that we will add shortly. It should look like this:

```C#
public class TestAncient : CustomAncientModel
{

}
```



# Option Pools
This will determine what relics your ancient offers.

Override `MakeOptionPools` in your ancient class and provide an option pool like so:

```C#
protected override OptionPools MakeOptionPools => new OptionPools(
    [
        AncientOption<Nunchaku>(),
        AncientOption<Lantern>(),
        AncientOption<ArtOfWar>()
        //more relic options
    ]
);
```

Three options from that pool will be selected randomly to be offered by your ancient.

You can also optionally make multiple pools by providing up to 3 arrays to the OptionPools constructor. If 2 pools are provided, 2 options will be selected from the first pool and one from the second. If 3 pools are provided, each option will be selected from a different pool. Base game ancients typically either have one big pool (Nonupeipe, Tanx) or three separate pools (Tezcatara, Orobas, Pael, Vakuu). Darv has two pools, one with all his relics and one with only Dusty Tome (with logic to only offer it half the time).

# Setting when the Ancient spawns

To change which act your ancient can spawn, override the `IsValidForAct` method (this example makes it spawn only in act 2):
```C#
public override bool IsValidForAct(ActModel act)
{
    return act.ActNumber() == 2;
}
```

# Localization

First, create the `ancients.json` file. It **must** be in the right folder : `[YourModName]/localization/eng`(changing `eng` to whatever language you're writing localization in). Create those folders if needed.

<img width="521" height="306" alt="image" src="https://github.com/user-attachments/assets/18f0ccba-77dd-4929-a964-5c8f7f04e2ec" />

<p> </p>

We can now add localization for our ancient. Go back to your ancient class file, and `right click > Show context actions > Generate localization > any of the three options`

<img width="756" height="60" alt="image" src="https://github.com/user-attachments/assets/beebd179-8cb5-43c5-ba15-c58144d1fa8f" />

<p> </p>

Cut the generated text and paste it into `ancients.json`, between the brackets, and save the file. This should clear the error shown in the class file. If it doesn't, check the names and location of the folders.

The generated localization will add the minimal amount of text to your ancient: name, epithet, first dialogue (one line), and generic repeatable dialogue (one line). To add more, check on the [BaseLib wiki](https://alchyr.github.io/BaseLib-Wiki/docs/extras/ancient-dialogue.html).

# Assets
For all these assets, it may be helpful to use [GDRE Tools to extract base game assets](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text) so you can see how the base game ancients work.
## Map icon

The map icon is a 278x278 image. It has to be accompanied by a separate outline image of the same size (similarly to relics). The default name and location is `images/packed/map/ancients/ancient_node_[modID]-[ancientclassname].png` and `ancient_node_[modID]-[ancientclassname]_outline.png` (all lowercase).

You can also provide another path using the `CustomMapIconPath` and `CustomMapIconOutlinePath` properties in your ancient's class.

## Run history icon

This icon will be used in the run history as well as in dialogues.

It's a 88x88 image, and also has to have a separate outline image.

The default name and location is `images/ui/run_history/[modID]-[ancientclassname].png` and `[modID]-[ancientclassname]_outline.png` (all lowercase).

You can also provide another path using the `CustomRunHistoryIconPath` and `CustomRunHistoryIconOutlinePath` properties in your ancient's class.


## Scene

You will need to make a Godot scene to be used as a background for your ancient. Typically it will only contain a background image (and this guide will only cover that), but Godot scenes also allow adding effects, particles and animations.

Open Godot or Megadot, and open the `project.godot` file from your mod's folder. You should now be able to open your mod as a Godot project.

Create a new 2d scene that will serve as the background of your ancient's event. The default path and name is `scenes/events/background_scenes/[modID]-[ancientclassname].tscn` (all lowercase). You can also override the `CustomScenePath` property in your ancient's class to provide another path.

<img width="420" height="300" alt="image" src="https://github.com/user-attachments/assets/ab84379d-c3aa-4470-a05c-957ac3804953" />
<img width="290" height="300" alt="image" src="https://github.com/user-attachments/assets/c4b08891-aa82-4f18-8b8b-092e09b7e9c4" />

<p> </p>

Change the root node into a control node, and rename it to just your ancient's name.

<img width="240" height="300" alt="image" src="https://github.com/user-attachments/assets/28c3e3fa-95e3-4d6d-9cc6-7513c19e8d3c"/>
&nbsp; &nbsp; &nbsp;
<img width="380" height="300" alt="image" src="https://github.com/user-attachments/assets/b86778e5-a376-445a-a18b-c9a42ecd0831" />

<p> </p>

Add a child node to the root node, with the `TextureRect` type.

<img width="377" height="210" alt="image" src="https://github.com/user-attachments/assets/51bc083a-1e49-4da2-821d-aeef5cb735aa" />

<p> </p>

Add your chosen image to your mod's files (base game images are 2560x1200), and select the image as the child node's texture.

<img width="300" height="124" alt="image" src="https://github.com/user-attachments/assets/409a3824-3820-4704-b3f7-23c94f07ac92" />
 
<p> </p>

To align the texture properly, go to `Project > Project Settings > Display > Window` and change viewport width and height to 1920 and 1080 respectively. This will change the blue rectangle representing the game's window in your scene to be the size the game expects. You can then center the image by dragging it.

Save the scene, and you're done! With everything in this guide done, your ancient should be able to spawn and work properly.