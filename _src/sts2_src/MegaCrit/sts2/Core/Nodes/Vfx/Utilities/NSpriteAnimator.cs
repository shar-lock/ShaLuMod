// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.NSpriteAnimator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NSpriteAnimator.cs")]
public class NSpriteAnimator : Sprite2D
{
  [ExportGroup("Animation Settings", "")]
  [Export]
  private Texture2D[] _frames;
  [Export]
  private float _fps = 15f;
  [Export]
  private bool _loop;
  [ExportGroup("Rotation Settings", "")]
  [Export]
  private bool _randomizeRotation;
  [Export]
  private Vector2 _rotationRange;
  private readonly CancellationTokenSource _cancelToken = new CancellationTokenSource();

  public override void _Ready()
  {
    if (this._randomizeRotation)
      ((Node2D) this).RotationDegrees = (float) new Random().Next((int) this._rotationRange.X, (int) this._rotationRange.Y);
    TaskHelper.RunSafely(this.PlayAnimation());
  }

  public override void _ExitTree() => this._cancelToken.Cancel();

  private async Task PlayAnimation()
  {
    int i = 0;
    int interval = Mathf.RoundToInt(1000f / this._fps);
    while (!this._cancelToken.IsCancellationRequested)
    {
      this.Texture = this._frames[i];
      ++i;
      if (this._loop)
        i %= this._frames.Length;
      await Task.Delay(interval, this._cancelToken.Token);
      if (this._frames.Length <= i)
        break;
    }
    if (this._cancelToken.IsCancellationRequested)
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
      new MethodInfo(NSpriteAnimator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpriteAnimator.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpriteAnimator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpriteAnimator.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpriteAnimator.MethodName._Ready) || StringName.op_Equality(ref method, NSpriteAnimator.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._frames))
    {
      this._frames = VariantUtils.ConvertToSystemArrayOfGodotObject<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._fps))
    {
      this._fps = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._loop))
    {
      this._loop = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._randomizeRotation))
    {
      this._randomizeRotation = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._rotationRange))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._rotationRange = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._frames))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._frames);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._fps))
    {
      value = VariantUtils.CreateFrom<float>(ref this._fps);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._loop))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._loop);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._randomizeRotation))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._randomizeRotation);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpriteAnimator.PropertyName._rotationRange))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._rotationRange);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit("Animation Settings"), (PropertyHint) 0L, "", (PropertyUsageFlags) 64L /*0x40*/, true),
      new PropertyInfo((Variant.Type) 28L, NSpriteAnimator.PropertyName._frames, (PropertyHint) 23L, "24/17:Texture2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NSpriteAnimator.PropertyName._fps, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NSpriteAnimator.PropertyName._loop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit("Rotation Settings"), (PropertyHint) 0L, "", (PropertyUsageFlags) 64L /*0x40*/, true),
      new PropertyInfo((Variant.Type) 1L, NSpriteAnimator.PropertyName._randomizeRotation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NSpriteAnimator.PropertyName._rotationRange, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSpriteAnimator.PropertyName._frames, Variant.CreateFrom((GodotObject[]) this._frames));
    info.AddProperty(NSpriteAnimator.PropertyName._fps, Variant.From<float>(ref this._fps));
    info.AddProperty(NSpriteAnimator.PropertyName._loop, Variant.From<bool>(ref this._loop));
    info.AddProperty(NSpriteAnimator.PropertyName._randomizeRotation, Variant.From<bool>(ref this._randomizeRotation));
    info.AddProperty(NSpriteAnimator.PropertyName._rotationRange, Variant.From<Vector2>(ref this._rotationRange));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpriteAnimator.PropertyName._frames, ref variant1))
      this._frames = ((Variant) ref variant1).AsGodotObjectArray<Texture2D>();
    Variant variant2;
    if (info.TryGetProperty(NSpriteAnimator.PropertyName._fps, ref variant2))
      this._fps = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NSpriteAnimator.PropertyName._loop, ref variant3))
      this._loop = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NSpriteAnimator.PropertyName._randomizeRotation, ref variant4))
      this._randomizeRotation = ((Variant) ref variant4).As<bool>();
    Variant variant5;
    if (!info.TryGetProperty(NSpriteAnimator.PropertyName._rotationRange, ref variant5))
      return;
    this._rotationRange = ((Variant) ref variant5).As<Vector2>();
  }

  public class MethodName : Sprite2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Sprite2D.PropertyName
  {
    public static readonly StringName _frames = StringName.op_Implicit(nameof (_frames));
    public static readonly StringName _fps = StringName.op_Implicit(nameof (_fps));
    public static readonly StringName _loop = StringName.op_Implicit(nameof (_loop));
    public static readonly StringName _randomizeRotation = StringName.op_Implicit(nameof (_randomizeRotation));
    public static readonly StringName _rotationRange = StringName.op_Implicit(nameof (_rotationRange));
  }

  public class SignalName : Sprite2D.SignalName
  {
  }
}
