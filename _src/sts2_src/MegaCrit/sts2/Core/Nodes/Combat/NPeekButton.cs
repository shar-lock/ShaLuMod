// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NPeekButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NPeekButton.cs")]
public class NPeekButton : NButton
{
  private static readonly StringName _pulseStrength = new StringName("pulse_strength");
  private readonly List<Control> _targets = new List<Control>();
  private readonly List<Control> _hiddenTargets = new List<Control>();
  private TextureRect _flash;
  private Control _visuals;
  private IOverlayScreen? _overlayScreenParent;
  private Tween? _hoverTween;
  private Tween? _wiggleTween;
  private 
  #nullable disable
  NPeekButton.ToggledEventHandler backing_Toggled;

  protected override 
  #nullable enable
  string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.peek)
      };
    }
  }

  public bool IsPeeking { get; private set; }

  public Marker2D CurrentCardMarker { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._flash = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Flash"));
    this._visuals = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Visuals"));
    this.CurrentCardMarker = ((Node) this).GetNode<Marker2D>(NodePath.op_Implicit("%CurrentCardMarker"));
    if (NCombatRoom.Instance != null)
    {
      if (((Node) NCombatRoom.Instance).IsNodeReady())
        this.OnCombatRoomReady();
      else
        ((GodotObject) NCombatRoom.Instance).Connect(Node.SignalName.Ready, Callable.From(new Action(this.OnCombatRoomReady)), 0U);
    }
    if (!CombatManager.Instance.IsInProgress)
      this.Disable();
    for (Node parent = ((Node) this).GetParent(); parent != null; parent = parent.GetParent())
    {
      if (parent is IOverlayScreen overlayScreen)
      {
        this._overlayScreenParent = overlayScreen;
        ((GodotObject) NOverlayStack.Instance)?.Connect(NOverlayStack.SignalName.Changed, Callable.From(new Action(this.OnOverlayStackChanged)), 0U);
        NCapstoneContainer instance = NCapstoneContainer.Instance;
        if (instance == null)
          break;
        ((GodotObject) instance).Connect(NCapstoneContainer.SignalName.Changed, Callable.From(new Action(this.OnOverlayStackChanged)), 0U);
        break;
      }
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    ((CanvasItem) this).Visible = true;
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    ((CanvasItem) this).Visible = false;
  }

  private void OnOverlayStackChanged()
  {
    if (!this.IsPeeking || this._overlayScreenParent == null || NCapstoneContainer.Instance?.CurrentCapstoneScreen != null || this._overlayScreenParent != NOverlayStack.Instance?.Peek())
      return;
    NOverlayStack.Instance.HideBackstop();
  }

  public void Wiggle()
  {
    ((CanvasItem) this._flash).Visible = true;
    TextureRect flash = this._flash;
    Color modulate = ((CanvasItem) this._flash).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) flash).Modulate = color;
    this._wiggleTween?.Kill();
    this._wiggleTween = ((Node) this).CreateTween();
    this._visuals.RotationDegrees = 0.0f;
    this._wiggleTween.TweenMethod(Callable.From<float>((Action<float>) (t => this._visuals.RotationDegrees = 10f * Mathf.Sin(t * 3f) * Mathf.Sin(t * 0.5f))), Variant.op_Implicit(0.0f), Variant.op_Implicit(6.28318548f), 0.5);
    this._wiggleTween.Parallel().TweenMethod(Callable.From<float>((Action<float>) (t => this._visuals.Scale = Vector2.op_Addition(Vector2.One, Vector2.op_Multiply(Vector2.op_Multiply(Vector2.op_Multiply(Vector2.One, 0.15f), Mathf.Sin(t)), Mathf.Sin(t * 0.5f))))), Variant.op_Implicit(0.0f), Variant.op_Implicit(3.14159274f), 0.25);
    this._wiggleTween.Parallel().TweenProperty((GodotObject) this._flash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.1);
    this._wiggleTween.Chain().TweenProperty((GodotObject) this._flash, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.3).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 1L);
    NDebugAudioManager.Instance.Play("deny.mp3", 0.5f, PitchVariance.Medium);
  }

  public void AddTargets(params Control[] targets)
  {
    this._targets.AddRange((IEnumerable<Control>) targets);
  }

  public void SetPeeking(bool isPeeking)
  {
    if (this.IsPeeking == isPeeking)
      return;
    this.IsPeeking = isPeeking;
    if (NOverlayStack.Instance.ScreenCount > 0)
    {
      if (this.IsPeeking)
        NOverlayStack.Instance.HideBackstop();
      else
        NOverlayStack.Instance.ShowBackstop();
    }
    if (this.IsPeeking)
    {
      foreach (Control control in this._targets.Where<Control>((Func<Control, bool>) (t => ((CanvasItem) t).Visible)))
      {
        this._hiddenTargets.Add(control);
        ((CanvasItem) control).Visible = false;
      }
    }
    else
    {
      foreach (CanvasItem hiddenTarget in this._hiddenTargets)
        hiddenTarget.Visible = true;
      this._hiddenTargets.Clear();
    }
    ((ShaderMaterial) ((CanvasItem) this._visuals).Material).SetShaderParameter(NPeekButton._pulseStrength, Variant.op_Implicit(this.IsPeeking ? 1 : 0));
    ((GodotObject) this).EmitSignal(NPeekButton.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  protected override void OnRelease()
  {
    this.SetPeeking(!this.IsPeeking);
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.15);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.95f)), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.1f)), 0.05);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.15);
  }

  private void OnCombatRoomReady() => NCombatRoom.Instance.Ui.OnPeekButtonReady(this);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NPeekButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnOverlayStackChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.Wiggle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.AddTargets, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 28L, StringName.op_Implicit("targets"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.SetPeeking, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isPeeking"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPeekButton.MethodName.OnCombatRoomReady, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPeekButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnOverlayStackChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnOverlayStackChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.Wiggle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Wiggle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.AddTargets) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddTargets(VariantUtils.ConvertToSystemArrayOfGodotObject<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.SetPeeking) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPeeking(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPeekButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPeekButton.MethodName.OnCombatRoomReady) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnCombatRoomReady();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPeekButton.MethodName._Ready) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnOverlayStackChanged) || StringName.op_Equality(ref method, NPeekButton.MethodName.Wiggle) || StringName.op_Equality(ref method, NPeekButton.MethodName.AddTargets) || StringName.op_Equality(ref method, NPeekButton.MethodName.SetPeeking) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnPress) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPeekButton.MethodName.OnCombatRoomReady) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName.IsPeeking))
    {
      this.IsPeeking = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName.CurrentCardMarker))
    {
      this.CurrentCardMarker = VariantUtils.ConvertTo<Marker2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName._flash))
    {
      this._flash = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName._visuals))
    {
      this._visuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPeekButton.PropertyName._wiggleTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._wiggleTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName.IsPeeking))
    {
      ref godot_variant local = ref value;
      bool isPeeking = this.IsPeeking;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isPeeking);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName.CurrentCardMarker))
    {
      ref godot_variant local = ref value;
      Marker2D currentCardMarker = this.CurrentCardMarker;
      godot_variant from = VariantUtils.CreateFrom<Marker2D>(ref currentCardMarker);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName._flash))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._flash);
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName._visuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NPeekButton.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPeekButton.PropertyName._wiggleTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._wiggleTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NPeekButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPeekButton.PropertyName.IsPeeking, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPeekButton.PropertyName._flash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPeekButton.PropertyName._visuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPeekButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPeekButton.PropertyName._wiggleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPeekButton.PropertyName.CurrentCardMarker, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName isPeeking1 = NPeekButton.PropertyName.IsPeeking;
    bool isPeeking2 = this.IsPeeking;
    Variant variant1 = Variant.From<bool>(ref isPeeking2);
    serializationInfo1.AddProperty(isPeeking1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName currentCardMarker1 = NPeekButton.PropertyName.CurrentCardMarker;
    Marker2D currentCardMarker2 = this.CurrentCardMarker;
    Variant variant2 = Variant.From<Marker2D>(ref currentCardMarker2);
    serializationInfo2.AddProperty(currentCardMarker1, variant2);
    info.AddProperty(NPeekButton.PropertyName._flash, Variant.From<TextureRect>(ref this._flash));
    info.AddProperty(NPeekButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
    info.AddProperty(NPeekButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NPeekButton.PropertyName._wiggleTween, Variant.From<Tween>(ref this._wiggleTween));
    info.AddSignalEventDelegate(NPeekButton.SignalName.Toggled, (Delegate) this.backing_Toggled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPeekButton.PropertyName.IsPeeking, ref variant1))
      this.IsPeeking = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NPeekButton.PropertyName.CurrentCardMarker, ref variant2))
      this.CurrentCardMarker = ((Variant) ref variant2).As<Marker2D>();
    Variant variant3;
    if (info.TryGetProperty(NPeekButton.PropertyName._flash, ref variant3))
      this._flash = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NPeekButton.PropertyName._visuals, ref variant4))
      this._visuals = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NPeekButton.PropertyName._hoverTween, ref variant5))
      this._hoverTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (info.TryGetProperty(NPeekButton.PropertyName._wiggleTween, ref variant6))
      this._wiggleTween = ((Variant) ref variant6).As<Tween>();
    NPeekButton.ToggledEventHandler toggledEventHandler;
    if (!info.TryGetSignalEventDelegate<NPeekButton.ToggledEventHandler>(NPeekButton.SignalName.Toggled, ref toggledEventHandler))
      return;
    this.backing_Toggled = toggledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NPeekButton.SignalName.Toggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("peekButton"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NPeekButton.ToggledEventHandler Toggled
  {
    add => this.backing_Toggled += value;
    remove => this.backing_Toggled -= value;
  }

  protected void EmitSignalToggled(NPeekButton peekButton)
  {
    ((GodotObject) this).EmitSignal(NPeekButton.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) peekButton)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NPeekButton.SignalName.Toggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NPeekButton.ToggledEventHandler backingToggled = this.backing_Toggled;
      if (backingToggled == null)
        return;
      backingToggled(VariantUtils.ConvertTo<NPeekButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NPeekButton.SignalName.Toggled) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ToggledEventHandler(
  #nullable enable
  NPeekButton peekButton);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public static readonly StringName OnOverlayStackChanged = StringName.op_Implicit(nameof (OnOverlayStackChanged));
    public static readonly StringName Wiggle = StringName.op_Implicit(nameof (Wiggle));
    public static readonly StringName AddTargets = StringName.op_Implicit(nameof (AddTargets));
    public static readonly StringName SetPeeking = StringName.op_Implicit(nameof (SetPeeking));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName OnCombatRoomReady = StringName.op_Implicit(nameof (OnCombatRoomReady));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName IsPeeking = StringName.op_Implicit(nameof (IsPeeking));
    public static readonly StringName CurrentCardMarker = StringName.op_Implicit(nameof (CurrentCardMarker));
    public static readonly StringName _flash = StringName.op_Implicit(nameof (_flash));
    public static readonly StringName _visuals = StringName.op_Implicit(nameof (_visuals));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _wiggleTween = StringName.op_Implicit(nameof (_wiggleTween));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Toggled = StringName.op_Implicit(nameof (Toggled));
  }
}
