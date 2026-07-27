// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerTimeoutOverlay
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerTimeoutOverlay.cs")]
public class NMultiplayerTimeoutOverlay : Control
{
  private const int _noResponseMsec = 3000;
  private const int _loadingNoResponseMsec = 8000;
  private bool _gameLevel;
  private TextureRect _icon;
  private NetClientGameService? _netService;

  public bool IsShown { get; private set; }

  public override void _Ready()
  {
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
  }

  public void Relocalize()
  {
    MegaLabel node1 = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Title"));
    MegaRichTextLabel node2 = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description"));
    node1.SetTextAutoSize(new LocString("main_menu_ui", "TIMEOUT_OVERLAY.title").GetFormattedText());
    node2.SetTextAutoSize(new LocString("main_menu_ui", "TIMEOUT_OVERLAY.description").GetFormattedText());
    node1.RefreshFont();
    node2.RefreshFont();
  }

  public void Initialize(INetGameService netService, bool isGameLevel)
  {
    if (!(netService is NetClientGameService clientGameService))
      return;
    this._netService = clientGameService;
    this._gameLevel = isGameLevel;
    TaskHelper.RunSafely(this.UpdateLoop());
  }

  private async Task UpdateLoop()
  {
    while (true)
    {
      NetClientGameService netService = this._netService;
      // ISSUE: explicit non-virtual call
      if ((netService != null ? (__nonvirtual (netService.IsConnected) ? 1 : 0) : 0) != 0 && ((Node) this).IsValid())
      {
        ConnectionStats statsForPeer = this._netService.GetStatsForPeer(this._netService.HostNetId);
        if (statsForPeer != null)
        {
          int num1 = statsForPeer.LastReceivedTime.HasValue ? (int) ((long) Time.GetTicksMsec() - (long) statsForPeer.LastReceivedTime.Value) : 0;
          bool flag1 = (this._gameLevel ? 1 : 0) == (this._netService.IsGameLoading ? 1 : (!RunManager.Instance.IsInProgress ? 1 : 0));
          int num2 = statsForPeer.RemoteIsLoading ? 8000 : 3000;
          bool flag2 = flag1 && num1 >= num2;
          if (!this.IsShown & flag2)
            ((CanvasItem) this).Visible = true;
          else if (this.IsShown && !flag2)
            ((CanvasItem) this).Visible = false;
          this.IsShown = flag2;
          await Task.Delay(200);
        }
        else
          break;
      }
      else
        goto label_9;
    }
    return;
label_9:
    if (((Node) this).IsValid())
      ((CanvasItem) this).Visible = false;
    this._netService = (NetClientGameService) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NMultiplayerTimeoutOverlay.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTimeoutOverlay.MethodName.Relocalize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerTimeoutOverlay.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerTimeoutOverlay.MethodName.Relocalize) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Relocalize();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerTimeoutOverlay.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerTimeoutOverlay.MethodName.Relocalize) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerTimeoutOverlay.PropertyName.IsShown))
    {
      this.IsShown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTimeoutOverlay.PropertyName._gameLevel))
    {
      this._gameLevel = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerTimeoutOverlay.PropertyName._icon))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerTimeoutOverlay.PropertyName.IsShown))
    {
      ref godot_variant local = ref value;
      bool isShown = this.IsShown;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isShown);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTimeoutOverlay.PropertyName._gameLevel))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._gameLevel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerTimeoutOverlay.PropertyName._icon))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NMultiplayerTimeoutOverlay.PropertyName._gameLevel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTimeoutOverlay.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerTimeoutOverlay.PropertyName.IsShown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isShown1 = NMultiplayerTimeoutOverlay.PropertyName.IsShown;
    bool isShown2 = this.IsShown;
    Variant variant = Variant.From<bool>(ref isShown2);
    serializationInfo.AddProperty(isShown1, variant);
    info.AddProperty(NMultiplayerTimeoutOverlay.PropertyName._gameLevel, Variant.From<bool>(ref this._gameLevel));
    info.AddProperty(NMultiplayerTimeoutOverlay.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerTimeoutOverlay.PropertyName.IsShown, ref variant1))
      this.IsShown = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerTimeoutOverlay.PropertyName._gameLevel, ref variant2))
      this._gameLevel = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (!info.TryGetProperty(NMultiplayerTimeoutOverlay.PropertyName._icon, ref variant3))
      return;
    this._icon = ((Variant) ref variant3).As<TextureRect>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Relocalize = StringName.op_Implicit(nameof (Relocalize));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsShown = StringName.op_Implicit(nameof (IsShown));
    public static readonly StringName _gameLevel = StringName.op_Implicit(nameof (_gameLevel));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
  }

  public class SignalName : Control.SignalName
  {
  }
}
