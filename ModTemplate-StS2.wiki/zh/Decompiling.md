反编译游戏以查看其源代码是模组开发中不可或缺的一部分。查看游戏是如何实现卡牌/能力/遗物等内容的，通常是学习如何实现类似功能的最佳方式。在编写补丁时也需要进行反编译。

要有效地使用反编译后的代码，需要理解基本的 C# 语法和概念。如果你对此感到吃力，学习一门简短的 C# 课程可能会有所帮助（我们通常推荐 Codeacademy 的交互式课程，或 [Essential C#](https://essentialcsharp.com/home) 作为文字教程）。

如果你想获取游戏使用的资源和文本，请参阅 [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text)。

# 在 Rider 中反编译
<img width="674" height="333" alt="image" src="https://github.com/user-attachments/assets/29993375-5540-4658-bf3d-638a9afc21c6" align="right" />
最简单便捷的反编译游戏的方式是在 Rider 中进行。Rider 能够灵活地查看基础游戏的代码以及 BaseLib 等库。


<br/>
<br/>

在 Rider 中打开你的项目后：
* 连按 Shift 四次可以按名称搜索所有内容（包括 StS 2 代码和 BaseLib）。例如，尝试搜索 `CloakAndDagger`
  * 如果需要更精确的结果，可以调整搜索窗口中的选项
* 当你查看使用 StS 2/BaseLib 中任何内容的代码时，按住 Ctrl+点击 类型/方法等即可跳转到其反编译代码（并在你探索时持续 Ctrl+点击 进行跳转）

在查看反编译代码时，你可以使用 Rider 的功能，例如 Find Usages（在类型/方法等的右键菜单中），查看它们在哪里被使用。

关于[反编译代码的其他方式](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling#decompiling-with-an-external-decompiler)以及[在反编译代码中搜索](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling#searching-with-a-decompiler)，请参阅下方章节。

# 反编译代码中的异常现象
> [!IMPORTANT]  
> 你需要理解这一点，否则反编译代码会让你困惑，和/或在你复制它时无法正常工作。

反编译代码有时会出现异常现象，因为它无法完全还原原始代码。例如，下面是你在反编译器中可能看到的来自 `WeakPower` 的代码：
```cs
public override IEnumerable<DynamicVar> CanonicalVars
{
  [OriginalAttributes(MethodAttributes.Family)] get
  {
    return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 0.75M));
  }
}
```

下面是你通常会这样编写此代码的方式：
```cs
public override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(_damageDecrease, 0.75)];
```

它们有何不同？
* 反编译代码中的 `[OriginalAttributes(MethodAttributes.Family)]` 用于指示 Publicizer 流程（模板用它来允许模组访问非公共类成员）已将该属性改为了公共的。原始代码并没有这个特性。
* 反编译代码对 getter 使用了更冗长的语法（带两层 `{{}}` 的 `get`）。使用 `=>` 会更简洁。
* 反编译代码中有一个到 `(IEnumerable<DynamicVar>)` 的转换。这是不必要的。
* 反编译代码中有一个编译器生成的类型 `\u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>`。这是反编译器对使用 `[]` 的简单列表语法的解读方式。
* 反编译代码使用字符串字面量 `"DamageDecrease"` 来代替常量字段 `_damageDecrease`，后者定义在 `WeakPower` 类中。对常量的引用在编译过程中会被替换为它们的值。
* 反编译代码写为 `0.75M` 以显式表明该值的类型为 `decimal`。这是不必要的。

反编译代码中还有其他异常现象；这里只是从 Rider 的反编译器中取的一些常见示例。具体细节会因你使用的反编译器而异。

### 其他异常现象及其含义
* 方法开头的 `HistoryCourse historyCourse = this;`：正常的代码会在整个方法中使用 `this`，但编译器在处理异步方法时会产生这种困惑
* `// ISSUE: reference to compiler-generated method`：反编译器无法正确处理 lambda（内联函数）（这似乎是 Rider/dotPeek 特有的问题）

# 使用外部反编译器进行反编译
> 这是可选的 Rider 反编译替代方案。

也可以使用外部反编译器来查看游戏代码。外部反编译器的优势在于它们可能提供更具人类可读性的代码、高级搜索功能或其他 Rider 不具备的功能。

游戏代码位于 `sts2.dll` 中，可通过导航到杀戮尖塔 2（StS2）的安装位置后进入 `data_*` 目录找到（例如 `data_sts2_windows_x86_64`，具体因平台而异）。`sts2.dll` 就在其中（与许多其他 dll 并存）。

你可以尝试的外部反编译器包括：
* ilspy（通常最具人类可读性；有各种分支，例如 ilspy-vscode、avaloniailspy）
* dnspy
* dotPeek（基本与 Rider 所做的相同；相比使用 Rider 没什么优势）

# 使用反编译器进行搜索

反编译器拥有强大的搜索和分析工具，可用于查找并追踪特定数据和方法所经过的路径。要搜索，一旦你安装了反编译器并加载了游戏的程序集：

1. 打开搜索功能（以 ILSpy 中的截图为例）
<img width="317" height="172" alt="image" src="https://github.com/user-attachments/assets/e5249d85-70c6-4428-8153-30f82c1a8a43" />

2. 搜索你想要的对象。将过滤器更改为 `Type` 以查找具体的类（例如 `StrengthPower`），更改为 `Method` 以查找方法（例如 `OnPlay`），更改为 `Member` 以查找类内的属性（例如 `Rarity`）。
<img width="653" height="271" alt="image" src="https://github.com/user-attachments/assets/0163d015-85f1-4a3b-8dd2-6028949cd803" />

3. 如果你想查找某个搜索结果有哪些用途，可以右键点击该搜索结果并点击 `Analyze` 来进行分析。
<img width="448" height="318" alt="image" src="https://github.com/user-attachments/assets/86acf06f-316f-46bb-a9c2-a95b6ec281a0" />

4. 从这里你可以找到搜索结果的各种用途。这在尝试查找某个能力的使用示例时尤为方便。
<img width="510" height="203" alt="image" src="https://github.com/user-attachments/assets/362e205d-3f72-49e3-af64-55d02dce8c24" />
