This guide does not cover how to write shaders or the gdshader language. Refer to the [Godot Documentation](https://docs.godotengine.org/en/stable/tutorials/shaders/introduction_to_shaders.html) for more information.

# Loading Shaders

### Loading `.gdshader` Files at Runtime
You can load shaders at runtime like you would any other resource
```cs
Shader MyShader = GD.Load<Shader>("res://<MODNAME>/shaders/my_shader.gdshader");
```
where `<MODNAME>` is the name of your mod.

### Re-Using Base Game Shaders
You can also load exisiting shaders from the base game
```cs
Shader BaseGameShader = GD.Load<Shader>("res://shaders/hsv.gdshader"),
```

### Writing Shader Code in Strings
If your shader is especially short, or if you don't want your mod to include a `.pck` file, you can include the shader source code directly within your C# source code by setting the Shader's `Code` property to a string containing the shader code

```cs
private static readonly Shader TintShader = new()
{
    Code = """
            shader_type canvas_item;

            uniform vec4 tint_color : source_color = vec4(1.0);

            void fragment() {
                COLOR.rgb *= tint_color.rgb;
            }
        """
};
```

# Applying Shaders

> [!NOTE]
> When applying shaders to anything in the base game, you will almost certainly need to look at the decompiled godot project to find which specific nodes to apply shaders to, so take a look at [Decompiling](../Decompiling) if you haven't already.

If you want to apply a shader to a node without children that doesn't have an existing shader applied, the process is straightforward. Set the node's `Material` to a new `ShaderMaterial` with your shader

```cs
node.Material = new ShaderMaterial() { Shader = MyShader };
```

To set shader uniforms, use `SetShaderParameter`

```cs
var material = new ShaderMaterial() { Shader = MyShader };
material.SetShaderParameter("tint_color", new Color(0.5f, 0.5f, 0.5f));
node.Material = material;
```


If you want to apply a shader to a group of nodes, or a node that already has a shader applied, you can use a `CanvasGroup` or a `SubViewport`

> [!NOTE]
> When using any approach that involves changing the scene tree, be careful that you don't break any references to nodes that aren't referred to by a unique name (the % symbol in the godot editor).

### CanvasGroup
You can wrap an existing node (or nodes) in a `CanvasGroup` and then apply your shader to the `CanvasGroup`. When doing so, you need to read from the screen in your shader, see https://docs.godotengine.org/en/4.5/classes/class_canvasgroup.html and https://docs.godotengine.org/en/4.5/tutorials/shaders/screen-reading_shaders.html for more details. Because of this, vertex shaders won't work correctly. This approach also won't work if any children of the target node are `CanvasGroup`s. 

Here is an example that recolors the map legend:
```cs
var legend = (Control)mapScreen.FindChild("MapLegend");
var position = legend.GlobalPosition;

var material = new ShaderMaterial()
{
    Shader = GD.Load<Shader>("res://ShaderDemo/shaders/canvas_group_hsv.gdshader"),
};
material.SetShaderParameter("h", 0.45);
material.SetShaderParameter("s", 2.0);

var canvasGroup = new CanvasGroup { Material = material };

mapScreen.RemoveChild(legend);

mapScreen.AddChild(canvasGroup);
canvasGroup.AddChild(legend);

legend.GlobalPosition = position;
```


### SubViewport
You can wrap an existing node in a SubViewport and SubViewportContainer. If the node was centered, you'll need to adjust the position of the viewport (as it must be positioned by its top-left corner). A SubViewport will also result in noticeable degradation to the quality of fonts.

Here is an example that recolors cards. (This makes use of the fact that the default SubViewport size is 512x512)
```cs
Control cardContainer = card.GetNode<Control>("%CardContainer");

ShaderMaterial material = new() { Shader = TintShader };
material.SetShaderParameter("tint_color", new Color(0.25f, 0.25f, 0.25f));

SubViewport viewport = new() { TransparentBg = true };

SubViewportContainer viewportContainer = new()
{
    Material = material,
    MouseFilter = Control.MouseFilterEnum.Ignore,
    Position = -viewport.Size / 2,
    PivotOffset = -viewport.Size / 2,
    Size = viewport.Size,
};

card.RemoveChild(cardContainer);
cardContainer.Position = viewport.Size / 2;

card.AddChild(viewportContainer);
viewportContainer.AddChild(viewport);
viewport.AddChild(cardContainer);
```

### Screen-Wide Shaders (Post-Processing)
If you want to apply a shader to the entire screen, you can use a `CanvasLayer` with a `ColorRect` child set to the `Full Rect` anchor preset.

Here is an example that applies a hex pixelization shader to the entire screen
```cs
CanvasLayer canvasLayer = new();
ColorRect rect = new() { MouseFilter = Control.MouseFilterEnum.Ignore };
rect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

rect.Material = new ShaderMaterial()
{
    Shader = GD.Load<Shader>("res://ShaderDemo/shaders/hex_pixelization.gdshader"),
};

game.AddChild(canvasLayer);
canvasLayer.AddChild(rect);
```

All 3 examples above can be seen in full context here: https://github.com/Ind-E/StS2ShaderDemo
