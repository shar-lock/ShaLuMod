// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LanguageCode
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public class LanguageCode
{
  public LanguageCode(string code)
  {
    this.Code = !string.IsNullOrEmpty(code) && code.Length >= 2 && code.Length <= 3 ? code.ToLowerInvariant() : throw new ArgumentException("Language code must be 2 to 3 characters long.");
  }

  public string Code { get; }

  public bool IsValid()
  {
    int length = this.Code.Length;
    return length >= 2 && length <= 3;
  }

  public override string ToString() => this.Code;
}
