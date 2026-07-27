// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryLayoutDecimillipede
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayoutDecimillipede.cs")]
public class NBestiaryLayoutDecimillipede : NBestiaryLayout
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_layout_decimillipede");
  private Control? _encounterVisuals;
  private Control _creatureContainer;
  private Control _encounterSlots;
  private List<NCreature> _creatures = new List<NCreature>();
  private NBestiary _bestiary;

  public static NBestiaryLayoutDecimillipede? Create(NBestiary bestiary)
  {
    if (TestMode.IsOn)
      return (NBestiaryLayoutDecimillipede) null;
    NBestiaryLayoutDecimillipede layoutDecimillipede = PreloadManager.Cache.GetScene(NBestiaryLayoutDecimillipede._scenePath).Instantiate<NBestiaryLayoutDecimillipede>((PackedScene.GenEditState) 0L);
    layoutDecimillipede._bestiary = bestiary;
    return layoutDecimillipede;
  }

  public override void _Ready()
  {
    this._creatureContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CreatureContainer"));
    this._encounterSlots = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EncounterSlots"));
  }

  public override void Cleanup()
  {
    Control encounterVisuals = this._encounterVisuals;
    if (encounterVisuals != null)
      ((Node) encounterVisuals).QueueFreeSafely();
    this._encounterVisuals = (Control) null;
  }

  public override List<BestiaryMonsterMove> Setup(BestiaryEntry entry, Tween tween)
  {
    EncounterModel mutable = entry.encounterModel.ToMutable();
    Control encounterVisuals = this._encounterVisuals;
    if (encounterVisuals != null)
      ((Node) encounterVisuals).QueueFreeSafely();
    mutable.GenerateMonstersWithSlots((IRunState) NullRunState.Instance);
    foreach ((MonsterModel monster, string slotName) in (IEnumerable<(MonsterModel, string)>) mutable.MonstersWithSlots)
    {
      monster.Rng = Rng.Chaotic;
      monster.SetUpForCombat();
      Creature entity = new Creature(monster, CombatSide.Enemy, slotName)
      {
        CombatState = (ICombatState) NullCombatState.Instance
      };
      NCreature child = NCreature.Create(entity);
      ((Node) this._creatureContainer).AddChildSafely((Node) child);
      this._creatures.Add(child);
      child.SetupForBestiary();
      child.GlobalPosition = ((Node2D) ((Node) this._encounterSlots).GetNode<Marker2D>(NodePath.op_Implicit(entity.SlotName))).GlobalPosition;
    }
    int capacity = 3;
    List<BestiaryMonsterMove> bestiaryMonsterMoveList = new List<BestiaryMonsterMove>(capacity);
    CollectionsMarshal.SetCount<BestiaryMonsterMove>(bestiaryMonsterMoveList, capacity);
    Span<BestiaryMonsterMove> span = CollectionsMarshal.AsSpan<BestiaryMonsterMove>(bestiaryMonsterMoveList);
    int num1 = 0;
    span[num1] = BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("WRITHE"), new Func<Task>(this.AnimAttack));
    int num2 = num1 + 1;
    span[num2] = BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("REATTACH"), new Func<Task>(this.AnimReattach));
    int num3 = num2 + 1;
    span[num3] = BestiaryMonsterMove.FromAction(this.GetBestiaryMoveName("DEAD"), new Func<Task>(this.AnimDie));
    return bestiaryMonsterMoveList;
  }

  private LocString GetBestiaryMoveName(string moveId)
  {
    return new LocString("monsters", $"{ModelDb.GetId<DecimillipedeSegment>().Entry}.moves.{moveId}.title");
  }

  private async Task AnimAttack()
  {
    foreach (NCreature creature in this._creatures)
      ((DecimillipedeSegment) creature.Entity.Monster).SegmentAttack();
    Node2D child = PreloadManager.Cache.GetScene(DecimillipedeSegment.rocksVfxPath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    ((Node) this._bestiary.VfxContainer).AddChildSafely((Node) child);
    Node2D node2D = child;
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    Vector2 vector2 = Vector2.op_Multiply(((Rect2) ref viewportRect).Size, 0.5f);
    node2D.GlobalPosition = vector2;
    await Cmd.Wait(0.5f);
  }

  private async Task AnimReattach()
  {
    foreach (NCreature creature in this._creatures)
    {
      creature.GetSpecialNode<NDecimillipedeSegmentVfx>("%NDecimillipedeSegmentVfx")?.Regenerate();
      await CreatureCmd.TriggerAnim(creature.Entity, "Revive", 0.15f);
    }
  }

  private async Task AnimDie()
  {
    foreach (NCreature creature in this._creatures)
      await CreatureCmd.TriggerAnim(creature.Entity, "Dead", 0.15f);
  }

  public override IEnumerable<NCreature> GetCreatures() => (IEnumerable<NCreature>) this._creatures;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NBestiaryLayoutDecimillipede.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("bestiary"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiaryLayoutDecimillipede.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLayoutDecimillipede.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NBestiaryLayoutDecimillipede layoutDecimillipede = NBestiaryLayoutDecimillipede.Create(VariantUtils.ConvertTo<NBestiary>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NBestiaryLayoutDecimillipede>(ref layoutDecimillipede);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName.Cleanup) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NBestiaryLayoutDecimillipede layoutDecimillipede = NBestiaryLayoutDecimillipede.Create(VariantUtils.ConvertTo<NBestiary>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NBestiaryLayoutDecimillipede>(ref layoutDecimillipede);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName.Create) || StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryLayoutDecimillipede.MethodName.Cleanup) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals))
    {
      this._encounterVisuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._creatureContainer))
    {
      this._creatureContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._encounterSlots))
    {
      this._encounterSlots = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._bestiary))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._bestiary = VariantUtils.ConvertTo<NBestiary>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._encounterVisuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._creatureContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._creatureContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._encounterSlots))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._encounterSlots);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryLayoutDecimillipede.PropertyName._bestiary))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NBestiary>(ref this._bestiary);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutDecimillipede.PropertyName._creatureContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutDecimillipede.PropertyName._encounterSlots, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutDecimillipede.PropertyName._bestiary, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals, Variant.From<Control>(ref this._encounterVisuals));
    info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._creatureContainer, Variant.From<Control>(ref this._creatureContainer));
    info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterSlots, Variant.From<Control>(ref this._encounterSlots));
    info.AddProperty(NBestiaryLayoutDecimillipede.PropertyName._bestiary, Variant.From<NBestiary>(ref this._bestiary));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterVisuals, ref variant1))
      this._encounterVisuals = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._creatureContainer, ref variant2))
      this._creatureContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._encounterSlots, ref variant3))
      this._encounterSlots = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NBestiaryLayoutDecimillipede.PropertyName._bestiary, ref variant4))
      return;
    this._bestiary = ((Variant) ref variant4).As<NBestiary>();
  }

  public new class MethodName : NBestiaryLayout.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName Cleanup = StringName.op_Implicit(nameof (Cleanup));
  }

  public new class PropertyName : NBestiaryLayout.PropertyName
  {
    public static readonly StringName _encounterVisuals = StringName.op_Implicit(nameof (_encounterVisuals));
    public static readonly StringName _creatureContainer = StringName.op_Implicit(nameof (_creatureContainer));
    public static readonly StringName _encounterSlots = StringName.op_Implicit(nameof (_encounterSlots));
    public static readonly StringName _bestiary = StringName.op_Implicit(nameof (_bestiary));
  }

  public new class SignalName : NBestiaryLayout.SignalName
  {
  }
}
