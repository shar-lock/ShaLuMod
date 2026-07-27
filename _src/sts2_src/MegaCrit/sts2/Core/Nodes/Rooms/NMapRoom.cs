// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NMapRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NMapRoom.cs")]
public class NMapRoom : Control, IScreenContext
{
  private const string _scenePath = "res://scenes/rooms/map_room.tscn";
  private ActModel _act;
  private int _actIndex;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.Add("res://scenes/rooms/map_room.tscn");
      items.AddRange(NMapScreen.AssetPaths);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  public static NMapRoom? Create(ActModel act, int actIndex)
  {
    if (TestMode.IsOn)
      return (NMapRoom) null;
    NMapRoom nmapRoom = PreloadManager.Cache.GetScene("res://scenes/rooms/map_room.tscn").Instantiate<NMapRoom>((PackedScene.GenEditState) 0L);
    nmapRoom._act = act;
    nmapRoom._actIndex = actIndex;
    return nmapRoom;
  }

  public override void _Ready()
  {
    NMapScreen nmapScreen = NMapScreen.Instance.Open();
    NRun.Instance.GlobalUi.TopBar.Map.Disable();
    nmapScreen.SetTravelEnabled(true);
    ((GodotObject) NCapstoneContainer.Instance).Connect(NCapstoneContainer.SignalName.CapstoneClosed, Callable.From(new Action(this.ReopenMap)), 0U);
    ((Node) NRun.Instance.GlobalUi.MapScreen).AddChildSafely((Node) NActBanner.Create(this._act, this._actIndex));
  }

  private void ReopenMap() => NMapScreen.Instance.Open();

  public override void _ExitTree()
  {
    ((GodotObject) NCapstoneContainer.Instance).Disconnect(NCapstoneContainer.SignalName.CapstoneClosed, Callable.From(new Action(this.ReopenMap)));
    NCapstoneContainer.Instance.Close();
    NRun.Instance?.GlobalUi.TopBar.Map.Enable();
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMapRoom.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapRoom.MethodName.ReopenMap, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapRoom.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapRoom.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapRoom.MethodName.ReopenMap) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReopenMap();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapRoom.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapRoom.MethodName._Ready) || StringName.op_Equality(ref method, NMapRoom.MethodName.ReopenMap) || StringName.op_Equality(ref method, NMapRoom.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NMapRoom.PropertyName._actIndex))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._actIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapRoom.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapRoom.PropertyName._actIndex))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._actIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMapRoom.PropertyName._actIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapRoom.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapRoom.PropertyName._actIndex, Variant.From<int>(ref this._actIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NMapRoom.PropertyName._actIndex, ref variant))
      return;
    this._actIndex = ((Variant) ref variant).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ReopenMap = StringName.op_Implicit(nameof (ReopenMap));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _actIndex = StringName.op_Implicit(nameof (_actIndex));
  }

  public class SignalName : Control.SignalName
  {
  }
}
