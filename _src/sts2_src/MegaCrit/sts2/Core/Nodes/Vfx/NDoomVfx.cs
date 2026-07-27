// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDoomVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NDoomVfx.cs")]
public class NDoomVfx : Node2D
{
  private Tween? _tween;
  private NDoomSubEmitterVfx _back;
  private NDoomSubEmitterVfx _front;
  private NCreatureVisuals _creatureVisuals;
  private Vector2 _position;
  private Vector2 _size;
  private bool _shouldDie;
  private CancellationToken _cancelToken;
  private const float _doomVfxSize = 260f;

  private CancellationTokenSource VfxCancellationToken { get; } = new CancellationTokenSource();

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_doom");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NDoomVfx.ScenePath);
    }
  }

  public Task? VfxTask { get; private set; }

  public static NDoomVfx? Create(
    NCreatureVisuals creatureVisuals,
    Vector2 position,
    Vector2 size,
    bool shouldDie)
  {
    if (TestMode.IsOn)
      return (NDoomVfx) null;
    NDoomVfx ndoomVfx = PreloadManager.Cache.GetScene(NDoomVfx.ScenePath).Instantiate<NDoomVfx>((PackedScene.GenEditState) 0L);
    ndoomVfx._creatureVisuals = creatureVisuals;
    ndoomVfx._position = position;
    ndoomVfx._size = size;
    ndoomVfx._shouldDie = shouldDie;
    return ndoomVfx;
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this.VfxCancellationToken.Cancel();
    this._tween?.Kill();
  }

  public override void _Ready()
  {
    this._back = ((Node) this).GetNode<NDoomSubEmitterVfx>(NodePath.op_Implicit("DoomVfxBack"));
    this._front = ((Node) this).GetNode<NDoomSubEmitterVfx>(NodePath.op_Implicit("DoomVfxFront"));
    this._cancelToken = this.VfxCancellationToken.Token;
    this.VfxTask = TaskHelper.RunSafely(this.PlayVfx(this._creatureVisuals, this._position, this._size, this._shouldDie));
  }

  private async Task PlayVfx(
    NCreatureVisuals creatureVisuals,
    Vector2 position,
    Vector2 size,
    bool shouldDie)
  {
    if (this._cancelToken.IsCancellationRequested)
      return;
    SfxCmd.Play("event:/sfx/characters/necrobinder/necrobinder_doom_kill");
    this.GlobalPosition = Vector2.op_Addition(position, Vector2.op_Multiply(new Vector2(size.X * 0.5f, size.Y), NCombatRoom.Instance.SceneContainer.Scale));
    this.Scale = NCombatRoom.Instance.SceneContainer.Scale;
    SubViewport node = ((Node) this).GetNode<SubViewport>(NodePath.op_Implicit("Viewport"));
    Vector2 vector2 = size;
    vector2.X *= 1.5f;
    vector2.Y *= 1.5f;
    node.Size = Vector2I.op_Explicit(vector2);
    if (shouldDie)
    {
      Node2D creatureBody = creatureVisuals.GetCurrentBody();
      Vector2 creatureOffset = Vector2.op_Addition(new Vector2(vector2.X / 2f, (float) node.Size.Y), creatureBody.Position);
      Vector2 originalGlobalScale = creatureBody.GlobalScale;
      await this.Reparent((Node) creatureBody, (Node) node);
      creatureBody.Position = creatureOffset;
      creatureBody.Scale = originalGlobalScale;
      creatureBody = (Node2D) null;
    }
    if (this._cancelToken.IsCancellationRequested)
      return;
    await this.PlayVfxInternal();
  }

  private async Task PlayVfxInternal()
  {
    try
    {
      SubViewport node1 = ((Node) this).GetNode<SubViewport>(NodePath.op_Implicit("Viewport"));
      Sprite2D node2 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("%Visual"));
      Sprite2D sprite2D = node2;
      ((Node2D) sprite2D).Position = Vector2.op_Addition(((Node2D) sprite2D).Position, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Up, (float) node1.Size.Y), 0.5f));
      NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 180f + Rng.Chaotic.NextFloat(-10f, 10f));
      this.ShowOrHideParticles((float) node1.Size.X / 260f, 0.5f);
      this._tween = ((Node) this).CreateTween();
      this._tween.TweenProperty((GodotObject) node2, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Node2D) node2).Position.Y + (float) node1.Size.Y), 0.75).SetEase((Tween.EaseType) 0L).SetDelay(0.75).SetTrans((Tween.TransitionType) 5L);
      if (!await this._tween.AwaitFinished((Node) this))
        return;
      this.ShowOrHideParticles(0.0f, 0.25f);
      await Task.Delay(2000, this._cancelToken);
    }
    finally
    {
      if (GodotObject.IsInstanceValid((GodotObject) this))
        ((Node) this).QueueFreeSafely();
    }
  }

  private void ShowOrHideParticles(float widthScale, float tweenTime)
  {
    this._back.ShowOrHide(widthScale, tweenTime);
    this._front.ShowOrHide(widthScale, tweenTime);
  }

  private async Task Reparent(Node creatureNode, Node newParent)
  {
    Node parent = creatureNode.GetParent();
    bool removeCompleted = false;
    Callable reparent = Callable.From<bool>((Func<bool>) (() => removeCompleted = true));
    ((GodotObject) creatureNode).Connect(Node.SignalName.TreeExited, reparent, 0U);
    parent.RemoveChildSafely(creatureNode);
    while (!removeCompleted)
    {
      double num = (double) await ((Node) this).AwaitProcessFrame();
    }
    newParent.AddChildSafely(creatureNode);
    ((GodotObject) creatureNode).Disconnect(Node.SignalName.TreeExited, reparent);
    reparent = new Callable();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NDoomVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("creatureVisuals"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("position"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("size"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("shouldDie"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDoomVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomVfx.MethodName.ShowOrHideParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("widthScale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("tweenTime"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDoomVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      NDoomVfx ndoomVfx = NDoomVfx.Create(VariantUtils.ConvertTo<NCreatureVisuals>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<NDoomVfx>(ref ndoomVfx);
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDoomVfx.MethodName.ShowOrHideParticles) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShowOrHideParticles(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDoomVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      NDoomVfx ndoomVfx = NDoomVfx.Create(VariantUtils.ConvertTo<NCreatureVisuals>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = VariantUtils.CreateFrom<NDoomVfx>(ref ndoomVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDoomVfx.MethodName.Create) || StringName.op_Equality(ref method, NDoomVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NDoomVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDoomVfx.MethodName.ShowOrHideParticles) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._back))
    {
      this._back = VariantUtils.ConvertTo<NDoomSubEmitterVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._front))
    {
      this._front = VariantUtils.ConvertTo<NDoomSubEmitterVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._creatureVisuals))
    {
      this._creatureVisuals = VariantUtils.ConvertTo<NCreatureVisuals>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._position))
    {
      this._position = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._size))
    {
      this._size = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDoomVfx.PropertyName._shouldDie))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._shouldDie = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._back))
    {
      value = VariantUtils.CreateFrom<NDoomSubEmitterVfx>(ref this._back);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._front))
    {
      value = VariantUtils.CreateFrom<NDoomSubEmitterVfx>(ref this._front);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._creatureVisuals))
    {
      value = VariantUtils.CreateFrom<NCreatureVisuals>(ref this._creatureVisuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._position))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._position);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomVfx.PropertyName._size))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._size);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDoomVfx.PropertyName._shouldDie))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._shouldDie);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDoomVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDoomVfx.PropertyName._back, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDoomVfx.PropertyName._front, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDoomVfx.PropertyName._creatureVisuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDoomVfx.PropertyName._position, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDoomVfx.PropertyName._size, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDoomVfx.PropertyName._shouldDie, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDoomVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NDoomVfx.PropertyName._back, Variant.From<NDoomSubEmitterVfx>(ref this._back));
    info.AddProperty(NDoomVfx.PropertyName._front, Variant.From<NDoomSubEmitterVfx>(ref this._front));
    info.AddProperty(NDoomVfx.PropertyName._creatureVisuals, Variant.From<NCreatureVisuals>(ref this._creatureVisuals));
    info.AddProperty(NDoomVfx.PropertyName._position, Variant.From<Vector2>(ref this._position));
    info.AddProperty(NDoomVfx.PropertyName._size, Variant.From<Vector2>(ref this._size));
    info.AddProperty(NDoomVfx.PropertyName._shouldDie, Variant.From<bool>(ref this._shouldDie));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDoomVfx.PropertyName._tween, ref variant1))
      this._tween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (info.TryGetProperty(NDoomVfx.PropertyName._back, ref variant2))
      this._back = ((Variant) ref variant2).As<NDoomSubEmitterVfx>();
    Variant variant3;
    if (info.TryGetProperty(NDoomVfx.PropertyName._front, ref variant3))
      this._front = ((Variant) ref variant3).As<NDoomSubEmitterVfx>();
    Variant variant4;
    if (info.TryGetProperty(NDoomVfx.PropertyName._creatureVisuals, ref variant4))
      this._creatureVisuals = ((Variant) ref variant4).As<NCreatureVisuals>();
    Variant variant5;
    if (info.TryGetProperty(NDoomVfx.PropertyName._position, ref variant5))
      this._position = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NDoomVfx.PropertyName._size, ref variant6))
      this._size = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (!info.TryGetProperty(NDoomVfx.PropertyName._shouldDie, ref variant7))
      return;
    this._shouldDie = ((Variant) ref variant7).As<bool>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ShowOrHideParticles = StringName.op_Implicit(nameof (ShowOrHideParticles));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _back = StringName.op_Implicit(nameof (_back));
    public static readonly StringName _front = StringName.op_Implicit(nameof (_front));
    public static readonly StringName _creatureVisuals = StringName.op_Implicit(nameof (_creatureVisuals));
    public static readonly StringName _position = StringName.op_Implicit(nameof (_position));
    public static readonly StringName _size = StringName.op_Implicit(nameof (_size));
    public static readonly StringName _shouldDie = StringName.op_Implicit(nameof (_shouldDie));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
