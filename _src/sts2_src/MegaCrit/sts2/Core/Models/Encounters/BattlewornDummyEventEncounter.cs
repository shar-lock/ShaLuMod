// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.BattlewornDummyEventEncounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public abstract class BattlewornDummyEventEncounter : EncounterModel
{
  private const string _ranOutOfTimeKey = "RanOutOfTime";
  private bool _ranOutOfTime;

  public bool RanOutOfTime
  {
    get => this._ranOutOfTime;
    set
    {
      this.AssertMutable();
      this._ranOutOfTime = value;
    }
  }

  public override Dictionary<string, string> SaveCustomState()
  {
    return new Dictionary<string, string>()
    {
      ["RanOutOfTime"] = this.RanOutOfTime.ToString()
    };
  }

  public override void LoadCustomState(Dictionary<string, string> state)
  {
    this.RanOutOfTime = bool.Parse(state["RanOutOfTime"]);
  }
}
