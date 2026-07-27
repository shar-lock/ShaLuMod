// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializableEpoch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class SerializableEpoch
{
  [JsonPropertyName("id")]
  public string Id { get; }

  [JsonPropertyName("state")]
  public EpochState State { get; set; }

  [JsonPropertyName("obtain_date")]
  public long ObtainDate { get; set; }

  public SerializableEpoch(string id, EpochState state)
  {
    this.Id = id;
    this.State = EpochState.NotObtained;
    bool flag;
    switch (state)
    {
      case EpochState.ObtainedNoSlot:
      case EpochState.Obtained:
      case EpochState.Revealed:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    this.SetObtained(state);
  }

  public void SetObtained(EpochState state)
  {
    if (this.State == EpochState.ObtainedNoSlot || this.State == EpochState.Obtained || this.State == EpochState.Revealed)
      return;
    this.ObtainDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    this.State = state;
  }
}
