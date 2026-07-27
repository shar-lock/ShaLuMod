// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDecimillipedeRocksVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NDecimillipedeRocksVfx.cs")]
public class NDecimillipedeRocksVfx : Node2D
{
  [Export]
  private Node2D[] _rocks;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

  public override void _Ready() => TaskHelper.RunSafely(this.Play(this._cancelToken.Token));

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._cancelToken.Cancel();
  }

  private async Task Play(CancellationToken cancellationToken)
  {
    Node2D[] node2DArray = this._rocks;
    for (int index = 0; index < node2DArray.Length; ++index)
    {
      Node2D rock = node2DArray[index];
      await Task.Delay(Rng.Chaotic.NextInt(100, 201), cancellationToken);
      new MegaSprite(Variant.op_Implicit((GodotObject) rock)).GetAnimationState().SetAnimation($"fall{Rng.Chaotic.NextInt(1, 5)}", false);
      rock = (Node2D) null;
    }
    node2DArray = (Node2D[]) null;
    await Task.Delay(5000, cancellationToken);
    if (cancellationToken.IsCancellationRequested)
      return;
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDecimillipedeRocksVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDecimillipedeRocksVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDecimillipedeRocksVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDecimillipedeRocksVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDecimillipedeRocksVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDecimillipedeRocksVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDecimillipedeRocksVfx.PropertyName._rocks))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._rocks = VariantUtils.ConvertToSystemArrayOfGodotObject<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDecimillipedeRocksVfx.PropertyName._rocks))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._rocks);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NDecimillipedeRocksVfx.PropertyName._rocks, (PropertyHint) 23L, "24/34:Node2D", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDecimillipedeRocksVfx.PropertyName._rocks, Variant.CreateFrom((GodotObject[]) this._rocks));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NDecimillipedeRocksVfx.PropertyName._rocks, ref variant))
      return;
    this._rocks = ((Variant) ref variant).AsGodotObjectArray<Node2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _rocks = StringName.op_Implicit(nameof (_rocks));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
