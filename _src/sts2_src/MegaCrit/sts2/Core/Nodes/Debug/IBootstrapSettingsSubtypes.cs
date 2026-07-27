// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.IBootstrapSettingsSubtypes
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

public static class IBootstrapSettingsSubtypes
{
  private static readonly Type[] _subtypes = Array.Empty<Type>();

  public static int Count => 0;

  public static IReadOnlyList<Type> All
  {
    get => (IReadOnlyList<Type>) IBootstrapSettingsSubtypes._subtypes;
  }

  [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2063", Justification = "The list only contains types stored with the correct DynamicallyAccessedMembers attribute, enforced by source generation.")]
  [return: DynamicallyAccessedMembers]
  public static Type Get(int i) => IBootstrapSettingsSubtypes._subtypes[i];
}
