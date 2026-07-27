// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NPower.cs")]
public class NPower : Control
{
  private static readonly StringName _pulse = new StringName("pulse");
  private PowerModel? _model;
  private TextureRect _icon;
  private MegaLabel _amountLabel;
  private CpuParticles2D _powerFlash;
  private Tween? _animInTween;

  public NPowerContainer Container { get; set; }

  private static string ScenePath => SceneHelper.GetScenePath("combat/power");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NPower.ScenePath);
    }
  }

  public PowerModel Model
  {
    get
    {
      return this._model ?? throw new InvalidOperationException("Model was accessed before it was set.");
    }
    set
    {
      if (this._model != null)
        this.UnsubscribeFromModelEvents();
      value.AssertMutable();
      this._model = value;
      if (this._model != null && ((Node) this).IsInsideTree())
        this.SubscribeToModelEvents();
      this.Reload();
    }
  }

  public static NPower Create(PowerModel power)
  {
    NPower npower = PreloadManager.Cache.GetScene(NPower.ScenePath).Instantiate<NPower>((PackedScene.GenEditState) 0L);
    npower.Model = power;
    return npower;
  }

  public override void _Ready()
  {
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._amountLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AmountLabel"));
    this._powerFlash = ((Node) this).GetNode<CpuParticles2D>(NodePath.op_Implicit("%PowerFlash"));
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
    this.Reload();
    this._animInTween?.Kill();
    this._animInTween = ((Node) this).CreateTween().SetParallel(true);
    this._animInTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("position:y"), Variant.op_Implicit(0.0f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(-24f));
    this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public override void _EnterTree() => this.SubscribeToModelEvents();

  public override void _ExitTree() => this.UnsubscribeFromModelEvents();

  private void Reload()
  {
    if (!((Node) this).IsNodeReady())
      return;
    this._icon.Texture = this._model?.Icon;
    this._powerFlash.Texture = this._model?.BigIcon;
    this.RefreshAmount();
  }

  private void OnPulsingStarted()
  {
    ((ShaderMaterial) ((CanvasItem) this._icon).Material).SetShaderParameter(NPower._pulse, Variant.op_Implicit(1));
  }

  private void OnPulsingStopped()
  {
    ((ShaderMaterial) ((CanvasItem) this._icon).Material).SetShaderParameter(NPower._pulse, Variant.op_Implicit(0));
  }

  private void RefreshAmount()
  {
    if (this._model != null)
    {
      ((Control) this._amountLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, this.Model.AmountLabelColor);
      this._amountLabel.SetTextAutoSize(this.Model.StackType == PowerStackType.Counter ? this.Model.DisplayAmount.ToString() : string.Empty);
    }
    else
      this._amountLabel.SetTextAutoSize(string.Empty);
  }

  private void OnDisplayAmountChanged()
  {
    this.FlashPower();
    this.RefreshAmount();
  }

  private void OnPowerFlashed(PowerModel _) => this.FlashPower();

  private void FlashPower() => this._powerFlash.Emitting = true;

  private void OnHovered()
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Model.Owner)?.ShowHoverTips(this.Model.HoverTips);
    ((Control) this._icon).Scale = Vector2.op_Multiply(Vector2.One, 1.1f);
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.ShowPowerHoverTips);
  }

  private void OnUnhovered()
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Model.Owner)?.HideHoverTips();
    ((Control) this._icon).Scale = Vector2.op_Multiply(Vector2.One, 1f);
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.ShowPowerHoverTips);
  }

  private void ShowPowerHoverTips(CombatState _)
  {
    NCombatRoom.Instance?.GetCreatureNode(this.Model.Owner)?.ShowHoverTips(this.Model.HoverTips);
  }

  private void SubscribeToModelEvents()
  {
    if (this._model == null)
      return;
    this.Model.DisplayAmountChanged += new Action(this.OnDisplayAmountChanged);
    this.Model.Flashed += new Action<PowerModel>(this.OnPowerFlashed);
    this.Model.Removed += new Action(this.OnPowerRemoved);
    this.Model.Owner.Died += new Action<Creature>(this.OnOwnerDied);
    this.Model.Owner.Revived += new Action<Creature>(this.OnOwnerRevived);
    this.Model.PulsingStarted += new Action(this.OnPulsingStarted);
    this.Model.PulsingStopped += new Action(this.OnPulsingStopped);
  }

  private void UnsubscribeFromModelEvents()
  {
    if (this._model == null)
      return;
    this.Model.DisplayAmountChanged -= new Action(this.OnDisplayAmountChanged);
    this.Model.Flashed -= new Action<PowerModel>(this.OnPowerFlashed);
    this.Model.Removed -= new Action(this.OnPowerRemoved);
    this.Model.Owner.Died -= new Action<Creature>(this.OnOwnerDied);
    this.Model.Owner.Revived -= new Action<Creature>(this.OnOwnerRevived);
    this.Model.PulsingStarted -= new Action(this.OnPulsingStarted);
    this.Model.PulsingStopped -= new Action(this.OnPulsingStopped);
  }

  private void OnPowerRemoved() => this.UnsubscribeFromModelEvents();

  private void OnOwnerDied(Creature _)
  {
    if (!GodotObject.IsInstanceValid((GodotObject) this) || !this.Model.ShouldPowerBeRemovedAfterOwnerDeath())
      return;
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
  }

  private void OnOwnerRevived(Creature _) => this.MouseFilter = (Control.MouseFilterEnum) 0L;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NPower.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.Reload, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.OnPulsingStarted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.OnPulsingStopped, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.RefreshAmount, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.OnDisplayAmountChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.FlashPower, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.OnHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.OnUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.SubscribeToModelEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.UnsubscribeFromModelEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPower.MethodName.OnPowerRemoved, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPower.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.Reload) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Reload();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.OnPulsingStarted) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPulsingStarted();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.OnPulsingStopped) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPulsingStopped();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.RefreshAmount) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshAmount();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.OnDisplayAmountChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisplayAmountChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.FlashPower) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FlashPower();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.OnHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.OnUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnhovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.SubscribeToModelEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SubscribeToModelEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPower.MethodName.UnsubscribeFromModelEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UnsubscribeFromModelEvents();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPower.MethodName.OnPowerRemoved) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnPowerRemoved();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPower.MethodName._Ready) || StringName.op_Equality(ref method, NPower.MethodName._EnterTree) || StringName.op_Equality(ref method, NPower.MethodName._ExitTree) || StringName.op_Equality(ref method, NPower.MethodName.Reload) || StringName.op_Equality(ref method, NPower.MethodName.OnPulsingStarted) || StringName.op_Equality(ref method, NPower.MethodName.OnPulsingStopped) || StringName.op_Equality(ref method, NPower.MethodName.RefreshAmount) || StringName.op_Equality(ref method, NPower.MethodName.OnDisplayAmountChanged) || StringName.op_Equality(ref method, NPower.MethodName.FlashPower) || StringName.op_Equality(ref method, NPower.MethodName.OnHovered) || StringName.op_Equality(ref method, NPower.MethodName.OnUnhovered) || StringName.op_Equality(ref method, NPower.MethodName.SubscribeToModelEvents) || StringName.op_Equality(ref method, NPower.MethodName.UnsubscribeFromModelEvents) || StringName.op_Equality(ref method, NPower.MethodName.OnPowerRemoved) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPower.PropertyName.Container))
    {
      this.Container = VariantUtils.ConvertTo<NPowerContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPower.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPower.PropertyName._amountLabel))
    {
      this._amountLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPower.PropertyName._powerFlash))
    {
      this._powerFlash = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPower.PropertyName._animInTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPower.PropertyName.Container))
    {
      ref godot_variant local = ref value;
      NPowerContainer container = this.Container;
      godot_variant from = VariantUtils.CreateFrom<NPowerContainer>(ref container);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPower.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NPower.PropertyName._amountLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._amountLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NPower.PropertyName._powerFlash))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._powerFlash);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPower.PropertyName._animInTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPower.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPower.PropertyName._amountLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPower.PropertyName._powerFlash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPower.PropertyName._animInTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPower.PropertyName.Container, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName container1 = NPower.PropertyName.Container;
    NPowerContainer container2 = this.Container;
    Variant variant = Variant.From<NPowerContainer>(ref container2);
    serializationInfo.AddProperty(container1, variant);
    info.AddProperty(NPower.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NPower.PropertyName._amountLabel, Variant.From<MegaLabel>(ref this._amountLabel));
    info.AddProperty(NPower.PropertyName._powerFlash, Variant.From<CpuParticles2D>(ref this._powerFlash));
    info.AddProperty(NPower.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPower.PropertyName.Container, ref variant1))
      this.Container = ((Variant) ref variant1).As<NPowerContainer>();
    Variant variant2;
    if (info.TryGetProperty(NPower.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NPower.PropertyName._amountLabel, ref variant3))
      this._amountLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NPower.PropertyName._powerFlash, ref variant4))
      this._powerFlash = ((Variant) ref variant4).As<CpuParticles2D>();
    Variant variant5;
    if (!info.TryGetProperty(NPower.PropertyName._animInTween, ref variant5))
      return;
    this._animInTween = ((Variant) ref variant5).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName Reload = StringName.op_Implicit(nameof (Reload));
    public static readonly StringName OnPulsingStarted = StringName.op_Implicit(nameof (OnPulsingStarted));
    public static readonly StringName OnPulsingStopped = StringName.op_Implicit(nameof (OnPulsingStopped));
    public static readonly StringName RefreshAmount = StringName.op_Implicit(nameof (RefreshAmount));
    public static readonly StringName OnDisplayAmountChanged = StringName.op_Implicit(nameof (OnDisplayAmountChanged));
    public static readonly StringName FlashPower = StringName.op_Implicit(nameof (FlashPower));
    public static readonly StringName OnHovered = StringName.op_Implicit(nameof (OnHovered));
    public static readonly StringName OnUnhovered = StringName.op_Implicit(nameof (OnUnhovered));
    public static readonly StringName SubscribeToModelEvents = StringName.op_Implicit(nameof (SubscribeToModelEvents));
    public static readonly StringName UnsubscribeFromModelEvents = StringName.op_Implicit(nameof (UnsubscribeFromModelEvents));
    public static readonly StringName OnPowerRemoved = StringName.op_Implicit(nameof (OnPowerRemoved));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Container = StringName.op_Implicit(nameof (Container));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _amountLabel = StringName.op_Implicit(nameof (_amountLabel));
    public static readonly StringName _powerFlash = StringName.op_Implicit(nameof (_powerFlash));
    public static readonly StringName _animInTween = StringName.op_Implicit(nameof (_animInTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
