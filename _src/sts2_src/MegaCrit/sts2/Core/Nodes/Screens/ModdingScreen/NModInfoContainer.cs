// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen.NModInfoContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;

[ScriptPath("res://src/Core/Nodes/Screens/ModdingScreen/NModInfoContainer.cs")]
public class NModInfoContainer : Control
{
  private MegaRichTextLabel _title;
  private TextureRect _image;
  private MegaRichTextLabel _description;

  public override void _Ready()
  {
    this._title = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ModTitle"));
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("ModImage"));
    this._description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ModDescription"));
    this._title.Text = "";
    this._image.Texture = (Texture2D) null;
    this._description.Text = "";
  }

  public void Clear()
  {
    this._image.Texture = (Texture2D) null;
    this._title.Text = "";
    this._description.Text = "";
  }

  public void Fill(Mod mod)
  {
    this._title.Text = mod.manifest?.name ?? "<No Name>";
    string path = $"res://{mod.manifest?.id}/mod_image.png";
    this._image.Texture = !ResourceLoader.Exists(path, "") ? (Texture2D) null : PreloadManager.Cache.GetAsset<Texture2D>(path);
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(21, 1, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[gold]Author[/gold]: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(mod.manifest?.author ?? "unknown");
    ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    stringBuilder3.AppendLine(ref local1);
    StringBuilder stringBuilder4 = stringBuilder1;
    StringBuilder stringBuilder5 = stringBuilder4;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(22, 1, stringBuilder4);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[gold]Version[/gold]: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(mod.manifest?.version ?? "unknown");
    ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
    stringBuilder5.AppendLine(ref local2);
    stringBuilder1.AppendLine();
    StringBuilder stringBuilder6 = stringBuilder1;
    StringBuilder stringBuilder7 = stringBuilder6;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(0, 1, stringBuilder6);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(mod.manifest?.description ?? "No description");
    ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
    stringBuilder7.AppendLine(ref local3);
    List<LocString> errors = mod.errors;
    // ISSUE: explicit non-virtual call
    if ((errors != null ? (__nonvirtual (errors.Count) > 0 ? 1 : 0) : 0) != 0)
    {
      stringBuilder1.AppendLine();
      foreach (LocString error in mod.errors)
      {
        StringBuilder stringBuilder8 = stringBuilder1;
        StringBuilder stringBuilder9 = stringBuilder8;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder8);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[red]");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(error.GetFormattedText());
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("[/red]");
        ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
        stringBuilder9.AppendLine(ref local4);
      }
    }
    this._description.Text = stringBuilder1.ToString();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NModInfoContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NModInfoContainer.MethodName.Clear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NModInfoContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NModInfoContainer.MethodName.Clear) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Clear();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NModInfoContainer.MethodName._Ready) || StringName.op_Equality(ref method, NModInfoContainer.MethodName.Clear) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModInfoContainer.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NModInfoContainer.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModInfoContainer.PropertyName._description))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NModInfoContainer.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NModInfoContainer.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (!StringName.op_Equality(ref name, NModInfoContainer.PropertyName._description))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NModInfoContainer.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModInfoContainer.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NModInfoContainer.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NModInfoContainer.PropertyName._title, Variant.From<MegaRichTextLabel>(ref this._title));
    info.AddProperty(NModInfoContainer.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NModInfoContainer.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NModInfoContainer.PropertyName._title, ref variant1))
      this._title = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NModInfoContainer.PropertyName._image, ref variant2))
      this._image = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (!info.TryGetProperty(NModInfoContainer.PropertyName._description, ref variant3))
      return;
    this._description = ((Variant) ref variant3).As<MegaRichTextLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Clear = StringName.op_Implicit(nameof (Clear));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
  }

  public class SignalName : Control.SignalName
  {
  }
}
