// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.ExtraRunFields
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class ExtraRunFields
{
  public bool StartedWithNeow { get; set; }

  public int TestSubjectKills { get; set; }

  public bool FreedRepy { get; set; }

  public SerializableExtraRunFields ToSerializable()
  {
    return new SerializableExtraRunFields()
    {
      StartedWithNeow = this.StartedWithNeow,
      TestSubjectKills = this.TestSubjectKills,
      FreedRepy = this.FreedRepy
    };
  }

  public static ExtraRunFields FromSerializable(SerializableExtraRunFields save)
  {
    return new ExtraRunFields()
    {
      StartedWithNeow = save.StartedWithNeow,
      TestSubjectKills = save.TestSubjectKills,
      FreedRepy = save.FreedRepy
    };
  }
}
