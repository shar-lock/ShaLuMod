Decompiling the game to look at its source code is an essential part of developing mods. Checking how the game implements cards/powers/relics/etc. is often the best way to learn how to implement something similar. Decompilation is also needed when writing patches.

Effectively using decompiled code requires understanding basic C# syntax and concepts. If you're struggling with that, taking a quick C# course may be beneficial (we often recommend Codeacademy for interactive courses or [Essential C#](https://essentialcsharp.com/home) for a text guide).

If you're looking to get the assets and text the game uses, see [Extracting Assets and Text](https://github.com/Alchyr/ModTemplate-StS2/wiki/Extracting-Assets-and-Text).

# Decompiling in Rider
<img width="674" height="333" alt="image" src="https://github.com/user-attachments/assets/29993375-5540-4658-bf3d-638a9afc21c6" align="right" />
The simplest and most convenient way to decompile the game is within Rider. Rider can flexibly look through the base game's code and libraries like BaseLib too.


<br/>
<br/>

With your project open in Rider:
* Press Shift 4 times to search everywhere (including the StS 2 code and BaseLib) by name. e.g. try searching for `CloakAndDagger`
  * Adjust the options in the search window if you need more precision
* When looking at code that uses anything from StS 2/BaseLib, Ctrl+Click on a type/method/etc. to jump to the decompiled code for it (and keep Ctrl+Clicking through as you explore)

When looking at decompiled code, you can use Rider functionality like Find Usages (in the right-click menu for types/methods/etc.) to see where things are used.

See the sections below for [other ways to decompile code](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling#decompiling-with-an-external-decompiler) and [searching in decompiled code](https://github.com/Alchyr/ModTemplate-StS2/wiki/Decompiling#searching-with-a-decompiler).

# Oddities in decompiled code
> [!IMPORTANT]  
> You need to understand this or decompiled code will confuse you and/or not work when you copy it.

Decompiled code sometimes has oddities because it cannot recover the exact original code. For example, here is code you might see from `WeakPower` in a decompiler:
```cs
public override IEnumerable<DynamicVar> CanonicalVars
{
  [OriginalAttributes(MethodAttributes.Family)] get
  {
    return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 0.75M));
  }
}
```

Here is how you'd typically write this code:
```cs
public override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(_damageDecrease, 0.75)];
```

What's different?
* The decompiled code has `[OriginalAttributes(MethodAttributes.Family)]` to indicate that the Publicizer process (used by the template to allow mods to access non-public class members) changed this property to be public. The original code did not have this attribute.
* The decompiled code uses more verbose syntax for the getter (`get` with two layers of `{{}}`). It's simpler to use `=>`.
* The decompiled code has a cast to `(IEnumerable<DynamicVar>)`. This is unnecessary.
* The decompiled code has a compiler-generated type `\u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>`. This is how the decompiler interprets the simple list syntax using `[]`.
* The decompiled code uses a string literal `"DamageDecrease"` in place of the constant field `_damageDecrease`, which is defined in the `WeakPower` class. References to constants get replaced by their value during compilation.
* The decompiled code writes `0.75M` to explicitly indicate that the value is of type `decimal`. This is unnecessary.

There are other oddities you'll see in decompiled code; these are just some common ones taken from Rider's decompiler. The details will differ depending on which decompiler you use.

### Other oddities and what they indicate
* `HistoryCourse historyCourse = this;` at the start of a method: normal code uses `this` throughout methods, but the compiler gets confused by that for async methods
* `// ISSUE: reference to compiler-generated method`: the decompiler wasn't able to process a lambda (inline function) properly (seems to be a Rider/dotPeek specific issue)

# Decompiling with an external decompiler
> This is an optional alternative to using Rider for decompiling.

External decompilers can also be used to look at the game's code. The advantage of external decompilers is that they may provide more human-readable code, advanced search capabilities, or other functionality that Rider doesn't. 

The game's code is in `sts2.dll`, which can be found by navigating to where Slay the Spire 2 is installed and then going to the `data_*` directory (e.g. `data_sts2_windows_x86_64`, will vary by platform).  `sts2.dll` will be in there (alongside many other dlls).

External decompilers you can try out include:
* ilspy (often the most human readable; has various forks, e.g. ilspy-vscode, avaloniailspy)
* dnspy
* dotPeek (basically the same as what Rider does; not much advantage over using Rider)

# Searching with a decompiler

Decompilers have powerful search and analysis tools to find and trace the route certain data and methods take. To search, once you have a decompiler installed and the game's assembly loaded:

1. Open up the search function (example screenshot in ILSpy) 
<img width="317" height="172" alt="image" src="https://github.com/user-attachments/assets/e5249d85-70c6-4428-8153-30f82c1a8a43" />

2. Search for your desired object. Change your filter to `Type` for specific classes (eg. `StrengthPower`), `Method` for methods (eg. `OnPlay`) or `Member` for attributes within classes (eg. `Rarity`).
<img width="653" height="271" alt="image" src="https://github.com/user-attachments/assets/0163d015-85f1-4a3b-8dd2-6028949cd803" />

3. If you wish to find what uses a given search hit has, you can analyze it by right clicking on the search result and clicking `Analyze`.
<img width="448" height="318" alt="image" src="https://github.com/user-attachments/assets/86acf06f-316f-46bb-a9c2-a95b6ec281a0" />

4. From here you can find usages of your search result. This is especially handy when trying to find examples of a given power.
<img width="510" height="203" alt="image" src="https://github.com/user-attachments/assets/362e205d-3f72-49e3-af64-55d02dce8c24" />
