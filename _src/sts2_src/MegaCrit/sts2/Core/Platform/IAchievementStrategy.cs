// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.IAchievementStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;

#nullable disable
namespace MegaCrit.Sts2.Core.Platform;

public interface IAchievementStrategy
{
  void Unlock(Achievement achievement);

  void Revoke(Achievement achievement);

  bool IsUnlocked(Achievement achievement);
}
