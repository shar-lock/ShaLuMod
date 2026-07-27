// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.RichTextTags.RichTextThinkyDots
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.RichTextTags;

[GlobalClass]
[Tool]
[ScriptPath("res://src/Core/RichTextTags/RichTextThinkyDots.cs")]
public class RichTextThinkyDots : AbstractMegaRichTextEffect
{
  private const float _amplitude = 1.5f;
  private const float _frequency = 0.4f;
  private const float _speed = 1f;
  private const float _spacing = 4f;
  public string bbcode = "thinky_dots";

  protected override string Bbcode => this.bbcode;

  public override bool _ProcessCustomFX(CharFXTransform charFx)
  {
    if (!this.ShouldTransformText())
      return false;
    Dictionary env = charFx.Env;
    charFx.Offset = Vector2.Zero;
    float num1 = Math.Max((float) (charFx.ElapsedTime * 1.0 - (double) charFx.RelativeIndex * 0.10000000149011612), 0.0f) % 4.4f;
    float num2 = (double) num1 >= 0.40000000596046448 ? 0.0f : 1.5f * Mathf.Sin((float) ((double) num1 / 0.40000000596046448 * 3.1415927410125732));
    CharFXTransform charFxTransform = charFx;
    charFxTransform.Offset = Vector2.op_Addition(charFxTransform.Offset, new Vector2(0.0f, -Mathf.Max(num2, 0.0f)));
    Variant variant;
    if (env.TryGetValue(RichTextUtil.colorKey, ref variant))
      charFx.Color = Variant.op_Explicit(variant);
    charFx.Visible = !env.ContainsKey(RichTextUtil.visibleKey) || Variant.op_Explicit(env[RichTextUtil.visibleKey]);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(RichTextThinkyDots.MethodName._ProcessCustomFX, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (!StringName.op_Equality(ref method, RichTextThinkyDots.MethodName._ProcessCustomFX) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = base._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, RichTextThinkyDots.MethodName._ProcessCustomFX) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, RichTextThinkyDots.PropertyName.bbcode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.bbcode = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, RichTextThinkyDots.PropertyName.Bbcode))
    {
      ref godot_variant local = ref value;
      string bbcode = this.Bbcode;
      godot_variant from = VariantUtils.CreateFrom<string>(ref bbcode);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, RichTextThinkyDots.PropertyName.bbcode))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string>(ref this.bbcode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, RichTextThinkyDots.PropertyName.bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, RichTextThinkyDots.PropertyName.Bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(RichTextThinkyDots.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(RichTextThinkyDots.PropertyName.bbcode, ref variant))
      return;
    this.bbcode = ((Variant) ref variant).As<string>();
  }

  public new class MethodName : AbstractMegaRichTextEffect.MethodName
  {
    public static readonly StringName _ProcessCustomFX = StringName.op_Implicit(nameof (_ProcessCustomFX));
  }

  public new class PropertyName : AbstractMegaRichTextEffect.PropertyName
  {
    public new static readonly StringName Bbcode = StringName.op_Implicit(nameof (Bbcode));
    public new static readonly StringName bbcode = StringName.op_Implicit(nameof (bbcode));
  }

  public new class SignalName : AbstractMegaRichTextEffect.SignalName
  {
  }
}
