// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Test.MockCloudGodotFileIo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Test;

public class MockCloudGodotFileIo(string saveDir) : MockGodotFileIo(saveDir), ICloudSaveStore, ISaveStore
{
  public bool hasUserEnabledCloudSync = true;

  public bool HasCloudFiles() => this._files.Any<KeyValuePair<string, MockGodotFileIo.File>>();

  public void ForgetFile(string path)
  {
    this.CanonicalizePath(ref path);
    MockGodotFileIo.File file;
    if (!this._files.TryGetValue(path, out file))
      throw new InvalidOperationException($"No file at {path}!");
    file.forgotten = true;
  }

  public bool IsFilePersisted(string path)
  {
    this.CanonicalizePath(ref path);
    MockGodotFileIo.File file;
    if (!this._files.TryGetValue(path, out file))
      throw new InvalidOperationException($"No file at {path}!");
    return !file.forgotten;
  }

  public void BeginSaveBatch()
  {
  }

  public void EndSaveBatch()
  {
  }

  public bool HasUserEnabledCloudSync() => this.hasUserEnabledCloudSync;
}
