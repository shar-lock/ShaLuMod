// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.CorruptFileHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class CorruptFileHandler
{
  public static string GenerateCorruptFilePath(string originalPath, ReadSaveStatus status)
  {
    int length = originalPath.LastIndexOf('/');
    if (length == -1)
      length = originalPath.LastIndexOf('\\');
    string str1 = length >= 0 ? originalPath.Substring(0, length) : "";
    string str2 = length >= 0 ? originalPath.Substring(length + 1) : originalPath;
    int num = str2.LastIndexOf('.');
    string str3 = num >= 0 ? str2.Substring(0, num) : str2;
    string str4 = num >= 0 ? str2.Substring(num) : "";
    long unixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    string reasonChar = CorruptFileHandler.GetReasonChar(status);
    string str5 = $"{str3}.{unixTimeSeconds}.{reasonChar}.corrupt";
    return string.IsNullOrEmpty(str1) ? str5 : $"{str1}/{str5}";
  }

  private static string GetReasonChar(ReadSaveStatus status)
  {
    string reasonChar;
    switch (status)
    {
      case ReadSaveStatus.JsonParseError:
        reasonChar = "JSN";
        break;
      case ReadSaveStatus.FileEmpty:
        reasonChar = "EMP";
        break;
      case ReadSaveStatus.MigrationFailed:
        reasonChar = "MIG";
        break;
      case ReadSaveStatus.MissingSchemaVersion:
        reasonChar = "SCH";
        break;
      case ReadSaveStatus.FutureVersion:
        reasonChar = "FUT";
        break;
      case ReadSaveStatus.VersionTooOld:
        reasonChar = "OLD";
        break;
      case ReadSaveStatus.FileAccessError:
        reasonChar = "ACC";
        break;
      case ReadSaveStatus.Unrecoverable:
        reasonChar = "UNR";
        break;
      case ReadSaveStatus.ValidationFailed:
        reasonChar = "VAL";
        break;
      default:
        reasonChar = "UNK";
        break;
    }
    return reasonChar;
  }
}
