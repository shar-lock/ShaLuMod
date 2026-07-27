// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.ReflectionHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Modding;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class ReflectionHelper
{
  public const BindingFlags allAccessLevels = (BindingFlags) 52;
  private static Type[]? _allTypes;
  private static Type[]? _modTypes;

  public static bool SubtypesAvailable => true;

  public static Type[] AllTypes
  {
    get
    {
      if (ReflectionHelper._allTypes == null)
        ReflectionHelper._allTypes = Assembly.GetAssembly(typeof (ReflectionHelper)).GetTypes();
      return ReflectionHelper._allTypes;
    }
  }

  public static Type[] ModTypes
  {
    get
    {
      if (ModManager.State == ModManagerState.None)
        throw new InvalidOperationException("ModManager is not finished initializing! ReflectionHelper.ModTypes cannot be called before that.");
      if (ReflectionHelper._modTypes == null)
        ReflectionHelper._modTypes = ModManager.GetLoadedMods().SelectMany<Mod, Assembly>((Func<Mod, IEnumerable<Assembly>>) (m => (IEnumerable<Assembly>) m.assemblies)).SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (a => (IEnumerable<Type>) a.GetTypes())).ToArray<Type>();
      return ReflectionHelper._modTypes;
    }
  }

  public static IEnumerable<Type> GetSubtypes(Type parentType)
  {
    return ReflectionHelper.GetSubtypesFromList((IList<Type>) ReflectionHelper.AllTypes, parentType);
  }

  public static IEnumerable<Type> GetSubtypesInMods(Type parentType)
  {
    return ReflectionHelper.GetSubtypesFromList((IList<Type>) ReflectionHelper.ModTypes, parentType);
  }

  public static IEnumerable<Type> GetSubtypesFromAssembly(Assembly assembly, Type parentType)
  {
    return ReflectionHelper.GetSubtypesFromList((IList<Type>) assembly.GetTypes(), parentType);
  }

  private static IEnumerable<Type> GetSubtypesFromList(IList<Type> list, Type parentType)
  {
    return list.Where<Type>((Func<Type, bool>) (type => (object) type != null && !type.IsAbstract && !type.IsInterface && ReflectionHelper.InheritsOrImplements(type, parentType)));
  }

  public static IEnumerable<Type> GetSubtypes<T>() where T : class
  {
    return ReflectionHelper.GetSubtypes(typeof (T));
  }

  public static IEnumerable<Type> GetSubtypesInMods<T>() where T : class
  {
    return ReflectionHelper.GetSubtypesInMods(typeof (T));
  }

  public static bool InheritsOrImplements([DynamicallyAccessedMembers] Type derived, Type baseType)
  {
    return derived.IsSubclassOf(baseType) || ((IEnumerable<Type>) derived.GetInterfaces()).Contains<Type>(baseType);
  }
}
