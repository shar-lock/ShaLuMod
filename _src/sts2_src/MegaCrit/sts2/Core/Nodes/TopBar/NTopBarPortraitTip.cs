// Decompiled with JetBrains decompiler
// Type: MegaCrit.sts2.Core.Nodes.TopBar.NTopBarPortraitTip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarPortraitTip.cs")]
public class NTopBarPortraitTip : NClickableControl
{
  private IHoverTip _hoverTip;

  public bool ShowTip { get; private set; }

  public override void _Ready() => this.ConnectSignals();

  public void Initialize(IRunState runState)
  {
    int ascensionLevel = runState.AscensionLevel;
    bool achievementsLocked = runState.GameMode.AreAchievementsAndEpochsLocked();
    this.ShowTip = ascensionLevel > 0 | achievementsLocked;
    if (this.ShowTip)
      this._hoverTip = (IHoverTip) AscensionHelper.GetHoverTip(LocalContext.GetMe((IPlayerCollection) runState).Character, ascensionLevel, achievementsLocked);
    this.FocusMode = this.ShowTip ? (Control.FocusModeEnum) 2L : (Control.FocusModeEnum) 0L;
  }

  protected override void OnFocus()
  {
    if (!this.ShowTip)
      return;
    NHoverTipSet.CreateAndShow((Control) this, this._hoverTip)?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(0.0f, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus() => NHoverTipSet.Remove((Control) this);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NTopBarPortraitTip.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarPortraitTip.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarPortraitTip.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarPortraitTip.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarPortraitTip.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarPortraitTip.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarPortraitTip.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarPortraitTip.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarPortraitTip.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarPortraitTip.PropertyName.ShowTip))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.ShowTip = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarPortraitTip.PropertyName.ShowTip))
      return base.GetGodotClassPropertyValue(in name, out value);
    ref godot_variant local = ref value;
    bool showTip = this.ShowTip;
    godot_variant from = VariantUtils.CreateFrom<bool>(ref showTip);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NTopBarPortraitTip.PropertyName.ShowTip, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName showTip1 = NTopBarPortraitTip.PropertyName.ShowTip;
    bool showTip2 = this.ShowTip;
    Variant variant = Variant.From<bool>(ref showTip2);
    serializationInfo.AddProperty(showTip1, variant);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NTopBarPortraitTip.PropertyName.ShowTip, ref variant))
      return;
    this.ShowTip = ((Variant) ref variant).As<bool>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName ShowTip = StringName.op_Implicit(nameof (ShowTip));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
