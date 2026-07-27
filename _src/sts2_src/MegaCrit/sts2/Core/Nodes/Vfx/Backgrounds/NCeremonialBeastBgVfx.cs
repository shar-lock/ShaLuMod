// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds.NCeremonialBeastBgVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Audio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/Backgrounds/NCeremonialBeastBgVfx.cs")]
public class NCeremonialBeastBgVfx : Node
{
  private bool _isGlowOn;
  private bool _areSkullsOn;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    ((CanvasItem) this._parent).Visible = false;
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
  }

  public override void _EnterTree()
  {
    CombatManager.Instance.StateTracker.CombatStateChanged += new Action<CombatState>(this.UpdateState);
  }

  public override void _ExitTree()
  {
    CombatManager.Instance.StateTracker.CombatStateChanged -= new Action<CombatState>(this.UpdateState);
  }

  private void UpdateState(CombatState? combatState)
  {
    if (combatState == null)
      return;
    this.UpdateRingingSfx(combatState);
    this.UpdateVfxAndMusic(combatState);
  }

  private void UpdateRingingSfx(CombatState combatState)
  {
    Player me = LocalContext.GetMe((ICombatState) combatState);
    NRunMusicController.Instance?.UpdateMusicParameter("ringing", (float) (me != null && me.Creature.HasPower<RingingPower>() ? 1 : 0));
  }

  private void UpdateVfxAndMusic(CombatState combatState)
  {
    Creature creature = combatState.Creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.Monster is CeremonialBeast));
    if (creature == null)
    {
      NRunMusicController.Instance?.UpdateMusicParameter("ceremonial_beast_progress", 5f);
      this.PlayFlowers();
    }
    else if (creature.Monster is CeremonialBeast monster && !monster.IsInSecondPhase)
    {
      ((CanvasItem) this._parent).Visible = false;
    }
    else
    {
      ((CanvasItem) this._parent).Visible = true;
      if ((double) creature.CurrentHp > (double) creature.MaxHp * 0.33000001311302185)
      {
        NRunMusicController.Instance?.UpdateMusicParameter("ceremonial_beast_progress", 1f);
        this.PlayGlow();
      }
      else if (creature.IsAlive)
      {
        NRunMusicController.Instance?.UpdateMusicParameter("ceremonial_beast_progress", 1f);
        this.PlaySkulls();
      }
      else
      {
        NRunMusicController.Instance?.UpdateMusicParameter("ceremonial_beast_progress", 5f);
        this.PlayFlowers();
      }
    }
  }

  private void PlayGlow()
  {
    if (this._isGlowOn)
      return;
    this._isGlowOn = true;
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("glow_spawn");
    animationState.AddAnimation("glow_idle");
  }

  private void PlaySkulls()
  {
    if (this._areSkullsOn)
      return;
    this._areSkullsOn = true;
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("skulls_spawn");
    animationState.AddAnimation("glow_and_skulls_idle");
  }

  private void PlayFlowers()
  {
    MegaAnimationState animationState = this._animController.GetAnimationState();
    animationState.SetAnimation("glow_and_skulls_idle");
    animationState.AddAnimation("plants_spawn", 4.5f, false);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NCeremonialBeastBgVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastBgVfx.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastBgVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastBgVfx.MethodName.PlayGlow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastBgVfx.MethodName.PlaySkulls, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCeremonialBeastBgVfx.MethodName.PlayFlowers, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName.PlayGlow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayGlow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName.PlaySkulls) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlaySkulls();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName.PlayFlowers) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.PlayFlowers();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName._EnterTree) || StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName.PlayGlow) || StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName.PlaySkulls) || StringName.op_Equality(ref method, NCeremonialBeastBgVfx.MethodName.PlayFlowers) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCeremonialBeastBgVfx.PropertyName._isGlowOn))
    {
      this._isGlowOn = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastBgVfx.PropertyName._areSkullsOn))
    {
      this._areSkullsOn = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCeremonialBeastBgVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCeremonialBeastBgVfx.PropertyName._isGlowOn))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isGlowOn);
      return true;
    }
    if (StringName.op_Equality(ref name, NCeremonialBeastBgVfx.PropertyName._areSkullsOn))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._areSkullsOn);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCeremonialBeastBgVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCeremonialBeastBgVfx.PropertyName._isGlowOn, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCeremonialBeastBgVfx.PropertyName._areSkullsOn, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCeremonialBeastBgVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCeremonialBeastBgVfx.PropertyName._isGlowOn, Variant.From<bool>(ref this._isGlowOn));
    info.AddProperty(NCeremonialBeastBgVfx.PropertyName._areSkullsOn, Variant.From<bool>(ref this._areSkullsOn));
    info.AddProperty(NCeremonialBeastBgVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCeremonialBeastBgVfx.PropertyName._isGlowOn, ref variant1))
      this._isGlowOn = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCeremonialBeastBgVfx.PropertyName._areSkullsOn, ref variant2))
      this._areSkullsOn = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (!info.TryGetProperty(NCeremonialBeastBgVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName PlayGlow = StringName.op_Implicit(nameof (PlayGlow));
    public static readonly StringName PlaySkulls = StringName.op_Implicit(nameof (PlaySkulls));
    public static readonly StringName PlayFlowers = StringName.op_Implicit(nameof (PlayFlowers));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _isGlowOn = StringName.op_Implicit(nameof (_isGlowOn));
    public static readonly StringName _areSkullsOn = StringName.op_Implicit(nameof (_areSkullsOn));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
