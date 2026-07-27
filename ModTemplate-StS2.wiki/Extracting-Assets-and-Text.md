Slay the Spire 2 uses the Godot game engine, so if you're looking for base game assets and text, you will need to use [GDRE Tools](https://github.com/GDRETools/gdsdecomp).
* Download GDRE Tools from Github (on the [releases page](https://github.com/GDRETools/gdsdecomp/releases); the latest release should be fine; expand the Assets section and download the one that matches your operating system)
* Extract GDRE Tools to a folder and run the .exe
* From the RE Tools menu, select Recover Project
* Open `SlayTheSpire2.pck` from the StS 2 install directory
* Wait for it to process the project

You can now browse through all the games assets. Text can be found under the `localization` folder. Other assets (images, scenes, sound, etc.) are distributed throughout various folders; the names are mostly sensible. The game's code is here too, under `src/Core`, though looking at it in a decompiler will be more convenient; see [Decompiling](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling).

If you want to reference assets more easily in the future, consider extracting the pck's contents to a folder of your own.
