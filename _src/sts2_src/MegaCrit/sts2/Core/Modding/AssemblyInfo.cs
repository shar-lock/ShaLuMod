// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.AssemblyInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public class AssemblyInfo
{
  public static Dictionary<Assembly, Mod>? ModMap { get; private set; }

  public static Assembly? BaseGame { get; private set; }

  public static Dictionary<Type, (Mod?, bool)>? MockTypes { get; set; }

  public static void Init()
  {
    AssemblyInfo.BaseGame = Assembly.GetExecutingAssembly();
    AssemblyInfo.ModMap = new Dictionary<Assembly, Mod>();
    foreach (Mod mod in (IEnumerable<Mod>) ModManager.Mods)
    {
      if (mod.state == ModLoadState.Loaded)
      {
        foreach (Assembly assembly in mod.assemblies)
          AssemblyInfo.ModMap[assembly] = mod;
      }
    }
  }

  public static Mod? ModForType(Type type, out bool isBaseGame)
  {
    (Mod, bool) tuple;
    if (AssemblyInfo.MockTypes != null && AssemblyInfo.MockTypes.TryGetValue(type, out tuple))
    {
      isBaseGame = tuple.Item2;
      return tuple.Item1;
    }
    if (AssemblyInfo.ModMap == null)
      throw new InvalidOperationException();
    Assembly assembly = type.Assembly;
    Mod mod;
    AssemblyInfo.ModMap.TryGetValue(assembly, out mod);
    isBaseGame = Assembly.op_Equality(AssemblyInfo.BaseGame, assembly);
    return mod;
  }

  public static void ClearForTests()
  {
    AssemblyInfo.BaseGame = (Assembly) null;
    AssemblyInfo.ModMap = (Dictionary<Assembly, Mod>) null;
  }
}
