// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NExhaustPileButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NExhaustPileButton.cs")]
public class NExhaustPileButton : NCombatCardPile
{
  private Viewport _viewport;
  private Vector2 _posOffset;
  private static readonly Vector2 _hideOffset = new Vector2(150f, 0.0f);

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight)
      };
    }
  }

  protected override PileType Pile => PileType.Exhaust;

  public override void _Ready()
  {
    this.ConnectSignals();
    ((CanvasItem) this).Visible = false;
    this._viewport = ((Node) this).GetViewport();
    this._posOffset = new Vector2(this.OffsetRight + 100f, (float) (-(double) this.OffsetBottom + 90.0));
    ((GodotObject) ((Node) this).GetTree().Root).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(((NCombatCardPile) this).SetAnimInOutPositions)), 0U);
    this.SetAnimInOutPositions();
    this.Disable();
  }

  public override void Initialize(Player player)
  {
    base.Initialize(player);
    if (this.Pile.GetPile(player).Cards.Count <= 0)
      return;
    ((CanvasItem) this).Visible = true;
    this.Position = this._showPosition;
    this.Enable();
  }

  protected override void AddCard()
  {
    base.AddCard();
    if (!((CanvasItem) this).Visible)
      this.AnimIn();
    this.Enable();
  }

  protected override void SetAnimInOutPositions()
  {
    this._showPosition = Vector2.op_Subtraction(NGame.Instance.Size, this._posOffset);
    this._hidePosition = Vector2.op_Addition(this._showPosition, NExhaustPileButton._hideOffset);
  }

  public override void AnimIn()
  {
    base.AnimIn();
    ((CanvasItem) this).Visible = true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NExhaustPileButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NExhaustPileButton.MethodName.AddCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NExhaustPileButton.MethodName.SetAnimInOutPositions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NExhaustPileButton.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NExhaustPileButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NExhaustPileButton.MethodName.AddCard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AddCard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NExhaustPileButton.MethodName.SetAnimInOutPositions) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetAnimInOutPositions();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NExhaustPileButton.MethodName.AnimIn) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AnimIn();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NExhaustPileButton.MethodName._Ready) || StringName.op_Equality(ref method, NExhaustPileButton.MethodName.AddCard) || StringName.op_Equality(ref method, NExhaustPileButton.MethodName.SetAnimInOutPositions) || StringName.op_Equality(ref method, NExhaustPileButton.MethodName.AnimIn) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NExhaustPileButton.PropertyName._viewport))
    {
      this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NExhaustPileButton.PropertyName._posOffset))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NExhaustPileButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NExhaustPileButton.PropertyName.Pile))
    {
      ref godot_variant local = ref value;
      PileType pile = this.Pile;
      godot_variant from = VariantUtils.CreateFrom<PileType>(ref pile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NExhaustPileButton.PropertyName._viewport))
    {
      value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
      return true;
    }
    if (!StringName.op_Equality(ref name, NExhaustPileButton.PropertyName._posOffset))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NExhaustPileButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NExhaustPileButton.PropertyName._viewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NExhaustPileButton.PropertyName._posOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NExhaustPileButton.PropertyName.Pile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NExhaustPileButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
    info.AddProperty(NExhaustPileButton.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NExhaustPileButton.PropertyName._viewport, ref variant1))
      this._viewport = ((Variant) ref variant1).As<Viewport>();
    Variant variant2;
    if (!info.TryGetProperty(NExhaustPileButton.PropertyName._posOffset, ref variant2))
      return;
    this._posOffset = ((Variant) ref variant2).As<Vector2>();
  }

  public new class MethodName : NCombatCardPile.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName AddCard = StringName.op_Implicit(nameof (AddCard));
    public new static readonly StringName SetAnimInOutPositions = StringName.op_Implicit(nameof (SetAnimInOutPositions));
    public new static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
  }

  public new class PropertyName : NCombatCardPile.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public new static readonly StringName Pile = StringName.op_Implicit(nameof (Pile));
    public static readonly StringName _viewport = StringName.op_Implicit(nameof (_viewport));
    public static readonly StringName _posOffset = StringName.op_Implicit(nameof (_posOffset));
  }

  public new class SignalName : NCombatCardPile.SignalName
  {
  }
}
