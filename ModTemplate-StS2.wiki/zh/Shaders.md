本指南不涵盖如何编写着色器或 gdshader 语言。更多信息请参阅 [Godot 文档](https://docs.godotengine.org/en/stable/tutorials/shaders/introduction_to_shaders.html)。

# 加载着色器

### 在运行时加载 `.gdshader` 文件
你可以像加载其他资源一样在运行时加载着色器
```cs
Shader MyShader = GD.Load<Shader>("res://<MODNAME>/shaders/my_shader.gdshader");
```
其中 `<MODNAME>` 是你的模组名称。

### 复用基础游戏着色器
你也可以从基础游戏中加载已有的着色器
```cs
Shader BaseGameShader = GD.Load<Shader>("res://shaders/hsv.gdshader"),
```

### 在字符串中编写着色器代码
如果你的着色器特别短，或者你不想让模组包含 `.pck` 文件，可以通过将 Shader 的 `Code` 属性设置为包含着色器代码的字符串，把着色器源代码直接写在 C# 源代码中

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

# 应用着色器

> [!NOTE]
> 在向基础游戏中的任何对象应用着色器时，你几乎必然需要查看反编译后的 godot 项目，以确定具体要把着色器应用到哪些节点，所以如果你还没有看过 [反编译](../Decompiling) 的话，请先看一下。

如果你想为一个没有子节点、且尚未应用着色器的节点应用着色器，过程非常简单。将该节点的 `Material` 设置为一个新的 `ShaderMaterial`，并使用你的着色器即可

```cs
node.Material = new ShaderMaterial() { Shader = MyShader };
```

要设置着色器的 uniform 参数，请使用 `SetShaderParameter`

```cs
var material = new ShaderMaterial() { Shader = MyShader };
material.SetShaderParameter("tint_color", new Color(0.5f, 0.5f, 0.5f));
node.Material = material;
```


如果你想对一组节点应用着色器，或者对已经应用了着色器的节点再次应用着色器，可以使用 `CanvasGroup` 或 `SubViewport`

> [!NOTE]
> 当使用任何会修改场景树的方式时，要小心不要破坏对那些并非通过唯一名称（即 godot 编辑器中的 % 符号）引用的节点的引用。

### CanvasGroup
你可以把现有的一或多个节点包裹在 `CanvasGroup` 中，然后把着色器应用到 `CanvasGroup` 上。这样做时，你需要在着色器中从屏幕读取数据，详见 https://docs.godotengine.org/en/4.5/classes/class_canvasgroup.html 和 https://docs.godotengine.org/en/4.5/tutorials/shaders/screen-reading_shaders.html。正因如此，顶点着色器无法正常工作。如果目标节点的任何子节点本身是 `CanvasGroup`，此方法也不起作用。

下面是一个为地图图例重新着色的示例：
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
你可以把现有节点包裹在 SubViewport 和 SubViewportContainer 中。如果原节点是居中的，你需要调整视口的位置（因为视口必须通过其左上角来定位）。SubViewport 还会导致字体质量明显下降。

下面是一个为卡牌重新着色的示例。（这里利用了 SubViewport 默认尺寸为 512x512 这一事实）
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

### 全屏着色器（后处理）
如果你想对整个屏幕应用着色器，可以使用一个 `CanvasLayer`，并为其添加一个设置为 `Full Rect` 锚点预设的 `ColorRect` 子节点。

下面是一个对整个屏幕应用六边形像素化着色器的示例
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

上述 3 个示例的完整上下文可以在这里查看：https://github.com/Ind-E/StS2ShaderDemo
