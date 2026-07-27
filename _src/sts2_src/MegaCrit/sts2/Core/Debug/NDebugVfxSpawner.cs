// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.NDebugVfxSpawner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

[ScriptPath("res://src/Core/Debug/NDebugVfxSpawner.cs")]
public class NDebugVfxSpawner : Control
{
  private static readonly StringName _viewDeck = new StringName("view_deck");

  private void Spawn()
  {
    NRelicFlashVfx child = NRelicFlashVfx.Create((RelicModel) ModelDb.Relic<PaelsEye>());
    if (child == null)
      return;
    Control parent = (Control) this;
    ((Node) parent).AddChildSafely((Node) child);
    child.Scale = Vector2.op_Multiply(Vector2.One, 2f);
    child.Position = Vector2.op_Multiply(parent.Size, 0.5f);
  }

  public override void _Process(double delta)
  {
    if (!Input.IsActionJustReleased(NDebugVfxSpawner._viewDeck, false))
      return;
    this.Spawn();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDebugVfxSpawner.MethodName.Spawn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDebugVfxSpawner.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NDebugVfxSpawner.MethodName.Spawn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Spawn();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDebugVfxSpawner.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDebugVfxSpawner.MethodName.Spawn) || StringName.op_Equality(ref method, NDebugVfxSpawner.MethodName._Process) || base.HasGodotClassMethod(ref method);
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
    public static readonly StringName Spawn = StringName.op_Implicit(nameof (Spawn));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
