// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NRemoteLobbyPlayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NRemoteLobbyPlayer.cs")]
public class NRemoteLobbyPlayer : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("ui/remote_lobby_player");
  private TextureRect _characterIcon;
  private Control _readyIndicator;
  private Control _disconnectedIndicator;
  private MegaLabel _nameplateLabel;
  private MegaLabel _characterLabel;
  private PlatformType _platform;
  private bool _isSingleplayer;
  private ScreenPunchInstance? _shake;
  private Vector2? _originalPosition;
  private ulong _playerId;
  private CharacterModel _character;
  private bool _isReady;
  private bool _isConnected;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NRemoteLobbyPlayer._scenePath);
    }
  }

  public ulong PlayerId => this._playerId;

  public static NRemoteLobbyPlayer Create(
    LobbyPlayer player,
    PlatformType platform,
    bool isSingleplayer)
  {
    NRemoteLobbyPlayer nremoteLobbyPlayer = PreloadManager.Cache.GetScene(NRemoteLobbyPlayer._scenePath).Instantiate<NRemoteLobbyPlayer>((PackedScene.GenEditState) 0L);
    nremoteLobbyPlayer._playerId = player.id;
    nremoteLobbyPlayer._platform = platform;
    nremoteLobbyPlayer._isSingleplayer = isSingleplayer;
    nremoteLobbyPlayer._character = player.character;
    nremoteLobbyPlayer._isReady = player.isReady;
    nremoteLobbyPlayer._isConnected = true;
    return nremoteLobbyPlayer;
  }

  public static NRemoteLobbyPlayer Create(
    LoadRunLobby runLobby,
    ulong playerId,
    PlatformType platform,
    bool isSingleplayer)
  {
    NRemoteLobbyPlayer nremoteLobbyPlayer = PreloadManager.Cache.GetScene(NRemoteLobbyPlayer._scenePath).Instantiate<NRemoteLobbyPlayer>((PackedScene.GenEditState) 0L);
    nremoteLobbyPlayer._playerId = playerId;
    nremoteLobbyPlayer._isSingleplayer = isSingleplayer;
    nremoteLobbyPlayer._platform = runLobby.NetService.Platform;
    nremoteLobbyPlayer._character = ModelDb.GetById<CharacterModel>(runLobby.Run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) playerId)).CharacterId);
    nremoteLobbyPlayer._isReady = runLobby.IsPlayerReady(playerId);
    nremoteLobbyPlayer._isConnected = runLobby.ConnectedPlayerIds.Contains(playerId);
    return nremoteLobbyPlayer;
  }

  public override void _Ready()
  {
    this._nameplateLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%NameplateLabel"));
    this._characterLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CharacterLabel"));
    this._characterIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%CharacterIcon"));
    this._readyIndicator = (Control) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%ReadyIndicator"));
    this._disconnectedIndicator = (Control) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%DisconnectedIndicator"));
    if (!this._isSingleplayer)
      this._nameplateLabel.SetTextAutoSize(PlatformUtil.GetPlayerNameRaw(this._platform, this._playerId));
    else
      this._characterLabel.SetTextAutoSize(string.Empty);
    this.RefreshVisuals();
  }

  public void OnPlayerChanged(LobbyPlayer lobbyPlayer)
  {
    this._playerId = lobbyPlayer.id;
    this.SetCharacter(lobbyPlayer.character);
    this._isReady = lobbyPlayer.isReady;
    this._isConnected = true;
    this.RefreshVisuals();
  }

  public void OnPlayerChanged(LoadRunLobby runLobby, ulong playerId)
  {
    this.SetCharacter(ModelDb.GetById<CharacterModel>(runLobby.Run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) playerId)).CharacterId));
    this._isReady = runLobby.IsPlayerReady(playerId);
    this._isConnected = runLobby.ConnectedPlayerIds.Contains(playerId);
    this.RefreshVisuals();
  }

  private void RefreshVisuals()
  {
    if (this._isSingleplayer)
      this._nameplateLabel.SetTextAutoSize(this._character.Title.GetFormattedText());
    else
      this._characterLabel.SetTextAutoSize(this._character.Title.GetFormattedText());
    this._characterIcon.Texture = this._character.IconTexture;
    ((CanvasItem) this._readyIndicator).Visible = this._isReady;
    ((CanvasItem) this._disconnectedIndicator).Visible = !this._isConnected;
  }

  private void SetCharacter(CharacterModel character)
  {
    if (this._character == character)
      return;
    this._shake?.Cancel();
    this.CancelShake();
    this._originalPosition = new Vector2?(this.Position);
    this._shake = new ScreenPunchInstance(3f, 0.40000000596046448, 90f);
    this._character = character;
  }

  public void CancelShake()
  {
    this._shake = (ScreenPunchInstance) null;
    if (!this._originalPosition.HasValue)
      return;
    this.Position = this._originalPosition.Value;
    this._originalPosition = new Vector2?();
  }

  public override void _Process(double delta)
  {
    ScreenPunchInstance shake1 = this._shake;
    if (shake1 == null || shake1.IsDone)
      return;
    ScreenPunchInstance shake2 = this._shake;
    this.Position = Vector2.op_Addition(this._originalPosition.Value, shake2 != null ? shake2.Update(delta) : Vector2.Zero);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NRemoteLobbyPlayer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteLobbyPlayer.MethodName.RefreshVisuals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteLobbyPlayer.MethodName.CancelShake, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRemoteLobbyPlayer.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName.RefreshVisuals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshVisuals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName.CancelShake) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CancelShake();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName._Ready) || StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName.RefreshVisuals) || StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName.CancelShake) || StringName.op_Equality(ref method, NRemoteLobbyPlayer.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._characterIcon))
    {
      this._characterIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._readyIndicator))
    {
      this._readyIndicator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._disconnectedIndicator))
    {
      this._disconnectedIndicator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._nameplateLabel))
    {
      this._nameplateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._characterLabel))
    {
      this._characterLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._platform))
    {
      this._platform = VariantUtils.ConvertTo<PlatformType>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._isSingleplayer))
    {
      this._isSingleplayer = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._playerId))
    {
      this._playerId = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._isReady))
    {
      this._isReady = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._isConnected))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isConnected = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName.PlayerId))
    {
      ref godot_variant local = ref value;
      ulong playerId = this.PlayerId;
      godot_variant from = VariantUtils.CreateFrom<ulong>(ref playerId);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._characterIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._characterIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._readyIndicator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._readyIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._disconnectedIndicator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._disconnectedIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._nameplateLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameplateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._characterLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._characterLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._platform))
    {
      value = VariantUtils.CreateFrom<PlatformType>(ref this._platform);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._isSingleplayer))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isSingleplayer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._playerId))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._playerId);
      return true;
    }
    if (StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._isReady))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isReady);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRemoteLobbyPlayer.PropertyName._isConnected))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isConnected);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayer.PropertyName._characterIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayer.PropertyName._readyIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayer.PropertyName._disconnectedIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayer.PropertyName._nameplateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRemoteLobbyPlayer.PropertyName._characterLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRemoteLobbyPlayer.PropertyName._platform, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRemoteLobbyPlayer.PropertyName._isSingleplayer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRemoteLobbyPlayer.PropertyName._playerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRemoteLobbyPlayer.PropertyName._isReady, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRemoteLobbyPlayer.PropertyName._isConnected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRemoteLobbyPlayer.PropertyName.PlayerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._characterIcon, Variant.From<TextureRect>(ref this._characterIcon));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._readyIndicator, Variant.From<Control>(ref this._readyIndicator));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._disconnectedIndicator, Variant.From<Control>(ref this._disconnectedIndicator));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._nameplateLabel, Variant.From<MegaLabel>(ref this._nameplateLabel));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._characterLabel, Variant.From<MegaLabel>(ref this._characterLabel));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._platform, Variant.From<PlatformType>(ref this._platform));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._isSingleplayer, Variant.From<bool>(ref this._isSingleplayer));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._playerId, Variant.From<ulong>(ref this._playerId));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._isReady, Variant.From<bool>(ref this._isReady));
    info.AddProperty(NRemoteLobbyPlayer.PropertyName._isConnected, Variant.From<bool>(ref this._isConnected));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._characterIcon, ref variant1))
      this._characterIcon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._readyIndicator, ref variant2))
      this._readyIndicator = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._disconnectedIndicator, ref variant3))
      this._disconnectedIndicator = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._nameplateLabel, ref variant4))
      this._nameplateLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._characterLabel, ref variant5))
      this._characterLabel = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._platform, ref variant6))
      this._platform = ((Variant) ref variant6).As<PlatformType>();
    Variant variant7;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._isSingleplayer, ref variant7))
      this._isSingleplayer = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._playerId, ref variant8))
      this._playerId = ((Variant) ref variant8).As<ulong>();
    Variant variant9;
    if (info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._isReady, ref variant9))
      this._isReady = ((Variant) ref variant9).As<bool>();
    Variant variant10;
    if (!info.TryGetProperty(NRemoteLobbyPlayer.PropertyName._isConnected, ref variant10))
      return;
    this._isConnected = ((Variant) ref variant10).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshVisuals = StringName.op_Implicit(nameof (RefreshVisuals));
    public static readonly StringName CancelShake = StringName.op_Implicit(nameof (CancelShake));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName PlayerId = StringName.op_Implicit(nameof (PlayerId));
    public static readonly StringName _characterIcon = StringName.op_Implicit(nameof (_characterIcon));
    public static readonly StringName _readyIndicator = StringName.op_Implicit(nameof (_readyIndicator));
    public static readonly StringName _disconnectedIndicator = StringName.op_Implicit(nameof (_disconnectedIndicator));
    public static readonly StringName _nameplateLabel = StringName.op_Implicit(nameof (_nameplateLabel));
    public static readonly StringName _characterLabel = StringName.op_Implicit(nameof (_characterLabel));
    public static readonly StringName _platform = StringName.op_Implicit(nameof (_platform));
    public static readonly StringName _isSingleplayer = StringName.op_Implicit(nameof (_isSingleplayer));
    public static readonly StringName _playerId = StringName.op_Implicit(nameof (_playerId));
    public static readonly StringName _isReady = StringName.op_Implicit(nameof (_isReady));
    public static readonly StringName _isConnected = StringName.op_Implicit(nameof (_isConnected));
  }

  public class SignalName : Control.SignalName
  {
  }
}
