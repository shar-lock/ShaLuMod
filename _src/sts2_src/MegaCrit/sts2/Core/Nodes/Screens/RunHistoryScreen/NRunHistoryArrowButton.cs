// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NRunHistoryArrowButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRunHistoryArrowButton.cs")]
public class NRunHistoryArrowButton : NGoldArrowButton
{
  private bool _isLeft;

  public bool IsLeft
  {
    get => this._isLeft;
    set
    {
      if (this._isLeft == value)
        return;
      this.UnregisterHotkeys();
      this._isLeft = value;
      this.RegisterHotkeys();
      this._icon.FlipH = !this.IsLeft;
      this.UpdateControllerButton();
    }
  }

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(this._isLeft ? MegaInput.viewDeckAndTabLeft : MegaInput.viewExhaustPileAndTabRight)
      };
    }
  }

  public override void _Ready()
  {
    base._Ready();
    this._icon.FlipH = !this.IsLeft;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NRunHistoryArrowButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NRunHistoryArrowButton.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunHistoryArrowButton.MethodName._Ready) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunHistoryArrowButton.PropertyName.IsLeft))
    {
      this.IsLeft = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunHistoryArrowButton.PropertyName._isLeft))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isLeft = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunHistoryArrowButton.PropertyName.IsLeft))
    {
      ref godot_variant local = ref value;
      bool isLeft = this.IsLeft;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isLeft);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistoryArrowButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunHistoryArrowButton.PropertyName._isLeft))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isLeft);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NRunHistoryArrowButton.PropertyName._isLeft, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRunHistoryArrowButton.PropertyName.IsLeft, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NRunHistoryArrowButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isLeft1 = NRunHistoryArrowButton.PropertyName.IsLeft;
    bool isLeft2 = this.IsLeft;
    Variant variant = Variant.From<bool>(ref isLeft2);
    serializationInfo.AddProperty(isLeft1, variant);
    info.AddProperty(NRunHistoryArrowButton.PropertyName._isLeft, Variant.From<bool>(ref this._isLeft));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunHistoryArrowButton.PropertyName.IsLeft, ref variant1))
      this.IsLeft = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (!info.TryGetProperty(NRunHistoryArrowButton.PropertyName._isLeft, ref variant2))
      return;
    this._isLeft = ((Variant) ref variant2).As<bool>();
  }

  public new class MethodName : NGoldArrowButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public new class PropertyName : NGoldArrowButton.PropertyName
  {
    public static readonly StringName IsLeft = StringName.op_Implicit(nameof (IsLeft));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _isLeft = StringName.op_Implicit(nameof (_isLeft));
  }

  public new class SignalName : NGoldArrowButton.SignalName
  {
  }
}
