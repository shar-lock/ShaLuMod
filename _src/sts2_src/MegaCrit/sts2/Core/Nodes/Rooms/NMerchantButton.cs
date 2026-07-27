// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Rooms.NMerchantButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Rooms;

[ScriptPath("res://src/Core/Nodes/Rooms/NMerchantButton.cs")]
public class NMerchantButton : NButton
{
  private MegaSkeleton? _merchantSkeleton;
  private NSelectionReticle _merchantSelectionReticle;
  private bool _focusedWhileTargeting;
  private 
  #nullable disable
  NMerchantButton.MerchantOpenedEventHandler backing_MerchantOpened;

  protected override 
  #nullable enable
  string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.select)
      };
    }
  }

  public bool IsLocalPlayerDead { get; set; }

  public IReadOnlyList<LocString> PlayerDeadLines { get; set; } = (IReadOnlyList<LocString>) Array.Empty<LocString>();

  public override void _Ready()
  {
    this.ConnectSignals();
    this._merchantSelectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%MerchantSelectionReticle"));
    MegaSprite sprite = new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this).GetNode(NodePath.op_Implicit("%MerchantVisual"))));
    ((Node) this).RunWhenSpineReady(sprite, (Action<MegaAnimationState>) (animState =>
    {
      this._merchantSkeleton = sprite.GetSkeleton();
      animState.SetAnimation("idle_loop");
      if (!this.IsFocused || this._focusedWhileTargeting)
        return;
      this._merchantSkeleton.SetSkinByName("outline");
      this._merchantSkeleton.SetSlotsToSetupPose();
    }));
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this.RefreshFocus();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._merchantSelectionReticle.OnDeselect();
    if (this._focusedWhileTargeting)
    {
      NTargetManager.Instance.OnNodeUnhovered((Node) this);
    }
    else
    {
      this._merchantSkeleton?.SetSkinByName("default");
      this._merchantSkeleton?.SetSlotsToSetupPose();
    }
    this._focusedWhileTargeting = false;
  }

  protected override void OnRelease()
  {
    if (this._focusedWhileTargeting)
    {
      this._merchantSelectionReticle.OnDeselect();
      this._focusedWhileTargeting = false;
      this.RefreshFocus();
    }
    else if (this.IsLocalPlayerDead)
    {
      LocString line = Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) this.PlayerDeadLines);
      if (line == null)
        return;
      this.PlayDialogue(line);
    }
    else
      this.EmitSignalMerchantOpened(this);
  }

  public NSpeechBubbleVfx? PlayDialogue(LocString? line, double duration = 2.0)
  {
    if (line == null)
      return (NSpeechBubbleVfx) null;
    NSpeechBubbleVfx child = NSpeechBubbleVfx.Create(line.GetFormattedText(), DialogueSide.Right, Vector2.op_Addition(this.GlobalPosition, Vector2.op_Multiply(this.Size.X, Vector2.Left)), duration, VfxColor.Blue);
    if (child != null)
      ((Node) this).GetParent().AddChildSafely((Node) child);
    return child;
  }

  private void RefreshFocus()
  {
    if (NTargetManager.Instance.IsInSelection && NTargetManager.Instance.AllowedToTargetNode((Node) this))
    {
      NTargetManager.Instance.OnNodeHovered((Node) this);
      this._merchantSelectionReticle.OnSelect();
      this._focusedWhileTargeting = true;
    }
    else
    {
      this._merchantSkeleton?.SetSkinByName("outline");
      this._merchantSkeleton?.SetSlotsToSetupPose();
      this._focusedWhileTargeting = false;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMerchantButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMerchantButton.MethodName.RefreshFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMerchantButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMerchantButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMerchantButton.MethodName.RefreshFocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.RefreshFocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMerchantButton.MethodName._Ready) || StringName.op_Equality(ref method, NMerchantButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NMerchantButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMerchantButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NMerchantButton.MethodName.RefreshFocus) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantButton.PropertyName.IsLocalPlayerDead))
    {
      this.IsLocalPlayerDead = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantButton.PropertyName._merchantSelectionReticle))
    {
      this._merchantSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantButton.PropertyName._focusedWhileTargeting))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._focusedWhileTargeting = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMerchantButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantButton.PropertyName.IsLocalPlayerDead))
    {
      ref godot_variant local = ref value;
      bool isLocalPlayerDead = this.IsLocalPlayerDead;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isLocalPlayerDead);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMerchantButton.PropertyName._merchantSelectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._merchantSelectionReticle);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMerchantButton.PropertyName._focusedWhileTargeting))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._focusedWhileTargeting);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NMerchantButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMerchantButton.PropertyName._merchantSelectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantButton.PropertyName._focusedWhileTargeting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMerchantButton.PropertyName.IsLocalPlayerDead, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isLocalPlayerDead1 = NMerchantButton.PropertyName.IsLocalPlayerDead;
    bool isLocalPlayerDead2 = this.IsLocalPlayerDead;
    Variant variant = Variant.From<bool>(ref isLocalPlayerDead2);
    serializationInfo.AddProperty(isLocalPlayerDead1, variant);
    info.AddProperty(NMerchantButton.PropertyName._merchantSelectionReticle, Variant.From<NSelectionReticle>(ref this._merchantSelectionReticle));
    info.AddProperty(NMerchantButton.PropertyName._focusedWhileTargeting, Variant.From<bool>(ref this._focusedWhileTargeting));
    info.AddSignalEventDelegate(NMerchantButton.SignalName.MerchantOpened, (Delegate) this.backing_MerchantOpened);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMerchantButton.PropertyName.IsLocalPlayerDead, ref variant1))
      this.IsLocalPlayerDead = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NMerchantButton.PropertyName._merchantSelectionReticle, ref variant2))
      this._merchantSelectionReticle = ((Variant) ref variant2).As<NSelectionReticle>();
    Variant variant3;
    if (info.TryGetProperty(NMerchantButton.PropertyName._focusedWhileTargeting, ref variant3))
      this._focusedWhileTargeting = ((Variant) ref variant3).As<bool>();
    NMerchantButton.MerchantOpenedEventHandler openedEventHandler;
    if (!info.TryGetSignalEventDelegate<NMerchantButton.MerchantOpenedEventHandler>(NMerchantButton.SignalName.MerchantOpened, ref openedEventHandler))
      return;
    this.backing_MerchantOpened = openedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMerchantButton.SignalName.MerchantOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("merchantButton"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NMerchantButton.MerchantOpenedEventHandler MerchantOpened
  {
    add => this.backing_MerchantOpened += value;
    remove => this.backing_MerchantOpened -= value;
  }

  protected void EmitSignalMerchantOpened(NMerchantButton merchantButton)
  {
    ((GodotObject) this).EmitSignal(NMerchantButton.SignalName.MerchantOpened, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) merchantButton)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NMerchantButton.SignalName.MerchantOpened) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMerchantButton.MerchantOpenedEventHandler backingMerchantOpened = this.backing_MerchantOpened;
      if (backingMerchantOpened == null)
        return;
      backingMerchantOpened(VariantUtils.ConvertTo<NMerchantButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NMerchantButton.SignalName.MerchantOpened) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void MerchantOpenedEventHandler(
  #nullable enable
  NMerchantButton merchantButton);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName RefreshFocus = StringName.op_Implicit(nameof (RefreshFocus));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName IsLocalPlayerDead = StringName.op_Implicit(nameof (IsLocalPlayerDead));
    public static readonly StringName _merchantSelectionReticle = StringName.op_Implicit(nameof (_merchantSelectionReticle));
    public static readonly StringName _focusedWhileTargeting = StringName.op_Implicit(nameof (_focusedWhileTargeting));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName MerchantOpened = StringName.op_Implicit(nameof (MerchantOpened));
  }
}
