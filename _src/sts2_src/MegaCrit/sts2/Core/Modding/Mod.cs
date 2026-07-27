// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.Mod
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Localization;
using System.Collections.Generic;
using System.Reflection;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public class Mod
{
  public ModSource modSource;
  public required string path;
  public ModLoadState state;
  public ModManifest? manifest;
  public SemanticVersion? version;
  public List<Assembly> assemblies = new List<Assembly>();
  public List<LocString>? errors;
  public ulong? workshopId;
}
