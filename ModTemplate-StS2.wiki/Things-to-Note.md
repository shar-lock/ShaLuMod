If creating scenes in Godot and attaching scripts from your mod, you will need to add this to your mod's initialization.

```c#
var assembly = Assembly.GetExecutingAssembly();
Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
```