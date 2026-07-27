// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.ContentSorter`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class ContentSorter<TIdType> where TIdType : IComparable<TIdType>
{
  public static List<ContentSorter<
  #nullable disable
  TIdType>.Item> Sort(
    #nullable enable
    IEnumerable<Type> types,
    Func<Type, TIdType> getId,
    bool affectsGameplayAtEnd = true)
  {
    if (AssemblyInfo.ModMap == null)
      throw new InvalidOperationException("ContentSorter called before AssemblyInfo was initialized. This is not allowed.");
    List<ContentSorter<TIdType>.Item> items = new List<ContentSorter<TIdType>.Item>();
    foreach (Type type in types)
    {
      bool isBaseGame;
      Mod mod = AssemblyInfo.ModForType(type, out isBaseGame);
      if (mod == null && !isBaseGame)
        Log.Error($"Attempting to register type {type} in assembly {type.Assembly}, but it is not associated with any mod! You may need to call {"ModManager"}.{"AssociateAssemblyWithMod"} to manually register the assembly. Type sorting may break because of this, causing errors in multiplayer.");
      items.Add(new ContentSorter<TIdType>.Item()
      {
        type = type,
        id = getId(type),
        mod = mod
      });
    }
    ContentSorter<TIdType>.Sort(items, affectsGameplayAtEnd);
    return items;
  }

  public static List<ContentSorter<
  #nullable disable
  TIdType>.Item> Sort(
  #nullable enable
  List<ContentSorter<
  #nullable disable
  TIdType>.Item> items, bool affectsGameplayAtEnd = true)
  {
    items.Sort((Comparison<ContentSorter<TIdType>.Item>) ((p1, p2) =>
    {
      if (affectsGameplayAtEnd)
      {
        bool flag = p1.mod?.manifest?.affectsGameplay ?? true;
        int num = (p2.mod?.manifest?.affectsGameplay ?? true).CompareTo(flag);
        if (num != 0)
          return num;
      }
      int num1 = p1.id.CompareTo(p2.id);
      if (num1 != 0)
        return num1;
      if (p1.mod != null && p2.mod == null)
        return 1;
      if (p1.mod == null && p2.mod != null)
        return -1;
      if (p1.mod == null && p2.mod == null)
        return 0;
      int num2 = string.CompareOrdinal(p1.mod.manifest.id, p2.mod.manifest.id);
      if (num2 != 0)
        return num2;
      int num3 = string.CompareOrdinal(p1.type.FullName, p2.type.FullName);
      if (num3 != 0)
        return num3;
      int num4 = string.CompareOrdinal(p1.type.Assembly.FullName, p2.type.Assembly.FullName);
      return num4 != 0 ? num4 : 0;
    }));
    return items;
  }

  public struct Item
  {
    public 
    #nullable enable
    Type type;
    public TIdType id;
    public Mod? mod;

    public override string ToString() => $"{this.type} {this.id} {this.mod?.manifest?.id}";
  }
}
