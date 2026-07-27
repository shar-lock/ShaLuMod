// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryLabelDivider
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLabelDivider.cs")]
public class NBestiaryLabelDivider : NButton
{
  private MegaRichTextLabel _nameLabel;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/bestiary/bestiary_label_divider");
  }

  private LocString LocString { get; set; }

  public static NBestiaryLabelDivider Create(ActModel act)
  {
    NBestiaryLabelDivider nbestiaryLabelDivider = PreloadManager.Cache.GetScene(NBestiaryLabelDivider.ScenePath).Instantiate<NBestiaryLabelDivider>((PackedScene.GenEditState) 0L);
    nbestiaryLabelDivider.LocString = act.Title;
    return nbestiaryLabelDivider;
  }

  public static NBestiaryLabelDivider Create(LocString locString)
  {
    NBestiaryLabelDivider nbestiaryLabelDivider = PreloadManager.Cache.GetScene(NBestiaryLabelDivider.ScenePath).Instantiate<NBestiaryLabelDivider>((PackedScene.GenEditState) 0L);
    nbestiaryLabelDivider.LocString = locString;
    return nbestiaryLabelDivider;
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._nameLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Label"));
    this._nameLabel.Text = this.LocString.GetFormattedText();
    ((CanvasItem) this._nameLabel).Modulate = StsColors.blue;
  }

  protected override void OnFocus()
  {
  }

  protected override void OnUnfocus()
  {
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBestiaryLabelDivider.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLabelDivider.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLabelDivider.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryLabelDivider.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryLabelDivider.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryLabelDivider.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryLabelDivider.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryLabelDivider.MethodName.OnFocus) || StringName.op_Equality(ref method, NBestiaryLabelDivider.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NBestiaryLabelDivider.PropertyName._nameLabel))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._nameLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NBestiaryLabelDivider.PropertyName._nameLabel))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._nameLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBestiaryLabelDivider.PropertyName._nameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBestiaryLabelDivider.PropertyName._nameLabel, Variant.From<MegaRichTextLabel>(ref this._nameLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NBestiaryLabelDivider.PropertyName._nameLabel, ref variant))
      return;
    this._nameLabel = ((Variant) ref variant).As<MegaRichTextLabel>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _nameLabel = StringName.op_Implicit(nameof (_nameLabel));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
