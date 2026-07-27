// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Potions.NPotion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Potions;

[ScriptPath("res://src/Core/Nodes/Potions/NPotion.cs")]
public class NPotion : Control
{
  private const float _newlyAcquiredPopDuration = 0.35f;
  private const float _newlyAcquiredFadeInDuration = 0.1f;
  private const float _newlyAcquiredPopDistance = 40f;
  private PotionModel? _model;
  private Control _container;
  private Tween? _bounceTween;
  private Tween? _obtainedTween;
  private CancellationTokenSource? _cancellationTokenSource;

  public TextureRect Image { get; private set; }

  public TextureRect Outline { get; private set; }

  private static string ScenePath => SceneHelper.GetScenePath("/potions/potion");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NPotion.ScenePath,
        NPotionFlashVfx.ScenePath
      });
    }
  }

  public PotionModel Model
  {
    get
    {
      return this._model ?? throw new InvalidOperationException("Model was accessed before it was set.");
    }
    set
    {
      this._model = value;
      this.Reload();
    }
  }

  public static NPotion? Create(PotionModel potion)
  {
    if (TestMode.IsOn)
      return (NPotion) null;
    NPotion npotion = PreloadManager.Cache.GetScene(NPotion.ScenePath).Instantiate<NPotion>((PackedScene.GenEditState) 0L);
    npotion.Model = potion;
    return npotion;
  }

  public override void _Ready()
  {
    this.Image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Image"));
    this.Outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this._container = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Container"));
    this.Reload();
  }

  public override void _ExitTree() => this._cancellationTokenSource?.Cancel();

  private void Reload()
  {
    if (!((Node) this).IsNodeReady() || this._model == null)
      return;
    this.Image.Texture = this._model.Image;
    this.Outline.Texture = this._model.Outline;
  }

  public async Task PlayNewlyAcquiredAnimation(Vector2? startLocation)
  {
    if (this._cancellationTokenSource != null)
      await this._cancellationTokenSource.CancelAsync();
    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
    this._cancellationTokenSource = cancelTokenSource;
    double num = (double) await ((Node) this).AwaitProcessFrame();
    if (cancelTokenSource.IsCancellationRequested)
    {
      cancelTokenSource = (CancellationTokenSource) null;
    }
    else
    {
      this._obtainedTween?.Kill();
      if (!startLocation.HasValue)
      {
        Control container1 = this._container;
        Vector2 position = this._container.Position;
        position.Y = 40f;
        Vector2 vector2 = position;
        container1.Position = vector2;
        Control container2 = this._container;
        Color modulate = ((CanvasItem) this._container).Modulate;
        modulate.A = 0.0f;
        Color color = modulate;
        ((CanvasItem) container2).Modulate = color;
        this._obtainedTween = ((Node) this).GetTree().CreateTween();
        this._obtainedTween.TweenProperty((GodotObject) this._container, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.10000000149011612);
        this._obtainedTween.Parallel();
        this._obtainedTween.SetEase((Tween.EaseType) 1L);
        this._obtainedTween.SetTrans((Tween.TransitionType) 10L);
        this._obtainedTween.TweenProperty((GodotObject) this._container, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.34999999403953552);
        this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
        cancelTokenSource = (CancellationTokenSource) null;
      }
      else
      {
        this._container.GlobalPosition = startLocation.Value;
        Control container = this._container;
        Color modulate = ((CanvasItem) this._container).Modulate;
        modulate.A = 1f;
        Color color = modulate;
        ((CanvasItem) container).Modulate = color;
        this._obtainedTween = ((Node) this).GetTree().CreateTween();
        this._obtainedTween.SetEase((Tween.EaseType) 1L);
        this._obtainedTween.SetTrans((Tween.TransitionType) 4L);
        this._obtainedTween.TweenProperty((GodotObject) this._container, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.34999999403953552);
        this._obtainedTween.TweenCallback(Callable.From(new Action(this.DoFlash)));
        cancelTokenSource = (CancellationTokenSource) null;
      }
    }
  }

  private void DoFlash() => ((Node) this).AddChildSafely((Node) NPotionFlashVfx.Create(this));

  public void DoBounce()
  {
    this._bounceTween?.Kill();
    this._bounceTween = ((Node) this).CreateTween();
    this._bounceTween.TweenProperty((GodotObject) this._container, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this.Position.Y - 12f), 0.125).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._bounceTween.TweenProperty((GodotObject) this._container, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.125).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NPotion.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotion.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotion.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotion.MethodName.DoFlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPotion.MethodName.DoBounce, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPotion.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotion.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotion.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPotion.MethodName.DoFlash) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DoFlash();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPotion.MethodName.DoBounce) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.DoBounce();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPotion.MethodName._Ready) || StringName.op_Equality(ref method, NPotion.MethodName._ExitTree) || StringName.op_Equality(ref method, NPotion.MethodName.Reload) || StringName.op_Equality(ref method, NPotion.MethodName.DoFlash) || StringName.op_Equality(ref method, NPotion.MethodName.DoBounce) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotion.PropertyName.Image))
    {
      this.Image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotion.PropertyName.Outline))
    {
      this.Outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotion.PropertyName._container))
    {
      this._container = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotion.PropertyName._bounceTween))
    {
      this._bounceTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotion.PropertyName._obtainedTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._obtainedTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPotion.PropertyName.Image))
    {
      ref godot_variant local = ref value;
      TextureRect image = this.Image;
      godot_variant from = VariantUtils.CreateFrom<TextureRect>(ref image);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotion.PropertyName.Outline))
    {
      ref godot_variant local = ref value;
      TextureRect outline = this.Outline;
      godot_variant from = VariantUtils.CreateFrom<TextureRect>(ref outline);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPotion.PropertyName._container))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._container);
      return true;
    }
    if (StringName.op_Equality(ref name, NPotion.PropertyName._bounceTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._bounceTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPotion.PropertyName._obtainedTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._obtainedTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPotion.PropertyName._container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotion.PropertyName.Image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotion.PropertyName.Outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotion.PropertyName._bounceTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPotion.PropertyName._obtainedTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName image1 = NPotion.PropertyName.Image;
    TextureRect image2 = this.Image;
    Variant variant1 = Variant.From<TextureRect>(ref image2);
    serializationInfo1.AddProperty(image1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName outline1 = NPotion.PropertyName.Outline;
    TextureRect outline2 = this.Outline;
    Variant variant2 = Variant.From<TextureRect>(ref outline2);
    serializationInfo2.AddProperty(outline1, variant2);
    info.AddProperty(NPotion.PropertyName._container, Variant.From<Control>(ref this._container));
    info.AddProperty(NPotion.PropertyName._bounceTween, Variant.From<Tween>(ref this._bounceTween));
    info.AddProperty(NPotion.PropertyName._obtainedTween, Variant.From<Tween>(ref this._obtainedTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPotion.PropertyName.Image, ref variant1))
      this.Image = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NPotion.PropertyName.Outline, ref variant2))
      this.Outline = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NPotion.PropertyName._container, ref variant3))
      this._container = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NPotion.PropertyName._bounceTween, ref variant4))
      this._bounceTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (!info.TryGetProperty(NPotion.PropertyName._obtainedTween, ref variant5))
      return;
    this._obtainedTween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public static readonly StringName DoFlash = StringName.op_Implicit(nameof (DoFlash));
    public static readonly StringName DoBounce = StringName.op_Implicit(nameof (DoBounce));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Image = StringName.op_Implicit(nameof (Image));
    public static readonly StringName Outline = StringName.op_Implicit(nameof (Outline));
    public static readonly StringName _container = StringName.op_Implicit(nameof (_container));
    public static readonly StringName _bounceTween = StringName.op_Implicit(nameof (_bounceTween));
    public static readonly StringName _obtainedTween = StringName.op_Implicit(nameof (_obtainedTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
