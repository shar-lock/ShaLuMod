// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryLayoutDefault
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayoutDefault.cs")]
public class NBestiaryLayoutDefault : NBestiaryLayout
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_layout_default");
  private NCreature? _creature;
  private Control _creatureContainer;

  public static NBestiaryLayoutDefault? Create()
  {
    return PreloadManager.Cache.GetScene(NBestiaryLayoutDefault._scenePath).Instantiate<NBestiaryLayoutDefault>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._creatureContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%MonsterVisualsContainer"));
  }

  public override void Cleanup()
  {
    NCreature creature = this._creature;
    if (creature != null)
      ((Node) creature).QueueFreeSafely();
    this._creature = (NCreature) null;
  }

  public override List<BestiaryMonsterMove> Setup(BestiaryEntry entry, Tween tween)
  {
    MonsterModel mutable = entry.monsterModel.ToMutable();
    mutable.Rng = Rng.Chaotic;
    NCreature creature = this._creature;
    if (creature != null)
      ((Node) creature).QueueFreeSafely();
    mutable.SetUpForCombat();
    this._creature = NCreature.Create(new Creature(mutable, CombatSide.Enemy, (string) null)
    {
      CombatState = (ICombatState) NullCombatState.Instance
    });
    ((Node) this._creatureContainer).AddChildSafely((Node) this._creature);
    this._creature.SetupForBestiary();
    this._creature.Position = new Vector2(0.0f, this._creature.Hitbox.Size.Y * 0.5f);
    ((CanvasItem) this._creature).Modulate = StsColors.transparentBlack;
    tween.TweenProperty((GodotObject) this._creature, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    return mutable.GenerateBestiaryMoveList(this._creature.Visuals);
  }

  public override IEnumerable<NCreature> GetCreatures()
  {
    // ISSUE: object of a compiler-generated type is created
    return this._creature == null ? (IEnumerable<NCreature>) Array.Empty<NCreature>() : (IEnumerable<NCreature>) new \u003C\u003Ez__ReadOnlySingleElementList<NCreature>(this._creature);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBestiaryLayoutDefault.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLayoutDefault.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLayoutDefault.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiaryLayoutDefault nbestiaryLayoutDefault = NBestiaryLayoutDefault.Create();
      ret = VariantUtils.CreateFrom<NBestiaryLayoutDefault>(ref nbestiaryLayoutDefault);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName.Cleanup) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Cleanup();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiaryLayoutDefault nbestiaryLayoutDefault = NBestiaryLayoutDefault.Create();
      ret = VariantUtils.CreateFrom<NBestiaryLayoutDefault>(ref nbestiaryLayoutDefault);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName.Create) || StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryLayoutDefault.MethodName.Cleanup) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryLayoutDefault.PropertyName._creature))
    {
      this._creature = VariantUtils.ConvertTo<NCreature>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryLayoutDefault.PropertyName._creatureContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._creatureContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryLayoutDefault.PropertyName._creature))
    {
      value = VariantUtils.CreateFrom<NCreature>(ref this._creature);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryLayoutDefault.PropertyName._creatureContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._creatureContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutDefault.PropertyName._creature, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutDefault.PropertyName._creatureContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBestiaryLayoutDefault.PropertyName._creature, Variant.From<NCreature>(ref this._creature));
    info.AddProperty(NBestiaryLayoutDefault.PropertyName._creatureContainer, Variant.From<Control>(ref this._creatureContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiaryLayoutDefault.PropertyName._creature, ref variant1))
      this._creature = ((Variant) ref variant1).As<NCreature>();
    Variant variant2;
    if (!info.TryGetProperty(NBestiaryLayoutDefault.PropertyName._creatureContainer, ref variant2))
      return;
    this._creatureContainer = ((Variant) ref variant2).As<Control>();
  }

  public new class MethodName : NBestiaryLayout.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
  }

  public new class PropertyName : NBestiaryLayout.PropertyName
  {
    public static readonly StringName _creature = StringName.op_Implicit(nameof (_creature));
    public static readonly StringName _creatureContainer = StringName.op_Implicit(nameof (_creatureContainer));
  }

  public new class SignalName : NBestiaryLayout.SignalName
  {
  }
}
