// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NTinyCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NTinyCard.cs")]
public class NTinyCard : NButton
{
  private TextureRect _cardBack;
  private TextureRect _cardPortrait;
  private TextureRect _cardPortraitShadow;
  private Control _cardBanner;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._cardBack = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%CardBack"));
    this._cardPortrait = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Portrait"));
    this._cardPortraitShadow = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PortraitShadow"));
    this._cardBanner = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Banner"));
  }

  public void SetCard(CardModel card)
  {
    this.SetCardBackColor(card.Pool);
    this.SetCardPortraitShape(card.Type);
    this.SetBannerColor(card.Rarity);
    ((CanvasItem) this._cardBack).Material = card.FrameMaterial;
  }

  public void Set(CardPoolModel cardPool, CardType type, CardRarity rarity)
  {
    this.SetCardBackColor(cardPool);
    this.SetCardPortraitShape(type);
    this.SetBannerColor(rarity);
    ((CanvasItem) this._cardBack).Material = cardPool.AllCards.First<CardModel>().FrameMaterial;
  }

  private void SetCardBackColor(CardPoolModel cardPool)
  {
    ((CanvasItem) this._cardBack).Modulate = cardPool.DeckEntryCardColor;
  }

  private void SetCardPortraitShape(CardType type)
  {
    if (type != CardType.Attack)
    {
      if (type == CardType.Power)
      {
        this._cardPortrait.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/power_portrait.png");
        this._cardPortraitShadow.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/power_portrait_shadow.png");
      }
      else
      {
        this._cardPortrait.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/skill_portrait.png");
        this._cardPortraitShadow.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/skill_portrait_shadow.png");
      }
    }
    else
    {
      this._cardPortrait.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/attack_portrait.png");
      this._cardPortraitShadow.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D("res://images/packed/run_history/attack_portrait_shadow.png");
    }
  }

  private void SetBannerColor(CardRarity rarity)
  {
    ((CanvasItem) this._cardBanner).Modulate = this.GetBannerColor(rarity);
  }

  private Color GetBannerColor(CardRarity rarity)
  {
    switch (rarity)
    {
      case CardRarity.Basic:
      case CardRarity.Common:
        return new Color("9C9C9CFF");
      case CardRarity.Uncommon:
        return new Color("64FFFFFF");
      case CardRarity.Rare:
        return new Color("FFDA36FF");
      case CardRarity.Event:
        return new Color("13BE1AFF");
      case CardRarity.Curse:
        return new Color("E669FFFF");
      case CardRarity.Quest:
        return new Color("F46836FF");
      default:
        Log.Warn($"Unspecified Rarity: {rarity}");
        return Colors.White;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NTinyCard.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTinyCard.MethodName.SetCardPortraitShape, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("type"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTinyCard.MethodName.SetBannerColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("rarity"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTinyCard.MethodName.GetBannerColor, new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("rarity"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTinyCard.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTinyCard.MethodName.SetCardPortraitShape) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCardPortraitShape(VariantUtils.ConvertTo<CardType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTinyCard.MethodName.SetBannerColor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetBannerColor(VariantUtils.ConvertTo<CardRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTinyCard.MethodName.GetBannerColor) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    Color bannerColor = this.GetBannerColor(VariantUtils.ConvertTo<CardRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<Color>(ref bannerColor);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTinyCard.MethodName._Ready) || StringName.op_Equality(ref method, NTinyCard.MethodName.SetCardPortraitShape) || StringName.op_Equality(ref method, NTinyCard.MethodName.SetBannerColor) || StringName.op_Equality(ref method, NTinyCard.MethodName.GetBannerColor) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTinyCard.PropertyName._cardBack))
    {
      this._cardBack = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTinyCard.PropertyName._cardPortrait))
    {
      this._cardPortrait = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTinyCard.PropertyName._cardPortraitShadow))
    {
      this._cardPortraitShadow = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTinyCard.PropertyName._cardBanner))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._cardBanner = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTinyCard.PropertyName._cardBack))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._cardBack);
      return true;
    }
    if (StringName.op_Equality(ref name, NTinyCard.PropertyName._cardPortrait))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._cardPortrait);
      return true;
    }
    if (StringName.op_Equality(ref name, NTinyCard.PropertyName._cardPortraitShadow))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._cardPortraitShadow);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTinyCard.PropertyName._cardBanner))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._cardBanner);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTinyCard.PropertyName._cardBack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTinyCard.PropertyName._cardPortrait, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTinyCard.PropertyName._cardPortraitShadow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTinyCard.PropertyName._cardBanner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTinyCard.PropertyName._cardBack, Variant.From<TextureRect>(ref this._cardBack));
    info.AddProperty(NTinyCard.PropertyName._cardPortrait, Variant.From<TextureRect>(ref this._cardPortrait));
    info.AddProperty(NTinyCard.PropertyName._cardPortraitShadow, Variant.From<TextureRect>(ref this._cardPortraitShadow));
    info.AddProperty(NTinyCard.PropertyName._cardBanner, Variant.From<Control>(ref this._cardBanner));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTinyCard.PropertyName._cardBack, ref variant1))
      this._cardBack = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NTinyCard.PropertyName._cardPortrait, ref variant2))
      this._cardPortrait = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NTinyCard.PropertyName._cardPortraitShadow, ref variant3))
      this._cardPortraitShadow = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (!info.TryGetProperty(NTinyCard.PropertyName._cardBanner, ref variant4))
      return;
    this._cardBanner = ((Variant) ref variant4).As<Control>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetCardPortraitShape = StringName.op_Implicit(nameof (SetCardPortraitShape));
    public static readonly StringName SetBannerColor = StringName.op_Implicit(nameof (SetBannerColor));
    public static readonly StringName GetBannerColor = StringName.op_Implicit(nameof (GetBannerColor));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _cardBack = StringName.op_Implicit(nameof (_cardBack));
    public static readonly StringName _cardPortrait = StringName.op_Implicit(nameof (_cardPortrait));
    public static readonly StringName _cardPortraitShadow = StringName.op_Implicit(nameof (_cardPortraitShadow));
    public static readonly StringName _cardBanner = StringName.op_Implicit(nameof (_cardBanner));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
