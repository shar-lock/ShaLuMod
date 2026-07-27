// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Potions.PotionBodyExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Potions;

public static class PotionBodyExtensions
{
  private static readonly Dictionary<(PotionBody, PotionOverlay), string> _potionOverlayMap = new Dictionary<(PotionBody, PotionOverlay), string>()
  {
    [(PotionBody.Cube, PotionOverlay.Bubbles)] = "potion_cube_bubbles.png",
    [(PotionBody.Cube, PotionOverlay.Curve)] = "potion_cube_curve.png",
    [(PotionBody.Diamond, PotionOverlay.Bubbles)] = "potion_diamond_bubbles.png",
    [(PotionBody.Fairy, PotionOverlay.Sparkle)] = "potion_fairy_sparkle.png",
    [(PotionBody.FatDiamond, PotionOverlay.Curve)] = "potion_fat_diamond_curve.png",
    [(PotionBody.Heart, PotionOverlay.Curve)] = "potion_heart_curve.png",
    [(PotionBody.Spiky, PotionOverlay.Curve)] = "potion_spiky_curve.png"
  };

  public static string GetBodyPath(this PotionBody body)
  {
    string str;
    switch (body)
    {
      case PotionBody.None:
        throw new ArgumentOutOfRangeException(nameof (body), (object) body, (string) null);
      case PotionBody.Anvil:
        str = "potion_anvil_body.png";
        break;
      case PotionBody.Bolt:
        str = "potion_bolt_body.png";
        break;
      case PotionBody.Card:
        str = "potion_card_body.png";
        break;
      case PotionBody.Cube:
        str = "potion_cube_body.png";
        break;
      case PotionBody.Diamond:
        str = "potion_diamond_body.png";
        break;
      case PotionBody.Eye:
        str = "potion_eye_body.png";
        break;
      case PotionBody.Fairy:
        str = "potion_fairy_body.png";
        break;
      case PotionBody.Fat:
        str = "potion_fat_body.png";
        break;
      case PotionBody.FatDiamond:
        str = "potion_fat_diamond_body.png";
        break;
      case PotionBody.Flask:
        str = "potion_flask_body.png";
        break;
      case PotionBody.Ghost:
        str = "potion_ghost_body.png";
        break;
      case PotionBody.Heart:
        str = "potion_heart_body.png";
        break;
      case PotionBody.Moon:
        str = "potion_moon_body.png";
        break;
      case PotionBody.Shield:
        str = "potion_shield_body.png";
        break;
      case PotionBody.Snecko:
        str = "potion_snecko_body.png";
        break;
      case PotionBody.Sphere:
        str = "potion_sphere_body.png";
        break;
      case PotionBody.Spiky:
        str = "potion_spiky_body.png";
        break;
      case PotionBody.Thin:
        str = "potion_thin_body.png";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (body), (object) body, (string) null);
    }
    return ImageHelper.GetImagePath("packed/potion/body/" + str);
  }

  public static string? GetGradientPath(this PotionBody body)
  {
    string str1;
    switch (body)
    {
      case PotionBody.Anvil:
        str1 = "potion_anvil_gradient.png";
        break;
      case PotionBody.Card:
        str1 = "potion_card_gradient.png";
        break;
      case PotionBody.Sphere:
        str1 = "potion_sphere_gradient.png";
        break;
      default:
        str1 = (string) null;
        break;
    }
    string str2 = str1;
    return str2 != null ? ImageHelper.GetImagePath("packed/potion/gradient/" + str2) : (string) null;
  }

  public static string GetJuicePath(this PotionBody body)
  {
    string str;
    switch (body)
    {
      case PotionBody.None:
        throw new ArgumentOutOfRangeException(nameof (body), (object) body, (string) null);
      case PotionBody.Anvil:
        str = "potion_anvil_juice.png";
        break;
      case PotionBody.Bolt:
        str = "potion_bolt_juice.png";
        break;
      case PotionBody.Card:
        str = "potion_card_juice.png";
        break;
      case PotionBody.Cube:
        str = "potion_cube_juice.png";
        break;
      case PotionBody.Diamond:
        str = "potion_diamond_juice.png";
        break;
      case PotionBody.Eye:
        str = "potion_eye_juice.png";
        break;
      case PotionBody.Fairy:
        str = "potion_fairy_juice.png";
        break;
      case PotionBody.Fat:
        str = "potion_fat_juice.png";
        break;
      case PotionBody.FatDiamond:
        str = "potion_fat_diamond_juice.png";
        break;
      case PotionBody.Flask:
        str = "potion_flask_juice.png";
        break;
      case PotionBody.Ghost:
        str = "potion_ghost_juice.png";
        break;
      case PotionBody.Heart:
        str = "potion_heart_juice.png";
        break;
      case PotionBody.Moon:
        str = "potion_moon_juice.png";
        break;
      case PotionBody.Shield:
        str = "potion_shield_juice.png";
        break;
      case PotionBody.Snecko:
        str = "potion_snecko_juice.png";
        break;
      case PotionBody.Sphere:
        str = "potion_sphere_juice.png";
        break;
      case PotionBody.Spiky:
        str = "potion_spiky_juice.png";
        break;
      case PotionBody.Thin:
        str = "potion_thin_juice.png";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (body), (object) body, (string) null);
    }
    return ImageHelper.GetImagePath("packed/potion/juice/" + str);
  }

  public static string? GetOverlayPath(this PotionBody body, PotionOverlay overlay)
  {
    string str;
    return PotionBodyExtensions._potionOverlayMap.TryGetValue((body, overlay), out str) ? ImageHelper.GetImagePath("packed/potion/overlay/" + str) : (string) null;
  }
}
