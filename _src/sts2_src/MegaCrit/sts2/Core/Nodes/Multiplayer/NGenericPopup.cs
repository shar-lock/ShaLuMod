// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NGenericPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NGenericPopup.cs")]
public class NGenericPopup : Control, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/generic_popup");
  private NVerticalPopup _verticalPopup;
  private TaskCompletionSource<bool> _confirmationCompletionSource;
  private ulong _steamId;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NGenericPopup._scenePath);
    }
  }

  public static NGenericPopup? Create()
  {
    return TestMode.IsOn ? (NGenericPopup) null : PreloadManager.Cache.GetScene(NGenericPopup._scenePath).Instantiate<NGenericPopup>((PackedScene.GenEditState) 0L);
  }

  public Task<bool> WaitForConfirmation(
    LocString body,
    LocString header,
    LocString? noButton,
    LocString yesButton)
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._confirmationCompletionSource = new TaskCompletionSource<bool>();
    this._verticalPopup.SetText(header, body);
    this._verticalPopup.InitYesButton(yesButton, new Action<NButton>(this.OnYesButtonPressed));
    if (noButton != null)
      this._verticalPopup.InitNoButton(noButton, new Action<NButton>(this.OnNoButtonPressed));
    else
      this._verticalPopup.HideNoButton();
    return this._confirmationCompletionSource.Task;
  }

  private void OnYesButtonPressed(NButton _)
  {
    this._confirmationCompletionSource.SetResult(true);
    ((Node) this).QueueFreeSafely();
  }

  private void OnNoButtonPressed(NButton _)
  {
    this._confirmationCompletionSource.SetResult(false);
    ((Node) this).QueueFreeSafely();
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NGenericPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGenericPopup.MethodName.OnYesButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGenericPopup.MethodName.OnNoButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGenericPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGenericPopup ngenericPopup = NGenericPopup.Create();
      ret = VariantUtils.CreateFrom<NGenericPopup>(ref ngenericPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NGenericPopup.MethodName.OnYesButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnYesButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGenericPopup.MethodName.OnNoButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnNoButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGenericPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGenericPopup ngenericPopup = NGenericPopup.Create();
      ret = VariantUtils.CreateFrom<NGenericPopup>(ref ngenericPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGenericPopup.MethodName.Create) || StringName.op_Equality(ref method, NGenericPopup.MethodName.OnYesButtonPressed) || StringName.op_Equality(ref method, NGenericPopup.MethodName.OnNoButtonPressed) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGenericPopup.PropertyName._verticalPopup))
    {
      this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGenericPopup.PropertyName._steamId))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._steamId = VariantUtils.ConvertTo<ulong>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGenericPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGenericPopup.PropertyName._verticalPopup))
    {
      value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGenericPopup.PropertyName._steamId))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ulong>(ref this._steamId);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGenericPopup.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NGenericPopup.PropertyName._steamId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGenericPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGenericPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
    info.AddProperty(NGenericPopup.PropertyName._steamId, Variant.From<ulong>(ref this._steamId));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGenericPopup.PropertyName._verticalPopup, ref variant1))
      this._verticalPopup = ((Variant) ref variant1).As<NVerticalPopup>();
    Variant variant2;
    if (!info.TryGetProperty(NGenericPopup.PropertyName._steamId, ref variant2))
      return;
    this._steamId = ((Variant) ref variant2).As<ulong>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName OnYesButtonPressed = StringName.op_Implicit(nameof (OnYesButtonPressed));
    public static readonly StringName OnNoButtonPressed = StringName.op_Implicit(nameof (OnNoButtonPressed));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
    public static readonly StringName _steamId = StringName.op_Implicit(nameof (_steamId));
  }

  public class SignalName : Control.SignalName
  {
  }
}
