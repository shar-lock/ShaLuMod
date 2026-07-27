// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere.NCrystalSphereDialogue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;

[ScriptPath("res://src/Core/Nodes/Events/Custom/CrystalSphere/NCrystalSphereDialogue.cs")]
public class NCrystalSphereDialogue : Node2D
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private static readonly Vector2 _xRange = new Vector2(1100f, 1200f);
  private static readonly LocString[] _startLines = new LocString[2]
  {
    new LocString("events", "CRYSTAL_SPHERE.banter.START.1"),
    new LocString("events", "CRYSTAL_SPHERE.banter.START.2")
  };
  private static readonly LocString[] _revealBadLines = new LocString[3]
  {
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_BAD.1"),
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_BAD.2"),
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_BAD.3")
  };
  private static readonly LocString[] _revealGoodLines = new LocString[5]
  {
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.1"),
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.2"),
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.3"),
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.4"),
    new LocString("events", "CRYSTAL_SPHERE.banter.REVEAL_GOOD.5")
  };
  private static readonly LocString[] _endLines = new LocString[2]
  {
    new LocString("events", "CRYSTAL_SPHERE.banter.END.1"),
    new LocString("events", "CRYSTAL_SPHERE.banter.END.2")
  };
  private MegaRichTextLabel _label;
  private Node2D _dialogueBox;
  private Tween? _tween;
  private Sprite2D _bubble;
  private ShaderMaterial _hsv;

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Text"));
    this._dialogueBox = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%DialogueBox"));
    this._bubble = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Bubble"));
    ((CanvasItem) this).Modulate = Colors.Transparent;
    this._hsv = (ShaderMaterial) ((CanvasItem) this._bubble).Material;
    this._hsv.SetShaderParameter(NCrystalSphereDialogue._h, Variant.op_Implicit(0.25f));
    this._hsv.SetShaderParameter(NCrystalSphereDialogue._s, Variant.op_Implicit(0.75f));
    this._hsv.SetShaderParameter(NCrystalSphereDialogue._v, Variant.op_Implicit(1f));
  }

  public void PlayStart()
  {
    this.Play(Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) NCrystalSphereDialogue._startLines));
  }

  public void PlayBad()
  {
    this.Play(Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) NCrystalSphereDialogue._revealBadLines));
  }

  public void PlayGood()
  {
    this.Play(Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) NCrystalSphereDialogue._revealGoodLines));
  }

  public void PlayEnd()
  {
    this.Play(Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) NCrystalSphereDialogue._endLines));
  }

  private void Play(LocString locString)
  {
    this._label.Text = $"[fly_in]{locString.GetFormattedText()}[/fly_in]";
    Log.Info(this._label.Text ?? "");
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    this.Position = new Vector2(Rng.Chaotic.NextFloat(NCrystalSphereDialogue._xRange.X, NCrystalSphereDialogue._xRange.Y), this.Position.Y);
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
    this._tween.TweenProperty((GodotObject) this._bubble, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(0.75f, 0.75f)), 0.5).From(Variant.op_Implicit(new Vector2(0.25f, 0.25f))).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._dialogueBox, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.5).From(Variant.op_Implicit(-80f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.Chain();
    this._tween.TweenInterval(1.5);
    this._tween.Chain();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCrystalSphereDialogue.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereDialogue.MethodName.PlayStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereDialogue.MethodName.PlayBad, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereDialogue.MethodName.PlayGood, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCrystalSphereDialogue.MethodName.PlayEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayBad) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayBad();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayGood) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayGood();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayEnd) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.PlayEnd();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName._Ready) || StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayStart) || StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayBad) || StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayGood) || StringName.op_Equality(ref method, NCrystalSphereDialogue.MethodName.PlayEnd) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._dialogueBox))
    {
      this._dialogueBox = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._bubble))
    {
      this._bubble = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._hsv))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._dialogueBox))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._dialogueBox);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._bubble))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._bubble);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCrystalSphereDialogue.PropertyName._hsv))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereDialogue.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereDialogue.PropertyName._dialogueBox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereDialogue.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereDialogue.PropertyName._bubble, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCrystalSphereDialogue.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCrystalSphereDialogue.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
    info.AddProperty(NCrystalSphereDialogue.PropertyName._dialogueBox, Variant.From<Node2D>(ref this._dialogueBox));
    info.AddProperty(NCrystalSphereDialogue.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCrystalSphereDialogue.PropertyName._bubble, Variant.From<Sprite2D>(ref this._bubble));
    info.AddProperty(NCrystalSphereDialogue.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._dialogueBox, ref variant2))
      this._dialogueBox = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NCrystalSphereDialogue.PropertyName._bubble, ref variant4))
      this._bubble = ((Variant) ref variant4).As<Sprite2D>();
    Variant variant5;
    if (!info.TryGetProperty(NCrystalSphereDialogue.PropertyName._hsv, ref variant5))
      return;
    this._hsv = ((Variant) ref variant5).As<ShaderMaterial>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName PlayStart = StringName.op_Implicit(nameof (PlayStart));
    public static readonly StringName PlayBad = StringName.op_Implicit(nameof (PlayBad));
    public static readonly StringName PlayGood = StringName.op_Implicit(nameof (PlayGood));
    public static readonly StringName PlayEnd = StringName.op_Implicit(nameof (PlayEnd));
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
