// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Shops.NMerchantDialogue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Shops;

[ScriptPath("res://src/Core/Nodes/Screens/Shops/NMerchantDialogue.cs")]
public class NMerchantDialogue : Node2D
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private static readonly Vector2 _xRange = new Vector2(450f, 1450f);
  private MegaRichTextLabel _label;
  private Node2D _dialogueBox;
  private Tween? _tween;
  private Sprite2D _bubble;
  private ShaderMaterial _hsv;
  private MerchantDialogueSet _dialogueSet;

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text"));
    this._dialogueBox = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%DialogueBox"));
    this._bubble = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Bubble"));
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this._hsv = (ShaderMaterial) ((CanvasItem) this._bubble).Material;
    this._hsv.SetShaderParameter(NMerchantDialogue._h, Variant.op_Implicit(1f));
    this._hsv.SetShaderParameter(NMerchantDialogue._s, Variant.op_Implicit(1.2f));
    this._hsv.SetShaderParameter(NMerchantDialogue._v, Variant.op_Implicit(0.4f));
  }

  public void Initialize(MerchantDialogueSet dialogueSet) => this._dialogueSet = dialogueSet;

  public void ShowOnInventoryOpen()
  {
    this.ShowRandom((IEnumerable<LocString>) this._dialogueSet.OpenInventoryLines);
  }

  public void ShowForPurchaseAttempt(PurchaseStatus status)
  {
    this.ShowRandom((IEnumerable<LocString>) this._dialogueSet.GetPurchaseSuccessLines(status));
  }

  private void ShowRandom(IEnumerable<LocString> lines)
  {
    LocString locString = Rng.Chaotic.NextItem<LocString>(lines);
    if (locString == null)
      return;
    this._label.Text = $"[fly_in]{locString.GetFormattedText()}[/fly_in]";
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    this.Position = new Vector2(Rng.Chaotic.NextFloat(NMerchantDialogue._xRange.X, NMerchantDialogue._xRange.Y), this.Position.Y);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._bubble, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(0.75f, 0.75f)), 0.5).From(Variant.op_Implicit(new Vector2(0.25f, 0.25f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._dialogueBox, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.5).From(Variant.op_Implicit(-80f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.Chain();
    this._tween.TweenInterval(1.0);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMerchantDialogue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantDialogue.MethodName.ShowOnInventoryOpen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantDialogue.MethodName.ShowForPurchaseAttempt, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("status"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantDialogue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantDialogue.MethodName.ShowOnInventoryOpen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowOnInventoryOpen();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantDialogue.MethodName.ShowForPurchaseAttempt) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShowForPurchaseAttempt(VariantUtils.ConvertTo<PurchaseStatus>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantDialogue.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantDialogue.MethodName.ShowOnInventoryOpen) || StringName.op_Equality(ref method, NMerchantDialogue.MethodName.ShowForPurchaseAttempt) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._dialogueBox))
    {
      this._dialogueBox = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._bubble))
    {
      this._bubble = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._hsv))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._dialogueBox))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._dialogueBox);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._bubble))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._bubble);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantDialogue.PropertyName._hsv))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMerchantDialogue.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantDialogue.PropertyName._dialogueBox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantDialogue.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantDialogue.PropertyName._bubble, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantDialogue.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMerchantDialogue.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NMerchantDialogue.PropertyName._dialogueBox, Variant.From<Node2D>(ref this._dialogueBox));
    info.AddProperty(NMerchantDialogue.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NMerchantDialogue.PropertyName._bubble, Variant.From<Sprite2D>(ref this._bubble));
    info.AddProperty(NMerchantDialogue.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantDialogue.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantDialogue.PropertyName._dialogueBox, ref variant2))
      this._dialogueBox = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantDialogue.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NMerchantDialogue.PropertyName._bubble, ref variant4))
      this._bubble = ((Variant) ref variant4).As<Sprite2D>();
    Variant variant5;
    if (!info.TryGetProperty(NMerchantDialogue.PropertyName._hsv, ref variant5))
      return;
    this._hsv = ((Variant) ref variant5).As<ShaderMaterial>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ShowOnInventoryOpen = StringName.op_Implicit(nameof (ShowOnInventoryOpen));
    public static readonly StringName ShowForPurchaseAttempt = StringName.op_Implicit(nameof (ShowForPurchaseAttempt));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _dialogueBox = StringName.op_Implicit(nameof (_dialogueBox));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _bubble = StringName.op_Implicit(nameof (_bubble));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
