// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NInvitePlayersButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NInvitePlayersButton.cs")]
public class NInvitePlayersButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private ShaderMaterial _shaderMaterial;
  private Control _container;
  private StartRunLobby? _startRunLobby;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.viewMap)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._container = ((Node) this).GetParent<Control>();
    Control node1 = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Background"));
    MegaRichTextLabel node2 = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Label"));
    this._shaderMaterial = (ShaderMaterial) ((CanvasItem) node1).Material;
    node2.SetTextAutoSize(new LocString("main_menu_ui", "INVITE").GetFormattedText());
    this.UpdateVisibility();
  }

  public void Initialize(StartRunLobby lobby)
  {
    this._startRunLobby = lobby;
    this._startRunLobby.PlayerConnected += new Action<LobbyPlayer>(this.OnPlayerConnected);
    this._startRunLobby.PlayerDisconnected += new Action<LobbyPlayer>(this.OnPlayerDisconnected);
    this.UpdateVisibility();
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    if (this._startRunLobby == null)
      return;
    this._startRunLobby.PlayerConnected -= new Action<LobbyPlayer>(this.OnPlayerConnected);
    this._startRunLobby.PlayerDisconnected -= new Action<LobbyPlayer>(this.OnPlayerDisconnected);
  }

  private void OnPlayerConnected(LobbyPlayer player) => this.UpdateVisibility();

  private void OnPlayerDisconnected(LobbyPlayer player) => this.UpdateVisibility();

  private void UpdateVisibility()
  {
    ((CanvasItem) this._container).Visible = this._startRunLobby != null && PlatformUtil.SupportsInviteDialog(this._startRunLobby.NetService.Platform) && this._startRunLobby.Players.Count < this._startRunLobby.MaxPlayers;
  }

  protected override void OnRelease()
  {
    if (this._startRunLobby == null)
      return;
    PlatformUtil.OpenInviteDialog(this._startRunLobby.NetService);
  }

  protected override void OnFocus()
  {
    this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._s, Variant.op_Implicit(1.1f));
    this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._v, Variant.op_Implicit(1.1f));
  }

  protected override void OnUnfocus()
  {
    this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._s, Variant.op_Implicit(1f));
    this._shaderMaterial.SetShaderParameter(NInvitePlayersButton._v, Variant.op_Implicit(1f));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NInvitePlayersButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInvitePlayersButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInvitePlayersButton.MethodName.UpdateVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInvitePlayersButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInvitePlayersButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NInvitePlayersButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NInvitePlayersButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInvitePlayersButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.UpdateVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NInvitePlayersButton.MethodName._Ready) || StringName.op_Equality(ref method, NInvitePlayersButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.UpdateVisibility) || StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NInvitePlayersButton.MethodName.OnUnfocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInvitePlayersButton.PropertyName._shaderMaterial))
    {
      this._shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInvitePlayersButton.PropertyName._container))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._container = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NInvitePlayersButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NInvitePlayersButton.PropertyName._shaderMaterial))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._shaderMaterial);
      return true;
    }
    if (!StringName.op_Equality(ref name, NInvitePlayersButton.PropertyName._container))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._container);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NInvitePlayersButton.PropertyName._shaderMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NInvitePlayersButton.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NInvitePlayersButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NInvitePlayersButton.PropertyName._shaderMaterial, Variant.From<ShaderMaterial>(ref this._shaderMaterial));
    info.AddProperty(NInvitePlayersButton.PropertyName._container, Variant.From<Control>(ref this._container));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NInvitePlayersButton.PropertyName._shaderMaterial, ref variant1))
      this._shaderMaterial = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (!info.TryGetProperty(NInvitePlayersButton.PropertyName._container, ref variant2))
      return;
    this._container = ((Variant) ref variant2).As<Control>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateVisibility = StringName.op_Implicit(nameof (UpdateVisibility));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _shaderMaterial = StringName.op_Implicit(nameof (_shaderMaterial));
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
