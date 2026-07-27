// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.TopBar.NTopBarMapButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarMapButton.cs")]
public class NTopBarMapButton : NTopBarButton
{
  private static readonly StringName _v = new StringName("v");
  private const float _defaultV = 0.9f;
  private Tween? _oscillateTween;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.viewMap)
      };
    }
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    if (this.IsOpen())
    {
      NCapstoneContainer instance = NCapstoneContainer.Instance;
      if ((instance != null ? (instance.InUse ? 1 : 0) : 0) != 0)
        NCapstoneContainer.Instance.Close();
      else
        NMapScreen.Instance.Close();
    }
    else
    {
      NCapstoneContainer.Instance.Close();
      NMapScreen.Instance.Open(true);
    }
    this._hsv?.SetShaderParameter(NTopBarMapButton._v, Variant.op_Implicit(0.9f));
  }

  protected override bool IsOpen() => ((CanvasItem) NMapScreen.Instance).Visible;

  public void StartOscillation()
  {
    this._oscillateTween?.Kill();
    this._oscillateTween = ((Node) this).CreateTween();
    this._oscillateTween.SetLoops(0);
    this._oscillateTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("rotation"), Variant.op_Implicit(-0.12f), 0.8).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
    this._oscillateTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("rotation"), Variant.op_Implicit(0.12f), 0.8).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
  }

  public void StopOscillation()
  {
    this._oscillateTween?.Kill();
    this._oscillateTween = ((Node) this).CreateTween();
    this._oscillateTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("rotation"), Variant.op_Implicit(0.0f), 0.5).SetTrans((Tween.TransitionType) 11L).SetEase((Tween.EaseType) 1L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    LocString title = new LocString("static_hover_tips", "MAP.title");
    title.Add("Hotkey", NInputManager.Instance.GetShortcutKey(MegaInput.viewMap).ToString());
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(title, new LocString("static_hover_tips", "MAP.description")));
    andShow?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(this.Size.X - andShow.Size.X, this.Size.Y + 20f)), false);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    NHoverTipSet.Remove((Control) this);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NTopBarMapButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarMapButton.MethodName.IsOpen, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarMapButton.MethodName.StartOscillation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarMapButton.MethodName.StopOscillation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarMapButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarMapButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarMapButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarMapButton.MethodName.IsOpen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsOpen();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarMapButton.MethodName.StartOscillation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartOscillation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarMapButton.MethodName.StopOscillation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopOscillation();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarMapButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarMapButton.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarMapButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NTopBarMapButton.MethodName.IsOpen) || StringName.op_Equality(ref method, NTopBarMapButton.MethodName.StartOscillation) || StringName.op_Equality(ref method, NTopBarMapButton.MethodName.StopOscillation) || StringName.op_Equality(ref method, NTopBarMapButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarMapButton.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarMapButton.PropertyName._oscillateTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._oscillateTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarMapButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarMapButton.PropertyName._oscillateTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._oscillateTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NTopBarMapButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarMapButton.PropertyName._oscillateTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NTopBarMapButton.PropertyName._oscillateTween, Variant.From<Tween>(ref this._oscillateTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NTopBarMapButton.PropertyName._oscillateTween, ref variant))
      return;
    this._oscillateTween = ((Variant) ref variant).As<Tween>();
  }

  public new class MethodName : NTopBarButton.MethodName
  {
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
    public static readonly StringName StartOscillation = StringName.op_Implicit(nameof (StartOscillation));
    public static readonly StringName StopOscillation = StringName.op_Implicit(nameof (StopOscillation));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NTopBarButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _oscillateTween = StringName.op_Implicit(nameof (_oscillateTween));
  }

  public new class SignalName : NTopBarButton.SignalName
  {
  }
}
