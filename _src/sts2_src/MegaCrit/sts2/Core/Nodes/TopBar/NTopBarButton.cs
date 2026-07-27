// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.TopBar.NTopBarButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarButton.cs")]
public abstract class NTopBarButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  protected Control _icon;
  protected ShaderMaterial? _hsv;
  private const float _hoverAngle = -0.209439516f;
  private const float _hoverShaderV = 1.1f;
  protected const float _hoverAnimDur = 0.5f;
  protected static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.1f);
  private CancellationTokenSource? _hoverAnimCancelToken;
  private const float _defaultV = 1f;
  protected const float _unhoverAnimDur = 1f;
  private CancellationTokenSource? _unhoverAnimCancelToken;
  private const float _pressDownV = 0.4f;
  protected const float _pressDownDur = 0.25f;
  private CancellationTokenSource? _pressDownCancelToken;

  protected bool IsScreenOpen { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._icon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Control/Icon"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
  }

  protected void InitTopBarButton()
  {
    this.ConnectSignals();
    this._icon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Control/Icon"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this.CancelAnimations();
  }

  protected override void OnRelease()
  {
    this._pressDownCancelToken?.Cancel();
    this._hoverAnimCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimHover(this._hoverAnimCancelToken));
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._hoverAnimCancelToken?.Cancel();
    this._pressDownCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimPressDown(this._pressDownCancelToken));
  }

  protected virtual async Task AnimPressDown(CancellationTokenSource cancelToken)
  {
    if (!((Node) this._icon).IsValid())
      return;
    float num1 = 0.0f;
    float startAngle = this._icon.Rotation;
    float targetAngle = startAngle + 0.418879032f;
    float num;
    for (; (double) num1 < 0.25; num1 = num + await ((Node) this).AwaitProcessFrame(cancelToken.Token))
    {
      this._icon.Rotation = Mathf.LerpAngle(startAngle, targetAngle, Ease.CubicOut(num1 / 0.25f));
      this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(Mathf.Lerp(1.1f, 0.4f, Ease.CubicOut(num1 / 0.25f))));
      num = num1;
    }
    this._icon.Rotation = targetAngle;
    this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(0.4f));
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    if (this.IsScreenOpen)
    {
      this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(1.1f));
      this._icon.Scale = NTopBarButton._hoverScale;
    }
    else
    {
      this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(1.1f));
      this._icon.Scale = NTopBarButton._hoverScale;
      this._unhoverAnimCancelToken?.Cancel();
      this._hoverAnimCancelToken?.Cancel();
      this._hoverAnimCancelToken = new CancellationTokenSource();
      TaskHelper.RunSafely(this.AnimHover(this._hoverAnimCancelToken));
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    ((CanvasItem) this).Modulate = Colors.White;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    ((CanvasItem) this).Modulate = StsColors.disabledTopBarButton;
  }

  protected virtual async Task AnimHover(CancellationTokenSource cancelToken)
  {
    if (!((Node) this._icon).IsValid())
      return;
    float num1 = 0.0f;
    float startAngle = this._icon.Rotation;
    float num;
    for (; (double) num1 < 0.5; num1 = num + await ((Node) this).AwaitProcessFrame(cancelToken.Token))
    {
      this._icon.Rotation = Mathf.LerpAngle(startAngle, -0.209439516f, Ease.BackOut(num1 / 0.5f));
      num = num1;
    }
    this._icon.Rotation = -0.209439516f;
  }

  protected override void OnUnfocus()
  {
    if (this.IsScreenOpen)
    {
      this._pressDownCancelToken?.Cancel();
      this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(1f));
      this._icon.Scale = Vector2.One;
    }
    else
    {
      this._hoverAnimCancelToken?.Cancel();
      this._pressDownCancelToken?.Cancel();
      this._unhoverAnimCancelToken?.Cancel();
      this._unhoverAnimCancelToken = new CancellationTokenSource();
      TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
    }
  }

  protected virtual async Task AnimUnhover(CancellationTokenSource cancelToken)
  {
    if (!((Node) this._icon).IsValid())
      return;
    float num1 = 0.0f;
    float startAngle = this._icon.Rotation;
    float num;
    for (; (double) num1 < 1.0; num1 = num + await ((Node) this).AwaitProcessFrame(cancelToken.Token))
    {
      this._icon.Rotation = Mathf.LerpAngle(startAngle, 0.0f, Ease.ElasticOut(num1 / 1f));
      this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(Mathf.Lerp(1.1f, 1f, Ease.ExpoOut(num1 / 1f))));
      this._icon.Scale = ((Vector2) ref NTopBarButton._hoverScale).Lerp(Vector2.One, Ease.ExpoOut(num1 / 1f));
      num = num1;
    }
    this._hsv?.SetShaderParameter(NTopBarButton._v, Variant.op_Implicit(1f));
    this._icon.Rotation = 0.0f;
    this._icon.Scale = Vector2.One;
  }

  protected void UpdateScreenOpen()
  {
    bool flag = this.IsOpen();
    if (this.IsScreenOpen == flag)
      return;
    this.IsScreenOpen = flag;
    if (this.IsScreenOpen)
      return;
    this.OnScreenClosed();
  }

  private void OnScreenClosed()
  {
    this.CancelAnimations();
    this._unhoverAnimCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
  }

  private void CancelAnimations()
  {
    this._hoverAnimCancelToken?.Cancel();
    this._pressDownCancelToken?.Cancel();
    this._unhoverAnimCancelToken?.Cancel();
  }

  protected abstract bool IsOpen();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NTopBarButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.InitTopBarButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.UpdateScreenOpen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.OnScreenClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.CancelAnimations, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarButton.MethodName.IsOpen, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.InitTopBarButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitTopBarButton();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.UpdateScreenOpen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateScreenOpen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.OnScreenClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnScreenClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarButton.MethodName.CancelAnimations) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelAnimations();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarButton.MethodName.IsOpen) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = this.IsOpen();
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarButton.MethodName._Ready) || StringName.op_Equality(ref method, NTopBarButton.MethodName.InitTopBarButton) || StringName.op_Equality(ref method, NTopBarButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnPress) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NTopBarButton.MethodName.UpdateScreenOpen) || StringName.op_Equality(ref method, NTopBarButton.MethodName.OnScreenClosed) || StringName.op_Equality(ref method, NTopBarButton.MethodName.CancelAnimations) || StringName.op_Equality(ref method, NTopBarButton.MethodName.IsOpen) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarButton.PropertyName.IsScreenOpen))
    {
      this.IsScreenOpen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarButton.PropertyName._hsv))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTopBarButton.PropertyName.IsScreenOpen))
    {
      ref godot_variant local = ref value;
      bool isScreenOpen = this.IsScreenOpen;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isScreenOpen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTopBarButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._icon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTopBarButton.PropertyName._hsv))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NTopBarButton.PropertyName.IsScreenOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTopBarButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isScreenOpen1 = NTopBarButton.PropertyName.IsScreenOpen;
    bool isScreenOpen2 = this.IsScreenOpen;
    Variant variant = Variant.From<bool>(ref isScreenOpen2);
    serializationInfo.AddProperty(isScreenOpen1, variant);
    info.AddProperty(NTopBarButton.PropertyName._icon, Variant.From<Control>(ref this._icon));
    info.AddProperty(NTopBarButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTopBarButton.PropertyName.IsScreenOpen, ref variant1))
      this.IsScreenOpen = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NTopBarButton.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NTopBarButton.PropertyName._hsv, ref variant3))
      return;
    this._hsv = ((Variant) ref variant3).As<ShaderMaterial>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName InitTopBarButton = StringName.op_Implicit(nameof (InitTopBarButton));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateScreenOpen = StringName.op_Implicit(nameof (UpdateScreenOpen));
    public static readonly StringName OnScreenClosed = StringName.op_Implicit(nameof (OnScreenClosed));
    public static readonly StringName CancelAnimations = StringName.op_Implicit(nameof (CancelAnimations));
    public static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsScreenOpen = StringName.op_Implicit(nameof (IsScreenOpen));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
