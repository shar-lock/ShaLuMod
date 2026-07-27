// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.IModManagerFileIo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System.IO;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public interface IModManagerFileIo
{
  string[] GetFilesAt(string path);

  string[] GetDirectoriesAt(string path);

  bool FileExists(string path);

  bool DirectoryExists(string path);

  Stream OpenStream(string path, FileAccess.ModeFlags mode);
}
