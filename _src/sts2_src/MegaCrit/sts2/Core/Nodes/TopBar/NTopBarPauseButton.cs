// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.TopBar.NTopBarPauseButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.TopBar;

[ScriptPath("res://src/Core/Nodes/TopBar/NTopBarPauseButton.cs")]
public class NTopBarPauseButton : NTopBarButton
{
  private static readonly StringName _v = new StringName("v");
  private const float _hoverAngle = -3.14159274f;
  private const float _hoverShaderV = 1.1f;
  private const float _defaultV = 0.9f;
  private const float _pressDownV = 0.4f;
  private IRunState _runState;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.pauseAndBack)
      };
    }
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    if (this.IsOpen())
      NCapstoneContainer.Instance.Close();
    else
      ((NPauseMenu) NRun.Instance.GlobalUi.SubmenuStack.ShowScreen(CapstoneSubmenuType.PauseMenu)).Initialize(this._runState);
    this.UpdateScreenOpen();
    this._hsv?.SetShaderParameter(NTopBarPauseButton._v, Variant.op_Implicit(0.9f));
  }

  protected override bool IsOpen()
  {
    return NCapstoneContainer.Instance.CurrentCapstoneScreen is NCapstoneSubmenuStack currentCapstoneScreen && currentCapstoneScreen.ScreenType == NetScreenType.PauseMenu;
  }

  public override void _Process(double delta)
  {
    if (!this.IsScreenOpen)
      return;
    this._icon.Rotation += (float) delta;
  }

  public void Initialize(IRunState runState) => this._runState = runState;

  protected override async Task AnimPressDown(CancellationTokenSource cancelToken)
  {
    if (!((Node) this._icon).IsValid())
      return;
    float num1 = 0.0f;
    float startAngle = this._icon.Rotation;
    float targetAngle = startAngle + 0.7853982f;
    float num;
    for (; (double) num1 < 0.25; num1 = num + await ((Node) this).AwaitProcessFrame(cancelToken.Token))
    {
      this._icon.Rotation = Mathf.LerpAngle(startAngle, targetAngle, Ease.CubicOut(num1 / 0.25f));
      this._hsv?.SetShaderParameter(NTopBarPauseButton._v, Variant.op_Implicit(Mathf.Lerp(1.1f, 0.4f, Ease.CubicOut(num1 / 0.25f))));
      num = num1;
    }
    this._icon.Rotation = targetAngle;
    this._hsv?.SetShaderParameter(NTopBarPauseButton._v, Variant.op_Implicit(0.4f));
  }

  protected override async Task AnimHover(CancellationTokenSource cancelToken)
  {
    if (!((Node) this._icon).IsValid())
      return;
    float num1 = 0.0f;
    float startAngle = this._icon.Rotation;
    float num;
    for (; (double) num1 < 0.5; num1 = num + await ((Node) this).AwaitProcessFrame(cancelToken.Token))
    {
      this._icon.Rotation = Mathf.LerpAngle(startAngle, -3.14159274f, Ease.BackOut(num1 / 0.5f));
      num = num1;
    }
    this._icon.Rotation = -3.14159274f;
  }

  protected override async Task AnimUnhover(CancellationTokenSource cancelToken)
  {
    if (!((Node) this._icon).IsValid())
      return;
    float num1 = 0.0f;
    float startAngle = this._icon.Rotation;
    float num;
    for (; (double) num1 < 1.0; num1 = num + await ((Node) this).AwaitProcessFrame(cancelToken.Token))
    {
      this._icon.Rotation = Mathf.LerpAngle(startAngle, 0.0f, Ease.ElasticOut(num1 / 1f));
      this._hsv?.SetShaderParameter(NTopBarPauseButton._v, Variant.op_Implicit(Mathf.Lerp(1.1f, 0.9f, Ease.ExpoOut(num1 / 1f))));
      this._icon.Scale = ((Vector2) ref NTopBarButton._hoverScale).Lerp(Vector2.One, Ease.ExpoOut(num1 / 1f));
      num = num1;
    }
    this._hsv?.SetShaderParameter(NTopBarPauseButton._v, Variant.op_Implicit(0.9f));
    this._icon.Rotation = 0.0f;
    this._icon.Scale = Vector2.One;
  }

  public void ToggleAnimState() => this.UpdateScreenOpen();

  protected override void OnFocus()
  {
    base.OnFocus();
    NHoverTipSet andShow = NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(new LocString("static_hover_tips", "SETTINGS.title"), new LocString("static_hover_tips", "SETTINGS.description")));
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
      new MethodInfo(NTopBarPauseButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarPauseButton.MethodName.IsOpen, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarPauseButton.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NTopBarPauseButton.MethodName.ToggleAnimState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarPauseButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTopBarPauseButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.IsOpen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsOpen();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarPauseButton.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.ToggleAnimState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ToggleAnimState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.IsOpen) || StringName.op_Equality(ref method, NTopBarPauseButton.MethodName._Process) || StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.ToggleAnimState) || StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NTopBarPauseButton.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NTopBarPauseButton.PropertyName.Hotkeys))
      return base.GetGodotClassPropertyValue(in name, out value);
    ref godot_variant local = ref value;
    string[] hotkeys = this.Hotkeys;
    godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NTopBarPauseButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NTopBarButton.MethodName
  {
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName ToggleAnimState = StringName.op_Implicit(nameof (ToggleAnimState));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NTopBarButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
  }

  public new class SignalName : NTopBarButton.SignalName
  {
  }
}
