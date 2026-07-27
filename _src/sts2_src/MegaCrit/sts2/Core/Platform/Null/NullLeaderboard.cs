// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Null;

public class NullLeaderboard
{
  [JsonPropertyName("name")]
  public string name = "";
  [JsonPropertyName("entries")]
  public List<NullLeaderboardFileEntry> entries = new List<NullLeaderboardFileEntry>();
}
