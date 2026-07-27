// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NLogoAnimation
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NLogoAnimation.cs")]
public class NLogoAnimation : Control, IScreenContext
{
  private const string _scenePath = "res://scenes/screens/main_menu/logo_animation.tscn";
  private Control _bg;
  private Control _logoContainer;
  private Node2D _logoSpineNode;
  private MegaSprite _spineSprite;
  private Color _logoBgColor = new Color("074254FF");
  private Tween? _tween;
  private bool _cancelled;
  private bool _skeletonReady;

  public static string[] AssetPaths
  {
    get
    {
      return new string[1]
      {
        "res://scenes/screens/main_menu/logo_animation.tscn"
      };
    }
  }

  public static NLogoAnimation Create()
  {
    return PreloadManager.Cache.GetScene("res://scenes/screens/main_menu/logo_animation.tscn").Instantiate<NLogoAnimation>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._bg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Bg"));
    this._logoContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Container"));
    this._logoSpineNode = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("Container/SpineSprite"));
    this._spineSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this._logoSpineNode));
    ((CanvasItem) this._logoSpineNode).Visible = false;
    MegaSkeleton skeleton = this._spineSprite.GetSkeleton();
    if (skeleton == null)
      return;
    this._skeletonReady = true;
    Rect2 bounds = skeleton.GetBounds();
    this._logoSpineNode.Scale = Vector2.op_Multiply(Math.Min(this.Size.X * 0.33f / ((Rect2) ref bounds).Size.X, this.Size.Y * 0.33f / ((Rect2) ref bounds).Size.Y), Vector2.One);
    this._logoSpineNode.Position = Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_UnaryNegation(((Rect2) ref bounds).Size), this._logoSpineNode.Scale), 0.5f);
  }

  public async Task PlayAnimation(CancellationToken token)
  {
    if (token.IsCancellationRequested || !this._skeletonReady)
    {
      this._cancelled = true;
    }
    else
    {
      this._tween = ((Node) this).CreateTween();
      this._tween.TweenInterval(1.0);
      if (!await this._tween.AwaitFinished((Node) this))
        return;
      if (token.IsCancellationRequested)
      {
        this._cancelled = true;
      }
      else
      {
        ((CanvasItem) this._logoSpineNode).Visible = true;
        this._spineSprite.GetAnimationState().SetAnimation("animation", false);
        NDebugAudioManager.Instance.Play("SOTE_Logo_Echoing_ShortTail.mp3");
        this._tween.Kill();
        this._tween = ((Node) this).CreateTween().SetParallel(true);
        this._tween.TweenProperty((GodotObject) this._logoSpineNode, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._logoSpineNode.Position.Y), 0.5).From(Variant.op_Implicit(this._logoSpineNode.Position.Y - 800f)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
        this._tween.TweenProperty((GodotObject) this._logoContainer, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
        double num1 = (double) await ((Node) this).AwaitProcessFrame();
        while (true)
        {
          bool flag;
          using (MegaTrackEntry current = this._spineSprite.GetAnimationState().GetCurrent(0))
            flag = current != null && !current.IsComplete();
          if (flag)
          {
            if (!token.IsCancellationRequested)
            {
              double num2 = (double) await ((Node) this).AwaitProcessFrame();
            }
            else
              break;
          }
          else
            goto label_19;
        }
        this._cancelled = true;
        this._tween.Kill();
        this._tween = ((Node) this).CreateTween().SetParallel(true);
        this._tween.TweenProperty((GodotObject) this._logoContainer, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
        if (!await this._tween.AwaitFinished((Node) this))
          return;
label_19:
        if (this._cancelled)
          return;
        this._tween.Kill();
        this._tween = ((Node) this).CreateTween();
        this._tween.TweenProperty((GodotObject) this._bg, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._logoBgColor), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        this._tween.Chain();
        this._tween.TweenInterval(1.0);
        bool flag1 = await this._tween.AwaitFinished((Node) this);
      }
    }
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NLogoAnimation.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLogoAnimation.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLogoAnimation.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NLogoAnimation nlogoAnimation = NLogoAnimation.Create();
      ret = VariantUtils.CreateFrom<NLogoAnimation>(ref nlogoAnimation);
      return true;
    }
    if (!StringName.op_Equality(ref method, NLogoAnimation.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLogoAnimation.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NLogoAnimation nlogoAnimation = NLogoAnimation.Create();
      ret = VariantUtils.CreateFrom<NLogoAnimation>(ref nlogoAnimation);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLogoAnimation.MethodName.Create) || StringName.op_Equality(ref method, NLogoAnimation.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._bg))
    {
      this._bg = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._logoContainer))
    {
      this._logoContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._logoSpineNode))
    {
      this._logoSpineNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._logoBgColor))
    {
      this._logoBgColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._cancelled))
    {
      this._cancelled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLogoAnimation.PropertyName._skeletonReady))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._skeletonReady = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._bg))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bg);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._logoContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._logoContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._logoSpineNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._logoSpineNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._logoBgColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._logoBgColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NLogoAnimation.PropertyName._cancelled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._cancelled);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLogoAnimation.PropertyName._skeletonReady))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._skeletonReady);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLogoAnimation.PropertyName._bg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLogoAnimation.PropertyName._logoContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLogoAnimation.PropertyName._logoSpineNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NLogoAnimation.PropertyName._logoBgColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLogoAnimation.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NLogoAnimation.PropertyName._cancelled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NLogoAnimation.PropertyName._skeletonReady, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLogoAnimation.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLogoAnimation.PropertyName._bg, Variant.From<Control>(ref this._bg));
    info.AddProperty(NLogoAnimation.PropertyName._logoContainer, Variant.From<Control>(ref this._logoContainer));
    info.AddProperty(NLogoAnimation.PropertyName._logoSpineNode, Variant.From<Node2D>(ref this._logoSpineNode));
    info.AddProperty(NLogoAnimation.PropertyName._logoBgColor, Variant.From<Color>(ref this._logoBgColor));
    info.AddProperty(NLogoAnimation.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NLogoAnimation.PropertyName._cancelled, Variant.From<bool>(ref this._cancelled));
    info.AddProperty(NLogoAnimation.PropertyName._skeletonReady, Variant.From<bool>(ref this._skeletonReady));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLogoAnimation.PropertyName._bg, ref variant1))
      this._bg = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NLogoAnimation.PropertyName._logoContainer, ref variant2))
      this._logoContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NLogoAnimation.PropertyName._logoSpineNode, ref variant3))
      this._logoSpineNode = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NLogoAnimation.PropertyName._logoBgColor, ref variant4))
      this._logoBgColor = ((Variant) ref variant4).As<Color>();
    Variant variant5;
    if (info.TryGetProperty(NLogoAnimation.PropertyName._tween, ref variant5))
      this._tween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NLogoAnimation.PropertyName._cancelled, ref variant6))
      this._cancelled = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (!info.TryGetProperty(NLogoAnimation.PropertyName._skeletonReady, ref variant7))
      return;
    this._skeletonReady = ((Variant) ref variant7).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _bg = StringName.op_Implicit(nameof (_bg));
    public static readonly StringName _logoContainer = StringName.op_Implicit(nameof (_logoContainer));
    public static readonly StringName _logoSpineNode = StringName.op_Implicit(nameof (_logoSpineNode));
    public static readonly StringName _logoBgColor = StringName.op_Implicit(nameof (_logoBgColor));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _cancelled = StringName.op_Implicit(nameof (_cancelled));
    public static readonly StringName _skeletonReady = StringName.op_Implicit(nameof (_skeletonReady));
  }

  public class SignalName : Control.SignalName
  {
  }
}
