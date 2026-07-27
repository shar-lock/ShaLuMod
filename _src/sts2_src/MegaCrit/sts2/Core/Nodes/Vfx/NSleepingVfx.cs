// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSleepingVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NSleepingVfx.cs")]
public class NSleepingVfx : Node2D
{
  private static readonly StringName _direction = new StringName("direction");
  public static readonly string scenePath = SceneHelper.GetScenePath("vfx/vfx_sleeping");
  [Export]
  private Array<GpuParticles2D> _burstParticles = new Array<GpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _continuousParticles = new Array<GpuParticles2D>();
  [Export]
  private GpuParticles2D? _zParticles;
  [Export]
  private LocalizedTexture? _localizedZTexture;
  private CancellationTokenSource? _cts;

  public static NSleepingVfx? Create(Vector2 targetTalkPosition, bool goingRight = true)
  {
    if (TestMode.IsOn)
      return (NSleepingVfx) null;
    NSleepingVfx nsleepingVfx = PreloadManager.Cache.GetScene(NSleepingVfx.scenePath).Instantiate<NSleepingVfx>((PackedScene.GenEditState) 0L);
    nsleepingVfx.GlobalPosition = targetTalkPosition;
    nsleepingVfx.SetFloatingDirection(goingRight);
    Texture2D texture;
    if (nsleepingVfx != null && nsleepingVfx._zParticles != null && nsleepingVfx._localizedZTexture != null && nsleepingVfx._localizedZTexture.TryGetTexture(out texture))
      nsleepingVfx._zParticles.Texture = texture;
    return nsleepingVfx;
  }

  public override void _Ready() => this.Play();

  public override void _ExitTree() => this._cts?.Cancel();

  private void Play()
  {
    foreach (GpuParticles2D burstParticle in this._burstParticles)
      burstParticle.Restart();
    foreach (GpuParticles2D continuousParticle in this._continuousParticles)
    {
      continuousParticle.Restart();
      continuousParticle.Emitting = true;
    }
  }

  public void SetFloatingDirection(bool goingRight)
  {
    this._zParticles.ProcessMaterial = (Material) ((Resource) this._zParticles.ProcessMaterial).Duplicate(false);
    ((GodotObject) this._zParticles.ProcessMaterial).Set(NSleepingVfx._direction, Variant.op_Implicit(new Vector3(goingRight ? 0.5f : -0.5f, -1f, 0.0f)));
  }

  public void Stop() => TaskHelper.RunSafely(this.Stopping());

  private async Task Stopping()
  {
    this._cts = new CancellationTokenSource();
    foreach (GpuParticles2D continuousParticle in this._continuousParticles)
      continuousParticle.Emitting = false;
    await Cmd.Wait(5f, this._cts.Token);
    ((Node) this).QueueFreeSafely();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NSleepingVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetTalkPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("goingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSleepingVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSleepingVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSleepingVfx.MethodName.Play, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSleepingVfx.MethodName.SetFloatingDirection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("goingRight"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSleepingVfx.MethodName.Stop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSleepingVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSleepingVfx nsleepingVfx = NSleepingVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSleepingVfx>(ref nsleepingVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NSleepingVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSleepingVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSleepingVfx.MethodName.Play) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Play();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSleepingVfx.MethodName.SetFloatingDirection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetFloatingDirection(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSleepingVfx.MethodName.Stop) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Stop();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSleepingVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NSleepingVfx nsleepingVfx = NSleepingVfx.Create(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NSleepingVfx>(ref nsleepingVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSleepingVfx.MethodName.Create) || StringName.op_Equality(ref method, NSleepingVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSleepingVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NSleepingVfx.MethodName.Play) || StringName.op_Equality(ref method, NSleepingVfx.MethodName.SetFloatingDirection) || StringName.op_Equality(ref method, NSleepingVfx.MethodName.Stop) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSleepingVfx.PropertyName._burstParticles))
    {
      this._burstParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSleepingVfx.PropertyName._continuousParticles))
    {
      this._continuousParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSleepingVfx.PropertyName._zParticles))
    {
      this._zParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSleepingVfx.PropertyName._localizedZTexture))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._localizedZTexture = VariantUtils.ConvertTo<LocalizedTexture>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSleepingVfx.PropertyName._burstParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._burstParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSleepingVfx.PropertyName._continuousParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._continuousParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSleepingVfx.PropertyName._zParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._zParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSleepingVfx.PropertyName._localizedZTexture))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<LocalizedTexture>(ref this._localizedZTexture);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NSleepingVfx.PropertyName._burstParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NSleepingVfx.PropertyName._continuousParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NSleepingVfx.PropertyName._zParticles, (PropertyHint) 34L, "GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NSleepingVfx.PropertyName._localizedZTexture, (PropertyHint) 17L, "LocalizedTexture", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSleepingVfx.PropertyName._burstParticles, Variant.CreateFrom<GpuParticles2D>(this._burstParticles));
    info.AddProperty(NSleepingVfx.PropertyName._continuousParticles, Variant.CreateFrom<GpuParticles2D>(this._continuousParticles));
    info.AddProperty(NSleepingVfx.PropertyName._zParticles, Variant.From<GpuParticles2D>(ref this._zParticles));
    info.AddProperty(NSleepingVfx.PropertyName._localizedZTexture, Variant.From<LocalizedTexture>(ref this._localizedZTexture));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSleepingVfx.PropertyName._burstParticles, ref variant1))
      this._burstParticles = ((Variant) ref variant1).AsGodotArray<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSleepingVfx.PropertyName._continuousParticles, ref variant2))
      this._continuousParticles = ((Variant) ref variant2).AsGodotArray<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NSleepingVfx.PropertyName._zParticles, ref variant3))
      this._zParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (!info.TryGetProperty(NSleepingVfx.PropertyName._localizedZTexture, ref variant4))
      return;
    this._localizedZTexture = ((Variant) ref variant4).As<LocalizedTexture>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Play = StringName.op_Implicit(nameof (Play));
    public static readonly StringName SetFloatingDirection = StringName.op_Implicit(nameof (SetFloatingDirection));
    public static readonly StringName Stop = StringName.op_Implicit(nameof (Stop));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _burstParticles = StringName.op_Implicit(nameof (_burstParticles));
    public static readonly StringName _continuousParticles = StringName.op_Implicit(nameof (_continuousParticles));
    public static readonly StringName _zParticles = StringName.op_Implicit(nameof (_zParticles));
    public static readonly StringName _localizedZTexture = StringName.op_Implicit(nameof (_localizedZTexture));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
