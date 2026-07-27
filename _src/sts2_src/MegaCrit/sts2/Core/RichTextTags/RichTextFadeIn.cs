// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.RichTextTags.RichTextFadeIn
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.RichTextTags;

[GlobalClass]
[Tool]
[ScriptPath("res://src/Core/RichTextTags/RichTextFadeIn.cs")]
public class RichTextFadeIn : AbstractMegaRichTextEffect
{
  private static readonly Variant _speedKey;
  private static readonly Variant _tickKey;
  public string bbcode = "fade_in";

  protected override string Bbcode => this.bbcode;

  public override bool _ProcessCustomFX(CharFXTransform charFx)
  {
    Dictionary env = charFx.Env;
    charFx.Offset = Vector2.Zero;
    Variant variant1;
    double num1 = !env.TryGetValue(RichTextFadeIn._speedKey, ref variant1) ? 4.0 : ((Variant) ref variant1).AsDouble();
    Variant variant2;
    double num2 = !env.TryGetValue(RichTextFadeIn._tickKey, ref variant2) ? 0.0099999997764825821 : ((Variant) ref variant2).AsDouble();
    double num3 = charFx.ElapsedTime * num1 - (double) charFx.RelativeIndex * num2;
    Color color = charFx.Color;
    color.A = Mathf.Clamp((float) num3, 0.0f, 1f);
    charFx.Color = color;
    Variant variant3;
    charFx.Visible = !env.TryGetValue(RichTextUtil.visibleKey, ref variant3) || ((Variant) ref variant3).AsBool();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(RichTextFadeIn.MethodName._ProcessCustomFX, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (!StringName.op_Equality(ref method, RichTextFadeIn.MethodName._ProcessCustomFX) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = base._ProcessCustomFX(VariantUtils.ConvertTo<CharFXTransform>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, RichTextFadeIn.MethodName._ProcessCustomFX) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, RichTextFadeIn.PropertyName.bbcode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this.bbcode = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, RichTextFadeIn.PropertyName.Bbcode))
    {
      ref godot_variant local = ref value;
      string bbcode = this.Bbcode;
      godot_variant from = VariantUtils.CreateFrom<string>(ref bbcode);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, RichTextFadeIn.PropertyName.bbcode))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string>(ref this.bbcode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, RichTextFadeIn.PropertyName.bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, RichTextFadeIn.PropertyName.Bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(RichTextFadeIn.PropertyName.bbcode, Variant.From<string>(ref this.bbcode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(RichTextFadeIn.PropertyName.bbcode, ref variant))
      return;
    this.bbcode = ((Variant) ref variant).As<string>();
  }

  static RichTextFadeIn()
  {
    string str1 = "speed";
    RichTextFadeIn._speedKey = Variant.From<string>(ref str1);
    string str2 = "tick";
    RichTextFadeIn._tickKey = Variant.From<string>(ref str2);
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
