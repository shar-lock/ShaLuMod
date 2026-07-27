// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NEndTurnLongPressBar
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
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NEndTurnLongPressBar.cs")]
public class NEndTurnLongPressBar : ColorRect
{
  private Control _outline;
  private double _pressTimer;
  private const double _longPressDuration = 0.5;
  private bool _isPressed;
  private const float _targetWidth = 204f;
  private NEndTurnButton _endTurnButton;
  private Tween? _tween;
  private bool _enabled = true;

  public override void _Ready()
  {
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BarOutline"));
  }

  public void Init(NEndTurnButton endTurnButton) => this._endTurnButton = endTurnButton;

  public void StartPress() => this._isPressed = true;

  public void CancelPress() => this._isPressed = false;

  public override void _Process(double delta)
  {
    if (!this._enabled)
      return;
    if (this._isPressed)
    {
      this._pressTimer += delta;
      if (this._pressTimer > 0.5)
      {
        this._enabled = false;
        ((Control) this).Size = new Vector2(204f, 6f);
        this._pressTimer = 0.0;
        this._endTurnButton.CallReleaseLogic();
        TaskHelper.RunSafely(this.PlayAnim());
      }
      else
        this.RecalculateBar();
    }
    else
    {
      if (this._pressTimer <= 0.0)
        return;
      this._pressTimer -= delta;
      if (this._pressTimer < 0.0)
      {
        this._pressTimer = 0.0;
        Color modulate = ((CanvasItem) this).Modulate;
        modulate.A = 0.0f;
        ((CanvasItem) this).Modulate = modulate;
      }
      else
        this.RecalculateBar();
    }
  }

  private void RecalculateBar()
  {
    float num = (float) (this._pressTimer / 0.5);
    ((Control) this).Size = new Vector2(num * 204f, 6f);
    this.Color = new Color(num * 2.5f, 0.6f + num, 0.6f, 1f);
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = Ease.CubicOut(num * 0.75f);
    ((CanvasItem) this).Modulate = modulate;
  }

  private async Task PlayAnim()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    this.Color = new Color(1f, 0.85f, 0.36f, 1f);
    this._isPressed = false;
    this._enabled = true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NEndTurnLongPressBar.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnLongPressBar.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("endTurnButton"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NEndTurnLongPressBar.MethodName.StartPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnLongPressBar.MethodName.CancelPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEndTurnLongPressBar.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEndTurnLongPressBar.MethodName.RecalculateBar, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.Init) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Init(VariantUtils.ConvertTo<NEndTurnButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.StartPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.CancelPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.RecalculateBar) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.RecalculateBar();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName._Ready) || StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.Init) || StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.StartPress) || StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.CancelPress) || StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName._Process) || StringName.op_Equality(ref method, NEndTurnLongPressBar.MethodName.RecalculateBar) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._pressTimer))
    {
      this._pressTimer = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._isPressed))
    {
      this._isPressed = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._endTurnButton))
    {
      this._endTurnButton = VariantUtils.ConvertTo<NEndTurnButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._enabled))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._enabled = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._pressTimer))
    {
      value = VariantUtils.CreateFrom<double>(ref this._pressTimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._isPressed))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isPressed);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._endTurnButton))
    {
      value = VariantUtils.CreateFrom<NEndTurnButton>(ref this._endTurnButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEndTurnLongPressBar.PropertyName._enabled))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._enabled);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEndTurnLongPressBar.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NEndTurnLongPressBar.PropertyName._pressTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEndTurnLongPressBar.PropertyName._isPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnLongPressBar.PropertyName._endTurnButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEndTurnLongPressBar.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NEndTurnLongPressBar.PropertyName._enabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEndTurnLongPressBar.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NEndTurnLongPressBar.PropertyName._pressTimer, Variant.From<double>(ref this._pressTimer));
    info.AddProperty(NEndTurnLongPressBar.PropertyName._isPressed, Variant.From<bool>(ref this._isPressed));
    info.AddProperty(NEndTurnLongPressBar.PropertyName._endTurnButton, Variant.From<NEndTurnButton>(ref this._endTurnButton));
    info.AddProperty(NEndTurnLongPressBar.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NEndTurnLongPressBar.PropertyName._enabled, Variant.From<bool>(ref this._enabled));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._outline, ref variant1))
      this._outline = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._pressTimer, ref variant2))
      this._pressTimer = ((Variant) ref variant2).As<double>();
    Variant variant3;
    if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._isPressed, ref variant3))
      this._isPressed = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._endTurnButton, ref variant4))
      this._endTurnButton = ((Variant) ref variant4).As<NEndTurnButton>();
    Variant variant5;
    if (info.TryGetProperty(NEndTurnLongPressBar.PropertyName._tween, ref variant5))
      this._tween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NEndTurnLongPressBar.PropertyName._enabled, ref variant6))
      return;
    this._enabled = ((Variant) ref variant6).As<bool>();
  }

  public class MethodName : ColorRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
    public static readonly StringName StartPress = StringName.op_Implicit(nameof (StartPress));
    public static readonly StringName CancelPress = StringName.op_Implicit(nameof (CancelPress));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName RecalculateBar = StringName.op_Implicit(nameof (RecalculateBar));
  }

  public class PropertyName : ColorRect.PropertyName
  {
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _pressTimer = StringName.op_Implicit(nameof (_pressTimer));
    public static readonly StringName _isPressed = StringName.op_Implicit(nameof (_isPressed));
    public static readonly StringName _endTurnButton = StringName.op_Implicit(nameof (_endTurnButton));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _enabled = StringName.op_Implicit(nameof (_enabled));
  }

  public class SignalName : ColorRect.SignalName
  {
  }
}
