// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.ReleaseInfoManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.IO;
using System.Text.Json;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public class ReleaseInfoManager
{
  private static ReleaseInfoManager? _instance;
  private const string _releaseInfoFileName = "release_info.json";

  public static ReleaseInfoManager Instance
  {
    get
    {
      return ReleaseInfoManager._instance ?? (ReleaseInfoManager._instance = new ReleaseInfoManager());
    }
  }

  public ReleaseInfo? ReleaseInfo { get; }

  public SemanticVersion? SemVer { get; }

  private ReleaseInfoManager()
  {
    this.ReleaseInfo = this.LoadConfig();
    if (this.ReleaseInfo?.Version == null)
      return;
    SemanticVersion version;
    SemanticVersion.TryFromString(this.ReleaseInfo.Version, out version);
    this.SemVer = version;
  }

  private ReleaseInfo? LoadConfig()
  {
    foreach (string possibleReleaseInfoPath in ReleaseInfoManager.GetPossibleReleaseInfoPaths())
    {
      string str = ProjectSettings.GlobalizePath(possibleReleaseInfoPath);
      if (FileAccess.FileExists(str))
      {
        Log.Info("Found release_info.json at: " + str);
        using (FileAccess fileAccess = FileAccess.Open(str, (FileAccess.ModeFlags) 1L))
        {
          if (fileAccess == null)
          {
            Log.Error("Failed to open file: " + str);
          }
          else
          {
            try
            {
              return JsonSerializer.Deserialize<ReleaseInfo>(fileAccess.GetAsText(false), ReleaseInfoJsonSerializerContext.Default.ReleaseInfo);
            }
            catch (JsonException ex)
            {
              Log.Error("Failed to deserialize release_info.json: " + ((Exception) ex).Message);
            }
            catch (Exception ex)
            {
              Log.Error("Unexpected error reading release_info.json: " + ex.Message);
            }
          }
        }
      }
    }
    Log.Info("File `release_info.json` not found in any of the expected locations.");
    return (ReleaseInfo) null;
  }

  private static string[] GetPossibleReleaseInfoPaths()
  {
    string str = Path.GetDirectoryName(OS.GetExecutablePath()) ?? string.Empty;
    return OS.GetName() == "macOS" ? new string[2]
    {
      Path.Combine(Path.Combine(str, "..", "Resources"), "release_info.json"),
      Path.Combine(str, "release_info.json")
    } : new string[1]
    {
      Path.Combine(str, "release_info.json")
    };
  }
}
