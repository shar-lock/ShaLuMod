// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.SettingsSaveMod
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public class SettingsSaveMod
{
  [JsonPropertyName("id")]
  public string Id { get; set; } = "";

  [JsonPropertyName("source")]
  public ModSource Source { get; set; }

  [JsonPropertyName("is_enabled")]
  public bool IsEnabled { get; set; } = true;

  public SettingsSaveMod()
  {
  }

  public SettingsSaveMod(Mod mod)
  {
    this.Id = mod.manifest?.id ?? throw new InvalidOperationException();
    this.Source = mod.modSource;
  }
}
