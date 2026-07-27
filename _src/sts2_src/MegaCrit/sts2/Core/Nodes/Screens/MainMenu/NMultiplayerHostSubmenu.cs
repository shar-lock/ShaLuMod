// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMultiplayerHostSubmenu
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMultiplayerHostSubmenu.cs")]
public class NMultiplayerHostSubmenu : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/multiplayer_host_submenu");
  private NSubmenuButton _standardButton;
  private NSubmenuButton _dailyButton;
  private NSubmenuButton _customButton;
  private const string _keyStandard = "STANDARD_MP";
  private const string _keyDaily = "DAILY_MP";
  private const string _keyCustom = "CUSTOM_MP";
  private Control _loadingOverlay;

  protected override Control InitialFocusedControl => (Control) this._standardButton;

  public static NMultiplayerHostSubmenu? Create()
  {
    return TestMode.IsOn ? (NMultiplayerHostSubmenu) null : PreloadManager.Cache.GetScene(NMultiplayerHostSubmenu._scenePath).Instantiate<NMultiplayerHostSubmenu>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._loadingOverlay = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LoadingOverlay"));
    this._standardButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("StandardButton"));
    ((GodotObject) this._standardButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnStandardPressed)), 0U);
    this._standardButton.SetIconAndLocalization("STANDARD_MP");
    this._dailyButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("DailyButton"));
    ((GodotObject) this._dailyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDailyPressed)), 0U);
    this._dailyButton.SetIconAndLocalization("DAILY_MP");
    this._customButton = ((Node) this).GetNode<NSubmenuButton>(NodePath.op_Implicit("CustomRunButton"));
    ((GodotObject) this._customButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnCustomPressed)), 0U);
    this._customButton.SetIconAndLocalization("CUSTOM_MP");
  }

  private void RefreshButtons()
  {
    this._dailyButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<DailyRunEpoch>());
    this._customButton.SetEnabled(SaveManager.Instance.IsEpochRevealed<CustomAndSeedsEpoch>());
  }

  public override void OnSubmenuOpened() => this.RefreshButtons();

  private void OnStandardPressed(NButton _) => this.StartHost(GameMode.Standard);

  private void OnDailyPressed(NButton _) => this.StartHost(GameMode.Daily);

  private void OnCustomPressed(NButton _) => this.StartHost(GameMode.Custom);

  public void StartHost(GameMode gameMode)
  {
    TaskHelper.RunSafely(NMultiplayerHostSubmenu.StartHostAsync(gameMode, this._loadingOverlay, this._stack));
  }

  public static async Task StartHostAsync(
    GameMode gameMode,
    Control loadingOverlay,
    NSubmenuStack stack)
  {
    PlatformType platformType = !SteamInitializer.Initialized || CommandLineHelper.HasArg("fastmp") ? PlatformType.None : PlatformType.Steam;
    ((CanvasItem) loadingOverlay).Visible = true;
    try
    {
      NetHostGameService netService = new NetHostGameService();
      NetErrorInfo? nullable = new NetErrorInfo?();
      if (platformType == PlatformType.Steam)
        nullable = await netService.StartSteamHost(4);
      else
        netService.StartENetHost((ushort) 33771, 4);
      if (!nullable.HasValue)
      {
        switch (gameMode)
        {
          case GameMode.Standard:
            NCharacterSelectScreen submenuType1 = stack.GetSubmenuType<NCharacterSelectScreen>();
            submenuType1.InitializeMultiplayerAsHost((INetGameService) netService, 4);
            stack.Push((NSubmenu) submenuType1);
            break;
          case GameMode.Daily:
            NDailyRunScreen submenuType2 = stack.GetSubmenuType<NDailyRunScreen>();
            submenuType2.InitializeMultiplayerAsHost((INetGameService) netService);
            stack.Push((NSubmenu) submenuType2);
            break;
          default:
            NCustomRunScreen submenuType3 = stack.GetSubmenuType<NCustomRunScreen>();
            submenuType3.InitializeMultiplayerAsHost((INetGameService) netService, 4);
            stack.Push((NSubmenu) submenuType3);
            break;
        }
      }
      else
      {
        NErrorPopup modalToCreate = NErrorPopup.Create(nullable.Value);
        if (modalToCreate != null)
          NModalContainer.Instance.Add((Node) modalToCreate);
      }
      netService = (NetHostGameService) null;
    }
    catch
    {
      NErrorPopup modalToCreate = NErrorPopup.Create(new NetErrorInfo(NetError.InternalError, false));
      if (modalToCreate != null)
        NModalContainer.Instance.Add((Node) modalToCreate);
      throw;
    }
    finally
    {
      ((CanvasItem) loadingOverlay).Visible = false;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.RefreshButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnStandardPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnDailyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.OnCustomPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerHostSubmenu.MethodName.StartHost, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("gameMode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerHostSubmenu nmultiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerHostSubmenu>(ref nmultiplayerHostSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.RefreshButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnStandardPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnStandardPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnDailyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDailyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnCustomPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCustomPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.StartHost) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.StartHost(VariantUtils.ConvertTo<GameMode>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerHostSubmenu nmultiplayerHostSubmenu = NMultiplayerHostSubmenu.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerHostSubmenu>(ref nmultiplayerHostSubmenu);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.Create) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.RefreshButtons) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnStandardPressed) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnDailyPressed) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.OnCustomPressed) || StringName.op_Equality(ref method, NMultiplayerHostSubmenu.MethodName.StartHost) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._standardButton))
    {
      this._standardButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._dailyButton))
    {
      this._dailyButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._customButton))
    {
      this._customButton = VariantUtils.ConvertTo<NSubmenuButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._loadingOverlay))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._loadingOverlay = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._standardButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._standardButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._dailyButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._dailyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._customButton))
    {
      value = VariantUtils.CreateFrom<NSubmenuButton>(ref this._customButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerHostSubmenu.PropertyName._loadingOverlay))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._loadingOverlay);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerHostSubmenu.PropertyName._standardButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerHostSubmenu.PropertyName._dailyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerHostSubmenu.PropertyName._customButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerHostSubmenu.PropertyName._loadingOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerHostSubmenu.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerHostSubmenu.PropertyName._standardButton, Variant.From<NSubmenuButton>(ref this._standardButton));
    info.AddProperty(NMultiplayerHostSubmenu.PropertyName._dailyButton, Variant.From<NSubmenuButton>(ref this._dailyButton));
    info.AddProperty(NMultiplayerHostSubmenu.PropertyName._customButton, Variant.From<NSubmenuButton>(ref this._customButton));
    info.AddProperty(NMultiplayerHostSubmenu.PropertyName._loadingOverlay, Variant.From<Control>(ref this._loadingOverlay));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._standardButton, ref variant1))
      this._standardButton = ((Variant) ref variant1).As<NSubmenuButton>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._dailyButton, ref variant2))
      this._dailyButton = ((Variant) ref variant2).As<NSubmenuButton>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._customButton, ref variant3))
      this._customButton = ((Variant) ref variant3).As<NSubmenuButton>();
    Variant variant4;
    if (!info.TryGetProperty(NMultiplayerHostSubmenu.PropertyName._loadingOverlay, ref variant4))
      return;
    this._loadingOverlay = ((Variant) ref variant4).As<Control>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshButtons = StringName.op_Implicit(nameof (RefreshButtons));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public static readonly StringName OnStandardPressed = StringName.op_Implicit(nameof (OnStandardPressed));
    public static readonly StringName OnDailyPressed = StringName.op_Implicit(nameof (OnDailyPressed));
    public static readonly StringName OnCustomPressed = StringName.op_Implicit(nameof (OnCustomPressed));
    public static readonly StringName StartHost = StringName.op_Implicit(nameof (StartHost));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _standardButton = StringName.op_Implicit(nameof (_standardButton));
    public static readonly StringName _dailyButton = StringName.op_Implicit(nameof (_dailyButton));
    public static readonly StringName _customButton = StringName.op_Implicit(nameof (_customButton));
    public static readonly StringName _loadingOverlay = StringName.op_Implicit(nameof (_loadingOverlay));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
