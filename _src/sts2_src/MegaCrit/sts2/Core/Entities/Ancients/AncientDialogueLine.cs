// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogueLine
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Ancients;

public class AncientDialogueLine
{
  public const string sfxFallbackPath = "event:/sfx/ui/enchant_simple";
  private readonly string _sfxPath;

  public LocString? LineText { get; set; }

  public LocString? NextButtonText { get; set; }

  public AncientDialogueSpeaker Speaker { get; set; }

  public AncientDialogueLine(string sfxPath) => this._sfxPath = sfxPath;

  public string GetSfxOrFallbackPath()
  {
    return !string.IsNullOrEmpty(this._sfxPath) ? this._sfxPath : "event:/sfx/ui/enchant_simple";
  }
}
