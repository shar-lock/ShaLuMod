// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NErrorPopup
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NErrorPopup.cs")]
public class NErrorPopup : NVerticalPopup, IScreenContext
{
  private NVerticalPopup _verticalPopup;
  private string _title;
  private string _body;
  private LocString? _cancel;
  private bool _showReportBugButton;
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/error_popup");

  public Control? DefaultFocusedControl => (Control) null;

  public new static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NErrorPopup._scenePath);
    }
  }

  public override void _Ready()
  {
    this._verticalPopup = ((Node) this).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup"));
    this._verticalPopup.SetText(this._title, this._body);
    if (this._showReportBugButton)
      this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "NETWORK_ERROR.report_bug"), new Action<NButton>(this.OnReportBugButtonPressed));
    else
      this._verticalPopup.InitYesButton(new LocString("main_menu_ui", "GENERIC_POPUP.ok"), new Action<NButton>(this.OnOkButtonPressed));
    if (this._cancel != null)
      this._verticalPopup.InitNoButton(this._cancel, new Action<NButton>(this.OnCancelButtonPressed));
    else
      this._verticalPopup.HideNoButton();
  }

  public static NErrorPopup? Create(NetErrorInfo info)
  {
    if (TestMode.IsOn)
      return (NErrorPopup) null;
    bool showReportBugButton;
    return info.SelfInitiated && info.GetReason() == NetError.Quit ? (NErrorPopup) null : NErrorPopup.Create(new LocString("main_menu_ui", "NETWORK_ERROR.header"), NErrorPopup.LocStringFromNetError(info, out showReportBugButton), (LocString) null, showReportBugButton);
  }

  public static NErrorPopup? Create(
    LocString title,
    LocString body,
    LocString? cancel,
    bool showReportBugButton)
  {
    if (TestMode.IsOn)
      return (NErrorPopup) null;
    NErrorPopup nerrorPopup = PreloadManager.Cache.GetScene(NErrorPopup._scenePath).Instantiate<NErrorPopup>((PackedScene.GenEditState) 0L);
    nerrorPopup._title = title.GetFormattedText();
    nerrorPopup._body = body.GetFormattedText();
    nerrorPopup._showReportBugButton = showReportBugButton;
    nerrorPopup._cancel = cancel;
    return nerrorPopup;
  }

  public static NErrorPopup? Create(string title, string body, bool showReportBugButton)
  {
    if (TestMode.IsOn)
      return (NErrorPopup) null;
    NErrorPopup nerrorPopup = PreloadManager.Cache.GetScene(NErrorPopup._scenePath).Instantiate<NErrorPopup>((PackedScene.GenEditState) 0L);
    nerrorPopup._title = title;
    nerrorPopup._body = body;
    nerrorPopup._showReportBugButton = showReportBugButton;
    return nerrorPopup;
  }

  private static LocString LocStringFromNetError(NetErrorInfo info, out bool showReportBugButton)
  {
    NetError reason = info.GetReason();
    string str;
    switch (reason)
    {
      case NetError.None:
        str = (string) null;
        break;
      case NetError.Quit:
        str = "NETWORK_ERROR.QUIT.body";
        break;
      case NetError.QuitGameOver:
        str = (string) null;
        break;
      case NetError.HostAbandoned:
        str = "NETWORK_ERROR.HOST_ABANDONED.body";
        break;
      case NetError.Kicked:
        str = "NETWORK_ERROR.KICKED.body";
        break;
      case NetError.InvalidJoin:
        str = "NETWORK_ERROR.INVALID_JOIN.body";
        break;
      case NetError.CancelledJoin:
        str = (string) null;
        break;
      case NetError.LobbyFull:
        str = "NETWORK_ERROR.LOBBY_FULL.body";
        break;
      case NetError.RunInProgress:
        str = "NETWORK_ERROR.RUN_IN_PROGRESS.body";
        break;
      case NetError.NotInSaveGame:
        str = "NETWORK_ERROR.NOT_IN_SAVE_GAME.body";
        break;
      case NetError.VersionMismatch:
        str = "NETWORK_ERROR.VERSION_MISMATCH.body";
        break;
      case NetError.JoinBlockedByUser:
        str = "NETWORK_ERROR.JOIN_BLOCKED_BY_USER.body";
        break;
      case NetError.StateDivergence:
        str = "NETWORK_ERROR.STATE_DIVERGENCE.body";
        break;
      case NetError.HandshakeTimeout:
        str = "NETWORK_ERROR.TIMEOUT.body";
        break;
      case NetError.ModMismatch:
        str = "NETWORK_ERROR.MOD_MISMATCH.body";
        break;
      case NetError.NoInternet:
        str = "NETWORK_ERROR.NO_INTERNET.body";
        break;
      case NetError.Timeout:
        str = "NETWORK_ERROR.TIMEOUT.body";
        break;
      case NetError.InternalError:
        str = "NETWORK_ERROR.INTERNAL_ERROR.body";
        break;
      case NetError.UnknownNetworkError:
        str = "NETWORK_ERROR.UNKNOWN_ERROR.body";
        break;
      case NetError.RateLimited:
        str = "NETWORK_ERROR.RATE_LIMITED.body";
        break;
      case NetError.TryAgainLater:
        str = "NETWORK_ERROR.TRY_AGAIN_LATER.body";
        break;
      case NetError.FailedToHost:
        str = "NETWORK_ERROR.FAILED_TO_HOST.body";
        break;
      case NetError.SecureConnectionFailed:
        str = "NETWORK_ERROR.SECURE_CONNECTION_FAILED.body";
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) reason);
        break;
    }
    string locEntryKey = str;
    bool flag;
    switch (reason)
    {
      case NetError.None:
      case NetError.StateDivergence:
      case NetError.InternalError:
      case NetError.UnknownNetworkError:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    showReportBugButton = flag;
    if (locEntryKey == null)
    {
      Log.Error($"Invalid net error passed to {nameof (NErrorPopup)}: {info}!");
      locEntryKey = "NETWORK_ERROR.INTERNAL_ERROR.body";
      showReportBugButton = true;
    }
    LocString locString = new LocString("main_menu_ui", locEntryKey);
    locString.Add(nameof (info), info.GetErrorString());
    return locString;
  }

  private void OnOkButtonPressed(NButton _) => ((Node) this).QueueFreeSafely();

  private void OnCancelButtonPressed(NButton _) => ((Node) this).QueueFreeSafely();

  private void OnReportBugButtonPressed(NButton _)
  {
    TaskHelper.RunSafely(this.OpenFeedbackScreen());
  }

  private async Task OpenFeedbackScreen()
  {
    SceneTree sceneTree = ((Node) this).GetTree();
    ((Node) this).QueueFreeSafely();
    Variant[] signal1 = await ((GodotObject) sceneTree).ToSignal((GodotObject) sceneTree, SceneTree.SignalName.ProcessFrame);
    Variant[] signal2 = await ((GodotObject) sceneTree).ToSignal((GodotObject) sceneTree, SceneTree.SignalName.ProcessFrame);
    await NFeedbackScreenOpener.Instance.OpenFeedbackScreen();
    sceneTree = (SceneTree) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NErrorPopup.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NErrorPopup.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("title"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("body"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("showReportBugButton"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NErrorPopup.MethodName.OnOkButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NErrorPopup.MethodName.OnCancelButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NErrorPopup.MethodName.OnReportBugButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NErrorPopup.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NErrorPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NErrorPopup nerrorPopup = NErrorPopup.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NErrorPopup>(ref nerrorPopup);
      return true;
    }
    if (StringName.op_Equality(ref method, NErrorPopup.MethodName.OnOkButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnOkButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NErrorPopup.MethodName.OnCancelButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCancelButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NErrorPopup.MethodName.OnReportBugButtonPressed) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnReportBugButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NErrorPopup.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      NErrorPopup nerrorPopup = NErrorPopup.Create(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = VariantUtils.CreateFrom<NErrorPopup>(ref nerrorPopup);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NErrorPopup.MethodName._Ready) || StringName.op_Equality(ref method, NErrorPopup.MethodName.Create) || StringName.op_Equality(ref method, NErrorPopup.MethodName.OnOkButtonPressed) || StringName.op_Equality(ref method, NErrorPopup.MethodName.OnCancelButtonPressed) || StringName.op_Equality(ref method, NErrorPopup.MethodName.OnReportBugButtonPressed) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName._verticalPopup))
    {
      this._verticalPopup = VariantUtils.ConvertTo<NVerticalPopup>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName._body))
    {
      this._body = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NErrorPopup.PropertyName._showReportBugButton))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._showReportBugButton = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName._verticalPopup))
    {
      value = VariantUtils.CreateFrom<NVerticalPopup>(ref this._verticalPopup);
      return true;
    }
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<string>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NErrorPopup.PropertyName._body))
    {
      value = VariantUtils.CreateFrom<string>(ref this._body);
      return true;
    }
    if (!StringName.op_Equality(ref name, NErrorPopup.PropertyName._showReportBugButton))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._showReportBugButton);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NErrorPopup.PropertyName._verticalPopup, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NErrorPopup.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NErrorPopup.PropertyName._body, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NErrorPopup.PropertyName._showReportBugButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NErrorPopup.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NErrorPopup.PropertyName._verticalPopup, Variant.From<NVerticalPopup>(ref this._verticalPopup));
    info.AddProperty(NErrorPopup.PropertyName._title, Variant.From<string>(ref this._title));
    info.AddProperty(NErrorPopup.PropertyName._body, Variant.From<string>(ref this._body));
    info.AddProperty(NErrorPopup.PropertyName._showReportBugButton, Variant.From<bool>(ref this._showReportBugButton));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NErrorPopup.PropertyName._verticalPopup, ref variant1))
      this._verticalPopup = ((Variant) ref variant1).As<NVerticalPopup>();
    Variant variant2;
    if (info.TryGetProperty(NErrorPopup.PropertyName._title, ref variant2))
      this._title = ((Variant) ref variant2).As<string>();
    Variant variant3;
    if (info.TryGetProperty(NErrorPopup.PropertyName._body, ref variant3))
      this._body = ((Variant) ref variant3).As<string>();
    Variant variant4;
    if (!info.TryGetProperty(NErrorPopup.PropertyName._showReportBugButton, ref variant4))
      return;
    this._showReportBugButton = ((Variant) ref variant4).As<bool>();
  }

  public new class MethodName : NVerticalPopup.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName OnOkButtonPressed = StringName.op_Implicit(nameof (OnOkButtonPressed));
    public static readonly StringName OnCancelButtonPressed = StringName.op_Implicit(nameof (OnCancelButtonPressed));
    public static readonly StringName OnReportBugButtonPressed = StringName.op_Implicit(nameof (OnReportBugButtonPressed));
  }

  public new class PropertyName : NVerticalPopup.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _verticalPopup = StringName.op_Implicit(nameof (_verticalPopup));
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _body = StringName.op_Implicit(nameof (_body));
    public static readonly StringName _showReportBugButton = StringName.op_Implicit(nameof (_showReportBugButton));
  }

  public new class SignalName : NVerticalPopup.SignalName
  {
  }
}
