// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.ModSettings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public class ModSettings
{
  [JsonPropertyName("mods_enabled")]
  public bool PlayerAgreedToModLoading { get; set; }

  [JsonPropertyName("mod_list")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SettingsSaveMod> ModList { get; set; } = new List<SettingsSaveMod>();

  public bool IsModDisabled(Mod mod) => this.IsModDisabled(mod.manifest?.id, mod.modSource);

  public bool IsModDisabled(string? id, ModSource source)
  {
    return this.ModList.Any<SettingsSaveMod>((Func<SettingsSaveMod, bool>) (m => m.Id == id && m.Source == source && !m.IsEnabled));
  }
}
