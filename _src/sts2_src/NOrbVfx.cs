// Decompiled with JetBrains decompiler
// Type: NOrbVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
[ScriptPath("res://src/Core/Nodes/Orbs/NOrbVfx.cs")]
public class NOrbVfx : Node2D
{
  [Export]
  private NParticlesContainer? _focusedParticles;
  [Export]
  private NParticlesContainer? _passiveActivatedParticles;
  [Export]
  private NParticlesContainer? _passiveActivatedFocusedParticles;
  [Export]
  private NShaker? _spineShaker;
  [Export]
  private string _evokeVfxSceneName = "";
  private static string _evokeVfxScenePath = "vfx/orbs/";
  protected bool _forcedFocusPower;
  private Control? _overrideCombatVfxContainer;
  protected OrbModel? _orbModel;
  protected Player? _owner;
  protected Node2D? _overridePlayerNode;
  private Tween? _shakeTween;

  protected Control VfxContainer
  {
    get
    {
      Control combatVfxContainer = this._overrideCombatVfxContainer;
      if (combatVfxContainer != null)
        return combatVfxContainer;
      return NCombatRoom.Instance?.CombatVfxContainer;
    }
  }

  public override void _Ready()
  {
    ((Node) this)._Ready();
    if (this._spineShaker == null)
      return;
    this._spineShaker.Strength = 0.0f;
  }

  public void Initialize(OrbModel orbModel)
  {
    this._orbModel = orbModel;
    this._owner = this._orbModel.Owner;
    if (this._owner != null)
    {
      this._owner.Creature.PowerApplied += new Action<PowerModel>(this.OnPowerApplied);
      this._owner.Creature.PowerIncreased += new Action<PowerModel, int, bool>(this.OnPowerIncreased);
      this._owner.Creature.PowerDecreased += new Action<PowerModel, bool>(this.OnPowerDecreased);
    }
    if (this._orbModel != null)
    {
      this._orbModel.PassiveActivated += new Action(this.OnPassiveActivated);
      this._orbModel.EvokeActivated += new Action<Creature[]>(this.OnEvoke);
    }
    this.UpdateFocusPowerState();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    this._shakeTween?.Kill();
    if (this._owner != null)
    {
      this._owner.Creature.PowerApplied -= new Action<PowerModel>(this.OnPowerApplied);
      this._owner.Creature.PowerIncreased -= new Action<PowerModel, int, bool>(this.OnPowerIncreased);
      this._owner.Creature.PowerDecreased -= new Action<PowerModel, bool>(this.OnPowerDecreased);
    }
    if (this._orbModel == null)
      return;
    this._orbModel.PassiveActivated -= new Action(this.OnPassiveActivated);
    this._orbModel.EvokeActivated -= new Action<Creature[]>(this.OnEvoke);
  }

  protected void OnPowerApplied(PowerModel powerModel) => this.UpdateFocusPowerState();

  protected void OnPowerIncreased(PowerModel powerModel, int amount, bool silent)
  {
    this.UpdateFocusPowerState();
  }

  protected void OnPowerDecreased(PowerModel powerModel, bool silent)
  {
    this.UpdateFocusPowerState();
  }

  protected virtual void UpdateFocusPowerState()
  {
    if (this._focusedParticles == null)
      return;
    this._focusedParticles.SetEmitting(this.HasFocusPower());
  }

  public virtual void SetForcedFocusPower(bool forcedFocusPower)
  {
    this._forcedFocusPower = forcedFocusPower;
    this.UpdateFocusPowerState();
  }

  protected virtual bool HasFocusPower()
  {
    bool forcedFocusPower = this._forcedFocusPower;
    if (this._owner != null)
      forcedFocusPower |= this._owner.Creature.GetPowerAmount<FocusPower>() > 0;
    return forcedFocusPower;
  }

  public void OnPassiveActivated()
  {
    if (this._orbModel == null)
      return;
    this.OnPassiveActivated(this._orbModel.PassiveVal, this._orbModel.EvokeVal);
  }

  public virtual void OnPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
    if (this._passiveActivatedParticles != null)
      this._passiveActivatedParticles.Restart();
    if (this._passiveActivatedFocusedParticles == null || !this.HasFocusPower())
      return;
    this._passiveActivatedFocusedParticles.Restart();
  }

  public virtual void AfterPassiveActivated(Decimal passiveVal, Decimal evokeVal)
  {
  }

  private void OnEvoke(Creature[] targets)
  {
    List<NCreature> ncreatureList = new List<NCreature>();
    foreach (Creature target in targets)
    {
      NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(target);
      if (creatureNode != null)
        ncreatureList.Add(creatureNode);
    }
    this.OnEvoke(ncreatureList.ToArray());
  }

  public void OnEvoke(NCreature[] targets)
  {
    Vector2[] targetVfxSpawnPositions = new Vector2[targets.Length];
    for (int index = 0; index < targets.Length; ++index)
      targetVfxSpawnPositions[index] = targets[index].VfxSpawnPosition;
    this.OnEvoke(targetVfxSpawnPositions);
  }

  public void OnEvoke(Vector2[] targetVfxSpawnPositions)
  {
    this.SpawnEvokeVfx();
    for (int index = 0; index < targetVfxSpawnPositions.Length; ++index)
      this.OnEvokeInternal(targetVfxSpawnPositions[index]);
  }

  private void SpawnEvokeVfx()
  {
    if (string.IsNullOrEmpty(this._evokeVfxSceneName))
      return;
    VfxCmd.PlayVfx(this.GlobalPosition, NOrbVfx._evokeVfxScenePath + this._evokeVfxSceneName, this.VfxContainer);
  }

  protected virtual void OnEvokeInternal(Vector2 targetVfxSpawnPosition)
  {
  }

  protected Vector2 GetPlayerVfxPosition()
  {
    if (this._overridePlayerNode != null)
      return this._overridePlayerNode.Position;
    if (this._owner == null)
      return this.Position;
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this._owner.Creature);
    return creatureNode != null ? creatureNode.VfxSpawnPosition : this.Position;
  }

  public void SetOverrideCombatVfxContainer(Control overrideCombatVfxContainer)
  {
    this._overrideCombatVfxContainer = overrideCombatVfxContainer;
  }

  public void SetOverridePlayerNode(Node2D overridePlayerNode)
  {
    this._overridePlayerNode = overridePlayerNode;
  }

  protected void ShakeOrb(float initialStrength, float duration)
  {
    if (this._spineShaker == null)
      return;
    if (this._shakeTween != null && this._shakeTween.IsValid())
      this._shakeTween.Kill();
    this._spineShaker.Strength = initialStrength;
    this._shakeTween = ((Node) this).GetTree().CreateTween();
    this._shakeTween.TweenProperty((GodotObject) this._spineShaker, NodePath.op_Implicit("_strength"), Variant.op_Implicit(0.0f), (double) duration).SetEase((Tween.EaseType) 3L).SetTrans((Tween.TransitionType) 1L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NOrbVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.UpdateFocusPowerState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.SetForcedFocusPower, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("forcedFocusPower"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.HasFocusPower, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.OnPassiveActivated, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.OnEvoke, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 28L, StringName.op_Implicit("targets"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.SpawnEvokeVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.OnEvokeInternal, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetVfxSpawnPosition"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.GetPlayerVfxPosition, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.SetOverrideCombatVfxContainer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("overrideCombatVfxContainer"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.SetOverridePlayerNode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("overridePlayerNode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false)
      }, (List<Variant>) null),
      new MethodInfo(NOrbVfx.MethodName.ShakeOrb, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("initialStrength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.UpdateFocusPowerState) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateFocusPowerState();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.SetForcedFocusPower) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetForcedFocusPower(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.HasFocusPower) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.HasFocusPower();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.OnPassiveActivated) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPassiveActivated();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.OnEvoke) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEvoke(VariantUtils.ConvertToSystemArrayOfGodotObject<NCreature>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.SpawnEvokeVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SpawnEvokeVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.OnEvokeInternal) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEvokeInternal(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.GetPlayerVfxPosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 playerVfxPosition = this.GetPlayerVfxPosition();
      ret = VariantUtils.CreateFrom<Vector2>(ref playerVfxPosition);
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.SetOverrideCombatVfxContainer) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetOverrideCombatVfxContainer(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOrbVfx.MethodName.SetOverridePlayerNode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetOverridePlayerNode(VariantUtils.ConvertTo<Node2D>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NOrbVfx.MethodName.ShakeOrb) || ((NativeVariantPtrArgs) ref args).Count != 2)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ShakeOrb(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOrbVfx.MethodName._Ready) || StringName.op_Equality(ref method, NOrbVfx.MethodName._ExitTree) || StringName.op_Equality(ref method, NOrbVfx.MethodName.UpdateFocusPowerState) || StringName.op_Equality(ref method, NOrbVfx.MethodName.SetForcedFocusPower) || StringName.op_Equality(ref method, NOrbVfx.MethodName.HasFocusPower) || StringName.op_Equality(ref method, NOrbVfx.MethodName.OnPassiveActivated) || StringName.op_Equality(ref method, NOrbVfx.MethodName.OnEvoke) || StringName.op_Equality(ref method, NOrbVfx.MethodName.SpawnEvokeVfx) || StringName.op_Equality(ref method, NOrbVfx.MethodName.OnEvokeInternal) || StringName.op_Equality(ref method, NOrbVfx.MethodName.GetPlayerVfxPosition) || StringName.op_Equality(ref method, NOrbVfx.MethodName.SetOverrideCombatVfxContainer) || StringName.op_Equality(ref method, NOrbVfx.MethodName.SetOverridePlayerNode) || StringName.op_Equality(ref method, NOrbVfx.MethodName.ShakeOrb) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._focusedParticles))
    {
      this._focusedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._passiveActivatedParticles))
    {
      this._passiveActivatedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._passiveActivatedFocusedParticles))
    {
      this._passiveActivatedFocusedParticles = VariantUtils.ConvertTo<NParticlesContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._spineShaker))
    {
      this._spineShaker = VariantUtils.ConvertTo<NShaker>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._evokeVfxSceneName))
    {
      this._evokeVfxSceneName = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._forcedFocusPower))
    {
      this._forcedFocusPower = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._overrideCombatVfxContainer))
    {
      this._overrideCombatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._overridePlayerNode))
    {
      this._overridePlayerNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOrbVfx.PropertyName._shakeTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._shakeTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName.VfxContainer))
    {
      ref godot_variant local = ref value;
      Control vfxContainer = this.VfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref vfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._focusedParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._focusedParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._passiveActivatedParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._passiveActivatedParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._passiveActivatedFocusedParticles))
    {
      value = VariantUtils.CreateFrom<NParticlesContainer>(ref this._passiveActivatedFocusedParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._spineShaker))
    {
      value = VariantUtils.CreateFrom<NShaker>(ref this._spineShaker);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._evokeVfxSceneName))
    {
      value = VariantUtils.CreateFrom<string>(ref this._evokeVfxSceneName);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._forcedFocusPower))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._forcedFocusPower);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._overrideCombatVfxContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._overrideCombatVfxContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NOrbVfx.PropertyName._overridePlayerNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._overridePlayerNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOrbVfx.PropertyName._shakeTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._shakeTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._focusedParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._passiveActivatedParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._passiveActivatedFocusedParticles, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._spineShaker, (PropertyHint) 34L, "Node", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 4L, NOrbVfx.PropertyName._evokeVfxSceneName, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NOrbVfx.PropertyName._forcedFocusPower, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._overrideCombatVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName.VfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._overridePlayerNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOrbVfx.PropertyName._shakeTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NOrbVfx.PropertyName._focusedParticles, Variant.From<NParticlesContainer>(ref this._focusedParticles));
    info.AddProperty(NOrbVfx.PropertyName._passiveActivatedParticles, Variant.From<NParticlesContainer>(ref this._passiveActivatedParticles));
    info.AddProperty(NOrbVfx.PropertyName._passiveActivatedFocusedParticles, Variant.From<NParticlesContainer>(ref this._passiveActivatedFocusedParticles));
    info.AddProperty(NOrbVfx.PropertyName._spineShaker, Variant.From<NShaker>(ref this._spineShaker));
    info.AddProperty(NOrbVfx.PropertyName._evokeVfxSceneName, Variant.From<string>(ref this._evokeVfxSceneName));
    info.AddProperty(NOrbVfx.PropertyName._forcedFocusPower, Variant.From<bool>(ref this._forcedFocusPower));
    info.AddProperty(NOrbVfx.PropertyName._overrideCombatVfxContainer, Variant.From<Control>(ref this._overrideCombatVfxContainer));
    info.AddProperty(NOrbVfx.PropertyName._overridePlayerNode, Variant.From<Node2D>(ref this._overridePlayerNode));
    info.AddProperty(NOrbVfx.PropertyName._shakeTween, Variant.From<Tween>(ref this._shakeTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOrbVfx.PropertyName._focusedParticles, ref variant1))
      this._focusedParticles = ((Variant) ref variant1).As<NParticlesContainer>();
    Variant variant2;
    if (info.TryGetProperty(NOrbVfx.PropertyName._passiveActivatedParticles, ref variant2))
      this._passiveActivatedParticles = ((Variant) ref variant2).As<NParticlesContainer>();
    Variant variant3;
    if (info.TryGetProperty(NOrbVfx.PropertyName._passiveActivatedFocusedParticles, ref variant3))
      this._passiveActivatedFocusedParticles = ((Variant) ref variant3).As<NParticlesContainer>();
    Variant variant4;
    if (info.TryGetProperty(NOrbVfx.PropertyName._spineShaker, ref variant4))
      this._spineShaker = ((Variant) ref variant4).As<NShaker>();
    Variant variant5;
    if (info.TryGetProperty(NOrbVfx.PropertyName._evokeVfxSceneName, ref variant5))
      this._evokeVfxSceneName = ((Variant) ref variant5).As<string>();
    Variant variant6;
    if (info.TryGetProperty(NOrbVfx.PropertyName._forcedFocusPower, ref variant6))
      this._forcedFocusPower = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (info.TryGetProperty(NOrbVfx.PropertyName._overrideCombatVfxContainer, ref variant7))
      this._overrideCombatVfxContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NOrbVfx.PropertyName._overridePlayerNode, ref variant8))
      this._overridePlayerNode = ((Variant) ref variant8).As<Node2D>();
    Variant variant9;
    if (!info.TryGetProperty(NOrbVfx.PropertyName._shakeTween, ref variant9))
      return;
    this._shakeTween = ((Variant) ref variant9).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName UpdateFocusPowerState = StringName.op_Implicit(nameof (UpdateFocusPowerState));
    public static readonly StringName SetForcedFocusPower = StringName.op_Implicit(nameof (SetForcedFocusPower));
    public static readonly StringName HasFocusPower = StringName.op_Implicit(nameof (HasFocusPower));
    public static readonly StringName OnPassiveActivated = StringName.op_Implicit(nameof (OnPassiveActivated));
    public static readonly StringName OnEvoke = StringName.op_Implicit(nameof (OnEvoke));
    public static readonly StringName SpawnEvokeVfx = StringName.op_Implicit(nameof (SpawnEvokeVfx));
    public static readonly StringName OnEvokeInternal = StringName.op_Implicit(nameof (OnEvokeInternal));
    public static readonly StringName GetPlayerVfxPosition = StringName.op_Implicit(nameof (GetPlayerVfxPosition));
    public static readonly StringName SetOverrideCombatVfxContainer = StringName.op_Implicit(nameof (SetOverrideCombatVfxContainer));
    public static readonly StringName SetOverridePlayerNode = StringName.op_Implicit(nameof (SetOverridePlayerNode));
    public static readonly StringName ShakeOrb = StringName.op_Implicit(nameof (ShakeOrb));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName VfxContainer = StringName.op_Implicit(nameof (VfxContainer));
    public static readonly StringName _focusedParticles = StringName.op_Implicit(nameof (_focusedParticles));
    public static readonly StringName _passiveActivatedParticles = StringName.op_Implicit(nameof (_passiveActivatedParticles));
    public static readonly StringName _passiveActivatedFocusedParticles = StringName.op_Implicit(nameof (_passiveActivatedFocusedParticles));
    public static readonly StringName _spineShaker = StringName.op_Implicit(nameof (_spineShaker));
    public static readonly StringName _evokeVfxSceneName = StringName.op_Implicit(nameof (_evokeVfxSceneName));
    public static readonly StringName _forcedFocusPower = StringName.op_Implicit(nameof (_forcedFocusPower));
    public static readonly StringName _overrideCombatVfxContainer = StringName.op_Implicit(nameof (_overrideCombatVfxContainer));
    public static readonly StringName _overridePlayerNode = StringName.op_Implicit(nameof (_overridePlayerNode));
    public static readonly StringName _shakeTween = StringName.op_Implicit(nameof (_shakeTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
