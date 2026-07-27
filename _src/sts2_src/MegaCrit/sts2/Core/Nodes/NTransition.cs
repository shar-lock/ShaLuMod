// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NTransition
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NTransition.cs")]
public class NTransition : ColorRect
{
  private static readonly NodePath _thresholdTweenPath = new NodePath("shader_parameter/threshold");
  private static readonly StringName _threshold = new StringName("threshold");
  private const string _fightTransitionPath = "res://materials/transitions/fight_transition_mat.tres";
  private const string _fadeTransitionPath = "res://materials/transitions/fade_transition_mat.tres";
  private float _initialGradientYPosition;
  private float _targetGradientYPosition;
  private Control _gradientTransition;
  private Control _simpleTransition;
  private Tween? _tween;

  public bool InTransition { get; private set; }

  public override void _Ready()
  {
    this._gradientTransition = ((Node) this).GetNode<Control>(NodePath.op_Implicit("GradientTransition"));
    this._simpleTransition = ((Node) this).GetNode<Control>(NodePath.op_Implicit("SimpleTransition"));
    this._initialGradientYPosition = this._gradientTransition.Position.Y;
    this._targetGradientYPosition = 0.0f;
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        "res://materials/transitions/fight_transition_mat.tres",
        "res://materials/transitions/fade_transition_mat.tres"
      });
    }
  }

  public async Task FadeOut(float time = 0.8f, string transitionPath = "res://materials/transitions/fade_transition_mat.tres", CancellationToken? cancelToken = null)
  {
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      this.InTransition = true;
      ((CanvasItem) this).Visible = false;
      transitionMaterial = (ShaderMaterial) null;
    }
    else
    {
      this.InTransition = true;
      Control simpleTransition = this._simpleTransition;
      Color modulate1 = ((CanvasItem) this._simpleTransition).Modulate;
      modulate1.A = 0.0f;
      Color color1 = modulate1;
      ((CanvasItem) simpleTransition).Modulate = color1;
      Control gradientTransition = this._gradientTransition;
      Color modulate2 = ((CanvasItem) this._gradientTransition).Modulate;
      modulate2.A = 0.0f;
      Color color2 = modulate2;
      ((CanvasItem) gradientTransition).Modulate = color2;
      this._tween?.Kill();
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._tween.TweenProperty((GodotObject) this._simpleTransition, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), (double) time).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
      if (!(((CanvasItem) this).Material is ShaderMaterial material))
      {
        Log.Warn($"NTransition.Material is null or not a ShaderMaterial (actual: {((CanvasItem) this).Material?.GetType().Name ?? "null"}). Skipping transition.");
        transitionMaterial = (ShaderMaterial) null;
      }
      else
      {
        Variant shaderParameter = material.GetShaderParameter(NTransition._threshold);
        if ((double) ((Variant) ref shaderParameter).AsSingle() > 0.99999898672103882)
        {
          transitionMaterial = (ShaderMaterial) null;
        }
        else
        {
          ((CanvasItem) this).Material = PreloadManager.Cache.GetMaterial(transitionPath);
          if (!(((CanvasItem) this).Material is ShaderMaterial transitionMaterial))
          {
            Log.Warn($"NTransition.Material failed to load from cache (path: {transitionPath}). Skipping transition.");
            transitionMaterial = (ShaderMaterial) null;
          }
          else
          {
            transitionMaterial.SetShaderParameter(NTransition._threshold, Variant.op_Implicit(0.0f));
            this._tween.TweenProperty((GodotObject) transitionMaterial, NTransition._thresholdTweenPath, Variant.op_Implicit(1f), (double) time);
            double t = 0.0;
            while (t < (double) time)
            {
              if (cancelToken.HasValue && cancelToken.GetValueOrDefault().IsCancellationRequested)
              {
                Tween tween = this._tween;
                if (tween != null)
                {
                  tween.FastForwardToCompletion();
                  break;
                }
                break;
              }
              t += ((Node) this).GetProcessDeltaTime();
              double num = (double) await ((Node) this).AwaitProcessFrame();
            }
            ((Control) this).MouseFilter = (Control.MouseFilterEnum) 0L;
            transitionMaterial.SetShaderParameter(NTransition._threshold, Variant.op_Implicit(1f));
            transitionMaterial = (ShaderMaterial) null;
          }
        }
      }
    }
  }

  public async Task FadeIn(float time = 0.8f, string transitionPath = "res://materials/transitions/fade_transition_mat.tres", CancellationToken? cancelToken = null)
  {
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      ((CanvasItem) this).Visible = false;
      this.InTransition = false;
      ((Control) this).MouseFilter = (Control.MouseFilterEnum) 2L;
      transitionMaterial = (ShaderMaterial) null;
    }
    else
    {
      this._tween?.Kill();
      Control simpleTransition = this._simpleTransition;
      Color modulate1 = ((CanvasItem) this._simpleTransition).Modulate;
      modulate1.A = 0.0f;
      Color color1 = modulate1;
      ((CanvasItem) simpleTransition).Modulate = color1;
      Control gradientTransition = this._gradientTransition;
      Color modulate2 = ((CanvasItem) this._gradientTransition).Modulate;
      modulate2.A = 0.0f;
      Color color2 = modulate2;
      ((CanvasItem) gradientTransition).Modulate = color2;
      if (!(((CanvasItem) this).Material is ShaderMaterial material))
      {
        Log.Warn($"NTransition.Material is null or not a ShaderMaterial (actual: {((CanvasItem) this).Material?.GetType().Name ?? "null"}). Skipping transition.");
        this.InTransition = false;
        transitionMaterial = (ShaderMaterial) null;
      }
      else
      {
        Variant shaderParameter = material.GetShaderParameter(NTransition._threshold);
        if ((double) ((Variant) ref shaderParameter).AsSingle() < 9.9999999747524271E-07)
        {
          this.InTransition = false;
          transitionMaterial = (ShaderMaterial) null;
        }
        else
        {
          ((CanvasItem) this).Material = PreloadManager.Cache.GetMaterial(transitionPath);
          if (!(((CanvasItem) this).Material is ShaderMaterial transitionMaterial))
          {
            Log.Warn($"NTransition.Material failed to load from cache (path: {transitionPath}). Skipping transition.");
            this.InTransition = false;
            transitionMaterial = (ShaderMaterial) null;
          }
          else
          {
            transitionMaterial.SetShaderParameter(NTransition._threshold, Variant.op_Implicit(1f));
            this._tween = ((Node) this).CreateTween();
            this._tween.TweenProperty((GodotObject) transitionMaterial, NTransition._thresholdTweenPath, Variant.op_Implicit(0.0f), (double) time).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 7L);
            ((Control) this).MouseFilter = (Control.MouseFilterEnum) 2L;
            double t = 0.0;
            while (t < (double) time)
            {
              if (cancelToken.HasValue && cancelToken.GetValueOrDefault().IsCancellationRequested)
              {
                Tween tween = this._tween;
                if (tween != null)
                {
                  tween.FastForwardToCompletion();
                  break;
                }
                break;
              }
              t += ((Node) this).GetProcessDeltaTime();
              double num = (double) await ((Node) this).AwaitProcessFrame();
              if (t / (double) time > 0.75)
                this.InTransition = false;
            }
            this.InTransition = false;
            transitionMaterial.SetShaderParameter(NTransition._threshold, Variant.op_Implicit(0.0f));
            ((Control) this).MouseFilter = (Control.MouseFilterEnum) 2L;
            transitionMaterial = (ShaderMaterial) null;
          }
        }
      }
    }
  }

  public async Task RoomFadeOut()
  {
    this.InTransition = true;
    if (TestMode.IsOn || SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
      return;
    Control simpleTransition = this._simpleTransition;
    Color modulate1 = ((CanvasItem) this._simpleTransition).Modulate;
    modulate1.A = 0.0f;
    Color color1 = modulate1;
    ((CanvasItem) simpleTransition).Modulate = color1;
    Control gradientTransition1 = this._gradientTransition;
    Color modulate2 = ((CanvasItem) this._gradientTransition).Modulate;
    modulate2.A = 1f;
    Color color2 = modulate2;
    ((CanvasItem) gradientTransition1).Modulate = color2;
    Control gradientTransition2 = this._gradientTransition;
    Vector2 position = this._gradientTransition.Position;
    position.Y = this._initialGradientYPosition;
    Vector2 vector2 = position;
    gradientTransition2.Position = vector2;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Normal)
    {
      this._tween.TweenProperty((GodotObject) this._gradientTransition, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._targetGradientYPosition), 0.6).SetDelay(0.5);
      this._tween.TweenProperty((GodotObject) this._simpleTransition, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.6).SetDelay(0.5);
    }
    else if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
      this._tween.TweenProperty((GodotObject) this._simpleTransition, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L).SetDelay(0.3);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    ((Control) this).MouseFilter = (Control.MouseFilterEnum) 0L;
  }

  public async Task RoomFadeIn(bool showTransition = true)
  {
    if (TestMode.IsOn)
      return;
    if (!showTransition || SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
    {
      Control simpleTransition = this._simpleTransition;
      Color modulate = ((CanvasItem) this._simpleTransition).Modulate;
      modulate.A = 0.0f;
      Color color = modulate;
      ((CanvasItem) simpleTransition).Modulate = color;
    }
    if (!(((CanvasItem) this).Material is ShaderMaterial material))
    {
      Log.Warn($"NTransition.Material is null or not a ShaderMaterial (actual: {((CanvasItem) this).Material?.GetType().Name ?? "null"}). Skipping transition.");
      this.InTransition = false;
    }
    else
    {
      material.SetShaderParameter(NTransition._threshold, Variant.op_Implicit(0.0f));
      Control gradientTransition = this._gradientTransition;
      Color modulate1 = ((CanvasItem) this._gradientTransition).Modulate;
      modulate1.A = 0.0f;
      Color color1 = modulate1;
      ((CanvasItem) gradientTransition).Modulate = color1;
      Control simpleTransition = this._simpleTransition;
      Color modulate2 = ((CanvasItem) this._simpleTransition).Modulate;
      modulate2.A = 1f;
      Color color2 = modulate2;
      ((CanvasItem) simpleTransition).Modulate = color2;
      this._tween?.Kill();
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast)
      {
        this._tween.TweenProperty((GodotObject) this._simpleTransition, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.3);
        ((Control) this).MouseFilter = (Control.MouseFilterEnum) 2L;
      }
      else
      {
        this._tween.TweenProperty((GodotObject) this._simpleTransition, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.8);
        this._tween.TweenCallback(Callable.From((Action) (() =>
        {
          ((Control) this).MouseFilter = (Control.MouseFilterEnum) 2L;
          this.InTransition = false;
        }))).SetDelay(0.2);
      }
      if (!await this._tween.AwaitFinished((Node) this))
        return;
      this.InTransition = false;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NTransition.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NTransition.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTransition.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTransition.PropertyName.InTransition))
    {
      this.InTransition = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._initialGradientYPosition))
    {
      this._initialGradientYPosition = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._targetGradientYPosition))
    {
      this._targetGradientYPosition = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._gradientTransition))
    {
      this._gradientTransition = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._simpleTransition))
    {
      this._simpleTransition = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTransition.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTransition.PropertyName.InTransition))
    {
      ref godot_variant local = ref value;
      bool inTransition = this.InTransition;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref inTransition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._initialGradientYPosition))
    {
      value = VariantUtils.CreateFrom<float>(ref this._initialGradientYPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._targetGradientYPosition))
    {
      value = VariantUtils.CreateFrom<float>(ref this._targetGradientYPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._gradientTransition))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._gradientTransition);
      return true;
    }
    if (StringName.op_Equality(ref name, NTransition.PropertyName._simpleTransition))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._simpleTransition);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTransition.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NTransition.PropertyName.InTransition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTransition.PropertyName._initialGradientYPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTransition.PropertyName._targetGradientYPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTransition.PropertyName._gradientTransition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTransition.PropertyName._simpleTransition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTransition.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName inTransition1 = NTransition.PropertyName.InTransition;
    bool inTransition2 = this.InTransition;
    Variant variant = Variant.From<bool>(ref inTransition2);
    serializationInfo.AddProperty(inTransition1, variant);
    info.AddProperty(NTransition.PropertyName._initialGradientYPosition, Variant.From<float>(ref this._initialGradientYPosition));
    info.AddProperty(NTransition.PropertyName._targetGradientYPosition, Variant.From<float>(ref this._targetGradientYPosition));
    info.AddProperty(NTransition.PropertyName._gradientTransition, Variant.From<Control>(ref this._gradientTransition));
    info.AddProperty(NTransition.PropertyName._simpleTransition, Variant.From<Control>(ref this._simpleTransition));
    info.AddProperty(NTransition.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTransition.PropertyName.InTransition, ref variant1))
      this.InTransition = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NTransition.PropertyName._initialGradientYPosition, ref variant2))
      this._initialGradientYPosition = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NTransition.PropertyName._targetGradientYPosition, ref variant3))
      this._targetGradientYPosition = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NTransition.PropertyName._gradientTransition, ref variant4))
      this._gradientTransition = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NTransition.PropertyName._simpleTransition, ref variant5))
      this._simpleTransition = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (!info.TryGetProperty(NTransition.PropertyName._tween, ref variant6))
      return;
    this._tween = ((Variant) ref variant6).As<Tween>();
  }

  public class MethodName : ColorRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : ColorRect.PropertyName
  {
    public static readonly StringName InTransition = StringName.op_Implicit(nameof (InTransition));
    public static readonly StringName _initialGradientYPosition = StringName.op_Implicit(nameof (_initialGradientYPosition));
    public static readonly StringName _targetGradientYPosition = StringName.op_Implicit(nameof (_targetGradientYPosition));
    public static readonly StringName _gradientTransition = StringName.op_Implicit(nameof (_gradientTransition));
    public static readonly StringName _simpleTransition = StringName.op_Implicit(nameof (_simpleTransition));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : ColorRect.SignalName
  {
  }
}
