// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.Formatters.StarIconsFormatter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using SmartFormat.Core.Extensions;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.Formatters;

public class StarIconsFormatter : IFormatter
{
  private const string _starIconPath = "res://images/packed/sprite_fonts/star_icon.png";
  public const string starIconSprite = "[img]res://images/packed/sprite_fonts/star_icon.png[/img]";

  public string Name
  {
    get => "starIcons";
    set => throw new NotImplementedException();
  }

  public bool CanAutoDetect { get; set; }

  public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
  {
    int count;
    switch (formattingInfo.CurrentValue)
    {
      case DynamicVar dynamicVar:
        count = (int) dynamicVar.PreviewValue;
        break;
      case Decimal num1:
        count = (int) num1;
        break;
      case int num2:
        count = num2;
        break;
      default:
        throw new LocException($"Unknown value='{formattingInfo.CurrentValue}' type={formattingInfo.CurrentValue?.GetType()}");
    }
    string str = string.Concat(Enumerable.Repeat<string>("[img]res://images/packed/sprite_fonts/star_icon.png[/img]", count));
    formattingInfo.Write(str);
    return true;
  }
}
