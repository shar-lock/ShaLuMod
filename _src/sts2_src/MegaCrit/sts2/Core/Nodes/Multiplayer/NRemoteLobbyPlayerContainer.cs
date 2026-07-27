// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayerContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteLobbyPlayerContainer.cs")]
public class NRemoteLobbyPlayerContainer : Control
{
  private readonly List<NRemoteLobbyPlayer> _nodes = new List<NRemoteLobbyPlayer>();
  private StartRunLobby? _lobby;
  private NInvitePlayersButton _inviteButton;
  private MegaLabel _soloLabel;
  private Container _container;
  private bool _displayLocalPlayer;

  public override void _Ready()
  {
    this._soloLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%SoloLabel"));
    this._container = ((Node) this).GetNode<Container>(NodePath.op_Implicit("Container"));
    this._inviteButton = ((Node) this).GetNode<NInvitePlayersButton>(NodePath.op_Implicit("%InviteButton"));
    this._soloLabel.SetTextAutoSize(new LocString("main_menu_ui", "MULTIPLAYER_CHAR_SELECT.SOLO").GetFormattedText());
  }

  public void Initialize(StartRunLobby lobby, bool displayLocalPlayer)
  {
    foreach (Node node in this._nodes)
      node.QueueFreeSafely();
    this._nodes.Clear();
    if (!lobby.NetService.Type.IsMultiplayer())
      return;
    this._displayLocalPlayer = displayLocalPlayer;
    this._inviteButton.Initialize(lobby);
    this._lobby = lobby;
    foreach (LobbyPlayer player in this._lobby.Players)
      this.OnPlayerConnected(player);
    this.RefreshSoloLabelVisibility();
  }

  public void OnPlayerConnected(LobbyPlayer player)
  {
    StartRunLobby lobby = this._lobby;
    if (lobby == null || (long) player.id == (long) lobby.LocalPlayer.id && !this._displayLocalPlayer)
      return;
    NRemoteLobbyPlayer child = NRemoteLobbyPlayer.Create(player, lobby.NetService.Platform, lobby.NetService.Type == NetGameType.Singleplayer);
    ((Node) this._container).AddChildSafely((Node) child);
    ((Node) this._container).MoveChildSafely(((Node) this._inviteButton).GetParent(), ((Node) this._container).GetChildCount(false) - 1);
    this._nodes.Add(child);
    this.RefreshSoloLabelVisibility();
  }

  public void OnPlayerDisconnected(LobbyPlayer player)
  {
    if (this._lobby == null)
      return;
    int index = this._nodes.FindIndex((Predicate<NRemoteLobbyPlayer>) (p => (long) p.PlayerId == (long) player.id));
    if (index >= 0)
    {
      ((Node) this._container).RemoveChildSafely((Node) this._nodes[index]);
      this._nodes.RemoveAt(index);
      foreach (NRemoteLobbyPlayer node in this._nodes)
        node.CancelShake();
    }
    this.RefreshSoloLabelVisibility();
  }

  public void OnPlayerChanged(LobbyPlayer player)
  {
    StartRunLobby lobby = this._lobby;
    if (lobby == null || (long) player.id == (long) lobby.LocalPlayer.id && !this._displayLocalPlayer)
      return;
    this._nodes.FirstOrDefault<NRemoteLobbyPlayer>((Func<NRemoteLobbyPlayer, bool>) (p => (long) p.PlayerId == (long) player.id))?.OnPlayerChanged(player);
  }

  private void RefreshSoloLabelVisibility()
  {
    StartRunLobby lobby = this._lobby;
    ((CanvasItem) this._soloLabel).Visible = (lobby != null ? (lobby.NetService.Type != NetGameType.Singleplayer ? 1 : 0) : 1) != 0 && lobby != null && lobby.Players.Count == 1;
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
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NRemoteLobbyPlayerContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteLobbyPlayerContainer.MethodName.RefreshSoloLabelVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteLobbyPlayerContainer.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteLobbyPlayerContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteLobbyPlayerContainer.MethodName.RefreshSoloLabelVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshSoloLabelVisibility();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRemoteLobbyPlayerContainer.MethodName.Cleanup) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Cleanup();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRemoteLobbyPlayerContainer.MethodName._Ready) || StringName.op_Equality(ref method, NRemoteLobbyPlayerContainer.MethodName.RefreshSoloLabelVisibility) || StringName.op_Equality(ref method, NRemoteLobbyPlayerContainer.MethodName.Cleanup) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._inviteButton))
    {
      this._inviteButton = VariantUtils.ConvertTo<NInvitePlayersButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._soloLabel))
    {
      this._soloLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._container))
    {
      this._container = VariantUtils.ConvertTo<Container>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._displayLocalPlayer = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._inviteButton))
    {
      value = VariantUtils.CreateFrom<NInvitePlayersButton>(ref this._inviteButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._soloLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._soloLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._container))
    {
      value = VariantUtils.CreateFrom<Container>(ref this._container);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._displayLocalPlayer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayerContainer.PropertyName._inviteButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayerContainer.PropertyName._soloLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayerContainer.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._inviteButton, Variant.From<NInvitePlayersButton>(ref this._inviteButton));
    info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._soloLabel, Variant.From<MegaLabel>(ref this._soloLabel));
    info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._container, Variant.From<Container>(ref this._container));
    info.AddProperty(NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer, Variant.From<bool>(ref this._displayLocalPlayer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._inviteButton, ref variant1))
      this._inviteButton = ((Variant) ref variant1).As<NInvitePlayersButton>();
    Variant variant2;
    if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._soloLabel, ref variant2))
      this._soloLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._container, ref variant3))
      this._container = ((Variant) ref variant3).As<Container>();
    Variant variant4;
    if (!info.TryGetProperty(NRemoteLobbyPlayerContainer.PropertyName._displayLocalPlayer, ref variant4))
      return;
    this._displayLocalPlayer = ((Variant) ref variant4).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshSoloLabelVisibility = StringName.op_Implicit(nameof (RefreshSoloLabelVisibility));
    public static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _inviteButton = StringName.op_Implicit(nameof (_inviteButton));
    public static readonly StringName _soloLabel = StringName.op_Implicit(nameof (_soloLabel));
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
    public static readonly StringName _displayLocalPlayer = StringName.op_Implicit(nameof (_displayLocalPlayer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
