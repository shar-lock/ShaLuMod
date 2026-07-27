// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsTickbox.cs")]
public class NSettingsTickbox : NTickbox
{
  private NSettingsScreen _settingsScreen;
  private NSelectionReticle _selectionReticle;

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._selectionReticle.OnDeselect();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NSettingsTickbox.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTickbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTickbox.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsTickbox.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTickbox.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsTickbox.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsTickbox.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NSettingsTickbox.MethodName.OnFocus) || StringName.op_Equality(ref method, NSettingsTickbox.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsTickbox.PropertyName._settingsScreen))
    {
      this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsTickbox.PropertyName._selectionReticle))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsTickbox.PropertyName._settingsScreen))
    {
      value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsTickbox.PropertyName._selectionReticle))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsTickbox.PropertyName._settingsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTickbox.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSettingsTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
    info.AddProperty(NSettingsTickbox.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsTickbox.PropertyName._settingsScreen, ref variant1))
      this._settingsScreen = ((Variant) ref variant1).As<NSettingsScreen>();
    Variant variant2;
    if (!info.TryGetProperty(NSettingsTickbox.PropertyName._selectionReticle, ref variant2))
      return;
    this._selectionReticle = ((Variant) ref variant2).As<NSelectionReticle>();
  }

  public new class MethodName : NTickbox.MethodName
  {
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NTickbox.PropertyName
  {
    public static readonly StringName _settingsScreen = StringName.op_Implicit(nameof (_settingsScreen));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
  }

  public new class SignalName : NTickbox.SignalName
  {
  }
}
