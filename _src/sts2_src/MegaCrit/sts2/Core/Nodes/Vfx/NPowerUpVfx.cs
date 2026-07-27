// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NPowerUpVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NPowerUpVfx.cs")]
public class NPowerUpVfx : Node2D
{
  private float _timer;
  private const float _vfxDuration = 1f;
  private Control _creatureVisuals;
  private Sprite2D _backVfx;

  private static string NormalScenePath
  {
    get => SceneHelper.GetScenePath("/vfx/vfx_power_up/vfx_power_up");
  }

  private static string GhostlyScenePath
  {
    get => SceneHelper.GetScenePath("/vfx/vfx_ghostly_power_up/vfx_ghostly_power_up");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NPowerUpVfx.NormalScenePath,
        NPowerUpVfx.GhostlyScenePath
      });
    }
  }

  public static NPowerUpVfx? CreateNormal(Creature target)
  {
    return NPowerUpVfx.CreatePowerUpVfx(target, NPowerUpVfx.NormalScenePath);
  }

  public static NPowerUpVfx? CreateGhostly(Creature target)
  {
    return NPowerUpVfx.CreatePowerUpVfx(target, NPowerUpVfx.GhostlyScenePath);
  }

  private static NPowerUpVfx? CreatePowerUpVfx(Creature target, string scenePath)
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
    if (creatureNode == null || !creatureNode.IsInteractable)
      return (NPowerUpVfx) null;
    NPowerUpVfx child = PreloadManager.Cache.GetScene(scenePath).Instantiate<NPowerUpVfx>((PackedScene.GenEditState) 0L);
    child.GlobalPosition = creatureNode.VfxSpawnPosition;
    ((Node) NCombatRoom.Instance.CombatVfxContainer).AddChildSafely((Node) child);
    return child;
  }

  public override void _Ready()
  {
    this._timer = 1f;
    this._backVfx = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%BackVfx"));
    Vector2 globalPosition = ((Node2D) this._backVfx).GlobalPosition;
    ((Node) this._backVfx).Reparent((Node) NCombatRoom.Instance.BackCombatVfxContainer, true);
    ((Node2D) this._backVfx).GlobalPosition = globalPosition;
  }

  public override void _Process(double delta)
  {
    this._timer -= (float) delta;
    float num = 1f;
    if ((double) Mathf.Abs((float) ((double) this._timer / 1.0 - 0.5)) > 0.40000000596046448)
      num = Mathf.Max(0.0f, (float) (1.0 - ((double) Mathf.Abs((float) ((double) this._timer / 1.0 - 0.5)) - 0.40000000596046448) / 0.10000000149011612));
    ((CanvasItem) this).Modulate = new Color(1f, 1f, 1f, num);
    ((CanvasItem) this._backVfx).Modulate = new Color(1f, 1f, 1f, num);
    if ((double) this._timer >= 0.0)
      return;
    ((Node) this).QueueFreeSafely();
    ((Node) this._backVfx).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NPowerUpVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerUpVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NPowerUpVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPowerUpVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPowerUpVfx.MethodName._Ready) || StringName.op_Equality(ref method, NPowerUpVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerUpVfx.PropertyName._timer))
    {
      this._timer = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerUpVfx.PropertyName._creatureVisuals))
    {
      this._creatureVisuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerUpVfx.PropertyName._backVfx))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._backVfx = VariantUtils.ConvertTo<Sprite2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPowerUpVfx.PropertyName._timer))
    {
      value = VariantUtils.CreateFrom<float>(ref this._timer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPowerUpVfx.PropertyName._creatureVisuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._creatureVisuals);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPowerUpVfx.PropertyName._backVfx))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Sprite2D>(ref this._backVfx);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NPowerUpVfx.PropertyName._timer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerUpVfx.PropertyName._creatureVisuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPowerUpVfx.PropertyName._backVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NPowerUpVfx.PropertyName._timer, Variant.From<float>(ref this._timer));
    info.AddProperty(NPowerUpVfx.PropertyName._creatureVisuals, Variant.From<Control>(ref this._creatureVisuals));
    info.AddProperty(NPowerUpVfx.PropertyName._backVfx, Variant.From<Sprite2D>(ref this._backVfx));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPowerUpVfx.PropertyName._timer, ref variant1))
      this._timer = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NPowerUpVfx.PropertyName._creatureVisuals, ref variant2))
      this._creatureVisuals = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (!info.TryGetProperty(NPowerUpVfx.PropertyName._backVfx, ref variant3))
      return;
    this._backVfx = ((Variant) ref variant3).As<Sprite2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _timer = StringName.op_Implicit(nameof (_timer));
    public static readonly StringName _creatureVisuals = StringName.op_Implicit(nameof (_creatureVisuals));
    public static readonly StringName _backVfx = StringName.op_Implicit(nameof (_backVfx));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
