// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Reaction.NReactionContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Reaction;

[ScriptPath("res://src/Core/Nodes/Reaction/NReactionContainer.cs")]
public class NReactionContainer : Control
{
  private ReactionSynchronizer? _synchronizer;

  public bool InMultiplayer
  {
    get
    {
      return this._synchronizer != null && this._synchronizer.NetService.Type != NetGameType.Singleplayer;
    }
  }

  public void InitializeNetworking(INetGameService netService)
  {
    if (this._synchronizer != null)
      this.DeinitializeNetworking();
    this._synchronizer = new ReactionSynchronizer(netService, this);
    this._synchronizer.NetService.Disconnected += new Action<NetErrorInfo>(this.NetServiceDisconnected);
  }

  private void NetServiceDisconnected(NetErrorInfo _) => this.DeinitializeNetworking();

  public void DeinitializeNetworking()
  {
    if (this._synchronizer == null)
      return;
    this._synchronizer.NetService.Disconnected -= new Action<NetErrorInfo>(this.NetServiceDisconnected);
    this._synchronizer.Dispose();
    this._synchronizer = (ReactionSynchronizer) null;
  }

  public override void _ExitTree() => this.DeinitializeNetworking();

  public void DoLocalReaction(Texture2D tex, Vector2 position)
  {
    NReaction child = NReaction.Create(tex);
    ((Node) this).AddChildSafely((Node) child);
    ((Control) child).GlobalPosition = Vector2.op_Subtraction(position, Vector2.op_Division(((Control) child).Size, 2f));
    child.BeginAnim();
    this._synchronizer?.SendLocalReaction(child.Type, position);
  }

  public void DoRemoteReaction(ReactionType type, Vector2 position)
  {
    NReaction child = NReaction.Create(type);
    ((Node) this).AddChildSafely((Node) child);
    ((Control) child).GlobalPosition = Vector2.op_Subtraction(position, Vector2.op_Division(((Control) child).Size, 2f));
    child.BeginAnim();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NReactionContainer.MethodName.DeinitializeNetworking, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NReactionContainer.MethodName.DoLocalReaction, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NReactionContainer.MethodName.DoRemoteReaction, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("type"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NReactionContainer.MethodName.DeinitializeNetworking) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DeinitializeNetworking();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NReactionContainer.MethodName.DoLocalReaction) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.DoLocalReaction(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NReactionContainer.MethodName.DoRemoteReaction) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.DoRemoteReaction(VariantUtils.ConvertTo<ReactionType>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NReactionContainer.MethodName.DeinitializeNetworking) || StringName.op_Equality(ref method, NReactionContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NReactionContainer.MethodName.DoLocalReaction) || StringName.op_Equality(ref method, NReactionContainer.MethodName.DoRemoteReaction) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NReactionContainer.PropertyName.InMultiplayer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    ref godot_variant local = ref value;
    bool inMultiplayer = this.InMultiplayer;
    godot_variant from = VariantUtils.CreateFrom<bool>(ref inMultiplayer);
    local = from;
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NReactionContainer.PropertyName.InMultiplayer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName DeinitializeNetworking = StringName.op_Implicit(nameof (DeinitializeNetworking));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName DoLocalReaction = StringName.op_Implicit(nameof (DoLocalReaction));
    public static readonly StringName DoRemoteReaction = StringName.op_Implicit(nameof (DoRemoteReaction));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName InMultiplayer = StringName.op_Implicit(nameof (InMultiplayer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
