// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen.NBadge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;

[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NBadge.cs")]
public class NBadge : NButton
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/game_over_screen/badge");
  private Tween? _tween;
  private HoverTip _hoverTip;
  private LocString _title;
  private LocString _description;
  private const string _table = "badges";
  private Control _hoverNode;
  private NSelectionReticle _selectionReticle;

  public static NBadge? Create(Badge badgeModel)
  {
    if (TestMode.IsOn)
      return (NBadge) null;
    NBadge nbadge = PreloadManager.Cache.GetScene(NBadge._scenePath).Instantiate<NBadge>((PackedScene.GenEditState) 0L);
    ((Node) nbadge).GetNode<TextureRect>(NodePath.op_Implicit("%BadgeHolder")).Texture = badgeModel.BadgeBase;
    ((Node) nbadge).GetNode<TextureRect>(NodePath.op_Implicit("%Icon")).Texture = badgeModel.BadgeIcon;
    if (LocString.Exists("badges", $"{badgeModel.Id}.{NBadge.GetRarityPrefix(badgeModel.Rarity)}Title"))
    {
      nbadge._title = new LocString("badges", $"{badgeModel.Id}.{NBadge.GetRarityPrefix(badgeModel.Rarity)}Title");
      nbadge._description = new LocString("badges", $"{badgeModel.Id}.{NBadge.GetRarityPrefix(badgeModel.Rarity)}Description");
    }
    else
    {
      nbadge._title = new LocString("badges", badgeModel.Id + ".title");
      nbadge._description = new LocString("badges", badgeModel.Id + ".description");
    }
    return nbadge;
  }

  public static NBadge? Create(string id, BadgeRarity rarity)
  {
    if (TestMode.IsOn)
      return (NBadge) null;
    NBadge nbadge = PreloadManager.Cache.GetScene(NBadge._scenePath).Instantiate<NBadge>((PackedScene.GenEditState) 0L);
    ((Node) nbadge).GetNode<TextureRect>(NodePath.op_Implicit("%BadgeHolder")).Texture = NBadge.GetBadgeBaseTexture(rarity);
    ((Node) nbadge).GetNode<TextureRect>(NodePath.op_Implicit("%Icon")).Texture = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath($"ui/game_over_screen/badge_{id.ToLowerInvariant()}.png"));
    if (LocString.Exists("badges", $"{id}.{NBadge.GetRarityPrefix(rarity)}Title"))
    {
      nbadge._title = new LocString("badges", $"{id}.{NBadge.GetRarityPrefix(rarity)}Title");
      nbadge._description = new LocString("badges", $"{id}.{NBadge.GetRarityPrefix(rarity)}Description");
    }
    else
    {
      nbadge._title = new LocString("badges", id + ".title");
      nbadge._description = new LocString("badges", id + ".description");
    }
    return nbadge;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._hoverNode = (Control) this;
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this._hoverTip = new HoverTip(this._title, this._description);
  }

  private static string GetRarityPrefix(BadgeRarity rarity)
  {
    string rarityPrefix;
    switch (rarity)
    {
      case BadgeRarity.Bronze:
        rarityPrefix = "bronze";
        break;
      case BadgeRarity.Silver:
        rarityPrefix = "silver";
        break;
      case BadgeRarity.Gold:
        rarityPrefix = "gold";
        break;
      default:
        rarityPrefix = "ERROR";
        break;
    }
    return rarityPrefix;
  }

  public async Task AnimateIn()
  {
    if (!((Node) this).IsValid())
      return;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this.Position.Y), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L).From(Variant.op_Implicit(this.Position.Y + 40f));
    bool flag = await this._tween.AwaitFinished((Node) this);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._tween?.Kill();
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    NHoverTipSet.CreateAndShow(this._hoverNode, (IHoverTip) this._hoverTip)?.SetGlobalPosition(Vector2.op_Addition(this._hoverNode.GlobalPosition, new Vector2(10f, -132f)), false);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    NHoverTipSet.Remove(this._hoverNode);
    this._selectionReticle.OnDeselect();
  }

  private static Texture2D GetBadgeBaseTexture(BadgeRarity rarity)
  {
    Texture2D texture2D;
    switch (rarity)
    {
      case BadgeRarity.Bronze:
        texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_bronze.png"));
        break;
      case BadgeRarity.Silver:
        texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_silver.png"));
        break;
      case BadgeRarity.Gold:
        texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("ui/game_over_screen/badge_gold.png"));
        break;
      default:
        texture2D = PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath("atlases/power_atlas.sprites/missing_power.tres"));
        break;
    }
    return texture2D;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NBadge.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("id"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("rarity"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBadge.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBadge.MethodName.GetRarityPrefix, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("rarity"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NBadge.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBadge.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBadge.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBadge.MethodName.GetBadgeBaseTexture, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NBadge.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NBadge nbadge = NBadge.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<BadgeRarity>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NBadge>(ref nbadge);
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName.GetRarityPrefix) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string rarityPrefix = NBadge.GetRarityPrefix(VariantUtils.ConvertTo<BadgeRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref rarityPrefix);
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBadge.MethodName.GetBadgeBaseTexture) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    Texture2D badgeBaseTexture = NBadge.GetBadgeBaseTexture(VariantUtils.ConvertTo<BadgeRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<Texture2D>(ref badgeBaseTexture);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBadge.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NBadge nbadge = NBadge.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<BadgeRarity>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NBadge>(ref nbadge);
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName.GetRarityPrefix) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string rarityPrefix = NBadge.GetRarityPrefix(VariantUtils.ConvertTo<BadgeRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref rarityPrefix);
      return true;
    }
    if (StringName.op_Equality(ref method, NBadge.MethodName.GetBadgeBaseTexture) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Texture2D badgeBaseTexture = NBadge.GetBadgeBaseTexture(VariantUtils.ConvertTo<BadgeRarity>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Texture2D>(ref badgeBaseTexture);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBadge.MethodName.Create) || StringName.op_Equality(ref method, NBadge.MethodName._Ready) || StringName.op_Equality(ref method, NBadge.MethodName.GetRarityPrefix) || StringName.op_Equality(ref method, NBadge.MethodName._ExitTree) || StringName.op_Equality(ref method, NBadge.MethodName.OnFocus) || StringName.op_Equality(ref method, NBadge.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NBadge.MethodName.GetBadgeBaseTexture) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBadge.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBadge.PropertyName._hoverNode))
    {
      this._hoverNode = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBadge.PropertyName._selectionReticle))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBadge.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NBadge.PropertyName._hoverNode))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._hoverNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBadge.PropertyName._selectionReticle))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBadge.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBadge.PropertyName._hoverNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBadge.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBadge.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NBadge.PropertyName._hoverNode, Variant.From<Control>(ref this._hoverNode));
    info.AddProperty(NBadge.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBadge.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NBadge.PropertyName._hoverNode, ref variant2))
      this._hoverNode = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NBadge.PropertyName._selectionReticle, ref variant3))
      return;
    this._selectionReticle = ((Variant) ref variant3).As<NSelectionReticle>();
  }

  public new class MethodName : NButton.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName GetRarityPrefix = StringName.op_Implicit(nameof (GetRarityPrefix));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName GetBadgeBaseTexture = StringName.op_Implicit(nameof (GetBadgeBaseTexture));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _hoverNode = StringName.op_Implicit(nameof (_hoverNode));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
