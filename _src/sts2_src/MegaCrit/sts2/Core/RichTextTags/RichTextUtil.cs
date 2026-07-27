// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.RichTextTags.RichTextUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable disable
namespace MegaCrit.Sts2.Core.RichTextTags;

public static class RichTextUtil
{
  public static readonly Variant colorKey;
  public static readonly Variant visibleKey;

  static RichTextUtil()
  {
    string str1 = "color";
    RichTextUtil.colorKey = Variant.From<string>(ref str1);
    string str2 = "visible";
    RichTextUtil.visibleKey = Variant.From<string>(ref str2);
  }
}
