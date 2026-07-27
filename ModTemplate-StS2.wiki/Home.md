This page explains how to get started with modding Slay the Spire 2 by setting up an initial project. Slay the Spire 2 is written in C# using the [Godot game engine](https://godotengine.org/) and is highly moddable. The first steps under Setup below include what you need to install and where to start.

Be aware that Slay the Spire 2 is currently in Early Access. As the game is frequently updated things are likely to change in ways that break mods. Any mods created during this period will need to be updated after each breaking change in the main game, and modding as a whole may be somewhat unstable.

This guide in a work in progress; given how new StS 2 modding is, we're actively working on making it easier to start modding.

# Setup
* [Initial Setup](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup)
* [Modding Basics](https://github.com/Alchyr/ModTemplate-StS2/wiki/Modding-Basics)
* [Decompiling](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling)
* [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text)
* [Testing and Debugging](https://github.com/Alchyr/ModTemplate-StS2/wiki/Testing-and-Debugging)

# Adding Content
* [Adding Cards](https://github.com/Alchyr/ModTemplate-StS2/wiki/Adding-Cards)
* [Adding Ancients](https://github.com/Alchyr/ModTemplate-StS2/wiki/Adding-Ancients)
* [Common Commands Cookbook](https://github.com/Alchyr/ModTemplate-StS2/wiki/Common-Commands-Cookbook)
* Check BaseLib for anything not listed here ([docs](https://alchyr.github.io/BaseLib-Wiki/docs/models/), [code](https://github.com/Alchyr/BaseLib-StS2/tree/master/Abstracts))

# More Modding
* [Shaders](https://github.com/Alchyr/ModTemplate-StS2/wiki/Shaders)
* [Replacing Base Game Text and Images](https://github.com/Alchyr/ModTemplate-StS2/wiki/Replacing-Base-Game-Text-&-Images)
* [Animating StS2 models with Blender](https://github.com/r2Nexus/The-Engineer/wiki/Animating-StS2-models-with-Blender-%E2%80%90-Introduction)

# Other Resources
* [BaseLib documentation](https://alchyr.github.io/BaseLib-Wiki/)
* [Harmony documentation](https://harmony.pardeike.net/articles/intro.html)
* [Godot getting started guide](https://docs.godotengine.org/en/stable/getting_started/step_by_step/index.html) (the GDScript stuff isn't as relevant but the overview of concepts is good)
* [Reading a stack trace](https://stackoverflow.com/questions/3988788/what-is-a-stack-trace-and-how-can-i-use-it-to-debug-my-application-errors)
* [Godot BBCode documentation](https://docs.godotengine.org/en/4.5/tutorials/ui/bbcode_in_richtextlabel.html) (used for localization)

# Rider Plugins
* [Localization Plugin](https://github.com/lamali292/STS2-Rider-Localization-Plugin)

# Getting Help
* The #sts2-modding channel on the [Slay the Spire Discord](https://discord.com/invite/SlayTheSpire) is the best place to ask for help

***
# Troubleshooting

If you're missing BaseLib features, check your project's NuGet dependencies and see if you're missing an update (alt+shift+7 to open in Rider).

<img width="921" height="171" alt="image" src="https://github.com/user-attachments/assets/2edd1184-efaf-4275-af38-ee1c29642145" />

Select an out-of-date dependency and click the arrow in a circle button to update.

<img width="887" height="181" alt="image" src="https://github.com/user-attachments/assets/dfee751a-8b33-4f2b-ac51-8475a76737fc" />

You're recommended to also keep the analyzer up-to-date.