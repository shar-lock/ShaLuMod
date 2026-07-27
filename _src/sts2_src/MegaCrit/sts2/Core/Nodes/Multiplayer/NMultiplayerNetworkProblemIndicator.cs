// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Multiplayer.NMultiplayerNetworkProblemIndicator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Multiplayer/NMultiplayerNetworkProblemIndicator.cs")]
public class NMultiplayerNetworkProblemIndicator : TextureRect
{
  private const float _qualityScoreToShowAt = 350f;
  private ulong _peerId;
  private Tween? _tween;

  public bool IsShown { get; private set; }

  public void Initialize(ulong peerId)
  {
    if (!RunManager.Instance.NetService.Type.IsMultiplayer())
      return;
    this._peerId = peerId;
    TaskHelper.RunSafely(this.UpdateLoop());
  }

  private async Task UpdateLoop()
  {
    while (RunManager.Instance.NetService.IsConnected && ((Node) this).IsValid())
    {
      ConnectionStats statsForPeer = RunManager.Instance.NetService.GetStatsForPeer(this._peerId);
      if (statsForPeer == null)
        break;
      bool flag = (double) (statsForPeer.PingMsec / (1f - statsForPeer.PacketLoss)) >= 350.0;
      if (!this.IsShown & flag)
      {
        this._tween?.Kill();
        ((CanvasItem) this).Visible = true;
        ((CanvasItem) this).Modulate = Colors.White;
      }
      else if (this.IsShown & flag)
      {
        NUiFlashVfx child = NUiFlashVfx.Create(this.Texture, Colors.White);
        ((Node) this).AddChildSafely((Node) child);
        TaskHelper.RunSafely(child.StartVfx());
      }
      else if (this.IsShown && !flag)
      {
        this._tween?.Kill();
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
      }
      this.IsShown = flag;
      await Task.Delay(2000);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMultiplayerNetworkProblemIndicator.MethodName.Initialize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("peerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NMultiplayerNetworkProblemIndicator.MethodName.Initialize) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Initialize(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerNetworkProblemIndicator.MethodName.Initialize) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerNetworkProblemIndicator.PropertyName.IsShown))
    {
      this.IsShown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerNetworkProblemIndicator.PropertyName._peerId))
    {
      this._peerId = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerNetworkProblemIndicator.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerNetworkProblemIndicator.PropertyName.IsShown))
    {
      ref godot_variant local = ref value;
      bool isShown = this.IsShown;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isShown);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerNetworkProblemIndicator.PropertyName._peerId))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._peerId);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerNetworkProblemIndicator.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMultiplayerNetworkProblemIndicator.PropertyName._peerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerNetworkProblemIndicator.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerNetworkProblemIndicator.PropertyName.IsShown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isShown1 = NMultiplayerNetworkProblemIndicator.PropertyName.IsShown;
    bool isShown2 = this.IsShown;
    Variant variant = Variant.From<bool>(ref isShown2);
    serializationInfo.AddProperty(isShown1, variant);
    info.AddProperty(NMultiplayerNetworkProblemIndicator.PropertyName._peerId, Variant.From<ulong>(ref this._peerId));
    info.AddProperty(NMultiplayerNetworkProblemIndicator.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerNetworkProblemIndicator.PropertyName.IsShown, ref variant1))
      this.IsShown = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerNetworkProblemIndicator.PropertyName._peerId, ref variant2))
      this._peerId = ((Variant) ref variant2).As<ulong>();
    Variant variant3;
    if (!info.TryGetProperty(NMultiplayerNetworkProblemIndicator.PropertyName._tween, ref variant3))
      return;
    this._tween = ((Variant) ref variant3).As<Tween>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName Initialize = StringName.op_Implicit(nameof (Initialize));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName IsShown = StringName.op_Implicit(nameof (IsShown));
    public static readonly StringName _peerId = StringName.op_Implicit(nameof (_peerId));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
