// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.RestSite.NRestSiteButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.RestSite;

[ScriptPath("res://src/Core/Nodes/RestSite/NRestSiteButton.cs")]
public class NRestSiteButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private ShaderMaterial _hsv;
  private Control _visuals;
  private TextureRect _icon;
  private Control _outline;
  private MegaLabel _label;
  private Vector2 _labelPosition;
  private bool _isUnclickable;
  private bool _executingOption;
  private const double _unfocusAnimDur = 1.0;
  private readonly CancellationTokenSource _cts = new CancellationTokenSource();
  private Tween? _currentTween;
  private RestSiteOption? _option;
  private static readonly string _scenePath = SceneHelper.GetScenePath("rest_site/rest_site_button");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NRestSiteButton._scenePath);
    }
  }

  public RestSiteOption Option
  {
    get => this._option ?? throw new InvalidOperationException("Option accessed before being set");
    set
    {
      this._option = value;
      this.Reload();
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    ((CanvasItem) this).Modulate = StsColors.transparentBlack;
    this._visuals = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Visuals"));
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
    this._labelPosition = ((Control) this._label).Position;
    TaskHelper.RunSafely(this.AnimateIn());
    this.Reload();
  }

  private async Task AnimateIn()
  {
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    await tween.AwaitFinished(this._cts.Token);
    if (!((Node) this).IsValid())
      return;
    this.MouseFilter = (Control.MouseFilterEnum) 0L;
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._cts.Cancel();
    this._currentTween?.Kill();
  }

  public static NRestSiteButton Create(RestSiteOption option)
  {
    NRestSiteButton nrestSiteButton = PreloadManager.Cache.GetScene(NRestSiteButton._scenePath).Instantiate<NRestSiteButton>((PackedScene.GenEditState) 0L);
    nrestSiteButton.Option = option;
    nrestSiteButton._isUnclickable = !option.IsEnabled;
    return nrestSiteButton;
  }

  private void Reload()
  {
    if (!((Node) this).IsNodeReady() || this._option == (RestSiteOption) null)
      return;
    this._icon.Texture = this.Option.Icon;
    this._label.SetTextAutoSize(this.Option.Title.GetFormattedText());
    if (!this._option.IsEnabled)
    {
      this._hsv.SetShaderParameter(NRestSiteButton._s, Variant.op_Implicit(0.0f));
      this._hsv.SetShaderParameter(NRestSiteButton._v, Variant.op_Implicit(0.6f));
    }
    else
    {
      this._hsv.SetShaderParameter(NRestSiteButton._s, Variant.op_Implicit(1f));
      this._hsv.SetShaderParameter(NRestSiteButton._v, Variant.op_Implicit(1f));
    }
  }

  protected override void OnRelease()
  {
    if (this._isUnclickable)
      return;
    base.OnRelease();
    TaskHelper.RunSafely(this.SelectOption(this.Option));
  }

  private async Task SelectOption(RestSiteOption option)
  {
    int index = NRestSiteRoom.Instance.Options.IndexOf<RestSiteOption>(option);
    if (index < 0)
      throw new InvalidOperationException($"Rest site option {option} was selected, but it was not in the list of rest site options!");
    this._executingOption = true;
    NRestSiteRoom.Instance.DisableOptions();
    this.RefreshTextState();
    bool success = false;
    try
    {
      success = await RunManager.Instance.RestSiteSynchronizer.ChooseLocalOption(index);
      if (success)
        NRestSiteRoom.Instance.AfterSelectingOption(option);
    }
    finally
    {
      this._executingOption = false;
      if (((Node) this).IsValid())
        this.RefreshTextState();
      if (!success && ((Node) this).IsValid())
      {
        double num = (double) await ((Node) this).AwaitProcessFrame();
        NRestSiteRoom.Instance.EnableOptions();
      }
    }
  }

  protected override void OnPress()
  {
    if (this._isUnclickable)
      return;
    base.OnPress();
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position"), Variant.op_Implicit(this._labelPosition), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1.2f), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
    this._currentTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.05);
    this._currentTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(this._labelPosition, new Vector2(0.0f, 6f))), 0.05);
    if (!this._isUnclickable)
      this._hsv.SetShaderParameter(NRestSiteButton._v, Variant.op_Implicit(1.2f));
    this.RefreshTextState();
  }

  protected override void OnUnfocus()
  {
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("position"), Variant.op_Implicit(this._labelPosition), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.9f)), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (!this._isUnclickable)
      this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1.2f), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this.RefreshTextState();
  }

  public void RefreshTextState()
  {
    NRestSiteRoom instance = NRestSiteRoom.Instance;
    if (instance == null)
      return;
    if (this.IsFocused || this._executingOption)
      instance.SetText(this.Option.Description.GetFormattedText());
    else
      instance.FadeOutOptionDescription();
  }

  private void UpdateShaderParam(float value)
  {
    this._hsv.SetShaderParameter(NRestSiteButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NRestSiteButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.RefreshTextState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteButton.MethodName.RefreshTextState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshTextState();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRestSiteButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRestSiteButton.MethodName._Ready) || StringName.op_Equality(ref method, NRestSiteButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.Reload) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnPress) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.RefreshTextState) || StringName.op_Equality(ref method, NRestSiteButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._visuals))
    {
      this._visuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._labelPosition))
    {
      this._labelPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._isUnclickable))
    {
      this._isUnclickable = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._executingOption))
    {
      this._executingOption = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteButton.PropertyName._currentTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._visuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._labelPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._labelPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._isUnclickable))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isUnclickable);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteButton.PropertyName._executingOption))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._executingOption);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteButton.PropertyName._currentTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRestSiteButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteButton.PropertyName._visuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NRestSiteButton.PropertyName._labelPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRestSiteButton.PropertyName._isUnclickable, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NRestSiteButton.PropertyName._executingOption, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteButton.PropertyName._currentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRestSiteButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NRestSiteButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
    info.AddProperty(NRestSiteButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NRestSiteButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NRestSiteButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NRestSiteButton.PropertyName._labelPosition, Variant.From<Vector2>(ref this._labelPosition));
    info.AddProperty(NRestSiteButton.PropertyName._isUnclickable, Variant.From<bool>(ref this._isUnclickable));
    info.AddProperty(NRestSiteButton.PropertyName._executingOption, Variant.From<bool>(ref this._executingOption));
    info.AddProperty(NRestSiteButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._hsv, ref variant1))
      this._hsv = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._visuals, ref variant2))
      this._visuals = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._icon, ref variant3))
      this._icon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._outline, ref variant4))
      this._outline = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._label, ref variant5))
      this._label = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._labelPosition, ref variant6))
      this._labelPosition = ((Variant) ref variant6).As<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._isUnclickable, ref variant7))
      this._isUnclickable = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NRestSiteButton.PropertyName._executingOption, ref variant8))
      this._executingOption = ((Variant) ref variant8).As<bool>();
    Variant variant9;
    if (!info.TryGetProperty(NRestSiteButton.PropertyName._currentTween, ref variant9))
      return;
    this._currentTween = ((Variant) ref variant9).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName RefreshTextState = StringName.op_Implicit(nameof (RefreshTextState));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _visuals = StringName.op_Implicit(nameof (_visuals));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _labelPosition = StringName.op_Implicit(nameof (_labelPosition));
    public static readonly StringName _isUnclickable = StringName.op_Implicit(nameof (_isUnclickable));
    public static readonly StringName _executingOption = StringName.op_Implicit(nameof (_executingOption));
    public static readonly StringName _currentTween = StringName.op_Implicit(nameof (_currentTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
