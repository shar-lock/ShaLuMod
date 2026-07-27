// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.AssemblyHasher
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Buffers.Binary;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

#nullable disable
namespace MegaCrit.Sts2.Core.Debug;

public static class AssemblyHasher
{
  private static int? _mainAssemblyHash;

  public static int GetMainAssemblyHash()
  {
    if (AssemblyHasher._mainAssemblyHash.HasValue)
      return AssemblyHasher._mainAssemblyHash.Value;
    if (OS.HasFeature("editor"))
    {
      Log.Info("Assembly hashing disabled in editor");
      AssemblyHasher._mainAssemblyHash = new int?(0);
      return 0;
    }
    Assembly assembly = typeof (AssemblyHasher).Assembly;
    try
    {
      using (FileStream fileStream = new FileStream(assembly.Location, (FileMode) 3, (FileAccess) 1, (FileShare) 3))
        AssemblyHasher._mainAssemblyHash = new int?(BinaryPrimitives.ReadInt32LittleEndian(ReadOnlySpan<byte>.op_Implicit(SHA1.HashData((Stream) fileStream))));
    }
    catch (Exception ex)
    {
      Log.Warn($"Could not read main assembly {assembly} from {assembly.Location}. Exception: {ex}");
      AssemblyHasher._mainAssemblyHash = new int?(0);
    }
    return AssemblyHasher._mainAssemblyHash.Value;
  }
}
