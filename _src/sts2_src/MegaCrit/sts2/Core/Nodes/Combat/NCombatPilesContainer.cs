// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCombatPilesContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCombatPilesContainer.cs")]
public class NCombatPilesContainer : Control
{
  public static readonly string scenePath = SceneHelper.GetScenePath("combat/combat_piles_container");
  private NDrawPileButton _drawPile;
  private NDiscardPileButton _discardPile;
  private NExhaustPileButton _exhaustPile;

  public NDrawPileButton DrawPile => this._drawPile;

  public NDiscardPileButton DiscardPile => this._discardPile;

  public NExhaustPileButton ExhaustPile => this._exhaustPile;

  public override void _Ready()
  {
    this._drawPile = ((Node) this).GetNode<NDrawPileButton>(NodePath.op_Implicit("%DrawPile"));
    this._discardPile = ((Node) this).GetNode<NDiscardPileButton>(NodePath.op_Implicit("%DiscardPile"));
    this._exhaustPile = ((Node) this).GetNode<NExhaustPileButton>(NodePath.op_Implicit("%ExhaustPile"));
  }

  public void Initialize(Player player)
  {
    this._drawPile.Initialize(player);
    this._discardPile.Initialize(player);
    this._exhaustPile.Initialize(player);
  }

  public void AnimIn()
  {
    this._drawPile.AnimIn();
    this._discardPile.AnimIn();
  }

  public void AnimOut()
  {
    this._drawPile.AnimOut();
    this._discardPile.AnimOut();
    this._exhaustPile.AnimOut();
  }

  public void Enable()
  {
    this._drawPile.Enable();
    this._discardPile.Enable();
    this._exhaustPile.Enable();
  }

  public void Disable()
  {
    this._drawPile.Disable();
    this._discardPile.Disable();
    this._exhaustPile.Disable();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCombatPilesContainer.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPilesContainer.MethodName.AnimIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPilesContainer.MethodName.AnimOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPilesContainer.MethodName.Enable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatPilesContainer.MethodName.Disable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatPilesContainer.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.AnimIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.AnimOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimOut();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.Enable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Enable();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.Disable) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.Disable();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatPilesContainer.MethodName._Ready) || StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.AnimIn) || StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.AnimOut) || StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.Enable) || StringName.op_Equality(ref method, NCombatPilesContainer.MethodName.Disable) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName._drawPile))
    {
      this._drawPile = VariantUtils.ConvertTo<NDrawPileButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName._discardPile))
    {
      this._discardPile = VariantUtils.ConvertTo<NDiscardPileButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName._exhaustPile))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._exhaustPile = VariantUtils.ConvertTo<NExhaustPileButton>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName.DrawPile))
    {
      ref godot_variant local = ref value;
      NDrawPileButton drawPile = this.DrawPile;
      godot_variant from = VariantUtils.CreateFrom<NDrawPileButton>(ref drawPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName.DiscardPile))
    {
      ref godot_variant local = ref value;
      NDiscardPileButton discardPile = this.DiscardPile;
      godot_variant from = VariantUtils.CreateFrom<NDiscardPileButton>(ref discardPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName.ExhaustPile))
    {
      ref godot_variant local = ref value;
      NExhaustPileButton exhaustPile = this.ExhaustPile;
      godot_variant from = VariantUtils.CreateFrom<NExhaustPileButton>(ref exhaustPile);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName._drawPile))
    {
      value = VariantUtils.CreateFrom<NDrawPileButton>(ref this._drawPile);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName._discardPile))
    {
      value = VariantUtils.CreateFrom<NDiscardPileButton>(ref this._discardPile);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatPilesContainer.PropertyName._exhaustPile))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NExhaustPileButton>(ref this._exhaustPile);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatPilesContainer.PropertyName._drawPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPilesContainer.PropertyName._discardPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPilesContainer.PropertyName._exhaustPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPilesContainer.PropertyName.DrawPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPilesContainer.PropertyName.DiscardPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatPilesContainer.PropertyName.ExhaustPile, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCombatPilesContainer.PropertyName._drawPile, Variant.From<NDrawPileButton>(ref this._drawPile));
    info.AddProperty(NCombatPilesContainer.PropertyName._discardPile, Variant.From<NDiscardPileButton>(ref this._discardPile));
    info.AddProperty(NCombatPilesContainer.PropertyName._exhaustPile, Variant.From<NExhaustPileButton>(ref this._exhaustPile));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatPilesContainer.PropertyName._drawPile, ref variant1))
      this._drawPile = ((Variant) ref variant1).As<NDrawPileButton>();
    Variant variant2;
    if (info.TryGetProperty(NCombatPilesContainer.PropertyName._discardPile, ref variant2))
      this._discardPile = ((Variant) ref variant2).As<NDiscardPileButton>();
    Variant variant3;
    if (!info.TryGetProperty(NCombatPilesContainer.PropertyName._exhaustPile, ref variant3))
      return;
    this._exhaustPile = ((Variant) ref variant3).As<NExhaustPileButton>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimIn = StringName.op_Implicit(nameof (AnimIn));
    public static readonly StringName AnimOut = StringName.op_Implicit(nameof (AnimOut));
    public static readonly StringName Enable = StringName.op_Implicit(nameof (Enable));
    public static readonly StringName Disable = StringName.op_Implicit(nameof (Disable));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DrawPile = StringName.op_Implicit(nameof (DrawPile));
    public static readonly StringName DiscardPile = StringName.op_Implicit(nameof (DiscardPile));
    public static readonly StringName ExhaustPile = StringName.op_Implicit(nameof (ExhaustPile));
    public static readonly StringName _drawPile = StringName.op_Implicit(nameof (_drawPile));
    public static readonly StringName _discardPile = StringName.op_Implicit(nameof (_discardPile));
    public static readonly StringName _exhaustPile = StringName.op_Implicit(nameof (_exhaustPile));
  }

  public class SignalName : Control.SignalName
  {
  }
}
