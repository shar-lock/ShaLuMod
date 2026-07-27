// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.RichTextTags.RichTextAncientBanner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.RichTextTags;

[GlobalClass]
[Tool]
[ScriptPath("res://src/Core/RichTextTags/RichTextAncientBanner.cs")]
public class RichTextAncientBanner : AbstractMegaRichTextEffect
{
  public string bbcode = "ancient_banner";

  public float Rotation { get; set; }

  public float Spacing { get; set; }

  public float CenterCharacter { get; set; }

  protected override string Bbcode => this.bbcode;

  public override bool _ProcessCustomFX(CharFXTransform charFx)
  {
    if (this.ShouldTransformText())
    {
      float num = (float) charFx.RelativeIndex + 0.5f - this.CenterCharacter;
      CharFXTransform charFxTransform1 = charFx;
      Transform2D transform1 = charFx.Transform;
      ref Transform2D local1 = ref transform1;
      Vector2 x = charFx.Transform.X;
      x.X = this.Rotation;
      Vector2 vector2_1 = x;
      local1.X = vector2_1;
      Transform2D transform2D1 = transform1;
      charFxTransform1.Transform = transform2D1;
      CharFXTransform charFxTransform2 = charFx;
      Transform2D transform2 = charFx.Transform;
      ref Transform2D local2 = ref transform2;
      Vector2 origin = charFx.Transform.Origin;
      origin.X = charFx.Transform.Origin.X + num * this.Spacing;
      Vector2 vector2_2 = origin;
      local2.Origin = vector2_2;
      Transform2D transform2D2 = transform2;
      charFxTransform2.Transform = transform2D2;
    }
    else
    {
      double num = charFx.ElapsedTime * 3.0 - (double) charFx.RelativeIndex * 0.014999999664723873;
      Color color = charFx.Color;
      color.A = Mathf.Clamp((float) num, 0.0f, 1f);
      charFx.Color = color;
    }
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(RichTextAncientBanner.MethodName._ProcessCustomFX, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("charFx"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("CharFXTransform"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, RichTextAncientBanner.MethodName._ProcessCustomFX) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = base._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, RichTextAncientBanner.MethodName._ProcessCustomFX) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.Rotation))
    {
      this.Rotation = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.Spacing))
    {
      this.Spacing = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.CenterCharacter))
    {
      this.CenterCharacter = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.bbcode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.bbcode = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.Rotation))
    {
      ref godot_variant local = ref value;
      float rotation = this.Rotation;
      godot_variant from = VariantUtils.CreateFrom<float>(ref rotation);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.Spacing))
    {
      ref godot_variant local = ref value;
      float spacing = this.Spacing;
      godot_variant from = VariantUtils.CreateFrom<float>(ref spacing);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.CenterCharacter))
    {
      ref godot_variant local = ref value;
      float centerCharacter = this.CenterCharacter;
      godot_variant from = VariantUtils.CreateFrom<float>(ref centerCharacter);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.Bbcode))
    {
      ref godot_variant local = ref value;
      string bbcode = this.Bbcode;
      godot_variant from = VariantUtils.CreateFrom<string>(ref bbcode);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, RichTextAncientBanner.PropertyName.bbcode))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string>(ref this.bbcode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, RichTextAncientBanner.PropertyName.Rotation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, RichTextAncientBanner.PropertyName.Spacing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, RichTextAncientBanner.PropertyName.CenterCharacter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, RichTextAncientBanner.PropertyName.bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, RichTextAncientBanner.PropertyName.Bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName rotation1 = RichTextAncientBanner.PropertyName.Rotation;
    float rotation2 = this.Rotation;
    Variant variant1 = Variant.From<float>(ref rotation2);
    serializationInfo1.AddProperty(rotation1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName spacing1 = RichTextAncientBanner.PropertyName.Spacing;
    float spacing2 = this.Spacing;
    Variant variant2 = Variant.From<float>(ref spacing2);
    serializationInfo2.AddProperty(spacing1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName centerCharacter1 = RichTextAncientBanner.PropertyName.CenterCharacter;
    float centerCharacter2 = this.CenterCharacter;
    Variant variant3 = Variant.From<float>(ref centerCharacter2);
    serializationInfo3.AddProperty(centerCharacter1, variant3);
    info.AddProperty(RichTextAncientBanner.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(RichTextAncientBanner.PropertyName.Rotation, ref variant1))
      this.Rotation = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(RichTextAncientBanner.PropertyName.Spacing, ref variant2))
      this.Spacing = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(RichTextAncientBanner.PropertyName.CenterCharacter, ref variant3))
      this.CenterCharacter = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (!info.TryGetProperty(RichTextAncientBanner.PropertyName.bbcode, ref variant4))
      return;
    this.bbcode = ((Variant) ref variant4).As<string>();
  }

  public new class MethodName : AbstractMegaRichTextEffect.MethodName
  {
    public static readonly StringName _ProcessCustomFX = StringName.op_Implicit(nameof (_ProcessCustomFX));
  }

  public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
  {
    public static readonly StringName Rotation = StringName.op_Implicit(nameof (Rotation));
    public static readonly StringName Spacing = StringName.op_Implicit(nameof (Spacing));
    public static readonly StringName CenterCharacter = StringName.op_Implicit(nameof (CenterCharacter));
    public new static readonly StringName Bbcode = StringName.op_Implicit(nameof (Bbcode));
    public new static readonly StringName bbcode = StringName.op_Implicit(nameof (bbcode));
  }

  public new class SignalName : AbstractMegaRichTextEffect.SignalName
  {
  }
}
