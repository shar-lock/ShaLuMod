// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NCombatBackground
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NCombatBackground.cs")]
public class NCombatBackground : Control
{
  public static NCombatBackground Create(BackgroundAssets bg)
  {
    if (bg.BackgroundScenePath == null)
      throw new InvalidOperationException("Encounter does not have a background.");
    NCombatBackground ncombatBackground = PreloadManager.Cache.GetScene(bg.BackgroundScenePath).Instantiate<NCombatBackground>((PackedScene.GenEditState) 0L);
    ncombatBackground.SetLayers(bg);
    return ncombatBackground;
  }

  private void SetLayers(BackgroundAssets bg)
  {
    this.SetBackgroundLayers((IReadOnlyList<string>) bg.BgLayers);
    this.SetForegroundLayer(bg.FgLayer);
  }

  private void SetBackgroundLayers(IReadOnlyList<string> backgroundLayers)
  {
    for (int index = 0; index < backgroundLayers.Count; ++index)
      this.AddLayer($"Layer_{index:D2}", backgroundLayers[index]);
  }

  private void SetForegroundLayer(string? foregroundLayer)
  {
    if (foregroundLayer == null)
      return;
    this.AddLayer("Foreground", foregroundLayer);
  }

  private void AddLayer(string layerName, string layerPath)
  {
    Node nodeOrNull = ((Node) this).GetNodeOrNull(NodePath.op_Implicit(layerName));
    if (nodeOrNull == null)
      throw new InvalidOperationException($"Layer node='{layerName}' not found in combat background scene.");
    Control child = PreloadManager.Cache.GetScene(layerPath).Instantiate<Control>((PackedScene.GenEditState) 0L);
    ((CanvasItem) child).Visible = true;
    nodeOrNull.AddChildSafely((Node) child);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCombatBackground.MethodName.SetForegroundLayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("foregroundLayer"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatBackground.MethodName.AddLayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("layerName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("layerPath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatBackground.MethodName.SetForegroundLayer) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetForegroundLayer(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatBackground.MethodName.AddLayer) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AddLayer(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatBackground.MethodName.SetForegroundLayer) || StringName.op_Equality(ref method, NCombatBackground.MethodName.AddLayer) || base.HasGodotClassMethod(ref method);
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
    public static readonly StringName SetForegroundLayer = StringName.op_Implicit(nameof (SetForegroundLayer));
    public static readonly StringName AddLayer = StringName.op_Implicit(nameof (AddLayer));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
