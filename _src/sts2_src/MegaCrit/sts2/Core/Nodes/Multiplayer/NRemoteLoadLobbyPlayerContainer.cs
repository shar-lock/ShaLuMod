// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLoadLobbyPlayerContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteLoadLobbyPlayerContainer.cs")]
public class NRemoteLoadLobbyPlayerContainer : Control
{
  private readonly List<NRemoteLobbyPlayer> _nodes = new List<NRemoteLobbyPlayer>();
  private LoadRunLobby? _lobby;
  private MegaLabel? _othersLabel;
  private Control _container;

  public override void _Ready()
  {
    this._othersLabel = ((Node) this).GetNodeOrNull<MegaLabel>(NodePath.op_Implicit("OthersLabel"));
    this._container = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Container"));
  }

  public void Initialize(LoadRunLobby runLobby, bool displayLocalPlayer)
  {
    this._lobby = runLobby;
    if (this._othersLabel != null)
    {
      LocString locString = new LocString("main_menu_ui", "MULTIPLAYER_LOAD_MENU.OTHERS");
      locString.Add("others", (Decimal) (runLobby.Run.Players.Count - 1));
      this._othersLabel.Text = locString.GetFormattedText();
    }
    foreach (SerializablePlayer player in runLobby.Run.Players)
    {
      if ((long) player.NetId != (long) this._lobby.NetService.NetId || displayLocalPlayer)
      {
        NRemoteLobbyPlayer child = NRemoteLobbyPlayer.Create(runLobby, player.NetId, runLobby.NetService.Platform, runLobby.NetService.Type == NetGameType.Singleplayer);
        ((Node) this._container).AddChildSafely((Node) child);
        this._nodes.Add(child);
      }
    }
  }

  public void OnPlayerConnected(ulong playerId) => this.OnPlayerChanged(playerId);

  public void OnPlayerDisconnected(ulong playerId) => this.OnPlayerChanged(playerId);

  public void OnPlayerChanged(ulong playerId)
  {
    if (this._lobby == null || (long) playerId == (long) this._lobby.NetService.NetId)
      return;
    int index = this._nodes.FindIndex((Predicate<NRemoteLobbyPlayer>) (p => (long) p.PlayerId == (long) playerId));
    if (index < 0)
      return;
    this._nodes[index].OnPlayerChanged(this._lobby, playerId);
  }

  public void Cleanup()
  {
    foreach (Node node in this._nodes)
      node.QueueFreeSafely();
    this._nodes.Clear();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerConnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerDisconnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRemoteLoadLobbyPlayerContainer.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerConnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPlayerConnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerDisconnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPlayerDisconnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnPlayerChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.Cleanup) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Cleanup();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName._Ready) || StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerConnected) || StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerDisconnected) || StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.OnPlayerChanged) || StringName.op_Equality(ref method, NRemoteLoadLobbyPlayerContainer.MethodName.Cleanup) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel))
    {
      this._othersLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteLoadLobbyPlayerContainer.PropertyName._container))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._container = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._othersLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteLoadLobbyPlayerContainer.PropertyName._container))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._container);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLoadLobbyPlayerContainer.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel, Variant.From<MegaLabel>(ref this._othersLabel));
    info.AddProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._container, Variant.From<Control>(ref this._container));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._othersLabel, ref variant1))
      this._othersLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NRemoteLoadLobbyPlayerContainer.PropertyName._container, ref variant2))
      return;
    this._container = ((Variant) ref variant2).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnPlayerConnected = StringName.op_Implicit(nameof (OnPlayerConnected));
    public static readonly StringName OnPlayerDisconnected = StringName.op_Implicit(nameof (OnPlayerDisconnected));
    public static readonly StringName OnPlayerChanged = StringName.op_Implicit(nameof (OnPlayerChanged));
    public static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _othersLabel = StringName.op_Implicit(nameof (_othersLabel));
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
  }

  public class SignalName : Control.SignalName
  {
  }
}
