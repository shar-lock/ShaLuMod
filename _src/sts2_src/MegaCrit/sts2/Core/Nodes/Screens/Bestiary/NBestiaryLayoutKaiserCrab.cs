// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiaryLayoutKaiserCrab
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
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiaryLayoutKaiserCrab.cs")]
public class NBestiaryLayoutKaiserCrab : NBestiaryLayout
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary_layout_kaiser_crab");
  private NCreature? _creature;
  private NKaiserCrabBossBackground _background;
  private Vector2 _backgroundPosition;

  public static NBestiaryLayoutKaiserCrab? Create()
  {
    return PreloadManager.Cache.GetScene(NBestiaryLayoutKaiserCrab._scenePath).Instantiate<NBestiaryLayoutKaiserCrab>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._background = ((Node) this).GetNode<NKaiserCrabBossBackground>(NodePath.op_Implicit("%KaiserCrab"));
    this._backgroundPosition = this._background.Position;
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
    switch (mutable)
    {
      case Crusher _:
        this._background.Position = Vector2.op_Addition(this._backgroundPosition, Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Multiply(1020f, Vector2.Right), 0.5f), this._background.Scale));
        break;
      case Rocket _:
        this._background.Position = Vector2.op_Addition(this._backgroundPosition, Vector2.op_Division(Vector2.op_Multiply(Vector2.op_Multiply(1240f, Vector2.Left), 0.5f), this._background.Scale));
        break;
    }
    ((CanvasItem) this._background).SetVisible(true);
    NCreature creature = this._creature;
    if (creature != null)
      ((Node) creature).QueueFreeSafely();
    mutable.SetUpForCombat();
    this._creature = NCreature.Create(new Creature(mutable, CombatSide.Enemy, (string) null)
    {
      CombatState = (ICombatState) NullCombatState.Instance
    });
    ((Node) this).AddChildSafely((Node) this._creature);
    this._creature.SetupForBestiary();
    this._creature.Position = new Vector2(0.0f, this._creature.Hitbox.Size.Y * 0.5f);
    ((CanvasItem) this._creature).Modulate = StsColors.transparentBlack;
    tween.TweenProperty((GodotObject) this._creature, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.25);
    List<BestiaryMonsterMove> bestiaryMoveList = mutable.GenerateBestiaryMoveList(this._creature.Visuals);
    bestiaryMoveList.Add(BestiaryMonsterMove.FromAction(new LocString("bestiary", "ACTION_NAME.hurt"), new Func<Task>(this.AnimHurt)));
    bestiaryMoveList.Add(BestiaryMonsterMove.FromAction(new LocString("bestiary", "ACTION_NAME.die"), new Func<Task>(this.AnimDie)));
    return bestiaryMoveList;
  }

  public Task AnimHurt()
  {
    this._background.PlayHurtAnim(this._creature.Entity.Monster is Crusher ? NKaiserCrabBossBackground.ArmSide.Left : NKaiserCrabBossBackground.ArmSide.Right);
    return Task.CompletedTask;
  }

  public Task AnimDie()
  {
    NAudioManager.Instance.PlayOneShot(this._creature.Entity.Monster.DeathSfx);
    this._background.PlayArmDeathAnim(this._creature.Entity.Monster is Crusher ? NKaiserCrabBossBackground.ArmSide.Left : NKaiserCrabBossBackground.ArmSide.Right);
    return Task.CompletedTask;
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
      new MethodInfo(NBestiaryLayoutKaiserCrab.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLayoutKaiserCrab.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiaryLayoutKaiserCrab.MethodName.Cleanup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiaryLayoutKaiserCrab layoutKaiserCrab = NBestiaryLayoutKaiserCrab.Create();
      ret = VariantUtils.CreateFrom<NBestiaryLayoutKaiserCrab>(ref layoutKaiserCrab);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName.Cleanup) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiaryLayoutKaiserCrab layoutKaiserCrab = NBestiaryLayoutKaiserCrab.Create();
      ret = VariantUtils.CreateFrom<NBestiaryLayoutKaiserCrab>(ref layoutKaiserCrab);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName.Create) || StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName._Ready) || StringName.op_Equality(ref method, NBestiaryLayoutKaiserCrab.MethodName.Cleanup) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryLayoutKaiserCrab.PropertyName._creature))
    {
      this._creature = VariantUtils.ConvertTo<NCreature>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryLayoutKaiserCrab.PropertyName._background))
    {
      this._background = VariantUtils.ConvertTo<NKaiserCrabBossBackground>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._backgroundPosition = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiaryLayoutKaiserCrab.PropertyName._creature))
    {
      value = VariantUtils.CreateFrom<NCreature>(ref this._creature);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiaryLayoutKaiserCrab.PropertyName._background))
    {
      value = VariantUtils.CreateFrom<NKaiserCrabBossBackground>(ref this._background);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._backgroundPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutKaiserCrab.PropertyName._creature, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiaryLayoutKaiserCrab.PropertyName._background, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBestiaryLayoutKaiserCrab.PropertyName._creature, Variant.From<NCreature>(ref this._creature));
    info.AddProperty(NBestiaryLayoutKaiserCrab.PropertyName._background, Variant.From<NKaiserCrabBossBackground>(ref this._background));
    info.AddProperty(NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition, Variant.From<Vector2>(ref this._backgroundPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiaryLayoutKaiserCrab.PropertyName._creature, ref variant1))
      this._creature = ((Variant) ref variant1).As<NCreature>();
    Variant variant2;
    if (info.TryGetProperty(NBestiaryLayoutKaiserCrab.PropertyName._background, ref variant2))
      this._background = ((Variant) ref variant2).As<NKaiserCrabBossBackground>();
    Variant variant3;
    if (!info.TryGetProperty(NBestiaryLayoutKaiserCrab.PropertyName._backgroundPosition, ref variant3))
      return;
    this._backgroundPosition = ((Variant) ref variant3).As<Vector2>();
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
    public static readonly StringName _background = StringName.op_Implicit(nameof (_background));
    public static readonly StringName _backgroundPosition = StringName.op_Implicit(nameof (_backgroundPosition));
  }

  public new class SignalName : NBestiaryLayout.SignalName
  {
  }
}
