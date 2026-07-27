如果在 Godot 中创建场景并附加你模组中的脚本，你需要在模组的初始化中添加以下内容。

```c#
var assembly = Assembly.GetExecutingAssembly();
Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(assembly);
```
