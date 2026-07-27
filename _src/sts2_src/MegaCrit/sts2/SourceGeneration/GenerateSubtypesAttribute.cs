// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.SourceGeneration.GenerateSubtypesAttribute
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace MegaCrit.Sts2.SourceGeneration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
internal sealed class GenerateSubtypesAttribute : Attribute
{
  public DynamicallyAccessedMemberTypes DynamicallyAccessedMemberTypes { get; set; }
}
