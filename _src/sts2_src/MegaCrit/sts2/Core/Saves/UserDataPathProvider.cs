// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.UserDataPathProvider
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Platform;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class UserDataPathProvider
{
  public static string SavesDir => "saves";

  public static bool IsRunningModded { get; set; }

  public static string GetProfileScopedPath(
    int profileId,
    string dataType,
    PlatformType? platformOverride = null,
    ulong? userIdOverride = null)
  {
    PlatformType platformType = (PlatformType) ((int) platformOverride ?? (int) PlatformUtil.PrimaryPlatform);
    ulong num = (ulong) ((long) userIdOverride ?? (long) PlatformUtil.GetLocalPlayerId(platformType));
    return $"user://{UserDataPathProvider.GetPlatformDirectoryName(platformType)}/{num}/{UserDataPathProvider.GetProfileDir(profileId)}/{dataType}";
  }

  public static string GetProfileScopedBasePath(
    int profileId,
    PlatformType? platformOverride = null,
    ulong? userIdOverride = null)
  {
    PlatformType platformType = (PlatformType) ((int) platformOverride ?? (int) PlatformUtil.PrimaryPlatform);
    ulong num = (ulong) ((long) userIdOverride ?? (long) PlatformUtil.GetLocalPlayerId(platformType));
    return $"user://{UserDataPathProvider.GetPlatformDirectoryName(platformType)}/{num}/{UserDataPathProvider.GetProfileDir(profileId)}";
  }

  public static string GetAccountScopedBasePath(
    string? dataType,
    PlatformType? platformOverride = null,
    ulong? userIdOverride = null)
  {
    PlatformType platformType = (PlatformType) ((int) platformOverride ?? (int) PlatformUtil.PrimaryPlatform);
    ulong num = (ulong) ((long) userIdOverride ?? (long) PlatformUtil.GetLocalPlayerId(platformType));
    string accountScopedBasePath = $"user://{UserDataPathProvider.GetPlatformDirectoryName(platformType)}/{num}";
    if (dataType != null)
      accountScopedBasePath = StringExtensions.PathJoin(accountScopedBasePath, dataType);
    return accountScopedBasePath;
  }

  public static string GetAccountDir(bool? forceModState = null)
  {
    return ((int) forceModState ?? (UserDataPathProvider.IsRunningModded ? 1 : 0)) == 0 ? "" : "modded";
  }

  public static string GetProfileDir(int profileId)
  {
    return UserDataPathProvider.GetProfileDir(profileId, new bool?());
  }

  public static string GetProfileDir(int profileId, bool? forceModState)
  {
    return StringExtensions.PathJoin(UserDataPathProvider.GetAccountDir(forceModState), $"profile{profileId}");
  }

  public static string GetLegacyPreAccountPath(string dataType) => "user://" + dataType;

  public static string GetPlatformDirectoryName(PlatformType platform)
  {
    return platform != PlatformType.Steam ? (OS.HasFeature("editor") ? "editor" : "default") : "steam";
  }

  public static bool IsLegacyPath(string path)
  {
    return !path.Contains("/steam/") && !path.Contains("/default/") && !path.Contains("/editor/");
  }
}
