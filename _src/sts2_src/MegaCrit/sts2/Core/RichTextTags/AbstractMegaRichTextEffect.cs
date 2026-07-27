// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.RichTextTags.AbstractMegaRichTextEffect
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.RichTextTags;

[ScriptPath("res://src/Core/RichTextTags/AbstractMegaRichTextEffect.cs")]
public abstract class AbstractMegaRichTextEffect : RichTextEffect
{
  public string bbcode => this.Bbcode;

  protected abstract string Bbcode { get; }

  protected bool ShouldTransformText()
  {
    return Engine.IsEditorHint() || SaveManager.Instance.PrefsSave.TextEffectsEnabled;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(AbstractMegaRichTextEffect.MethodName.ShouldTransformText, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, AbstractMegaRichTextEffect.MethodName.ShouldTransformText) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    bool flag = this.ShouldTransformText();
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, AbstractMegaRichTextEffect.MethodName.ShouldTransformText) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, AbstractMegaRichTextEffect.PropertyName.bbcode))
    {
      ref godot_variant local = ref value;
      string bbcode = this.bbcode;
      godot_variant from = VariantUtils.CreateFrom<string>(ref bbcode);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, AbstractMegaRichTextEffect.PropertyName.Bbcode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    ref godot_variant local1 = ref value;
    string bbcode1 = this.Bbcode;
    godot_variant from1 = VariantUtils.CreateFrom<string>(ref bbcode1);
    local1 = from1;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, AbstractMegaRichTextEffect.PropertyName.bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, AbstractMegaRichTextEffect.PropertyName.Bbcode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : RichTextEffect.MethodName
  {
    public static readonly StringName ShouldTransformText = StringName.op_Implicit(nameof (ShouldTransformText));
  }

  public class PropertyName : RichTextEffect.PropertyName
  {
    public static readonly StringName bbcode = StringName.op_Implicit(nameof (bbcode));
    public static readonly StringName Bbcode = StringName.op_Implicit(nameof (Bbcode));
  }

  public class SignalName : RichTextEffect.SignalName
  {
  }
}
