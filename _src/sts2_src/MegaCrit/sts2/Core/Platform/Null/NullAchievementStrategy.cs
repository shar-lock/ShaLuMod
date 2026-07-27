// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Null.NullAchievementStrategy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;

#nullable disable
namespace MegaCrit.Sts2.Core.Platform.Null;

public class NullAchievementStrategy : IAchievementStrategy
{
  public void Unlock(Achievement achievement)
  {
  }

  public void Revoke(Achievement achievement)
  {
  }

  public bool IsUnlocked(Achievement achievement) => false;
}
