// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.Holders.NPreviewCardHolder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards.Holders;

[ScriptPath("res://src/Core/Nodes/Cards/Holders/NPreviewCardHolder.cs")]
public class NPreviewCardHolder : NCardHolder
{
  private bool _showHoverTips;
  private bool _scaleOnHover;
  private Vector2 _originalScale = Vector2.One;

  protected override Vector2 HoverScale => Vector2.op_Multiply(this._originalScale, 1.1f);

  public override Vector2 SmallScale => this._originalScale;

  public override bool IsShowingUpgradedCard
  {
    get
    {
      if (base.IsShowingUpgradedCard)
        return true;
      return this.CardModel != null && this.CardModel.UpgradePreviewType.IsPreview();
    }
  }

  private static string ScenePath => SceneHelper.GetScenePath("cards/holders/preview_card_holder");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NPreviewCardHolder.ScenePath);
    }
  }

  public static NPreviewCardHolder? Create(NCard card, bool showHoverTips, bool scaleOnHover)
  {
    if (TestMode.IsOn)
      return (NPreviewCardHolder) null;
    NPreviewCardHolder npreviewCardHolder = PreloadManager.Cache.GetScene(NPreviewCardHolder.ScenePath).Instantiate<NPreviewCardHolder>((PackedScene.GenEditState) 0L);
    npreviewCardHolder.Initialize(card, showHoverTips, scaleOnHover);
    return npreviewCardHolder;
  }

  public override void _Ready() => this.ConnectSignals();

  private void Initialize(NCard card, bool showHoverTips, bool scaleOnHover)
  {
    ((Node) this).Name = StringName.op_Implicit($"UpgradePreviewCardHolder-{card.Model?.Id}");
    this.SetCard(card);
    this.Scale = this.SmallScale;
    this._showHoverTips = showHoverTips;
    this._scaleOnHover = scaleOnHover;
  }

  protected override void OnFocus()
  {
    this._isHovered = true;
    if (this._scaleOnHover)
    {
      this._hoverTween?.Kill();
      this.Scale = this.HoverScale;
    }
    if (!this._showHoverTips)
      return;
    this.CreateHoverTips();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    if (this._scaleOnHover)
    {
      this._hoverTween?.Kill();
      this._hoverTween = ((Node) this).CreateTween();
      this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.SmallScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    if (!this._showHoverTips)
      return;
    this.ClearHoverTips();
  }

  public void SetCardScale(Vector2 scale)
  {
    this._originalScale = scale;
    this.Scale = this._originalScale;
  }

  protected override void CreateHoverTips()
  {
    if (this.CardNode == null)
      return;
    NHoverTipSet.CreateAndShow((Control) this, this.CardNode.Model.HoverTips)?.SetAlignmentForCardHolder((NCardHolder) this);
  }

  public override void _ExitTree()
  {
    if (this.CardNode == null)
      return;
    if (((Node) this).IsAncestorOf((Node) this.CardNode))
      ((Node) this.CardNode).QueueFreeSafely();
    this.CardNode = (NCard) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NPreviewCardHolder.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showHoverTips"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("scaleOnHover"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showHoverTips"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("scaleOnHover"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName.SetCardScale, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("scale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName.CreateHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPreviewCardHolder.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NPreviewCardHolder>(ref npreviewCardHolder);
      return true;
    }
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.Initialize) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.Initialize(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.SetCardScale) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCardScale(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.CreateHoverTips) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateHoverTips();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPreviewCardHolder.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NPreviewCardHolder npreviewCardHolder = NPreviewCardHolder.Create(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NPreviewCardHolder>(ref npreviewCardHolder);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.Create) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName._Ready) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.Initialize) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.OnFocus) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.SetCardScale) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName.CreateHoverTips) || StringName.op_Equality(ref method, NPreviewCardHolder.MethodName._ExitTree) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName._showHoverTips))
    {
      this._showHoverTips = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName._scaleOnHover))
    {
      this._scaleOnHover = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName._originalScale))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._originalScale = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName.HoverScale))
    {
      ref godot_variant local = ref value;
      Vector2 hoverScale = this.HoverScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hoverScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName.SmallScale))
    {
      ref godot_variant local = ref value;
      Vector2 smallScale = this.SmallScale;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref smallScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName.IsShowingUpgradedCard))
    {
      ref godot_variant local = ref value;
      bool showingUpgradedCard = this.IsShowingUpgradedCard;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref showingUpgradedCard);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName._showHoverTips))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._showHoverTips);
      return true;
    }
    if (StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName._scaleOnHover))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._scaleOnHover);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPreviewCardHolder.PropertyName._originalScale))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._originalScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NPreviewCardHolder.PropertyName.HoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NPreviewCardHolder.PropertyName.SmallScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPreviewCardHolder.PropertyName._showHoverTips, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPreviewCardHolder.PropertyName._scaleOnHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NPreviewCardHolder.PropertyName._originalScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPreviewCardHolder.PropertyName.IsShowingUpgradedCard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NPreviewCardHolder.PropertyName._showHoverTips, Variant.From<bool>(ref this._showHoverTips));
    info.AddProperty(NPreviewCardHolder.PropertyName._scaleOnHover, Variant.From<bool>(ref this._scaleOnHover));
    info.AddProperty(NPreviewCardHolder.PropertyName._originalScale, Variant.From<Vector2>(ref this._originalScale));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPreviewCardHolder.PropertyName._showHoverTips, ref variant1))
      this._showHoverTips = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NPreviewCardHolder.PropertyName._scaleOnHover, ref variant2))
      this._scaleOnHover = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (!info.TryGetProperty(NPreviewCardHolder.PropertyName._originalScale, ref variant3))
      return;
    this._originalScale = ((Variant) ref variant3).As<Vector2>();
  }

  public new class MethodName : NCardHolder.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName SetCardScale = StringName.op_Implicit(nameof (SetCardScale));
    public new static readonly StringName CreateHoverTips = StringName.op_Implicit(nameof (CreateHoverTips));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public new class PropertyName : NCardHolder.PropertyName
  {
    public new static readonly StringName HoverScale = StringName.op_Implicit(nameof (HoverScale));
    public new static readonly StringName SmallScale = StringName.op_Implicit(nameof (SmallScale));
    public new static readonly StringName IsShowingUpgradedCard = StringName.op_Implicit(nameof (IsShowingUpgradedCard));
    public static readonly StringName _showHoverTips = StringName.op_Implicit(nameof (_showHoverTips));
    public static readonly StringName _scaleOnHover = StringName.op_Implicit(nameof (_scaleOnHover));
    public static readonly StringName _originalScale = StringName.op_Implicit(nameof (_originalScale));
  }

  public new class SignalName : NCardHolder.SignalName
  {
  }
}
