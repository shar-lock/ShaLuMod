// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.NetTypeCache`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public class NetTypeCache<[DynamicallyAccessedMembers] TBase> where TBase : class
{
  private readonly Dictionary<Type, int> _typeToId = new Dictionary<Type, int>();
  private readonly List<Type> _idToType;

  public int Count => this._idToType.Count;

  public NetTypeCache(List<Type> types)
  {
    this._idToType = ContentSorter<string>.Sort((IEnumerable<Type>) types, (Func<Type, string>) (t => t.Name)).Select<ContentSorter<string>.Item, Type>((Func<ContentSorter<string>.Item, Type>) (i => i.type)).ToList<Type>();
    for (int index = 0; index < this._idToType.Count; ++index)
    {
      Type key = this._idToType[index];
      if (!((IEnumerable<Type>) key.GetInterfaces()).Contains<Type>(typeof (TBase)))
        throw new InvalidOperationException($"Type {this._idToType[index]} does not implement interface {typeof (TBase)}!");
      this._typeToId[key] = index;
    }
  }

  public int TypeToId<T>() where T : TBase => this.TypeToId(typeof (T));

  public int TypeToId(Type type) => this._typeToId[type];

  public bool TryGetTypeFromId(int id, out Type? type)
  {
    if (id < 0 || id >= this._idToType.Count)
    {
      type = (Type) null;
      return false;
    }
    type = this._idToType[id];
    return true;
  }
}
