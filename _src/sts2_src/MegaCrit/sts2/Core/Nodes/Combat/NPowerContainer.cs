// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NPowerContainer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NPowerContainer.cs")]
public class NPowerContainer : Control
{
  private Creature? _creature;
  private Vector2? _originalPosition;
  private readonly List<NPower> _powerNodes = new List<NPower>();

  public override void _EnterTree()
  {
    ((Node) this)._EnterTree();
    this.ConnectCreatureSignals();
  }

  public override void _ExitTree()
  {
    ((Node) this)._ExitTree();
    if (this._creature == null)
      return;
    this._creature.PowerApplied -= new Action<PowerModel>(this.OnPowerApplied);
    this._creature.PowerRemoved -= new Action<PowerModel>(this.OnPowerRemoved);
  }

  private void ConnectCreatureSignals()
  {
    if (this._creature == null)
      return;
    this._creature.PowerApplied -= new Action<PowerModel>(this.OnPowerApplied);
    this._creature.PowerRemoved -= new Action<PowerModel>(this.OnPowerRemoved);
    this._creature.PowerApplied += new Action<PowerModel>(this.OnPowerApplied);
    this._creature.PowerRemoved += new Action<PowerModel>(this.OnPowerRemoved);
  }

  public void SetCreatureBounds(Control bounds)
  {
    this.GlobalPosition = new Vector2(bounds.GlobalPosition.X, this.GlobalPosition.Y);
    this.Size = new Vector2((float) ((double) bounds.Size.X * (double) bounds.Scale.X + 25.0), this.Size.Y);
    this._originalPosition = new Vector2?(this.Position);
    this.UpdatePositions();
  }

  private void Add(PowerModel power)
  {
    if (!power.IsVisible)
      return;
    NPower child = NPower.Create(power);
    child.Container = this;
    this._powerNodes.Add(child);
    ((Node) this).AddChildSafely((Node) child);
    this.UpdatePositions();
  }

  private void Remove(PowerModel power)
  {
    if (!CombatManager.Instance.IsInProgress)
      return;
    NPower npower = this._powerNodes.FirstOrDefault<NPower>((Func<NPower, bool>) (n => n.Model == power));
    if (npower == null)
      return;
    this._powerNodes.Remove(npower);
    this.UpdatePositions();
    ((Node) npower).QueueFreeSafely();
  }

  private void UpdatePositions()
  {
    if (this._powerNodes.Count == 0)
      return;
    float x = this._powerNodes[0].Size.X;
    float num1 = (float) Mathf.CeilToInt(this.Size.X / x);
    float num2 = Mathf.Max((float) Mathf.CeilToInt((float) this._powerNodes.Count / 2f), num1);
    for (int index = 0; index < this._powerNodes.Count; ++index)
      this._powerNodes[index].Position = new Vector2(x * ((float) index % num2), Mathf.Floor((float) index / num2) * x);
    float num3 = x * Mathf.Min(num2, (float) this._powerNodes.Count);
    this.Position = Vector2.op_Addition(this._originalPosition ?? this.Position, Vector2.op_Division(Vector2.op_Multiply(Vector2.Left, Mathf.Max(0.0f, num3 - this.Size.X)), 2f));
  }

  public void SetCreature(Creature creature)
  {
    this._creature = this._creature == null ? creature : throw new InvalidOperationException("Creature was already set.");
    this.ConnectCreatureSignals();
    foreach (PowerModel power in (IEnumerable<PowerModel>) this._creature.Powers)
      this.Add(power);
  }

  private void OnPowerApplied(PowerModel power) => this.Add(power);

  private void OnPowerRemoved(PowerModel power) => this.Remove(power);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NPowerContainer.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerContainer.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerContainer.MethodName.ConnectCreatureSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPowerContainer.MethodName.SetCreatureBounds, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("bounds"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NPowerContainer.MethodName.UpdatePositions, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPowerContainer.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPowerContainer.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPowerContainer.MethodName.ConnectCreatureSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectCreatureSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPowerContainer.MethodName.SetCreatureBounds) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetCreatureBounds(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPowerContainer.MethodName.UpdatePositions) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdatePositions();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPowerContainer.MethodName._EnterTree) || StringName.op_Equality(ref method, NPowerContainer.MethodName._ExitTree) || StringName.op_Equality(ref method, NPowerContainer.MethodName.ConnectCreatureSignals) || StringName.op_Equality(ref method, NPowerContainer.MethodName.SetCreatureBounds) || StringName.op_Equality(ref method, NPowerContainer.MethodName.UpdatePositions) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName ConnectCreatureSignals = StringName.op_Implicit(nameof (ConnectCreatureSignals));
    public static readonly StringName SetCreatureBounds = StringName.op_Implicit(nameof (SetCreatureBounds));
    public static readonly StringName UpdatePositions = StringName.op_Implicit(nameof (UpdatePositions));
  }

  public class PropertyName : Control.PropertyName
  {
  }

  public class SignalName : Control.SignalName
  {
  }
}
