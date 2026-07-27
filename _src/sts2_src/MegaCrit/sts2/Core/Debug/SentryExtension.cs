// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.SentryExtension
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using Sentry;
using System.IO;
using System.IO.Compression;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public static class SentryExtension
{
  public static void AddCompressedAttachment(this Scope scope, string text, string fileName)
  {
    byte[] numArray = SentryExtension.GzipCompress(text);
    if (numArray.Length <= 102400 /*0x019000*/)
      scope.AddAttachment(numArray, fileName, (AttachmentType) 0, (string) null);
    else
      Log.Warn($"Skipping Sentry attachment {fileName}: {numArray.Length / 1024 /*0x0400*/} KB exceeds {100} KB limit");
  }

  private static byte[] GzipCompress(string text)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(text);
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionLevel.Optimal, true))
        ((Stream) gzipStream).Write(bytes, 0, bytes.Length);
      return memoryStream.ToArray();
    }
  }
}
