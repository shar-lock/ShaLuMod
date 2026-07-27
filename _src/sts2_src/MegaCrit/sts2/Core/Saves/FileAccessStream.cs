// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.FileAccessStream
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.IO;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class FileAccessStream : Stream
{
  private readonly FileAccess _file;
  private readonly FileAccess.ModeFlags _flags;
  private readonly string _filePath;

  public override bool CanRead
  {
    get
    {
      return this._file.IsOpen() && ((Enum) (object) this._flags).HasFlag((Enum) (object) (FileAccess.ModeFlags) 1L);
    }
  }

  public override bool CanSeek => this._file.IsOpen();

  public override bool CanWrite
  {
    get
    {
      return this._file.IsOpen() && ((Enum) (object) this._flags).HasFlag((Enum) (object) (FileAccess.ModeFlags) 2L);
    }
  }

  public override long Length => (long) this._file.GetLength();

  public override long Position
  {
    get => (long) this._file.GetPosition();
    set => this._file.Seek((ulong) value);
  }

  public FileAccessStream(string filePath, FileAccess.ModeFlags flags)
  {
    this._flags = flags;
    this._filePath = filePath;
    this._file = FileAccess.Open(filePath, flags) ?? throw new IOException($"Opening file {filePath} with flags {flags} failed: {FileAccess.GetOpenError()}");
  }

  public override void Flush() => this._file.Flush();

  public override int Read(byte[] buffer, int offset, int count)
  {
    byte[] buffer1 = this._file.GetBuffer((long) count);
    int length = Math.Min(count, buffer1.Length);
    Array.Copy((Array) buffer1, 0, (Array) buffer, offset, length);
    return length;
  }

  public override void Write(byte[] buffer, int offset, int count)
  {
    int length = Math.Min(buffer.Length - offset, count);
    bool flag;
    if (offset == 0 && buffer.Length <= count)
    {
      flag = this._file.StoreBuffer(buffer);
    }
    else
    {
      byte[] destinationArray = new byte[length];
      Array.Copy((Array) buffer, offset, (Array) destinationArray, 0, length);
      flag = this._file.StoreBuffer(destinationArray);
    }
    if (!flag)
      throw new IOException($"Failed to write {length} bytes to file {this._filePath}: {this._file.GetError()}");
  }

  public override long Seek(long offset, SeekOrigin origin)
  {
    switch ((int) origin)
    {
      case 0:
        this._file.Seek((ulong) offset);
        break;
      case 1:
        this._file.Seek(this._file.GetPosition() + (ulong) offset);
        break;
      case 2:
        this._file.Seek(this._file.GetLength() - (ulong) offset);
        break;
    }
    return (long) this._file.GetPosition();
  }

  public override void SetLength(long value) => throw new NotImplementedException();

  protected override void Dispose(bool disposing) => this._file.Close();
}
