// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.LocalizedTexture
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/LocalizedTexture.cs")]
public class LocalizedTexture : Resource
{
  [Export]
  private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();

  public bool TryGetTexture(out Texture2D? texture)
  {
    texture = (Texture2D) null;
    if (SaveManager.Instance.SettingsSave == null)
      return false;
    string language = SaveManager.Instance.SettingsSave.Language;
    Texture2D texture2D;
    if (string.IsNullOrEmpty(language) || !this._textures.TryGetValue(language, ref texture2D))
      return false;
    texture = texture2D;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, LocalizedTexture.PropertyName._textures))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._textures = VariantUtils.ConvertToDictionary<string, Texture2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, LocalizedTexture.PropertyName._textures))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromDictionary<string, Texture2D>(this._textures);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 27L, LocalizedTexture.PropertyName._textures, (PropertyHint) 23L, "4/0:;24/17:Texture2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(LocalizedTexture.PropertyName._textures, Variant.CreateFrom<string, Texture2D>(this._textures));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(LocalizedTexture.PropertyName._textures, ref variant))
      return;
    this._textures = ((Variant) ref variant).AsGodotDictionary<string, Texture2D>();
  }

  public class MethodName : Resource.MethodName
  {
  }

  public class PropertyName : Resource.PropertyName
  {
    public static readonly StringName _textures = StringName.op_Implicit(nameof (_textures));
  }

  public class SignalName : Resource.SignalName
  {
  }
}
