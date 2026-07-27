// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NEnergyCounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NEnergyCounter.cs")]
public class NEnergyCounter : Control
{
  private const string _darkenedMatPath = "res://materials/ui/energy_orb_dark.tres";
  private Player _player;
  private MegaLabel _label;
  private Control _layers;
  private Control _rotationLayers;
  private NParticlesContainer? _backVfx;
  private NParticlesContainer? _frontVfx;
  private HoverTip _hoverTip;
  private Tween? _animInTween;
  private Tween? _animOutTween;
  private const float _animDuration = 0.6f;
  private static readonly Vector2 _showPosition = Vector2.Zero;
  private static readonly Vector2 _hidePosition = new Vector2(-480f, 128f);

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://materials/ui/energy_orb_dark.tres");
    }
  }

  public static NEnergyCounter? Create(Player player)
  {
    if (TestMode.IsOn)
      return (NEnergyCounter) null;
    NEnergyCounter nenergyCounter = PreloadManager.Cache.GetScene(player.Character.EnergyCounterPath).Instantiate<NEnergyCounter>((PackedScene.GenEditState) 0L);
    nenergyCounter._player = player;
    return nenergyCounter;
  }

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._layers = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Layers"));
    this._rotationLayers = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RotationLayers"));
    this._backVfx = ((Node) this).GetNode<NParticlesContainer>(NodePath.op_Implicit("%EnergyVfxBack"));
    this._frontVfx = ((Node) this).GetNode<NParticlesContainer>(NodePath.op_Implicit("%EnergyVfxFront"));
    LocString description = new LocString("static_hover_tips", "ENERGY_COUNT.description");
    description.Add("energyPrefix", EnergyIconHelper.GetPrefix((AbstractModel) this._player.Character.CardPool));
    this._hoverTip = new HoverTip(new LocString("static_hover_tips", "ENERGY_COUNT.title"), description);
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnHovered)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnhovered)), 0U);
    this.RefreshLabel();
  }

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.OnCombatStateChanged);
    this._player.PlayerCombatState.EnergyChanged += new Action<int, int>(this.OnEnergyChanged);
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.OnCombatStateChanged);
    this._player.PlayerCombatState.EnergyChanged -= new Action<int, int>(this.OnEnergyChanged);
  }

  private void OnHovered()
  {
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) this._hoverTip)?.SetGlobalPosition(Vector2.op_Addition(this.GlobalPosition, new Vector2(-70f, -200f)), false);
  }

  private void OnUnhovered() => NHoverTipSet.Remove((Control) this);

  private void OnCombatStateChanged(CombatState combatState) => this.RefreshLabel();

  private void RefreshLabel()
  {
    PlayerCombatState playerCombatState = this._player.PlayerCombatState;
    this._label.SetTextAutoSize($"{playerCombatState.Energy}/{playerCombatState.MaxEnergy}");
    ((Control) this._label).AddThemeColorOverride(ThemeConstants.Label.FontColor, playerCombatState.Energy == 0 ? StsColors.red : StsColors.cream);
    ((Control) this._label).AddThemeColorOverride(ThemeConstants.Label.FontOutlineColor, playerCombatState.Energy == 0 ? StsColors.unplayableEnergyCostOutline : this.OutlineColor);
    Material material = playerCombatState.Energy == 0 ? PreloadManager.Cache.GetMaterial("res://materials/ui/energy_orb_dark.tres") : (Material) null;
    foreach (CanvasItem canvasItem in ((IEnumerable) ((Node) this._layers).GetChildren(false)).OfType<Control>())
      canvasItem.Material = material;
    foreach (CanvasItem canvasItem in ((IEnumerable) ((Node) this._rotationLayers).GetChildren(false)).OfType<Control>())
      canvasItem.Material = material;
    ((CanvasItem) this._layers).Modulate = playerCombatState.Energy == 0 ? Colors.DarkGray : Colors.White;
  }

  private void OnEnergyChanged(int oldEnergy, int newEnergy)
  {
    if (oldEnergy >= newEnergy)
      return;
    this._backVfx?.Restart();
    this._frontVfx?.Restart();
  }

  private Color OutlineColor => this._player.Character.EnergyLabelOutlineColor;

  public override void _Process(double delta)
  {
    float num = this._player.PlayerCombatState.Energy == 0 ? 5f : 30f;
    for (int index = 0; index < ((Node) this._rotationLayers).GetChildCount(false); ++index)
      ((Node) this._rotationLayers).GetChild<Control>(index, false).RotationDegrees += (float) delta * num * (float) (index + 1);
  }

  public void AnimIn()
  {
    this._animOutTween?.Kill();
    this._animInTween = ((Node) this).CreateTween();
    this.Position = NEnergyCounter._hidePosition;
    this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NEnergyCounter._showPosition), 0.60000002384185791).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void AnimOut()
  {
    this._animInTween?.Kill();
    this._animOutTween = ((Node) this).CreateTween();
    this.Position = NEnergyCounter._showPosition;
    this._animOutTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(NEnergyCounter._hidePosition), 0.60000002384185791).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NEnergyCounter.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName.OnHovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName.OnUnhovered, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName.RefreshLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName.OnEnergyChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("oldEnergy"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("newEnergy"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NEnergyCounter.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName.OnHovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName.OnUnhovered) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnhovered();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName.RefreshLabel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLabel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName.OnEnergyChanged) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.OnEnergyChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NEnergyCounter.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NEnergyCounter.MethodName.AnimOut) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AnimOut();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NEnergyCounter.MethodName._Ready) || StringName.op_Equality(ref method, NEnergyCounter.MethodName._EnterTree) || StringName.op_Equality(ref method, NEnergyCounter.MethodName._ExitTree) || StringName.op_Equality(ref method, NEnergyCounter.MethodName.OnHovered) || StringName.op_Equality(ref method, NEnergyCounter.MethodName.OnUnhovered) || StringName.op_Equality(ref method, NEnergyCounter.MethodName.RefreshLabel) || StringName.op_Equality(ref method, NEnergyCounter.MethodName.OnEnergyChanged) || StringName.op_Equality(ref method, NEnergyCounter.MethodName._Process) || StringName.op_Equality(ref method, NEnergyCounter.MethodName.AnimIn) || StringName.op_Equality(ref method, NEnergyCounter.MethodName.AnimOut) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._layers))
    {
      this._layers = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._rotationLayers))
    {
      this._rotationLayers = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._backVfx))
    {
      this._backVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._frontVfx))
    {
      this._frontVfx = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._animInTween))
    {
      this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEnergyCounter.PropertyName._animOutTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._animOutTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName.OutlineColor))
    {
      ref godot_variant local = ref value;
      Color outlineColor = this.OutlineColor;
      godot_variant from = VariantUtils.CreateFrom<Color>(ref outlineColor);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._layers))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._layers);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._rotationLayers))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rotationLayers);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._backVfx))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._backVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._frontVfx))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._frontVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NEnergyCounter.PropertyName._animInTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NEnergyCounter.PropertyName._animOutTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._animOutTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._layers, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._rotationLayers, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._backVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._frontVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._animInTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NEnergyCounter.PropertyName._animOutTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NEnergyCounter.PropertyName.OutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NEnergyCounter.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NEnergyCounter.PropertyName._layers, Variant.From<Control>(ref this._layers));
    info.AddProperty(NEnergyCounter.PropertyName._rotationLayers, Variant.From<Control>(ref this._rotationLayers));
    info.AddProperty(NEnergyCounter.PropertyName._backVfx, Variant.From<NParticlesContainer>(ref this._backVfx));
    info.AddProperty(NEnergyCounter.PropertyName._frontVfx, Variant.From<NParticlesContainer>(ref this._frontVfx));
    info.AddProperty(NEnergyCounter.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
    info.AddProperty(NEnergyCounter.PropertyName._animOutTween, Variant.From<Tween>(ref this._animOutTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NEnergyCounter.PropertyName._label, ref variant1))
      this._label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NEnergyCounter.PropertyName._layers, ref variant2))
      this._layers = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NEnergyCounter.PropertyName._rotationLayers, ref variant3))
      this._rotationLayers = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NEnergyCounter.PropertyName._backVfx, ref variant4))
      this._backVfx = ((Variant) ref variant4).As<NParticlesContainer>();
    Variant variant5;
    if (info.TryGetProperty(NEnergyCounter.PropertyName._frontVfx, ref variant5))
      this._frontVfx = ((Variant) ref variant5).As<NParticlesContainer>();
    Variant variant6;
    if (info.TryGetProperty(NEnergyCounter.PropertyName._animInTween, ref variant6))
      this._animInTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (!info.TryGetProperty(NEnergyCounter.PropertyName._animOutTween, ref variant7))
      return;
    this._animOutTween = ((Variant) ref variant7).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnHovered = StringName.op_Implicit(nameof (OnHovered));
    public static readonly StringName OnUnhovered = StringName.op_Implicit(nameof (OnUnhovered));
    public static readonly StringName RefreshLabel = StringName.op_Implicit(nameof (RefreshLabel));
    public static readonly StringName OnEnergyChanged = StringName.op_Implicit(nameof (OnEnergyChanged));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName OutlineColor = StringName.op_Implicit(nameof (OutlineColor));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _layers = StringName.op_Implicit(nameof (_layers));
    public static readonly StringName _rotationLayers = StringName.op_Implicit(nameof (_rotationLayers));
    public static readonly StringName _backVfx = StringName.op_Implicit(nameof (_backVfx));
    public static readonly StringName _frontVfx = StringName.op_Implicit(nameof (_frontVfx));
    public static readonly StringName _animInTween = StringName.op_Implicit(nameof (_animInTween));
    public static readonly StringName _animOutTween = StringName.op_Implicit(nameof (_animOutTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
